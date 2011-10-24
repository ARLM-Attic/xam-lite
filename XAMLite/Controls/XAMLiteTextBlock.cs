using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;
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

        StringBuilder sb;

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        public TextAlignment TextAlignment { get; set; }

        // public FontStyle FontStyle { get; set; } // possibly more spriteFonts preloaded??

        // An idea for establishing a set of possible preloaded SpriteFonts??
        private FontFamily _fontFamily;
        private bool fontFamilyChanged; // used in the Update() method

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; fontFamilyChanged = true; }
        }

        // public FontWeight FontWeight { get; set; }

        public Thickness Padding { get; set; }

        Vector2 paddedPosition;

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

                if ((SolidColorBrush)value == Brushes.Transparent)
                    transparent = true;
                else
                    transparent = false;
            }
        }

        private bool transparent;

        private bool widthSet;
        private bool heightSet;

        private bool textWrappingSet;
        private bool widthHeightContainerSet;

        private bool rotated;

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

        public XAMLiteTextBlock(Game game)
            : base(game)
        {
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            this.spriteFont = courier10SpriteFont;
            this.Padding = new Thickness(0, 0, 0, 0);
            //this.Visible = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBlock(Game game, Run run)
            : base(game)
        {
            this.Text = run.textBlock;
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            this._foregroundColor = Color.Black;
            this._backgroundColor = Color.Transparent;
            this.Padding = new Thickness(0, 0, 0, 0);
            //this.Visible = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="fontName"></param>
        protected override void LoadContent()
        {
            base.LoadContent();

            // triggers whether Width and Height of textblock were set by user
            if (this.Width == 0)
                widthSet = false;
            else
                widthSet = true;
            if (this.Height == 0)
                heightSet = false;
            else
                heightSet = true;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
            }
            if (marginChanged)
            {
                marginChanged = false;
                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
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
                if (TextWrapping == TextWrapping.Wrap && !textWrappingSet)
                {
                    textWrappingSet = true;
                    this.Text = WordWrap(this.Text, (int)this.spriteFont.MeasureString(this.Text).X);
                }

                if (!widthHeightContainerSet)
                {
                    widthHeightContainerSet = true;
                    CalculateWidthAndHeight(this.Text);
                }

                if (Rotate90)
                {
                    if (!rotated)
                    {
                        rotated = true;
                        int tempHeight = this.Height;
                        this.Height = this.Width;
                        this.Width = tempHeight;
                        Padding = new Thickness(Padding.Top, Padding.Left, Padding.Bottom, Padding.Right);
                        CreateTextBlockContainer();
                    }
                    paddedPosition = new Vector2(_panel.X + _panel.Width - (int)Padding.Left, _panel.Y + (int)Padding.Top);
                }
                else
                    paddedPosition = new Vector2(_panel.X + (int)Padding.Left, _panel.Y + (int)Padding.Top);



                spriteBatch.Begin();

                if (!transparent)
                {
                    spriteBatch.Draw(_pixel, _panel, this._backgroundColor);
                }

                if (Rotate90)
                {
                    spriteBatch.DrawString(this.spriteFont, this.Text, paddedPosition, this._foregroundColor, -MathHelper.PiOver2, spriteFont.MeasureString(this.Text), 1, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(this.spriteFont, this.Text, paddedPosition, this._foregroundColor);
                }
                spriteBatch.End();
            }
        }

        // Determines the size of the textblock based on Width, Height.
        protected void CreateTextBlockContainer()
        {
            if (this.Width != 0 && this.Height != 0)
                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        /// <summary>
        /// Set width and height if not already set or adjusting for noWrap
        /// </summary>
        /// <param name="text"></param>
        private void CalculateWidthAndHeight(string text)
        {
            if (!widthSet || TextWrapping == TextWrapping.NoWrap)
                this.Width = (int)this.spriteFont.MeasureString(text).X + (int)Padding.Left + (int)Padding.Right;

            if (!heightSet || TextWrapping == TextWrapping.NoWrap)
                this.Height = (int)this.spriteFont.MeasureString(text).Y + (int)Padding.Top + (int)Padding.Bottom;
            marginChanged = true;
            Console.WriteLine("In here");
        }

        // used to break the string into seperate lines of text
        protected const string _newline = "\r\n";

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
            if (this.Width > width)
                return text;

            // just for clarity
            float strLenPixels = width;
            int numCharsinString = 0;

            // determining total number of characters in the string 
            for (int i = 0; i < text.Length; i++)
                numCharsinString++;

            // Now removing any whitespaces that might be at the end of the string
            while ((numCharsinString - 1) >= 0 && Char.IsWhiteSpace(text[numCharsinString - 1]))
                numCharsinString--;

            // finding number of pixels per character in string length
            float pxPerChar = strLenPixels / numCharsinString;

            // determining max number of characters per line to fit textblock
            int charsPerLine;

            int paddingAdjust = (int)Padding.Left + (int)Padding.Right;
            if (paddingAdjust < this.Width)
                charsPerLine = this.Width / (int)pxPerChar;
            else
                charsPerLine = 1;

            Console.WriteLine(text);
            sb = new StringBuilder();
            int pos, next;
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(_newline, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + _newline.Length;

                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > charsPerLine)
                            len = BreakLine(text, pos, charsPerLine);
                        sb.Append(text, pos, len);

                        // update position
                        pos += len;

                        // "if" statement prevents extra line being added at end of text for drawing the background block
                        if (pos != text.Length)
                            sb.Append(_newline);

                        // Trim whitespace following break
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;

                    } while (eol > pos);
                }
                else sb.Append(_newline); // Empty line
            }

            return sb.ToString();
        }

        public int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;
            if (i < 0)
                return max; // No whitespace found; break at maximum length
            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;
            // Return length of text before whitespace
            return i + 1;
        }

    }

    // for mocking the WPF constructor
    // XAMLiteTextBlock textBlock = new XAMLiteTextBlock(this, new Run("This is text."));
    // this value gets placed into the "string Run" of XAMLiteTextBlock.cs
    public class Run
    {
        public string textBlock { get; set; }

        public Run(string textBlock)
        {
            this.textBlock = textBlock;
        }
    }
}
