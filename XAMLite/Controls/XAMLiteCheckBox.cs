using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteCheckBox : XAMLiteControl
    {
        public bool IsChecked { get; set; }

        private Rectangle _checkBox;
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
                return this.Text;
            }

            set
            {
                this.Text = value;
                if (this.spriteFont != null)
                {
                    RecalculateWidthAndHeight(value);
                }
                base.Text = value;
            }
        }

        // An idea for establishing a set of possible preloaded SpriteFonts??
        private FontFamily _fontFamily;
        private bool fontFamilyChanged; // used in the Update() method

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; fontFamilyChanged = true; }
        }

        // character spacing
        public int Spacing { get; set; }
        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string CheckBoxSourceName
        {
            get;
            set;
        }

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string CheckBoxSelectedSourceName
        {
            get;
            set;
        }


        //public string GroupName { get; set; }

        private Texture2D _checkBoxUnchecked;
        private Texture2D _checkBoxChecked;

        private bool _checked;

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

        public XAMLiteCheckBox(Game game)
            : base(game)
        {
            this.Content = "";
            this.IsChecked = false;
            this.Text = string.Empty;
            this._foregroundColor = Color.White;

            CheckBoxSourceName = "Icons/RadioButton";
            CheckBoxSelectedSourceName = "Icons/RadioButtonSelected";

            this.Spacing = 2;
        }

        public override void Initialize()
        {
            base.Initialize();

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            RecalculateWidthAndHeight(this.Text);

            _checkBoxChecked = Game.Content.Load<Texture2D>(CheckBoxSelectedSourceName);
            _checkBoxUnchecked = Game.Content.Load<Texture2D>(CheckBoxSourceName);

            _checkBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width, _checkBoxChecked.Height);
            _textPos = new Vector2((this.Position.X + _checkBox.Width + 10), this.Position.Y);
            _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width + this.Width + 10, _checkBoxChecked.Height + this.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (marginChanged)
            {
                marginChanged = false;
                _checkBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width, _checkBoxChecked.Height);
                _textPos = new Vector2((this.Position.X + _checkBox.Width + 10), this.Position.Y);
                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width + this.Width + 10, this.Height);
            }

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
            }

            if (!_checked && _mouseDown && _panel.Contains(_msRect) && IsEnabled)
            {
                if (!this.IsChecked)
                {
                    _checked = true;
                    this.IsChecked = true;
                }
                else
                {
                    _checked = true;
                    this.IsChecked = false;  
                }
            }
            else if (_mouseUp && _panel.Contains(_msRect) && IsEnabled)
            {
                _checked = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //
            if (Visible == System.Windows.Visibility.Visible)
            {
                this.spriteBatch.Begin();

                if (this.IsEnabled)
                {
                    this.spriteFont.Spacing = this.Spacing;
                    spriteBatch.DrawString(this.spriteFont, Text, _textPos, this._foregroundColor);

                    if (this.IsChecked)
                        this.spriteBatch.Draw(this._checkBoxChecked, _checkBox, (Color.White * (float)Opacity));
                    else
                        this.spriteBatch.Draw(this._checkBoxUnchecked, _checkBox, (Color.White * (float)Opacity));
                }

                else
                {
                    spriteBatch.DrawString(this.spriteFont, Text, _textPos, (this._foregroundColor * (float)0.50));
                    this.spriteBatch.Draw(this._checkBoxUnchecked, _checkBox, (Color.White * (float)0.50));
                }

                this.spriteBatch.End();
            }
        }
    }
}
