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
        public List<XAMLiteMenuItem> Items;

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
        private Rectangle _subMenuPanel;

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

        protected bool displaySubMenu;

        protected bool _setSubMenu;

        protected bool adjusted;

        protected int longestWidth;

        public XAMLiteMenuItem(Game game)
            : base(game)
        {
            
            this._foregroundColor = Color.White;
            bc = new System.Windows.Media.BrushConverter();
            _stroke = Color.Black;
            _strokeThickness = 2;
            _strokePanel = new Rectangle();
            _subMenuPanel = new Rectangle();
            Items = new List<XAMLiteMenuItem>();
            longestWidth = 0;
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

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                this.spriteFont.Spacing = Spacing;
                RecalculateWidthAndHeight(this.Header);
            }

            if (!_setSubMenu)
            {
                _setSubMenu = true;
                setSubMenuItems(gameTime);
               // this.marginChanged = true;
            }

            if (marginChanged)
            {
                marginChanged = false;
                _panel = new Rectangle((int)this.Position.X - (int)this.Padding.Left, (int)this.Position.Y, this.Width, this.Height);
                _subMenuPanel = new Rectangle(this._panel.X + this.Width, (int)this.Position.Y, longestWidth + 10, this._panel.Height * Items.Count);
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

            if (_mouseEnter && Items.Count > 0 && _panel.Contains(_msRect) && this.Visible == Visibility.Visible)
            {
                displaySubMenu = true;
                _subMenuSelected = true;
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Visible = Visibility.Visible;
                }
            }

            

            /*if (_mouseEnter && displaySubMenu && this.Visible == Visibility.Visible)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Visible = Visibility.Visible;
                }
            }*/
            else
            {
                if (!_subMenuSelected)
                {
                    displaySubMenu = false;
                    for (int i = 0; i < Items.Count; i++)
                    {
                        Items[i].Visible = Visibility.Hidden;
                    }
                }
                else if (Items.Count > 0 && !_subMenuPanel.Contains(_msRect))
                {
                    _subMenuSelected = false;
                }
            }

            if (displaySubMenu && !adjusted)
            {
                adjusted = true;
                
                int tempHeight = this._panel.Y;
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Margin = new Thickness(this.Margin.Left + this.Width, this.Margin.Top + Items[i].Height * i, Items[i].Margin.Right + Items[i].Padding.Right, this.Margin.Bottom + Items[i].Padding.Bottom);
                    tempHeight += Items[i].Height;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
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

                    if (_allSubMenuTitles.Contains(this))
                    {
                        arrowRect.X = this._panel.X + this.Width - arrow.Width;
                        arrowRect.Y = this._panel.Y + this.Height / 4;
                        this.spriteBatch.Draw(arrow, arrowRect, Color.White);
                    }
                }

                spriteBatch.DrawString(this.spriteFont, Text, Position, this._foregroundColor);
                
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        /// <param name></param>
        private void setSubMenuItems(GameTime gameTime)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                this.Game.Components.Add(Items[i]);
                Items[i].Visible = Visibility.Hidden;
                Items[i].Padding = new Thickness(10, 0, 10, 0);
                Items[i].HorizontalAlignment = this.HorizontalAlignment;
                Items[i].VerticalAlignment = this.VerticalAlignment;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (_allSubMenuTitles.Contains(Items[i]))
                {
                    Items[i].Width += 20;
                }

                if (longestWidth <= Items[i].Width)
                    longestWidth = Items[i].Width;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Width = longestWidth + 20;  
            }
        }
    }
}
