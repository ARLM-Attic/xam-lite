using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteRadioButton : XAMLiteControl
    {
        /// <summary>
        /// True when the radio button is selected.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// The rectangle in which the radio button is drawn.
        /// </summary>
        private Rectangle _radio;

        /// <summary>
        /// Position of the text in relation to the radio button.
        /// </summary>
        private Vector2 _textPos;

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
                    RecalculateWidthAndHeight(value);
                }

                base.Text = value;
            }
        }

        /// <summary>
        /// Family of fonts the text belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// true when the font family changes.
        /// </summary>
        private bool _fontFamilyChanged;

        /// <summary>
        /// Family of fonts the text belongs to.
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
            }
        }

        /// <summary>
        /// character spacing
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string RadioButtonSourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string RadioButtonSelectedSourceName { get; set; }

        /// <summary>
        /// The group in which a radio button belongs.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Contains the 2D texture for an unselected radio button.
        /// </summary>
        private Texture2D _radioUnselected;

        /// <summary>
        /// Contains the 2D texture for an selected radio button.
        /// </summary>
        private Texture2D _radioSelected;

        /// <summary>
        /// The text color.
        /// </summary>
        private Color _foregroundColor;

        /// <summary>
        /// The text color.
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
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRadioButton(Game game)
            : base(game)
        {
            Content = "";
            IsChecked = false;
            Text = string.Empty;
            _foregroundColor = Color.White;

            RadioButtonSourceName = "Icons/RadioButton";
            RadioButtonSelectedSourceName = "Icons/RadioButtonSelected";

            Spacing = 2;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            RecalculateWidthAndHeight(Text);

            _radioSelected = Game.Content.Load<Texture2D>(RadioButtonSelectedSourceName);
            _radioUnselected = Game.Content.Load<Texture2D>(RadioButtonSourceName);

            _radio = new Rectangle((int)Position.X, (int)Position.Y, _radioSelected.Width, _radioSelected.Height);
            _textPos = new Vector2(Position.X + _radio.Width + 10, Position.Y);
            Panel = new Rectangle((int)Position.X, (int)Position.Y, _radioSelected.Width + Width + 10, _radioSelected.Height + Height);

            AllRadioButtons.Add(this);
        }

        /// <summary>
        /// Updates the radio button.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MarginChanged)
            {
                MarginChanged = false;
                _radio = new Rectangle((int)Position.X, (int)Position.Y, _radioSelected.Width, _radioSelected.Height);
                _textPos = new Vector2(Position.X + _radio.Width + 10, Position.Y);
                Panel = new Rectangle((int)Position.X, (int)Position.Y, _radioSelected.Width + Width + 10, Height);
            }

            if (_fontFamilyChanged)
            {
                _fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
            }

            if (MousePressed && !Selected && Panel.Contains(MsRect) && IsEnabled)
            {
                Selected = true;
                foreach (var t in AllRadioButtons)
                {
                    if (t.IsChecked && t.GroupName == GroupName && t.IsEnabled)
                    {
                        t.IsChecked = false;
                    }
                }

                IsChecked = true;
            }
            else if (Selected)
            {
                Selected = false;
            }
        }

        /// <summary>
        /// Draws the radio button.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible != Visibility.Visible)
            {
                return;
            }

            SpriteBatch.Begin();

            if (IsEnabled)
            {
                SpriteFont.Spacing = Spacing;
                SpriteBatch.DrawString(SpriteFont, Text, _textPos, _foregroundColor * (float)Opacity);

                SpriteBatch.Draw(IsChecked ? _radioSelected : _radioUnselected, _radio, Color.White * (float)Opacity);
            }
            else
            {
                var opacity = (float)Opacity * 0.5f;

                SpriteBatch.DrawString(SpriteFont, Text, _textPos, _foregroundColor * opacity);
                SpriteBatch.Draw(_radioUnselected, _radio, Color.White * opacity);
            }

            SpriteBatch.End();
        }
    }
}
