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
    public class XAMLiteRadioButton : XAMLiteControl
    {
        public bool IsChecked { get; set; }

        private Rectangle radio;
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
        public string RadioButtonSourceName
        {
            get;
            set;
        }

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string RadioButtonSelectedSourceName
        {
            get;
            set;
        }


        public string GroupName { get; set; }

        private Texture2D _radioUnselected;
        private Texture2D _radioSelected;

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

        public XAMLiteRadioButton(Game game)
            : base(game)
        {
            this.Content = "";
            this.IsChecked = false;
            this.Text = string.Empty;
            this._foregroundColor = Color.White;

            RadioButtonSourceName = "Icons/RadioButton";
            RadioButtonSelectedSourceName = "Icons/RadioButtonSelected";

            this.Spacing = 2;
        }

        /// <summary>
        /// 
        /// </summary>
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

            _radioSelected = Game.Content.Load<Texture2D>(RadioButtonSelectedSourceName);
            _radioUnselected = Game.Content.Load<Texture2D>(RadioButtonSourceName);

            radio = new Rectangle((int)this.Position.X, (int)this.Position.Y, _radioSelected.Width, _radioSelected.Height);
            _textPos = new Vector2((this.Position.X + radio.Width + 10), this.Position.Y);
            Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, _radioSelected.Width + this.Width + 10, _radioSelected.Height + this.Height);

            AllRadioButtons.Add(this);
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
                radio = new Rectangle((int)this.Position.X, (int)this.Position.Y, _radioSelected.Width, _radioSelected.Height);
                _textPos = new Vector2((this.Position.X + radio.Width + 10), this.Position.Y);
                Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, _radioSelected.Width + this.Width + 10, this.Height);
            }

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
            }

            if (MousePressed && !Selected && Panel.Contains(MsRect) && IsEnabled)
            {
                Selected = true;
                for (int i = 0; i < AllRadioButtons.Count; i++)
                {
                    if (AllRadioButtons[i].IsChecked && AllRadioButtons[i].GroupName == this.GroupName && AllRadioButtons[i].IsEnabled == true)
                    {
                        AllRadioButtons[i].IsChecked = false;
                    }
                }
                this.IsChecked = true;
            }
            else if (Selected)
            {
                Selected = false;
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
                SpriteBatch.Begin();

                if (this.IsEnabled)
                {
                    this.SpriteFont.Spacing = this.Spacing;
                    SpriteBatch.DrawString(this.SpriteFont, Text, _textPos, (this._foregroundColor * (float)Opacity));

                    if (this.IsChecked)
                        SpriteBatch.Draw(this._radioSelected, radio, (Color.White * (float)Opacity));
                    else
                        SpriteBatch.Draw(this._radioUnselected, radio, (Color.White * (float)Opacity));
                }

                else
                {
                    float opacity = (float)Opacity * 0.5f;

                    SpriteBatch.DrawString(this.SpriteFont, Text, _textPos, (this._foregroundColor * opacity));
                    SpriteBatch.Draw(this._radioUnselected, radio, (Color.White * opacity));
                }

                SpriteBatch.End();
            }
        }
    }
}
