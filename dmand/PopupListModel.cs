using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dmand
{
    public class PopupListItem : IComparable<PopupListItem>
    {
        public string Value
        {
            get;
            private set;
        }
        public string Hint
        {
            get;
            private set;
        }

        public PopupListItem( string value, string hint = null )
        {
            Value = value;
            Hint = hint;
        }

        public int CompareTo( PopupListItem other )
        {
            return Value.CompareTo( other.Value );
        }
    }

    public class PopupListModel
    {
        public readonly SortedSet<PopupListItem> Items = new SortedSet<PopupListItem>();

        public List<PopupListItem> GetFilteredList( string filter )
        {
            var patternBuilder = new StringBuilder();
            foreach ( var c in filter )
            {
                patternBuilder.Append( $".*{c}" );
            }
            patternBuilder.Append( $".*" );
            var pattern = patternBuilder.ToString();

            return Items.Where<PopupListItem>( p =>
            {
                return Regex.Match( p.Value, pattern, RegexOptions.IgnoreCase ).Success;
            } ).ToList();
        }
    }
}
