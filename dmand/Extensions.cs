using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmand
{
    public class TableLayoutPanelExtension : Panel
    {
        public PanelProfile PanelProfile
        {
            get;
            private set;
        }

        private readonly TextBox textBox;
        private readonly ListView listView;

        public TableLayoutPanelExtension( PanelProfile panelProfile )
        {
            PanelProfile = panelProfile;

            SuspendLayout();

            textBox = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.FixedSingle,
            };
            listView = new ListView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                HeaderStyle = ColumnHeaderStyle.Clickable,
                LabelEdit = false,
                AllowColumnReorder = true,
                FullRowSelect = true,
                GridLines = true,
            };

            Dock = DockStyle.Fill;
            Controls.Add( listView );
            Controls.Add( textBox );

            textBox.Text = panelProfile.Location;
            textBox.KeyDown += ( object sender, KeyEventArgs e ) =>
            {
                if ( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return )
                {
                    DeferSwitchLocation( textBox.Text );
                }
            };

            ColumnHeader iconHeader = new ColumnHeader { Width = 20, Text = "Icon" };
            ColumnHeader nameHeader = new ColumnHeader { Width = 200, Text = "Name" };
            ColumnHeader sizeHeader = new ColumnHeader { Width = 50, Text = "Size" };
            ColumnHeader dateHeader = new ColumnHeader { Width = 50, Text = "Date" };

            listView.View = (View) Enum.Parse( typeof( View ), panelProfile.View );
            listView.Columns.AddRange( new ColumnHeader[]
            {
                new ColumnHeader(),
                iconHeader,
                nameHeader,
                sizeHeader,
                dateHeader,
            } );

            PerformLayout();
            ResumeLayout();

            DeferSwitchLocation( panelProfile.Location );
        }

        private void DeferSwitchLocation( string path )
        {
            Task.Factory.StartNew( () =>
            {
                SwitchLocation( path );
            } );
        }

        private void SwitchLocation( string path )
        {
            string newLocation = path;
            string oldLocation = PanelProfile.Location;

            // Try and change to new location. 
            // If it works, set that into PanelProfile.Location and exit
            // If it fails:
            //      If PanelProfile.Location is the same value then panic - maybe drop back to root of main drive?
            //      Otherwise, revert back to original location
            //      In both cases, show a message box to explain
            //
            // Remember that we are not (should not be) on the event thread here

            // Clear the view so it is obvious we are not in Kansas anymore
            ClearListView();
            UpdateTextField( "" );

            // Build the view model and then update the list view
            var directories = Directory.EnumerateDirectories( path );
            var files = Directory.EnumerateFiles( path );

            var items = new List<string>();
            foreach ( var item in directories )
            {
                items.Add( item );
            }

            // Set the text field to the location and update the list view
            UpdateTextField( newLocation );
            UpdateListView( items );
        }

        private void ClearListView()
        {
            if ( InvokeRequired )
            {
                Invoke( (MethodInvoker) delegate
                {
                    ClearListView();
                } );
                return;
            }

            listView.Items.Clear();
        }

        private void UpdateListView( List<string> items )
        {
            if ( InvokeRequired )
            {
                Invoke( (MethodInvoker) delegate
                {
                    UpdateListView( items );
                } );
                return;
            }

            listView.BeginUpdate();
            foreach ( var item in items )
            {
                var lvi = new ListViewItem( item, 0 );
                lvi.Name = item;
                lvi.SubItems.Add( item );
                lvi.SubItems.Add( item );
                lvi.SubItems.Add( item );
//                listView.Items.Add( lvi );
            }
            listView.EndUpdate();
            Refresh();
        }

        private void UpdateTextField( string path )
        {
            if ( InvokeRequired )
            {
                Invoke( (MethodInvoker) delegate
                {
                    UpdateTextField( path );
                } );
                return;
            }

            textBox.Text = path;
        }
    }

    public class ListBoxExtension : ListBox
    {
        public Color SelectionForeColor
        {
            get;
            set;
        }

        public Color SelectionBackColor
        {
            get;
            set;
        }
    }

    public class ListViewExtension : ListView
    {
        
    }
}
