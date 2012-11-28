using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteLabel class until this class replaces it.
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ms611056.aspx"/>
    public class XAMLiteLabelNew : XAMLiteBaseContent
    {
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

            if (Content != null)
            {
                SpriteBatch.Begin();

                if (!AttachedToGrid)
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