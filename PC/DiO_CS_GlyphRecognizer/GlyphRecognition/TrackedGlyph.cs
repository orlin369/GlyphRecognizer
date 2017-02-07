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

using System.Collections.Generic;

using AForge.Vision.GlyphRecognition.Data;

namespace AForge.Vision.GlyphRecognition
{

    /// <summary>
    /// Information about the tracked glyph.
    /// </summary>
    public class TrackedGlyph
    {

        #region Constants

        /// <summary>
        /// Max motion history length.
        /// </summary>
        private const int MaxMotionHistoryLength = 11;

        /// <summary>
        /// Recent steps count.
        /// </summary>
        private const int RecentStepsCount = 10;

        #endregion

        #region Variables

        /// <summary>
        /// List of the motion history.
        /// </summary>
        private readonly List<Point> motionHistory = new List<Point>();

        #endregion

        #region Properties

        /// <summary>
        /// Glyph ID.
        /// </summary>
        public readonly int ID;

        /// <summary>
        /// Glyph data.
        /// </summary>
        public ExtractedGlyphData Glyph { get; set; }

        /// <summary>
        /// Actual position
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Recent path length.
        /// </summary>
        public double RecentPathLength { get; set; }

        /// <summary>
        /// Average recent motion.
        /// </summary>
        public double AverageRecentMotion { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Glyph ID</param>
        /// <param name="glyph">Glyph</param>
        /// <param name="position">Position</param>
        public TrackedGlyph(int id, ExtractedGlyphData glyph, Point position)
        {
            this.ID = id;
            this.Glyph = glyph;
            this.Position = position;
            this.Age = 0;
            this.RecentPathLength = 0;
            this.AverageRecentMotion = 0;
    }

    #endregion

        #region Public Methods

        /// <summary>
        /// Add motion history.
        /// </summary>
        /// <param name="position">Point in time.</param>
        public void AddMotionHistory(Point position)
        {
            this.motionHistory.Add(position);

            if (this.motionHistory.Count > MaxMotionHistoryLength)
            {
                this.motionHistory.RemoveAt(0);
            }

            // calculate amount of recent movement
            this.RecentPathLength = 0;
            int stepsCount = System.Math.Min(RecentStepsCount, motionHistory.Count - 1);
            int historyLimit = MaxMotionHistoryLength - stepsCount;

            for (int i = this.motionHistory.Count - 1; i >= historyLimit; i--)
            {
                this.RecentPathLength += this.motionHistory[i].DistanceTo(this.motionHistory[i - 1]);
            }

            this.AverageRecentMotion = (stepsCount == 0) ? 0 : this.RecentPathLength / stepsCount;
        }

        #endregion

    }
}
