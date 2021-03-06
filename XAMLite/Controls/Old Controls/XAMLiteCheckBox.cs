﻿using System.Diagnostics;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteCheckBox : XAMLiteControl
    {
        /// <summary>
        /// True when the checkbox is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Container for the checkbox texture asset.
        /// </summary>
        private Rectangle _checkBox;

        /// <summary>
        /// Position for the text in relation to the CheckBox asset.
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
        /// An idea for establishing a set of possible preloaded SpriteFonts??
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// 
        /// </summary>
        private bool _fontFamilyChanged; // used in the Update() method

        /// <summary>
        /// 
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
        /// Texture asset for the checkbox when it is unchecked.
        /// </summary>
        private Texture2D _checkBoxUnchecked;

        /// <summary>
        /// This is the image file path, minus the file extension for an 
        /// unchecked checkbox.
        /// </summary>
        public string CheckBoxSourceName { get; set; }

        /// <summary>
        /// Texture asset for the checkbox when it is hovered over but 
        /// unchecked.
        /// </summary>
        private Texture2D _checkBoxUncheckedHover;

        /// <summary>
        /// This is the image file path, minus the file extension for when the
        /// checkbox is hovered over but is unchecked.
        /// </summary>
        public string CheckBoxHoverSourceName { get; set; }

        /// <summary>
        /// Texture asset for a checked checkbox.
        /// </summary>
        private Texture2D _checkBoxChecked;

        /// <summary>
        /// This is the image file path, minus the file extension for when the 
        /// checkbox is checked.
        /// </summary>
        public string CheckBoxSelectedSourceName { get; set; }

        /// <summary>
        /// Texture asset for a hovered and checked checkbox.
        /// </summary>
        private Texture2D _checkBoxCheckedHover;

        /// <summary>
        /// This is the image file path, minus the file extension for when the
        /// checkbox is hovered over and checked.
        /// </summary>
        public string CheckBoxHoverSelectedSourceName { get; set; }

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
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteCheckBox(Game game)
            : base(game)
        {
            Content = "";
            IsChecked = false;
            Text = string.Empty;
            _foregroundColor = Color.White;

            CheckBoxSourceName = "Icons/RadioButton";
            CheckBoxSelectedSourceName = "Icons/RadioButtonSelected";

            Spacing = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseDown += XAMLiteCheckBoxMouseDown;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            RecalculateWidthAndHeight(Text);

            Debug.Assert((CheckBoxSourceName != null), "Must set CheckBoxSourceName property. This is the image file path, minus the file extension.");
            _checkBoxUnchecked = Game.Content.Load<Texture2D>(CheckBoxSourceName);

            Debug.Assert((CheckBoxSelectedSourceName != null), "Must set CheckBoxSelectedSourceName property. This is the image file path, minus the file extension.");
            _checkBoxChecked = Game.Content.Load<Texture2D>(CheckBoxSelectedSourceName);

            if (CheckBoxHoverSourceName != null)
            {
                _checkBoxUncheckedHover = Game.Content.Load<Texture2D>(CheckBoxHoverSourceName);
            }

            if (CheckBoxHoverSelectedSourceName != null)
            {
                _checkBoxCheckedHover = Game.Content.Load<Texture2D>(CheckBoxHoverSelectedSourceName);
            }

            _checkBox = new Rectangle((int)Position.X, (int)Position.Y, _checkBoxChecked.Width, _checkBoxChecked.Height);
            _textPos = new Vector2((Position.X + _checkBox.Width + 10), Position.Y - 2.55f);
            Panel = new Rectangle((int)Position.X, (int)Position.Y, _checkBoxChecked.Width + Width + 10, _checkBoxChecked.Height + Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MarginChanged)
            {
                MarginChanged = false;
                _checkBox = new Rectangle((int)Position.X, (int)Position.Y, _checkBoxChecked.Width, _checkBoxChecked.Height);
                _textPos = new Vector2((Position.X + _checkBox.Width + 10), Position.Y + ((float)_checkBoxChecked.Height / 2) - (SpriteFont.MeasureString(Text).Y / 2));
                Panel = new Rectangle((int)Position.X, (int)Position.Y, _checkBoxChecked.Width + Width + 10, Height);
            }

            if (_fontFamilyChanged)
            {
                _fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == System.Windows.Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (IsEnabled)
                {
                    SpriteFont.Spacing = Spacing;
                    SpriteBatch.DrawString(SpriteFont, Text, _textPos, _foregroundColor);

                    if (_checkBoxCheckedHover != null && _checkBoxUncheckedHover != null)
                    {
                        SpriteBatch.Draw(MouseEntered ? IsChecked ? _checkBoxCheckedHover : _checkBoxUncheckedHover :
                        IsChecked ? _checkBoxChecked : _checkBoxUnchecked,
                        _checkBox,
                        (Color.White * (float)Opacity));
                    }
                    else
                    {
                        SpriteBatch.Draw(
                        IsChecked ? _checkBoxChecked : _checkBoxUnchecked,
                        _checkBox,
                        (Color.White * (float)Opacity));
                    }
                }
                else
                {
                    var opacity = (float)Opacity - 0.5f;
                    if (opacity < 0f)
                    {
                        opacity = 0f;
                    }

                    SpriteBatch.DrawString(SpriteFont, Text, _textPos, (_foregroundColor * opacity));
                    SpriteBatch.Draw(_checkBoxUnchecked, _checkBox, (Color.White * opacity));
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XAMLiteCheckBoxMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsEnabled)
            {
                IsChecked = !IsChecked;
            }
        }
    }
}
