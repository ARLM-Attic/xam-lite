using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// Button class with rollover texture.
    /// </summary>
    public class XAMLiteButton : XAMLiteControl
    {
        /// <summary>
        /// The 2-D image for the button that is not hovered over nor being
        /// clicked.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// This is the image file path, minus the file extension for the basic
        /// texture.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// The hover 2-D image for the button.
        /// </summary>
        private Texture2D _rolloverTexture;

        /// <summary>
        /// This is the image file path, minus the file extension for the Rollover image.
        /// </summary>
        public string RolloverSourceName
        {
            get;
            set;
        }

        /// <summary>
        /// The clicked 2-D image for the button.
        /// </summary>
        private Texture2D _clickTexture;

        /// <summary>
        /// This is the image file path, minus the file extension for the Clicked Button image.
        /// </summary>
        public string ClickSourceName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAMLiteButton"/> class. 
        /// </summary>
        /// <param name="game">
        /// Reference to the Game
        /// </param>
        public XAMLiteButton(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Loads the button content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            Debug.Assert((SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension.");
            _texture = Game.Content.Load<Texture2D>(SourceName);

            if (Width == 0)
            {
                Width = _texture.Width;
            }

            if (Height == 0)
            {
                Height = _texture.Height;
            }

            if (RolloverSourceName != null)
            {
                _rolloverTexture = Game.Content.Load<Texture2D>(RolloverSourceName);
            }

            if (ClickSourceName != null)
            {
                _clickTexture = Game.Content.Load<Texture2D>(ClickSourceName);
            }

            // Sets the size and location of the image.
            Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// Updates the Button.
        /// </summary>
        /// <param name="gameTime">Reference to the GameTime.</param>
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
        /// Draws the Button.
        /// </summary>
        /// <param name="gameTime">The GameTime reference.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == System.Windows.Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (_clickTexture != null && _rolloverTexture != null)
                {
                    SpriteBatch.Draw(
                        MousePressed ? _clickTexture : MouseEntered ? _rolloverTexture : _texture,
                        Panel,
                        Color.White * (float)Opacity);
                }
                else if (_rolloverTexture != null)
                {
                    SpriteBatch.Draw(
                        MouseEntered ? _rolloverTexture : _texture,
                        Panel,
                        Color.White * (float)Opacity);
                }
                else
                {
                    SpriteBatch.Draw(
                        _texture,
                        Panel,
                        Color.White * (float)Opacity);
                }

                SpriteBatch.End();
            }
        }
    }
}
