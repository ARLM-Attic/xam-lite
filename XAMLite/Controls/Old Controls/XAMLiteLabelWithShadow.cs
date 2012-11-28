using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// 
    /// </summary>
    public class XAMLiteLabelWithShadow : XAMLiteLabel
    {
        /// <summary>
        /// Position of the drop shadow beneath the text.
        /// </summary>
        private readonly Vector2 _shadowPosition;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="initialText"></param>
        public XAMLiteLabelWithShadow(Game game, string initialText)
            : base(game, initialText)
        {
            _shadowPosition = new Vector2(1, 1);
        }

        /// <summary>
        /// Draws the Label with drop shadow.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == System.Windows.Visibility.Visible && !FirstUpdate && !FontFamilyChanged)
            {
                // Begin.
                SpriteBatch.Begin();
                
                // Draw shadow text.
                SpriteBatch.DrawString(SpriteFont, Text, Position + _shadowPosition, Color.Black * (float)Opacity);

                // Draw text.
                SpriteBatch.DrawString(SpriteFont, Text, Position, ForegroundColor * (float)Opacity);

                // End.
                SpriteBatch.End();
            }
        }
    }
}