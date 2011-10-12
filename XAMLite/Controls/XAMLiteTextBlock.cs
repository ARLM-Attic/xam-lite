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
        // spritefont is toggled to whichever font the developer chooses
        protected SpriteFont spriteFont { get; set; }

        // the set of possible fonts that are preloaded in LoadContent()
        protected SpriteFont arialSpriteFont { get; set; }
        protected SpriteFont courier10SpriteFont { get; set; }

        private Rectangle textBlockContainer;

        private Texture2D _pixel;

        // for WPF developers, in case they do not load Run in the Constructor.
        // They could then type: textblock.Run
        public string Run 
        { 
            get { return base.Text; } 
            set {
                    /*if (this.spriteFont != null)
                    {
                        CalculateWidthAndHeight(value);
                    }*/
                    base.Text = value;
            } 
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


        /*
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

        public XAMLiteTextBlock( Game game )
            : base( game )
        {
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            this.spriteFont = courier10SpriteFont;
            this.Padding = new Thickness(0, 0, 0, 0);

            // for Background Color
            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBlock ( Game game, Run run )
            : base( game )
        {
            this.Text = run.textBlock;
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            this._foregroundColor = Color.Black;
            this._backgroundColor = Color.Transparent;
            this.Padding = new Thickness(0, 0, 0, 0);

            // for Background Color
            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
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
            this.arialSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
            this.courier10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
            this.spriteFont = courier10SpriteFont;
            
            if(this.Width == 0)
                CalculateWidthAndHeight(this.Text);
            //AdjustPadding();
            //CreateTextBlockContainer();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                if (_fontFamily.ToString() == "Arial")
                    this.spriteFont = arialSpriteFont;
                else
                    this.spriteFont = courier10SpriteFont;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {

            if(TextWrapping == TextWrapping.Wrap)
                this.Text = WordWrap(this.Text, (int)this.spriteFont.MeasureString(this.Text).X);

            spriteBatch.Begin();

            if (!transparent)
            {
                CalculateWidthAndHeight(this.Text);
                CreateTextBlockContainer();
                spriteBatch.Draw(_pixel, textBlockContainer, this._backgroundColor);
            }
                
            spriteBatch.DrawString(this.spriteFont, this.Text, Position, this._foregroundColor);
            spriteBatch.End();
        }

        /// <summary>
        /// Set when the width of the textblock is not specified.
        /// </summary>
        /// <param name="text"></param>
        private void CalculateWidthAndHeight(string text)
        {
            this.Width = (int)this.spriteFont.MeasureString(text).X;
            this.Height = (int)this.spriteFont.MeasureString(text).Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void AdjustPadding()
        {
            if (this.Width == 0)
                this.Width = this.viewport.Width - (int)this.Padding.Left - (int)this.Margin.Right;

            if (this.Height == 0)
                this.Height = this.viewport.Height - (int)this.Margin.Top - (int)this.Margin.Bottom;
        }

        // Determines the size of the textblock based on Width, Height.
        // Finds the size of the inner rectangle to contain the text based on the padding.
        private void CreateTextBlockContainer()
        {
            if(this.Width != 0  && this.Height != 0)
                textBlockContainer = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
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
            int charsPerLine = this.Width / (int)pxPerChar;

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
                        sb.Append(_newline);
                        // Trim whitespace following break
                        pos += len;
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
