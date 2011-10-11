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
        protected SpriteFont spriteFont;

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        public TextAlignment TextAlignment { get; set; }

        public FontFamily FontFamily { get; set; }

        public FontStyle FontStyle { get; set; }

        public FontWeight FontWeight { get; set; }

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBlock ( Game game, string text )
            : base( game )
        {
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            this.Text = text;
            this._foregroundColor = Color.Black;
            this._backgroundColor = Color.Transparent;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="fontName"></param>
        protected override void LoadContent()
        {
            this.spriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
            //RecalculateWidthAndHeight(this.Text);
            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(this.spriteFont, Text, Position, this._foregroundColor);
            spriteBatch.End();
        }

    }
}
