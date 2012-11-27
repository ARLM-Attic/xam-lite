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
        /// True when the control is a part of another control.  For example, 
        /// a XAMLiteLabel associated with the XAMLiteCheckBox class.
        /// </summary>
        protected internal bool AttachedToOtherControl;

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
        /// Constructor.
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
                if (this is XAMLiteLabelNew && !AttachedToOtherControl)
                {
                    SpriteBatch.DrawString(
                        SpriteFont,
                        Content.ToString(),
                        new Vector2(ContentPosition.X, ContentPosition.Y - (float)(Height * 0.14)),
                        ForegroundColor * (float)Opacity);
                }
                else
                {
                    SpriteBatch.DrawString(
                        SpriteFont,
                        Content.ToString(),
                        ContentPosition,
                        ForegroundColor * (float)Opacity);
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Updates the FontFamily, Spacing, and recalculates the new
        /// Width and Height.
        /// </summary>
        protected override void UpdateFontMeasurements()
        {
            base.UpdateFontMeasurements();

            RecalculateWidthAndHeight(Content);
        }
    }
}
