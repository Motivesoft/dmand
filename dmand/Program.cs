using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmand
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args )
        {
            var themes = Utilities.LoadFrom<List<Theme>>( "themes" );
            if ( themes.Count > 0 )
            {
                ThemeManager.CurrentTheme = themes[ 0 ];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new Form1( args ) );
        }
    }
}
