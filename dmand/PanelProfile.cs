using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dmand
{
    /// <summary>
    /// A transient object used to serialise the configuration of a single panel in the browser.
    /// These have no value during runtime, only to load and save
    /// </summary>
    public class PanelProfile
    {
        public static readonly PanelProfile Default = new PanelProfile
        {
            Location = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ),
            PanelId = Utilities.CreateUniqueId(),
            View = System.Windows.Forms.View.Details.ToString()
        };

        public string PanelId
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public string View
        {
            get;
            set;
        }

        public static PanelProfile LoadFrom( string fileName )
        {
            var jsonString = File.ReadAllText( fileName );
            return JsonSerializer.Deserialize<PanelProfile>( jsonString );
        }

        public static void SaveTo( PanelProfile profile, string fileName )
        {
            var jsonString = JsonSerializer.Serialize( profile );
            File.WriteAllText( fileName, jsonString );
        }
    }
}
