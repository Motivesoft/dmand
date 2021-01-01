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
    public class ThemeManager
    {
        public static Theme CurrentTheme;

        public static bool SetTheme( string themeName )
        {
            if ( !String.IsNullOrEmpty( themeName ) )
            {
                var themes = Utilities.LoadFrom<List<Theme>>( "themes" );
                foreach ( var theme in themes )
                {
                    if ( theme.Name.Equals( themeName ) )
                    {
                        CurrentTheme = theme;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryAndGet( Control control, string propertyName, out Color color, Theme theme = null )
        {
            if ( theme == null )
            {
                if ( CurrentTheme == null )
                {
                    // With a false return, color can simply be anything as it should be ignored
                    color = Color.Transparent;
                    return false;
                }
                theme = CurrentTheme;
            }

            Type t = control.GetType();

            // Get a list of types most specific to most general
            var typeStack = new List<Type>();
            while ( typeof( Control ).IsAssignableFrom( t ) )
            {
                typeStack.Add( t );
                t = t.BaseType;
            }

            // See if there is a property to set on the object based on its class
            // Note that this ignores interfaces

            foreach ( var type in typeStack )
            {
                var key = $"{type.Name}.{propertyName}";
                if ( theme.Colors.ContainsKey( key ) )
                {
                    // Set the property on the most specific type and then
                    // go on to the next property
                    color = theme.Colors[ key ];
                    return true;
                }
            }

            // With a false return, color can simply be anything as it should be ignored
            color = Color.Transparent;
            return false;
        }

        public static void Apply( Control parent, Theme theme = null )
        {
            if ( theme == null )
            {
                if ( CurrentTheme == null )
                {
                    return;
                }
                theme = CurrentTheme;
            }

            var list = new Queue<Control>();
            list.Enqueue( parent );

            while ( list.Count > 0 )
            {
                var control = list.Dequeue();

                ApplyTheme( theme, control );
                foreach ( Control child in control.Controls )
                {
                    list.Enqueue( child );
                }
            }
        }

        private static void ApplyTheme( Theme theme, Control control )
        {
            Type t = control.GetType();

            // Get a list of types most specific to most general
            var typeStack = new List<Type>();
            while ( typeof( Control ).IsAssignableFrom( t ) )
            {
                typeStack.Add( t );
                t = t.BaseType;
            }

            // See if there is a property to set on the object based on its class
            // Note that this ignores interfaces

            foreach ( var property in control.GetType().GetProperties() )
            {
                foreach ( var type in typeStack )
                {
                    var key = $"{type.Name}.{property.Name}";
                    if ( theme.Colors.ContainsKey( key ) )
                    {
                        // Set the property on the most specific type and then
                        // go on to the next property
                        property.SetValue( control, theme.Colors[ key ] );
                        break;
                    }
                }
            }
        }
    }
}
