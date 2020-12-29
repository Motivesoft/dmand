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

            ThemeManager.Apply( this );

            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += ( object sender, DrawItemEventArgs e ) => {
                e.DrawBackground();
                e.DrawFocusRectangle();

                var item = (PopupListItem) listBox1.Items[ e.Index ];
                var value = item.Value;
                var hint = item.Hint;

                var sizeValue = e.Graphics.MeasureString( value, e.Font );
                var sizeHint = string.IsNullOrEmpty( hint ) ? new SizeF( 0, 0 ) : e.Graphics.MeasureString( hint, e.Font );

                e.Graphics.DrawString( value, e.Font, new SolidBrush( Color.Red ), e.Bounds );

                if ( e.Bounds.Width > sizeValue.Width + sizeHint.Width )
                {
                    var hintBounds = new Rectangle( (int) ( e.Bounds.X + e.Bounds.Width - sizeHint.Width ), e.Bounds.Y, (int) sizeHint.Width, e.Bounds.Height );
                    e.Graphics.DrawString( hint, e.Font, new SolidBrush( Color.Green ), e.Bounds );
                }
            };

            UpdateList();

            // Set a default in case any creative exit routes are discovered
            DialogResult = DialogResult.Cancel;
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
                listBox1.Items.Add( item );
            }
            listBox1.SelectedItem = selection;
            listBox1.EndUpdate();
        }

        private void listBox1_Click( object sender, EventArgs e )
        {
            var item = (PopupListItem) listBox1.SelectedItem;
            textBox1.Text = item.Value;
            CloseWithResult( DialogResult.OK );
        }

        private void CloseWithResult( DialogResult result )
        {
            DialogResult = result;
            Outcome = result == DialogResult.OK ? textBox1.Text : null;
            Dispose();
        }

        private void PopupList_Deactivate( object sender, EventArgs e )
        {
            if ( !System.Diagnostics.Debugger.IsAttached )
            {
                CloseWithResult( DialogResult.Cancel );
            }
        }
    }
}
