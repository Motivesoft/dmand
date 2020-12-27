using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dmand
{
    public class ColorConverter : JsonConverter<Color>
    {
        public override Color Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        {
            var value = reader.GetString();

            Color color;
            if ( value.StartsWith( "#" ) )
            {
                var hex = value.Substring( 1 );
                int colorValue = 0;

                // Allow 6 digit RGB and convert to ARGB with max alpha
                if ( hex.Length == 6 )
                {
                    hex = "ff" + hex;
                }

                bool first = true;
                foreach ( var c in hex )
                {
                    if ( !first )
                    {
                        colorValue <<= 4;
                    }
                    first = false;
                    if ( c >= '0' && c <= '9' )
                    {
                        colorValue += ( c - '0' );
                    }
                    else if ( c >= 'a' && c <= 'f' )
                    {
                        colorValue += ( c - 'a' + 10 );
                    }
                    else if ( c >= 'A' && c <= 'F' )
                    {
                        colorValue += ( c - 'A' + 10 );
                    }
                    else
                    {
                        throw new ArgumentException( $"Malformed color: {value}" );
                    }
                }

                color = Color.FromArgb( colorValue );
            }
            else
            {
                throw new ArgumentException( $"Malformed color: {value}" );
            }
            return color;
        }

        public override void Write( Utf8JsonWriter writer, Color value, JsonSerializerOptions options )
        {
            var colorString = string.Format( $"{value.ToArgb():x6}" );

            // If using max alpha, trim to a straightforward 6-digit RGB value
            if ( colorString.Length == 8 && colorString.StartsWith( "ff" ) )
            {
                colorString = colorString.Substring( 2 );
            }

            writer.WriteStringValue( "#" + colorString );
        }
    }
}
