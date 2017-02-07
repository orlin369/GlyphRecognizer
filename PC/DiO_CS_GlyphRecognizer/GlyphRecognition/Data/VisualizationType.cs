// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition.Data
{
    /// <summary>
    /// Enumeration of visualization types.
    /// </summary>
    public enum VisualizationType
    {
        /// <summary>
        /// Hightlight glyph with border only.
        /// </summary>
        BorderOnly,

        /// <summary>
        /// Hightlight glyph with border and put its name in the center.
        /// </summary>
        Name,

        /// <summary>
        /// Substitue glyph with its image.
        /// </summary>
        Image,

        /// <summary>
        /// Show 3D model over the glyph.
        /// </summary>
        Model
    }
}
