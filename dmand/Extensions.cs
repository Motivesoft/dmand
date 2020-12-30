using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmand
{
    public class PanelExtension : Panel
    {
        public PanelProfile PanelProfile
        {
            get;
            private set;
        }

        public PanelExtension( PanelProfile panelProfile )
        {
            PanelProfile = panelProfile;
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
}
