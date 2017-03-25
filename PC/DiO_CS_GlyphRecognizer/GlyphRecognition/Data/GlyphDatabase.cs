// Glyph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AForge.Vision.GlyphRecognition.Data
{

    /// <summary>
    /// Glyphs' database.
    /// </summary>
    /// <remarks><para>The class represents collection of glyphs, which cab be recognized with the help of
    /// <see cref="GlyphRecognizer"/>.</para></remarks>
    [Serializable]
    public class GlyphDatabase : IEnumerable<Glyph>
    {

        #region Variables

        /// <summary>
        /// Glyphs.
        /// </summary>
        private Dictionary<string, Glyph> container = new Dictionary<string, Glyph>();

        /// <summary>
        /// Size
        /// </summary>
        private int size;

        #endregion

        #region Constructor

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public GlyphDatabase()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphDatabase"/> class.
        /// </summary>
        /// 
        /// <param name="size"><see cref="Size">Size</see> of glyphs to store in the database.</param>
        /// 
        public GlyphDatabase(int size)
        {
            this.size = size;
        }

        #endregion

        #region IEnumerable<Glyph> 

        /// <summary>
        /// Get glyph's enumerator.
        /// </summary>
        /// <returns>Returns glyph's enumerator.</returns>
        public IEnumerator<Glyph> GetEnumerator()
        {
            foreach (KeyValuePair<string, Glyph> item in container)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Size of glyphs in the database.
        /// </summary>
        /// <remarks><para>All glyph of a database are <b>size</b>x<b>size</b> square glyph.</para></remarks>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Number of glyphs in the database.
        /// </summary>
        public int Count
        {
            get { return container.Count; }
        }

        /// <summary>
        /// Get glyph by its name.
        /// </summary>
        /// 
        /// <param name="name">Name of the glyph to retrieve for the database.</param>
        /// 
        /// <returns>Returns the glyph with the specified name or <see langword="null"/> if such
        /// glyph does not exist.</returns>
        /// 
        public Glyph this[string name]
        {
            get { return (container.ContainsKey(name)) ? container[name] : null; }
        }
        
        /// <summary>
        /// Add glyph to the database.
        /// </summary>
        /// 
        /// <param name="glyph">Glyph to add to the database.</param>
        /// 
        /// <exception cref="ApplicationException">Glyph size does not match the database.</exception>
        /// 
        public void Add(Glyph glyph)
        {
            if (glyph.Size != size)
            {
                throw new ApplicationException("Glyph size does not match the database.");
            }

            container.Add(glyph.Name, glyph);
        }

        /// <summary>
        /// Remove a glyph from the database.
        /// </summary>
        /// 
        /// <param name="name">Glyph name to remove from the database.</param>
        /// 
        public void Remove(string name)
        {
            if (container.ContainsKey(name))
                container.Remove(name);
        }

        /// <summary>
        /// Replace a glyph in the database.
        /// </summary>
        /// 
        /// <param name="name">Name of the glyph to replace.</param>
        /// <param name="newGlyph">New glyph to put into the database.</param>
        /// 
        /// <remarks><para>If the specified glyph's <paramref name="name"/> equals to the <see cref="Glyph.Name">name of the new glyph</see>,
        /// then the database is just updated with the new glyph. But if these names are different, then the old glyph with the specified
        /// name is removed from the database and the new glyph is added.</para></remarks>
        /// 
        /// <exception cref="ArgumentException">A glyph with the specified <paramref name="name"/> does not exist in the database.</exception>
        /// 
        public void Replace(string name, Glyph newGlyph)
        {
            if (!container.ContainsKey(name))
            {
                throw new ArgumentException("A glyph with the specified name does not exist in the database.");
            }

            if (name == newGlyph.Name)
            {
                container[name] = newGlyph;
            }
            else
            {
                Remove(name);
                Add(newGlyph);
            }
        }

        /// <summary>
        /// Rename a glyph in the database.
        /// </summary>
        /// 
        /// <param name="name">Name of the glyph to rename.</param>
        /// <param name="newName">New name of the glyph to set.</param>
        /// 
        /// <exception cref="ArgumentException">A glyph with the specified <paramref name="name"/> does not exist in the database.</exception>
        /// 
        public void Rename(string name, string newName)
        {
            if (!container.ContainsKey(name))
            {
                throw new ArgumentException("A glyph with the specified name does not exist in the database.");
            }

            if (name == newName)
                return;

            Glyph glyph = container[name];
            container.Remove(name);

            glyph.Name = newName;
            container.Add(newName, glyph);
        }

        /// <summary>
        /// Get collection of glyph names available in the database.
        /// </summary>
        /// 
        /// <returns>Returns read only collection of glyph names.</returns>
        /// 
        public ReadOnlyCollection<string> GetGlyphNames()
        {
            return new ReadOnlyCollection<string>(new List<string>(container.Keys));
        }

        /// <summary>
        /// Recognize the glyph represented by the specified raw glyph's data.
        /// </summary>
        /// <param name="rawGlyphData">Raw glyph data to recognize.</param>
        /// <param name="rotation">Contains rotation angle of the match on success (0, 90, 180 or 270) -
        /// see <see cref="Glyph.CheckForMatching(byte[,])"/>. In the case of no matching is found the value is
        /// assigned to -1.</param>
        /// <returns>Returns a glyph from the database which matches the specified raw glyph data. If there is
        /// no matching found, then <see langword="null"/> is returned.</returns>
        /// <remarks><para>The method searches for a glyph in the database which matches (see <see cref="Glyph.CheckForMatching(byte[,])"/>)
        /// the specified raw glyph data.</para></remarks>
        /// 
        public Glyph RecognizeGlyph(byte[,] rawGlyphData, out int rotation)
        {
            foreach (KeyValuePair<string, Glyph> pair in container)
            {
                if ((rotation = pair.Value.CheckForMatching(rawGlyphData)) != -1)
                    return pair.Value;
            }

            rotation = -1;
            return null;
        }

        #endregion

    }
}
