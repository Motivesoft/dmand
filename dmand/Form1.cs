using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace dmand
{
    public partial class Form1 : Form
    {
        public Form1( string[] args )
        {
            InitializeComponent();

            LaunchProfile profile;
            if ( args.Length == 0 )
            {
                try
                {
                    profile = Utilities.LoadFrom<LaunchProfile>( "launch" );
                }
                catch ( Exception )
                {
                    // TODO log that we failed to read the profile and so we're using the default
                    profile = LaunchProfile.Default;
                }
            }
            else
            {
                // TODO Implement this propery
                try
                {
                    profile = LaunchProfile.LoadFrom( args[ 0 ] );
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( $"Failed to load from profile: {args[ 0 ]}\n\nReason:\n{ex.Message}", "Profile error", MessageBoxButtons.OK, MessageBoxIcon.Error );

                    // Exit here as this was attempted due to a user request and they probably just mis-typed the path
                    Application.Exit();

                    // We won't get here, but it helps the compiler know this code path ends here
                    return;
                }
            }

            ThemeManager.SetTheme( profile.Theme );
            ThemeManager.Apply( this );

            Utilities.SaveTo<LaunchProfile>( profile, "launch" );
        }

        private void Form1_Load( object sender, EventArgs e )
        {
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }

        private void toolStripCommandPalette_Click( object sender, EventArgs e )
        {
        }

        private void toolStripCommandPalette_Leave( object sender, EventArgs e )
        {
        }

        private void toolStripCommandPalette_Enter( object sender, EventArgs e )
        {
        }

        private void toolStripCommandPalette_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Enter )
            {
            }
            //else
                //ForceDropDownState( true );
        }

        private void toolStripContainer1_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.P && e.Control )
            {
                var model = new PopupListModel();

                // Dummy items - pass model into constructor?
                for ( int loop = 1; loop <= 99; loop++ )
                {
                    model.Items.Add( new PopupListItem( $"Item {loop}" ) );
                }

                var popup = new PopupList( model );
                popup.Show(this);
                var x = popup.Text;
            }

        }
    }
}
