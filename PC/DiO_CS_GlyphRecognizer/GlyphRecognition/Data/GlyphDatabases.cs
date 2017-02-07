// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;

namespace AForge.Vision.GlyphRecognition.Data
{
    /// <summary>
    /// Graphics data base descriptor class.
    /// </summary>
    public class GlyphDatabases
    {

        #region Constants

        private const string databaseTag = "Database";
        private const string glyphTag = "Glyph";
        private const string nameAttr = "Name";
        private const string sizeAttr = "Size";
        private const string dataAttr = "Data";
        private const string countAttr = "Count";
        private const string colorAttr = "Color";
        private const string iconAttr = "Icon";
        private const string modelAttr = "Model";

        #endregion

        #region Variables

        /// <summary>
        /// Data container.
        /// </summary>
        private Dictionary<string, GlyphDatabase> container = new Dictionary<string, GlyphDatabase>( );

        #endregion

        #region Properties

        /// <summary>
        /// Array indexer.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Database</returns>
        public GlyphDatabase this[string name]
        {
            get { return container[name]; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add glyph database to collection.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="db">Database.</param>
        public void AddGlyphDatabase(string name, GlyphDatabase db)
        {
            if ( !container.ContainsKey( name ) )
            {
                container.Add( name, db );
            }
            else
            {
                throw new ApplicationException( "A glyph database with such name already exists : " + name );
            }
        }

        /// <summary>
        /// Remove glyph database from collection.
        /// </summary>
        /// <param name="name">Name</param>
        public void RemoveGlyphDatabase(string name)
        {
            if ( container.ContainsKey( name ) )
            {
                container.Remove( name );
            }
        }

        /// <summary>
        /// Rename glyph database.
        /// </summary>
        /// <param name="oldName">Old name</param>
        /// <param name="newName">New name</param>
        public void RenameGlyphDatabase( string oldName, string newName )
        {
            if ( oldName != newName )
            {
                if ( container.ContainsKey( newName ) )
                {
                    throw new ApplicationException( "A glyph database with such name already exists : " + newName );
                }

                if ( !container.ContainsKey( oldName ) )
                {
                    throw new ApplicationException( "A glyph database with such name does not exist : " + oldName );
                }

                // insert it with new key
                container.Add( newName, container[oldName] );
                // remove it from dictonary with the old key
                container.Remove( oldName );
            }
        }

        /// <summary>
        /// Get list of available databases' names.
        /// </summary>
        /// <returns>List of names.</returns>
        public List<string> GetDatabaseNames( )
        {
            return new List<string>( container.Keys );
        }

        /// <summary>
        /// Save infromation about all databases and glyphs into XML writer.
        /// </summary>
        /// <param name="xmlOut">XML writer.</param>
        public void Save( XmlTextWriter xmlOut )
        {
            foreach ( KeyValuePair<string, GlyphDatabase> kvp in container )
            {
                xmlOut.WriteStartElement( databaseTag );
                xmlOut.WriteAttributeString( nameAttr, kvp.Key );
                xmlOut.WriteAttributeString( sizeAttr, kvp.Value.Size.ToString( ) );
                xmlOut.WriteAttributeString( countAttr, kvp.Value.Count.ToString( ) );

                // save glyps
                foreach ( Glyph glyph in kvp.Value )
                {
                    xmlOut.WriteStartElement( glyphTag );
                    xmlOut.WriteAttributeString( nameAttr, glyph.Name );
                    xmlOut.WriteAttributeString( dataAttr, GlyphDataToString( glyph.Data ) );

                    if ( glyph.UserData != null )
                    {
                        GlyphVisualizationData visualization = (GlyphVisualizationData) glyph.UserData;

                        // highlight color
                        xmlOut.WriteAttributeString( colorAttr, string.Format( "{0},{1},{2}",
                            visualization.Color.R, visualization.Color.G, visualization.Color.B ) );
                        // glyph's image
                        xmlOut.WriteAttributeString( iconAttr, visualization.ImageName );
                        // glyph's 3D model
                        xmlOut.WriteAttributeString( modelAttr, visualization.ModelName );
                    }

                    xmlOut.WriteEndElement( );
                }

                xmlOut.WriteEndElement( );
            }
        }

        /// <summary>
        /// Load information about databases and glyphs from XML reader.
        /// </summary>
        /// <param name="xmlIn">XML Reader</param>
        public void Load( XmlTextReader xmlIn )
        {
            // read to the first node
            xmlIn.Read( );

            int startingDept = xmlIn.Depth;

            while ( ( xmlIn.Name == databaseTag ) && ( xmlIn.NodeType == XmlNodeType.Element ) && ( xmlIn.Depth >= startingDept ) )
            {
                string name = xmlIn.GetAttribute( nameAttr );
                int size = int.Parse( xmlIn.GetAttribute( sizeAttr ) );
                int count = int.Parse( xmlIn.GetAttribute( countAttr ) );

                // create new database and add it to collection
                GlyphDatabase db = new GlyphDatabase( size );
                AddGlyphDatabase( name, db );

                if ( count > 0 )
                {
                    // read all glyphs
                    for ( int i = 0; i < count; i++ )
                    {
                        // read to the next glyph node
                        xmlIn.Read( );

                        string glyphName = xmlIn.GetAttribute( nameAttr );
                        string glyphStrData = xmlIn.GetAttribute( dataAttr );

                        // create new glyph and add it database
                        Glyph glyph = new Glyph( glyphName, GlyphDataFromString( glyphStrData, size ) );
                        db.Add( glyph );

                        // read visualization params
                        GlyphVisualizationData visualization = new GlyphVisualizationData( Color.Red );

                        visualization.ImageName = xmlIn.GetAttribute( iconAttr );
                        visualization.ModelName = xmlIn.GetAttribute( modelAttr );

                        string colorStr = xmlIn.GetAttribute( colorAttr );

                        if ( colorStr != null )
                        {
                            string[] rgbStr = colorStr.Split( ',' );

                            visualization.Color = Color.FromArgb(
                                int.Parse( rgbStr[0] ), int.Parse( rgbStr[1] ), int.Parse( rgbStr[2] ) );
                        }

                        glyph.UserData = visualization;
                    }

                    // read to the end tag
                    xmlIn.Read( );
                }

                // read to the next node
                xmlIn.Read( );
            }
        }

        #endregion

        #region Tool Methods

        private static string GlyphDataToString( byte[,] glyphData )
        {
            StringBuilder sb = new StringBuilder( );
            int glyphSize = glyphData.GetLength( 0 );

            for ( int i = 0; i < glyphSize; i++ )
            {
                for ( int j = 0; j < glyphSize; j++ )
                {
                    sb.Append( glyphData[i, j] );
                }
            }

            return sb.ToString( );
        }

        private static byte[,] GlyphDataFromString( string glyphStrData, int glyphSize )
        {
            byte[,] glyphData = new byte[glyphSize, glyphSize];

            for ( int i = 0, k = 0; i < glyphSize; i++ )
            {
                for ( int j = 0; j < glyphSize; j++, k++ )
                {
                    glyphData[i, j] = (byte) ( ( glyphStrData[k] == '0' ) ? 0 : 1 );
                }
            }

            return glyphData;
        }

        #endregion

    }
}
