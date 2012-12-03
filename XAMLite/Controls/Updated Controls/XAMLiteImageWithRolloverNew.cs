namespace XAMLite
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Emulates a xaml image with rollover
    /// </summary>
    public class XAMLiteImageWithRolloverNew : XAMLiteImageNew
    {
        /// <summary>
        /// This is the image file path, minus the file extension for the Rollover image.
        /// </summary>
        public string RolloverSourceName
        {
            get;
            set;
        }

        /// <summary>
        /// Texture for when the mouse hovers on the control.
        /// </summary>
        private Texture2D _rolloverTexture;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteImageWithRolloverNew(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _rolloverTexture = Game.Content.Load<Texture2D>(RolloverSourceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible != System.Windows.Visibility.Visible)
            {
                return;
            }

            SpriteBatch.Begin();
            if (RenderTransform == null)
            {
                SpriteBatch.Draw(MouseEntered ? _rolloverTexture : Texture, Panel, (Color.White * (float)Opacity));
            }
            else
            {
                SpriteBatch.Draw(MouseEntered ? _rolloverTexture : Texture, Panel, null, Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1);
            }

            SpriteBatch.End();
        }
    }
}
