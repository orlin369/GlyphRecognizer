using AForge;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DiO_CS_GliphRecognizer
{
    internal class Utils
    {
        /// <summary>
        /// Resize bitmap images.
        /// </summary>
        /// <param name="imgToResize">Source image.</param>
        /// <param name="size">Output size.</param>
        /// <returns>Resized new bitmap.</returns>
        public static Bitmap ResizeImage(Bitmap sourceImage, Size size)
        {

            int sourceWidth = sourceImage.Width;
            int sourceHeight = sourceImage.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bitmapImage = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage((Image)bitmapImage);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(sourceImage, 0, 0, destWidth, destHeight);
            graphics.Dispose();

            return bitmapImage;
        }

        /// <summary>
        /// Convert AForge integer points to .NET point.
        /// </summary>
        /// <param name="points">AForge points.</param>
        /// <returns>DotNET points.</returns>
        public static List<System.Drawing.Point> ConvertToPoint(List<IntPoint> points)
        {
            if (points == null)
            {
                throw new NullReferenceException("Points can not be null.");
            }

            List<System.Drawing.Point> netPoints = new List<System.Drawing.Point>();

            foreach (IntPoint p in points)
            {
                netPoints.Add(new System.Drawing.Point(p.X, p.Y));
            }

            return netPoints;
        }

    }
}
