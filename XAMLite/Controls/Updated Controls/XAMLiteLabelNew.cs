using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;

    /// <summary>
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteLabel class until this class replaces it.
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ms611056.aspx"/>
    public class XAMLiteLabelNew : XAMLiteBaseContent
    {
        /// <summary>
        /// True when the control has been repositioned because the Font 
        /// Family, Margin, or Position was not at its default state. 
        /// </summary>
        private bool _isModified;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteLabelNew(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Constructor that includes the text.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="content"> </param>
        public XAMLiteLabelNew(Game game, object content)
            : base(game)
        {
            SetContent(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // allow the control to update once before drawing when
            // Margins, positions, or Font are not default.
            if (!_isModified && (Margin != new Thickness() || Position != Vector2.Zero || SpriteFont != Courier10SpriteFont))
            {
                _isModified = true;
                return;
            }

            if (Visibility == Visibility.Hidden) // || GridIsHidden)
            {
                return;
            }

            if (Content != null)
            {
                SpriteBatch.Begin();

                if (!IsAttachedToGrid)
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
        /// Sets the content of the control.
        /// </summary>
        /// <param name="content"></param>
        private void SetContent(object content)
        {
            Content = content;
        }
    }
}