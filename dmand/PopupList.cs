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

        public string Outcome
        {
            get;
            private set;
        }

        public PopupList( PopupListModel model, int visibleItemCount = 20 )
        {
            Model = model;
            VisibleItemCount = visibleItemCount;

            InitializeComponent();

            UpdateList();

            ThemeManager.Apply( this );
        }

        private void textBox1_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Escape )
            {
                CloseWithResult( DialogResult.Cancel );
            }
            else if ( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return )
            {
                CloseWithResult( DialogResult.OK );
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
            var selection = listBox1.SelectedItem;
            listBox1.Items.Clear();
            foreach ( var item in Model.GetFilteredList( textBox1.Text ) )
            {
                listBox1.Items.Add( item.Value );
            }
            listBox1.SelectedItem = selection;
            listBox1.EndUpdate();
        }

        private void listBox1_Click( object sender, EventArgs e )
        {
            textBox1.Text = listBox1.SelectedItem.ToString();
            CloseWithResult( DialogResult.OK );
        }

        private void CloseWithResult( DialogResult result )
        {
            DialogResult = result;
            Outcome = result == DialogResult.OK ? textBox1.Text : null;
            Dispose();
        }
    }
}
