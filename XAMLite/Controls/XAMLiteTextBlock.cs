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
                    if (this.spriteFont != null)
                    {
                        //RecalculateWidthAndHeight(value);
                    }
                    base.Text = value;
                    //this.Text = WordWrap(this.Text, this.Width);
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
            }
        }


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
            
            //RecalculateWidthAndHeight(this.Text);
            //AdjustPadding();
            //CreateTextBlockContainer();
            //this.Text = WordWrap2(this.Text, this.Width);
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
                //RecalculateWidthAndHeight(this.Text);
                //CreateTextBlockContainer();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //CreateTextBlockContainer();

            this.Text = WordWrap(this.Text, (int)this.spriteFont.MeasureString(this.Text).X);

            spriteBatch.Begin();
            if(_backgroundColor != Color.Transparent) 
                spriteBatch.Draw(_pixel, textBlockContainer, this._backgroundColor);
            spriteBatch.DrawString(this.spriteFont, this.Text, Position, this._foregroundColor);
            spriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void RecalculateWidthAndHeight(string text)
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
            //this.Text = WordWrap2(this.Text, this.Width);
            /*double stringWidth = this.spriteFont.MeasureString(this.Text).X;
            double stringHeight = this.spriteFont.MeasureString(this.Text).Y;
            
            if(this.Width != 0  && this.Height != 0)
                textBlockContainer = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            */
        }

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
            if (this.Width > width)
                return text;

            float wrdLenPixels = this.spriteFont.MeasureString(text).X;
            int numCharsinString = 0;

            for (int i = 0; i < text.Length; i++)
                numCharsinString++;

            // Find start of whitespace at end of text
            while ((numCharsinString - 1) >= 0 && Char.IsWhiteSpace(text[numCharsinString - 1]))
                numCharsinString--;

            Console.WriteLine("Width of TextBox (Pixels): " + this.Width);
            Console.WriteLine("Width of Text (Pixels: " + wrdLenPixels);
            Console.WriteLine("Num Characters: " + numCharsinString + "\n");

            sb = new StringBuilder();

            return sb.ToString();
        }

    }

    // for mocking the WPF constructor
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
