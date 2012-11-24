using System.Diagnostics;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

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
        /// Text or a single object.
        /// </summary>
        public object Content { get; set; }

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
        /// The font family the text belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// True when the font family has changed.
        /// </summary>
        protected bool FontFamilyChanged;

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
                FontFamilyChanged = true;
                FirstUpdate = true;
            }
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
        /// 
        /// </summary>
        private Color _foregroundColor;

        /// <summary>
        /// 
        /// </summary>
        public Brush Foreground
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _foregroundColor = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// character spacing.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAMLiteButton"/> class. 
        /// </summary>
        /// <param name="game">
        /// Reference to the Game
        /// </param>
        public XAMLiteButton(Game game)
            : base(game)
        {
            Spacing = 0;
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

            if (FontFamilyChanged)
            {
                UpdateFontMeasurements();
            }
        }

        /// <summary>
        /// Updates the spacing, font family, and retakes the string 
        /// measurements
        /// </summary>
        private void UpdateFontMeasurements()
        {
            UpdateFontFamily(_fontFamily);
            SpriteFont.Spacing = Spacing;
            RecalculateWidthAndHeight(Content);
            FontFamilyChanged = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected void RecalculateWidthAndHeight(object content)
        {
            Width = (int)SpriteFont.MeasureString(content.ToString()).X;
            var height = (int)SpriteFont.MeasureString(content.ToString()).Y;
            Height = height > _texture.Height ? height : _texture.Height;
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

                if (Content != null)
                {
                    SpriteBatch.DrawString(SpriteFont, Content.ToString(), new Vector2(Panel.X, Panel.Y), _foregroundColor * (float)Opacity);
                }

                SpriteBatch.End();
            }
        }
    }
}
