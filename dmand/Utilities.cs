using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmand
{
    public class Utilities
    {
        private const string HEX_DIGITS = "0123456789abcdef";
        private const int MINIMUM_UID_LENGTH = 16;

        private static readonly SortedSet<string> GeneratedIds = new SortedSet<string>();
        private static readonly Random random = new Random( (int) DateTimeOffset.Now.ToUnixTimeSeconds() );
        private static readonly ImageList imageList = new ImageList();

        /// <summary>
        /// Create a hexadecimal identifier that is unique within this execution
        /// </summary>
        /// <returns>The generated identifier</returns>
        public static string CreateUniqueId( int length = MINIMUM_UID_LENGTH )
        {
            string value = "";

            if ( length < MINIMUM_UID_LENGTH )
            {
                throw new ArgumentException( $"UID length must be {MINIMUM_UID_LENGTH} or higher" );
            }

            lock ( GeneratedIds )
            {
                // Create a hex string. If we can add it to the set of previously generated
                // values, then it must be unique. I doubt there would ever be a clash in
                // normal usage as long as the length remains suitably high (16?)

                do
                {
                    StringBuilder Id = new StringBuilder();
                    for ( int loop = 0; loop < length; loop++ )
                    {
                        Id.Append( HEX_DIGITS[ random.Next( HEX_DIGITS.Length ) ] );
                    }
                    value = Id.ToString();
                }
                while ( !GeneratedIds.Add( value ) );
            }

            return value;
        }

        public static T LoadFrom<T>( string fileName )
        {
            string path = PathForUserConfiguration( fileName );

            var jsonString = File.ReadAllText( path );
            var options = new JsonSerializerOptions
            {
                Converters = { new ColorConverter() }
            };

            return JsonSerializer.Deserialize<T>( jsonString, options );
        }

        public static void SaveTo<T>( T themes, string fileName )
        {
            string path = PathForUserConfiguration( fileName );
            Directory.CreateDirectory( Path.GetDirectoryName( path ) );

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new ColorConverter() }
            };
            var jsonString = JsonSerializer.Serialize( themes, options );

            File.WriteAllText( path, jsonString );
        }

        public static string PathForUserConfiguration( string filename )
        {
            return PathForConfiguration( true, filename );
        }

        public static string PathForSystemConfiguration( string filename )
        {
            return PathForConfiguration( false, filename );
        }

        private static string PathForConfiguration( bool perUser, string filename )
        {
#if DEBUG
            // For debug purposes, run from a local set of config files
            var parentFolder = Path.GetFullPath( "." );
#else
            string folderPath;
            if ( perUser )
            {
                folderPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
            }
            else
            {
                folderPath = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
            }
            var parentFolder = Path.Combine( folderPath, Application.CompanyName, Application.ProductName );
#endif
            return Path.Combine( parentFolder, filename + ".json" ); 
        }
    }
}
