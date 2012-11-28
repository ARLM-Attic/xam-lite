using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    using System;
    using System.Windows.Input;

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
            }
        }

        /// <summary>
        /// character spacing
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// The color of the text.
        /// </summary>
        protected Color ForegroundColor;

        /// <summary>
        /// The color of the text.
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

                _transparent = value == Brushes.Transparent;
            }
        }

        /// <summary>
        /// Amount of space between the text and the edge of the background 
        /// that contains it.
        /// </summary>
        public Thickness Padding;

        /// <summary>
        /// True when the background color of the menu item is transparent.
        /// </summary>
        private bool _transparent;

        /// <summary>
        /// The color drawn along the edges of the menus.
        /// </summary>
        private Color _stroke;

        /// <summary>
        /// The rectangle used to draw the fine edge along the top of the menu 
        /// item that is colored by the stroke color.
        /// </summary>
        private Rectangle _strokePanelTop;

        /// <summary>
        /// The rectangle used to draw the fine edge along the bottom of the 
        /// menu item that is colored by the stroke color.
        /// </summary>
        private Rectangle _strokePanelBottom;

        /// <summary>
        /// The rectangle used to draw the fine edge along the left of the menu 
        /// item that is colored by the stroke color.
        /// </summary>
        private Rectangle _strokePanelLeft;

        /// <summary>
        /// The rectangle used to draw the fine edge along the right of the 
        /// menu item that is colored by the stroke color.
        /// </summary>
        private Rectangle _strokePanelRight;

        /// <summary>
        /// The container for the drop shadow around the menu item.
        /// </summary>
        private Rectangle _dropShadowRect;

        /// <summary>
        /// True when the drop shadow around the menu item has been set.
        /// </summary>
        private bool _dropShadowRectMeasured;

        /// <summary>
        /// The rectangle that contains all of the submenu items of a 
        /// particular menu item, if any.
        /// </summary>
        private Rectangle _subMenuPanel;

        /// <summary>
        /// When true, the menu item has the ability to be checked.
        /// </summary>
        public bool IsCheckable;

        /// <summary>
        /// If IsCheckable is true, this will toggle a check mark when a menu item is selected.
        /// </summary>
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
                new Color(color.R, color.G, color.B, color.A);
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
        private int StrokeThickness { get; set; }

        /// <summary>
        /// If true, the user has clicked on a menu and the menu item should display, unless it is the
        /// Header for the menu, in which case it will always draw.
        /// </summary>
        protected bool draw;

        /// <summary>
        /// True each time a sub menu is opened.
        /// </summary>
        protected bool SubMenuOpened;

        /// <summary>
        /// True when the sub menu key ( a string value ) has been added to the 
        /// _openSubMenuDictionary containing currently open sub menu items.
        /// </summary>
        protected bool SubMenuKeyAdded;

        /// <summary>
        /// True when all of the menu items have been set.
        /// </summary>
        private bool _lateInitialize;

        /// <summary>
        /// This holds the value of the greatest menu item width so
        /// that all widths can be set to this one standard.
        /// </summary>
        protected int LongestWidth;

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
        private readonly int _checkMarkWidth;

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
            ForegroundColor = Color.White;
            _stroke = Color.Black;
            StrokeThickness = 1;
            _subMenuPanel = new Rectangle();
            Items = new List<XAMLiteMenuItem>();
            LongestWidth = 0;
            _checkMarkWidth = 30;
            Visible = Visibility.Hidden;
        }

        /// <summary>
        /// Initializes the menu item.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            ////MouseUp += OnMouseUp;
            MouseDown += OnMouseDown;
        }

        protected void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            MouseDownUpEvent();
        }

        /// <summary>
        /// Loads some of the content for the menu items.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            Spacing = 2;
            
            RecalculateWidthAndHeight(Header);
            Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        private void LateInitialize()
        {
            _lateInitialize = true;

            if (Items.Count > 0)
            {
                foreach (var t in Items)
                {
                    Game.Components.Add(t);
                }

                // space for the arrow to draw.
                Width += 20;

                // Add the header to the list of submenus so that its parent
                // menus will not close.
                AllSubMenuTitles.Add(Header);
                if (!OpenSubMenuDictionary.ContainsKey(Header))
                {
                    OpenSubMenuDictionary.Add(Header, false);
                }

                CalculateGreatestWidth();

                SetWidthAndHeight();

                foreach (var t in Items)
                {
                    t.Visible = Visibility.Hidden;
                    t.HorizontalAlignment = HorizontalAlignment;
                    t.VerticalAlignment = VerticalAlignment;
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
                SpriteFont.Spacing = Spacing;
                RecalculateWidthAndHeight(Header);
                MarginChanged = true;
            }

            if (MarginChanged)
            {
                MarginChanged = false;
                Margin = new Thickness(Margin.Left, Margin.Top, Margin.Right, Margin.Bottom);
                if (AllMenuTitles.Contains(Header))
                {
                    Panel = new Rectangle((int)Position.X, (int)Position.Y, (int)SpriteFont.MeasureString(Text).X + (int)Padding.Left + (int)Padding.Right, Height + (int)Padding.Top + (int)Padding.Bottom);
                    _textPos = new Vector2(Position.X + (int)Padding.Left, Position.Y + (int)Padding.Top);
                }
                else
                {
                    Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                    _textPos = new Vector2(Position.X + (int)Padding.Left + _checkMarkWidth, Position.Y + (int)Padding.Top);
                }
            }

            if (!_lateInitialize)
            {
                LateInitialize();
            }

            if (!_headerHeightRecalculated)
            {
                _headerHeightRecalculated = true;
                if (AllMenuTitles.Contains(Header))
                {
                    Panel = new Rectangle((int)Margin.Left, (int)Margin.Top, Width + (int)Padding.Left + (int)Padding.Right, Height);
                }
            }

            if (IsEnabled)
            {
                // opens a sub-menu panel, if it exists.
                if (MouseEntered && Items.Count > 0 && Visible == Visibility.Visible)
                {
                    SubMenuOpened = true;

                    foreach (var t in Items)
                    {
                        t.Visible = Visibility.Visible;
                    }

                    if (!SubMenuKeyAdded)
                    {
                        SubMenuKeyAdded = true;
                        if (OpenSubMenuDictionary.ContainsKey(Header))
                        {
                            OpenSubMenuDictionary.Remove(Header);
                            OpenSubMenuDictionary.Add(Header, true);
                        }
                    }
                }
                else
                {
                    if (!SubMenuOpened)
                    {
                        foreach (var t in Items)
                        {
                            t.Visible = Visibility.Hidden;
                        }
                    }
                    else if (Items.Count > 0 && !_subMenuPanel.Contains(MsRect))
                    {
                        // Check whether the submenu has a submenu.  If so,
                        // don't close the menu.
                        foreach (var xamLiteMenuItem in Items)
                        {
                            if (xamLiteMenuItem.Items.Count > 0 && xamLiteMenuItem.Items[0].Visible == Visibility.Visible)
                            {
                                return;
                            }
                        }

                        SubMenuOpened = false;
                        SubMenuKeyAdded = false;
                        if (OpenSubMenuDictionary.ContainsKey(Header))
                        {
                            OpenSubMenuDictionary.Remove(Header);
                            OpenSubMenuDictionary.Add(Header, false);
                        }
                    }
                }

                // closes sub-menu panel after a menu item has been selected.
                if (Ms.LeftButton == ButtonState.Pressed)
                {
                    if (Math.Abs(MousePressPosition.X - Ms.X) < 0.01 && Math.Abs(MousePressPosition.Y - Ms.Y) < 0.01)
                    {
                        if (_subMenuPanel.Contains(MsRect))
                        {
                            SubMenuOpened = false;
                            SubMenuKeyAdded = false;
                            OpenSubMenuDictionary.Remove(Header);
                            OpenSubMenuDictionary.Add(Header, false);
                        }
                    }                    
                }
            }
        }

        /// <summary>
        /// Draws the Menu Item.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == Visibility.Visible)
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
                    if (AllMenuTitles.Contains(Header))
                    {
                        if (MouseEntered)
                        {
                            SpriteBatch.Draw(Pixel, Panel, Color.LightGray * 0.25f);
                        }
                        else
                        {
                            SpriteBatch.Draw(Pixel, Panel, _backgroundColor);
                        }
                    }

                    if (!AllMenuTitles.Contains(Header))
                    {
                        var opacity = (float)Opacity * 0.45f;

                        // drawing the drop shadow.
                        if (!_dropShadowRectMeasured)
                        {
                            MeasureGhostRect();
                        }
                        
                        SpriteBatch.Draw(Pixel, _dropShadowRect, Color.Black * opacity);

                        // highlights the hovered menu item.
                        if (MouseEntered || (_subMenuPanel.Contains(MsRect) && Items[0].Visible == Visibility.Visible))
                        {
                            SpriteBatch.Draw(Pixel, Panel, _backgroundColor);
                            SpriteBatch.Draw(Pixel, Panel, Color.LightGray * 0.35f);

                            // borders the top of the menu item
                            SpriteBatch.Draw(Pixel, _strokePanelTop, _stroke * (float)Opacity);

                            // borders the bottom of the menu item.
                            SpriteBatch.Draw(Pixel, _strokePanelBottom, _stroke * (float)Opacity);

                            // borders the left side of the menu item.
                            SpriteBatch.Draw(Pixel, _strokePanelLeft, _stroke * (float)Opacity);

                            // borders the right side of the menu item.
                            SpriteBatch.Draw(Pixel, _strokePanelRight, _stroke * (float)Opacity);
                        }
                        else
                        {
                            if (!AllMenuTitles.Contains(Header))
                            {
                                SpriteBatch.Draw(Pixel, Panel, _backgroundColor);

                                SpriteBatch.Draw(Pixel, _strokePanelLeft, _stroke * (float)Opacity);

                                SpriteBatch.Draw(Pixel, _strokePanelRight, _stroke * (float)Opacity);
                            }
                        }
                    }

                    if (AllSubMenuTitles.Contains(Header))
                    {
                        ArrowRect.X = Panel.X + Panel.Width - (Arrow.Width + 5);
                        ArrowRect.Y = (Panel.Y + (Height / 2)) - (ArrowRect.Height / 2);
                        SpriteBatch.Draw(Arrow, ArrowRect, Color.White * (float)Opacity);
                    }
                }

                if (SubMenuOpened)
                {
                    SpriteBatch.Draw(Pixel, _subMenuPanel, Color.Black);
                }

                if (IsEnabled)
                {
                    SpriteBatch.DrawString(SpriteFont, Text, _textPos, ForegroundColor * (float)Opacity);
                }
                else
                {
                    SpriteBatch.DrawString(SpriteFont, Text, _textPos, ForegroundColor * 0.3f);
                }

                if (IsChecked)
                {
                    MeasureCheckMark();

                    SpriteBatch.Draw(CheckMark, CheckMarkRect, Color.White * (float)Opacity);
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
            LongestWidth = 0;

            foreach (var t in Items)
            {
                t.Width += (int)t.Padding.Left + (int)t.Padding.Right + _checkMarkWidth;

                if (LongestWidth <= t.Width)
                {
                    LongestWidth = t.Width;
                }
            }
        }

        /// <summary>
        /// Measures the rectangle to contain the check mark.
        /// </summary>
        private void MeasureCheckMark()
        {
            CheckMarkRect.X = (int)Position.X + 5;
            CheckMarkRect.Y = (int)Position.Y;
        }

        /// <summary>
        /// Measures the ghost rectangle that traces all of the menu items.
        /// </summary>
        private void MeasureGhostRect()
        {
            _dropShadowRectMeasured = true;

            _dropShadowRect = new Rectangle(Panel.X + 5, Panel.Y + 5, Panel.Width, Panel.Height);
        }

        /// <summary>
        /// When a font changes from the default, the width and height must be
        /// reset.
        /// </summary>
        private void SetWidthAndHeight()
        {
            var height = (int)Margin.Top;
            foreach (var t in Items)
            {
                t.Width = LongestWidth;
                t.Height += (int)t.Padding.Top + (int)t.Padding.Bottom;
                t.Margin = new Thickness(Margin.Left + Width, height, Margin.Right, Margin.Bottom);
                height += t.Height;
            }

            _subMenuPanel = new Rectangle((int)Position.X + Width, (int)Position.Y - 1, LongestWidth, height - (int)Margin.Top + 2);

            _strokePanelTop = new Rectangle((int)Position.X - (int)Padding.Left,
                                    (int)Position.Y, Panel.Width, StrokeThickness);

            _strokePanelBottom = new Rectangle((int)Position.X - (int)Padding.Left,
                                (int)Position.Y + Height - StrokeThickness, Panel.Width, StrokeThickness);

            _strokePanelLeft = new Rectangle((int)Position.X - (int)Padding.Left,
                                (int)Position.Y, StrokeThickness, Panel.Height);

            _strokePanelRight = new Rectangle((int)Position.X - (int)Padding.Left +
                                Panel.Width - StrokeThickness, (int)Position.Y, StrokeThickness, Panel.Height);
        }

        /// <summary>
        /// Handles when a mouse down event occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownUpEvent();
        }

        private void MouseDownUpEvent()
        {
            // set bool to toggle check marks if IsCheckable on mouse down.
            if (IsCheckable)
            {
                IsChecked = !IsChecked;
            }

            // causes the menu to close when a menu item is clicked, and it is not the head
            // of the menu.
            if (!AllSubMenuTitles.Contains(Header) && !AllMenuTitles.Contains(Header))
            {
                MenuVisibilityCount = 0;
            }
        }
    }
}
