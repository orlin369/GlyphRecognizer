using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using AForge.Vision.GlyphRecognition;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DiO_CS_GliphRecognizer
{
    public class GlyphProcessor
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
        /// Object used for synchronization.
        /// </summary>
        private object sync = new object( );

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

        public bool ShowPoints
        {
            get;
            set;
        }

        public bool ShowTitle
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public GlyphProcessor()
        {
            // TODO: Constructor...
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
            glyphTracker.ImageSize = bitmap.Size;

            // get list of recognized glyphs
            glyphs.AddRange(recognizer.FindGlyphs(bitmap));
            List<int> glyphIDs = glyphTracker.TrackGlyphs(glyphs);



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
