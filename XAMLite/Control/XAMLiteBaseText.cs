using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteBaseText : XAMLiteBaseContentControl
    {
        /// <summary>
        /// Text contained in the control.
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// The text color.
        /// </summary>
        private Color ForegroundColor
        {
            get
            {
                var solidBrush = (SolidColorBrush)Foreground;
                var color = solidBrush.Color;
                return new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseText(Game game) 
            : base(game)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Text != null)
            {
                SpriteBatch.DrawString(SpriteFont, Text, new Vector2(Panel.X, Panel.Y), ForegroundColor * (float)Opacity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UpdateFontMeasurements()
        {
            base.UpdateFontMeasurements();

            RecalculateWidthAndHeight(Text);
        }
    }
}
