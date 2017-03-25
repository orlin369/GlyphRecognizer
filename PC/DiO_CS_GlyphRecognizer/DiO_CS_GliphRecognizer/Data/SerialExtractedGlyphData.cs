using AForge.Vision.GlyphRecognition.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiO_CS_GliphRecognizer.Data
{
    class SerialExtractedGlyphData
    {
        public string Name { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public float Roll { get; private set; }
        public double Area { get; private set; }
        public double CsWidth { get; private set; }
        public double CsHeigth { get; private set; }



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

    }
}
