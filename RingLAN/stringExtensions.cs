using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Extensions {
    /// <summary>
    /// Provides additional support for string extension methods for various purposes
    /// </summary>
    public static class stringExtensions {
        /// <summary>
        /// Returns the width of a string rendered in a particular font
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="withFont">The font to render with</param>
        /// <returns>The width in pixels</returns>
        public static int Width(this string str, Font withFont) {
            int width = TextRenderer.MeasureText(str, withFont).Width;
            return width;
        }

        /// <summary>
        /// Returns the height of a string rendered in a particular font
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="withFont">The font to render with</param>
        /// <returns>The height in pixels</returns>
        public static int Height(this string str, Font withFont) {
            int height = TextRenderer.MeasureText(str, withFont).Height;
            return height;
        }

        /// <summary>
        /// Shorthand for String.Format
        /// </summary>
        /// <param name="str">The string to format</param>
        /// <param name="formatWith">The parameters to format with</param>
        /// <returns>A formatted string</returns>
        public static string With(this string str, params object[] formatWith) {
            return String.Format(str, formatWith);
        }
    }
}