using System.Windows;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Windows.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteMenu : XAMLiteControl
    {
        /// <summary>
        /// Holds a list of all menu items specific to this menu.
        /// </summary>
        public List<XAMLiteMenuItem> Items;

        /// <summary>
        /// This holds the value of the greatest menu item width so
        /// that all widths can be set to this one standard.
        /// </summary>
        private int _longestWidth;

        /// <summary>
        /// The background color of the menu.
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

                if (value == Brushes.Transparent)
                    _transparent = true;
                else
                    _transparent = false;
            }
        }

        private bool _transparent;

        /// <summary>
        /// True when all of the menu items have been set.
        /// </summary>
        private bool _lateInitialize;

        /// <summary>
        /// True when a menu is open.
        /// </summary>
        private bool _fullMenuIsVisible;

        private bool _menuVisibilityCounted;

        BrushConverter bc;

        /// <summary>
        /// Used to determine whether the mouse is contained in an open menu.
        /// </summary>
        private Rectangle _menuItemPanel;

        /// <summary>
        /// Used for drawing the background of the panel.
        /// </summary>
        private Rectangle _menuItemsDrawPanel;

        /// <summary>
        /// Width allowed for a check mark to the left of the text.
        /// </summary>
        private int _checkMarkWidth;

        private bool _menuItemVariablesFinalized;

        private bool _headerVisibilityOn;

        /// <summary>
        /// True when the width and height of the menu items have been measured
        /// once all the Fonts, etc., hav been set.
        /// </summary>
        //private bool _menuItemsMeasured;

        public XAMLiteMenu(Game game)
            : base(game)
        {
            Items = new List<XAMLiteMenuItem>();
            bc = new BrushConverter();
            _longestWidth = 0;
            _checkMarkWidth = 30;
            IsEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseDown += XamLiteMenuMouseDown;

            MouseEnter += XamLiteMenuMouseEnter;
        }

        /// <summary>
        /// Initializes the menu components.
        /// </summary>
        public void LateInitialize()
        {
            _lateInitialize = true;

            // adding the head of the menu to the list of menus
            AllMenuTitles.Add(Items[0].Header);

            for (int i = 0; i < Items.Count; i++)
            {
                Game.Components.Add(Items[i]);
                // Add the child component to the game with the modified parameters.
                if (Items[i].Items.Count > 0)
                {
                    AllSubMenuTitles.Add(Items[i].Header);
                    if (!OpenSubMenuDictionary.ContainsKey(Items[i].Header))
                        OpenSubMenuDictionary.Add(Items[i].Header, false);
                }
            }

            Items[0].Width += (int)Items[0].Padding.Left + (int)Items[0].Padding.Right;
            Items[0].Height += (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom;
            Items[0].Margin = new Thickness(this.Margin.Left, this.Margin.Top, this.Margin.Right, this.Margin.Bottom);
            
            CalculateGreatestWidth();

            SetWidthAndHeight();

            IsEnabled = true;
        }


        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsEnabled)
            {
                // setting up the menu
                if (!_lateInitialize)
                {
                    LateInitialize();
                }
                else
                {
                    if (_headerVisibilityOn)
                    {
                        CloseMenu();
                        Items[0].Visible = Visibility.Hidden;
                        _headerVisibilityOn = false;
                        _menuVisibilityCounted = false;
                        MenuVisibilityCount = 0;
                    }
                }
            }
            else
            {
                if (!_headerVisibilityOn)
                {
                    _headerVisibilityOn = true;
                    Items[0].Visible = Visibility.Visible;
                }
                if (!Panel.Contains(MsRect) && !_menuItemPanel.Contains(MsRect))
                {
                    CloseMenu();
                }

                // updating the menu visibility count.  If zero, the user must make a mouse down event
                // in order to reopen any menu.  If greater than zero, the menus will open with a mouse
                // enter event.
                if (!_fullMenuIsVisible && _menuVisibilityCounted)
                {
                    _menuVisibilityCounted = false;
                    if (MenuVisibilityCount != 0)
                    {
                        MenuVisibilityCount--;
                    }
                }

                else if (_fullMenuIsVisible && !_menuVisibilityCounted)
                {

                    _menuVisibilityCounted = true;
                    MenuVisibilityCount++;
                }
                else if (MenuVisibilityCount == 0)
                {
                    _menuVisibilityCounted = false;
                    if (!OpenSubMenuDictionary.ContainsValue(true))
                    {
                        CloseMenu();
                        MenuSelected = false;
                    }
                }

                if (_fullMenuIsVisible || MouseEntered)
                {
                    if (!_menuItemVariablesFinalized)
                    {
                        _menuItemVariablesFinalized = true;
                        CalculateGreatestWidth();
                        SetWidthAndHeight();
                    }
                    Background = Brushes.LightGray;
                }
                else
                {
                    Background = Brushes.Transparent;
                    Items[0].Background = Brushes.Transparent;
                }
            }
        }

        /// <summary>
        /// Draws the Header background highlight color when the menu is open
        /// and the mouse is over the menu but not over the header. Also draws
        /// the backdrop for the open menu.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible && IsEnabled)
            {
                SpriteBatch.Begin();
                if (!_transparent)
                {
                    SpriteBatch.Draw(Pixel, Panel, _backgroundColor * 0.55f);
                }

                if (_fullMenuIsVisible)
                {
                    SpriteBatch.Draw(Pixel, _menuItemsDrawPanel, Color.Black);
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Closes a menu.
        /// </summary>
        private void CloseMenu()
        {
            if (!OpenSubMenuDictionary.ContainsValue(true))
            {
                for (int i = 1; i < Items.Count; i++)
                {
                    Items[i].Visible = Visibility.Hidden;
                }
                _fullMenuIsVisible = false;
            }
        }

        /// <summary>
        /// Opens a menu, making all menu items visible.
        /// </summary>
        private void OpenMenu()
        {
            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Visible = Visibility.Visible;
            }
            _fullMenuIsVisible = true;
        }

        /// <summary>
        /// When a font changes from the default, longest width of all
        /// menu items must be calculated again so that all menu items
        /// share the same width.
        /// </summary>
        private void CalculateGreatestWidth()
        {
            _longestWidth = 0;

            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Width += (int)Items[i].Padding.Left + (int)Items[i].Padding.Right + _checkMarkWidth;

                // adding space for the arrow to denote that a submenu is available.
                if (AllSubMenuTitles.Contains(Items[i].Header))
                {
                    Items[i].Width += 20;
                }
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
            height = (int)this.Margin.Top + Items[0].Height + (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom;
            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Width = _longestWidth;
                Items[i].Height += (int)Items[i].Padding.Top + (int)Items[i].Padding.Bottom;
                Items[i].Margin = new Thickness(this.Margin.Left, height, this.Margin.Right, this.Margin.Bottom);
                Items[i].IsEnabled = true;
                height += Items[i].Height;
            }

            Panel = new Rectangle((int)Items[0].Position.X, (int)Items[0].Position.Y, Items[0].Width + (int)Items[0].Padding.Left + (int)Items[0].Padding.Right, Items[0].Height + (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom);
            _menuItemPanel = new Rectangle((int)this.Position.X, (int)this.Position.Y + Items[0].Height, _longestWidth, height - (Items[0].Height + (int)this.Margin.Top));
            _menuItemsDrawPanel = new Rectangle((int)this.Position.X, (int)Items[1].Position.Y - 1, _longestWidth, height - (int)Items[1].Position.Y + 2);
        }

        /// <summary>
        /// Will highlight the Head of the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void XamLiteMenuMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (MenuSelected)
            {
                OpenMenu();
            }
        }

        /// <summary>
        /// Will either open or close a menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void XamLiteMenuMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Items[1] != null && Items[1].Visible == Visibility.Hidden)
            {
                OpenMenu();
                MenuSelected = true;
            }
            else
            {
                CloseMenu();
            }
        }
    }
}
