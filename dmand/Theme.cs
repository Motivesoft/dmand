using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dmand
{
    public partial class Theme
    {
        public String Name
        {
            get;
            set;
        }

        public Dictionary<string, Color> Colors
        {
            get;
            set;
        }

        public static List<Theme> LoadFrom( string fileName )
        {
            var jsonString = File.ReadAllText( fileName );
            return JsonSerializer.Deserialize<List<Theme>>( jsonString );
        }

        public static void SaveTo( List<Theme> themes, string fileName )
        {
            var jsonString = JsonSerializer.Serialize( themes );
            File.WriteAllText( Utilities.PathForUserConfiguration( fileName ), jsonString );
        }
    }
}
