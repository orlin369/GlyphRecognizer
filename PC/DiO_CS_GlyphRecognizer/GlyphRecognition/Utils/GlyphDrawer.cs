/*

Copyright (c) [2016] [Orlin Dimitrov]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Collections.Generic;
using System.Drawing;

using AForge.Vision.GlyphRecognition.Data;

namespace AForge.Vision.GlyphRecognition.Utils
{

    /// <summary>
    /// Utility class for drawing on Graphics object the glyphs.
    /// </summary>
    public static class GlyphDrawer
    {

        #region Variables

        /// <summary>
        /// Draw lock object.
        /// </summary>
        private static object drawLock = new object();

        /// <summary>
        /// Colors used to highlight points on image.
        /// </summary>
        private static Color[] pointsColors = new Color[4]
        {
            Color.Yellow, Color.Blue, Color.Red, Color.Lime
        };

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Render the image and components.
        /// </summary>
        /// <param name="eGlyphData">Extracted glyph data</param>
        /// <param name="graphics">Graphics canvas.</param>
        public static void DrawContour(ExtractedGlyphData eGlyphData, Graphics graphics)
        {
            lock (GlyphDrawer.drawLock)
            {
                // If graphics is not instanced, return.
                if (graphics == null)
                {
                    return;
                }

                Pen penTraj = new Pen(Color.Red, 3);

                if (eGlyphData.Quadrilateral.Count == 4)
                {
                    graphics.DrawPolygon(penTraj, GlyphDrawer.ConvertToPoint(eGlyphData.Quadrilateral).ToArray());
                }
            }
        }

        /// <summary>
        /// Draw the centroid of the glyph.
        /// </summary>
        /// <param name="eGlyphData">Extracted glyph data</param>
        /// <param name="graphics"></param>
        public static void DrawCentroid(ExtractedGlyphData eGlyphData, Graphics graphics)
        {
            lock (GlyphDrawer.drawLock)
            {
                // If graphics is not instanced, return.
                if (graphics == null)
                {
                    return;
                }

                Pen penTraj = new Pen(Color.Red, 3);

                if (eGlyphData.Quadrilateral.Count == 4)
                {
                    PointF c = eGlyphData.Centroid();
                    // Draw the center point.
                    System.Drawing.Point p = System.Drawing.Point.Add(System.Drawing.Point.Ceiling(c), new Size(-4, -4));
                    RectangleF centroidShape = new RectangleF(p, new Size(8, 8));
                    graphics.DrawEllipse(penTraj, centroidShape);
                }
            }
        }

        /// <summary>
        /// Draw points of the quadrilateral on the graphics canvas.
        /// </summary>
        /// <param name="eGlyphData">Extracted glyph data</param>
        /// <param name="graphics">Graphics canvas.</param>
        public static void DrawPoints(ExtractedGlyphData eGlyphData, Graphics graphics)
        {
            lock (GlyphDrawer.drawLock)
            {
                // Load the points of the glyph.
                AForge.Point[] imPoints = new AForge.Point[4];
                for (int index = 0; index < eGlyphData.Quadrilateral.Count; index++)
                {
                    float x = System.Math.Max(0, System.Math.Min(eGlyphData.Quadrilateral[index].X, eGlyphData.CoordinateSystemSize.Width - 1));
                    float y = System.Math.Max(0, System.Math.Min(eGlyphData.Quadrilateral[index].Y, eGlyphData.CoordinateSystemSize.Height - 1));

                    imPoints[index] = new AForge.Point(x - eGlyphData.CoordinateSystemSize.Width / 2, eGlyphData.CoordinateSystemSize.Height / 2 - y);
                }

                // Calculate the half width and height.
                int cx = eGlyphData.CoordinateSystemSize.Width / 2;
                int cy = eGlyphData.CoordinateSystemSize.Height / 2;

                // Draw corner points.
                for (int i = 0; i < 4; i++)
                {
                    using (Brush brush = new SolidBrush(GlyphDrawer.pointsColors[i]))
                    {
                        graphics.FillEllipse(brush, new Rectangle(
                            (int)(cx + imPoints[i].X - 3),
                            (int)(cy - imPoints[i].Y - 3),
                            7, 7));
                    }
                }
            }
        }

        /// <summary>
        /// Draw coordinates of the recognized object on the graphics object.
        /// </summary>
        /// <param name="eGlyphData">Extracted glyph data</param>
        /// <param name="graphics">Graphics canvas.</param>
        public static void DrawCoordinates(ExtractedGlyphData eGlyphData, Graphics graphics)
        {
            // Calculate the half width and height.
            int cx = eGlyphData.CoordinateSystemSize.Width / 2;
            int cy = eGlyphData.CoordinateSystemSize.Height / 2;
            int thicknes = 3;

            AForge.Point[] projectedAxes = eGlyphData.PerformProjection();

            using (Pen pen = new Pen(Color.Blue, thicknes))
            {
                graphics.DrawLine(pen,
                    cx + projectedAxes[0].X, cy - projectedAxes[0].Y,
                    cx + projectedAxes[1].X, cy - projectedAxes[1].Y);
            }

            using (Pen pen = new Pen(Color.Red, thicknes))
            {
                graphics.DrawLine(pen,
                    cx + projectedAxes[0].X, cy - projectedAxes[0].Y,
                    cx + projectedAxes[2].X, cy - projectedAxes[2].Y);
            }

            using (Pen pen = new Pen(Color.Lime, thicknes))
            {
                graphics.DrawLine(pen,
                    cx + projectedAxes[0].X, cy - projectedAxes[0].Y,
                    cx + projectedAxes[3].X, cy - projectedAxes[3].Y);
            }
        }

        #endregion

        #region Private Static Methods

        private static List<System.Drawing.Point> ConvertToPoint(List<IntPoint> points)
        {
            List<System.Drawing.Point> netPoints = new List<System.Drawing.Point>();

            if (points == null)
            {
                throw new NullReferenceException("Points can not be null.");
            }

            foreach (IntPoint p in points)
            {
                netPoints.Add(new System.Drawing.Point(p.X, p.Y));
            }

            return netPoints;
        }

        #endregion

    }
}
