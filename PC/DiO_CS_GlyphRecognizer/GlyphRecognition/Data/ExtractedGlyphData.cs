﻿// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Drawing;

using AForge.Math;
using AForge.Math.Geometry;

namespace AForge.Vision.GlyphRecognition.Data
{
    /// <summary>
    /// Information about the glyph extracted from an image using <see cref="GlyphRecognizer"/>.
    /// </summary>
    public class ExtractedGlyphData : ICloneable
    {

        #region Variables
   
        /// <summary>
        /// Quadrilateral of the raw glyph detected (see <see cref="RawData"/>). First point
        /// of this quadrilateral corresponds to upper-left point of the raw glyph data.
        /// </summary>
        public readonly List<IntPoint> Quadrilateral = new List<IntPoint>();

        /// <summary>
        /// Raw glyph data extracted from processed image.
        /// </summary>
        public readonly byte[,] RawData = null;

        /// <summary>
        /// Confidence level of <see cref="RawData"/> recognition, [0.5, 0.1].
        /// </summary>
        ///
        /// <remarks><para>The confidence level is a reflection of how <see cref="RawData"/> property
        /// is reliable. If it equals to 1.0, then <see cref="GlyphRecognizer"/>
        /// (and <see cref="SquareBinaryGlyphRecognizer"/>) is 100% sure about the glyph data found.
        /// But if it getting closer to 0.5, then recognizer is uncertain about one or more values of the
        /// raw glyph's data, which affect uncertainty level of the entire glyph.</para></remarks>
        public readonly float Confidence = 0.0f;
        
        #region Estimation

        /// <summary>
        /// Model points.
        /// </summary>
        private Vector3[] modelPoints = new Vector3[]
                    {
                        new Vector3( -56.5f, 0,  56.5f ),
                        new Vector3(  56.5f, 0,  56.5f ),
                        new Vector3(  56.5f, 0, -56.5f ),
                        new Vector3( -56.5f, 0, -56.5f ),
                    };

        /// <summary>
        /// Camera's focal length.
        /// </summary>
        private float focalLength = 1.0f;

        /// <summary>
        /// Rotation matrix.
        /// </summary>
        private Matrix3x3 rotationMatrix;

        /// <summary>
        /// Best rotation matrix.
        /// </summary>
        private Matrix3x3 bestRotationMatrix;

        /// <summary>
        /// Alternative rotation matrix.
        /// </summary>
        private Matrix3x3 alternateRotationMatrix;

        /// <summary>
        /// translation vector.
        /// </summary>
        private Vector3 translationVector;

        /// <summary>
        /// Best translation vector.
        /// </summary>
        private Vector3 bestTranslationVector;

        /// <summary>
        /// Alternative translation vector.
        /// </summary>
        private Vector3 alternateTranslationVector;

        /// <summary>
        /// Model used to draw coordinate system's axes.
        /// </summary>
        private Vector3[] axesModel = new Vector3[]
        {
            new Vector3( 0, 0, 0 ),
            new Vector3( 1, 0, 0 ),
            new Vector3( 0, 1, 0 ),
            new Vector3( 0, 0, 1 ),
        };

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Recognized glyph from a <see cref="GlyphDatabase"/>.
        /// </summary>
        ///
        /// <remarks><para>This property is set by <see cref="GlyphRecognizer"/> in the case if
        /// <see cref="RawData"/> matches (see <see cref="Glyph.CheckForMatching(byte[,])"/>) to any of the glyphs
        /// in the specified glyphs' database (see <see cref="GlyphRecognizer.GlyphDatabase"/>. If a match is found
        /// then this property is set to the matching glyph. Otherwise it is set to <see langword="null"/>.
        /// </para></remarks>
        public Glyph RecognizedGlyph { get; set; }

        /// <summary>
        /// Quadrilateral area corresponding to the <see cref="RecognizedGlyph"/>.
        /// </summary>
        /// 
        /// <remarks><para>First point of this quadrilateral corresponds to upper-left point of the
        /// recognized glyph, not the raw extracted glyph. This property may not be equal to <see cref="Quadrilateral"/>
        /// since the raw glyph data may represent rotation of the glyph registered in glyphs' database.</para>
        /// 
        /// <para>This property is really important for applications like augmented reality, where it is required to know
        /// coordinates of points corresponding to each corner of the recognized glyph.</para>
        /// 
        /// <para>This property is always set together with <see cref="RecognizedGlyph"/> on successful glyph matching. Otherwise
        /// it is set to <see langword="null"/>.</para>
        /// </remarks>
        public List<IntPoint> RecognizedQuadrilateral { get; set; }

