using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteMenuItem : XAMLiteControl
    {
        /// <summary>
        ///  To hold sub-menu items that draw either to the right or left of this menu item
        ///  depending on its parent's location.
        /// </summary>
        List<XAMLiteMenuItem> Items;

        /// <summary>
        /// This just duplicates the Text property but is here since XAML developer will expect to be able
        /// to set the Header property of a menu item.
        /// </summary>
        public string Header
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
                    this.spriteFont.Spacing = Spacing;
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
        /// 
        /// </summary>
        protected Color _foregroundColor;

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
        private Color _backgroundColor;

        /// <summary>
        /// 
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                if ((SolidColorBrush)value == Brushes.Transparent)
                    transparent = true;
                else
                    transparent = false;

            }
        }

        public Thickness Padding;

        private bool transparent;

        BrushConverter bc;

        private Color _fill;
        private Color _stroke;

        private Rectangle _strokePanel;

        /// <summary>
        /// May become Public used to set up a Fill property later as described by the user.
        /// </summary>
        private Brush Fill
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _fill = new Color(color.R, color.G, color.B, color.A);
                
            }
        }

        /// <summary>
        /// May become Public and used to set up a Fill property later as described by the user.
        /// </summary>
        private Brush Stroke
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _stroke = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// May become public and used to set up a Stroke Thickness later as described by the user.
        /// </summary>
        private int _strokeThickness { get; set; }

        /// <summary>
        /// If true, the user has clicked on a menu and the menu item should display, unless it is the
        /// Header for the menu, in which case it will always draw.
        /// </summary>
        protected bool draw;

        public XAMLiteMenuItem(Game game)
            : base(game)
        {
            
            this._foregroundColor = Color.White;
            bc = new System.Windows.Media.BrushConverter();
            _stroke = Color.Black;
            _strokeThickness = 2;
            _strokePanel = new Rectangle();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteFont = verdana15SpriteFont;
            this.Spacing = 2;
            RecalculateWidthAndHeight(this.Header);
            _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //System.Console.WriteLine(Height);
            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                this.spriteFont.Spacing = Spacing;
                RecalculateWidthAndHeight(this.Header);
            }

            if (marginChanged)
            {
                marginChanged = false;
                _panel = new Rectangle((int)this.Position.X - (int)this.Padding.Left, (int)this.Position.Y, this.Width, this.Height);
            }

            if (_mouseEnter)
            {
                this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#cccccc");
            }
            else
            {
                 if(!_allMenuTitles.Contains(this))
                    this.Background = Brushes.Black;
            }

            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                //if (draw)
                //{
                    spriteBatch.Begin();
                    if (!transparent)
                    {
                        if (_allMenuTitles.Contains(this))
                            spriteBatch.Draw(_pixel, _panel, this._backgroundColor);
                        else
                        {
                            Rectangle ghostRect = new Rectangle(_panel.X + 5, _panel.Y + 5, _panel.Width, _panel.Height);
                            spriteBatch.Draw(_pixel, ghostRect, (Color.Black * 0.45f));
                            spriteBatch.Draw(_pixel, _panel, this._backgroundColor);
                            _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left, (int)this.Position.Y, this.Width, _strokeThickness);
                            this.spriteBatch.Draw(_pixel, _strokePanel, _stroke);
                            _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left, ((int)this.Position.Y + this.Height - _strokeThickness), this.Width, _strokeThickness);
                            this.spriteBatch.Draw(_pixel, _strokePanel, _stroke);
                            _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left, (int)this.Position.Y, _strokeThickness, this.Height);
                            this.spriteBatch.Draw(_pixel, _strokePanel, _stroke);
                            _strokePanel = new Rectangle(((int)this.Position.X - (int)this.Padding.Left + this.Width - _strokeThickness), (int)this.Position.Y, _strokeThickness, this.Height);
                            this.spriteBatch.Draw(_pixel, _strokePanel, _stroke);
                        }
                            
                    }
                    spriteBatch.DrawString(this.spriteFont, Text, Position, this._foregroundColor);
                    spriteBatch.End();
               // }
            }
        }
    }
}
