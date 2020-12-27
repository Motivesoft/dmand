﻿using System;
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

        public PopupList( int visibleItemCount = 20 )
        {
            VisibleItemCount = visibleItemCount;

            InitializeComponent();

            // Dummy items - pass model into constructor?
            for ( int loop = 1; loop <= 30; loop++ )
            {
                listBox1.Items.Add( $"Item {loop}" );
            }

            var themes = Utilities.LoadFrom<List<Theme>>( "themes" );
            if ( themes.Count > 0 )
            {
                new ThemeManager().Apply( themes[ 0 ], this );
            }
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
            if ( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return )
            {
                Dispose();
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
        }
    }
}