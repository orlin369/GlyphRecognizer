using AForge.Vision.GlyphRecognition.Data;
using DiO_CS_GliphRecognizer.Adapters;
using DiO_CS_GliphRecognizer.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiO_CS_GliphRecognizer.Connectors
{
    class DataConnector
    {
        private Adapter adapter;

        public bool IsConnected
        {
            get
            {
                if (this.adapter == null) return false;

                return this.adapter.IsConnected;
            }
        }

        public DataConnector(Adapter adapter)
        {
            this.adapter = adapter;
        }

        public void SendData(string data)
        {
            adapter.SendRequest(data);
        }

        public void SendData(byte[] data)
        {
            adapter.SendImageBytes(data);
        }

        public void SendGlyph(ExtractedGlyphData egd)
        {
            SerialExtractedGlyphData sgd = new SerialExtractedGlyphData(egd);
            string json = JsonConvert.SerializeObject(sgd);
            this.SendData(json);
        }

        public void SendImage(Bitmap image)
        {
            if (this.adapter == null) return;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                this.adapter.SendImageBytes(ms.ToArray());
            }
        }

        internal void Connect()
        {
            if (adapter == null) return;

            this.adapter.Connect();
        }

        internal void Disconnect()
        {
            if (adapter == null) return;

            this.adapter.Disconnect();
        }
    }
}
