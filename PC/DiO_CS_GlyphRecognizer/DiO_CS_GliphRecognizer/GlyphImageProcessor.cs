// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Math.Geometry;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Vision.GlyphRecognition;
using System.Drawing.Drawing2D;

namespace DiO_CS_GliphRecognizer
{
    class GlyphImageProcessor
    {

        #region Variables

        /// <summary>
        /// Glyph recognizer to use for glyph recognition in video.
        /// </summary>
        private GlyphRecognizer recognizer = new GlyphRecognizer( 5 );
        
        /// <summary>
        /// Glyph tracker to track position of glyphs.
        /// </summary>
        private GlyphTracker glyphTracker = new GlyphTracker( );

        /// <summary>
        /// Quadrilateral transformation used to put image in place of glyph.
        /// </summary>
        private BackwardQuadrilateralTransformation quadrilateralTransformation = new BackwardQuadrilateralTransformation( );

        /// <summary>
        /// Default font to highlight glyphs.
        /// </summary>
        private Font defaultFont = new Font( FontFamily.GenericSerif, 15, FontStyle.Bold );

        /// <summary>
        /// Object used for synchronization.
        /// </summary>
        private object sync = new object( );

        /// <summary>
        /// Visualization types.
        /// </summary>
        /// <remarks>
        /// VisualizationType.Image - last state of the engine.
        /// </remarks>
        public VisualizationType VisualizationType = VisualizationType.Image;

        #endregion

        #region Propertys

        /// <summary>
        /// Database of glyphs to recognize.
        /// </summary>
        public GlyphDatabase GlyphDatabase
        {
            get { return recognizer.GlyphDatabase; }
            set
            {
                lock ( sync )
                {
                    recognizer.GlyphDatabase = value;
                }
            }
        }

        /// <summary>
        /// Effective focal length of camera.
        /// </summary>
        public float CameraFocalLength
        {
            get { return glyphTracker.CameraFocalLength; }
            set { glyphTracker.CameraFocalLength = value; }
        }

        /// <summary>
        /// Real size of glyphs.
        /// </summary>
        public float GlyphSize
        {
            get { return glyphTracker.GlyphSize; }
            set { glyphTracker.GlyphSize = value; }
        }

        #endregion

        #region Public

        /// <summary>
        /// Process image searching for glyphs and highlighting them.
        /// </summary>
        /// <param name="bitmap">Image for processing.</param>
        /// <returns>List of the recognised glyphs.</returns>
        public List<ExtractedGlyphData> ProcessImage(Bitmap bitmap)
        {
            List<ExtractedGlyphData> glyphs = new List<ExtractedGlyphData>();

            lock (sync)
            {
                glyphTracker.ImageSize = bitmap.Size;

                // get list of recognized glyphs
                glyphs.AddRange(recognizer.FindGlyphs(bitmap));
                List<int> glyphIDs = glyphTracker.TrackGlyphs(glyphs);

                if (glyphs.Count > 0)
                {
                    if ((VisualizationType == VisualizationType.BorderOnly) ||
                         (VisualizationType == VisualizationType.Name))
                    {
                        Graphics g = Graphics.FromImage(bitmap);
                        // Setup the graphics.
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        //g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //g.SmoothingMode = SmoothingMode.HighQuality;
                        //g.PixelOffsetMode = PixelOffsetMode.HighQuality;



                        int i = 0;

                        // highlight each found glyph
                        foreach (ExtractedGlyphData glyphData in glyphs)
                        {
                            List<IntPoint> glyphPoints = (glyphData.RecognizedGlyph == null) ?
                                glyphData.Quadrilateral : glyphData.RecognizedQuadrilateral;

                            Pen pen = new Pen(((glyphData.RecognizedGlyph == null) || (glyphData.RecognizedGlyph.UserData == null)) ?
                                Color.Red : ((GlyphVisualizationData)glyphData.RecognizedGlyph.UserData).Color, 3);

                            // highlight border
                            g.DrawPolygon(pen, ToPointsArray(glyphPoints));

                            string glyphTitle = null;

                            // prepare glyph's title
                            if ((VisualizationType == VisualizationType.Name) && (glyphData.RecognizedGlyph != null))
                            {
                                glyphTitle = string.Format("{0}: {1}",
                                    glyphIDs[i], glyphData.RecognizedGlyph.Name);
                            }
                            else
                            {
                                glyphTitle = string.Format("Tracking ID: {0}", glyphIDs[i]);
                            }

                            // show glyph's title
                            if (!string.IsNullOrEmpty(glyphTitle))
                            {
                                // get glyph's center point
                                IntPoint minXY, maxXY;
                                PointsCloud.GetBoundingRectangle(glyphPoints, out minXY, out maxXY);
                                IntPoint center = (minXY + maxXY) / 2;

                                // glyph's name size
                                SizeF nameSize = g.MeasureString(glyphTitle, defaultFont);

                                // paint the name
                                Brush brush = new SolidBrush(pen.Color);

                                g.DrawString(glyphTitle, defaultFont, brush,
                                    new System.Drawing.Point(center.X - (int)nameSize.Width / 2, center.Y - (int)nameSize.Height / 2));

                                brush.Dispose();
                            }

                            i++;
                            pen.Dispose();
                        }
                    }
                    else if (VisualizationType == VisualizationType.Image)
                    {
                        // lock image for further processing
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                            ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        UnmanagedImage unmanagedImage = new UnmanagedImage(bitmapData);

                        // highlight each found glyph
                        foreach (ExtractedGlyphData glyphData in glyphs)
                        {
                            if ((glyphData.RecognizedGlyph != null) && (glyphData.RecognizedGlyph.UserData != null))
                            {
                                GlyphVisualizationData visualization =
                                    (GlyphVisualizationData)glyphData.RecognizedGlyph.UserData;

                                if (visualization.ImageName != null)
                                {
                                    // get image associated with the glyph
                                    Bitmap glyphImage = EmbeddedImageCollection.Instance.GetImage(visualization.ImageName);

                                    if (glyphImage != null)
                                    {
                                        // put glyph's image onto the glyph using quadrilateral transformation
                                        quadrilateralTransformation.SourceImage = glyphImage;
                                        quadrilateralTransformation.DestinationQuadrilateral = glyphData.RecognizedQuadrilateral;

                                        quadrilateralTransformation.ApplyInPlace(unmanagedImage);
                                    }
                                }
                            }
                        }

                        bitmap.UnlockBits(bitmapData);
                    }
                }
            }

            return glyphs;
        }

        /// <summary>
        /// Reset glyph processor to initial state.
        /// </summary>
        public void Reset()
        {
            glyphTracker.Reset( );
        }

        #endregion

        #region Private

        /// <summary>
        /// Convert list of AForge.NET framework's points to array of .NET's points
        /// </summary>
        /// <param name="points">List of AForg points.</param>
        /// <returns>Array of .NET point.</returns>
        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            int count = points.Count;
            System.Drawing.Point[] pointsArray = new System.Drawing.Point[count];

            for ( int i = 0; i < count; i++ )
            {
                pointsArray[i] = new System.Drawing.Point( points[i].X, points[i].Y );
            }

            return pointsArray;
        }
        
        #endregion
    }
}
