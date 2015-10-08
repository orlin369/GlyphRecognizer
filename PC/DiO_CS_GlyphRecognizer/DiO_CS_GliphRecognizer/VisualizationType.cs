// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiO_CS_GliphRecognizer
{
    /// <summary>
    /// Enumeration of visualization types.
    /// </summary>
    enum VisualizationType
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
