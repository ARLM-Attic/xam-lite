using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// Button class with rollover and mouse down textures.
    /// 
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteButton class until this class replaces it.
    /// </summary>
    public class XAMLiteButtonNew : XAMLiteBaseContent
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
        public string RolloverSourceName { get; set; }

        /// <summary>
        /// The clicked 2-D image for the button.
        /// </summary>
        private Texture2D _clickTexture;

        /// <summary>
        /// This is the image file path, minus the file extension for the Clicked Button image.
        /// </summary>
        public string ClickSourceName { get; set; }

        //private int count = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="XAMLite.XAMLiteButton"/> class. 
        /// </summary>
        /// <param name="game">
        /// Reference to the Game
        /// </param>
        public XAMLiteButtonNew(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Loads the button content.
        /// </summary>
        protected override void LoadContent()
        {
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

            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected override void RecalculateWidthAndHeight(object content)
        {
            base.RecalculateWidthAndHeight(content);

            var height = (int)SpriteFont.MeasureString(content.ToString()).Y;
            Height = height > _texture.Height ? height : _texture.Height;
        }

        /// <summary>
        /// Draws the Button.
        /// </summary>
        /// <param name="gameTime">The GameTime reference.</param>
        public override void Draw(GameTime gameTime)
        {
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

                base.Draw(gameTime);
            }
        }
    }
}
