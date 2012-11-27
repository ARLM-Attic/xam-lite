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
        /// Sets the content of the control.
        /// </summary>
        /// <param name="content"></param>
        private void SetContent(object content)
        {
            Content = content;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // for debugging purposes.
            SpriteBatch.Begin();
            //SpriteBatch.Draw(Pixel, new Rectangle((int)Position.X, (int)Position.Y, 1, 1), Color.Red);
            //SpriteBatch.Draw(Pixel, new Rectangle((int)Position.X, (int)Position.Y + Height, 1, 1), Color.Red);
            SpriteBatch.End();
        }
    }
}