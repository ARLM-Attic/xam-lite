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

        private Texture2D _edgeTextureNormal;

        private Texture2D _edgeTextureOver;

        private Texture2D _edgeTextureDown;

        /// <summary>
        /// This is the image file path, minus the file extension for the Clicked Button image.
        /// </summary>
        public string ClickSourceName { get; set; }

        /// <summary>
        /// True when default textures are being used.
        /// </summary>
        private bool _isDefaultTextures;

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
            base.LoadContent();

            if (SourceName == null)
            {
                LoadDefaultTextures();
            }

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
        }

        /// <summary>
        /// Loads default textures when the basic texture has not been set.
        /// </summary>
        private void LoadDefaultTextures()
        {
            _isDefaultTextures = true;

            _edgeTextureNormal = Game.Content.Load<Texture2D>("Images/ButtonEdgeNormal");
            _edgeTextureOver = Game.Content.Load<Texture2D>("Images/ButtonEdgeOver");
            _edgeTextureDown = Game.Content.Load<Texture2D>("Images/ButtonEdgeDown");

            SourceName = "Images/ButtonCenterNormal";
            RolloverSourceName = "Images/ButtonCenterOver";
            ClickSourceName = "Images/ButtonCenterDown";

            if (Content != null)
            {
                RecalculateWidthAndHeight(Content);

                Width += (int)Padding.Left + (int)Padding.Right + _edgeTextureNormal.Width;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected override void RecalculateWidthAndHeight(object content)
        {
            base.RecalculateWidthAndHeight(content);

            if (content != null)
            {
                var height = (int)SpriteFont.MeasureString(content.ToString()).Y;
                Height = height > _texture.Height ? height : _texture.Height;
            }
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

                if (_isDefaultTextures)
                {
                    
                }

                SpriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
}
