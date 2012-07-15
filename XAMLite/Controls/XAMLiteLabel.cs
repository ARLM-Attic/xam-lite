using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ms611056.aspx"/>
    public class XAMLiteLabel : XAMLiteControl
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (SpriteFont != null)
                {
                    SpriteFont.Spacing = Spacing;
                    RecalculateWidthAndHeight(value);
                }

                base.Text = value;
            }
        }

        /// <summary>
        /// This just duplicates the Text property but is here since XAML developer will expect to be able
        /// to set the Content property of a label. Note: This Content property shouldn't be confuse with 
        /// XNA's concept of Content (i.e. textures and models, etc).
        /// </summary>
        public string Content
        {
            get
            {
                return Text;
            }

            set
            {
                Text = value;
                if (SpriteFont != null)
                {
                    SpriteFont.Spacing = Spacing;
                    RecalculateWidthAndHeight(value);
                }

                base.Text = value;
            }
        }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// True when the font family has changed.
        /// </summary>
        private bool _fontFamilyChanged;

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
                _fontFamilyChanged = true; 
                FirstUpdate = true;
            }
        }

        /// <summary>
        /// character spacing.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Color of the text.
        /// </summary>
        protected Color ForegroundColor;

        /// <summary>
        /// Color of the text.
        /// </summary>
        public Brush Foreground
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                ForegroundColor = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteLabel(Game game)
            : base(game)
        {
            base.Text = string.Empty;
            ForegroundColor = Color.White;
        }

        /// <summary>
        /// Constructor that includes the text.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="text"> </param>
        public XAMLiteLabel(Game game, string text)
            : base(game)
        {
            base.Text = text;
            Spacing = 0;
            ForegroundColor = Color.White;
        }

        /// <summary>
        /// Loads the content of the label
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            RecalculateWidthAndHeight(Text);
        }

        /// <summary>
        /// Updates the label.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (FirstUpdate)
            {
                if (_fontFamilyChanged)
                {
                    _fontFamilyChanged = false;
                    UpdateFontFamily(_fontFamily);
                    SpriteFont.Spacing = Spacing;
                    RecalculateWidthAndHeight(Text);

                    FirstUpdate = false;
                }
            }
        }

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible && !FirstUpdate)
            {
                SpriteBatch.Begin();
                SpriteBatch.DrawString(SpriteFont, Text, Position, ForegroundColor * (float)Opacity);
                SpriteBatch.End();
            }
        }
    }
}