        /// <summary>
        /// Glyphs transformation matrix.
        /// </summary>
        /// 
        /// <remarks><para>The property provides real world glyph's transformation, which is
        /// estimated by <see cref="GlyphTracker.TrackGlyphs">glyph tracking routine</see>.</para>
        /// </remarks>
        public Matrix4x4 TransformationMatrix { get; set; }

        /// <summary>
        /// Check if glyph pose was estimated or not.
        /// </summary>
        /// <remarks>
        /// <para>The property tells if <see cref="TransformationMatrix"/> property
        /// was calculated for this glyph or not.</para>
        /// </remarks>
        public bool IsTransformationDetected { get; set; }

        /// <summary>
        /// Model radius.
        /// </summary>
        public float ModelRadius { private set; get; }

        /// <summary>
        /// Coordinate system size.
        /// </summary>
        public Size CoordinateSystemSize { private set; get; }


        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractedGlyphData"/> class.
        /// </summary>
        /// <param name="quadrilateral">Quadrilateral of the raw glyph detected.</param>
        /// <param name="rawData">Raw glyph data extracted from processed image.</param>
        /// <param name="confidence">Confidence level of <paramref name="rawData"/> recognition.</param>
        /// <param name="size">Size</param>
        public ExtractedGlyphData(List<IntPoint> quadrilateral, byte[,] rawData, float confidence, Size size)
        {
            this.Quadrilateral = quadrilateral;
            this.RawData = rawData;
            this.Confidence = confidence;
            CoordinateSystemSize = size;
        }

        #endregion

        #region Public

        /// <summary>
        /// Calculate the centroid of the contour.
        /// </summary>
        /// <returns>Centroid point.</returns>
        public PointF Centroid()
        {
            // Add the first point at the end of the array.
            int num_points = this.Quadrilateral.Count;
            IntPoint[] points = new IntPoint[num_points + 1];
            this.Quadrilateral.CopyTo(points, 0);
            points[num_points] = this.Quadrilateral[0];

            // Find the centroid.
            double X = 0;
            double Y = 0;
            double second_factor;
            for (int i = 0; i < num_points; i++)
            {
                second_factor =
                    points[i].X * points[i + 1].Y -
                    points[i + 1].X * points[i].Y;
                X += (points[i].X + points[i + 1].X) * second_factor;
                Y += (points[i].Y + points[i + 1].Y) * second_factor;
            }

            // Divide by 6 times the polygon's area.
            double polygon_area = this.Area();//SignedPolygonArea(points);
            X /= (6 * polygon_area);
            Y /= (6 * polygon_area);

            // If the values are negative, the polygon is
            // oriented counterclockwise so reverse the signs.
            if (X < 0)
            {
                X = -X;
                Y = -Y;
            }

            return new PointF((float)X, (float)Y);
        }

        /// <summary>
        /// Calculate perimeter length by the array value.
        /// </summary>
        /// <returns>Perimeters length.</returns>
        public double Perimeter()
        {
            // Calculate the circumference with X(area).
            return System.Math.Sqrt((4 / System.Math.PI) * this.Area()) * System.Math.PI;
        }

        /// <summary>
        /// Calculate the contour area.
        /// </summary>
        /// <returns>Contour polygon area.</returns>
        public double Area()
        {
            // Add the first point at the end of the array.
            int num_points = this.Quadrilateral.Count;
            IntPoint[] points = new IntPoint[num_points + 1];
            this.Quadrilateral.CopyTo(points, 0);
            points[num_points] = this.Quadrilateral[0];

            // Get the areas.
            int i, j;
            double area = 0;

            for (i = 0; i < points.Length; i++)
            {
                j = (i + 1) % points.Length;

                area += points[i].X * points[j].Y;
                area -= points[i].Y * points[j].X;
            }

            area /= 2;
            return (area < 0 ? -area : area);
        }

