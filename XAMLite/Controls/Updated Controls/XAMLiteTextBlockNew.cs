using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// This is the wrappable text chunk, which doesn't support user input.
    /// Here are some projects that might have good references for this control:
    /// http://simplegui.codeplex.com/
    /// http://xnagui.codeplex.com/
    /// http://xnatoolgui.codeplex.com/
    /// http://neoforce.codeplex.com/
    /// </summary>
    public class XAMLiteTextBlockNew : XAMLiteGridNew
    {
        // for WPF developers, in case they do not load Run in the Constructor.
        // They could then type: textblock.Run
        // I do not see that a Run is still included in WPF.
        public string Run
        {
            get { return Text; }
            set { Text = value; }
        }

        private string _text;

        /// <summary>
        /// Sets the content of the label within the text box.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;

                if (_textLabel != null)
                {
                    _textLabel.Content = value;
                }
            }
        }

        /// <summary>
        /// Used to build a string for the text block with formatting such as
        /// characters per line.
        /// </summary>
        private StringBuilder _sb;

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// Specifies the alignment of the text.
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        ///// <summary>
        ///// True when the font family has changed.
        ///// </summary>
        //private bool _fontFamilyChanged;

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
                //_fontFamilyChanged = true;
            }
        }

        /// <summary>
        /// Character spacing for the font
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Amount of space between the text and the edge of the background 
        /// that contains it.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// The color of the content, whether text or some other object.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// The text contained in the text box.
        /// </summary>
        private XAMLiteLabelNew _textLabel;

        /// <summary>
        /// Background fo the text block.
        /// </summary>
        private XAMLiteRectangleNew _background;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBlockNew(Game game)
            : base(game)
        {
            Text = "TextBlock";
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            FontFamily = new FontFamily("Verdana12"); 
            Foreground = Brushes.Black;
            Padding = new Thickness(5, 2, 5, 5);
            Spacing = 2;
            Width = 50;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="run"> </param>
        public XAMLiteTextBlockNew(Game game, Run run)
            : base(game)
        {
            Text = run.TextBlock; 
            FontFamily = new FontFamily("Verdana12");
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            Foreground = Brushes.Black;
            Padding = new Thickness(5, 2, 5, 5);
            Spacing = 2;
            Width = 50;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _textLabel = new XAMLiteLabelNew(Game)
            {
                Content = Text,
                HorizontalAlignment = TextAlignment == TextAlignment.Left
                    || TextAlignment == TextAlignment.Justify ?
                    HorizontalAlignment.Left : TextAlignment == TextAlignment.Center ?
                    HorizontalAlignment.Center : HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = FontFamily,
                Spacing = Spacing,
                Foreground = Foreground,
                Padding = Padding,
                Width = Width
            };

            if (Text != string.Empty || _textLabel.Width == 0)
            {
                // Add to the game so that it has a measureable size.
                Game.Components.Add(_textLabel);
                UpdateForTextWrapping();
            }

            UpdateWidthAndHeight();

            if (Game.Components.Contains(_textLabel))
            {
                Game.Components.Remove(_textLabel);
            }

            _background = new XAMLiteRectangleNew(Game)
                {
                    Fill = Background,
                    Width = Width,
                    Height = Height,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
            Children.Add(_background);

            Children.Add(_textLabel);
        }

        /// <summary>
        /// Updates the Width and Height of the control.
        /// </summary>
        private void UpdateWidthAndHeight()
        {
            var w = (int)_textLabel.MeasureString().X + (int)_textLabel.Padding.Left + (int)_textLabel.Padding.Right;
            Console.WriteLine("w: " + w);
            Console.WriteLine("Width: " + Width);
            Width = Width < w ? w : Width;

            Console.WriteLine("Final Width: " + Width);

            var h = (int)_textLabel.MeasureString().Y + (int)_textLabel.Padding.Top + (int)_textLabel.Padding.Bottom;
            Height = Height < h ? h : Height;
        }

        /// <summary>
        /// Word wraps the text when TextWrapping = Wrap.
        /// </summary>
        private void UpdateForTextWrapping()
        {
            // 1.  if no word wrapping, adjust the width and height of the control to match the text label dimensions.
            // 2.  if word wrapping, wrap the text and create the label.
            // 3.  fit the label and modify the whole control as necessary.
            if (TextWrapping == TextWrapping.Wrap)
            {
                Text = WordWrap(Text, (int)_textLabel.MeasureString().X);
            }
        }

        // used to break the string into seperate lines of text
        protected const string Newline = "\n";

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in pixels, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        /// 
        public string WordWrap(string text, int width)
        {
            // return if string length is less than width of textblock
            if (Width > width)
            {
                return text;
            }

            // just for clarity
            float strLenPixels = width;
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
            float pxPerChar = strLenPixels / numCharsinString;

            // determining max number of characters per line to fit textblock
            int charsPerLine;

            var paddingAdjust = (int)Padding.Left + (int)Padding.Right;

            if (paddingAdjust < Width)
            {
                charsPerLine = (int)(Width / pxPerChar);
            }
            else
            {
                charsPerLine = 1;
            }

            _sb = new StringBuilder();
            int pos, next;
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Newline, pos, StringComparison.Ordinal);
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
                            len = BreakLine(text, pos, charsPerLine);
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

        public int BreakLine(string text, int pos, int max)
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
