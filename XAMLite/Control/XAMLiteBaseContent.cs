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
    public class XAMLiteBaseContent : XAMLiteBaseContentControl
    {
        /// <summary>
        /// Object contained in the control, which might include
        /// string, date/time, etc.
        /// </summary>
        public virtual object Content { get; set; }

        /// <summary>
        /// The content color.
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
        public XAMLiteBaseContent(Game game)
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

            if (Content != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.DrawString(
                        SpriteFont, Content.ToString(), ContentPosition, ForegroundColor * (float)Opacity);
                SpriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UpdateFontMeasurements()
        {
            base.UpdateFontMeasurements();

            RecalculateWidthAndHeight(Content);
        }
    }
}
