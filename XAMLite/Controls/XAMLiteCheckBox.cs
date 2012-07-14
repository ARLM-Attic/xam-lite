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
        /// <summary>
        /// 
        /// </summary>
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
                if (this.SpriteFont != null)
                {
                    RecalculateWidthAndHeight(value);
                }
                base.Text = value;
            }
        }

        // An idea for establishing a set of possible preloaded SpriteFonts??
        private FontFamily _fontFamily;
        private bool fontFamilyChanged; // used in the Update() method

        /// <summary>
        /// 
        /// </summary>
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

        private Texture2D _checkBoxUnchecked;
        private Texture2D _checkBoxChecked;

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
            this.Content = "";
            this.IsChecked = false;
            this.Text = string.Empty;
            this._foregroundColor = Color.White;

            CheckBoxSourceName = "Icons/RadioButton";
            CheckBoxSelectedSourceName = "Icons/RadioButtonSelected";

            this.Spacing = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseDown += new System.Windows.Input.MouseButtonEventHandler(XAMLiteCheckBox_MouseDown);
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
            Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width + this.Width + 10, _checkBoxChecked.Height + this.Height);
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
                _checkBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width, _checkBoxChecked.Height);
                _textPos = new Vector2((this.Position.X + _checkBox.Width + 10), this.Position.Y);
                Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, _checkBoxChecked.Width + this.Width + 10, this.Height);
            }

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == System.Windows.Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (this.IsEnabled)
                {
                    this.SpriteFont.Spacing = this.Spacing;
                    SpriteBatch.DrawString(this.SpriteFont, Text, _textPos, this._foregroundColor);

                    if (this.IsChecked)
                        SpriteBatch.Draw(this._checkBoxChecked, _checkBox, (Color.White * (float)Opacity));
                    else
                        SpriteBatch.Draw(this._checkBoxUnchecked, _checkBox, (Color.White * (float)Opacity));
                }

                else
                {
                    float opacity = (float)Opacity - 0.5f;
                    if (opacity < 0f)
                    {
                        opacity = 0f;
                    }
                    SpriteBatch.DrawString(this.SpriteFont, Text, _textPos, (this._foregroundColor * opacity));
                    SpriteBatch.Draw(this._checkBoxUnchecked, _checkBox, (Color.White * opacity));
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void XAMLiteCheckBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsEnabled)
            {
                if (!this.IsChecked)
                {
                    this.IsChecked = true;
                }
                else
                {
                    this.IsChecked = false;
                }
            }
        }
    }
}
