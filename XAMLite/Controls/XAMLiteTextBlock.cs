using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// This is the wrappable text chunk, which doesn't support user input.
    /// Here are some projects that might have good references for this contorl:
    /// http://simplegui.codeplex.com/
    /// http://xnagui.codeplex.com/
    /// http://xnatoolgui.codeplex.com/
    /// http://neoforce.codeplex.com/
    /// </summary>
    public class XAMLiteTextBlock : XAMLiteControl
    {
        // for WPF developers, in case they do not load Run in the Constructor.
        // They could then type: textblock.Run
        public string Run
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder _sb;

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private FontFamily _fontFamily;
        
        /// <summary>
        /// 
        /// </summary>
        private bool _fontFamilyChanged; // used in the Update() method

        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
                _fontFamilyChanged = true;
            }
        }

        /// <summary>
        /// character spacing for the font
        /// </summary>
        public int Spacing { get; set; }

        // public FontWeight FontWeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 _paddedPosition;

        /// <summary>
        /// 
        /// </summary>
        private Color _foregroundColor;

        /// <summary>
        /// 
        /// </summary>
        public Brush Foreground
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _foregroundColor = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// 
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                _transparent = value == Brushes.Transparent;
            }
        }

        private bool _transparent;

        private bool _widthSet;

        private bool _heightSet;

        private bool _textWrappingSet;

        private bool _widthHeightContainerSet;

        private bool _rotated;

        //private bool _applyTransform;

        /* A POTENTIAL WISH LIST FOR TEXTBLOCK
         *  TextBlock textBlock = new TextBlock(new Run("A bit of text content..."));

            textBlock.FontFamily              = new FontFamily("Century Gothic");
            textBlock.FontSize                = 12;
            textBlock.FontStretch             = FontStretches.UltraExpanded;
            textBlock.FontStyle               = FontStyles.Italic;
            textBlock.FontWeight              = FontWeights.UltraBold;

            textBlock.LineHeight              = Double.NaN;
            textBlock.Padding                 = new Thickness(5, 10, 5, 10);
            textBlock.TextAlignment           = TextAlignment.Center;
            textBlock.TextWrapping            = TextWrapping.Wrap;

            textBlock.Typography.NumeralStyle = FontNumeralStyle.OldStyle;
            textBlock.Typography.SlashedZero  = true;

         */

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBlock(Game game)
            : base(game)
        {
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            SpriteFont = Courier10SpriteFont;
            Padding = new Thickness(0, 0, 0, 0);
            Spacing = 2;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="run"> </param>
        public XAMLiteTextBlock(Game game, Run run)
            : base(game)
        {
            Text = run.TextBlock;
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            Padding = new Thickness(0, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // triggers whether Width and Height of textblock were set by user
            _widthSet = Width != 0;
            _heightSet = Height != 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_fontFamilyChanged)
            {
                _fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                SpriteFont.Spacing = Spacing;
                RecalculateWidthAndHeight(Text);
            }

            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                if (TextWrapping == TextWrapping.Wrap && !_textWrappingSet)
                {
                    _textWrappingSet = true;
                    Text = WordWrap(Text, (int)SpriteFont.MeasureString(Text).X);
                }

                if (!_widthHeightContainerSet)
                {
                    _widthHeightContainerSet = true;
                    CalculateWidthAndHeight(Text);
                }

                if (Rotate90)
                {
                    if (!_rotated)
                    {
                        _rotated = true;
                        var tempHeight = Height;
                        Height = Width;
                        Width = tempHeight;
                        Padding = new Thickness(Padding.Top, Padding.Left, Padding.Bottom, Padding.Right);
                        CreateTextBlockContainer();
                    }

                    _paddedPosition = new Vector2(Panel.X + Panel.Width - (int)Padding.Left, Panel.Y + (int)Padding.Top);
                }
                else
                {
                    _paddedPosition = new Vector2(Panel.X + (int)Padding.Left, Panel.Y + (int)Padding.Top);
                }

                SpriteBatch.Begin();

                SpriteFont.Spacing = Spacing;

                if (!_transparent)
                {
                    SpriteBatch.Draw(Pixel, Panel, _backgroundColor * (float)Opacity);
                }

                if (Rotate90)
                {
                    SpriteBatch.DrawString(SpriteFont, Text, _paddedPosition, _foregroundColor * (float)Opacity, -MathHelper.PiOver2, SpriteFont.MeasureString(Text), 1, SpriteEffects.None, 0);
                }
                else
                {
                    SpriteBatch.DrawString(SpriteFont, Text, _paddedPosition, _foregroundColor * (float)Opacity);
                }

                SpriteBatch.End();
            }
        }

        // Determines the size of the textblock based on Width, Height.
        protected void CreateTextBlockContainer()
        {
            if (Width != 0 && Height != 0)
            {
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// Set width and height if not already set or adjusting for noWrap
        /// </summary>
        /// <param name="text"></param>
        private void CalculateWidthAndHeight(string text)
        {
            if (!_widthSet || TextWrapping == TextWrapping.NoWrap)
            {
                Width = (int)SpriteFont.MeasureString(text).X + (int)Padding.Left + (int)Padding.Right;
            }

            if (!_heightSet || TextWrapping == TextWrapping.NoWrap)
            {
                Height = (int)SpriteFont.MeasureString(text).Y + (int)Padding.Top + (int)Padding.Bottom;
            }

            MarginChanged = true;
        }

        // used to break the string into seperate lines of text
        protected const string Newline = "\r\n";

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
                var eol = text.IndexOf(Newline, pos, System.StringComparison.Ordinal);
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

    // for mocking the WPF constructor
    // XAMLiteTextBlock textBlock = new XAMLiteTextBlock(this, new Run("This is text."));
    // this value gets placed into the "string Run" of XAMLiteTextBlock.cs
    public class Run
    {
        public string TextBlock { get; set; }

        public Run(string textBlock)
        {
            TextBlock = textBlock;
        }
    }
}
