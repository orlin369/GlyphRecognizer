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
using AForge.Vision.GlyphRecognition.Data;

namespace DiO_CS_GliphRecognizer.Data
{
    [Serializable]
    public class SerialExtractedGlyphData
    {

        #region Properties

        /// <summary>
        /// Name of the glyph.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// X [pix]
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y [pix]
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Yaw [deg]
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// Pitch [deg]
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// Roll [deg]
        /// </summary>
        public float Roll { get; set; }

        /// <summary>
        /// Area [pix^2]
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// Coordinate system width [pix]
        /// </summary>
        public double CsWidth { get; set; }

        /// <summary>
        /// Coordinate system height [pix]
        /// </summary>
        public double CsHeigth { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SerialExtractedGlyphData()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Extracted glyph data.</param>
        public SerialExtractedGlyphData(ExtractedGlyphData data)
        {
            float yaw = 0.0f;
            float pitch = 0.0f;
            float roll = 0.0f;
            data.EstimateOrientation(true, out yaw, out pitch, out roll);
            this.Yaw = yaw;
            this.Pitch = pitch;
            this.Roll = roll;

            AForge.Point[] pp = data.PerformProjection();
            this.X = pp[0].X;
            this.Y = pp[0].Y;

            this.Area = data.Area();

            this.CsHeigth = data.CoordinateSystemSize.Height;
            this.CsWidth = data.CoordinateSystemSize.Width;

            if(data.RecognizedGlyph != null)
            {
                this.Name = data.RecognizedGlyph.Name;
            }
        }

        #endregion

    }
}
