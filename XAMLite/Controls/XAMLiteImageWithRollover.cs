namespace XAMLite
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Emulates a xaml image with rollover
    /// </summary>
    public class XAMLiteImageWithRollover : XAMLiteImage
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
        public XAMLiteImageWithRollover(Game game)
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
            Width = Texture.Width;
            Height = Texture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible != System.Windows.Visibility.Visible)
            {
                return;
            }

            SpriteBatch.Begin();

            SpriteBatch.Draw(MouseEntered ? _rolloverTexture : Texture, Panel, (Color.White * (float)Opacity));

            SpriteBatch.End();
        }
    }
}