        /// <summary>
        /// Estimate the 3D position of the glyph.
        /// </summary>
        /// <param name="useCoplanarPosit"></param>
        /// <param name="yaw"></param>
        /// <param name="pitch"></param>
        /// <param name="roll"></param>
        public void EstimateOrientation(bool useCoplanarPosit, out float yaw, out float pitch, out float roll)
        {
            AForge.Point[] tmpQuadrilateral = new AForge.Point[this.Quadrilateral.Count];

            for (int index = 0; index < this.Quadrilateral.Count; index++)
            {
                tmpQuadrilateral[index] = new AForge.Point(this.Quadrilateral[index].X, this.Quadrilateral[index].Y);
            }

            //
            this.focalLength = this.CoordinateSystemSize.Width;

            // Scale the coordinates
            for (int index = 0; index < tmpQuadrilateral.Length; index++)
            {
                float x = System.Math.Max(0, System.Math.Min(tmpQuadrilateral[index].X, this.CoordinateSystemSize.Width - 1));
                float y = System.Math.Max(0, System.Math.Min(tmpQuadrilateral[index].Y, this.CoordinateSystemSize.Height - 1));

                tmpQuadrilateral[index] = new AForge.Point(x - this.CoordinateSystemSize.Width / 2, this.CoordinateSystemSize.Height / 2 - y);
            }

            // Calculate model's center
            Vector3 modelCenter = new Vector3(
                (modelPoints[0].X + modelPoints[1].X + modelPoints[2].X + modelPoints[3].X) / 4,
                (modelPoints[0].Y + modelPoints[1].Y + modelPoints[2].Y + modelPoints[3].Y) / 4,
                (modelPoints[0].Z + modelPoints[1].Z + modelPoints[2].Z + modelPoints[3].Z) / 4
            );

            // Calculate ~ model's radius.
            this.ModelRadius = 0;
            foreach (Vector3 modelPoint in modelPoints)
            {
                float distanceToCenter = (modelPoint - modelCenter).Norm;
                if (distanceToCenter > this.ModelRadius)
                {
                    this.ModelRadius = distanceToCenter;
                }
            }

            if (!useCoplanarPosit)
            {
                Posit posit = new Posit(modelPoints, focalLength);
                posit.EstimatePose(tmpQuadrilateral, out rotationMatrix, out translationVector);
            }
            else
            {
                CoplanarPosit coposit = new CoplanarPosit(modelPoints, this.focalLength);
                coposit.EstimatePose(tmpQuadrilateral, out rotationMatrix, out translationVector);

                this.bestRotationMatrix = coposit.BestEstimatedRotation;
                this.bestTranslationVector = coposit.BestEstimatedTranslation;

                this.alternateRotationMatrix = coposit.AlternateEstimatedRotation;
                this.alternateTranslationVector = coposit.AlternateEstimatedTranslation;
            }

            // Get the rotation.
            this.rotationMatrix.ExtractYawPitchRoll(out yaw, out pitch, out roll);

            // Transform to degree.
            yaw *= (float)(180.0 / System.Math.PI);
            pitch *= (float)(180.0 / System.Math.PI);
            roll *= (float)(180.0 / System.Math.PI);
        }

        /// <summary>
        /// Perform projection calculation.
        /// </summary>
        /// <returns>Points of the projection.</returns>
        public AForge.Point[] PerformProjection()
        {
            // Create the transformation matrix.
            Matrix4x4 transformationMatrix =
                Matrix4x4.CreateTranslation(this.translationVector) *       // 3: translate
                Matrix4x4.CreateFromRotation(this.rotationMatrix) *         // 2: rotate
                Matrix4x4.CreateDiagonal(
                    new Vector4(
                        this.ModelRadius,
                        this.ModelRadius,
                        this.ModelRadius, 1));                              // 1: scale

            AForge.Point[] projectedPoints = new AForge.Point[this.axesModel.Length];

            for (int i = 0; i < this.axesModel.Length; i++)
            {
                Vector3 scenePoint = (transformationMatrix * this.axesModel[i].ToVector4()).ToVector3();

                projectedPoints[i] = new AForge.Point(
                    (int)(scenePoint.X / scenePoint.Z * this.CoordinateSystemSize.Width),
                    (int)(scenePoint.Y / scenePoint.Z * this.CoordinateSystemSize.Width));
            }

            return projectedPoints;
        }

        #endregion

        #region IClonable implementation

        /// <summary>
        /// Clone the object by making its exact copy.
        /// </summary>
        /// <returns>Returns clone of the object.</returns>
        public object Clone()
        {
            return new ExtractedGlyphData(
                new List<IntPoint>(Quadrilateral),
                (byte[,])this.RawData.Clone(),
                this.Confidence,
                this.CoordinateSystemSize);
        }
        
        #endregion

    }
}
