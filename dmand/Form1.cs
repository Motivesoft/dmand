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
        private readonly LaunchProfile Profile;

        public Form1( string[] args )
        {
            InitializeComponent();

            if ( args.Length == 0 )
            {
                try
                {
                    Profile = Utilities.LoadFrom<LaunchProfile>( "launch" );
                }
                catch ( Exception )
                {
                    // TODO log that we failed to read the profile and so we're using the default
                    Profile = LaunchProfile.Default;
                }
            }
            else
            {
                // TODO Implement this propery
                try
                {
                    Profile = LaunchProfile.LoadFrom( args[ 0 ] );
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

            ThemeManager.SetTheme( Profile.Theme );

            Utilities.SaveTo<LaunchProfile>( Profile, "launch" );
        }

        private void Form1_Load( object sender, EventArgs e )
        {
        }

        private Panel CreatePanel( PanelProfile panelProfile )
        {
            var panel = new TableLayoutPanelExtension( panelProfile );

            panel.Dock = DockStyle.Fill;
            panel.RowCount = 2;
            panel.ColumnCount = 1;

            var textBox = new TextBox();
            textBox.Dock = DockStyle.Top | DockStyle.Right;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Text = panelProfile.Location;
            panel.Controls.Add( textBox );

            var listView = new ListView();
            listView.BorderStyle = BorderStyle.FixedSingle;
            listView.Dock = DockStyle.Fill;
            listView.View = (View) Enum.Parse( typeof( View ), panelProfile.View );
            panel.Controls.Add( listView );

            return panel;
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
                    model.Items.Add( new PopupListItem( $"Item {loop}", $"Hint {loop}" ) );
                }

                var popup = new PopupList( model );
                popup.Show(this);

                if ( popup.DialogResult == DialogResult.OK )
                {
                }
            }

        }

        private void Form1_Shown( object sender, EventArgs e )
        {
            var panelCollection = new List<Panel>();
            foreach ( var panelProfile in Profile.Panels )
            {
                panelCollection.Add( CreatePanel( panelProfile ) );
            }

            if ( panelCollection.Count == 1 )
            {
                Panel container = new Panel();
                container.Dock = DockStyle.Fill;
                container.Controls.Add( panelCollection[ 0 ] );
                panelCollection[ 0 ].Dock = DockStyle.Fill;
                toolStripContainer1.ContentPanel.Controls.Add( container );
            }
            foreach ( var panel in panelCollection )
            {
                SplitContainer c = new SplitContainer();

            }

            ThemeManager.Apply( this );
        }
    }
}
