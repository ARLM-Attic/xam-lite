using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace XAMLite
{
    /// <summary>
    /// Provides methods for wrapping text.
    /// </summary>
    public static class WordWrapper
    {
        // used to break the string into seperate lines of text
        private const string Newline = "\n";

        /// <summary>
        /// Used to build a string for the text block with formatting such as
        /// characters per line.
        /// </summary>
        private static StringBuilder _sb;

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="controlWidth"> Width, in pixels, of the control that the string must be fit into.</param>
        /// <param name="stringWidth"> Initial Width of the string that must be wrapped into the control.</param>
        /// <param name="padding"> </param>
        /// <returns>The modified text</returns>
        public static string Wrap(string text, int controlWidth, int stringWidth, Thickness padding)
        {
            // return if string length is less than width of textblock
            if (controlWidth > stringWidth)
            {
                return text;
            }

            // just for clarity
            float strLenPixels = stringWidth;
            var numCharsinString = 0;

            // determining total number of characters in the string 
            for (var i = 0; i < text.Length; i++)
            {
                numCharsinString++;
            }

            // Now removing any whitespaces that might be at the end of the string
            while ((numCharsinString - 1) >= 0 && char.IsWhiteSpace(text[numCharsinString - 1]))
            {
                numCharsinString--;
            }

            // finding number of pixels per character in string length
            var pxPerChar = strLenPixels / numCharsinString;
            //Console.WriteLine("Pixels per character: " + pxPerChar);

            // determining max number of characters per line to fit textblock
            float charsPerLine;

            var paddingAdjust = (int)padding.Left + (int)padding.Right;

            if (paddingAdjust < controlWidth)
            {
                charsPerLine = ((controlWidth - paddingAdjust) / pxPerChar);
            }
            else
            {
                charsPerLine = 1;
            }

            //Console.WriteLine("Characters per line: " + charsPerLine);

            _sb = new StringBuilder();
            int pos, next;
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Newline, pos, StringComparison.Ordinal);
                //Console.WriteLine("EOL: " + (eol - pos));
                if (eol == -1)
                {
                    next = eol = text.Length;
                }
                else
                {
                    next = eol + Newline.Length;
                }

                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;
                        if (len > charsPerLine)
                        {
                            len = BreakLine(text, pos, (int)charsPerLine);
                        }

                        _sb.Append(text, pos, len);

                        // update position
                        pos += len;

                        // "if" statement prevents extra line being added at end of text for drawing the background block
                        if (pos != text.Length)
                        {
                            _sb.Append(Newline);
                        }

                        // Trim whitespace following break
                        while (pos < eol && char.IsWhiteSpace(text[pos]))
                        {
                            pos++;
                        }
                    }
                    while (eol > pos);
                }
                else
                {
                    _sb.Append(Newline);
                }
            }

            return _sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max - 1;
            while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            if (i < 0)
            {
                return max; // No whitespace found; break at maximum length
            }
            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            // Return length of text before whitespace
            return i + 1;
        }
    }
}
