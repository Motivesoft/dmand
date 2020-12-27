using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dmand
{
    /// <summary>
    /// A transient object used to serialise the configuration of the panels in the browser.
    /// These have no value during runtime, only to load and save
    /// </summary>
    public class LaunchProfile
    {
        public static readonly LaunchProfile Default = new LaunchProfile
        {
            Panels = new PanelProfile[] { PanelProfile.Default }
        };

        public PanelProfile[] Panels
        {
            get;
            set;
        }

        public static LaunchProfile LoadFrom( string fileName )
        {
            var jsonString = File.ReadAllText( fileName );
            return JsonSerializer.Deserialize<LaunchProfile>( jsonString );
        }

        public static void SaveTo( LaunchProfile profile, string fileName )
        {
            var jsonString = JsonSerializer.Serialize( profile );
            File.WriteAllText( fileName, jsonString );
        }
    }
}
