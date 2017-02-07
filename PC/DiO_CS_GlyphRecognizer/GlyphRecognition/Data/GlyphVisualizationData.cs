// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Drawing;

namespace AForge.Vision.GlyphRecognition.Data
{
    /// <summary>
    /// Data used for visualization of recognized glyph
    /// </summary>
    [Serializable]
    public class GlyphVisualizationData
    {

        #region Properties

        /// <summary>
        /// Color to use for highlight and glyph name.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Image to show in the quadrilateral of recognized glyph.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// 3D model name to show for the glyph.
        /// </summary>
        public string ModelName { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">Color</param>
        public GlyphVisualizationData(Color color)
        {
            Color = color;
            ImageName = null;
            ModelName = null;
        }

        #endregion

    }
}
