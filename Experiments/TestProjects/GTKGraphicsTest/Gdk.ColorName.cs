using System;
using GLib;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

//------------------------------------------------
// Modified from https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/Drawing/Color.cs
// 
// Note that the original System.Drawing.Color was made by the team at Microsoft.
// 
// This was modified to allow color names to work with the GTK UI.
//

namespace Gdk
{
    public readonly struct ColorName : IEquatable<ColorName>
    {
        public static readonly ColorName Empty;

        // -------------------------------------------------------------------
        //  static list of "web" colors...
        //
        public static RGBA Transparent => new RGBA()
        {
            Red = 255,
            Green = 255,
            Blue = 255,
            Alpha = 0
        };

        public static RGBA AliceBlue => new RGBA()
        {
            Red = 240,
            Green = 248,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA AntiqueWhite => new RGBA()
        {
            Red = 250,
            Green = 235,
            Blue = 215
        };

        public static RGBA Aqua => new RGBA()
        {
            Red = 0,
            Green = 255,
            Blue = 255
        };

        public static RGBA Aquamarine => new RGBA()
        {
            Red = 127,
            Green = 255,
            Blue = 212,
            Alpha = 255
        };

        public static RGBA Azure => new RGBA()
        {
            Red = 240,
            Green = 255,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA Beige => new RGBA()
        {
            Red = 245,
            Green = 245,
            Blue = 220,
            Alpha = 255
        };

        public static RGBA Bisque => new RGBA()
        {
            Red = 255,
            Green = 228,
            Blue = 196,
            Alpha = 255
        };

        public static RGBA Black => new RGBA()
        {
            Red = 0,
            Green = 0,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA BlanchedAlmond => new RGBA()
        {
            Red = 255,
            Green = 235,
            Blue = 205,
            Alpha = 255
        };

        public static RGBA Blue => new RGBA()
        {
            Red = 0,
            Green = 0,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA BlueViolet => new RGBA()
        {
            Red = 138,
            Green = 43,
            Blue = 226,
            Alpha = 255
        };

        public static RGBA Brown => new RGBA()
        {
            Red = 165,
            Green = 42,
            Blue = 42,
            Alpha = 255
        };

        public static RGBA BurlyWood => new RGBA()
        {
            Red = 222,
            Green = 184,
            Blue = 135,
            Alpha = 255
        };

        public static RGBA CadetBlue => new RGBA()
        {
            Red = 95,
            Green = 158,
            Blue = 160,
            Alpha = 255
        };

        public static RGBA Chartreuse => new RGBA()
        {
            Red = 127,
            Green = 255,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA Chocolate => new RGBA()
        {
            Red = 210,
            Green = 105,
            Blue = 30,
            Alpha = 255
        };

        public static RGBA Coral => new RGBA()
        {
            Red = 255,
            Green = 127,
            Blue = 80,
            Alpha = 255
        };

        public static RGBA CornflowerBlue => new RGBA()
        {
            Red = 100,
            Green = 149,
            Blue = 237,
            Alpha = 255
        };

        public static RGBA Cornsilk => new RGBA()
        {
            Red = 255,
            Green = 248,
            Blue = 220,
            Alpha = 255
        };

        public static RGBA Crimson => new RGBA()
        {
            Red = 220,
            Green = 20,
            Blue = 60,
            Alpha = 255
        };

        public static RGBA Cyan => new RGBA()
        {
            Red = 0,
            Green = 255,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA DarkBlue => new RGBA()
        {
            Red = 0,
            Green = 0,
            Blue = 139,
            Alpha = 255
        };

        public static RGBA DarkCyan => new RGBA()
        {
            Red = 0,
            Green = 139,
            Blue = 139,
            Alpha = 255
        };

        public static RGBA DarkGoldenrod => new RGBA()
        {
            Red = 184,
            Green = 134,
            Blue = 11,
            Alpha = 255
        };

        public static RGBA DarkGray => new RGBA()
        {
            Red = 169,
            Green = 169,
            Blue = 169,
            Alpha = 255
        };

        public static RGBA DarkGreen => new RGBA()
        {
            Red = 0,
            Green = 100,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA DarkKhaki => new RGBA()
        {
            Red = 189,
            Green = 183,
            Blue = 107,
            Alpha = 255
        };

        public static RGBA DarkMagenta => new RGBA()
        {
            Red = 139,
            Green = 0,
            Blue = 139,
            Alpha = 255
        };

        public static RGBA DarkOliveGreen => new RGBA()
        {
            Red = 85,
            Green = 107,
            Blue = 47,
            Alpha = 255
        };

        public static RGBA DarkOrange => new RGBA()
        {
            Red = 255,
            Green = 140,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA DarkOrchid => new RGBA()
        {
            Red = 153,
            Green = 50,
            Blue = 204,
            Alpha = 255
        };

        public static RGBA DarkRed => new RGBA()
        {
            Red = 139,
            Green = 0,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA DarkSalmon => new RGBA()
        {
            Red = 233,
            Green = 150,
            Blue = 122,
            Alpha = 255
        };

        public static RGBA DarkSeaGreen => new RGBA()
        {
            Red = 143,
            Green = 188,
            Blue = 143,
            Alpha = 255
        };

        public static RGBA DarkSlateBlue => new RGBA()
        {
            Red = 72,
            Green = 61,
            Blue = 139,
            Alpha = 255
        };

        public static RGBA DarkSlateGray => new RGBA()
        {
            Red = 47,
            Green = 79,
            Blue = 79,
            Alpha = 255
        };

        public static RGBA DarkTurquoise => new RGBA()
        {
            Red = 0,
            Green = 206,
            Blue = 209,
            Alpha = 255
        };

        public static RGBA DarkViolet => new RGBA()
        {
            Red = 148,
            Green = 0,
            Blue = 211,
            Alpha = 255
        };

        public static RGBA DeepPink => new RGBA()
        {
            Red = 255,
            Green = 20,
            Blue = 147,
            Alpha = 255
        };

        public static RGBA DeepSkyBlue => new RGBA()
        {
            Red = 0,
            Green = 191,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA DimGray => new RGBA()
        {
            Red = 105,
            Green = 105,
            Blue = 105,
            Alpha = 255
        };

        public static RGBA DodgerBlue => new RGBA()
        {
            Red = 30,
            Green = 144,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA Firebrick => new RGBA()
        {
            Red = 178,
            Green = 34,
            Blue = 34,
            Alpha = 255
        };

        public static RGBA FloralWhite => new RGBA()
        {
            Red = 255,
            Green = 250,
            Blue = 240,
            Alpha = 255
        };

        public static RGBA ForestGreen => new RGBA()
        {
            Red = 34,
            Green = 139,
            Blue = 34,
            Alpha = 255
        };

        public static RGBA Fuchsia => new RGBA()
        {
            Red = 255,
            Green = 0,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA Gainsboro => new RGBA()
        {
            Red = 220,
            Green = 220,
            Blue = 220,
            Alpha = 255
        };

        public static RGBA GhostWhite => new RGBA()
        {
            Red = 248,
            Green = 248,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA Gold => new RGBA()
        {
            Red = 255,
            Green = 215,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA Goldenrod => new RGBA()
        {
            Red = 218,
            Green = 165,
            Blue = 32,
            Alpha = 255
        };

        public static RGBA Gray => new RGBA()
        {
            Red = 128,
            Green = 128,
            Blue = 128,
            Alpha = 255
        };

        public static RGBA Green => new RGBA()
        {
            Red = 0,
            Green = 128,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA GreenYellow => new RGBA()
        {
            Red = 173,
            Green = 255,
            Blue = 47,
            Alpha = 255
        };

        public static RGBA Honeydew => new RGBA()
        {
            Red = 240,
            Green = 255,
            Blue = 240,
            Alpha = 255
        };

        public static RGBA HotPink => new RGBA()
        {
            Red = 255,
            Green = 105,
            Blue = 180,
            Alpha = 255
        };

        public static RGBA IndianRed => new RGBA()
        {
            Red = 205,
            Green = 92,
            Blue = 92,
            Alpha = 255
        };

        public static RGBA Indigo => new RGBA()
        {
            Red = 75,
            Green = 0,
            Blue = 130,
            Alpha = 255
        };

        public static RGBA Ivory => new RGBA()
        {
            Red = 255,
            Green = 255,
            Blue = 240,
            Alpha = 255
        };

        public static RGBA Khaki => new RGBA()
        {
            Red = 240,
            Green = 230,
            Blue = 140,
            Alpha = 255
        };

        public static RGBA Lavender => new RGBA()
        {
            Red = 230,
            Green = 230,
            Blue = 230,
            Alpha = 255
        };

        public static RGBA LavenderBlush => new RGBA()
        {
            Red = 255,
            Green = 240,
            Blue = 245,
            Alpha = 255
        };

        public static RGBA LawnGreen => new RGBA()
        {
            Red = 124,
            Green = 252,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA LemonChiffon => new RGBA()
        {
            Red = 255,
            Green = 250,
            Blue = 205,
            Alpha = 255
        };

        public static RGBA LightBlue => new RGBA()
        {
            Red = 173,
            Green = 216,
            Blue = 230,
            Alpha = 255
        };

        public static RGBA LightCoral => new RGBA()
        {
            Red = 240,
            Green = 128,
            Blue = 128,
            Alpha = 255
        };

        public static RGBA LightCyan => new RGBA()
        {
            Red = 224,
            Green = 255,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA LightGoldenrodYellow => new RGBA()
        {
            Red = 250,
            Green = 250,
            Blue = 210,
            Alpha = 255
        };

        public static RGBA LightGreen => new RGBA()
        {
            Red = 211,
            Green = 211,
            Blue = 211,
            Alpha = 255
        };

        public static RGBA LightGray => new RGBA()
        {
            Red = 144,
            Green = 238,
            Blue = 144,
            Alpha = 255
        };

        public static RGBA LightPink => new RGBA()
        {
            Red = 255,
            Green = 182,
            Blue = 193,
            Alpha = 255
        };

        public static RGBA LightSalmon => new RGBA()
        {
            Red = 255,
            Green = 160,
            Blue = 122,
            Alpha = 255
        };

        public static RGBA LightSeaGreen => new RGBA()
        {
            Red = 32,
            Green = 178,
            Blue = 170,
            Alpha = 255
        };

        public static RGBA LightSkyBlue => new RGBA()
        {
            Red = 135,
            Green = 206,
            Blue = 250,
            Alpha = 255
        };

        public static RGBA LightSlateGray => new RGBA()
        {
            Red = 119,
            Green = 136,
            Blue = 153,
            Alpha = 255
        };

        public static RGBA LightSteelBlue => new RGBA()
        {
            Red = 176,
            Green = 196,
            Blue = 222,
            Alpha = 255
        };

        public static RGBA LightYellow => new RGBA()
        {
            Red = 255,
            Green = 255,
            Blue = 224,
            Alpha = 255
        };

        public static RGBA Lime => new RGBA()
        {
            Red = 0,
            Green = 255,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA LimeGreen => new RGBA()
        {
            Red = 50,
            Green = 205,
            Blue = 50,
            Alpha = 255
        };

        public static RGBA Linen => new RGBA()
        {
            Red = 250,
            Green = 240,
            Blue = 230,
            Alpha = 255
        };

        public static RGBA Magenta => new RGBA()
        {
            Red = 255,
            Green = 0,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA Maroon => new RGBA()
        {
            Red = 128,
            Green = 0,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA MediumAquamarine => new RGBA()
        {
            Red = 102,
            Green = 205,
            Blue = 170,
            Alpha = 255
        };

        public static RGBA MediumBlue => new RGBA()
        {
            Red = 0,
            Green = 0,
            Blue = 205,
            Alpha = 255
        };

        public static RGBA MediumOrchid => new RGBA()
        {
            Red = 186,
            Green = 85,
            Blue = 211,
            Alpha = 255
        };

        public static RGBA MediumPurple => new RGBA()
        {
            Red = 147,
            Green = 112,
            Blue = 219,
            Alpha = 255
        };

        public static RGBA MediumSeaGreen => new RGBA()
        {
            Red = 60,
            Green = 179,
            Blue = 113,
            Alpha = 255
        };

        public static RGBA MediumSlateBlue => new RGBA()
        {
            Red = 123,
            Green = 104,
            Blue = 238,
            Alpha = 255
        };

        public static RGBA MediumSpringGreen => new RGBA()
        {
            Red = 0,
            Green = 250,
            Blue = 154,
            Alpha = 255
        };

        public static RGBA MediumTurquoise => new RGBA()
        {
            Red = 72,
            Green = 209,
            Blue = 204,
            Alpha = 255
        };

        public static RGBA MediumVioletRed => new RGBA()
        {
            Red = 199,
            Green = 21,
            Blue = 133,
            Alpha = 255
        };

        public static RGBA MidnightBlue => new RGBA()
        {
            Red = 25,
            Green = 25,
            Blue = 112,
            Alpha = 255
        };

        public static RGBA MintCream => new RGBA()
        {
            Red = 245,
            Green = 255,
            Blue = 250,
            Alpha = 255
        };

        public static RGBA MistyRose => new RGBA()
        {
            Red = 255,
            Green = 228,
            Blue = 225,
            Alpha = 255
        };

        public static RGBA Moccasin => new RGBA()
        {
            Red = 255,
            Green = 228,
            Blue = 181,
            Alpha = 255
        };

        public static RGBA NavajoWhite => new RGBA()
        {
            Red = 255,
            Green = 222,
            Blue = 173,
            Alpha = 255
        };

        public static RGBA Navy => new RGBA()
        {
            Red = 0,
            Green = 0,
            Blue = 128,
            Alpha = 255
        };

        public static RGBA OldLace => new RGBA()
        {
            Red = 253,
            Green = 245,
            Blue = 230,
            Alpha = 255
        };

        public static RGBA Olive => new RGBA()
        {
            Red = 128,
            Green = 128,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA OliveDrab => new RGBA()
        {
            Red = 107,
            Green = 142,
            Blue = 35,
            Alpha = 255
        };

        public static RGBA Orange => new RGBA()
        {
            Red = 255,
            Green = 165,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA OrangeRed => new RGBA()
        {
            Red = 255,
            Green = 69,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA Orchid => new RGBA()
        {
            Red = 218,
            Green = 112,
            Blue = 214,
            Alpha = 255
        };

        public static RGBA PaleGoldenrod => new RGBA()
        {
            Red = 238,
            Green = 232,
            Blue = 170,
            Alpha = 255
        };

        public static RGBA PaleGreen => new RGBA()
        {
            Red = 152,
            Green = 251,
            Blue = 152,
            Alpha = 255
        };

        public static RGBA PaleTurquoise => new RGBA()
        {
            Red = 175,
            Green = 238,
            Blue = 238,
            Alpha = 255
        };

        public static RGBA PaleVioletRed => new RGBA()
        {
            Red = 219,
            Green = 112,
            Blue = 147,
            Alpha = 255
        };

        public static RGBA PapayaWhip => new RGBA()
        {
            Red = 255,
            Green = 239,
            Blue = 213,
            Alpha = 255
        };

        public static RGBA PeachPuff => new RGBA()
        {
            Red = 255,
            Green = 218,
            Blue = 185,
            Alpha = 255
        };

        public static RGBA Peru => new RGBA()
        {
            Red = 205,
            Green = 133,
            Blue = 63,
            Alpha = 255
        };

        public static RGBA Pink => new RGBA()
        {
            Red = 255,
            Green = 192,
            Blue = 203,
            Alpha = 255
        };

        public static RGBA Plum => new RGBA()
        {
            Red = 221,
            Green = 160,
            Blue = 221,
            Alpha = 255
        };

        public static RGBA PowderBlue => new RGBA()
        {
            Red = 176,
            Green = 224,
            Blue = 230,
            Alpha = 255
        };

        public static RGBA Purple => new RGBA()
        {
            Red = 128,
            Green = 0,
            Blue = 128,
            Alpha = 255
        };

        /// <summary>
        /// Gets a system-defined RGBA that has an ARGB value of <c>#663399</c>.
        /// </summary>
        /// <value>A system-defined RGBA.</value>
        public static RGBA RebeccaPurple => new RGBA()
        {
            Red = 102,
            Green = 51,
            Blue = 153,
            Alpha = 255
        };

        public static RGBA Red => new RGBA()
        {
            Red = 255,
            Green = 0,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA RosyBrown => new RGBA()
        {
            Red = 188,
            Green = 143,
            Blue = 143,
            Alpha = 255
        };

        public static RGBA RoyalBlue => new RGBA()
        {
            Red = 65,
            Green = 105,
            Blue = 225,
            Alpha = 255
        };

        public static RGBA SaddleBrown => new RGBA()
        {
            Red = 139,
            Green = 69,
            Blue = 19,
            Alpha = 255
        };

        public static RGBA Salmon => new RGBA()
        {
            Red = 250,
            Green = 128,
            Blue = 114,
            Alpha = 255
        };

        public static RGBA SandyBrown => new RGBA()
        {
            Red = 244,
            Green = 164,
            Blue = 96,
            Alpha = 255
        };

        public static RGBA SeaGreen => new RGBA()
        {
            Red = 46,
            Green = 139,
            Blue = 87,
            Alpha = 255
        };

        public static RGBA SeaShell => new RGBA()
        {
            Red = 255,
            Green = 245,
            Blue = 238,
            Alpha = 255
        };

        public static RGBA Sienna => new RGBA()
        {
            Red = 160,
            Green = 82,
            Blue = 45,
            Alpha = 255
        };

        public static RGBA Silver => new RGBA()
        {
            Red = 192,
            Green = 192,
            Blue = 192,
            Alpha = 255
        };

        public static RGBA SkyBlue => new RGBA()
        {
            Red = 135,
            Green = 206,
            Blue = 235,
            Alpha = 255
        };

        public static RGBA SlateBlue => new RGBA()
        {
            Red = 106,
            Green = 90,
            Blue = 205,
            Alpha = 255
        };

        public static RGBA SlateGray => new RGBA()
        {
            Red = 112,
            Green = 128,
            Blue = 144,
            Alpha = 255
        };

        public static RGBA Snow => new RGBA()
        {
            Red = 255,
            Green = 250,
            Blue = 250,
            Alpha = 255
        };

        public static RGBA SpringGreen => new RGBA()
        {
            Red = 0,
            Green = 255,
            Blue = 127,
            Alpha = 255
        };

        public static RGBA SteelBlue => new RGBA()
        {
            Red = 70,
            Green = 130,
            Blue = 180,
            Alpha = 255
        };

        public static RGBA Tan => new RGBA()
        {
            Red = 210,
            Green = 180,
            Blue = 140,
            Alpha = 255
        };

        public static RGBA Teal => new RGBA()
        {
            Red = 0,
            Green = 128,
            Blue = 128,
            Alpha = 255
        };

        public static RGBA Thistle => new RGBA()
        {
            Red = 216,
            Green = 191,
            Blue = 216,
            Alpha = 255
        };

        public static RGBA Tomato => new RGBA()
        {
            Red = 255,
            Green = 99,
            Blue = 71,
            Alpha = 255
        };

        public static RGBA Turquoise => new RGBA()
        {
            Red = 64,
            Green = 224,
            Blue = 208,
            Alpha = 255
        };

        public static RGBA Violet => new RGBA()
        {
            Red = 238,
            Green = 130,
            Blue = 238,
            Alpha = 255
        };

        public static RGBA Wheat => new RGBA()
        {
            Red = 245,
            Green = 222,
            Blue = 179,
            Alpha = 255
        };

        public static RGBA White => new RGBA()
        {
            Red = 255,
            Green = 255,
            Blue = 255,
            Alpha = 255
        };

        public static RGBA WhiteSmoke => new RGBA()
        {
            Red = 245,
            Green = 245,
            Blue = 245,
            Alpha = 255
        };

        public static RGBA Yellow => new RGBA()
        {
            Red = 255,
            Green = 255,
            Blue = 0,
            Alpha = 255
        };

        public static RGBA YellowGreen => new RGBA()
        {
            Red = 154,
            Green = 205,
            Blue = 50,
            Alpha = 255
        };
        //
        //  end "web" colors
        // -------------------------------------------------------------------

        // NOTE : The "zero" pattern (all members being 0) must represent
        //      : "not set". This allows "ColorName c;" to be correct.

        private const short StateKnownColorValid = 0x0001;
        private const short StateARGBValueValid = 0x0002;
        private const short StateValueMask = StateARGBValueValid;
        private const short StateNameValid = 0x0008;
        private const long NotDefinedValue = 0;

        // Shift counts and bit masks for A, R, G, B components in ARGB mode

        internal const int ARGBAlphaShift = 24;
        internal const int ARGBRedShift = 16;
        internal const int ARGBGreenShift = 8;
        internal const int ARGBBlueShift = 0;
        internal const uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;
        internal const uint ARGBRedMask = 0xFFu << ARGBRedShift;
        internal const uint ARGBGreenMask = 0xFFu << ARGBGreenShift;
        internal const uint ARGBBlueMask = 0xFFu << ARGBBlueShift;

        // User supplied name of color. Will not be filled in if
        // we map to a "knowncolor"
        private readonly string? name; // Do not rename (binary serialization)

        // Standard 32bit sRGB (ARGB)
        private readonly long value; // Do not rename (binary serialization)

        // Ignored, unless "state" says it is valid
        private readonly short knownColor; // Do not rename (binary serialization)

        // State flags.
        private readonly short state; // Do not rename (binary serialization)

        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        public bool IsKnownColor => (state & StateKnownColorValid) != 0;

        public bool IsEmpty => state == 0;

        public bool IsNamedColor => ((state & StateNameValid) != 0) || IsKnownColor;

        // Used for the [DebuggerDisplay]. Inlining in the attribute is possible, but
        // against best practices as the current project language parses the string with
        // language specific heuristics.

        private string NameAndARGBValue => $"{{Name={Name}, ARGB=({A}, {R}, {G}, {B})}}";

        public string Name
        {
            get
            {
                if ((state & StateNameValid) != 0)
                {
                    Debug.Assert(name != null);
                    return name;
                }

                // if we reached here, just encode the value
                //
                return value.ToString("x");
            }
        }

        private long Value
        {
            get
            {
                if ((state & StateValueMask) != 0)
                {
                    return value;
                }

                return NotDefinedValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetRgbValues(out int r, out int g, out int b)
        {
            uint value = (uint)Value;
            r = (int)(value & ARGBRedMask) >> ARGBRedShift;
            g = (int)(value & ARGBGreenMask) >> ARGBGreenShift;
            b = (int)(value & ARGBBlueMask) >> ARGBBlueShift;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MinMaxRgb(out int min, out int max, int r, int g, int b)
        {
            if (r > g)
            {
                max = r;
                min = g;
            }
            else
            {
                max = g;
                min = r;
            }
            if (b > max)
            {
                max = b;
            }
            else if (b < min)
            {
                min = b;
            }
        }

        public float GetBrightness()
        {
            GetRgbValues(out int r, out int g, out int b);

            MinMaxRgb(out int min, out int max, r, g, b);

            return (max + min) / (byte.MaxValue * 2f);
        }

        public float GetHue()
        {
            GetRgbValues(out int r, out int g, out int b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out int min, out int max, r, g, b);

            float delta = max - min;
            float hue;

            if (r == max)
                hue = (g - b) / delta;
            else if (g == max)
                hue = (b - r) / delta + 2f;
            else
                hue = (r - g) / delta + 4f;

            hue *= 60f;
            if (hue < 0f)
                hue += 360f;

            return hue;
        }

        public float GetSaturation()
        {
            GetRgbValues(out int r, out int g, out int b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out int min, out int max, r, g, b);

            int div = max + min;
            if (div > byte.MaxValue)
                div = byte.MaxValue * 2 - max - min;

            return (max - min) / (float)div;
        }

        public int ToArgb() => unchecked((int)Value);

        public override string ToString() =>
            IsNamedColor ? $"{nameof(ColorName)} [{Name}]" :
            (state & StateValueMask) != 0 ? $"{nameof(ColorName)} [A={A}, R={R}, G={G}, B={B}]" :
            $"{nameof(ColorName)} [Empty]";

        public static bool operator ==(ColorName left, ColorName right) =>
            left.value == right.value
                && left.state == right.state
                && left.knownColor == right.knownColor
                && left.name == right.name;

        public static bool operator !=(ColorName left, ColorName right) => !(left == right);

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is ColorName other && Equals(other);

        public bool Equals(ColorName other) => this == other;

        public override int GetHashCode()
        {
            // Three cases:
            // 1. We don't have a name. All relevant data, including this fact, is in the remaining fields.
            // 2. We have a known name. The name will be the same instance of any other with the same
            // knownColor value, so we can ignore it for hashing. Note this also hashes different to
            // an unnamed color with the same ARGB value.
            // 3. Have an unknown name. Will differ from other unknown-named colors only by name, so we
            // can usefully use the names hash code alone.
            if (name != null && !IsKnownColor)
                return name.GetHashCode();

            return HashCode.Combine(value.GetHashCode(), state.GetHashCode(), knownColor.GetHashCode());
        }
    }
}
