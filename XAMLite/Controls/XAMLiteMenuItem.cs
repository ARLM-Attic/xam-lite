using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Input;

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
                if (this.SpriteFont != null)
                {
                    this.SpriteFont.Spacing = Spacing;
                    RecalculateWidthAndHeight(value);
                }
                base.Text = value;
            }
        }

        /// <summary>
        /// The font style used.
        /// </summary>
        private FontFamily _fontFamily;

        private bool _fontFamilyChanged; // used in the Update() method

        /// <summary>
        /// The font style used.
        /// </summary>
        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; _fontFamilyChanged = true; }
        }

        // character spacing
        public int Spacing { get; set; }

        /// <summary>
        /// The color of the text.
        /// </summary>
        protected Color _foregroundColor;

        /// <summary>
        /// The color of the text.
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
        /// The background color of the areas containing the text.
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// The background color of the areas containing the text.
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                if ((SolidColorBrush)value == Brushes.Transparent)
                    _transparent = true;
                else
                    _transparent = false;
            }
        }

        /// <summary>
        /// Amount of space between the text and the edge of the background 
        /// that contains it.
        /// </summary>
        public Thickness Padding;

        private bool _transparent;

        private BrushConverter _bc;

        /// <summary>
        /// 
        /// </summary>
        private Color _fill;

        /// <summary>
        /// The color drawn along the edges of the menus.
        /// </summary>
        private Color _stroke;

        /// <summary>
        /// The rectangles used to draw the fine edges along the meu items
        /// that are colored by the stroke color.
        /// </summary>
        private Rectangle _strokePanel;

        /// <summary>
        /// 
        /// </summary>
        private Rectangle _subMenuPanel;

        // If set, the menu item has the ability to be checked.
        public bool IsCheckable;

        // If IsCheckable is true, this will toggle a check mark when a menu item is selected.
        public bool IsChecked;

        /// <summary>
        /// May become Public used to set up a Fill property later as described by the user.
        /// </summary>
        public Brush Fill
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
        public Brush Stroke
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

        /// <summary>
        /// True each time a sub menu is opened.
        /// </summary>
        protected bool subMenuOpened;

        /// <summary>
        /// True when the sub menu key ( a string value ) has been added to the 
        /// _openSubMenuDictionary containing currently open sub menu items.
        /// </summary>
        protected bool _subMenuKeyAdded;

        /// <summary>
        /// True when all of the menu items have been set.
        /// </summary>
        protected bool _lateInitialize;

        /// <summary>
        /// This holds the value of the greatest menu item width so
        /// that all widths can be set to this one standard.
        /// </summary>
        protected int _longestWidth;

        /// <summary>
        /// The vector position used to place the text of the menu item.
        /// </summary>
        private Vector2 _textPos;

        /// <summary>
        /// True when the header font changes from default, signifying that 
        /// its height must be recalculated.
        /// </summary>
        private bool _headerHeightRecalculated;

        /// <summary>
        /// Width allowed for a check mark to the left of the text.
        /// </summary>
        private int _checkMarkWidth;

        /// <summary>
        /// True when all of the menu item variable settings have been made.
        /// </summary>
        private bool _menuItemVariablesFinalized;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuItem(Game game)
            : base(game)
        {

            this._foregroundColor = Color.White;
            _bc = new System.Windows.Media.BrushConverter();
            _stroke = Color.Black;
            _strokeThickness = 1;
            _strokePanel = new Rectangle();
            _subMenuPanel = new Rectangle();
            Items = new List<XAMLiteMenuItem>();
            _longestWidth = 0;
            _checkMarkWidth = 30;
            Visible = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseDown += new System.Windows.Input.MouseButtonEventHandler(XAMLiteMenuItem_MouseDown);
        }

        /// <summary>
        /// Loads some of the content for the menu items.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.Spacing = 2;
            
            RecalculateWidthAndHeight(this.Header);
            Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        /// <param name></param>
        private void LateInitialize()
        {
            _lateInitialize = true;

            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Game.Components.Add(Items[i]);
                }

                CalculateGreatestWidth();

                SetWidthAndHeight();

                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Visible = Visibility.Hidden;
                    Items[i].HorizontalAlignment = this.HorizontalAlignment;
                    Items[i].VerticalAlignment = this.VerticalAlignment;
                }
            }
        }

        /// <summary>
        /// Updates the menu item.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_fontFamilyChanged)
            {
                _fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                this.SpriteFont.Spacing = Spacing;
                RecalculateWidthAndHeight(this.Header);
                MarginChanged = true;
            }

            if (MarginChanged)
            {
                MarginChanged = false;
                Margin = new Thickness(this.Margin.Left, this.Margin.Top, this.Margin.Right, this.Margin.Bottom);
                if (AllMenuTitles.Contains(this.Header))
                {
                    Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.SpriteFont.MeasureString(Text).X + (int)Padding.Left + (int)Padding.Right, this.Height + (int)Padding.Top + (int)Padding.Bottom);
                    _textPos = new Vector2(this.Position.X + (int)this.Padding.Left, this.Position.Y + (int)this.Padding.Top);
                }
                else
                {
                    Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
                    _textPos = new Vector2(this.Position.X + (int)this.Padding.Left + _checkMarkWidth, this.Position.Y + (int)this.Padding.Top);
                }
            }

            if (!_lateInitialize)
            {
                LateInitialize();
            }

            if (!_headerHeightRecalculated)
            {
                _headerHeightRecalculated = true;
                if (AllMenuTitles.Contains(this.Header))
                {
                    Panel = new Rectangle((int)this.Margin.Left, (int)this.Margin.Top, this.Width + (int)Padding.Left + (int)this.Padding.Right, this.Height);
                }
            }

            if (IsEnabled)
            {
                // opens a sub-menu panel, if it exists.
                if (MouseEntered && Items.Count > 0 && this.Visible == Visibility.Visible)
                {
                    subMenuOpened = true;

                    for (int i = 0; i < Items.Count; i++)
                    {
                        Items[i].Visible = Visibility.Visible;
                    }

                    if (!_subMenuKeyAdded)
                    {
                        _subMenuKeyAdded = true;
                        if (OpenSubMenuDictionary.ContainsKey(this.Header))
                        {
                            OpenSubMenuDictionary.Remove(this.Header);
                            OpenSubMenuDictionary.Add(this.Header, true);
                        }
                    }
                }
                else
                {
                    if (!subMenuOpened)
                    {
                        for (int i = 0; i < Items.Count; i++)
                        {
                            Items[i].Visible = Visibility.Hidden;
                        }
                    }
                    else if (Items.Count > 0 && !_subMenuPanel.Contains(MsRect))
                    {
                        subMenuOpened = false;
                        _subMenuKeyAdded = false;
                        if (OpenSubMenuDictionary.ContainsKey(this.Header))
                        {
                            OpenSubMenuDictionary.Remove(this.Header);
                            OpenSubMenuDictionary.Add(this.Header, false);
                        }
                    }
                }

                // closes sub-menu panel after a menu item has been selected.
                if (Ms.LeftButton == ButtonState.Pressed)
                {
                    if (_subMenuPanel.Contains(MsRect))
                    {
                        subMenuOpened = false;
                        _subMenuKeyAdded = false;
                        OpenSubMenuDictionary.Remove(this.Header);
                        OpenSubMenuDictionary.Add(this.Header, false);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible && IsEnabled)
            {
                if (Items.Count > 0 && !_menuItemVariablesFinalized)
                {
                    _menuItemVariablesFinalized = true;
                    CalculateGreatestWidth();
                    SetWidthAndHeight();
                }

                SpriteBatch.Begin();

                if (!_transparent)
                {
                    if (AllMenuTitles.Contains(this.Header))
                    {
                        if (MouseEntered)
                        {
                            SpriteBatch.Draw(Pixel, Panel, Color.LightGray * 0.25f);
                        }
                        else
                        {
                            SpriteBatch.Draw(Pixel, Panel, this._backgroundColor);
                        }
                    }
                    if (!AllMenuTitles.Contains(this.Header))
                    {
                        float opacity = (float)Opacity * 0.45f;

                        // drawing the slightly transparent offset background.
                        Rectangle ghostRect = new Rectangle(Panel.X + 5, Panel.Y + 5, Panel.Width, Panel.Height);
                        SpriteBatch.Draw(Pixel, ghostRect, (Color.Black * opacity));

                        // highlights the hovered menu item.
                        if (MouseEntered || (_subMenuPanel.Contains(MsRect) && this.Items[0].Visible == Visibility.Visible))
                        {
                            SpriteBatch.Draw(Pixel, Panel, this._backgroundColor);
                            SpriteBatch.Draw(Pixel, Panel, Color.LightGray * 0.35f);

                            // borders the top of the menu item
                            _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left,
                                    (int)this.Position.Y, Panel.Width, _strokeThickness);
                            this.SpriteBatch.Draw(Pixel, _strokePanel, (_stroke * (float)Opacity));

                            // borders the bottom of the menu item.
                            _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left,
                                ((int)this.Position.Y + this.Height - _strokeThickness), Panel.Width, _strokeThickness);
                            this.SpriteBatch.Draw(Pixel, _strokePanel, (_stroke * (float)Opacity));

                            // borders the left side of the menu item.
                            _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left,
                                (int)this.Position.Y, _strokeThickness, Panel.Height);
                            this.SpriteBatch.Draw(Pixel, _strokePanel, (_stroke * (float)Opacity));

                            // borders the right side of the menu item.
                            _strokePanel = new Rectangle(((int)this.Position.X - (int)this.Padding.Left +
                                Panel.Width - _strokeThickness), (int)this.Position.Y, _strokeThickness, Panel.Height);
                            this.SpriteBatch.Draw(Pixel, _strokePanel, (_stroke * (float)Opacity));
                        }
                        else
                        {
                            if (!AllMenuTitles.Contains(this.Header))
                            {
                                SpriteBatch.Draw(Pixel, Panel, this._backgroundColor);

                                // borders the left side of the menu item.
                                _strokePanel = new Rectangle((int)this.Position.X - (int)this.Padding.Left,
                                    (int)this.Position.Y, _strokeThickness, Panel.Height);
                                this.SpriteBatch.Draw(Pixel, _strokePanel, (_stroke * (float)Opacity));

                                // borders the right side of the menu item.
                                _strokePanel = new Rectangle(((int)this.Position.X - (int)this.Padding.Left +
                                    Panel.Width - _strokeThickness), (int)this.Position.Y, _strokeThickness, Panel.Height);
                                this.SpriteBatch.Draw(Pixel, _strokePanel, (_stroke * (float)Opacity));
                            }
                        }
                    }

                    if (AllSubMenuTitles.Contains(this.Header))
                    {
                        ArrowRect.X = this.Panel.X + Panel.Width - (Arrow.Width + 5);
                        ArrowRect.Y = this.Panel.Y + this.Height / 3;
                        this.SpriteBatch.Draw(Arrow, ArrowRect, (Color.White * (float)Opacity));
                    }
                }

                if (subMenuOpened)
                {
                    SpriteBatch.Draw(Pixel, _subMenuPanel, Color.Black);
                }

                SpriteBatch.DrawString(this.SpriteFont, Text, _textPos, (this._foregroundColor * (float)Opacity));

                if (IsChecked)
                {
                    CheckMarkRect = new Rectangle((int)this.Position.X + 5, (int)this.Position.Y, CheckMark.Width, CheckMark.Height);
                    this.SpriteBatch.Draw(CheckMark, CheckMarkRect, (Color.White * (float)Opacity));
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// When a font changes from the default, longest width of all
        /// menu items must be calculated again so that all menu items
        /// share the same width.
        /// </summary>
        private void CalculateGreatestWidth()
        {
            _longestWidth = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Width += (int)Items[i].Padding.Left + (int)Items[i].Padding.Right + _checkMarkWidth;

                if (_longestWidth <= Items[i].Width)
                {
                    _longestWidth = Items[i].Width;
                }
            }
        }

        /// <summary>
        /// When a font changes from the default, the width and height must be
        /// reset.
        /// </summary>
        private void SetWidthAndHeight()
        {
            int height = 0;
            height = (int)this.Margin.Top;
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Width = _longestWidth;
                Items[i].Height += (int)Items[i].Padding.Top + (int)Items[i].Padding.Bottom;
                Items[i].Margin = new Thickness(this.Margin.Left + this.Width, height, this.Margin.Right, this.Margin.Bottom);
                height += Items[i].Height;
            }

            _subMenuPanel = new Rectangle((int)this.Position.X + this.Width, (int)this.Position.Y - 1, _longestWidth, height - (int)this.Margin.Top + 2);
        }

        /// <summary>
        /// Handles when a mouse down event occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void XAMLiteMenuItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // set bool to toggle check marks if IsCheckable on mouse down.
            if (IsCheckable)
            {
                if (IsChecked)
                {
                    IsChecked = false;
                }
                else
                {
                    IsChecked = true;
                }
            }

            // causes the menu to close when a menu item is clicked, and it is not the head
            // of the menu.
            if (!AllSubMenuTitles.Contains(this.Header) && !AllMenuTitles.Contains(this.Header))
            {
                MenuVisibilityCount = 0;
            }

            // HACK: When a tutorial is selected, all Menu Title Headers are erased, so currently 
            // they are being manually added again.
            if (this.Header.Contains("Tutorial") || this.Header.Contains("Toggle Particle Counter"))
            {
                ResetMenuItems();
            }
        }
    }
}
