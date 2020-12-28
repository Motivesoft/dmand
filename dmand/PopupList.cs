using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmand
{
    public partial class PopupList : Form
    {
        private readonly int VisibleItemCount;
        private readonly PopupListModel Model;

        public PopupList( PopupListModel model, int visibleItemCount = 20 )
        {
            Model = model;
            VisibleItemCount = visibleItemCount;

            InitializeComponent();

            UpdateList();

            ThemeManager.Apply( this );
        }

        private void PopupList_KeyDown( object sender, KeyEventArgs e )
        {

        }

        private void textBox1_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Escape )
            {
                Dispose();
            }
            else if ( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return )
            {
                Dispose();
            }
            else if ( e.KeyCode == Keys.Down )
            {
                if ( listBox1.SelectedIndex == listBox1.Items.Count - 1 )
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    listBox1.SelectedIndex++;
                }
            }
            else if ( e.KeyCode == Keys.Up )
            {
                if ( listBox1.SelectedIndex == 0 )
                {
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    listBox1.SelectedIndex--;
                }
            }
            else
            {
                // TODO filter listbox based on textbox
            }
        }

        private void PopupList_Shown( object sender, EventArgs e )
        {
            var x = (double) listBox1.Height / listBox1.ItemHeight;
            if ( x != VisibleItemCount )
            {
                var th = textBox1.Height;
                var lh = listBox1.ItemHeight * VisibleItemCount;
                var dh = th + lh + 8;
                Height = dh;
            }

            Width = Math.Min( 400, ( Owner.Width * 2 ) / 3 );

            var ownerRectangle = Owner.RectangleToScreen( Owner.ClientRectangle );

            Location = new Point( Owner.Location.X + ( ( Owner.Width - Width ) / 2 ), ownerRectangle.Top + 10 );
        }

        private void textBox1_TextChanged( object sender, EventArgs e )
        {
            UpdateList();
        }

        private void UpdateList()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach ( var item in Model.GetFilteredList( textBox1.Text ) )
            {
                Console.WriteLine( "CCC" );
                listBox1.Items.Add( item.Value );
            }
            listBox1.EndUpdate();
        }
    }
}
