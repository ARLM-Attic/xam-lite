using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteMenu : XAMLiteControl
    {
        /// <summary>
        /// Width allowed for a check mark to the left of the text.
        /// </summary>
        private readonly int _checkMarkWidth;

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
        /// The background color of the menu.
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

        private bool _transparent;

        /// <summary>
        /// True when all of the menu items have been set.
        /// </summary>
        private bool _lateInitialize;

        /// <summary>
        /// True when a menu is open.
        /// </summary>
        private bool _fullMenuOpen;

        /// <summary>
        /// 
        /// </summary>
        private bool _menuVisibilityCounted;

        /// <summary>
        /// Used to determine whether the mouse is contained in an open menu.
        /// </summary>
        private Rectangle _menuItemPanel;

        /// <summary>
        /// Used for drawing the background of the panel.
        /// </summary>
        private Rectangle _menuItemsDrawPanel;

        /// <summary>
        /// 
        /// </summary>
        private bool _menuItemVariablesFinalized;

        /// <summary>
        /// True when the menu header should become visible.
        /// </summary>
        private bool _headerVisibilityOn;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenu(Game game)
            : base(game)
        {
            Items = new List<XAMLiteMenuItem>();
            _longestWidth = 0;
            _checkMarkWidth = 30;
            IsEnabled = false;
        }

        /// <summary>
        /// Wires the events.
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
            // adding the head of the menu to the list of menus
            AllMenuTitles.Add(Items[0].Header);

            foreach (XAMLiteMenuItem t in Items)
            {
                Game.Components.Add(t);
                // Add the child component to the game with the modified parameters.
                if (t.Items.Count > 0)
                {
                    AllSubMenuTitles.Add(t.Header);
                    if (!OpenSubMenuDictionary.ContainsKey(t.Header))
                    {
                        OpenSubMenuDictionary.Add(t.Header, false);
                    }
                }
            }

            Items[0].Width += (int)Items[0].Padding.Left + (int)Items[0].Padding.Right;
            Items[0].Height += (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom;
            Items[0].Margin = new Thickness(Margin.Left, Margin.Top, Margin.Right, Margin.Bottom);
            
            CalculateGreatestWidth();

            SetWidthAndHeight();

            IsEnabled = true;

            _lateInitialize = true;
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
                        // If the menu was disabled while the application is running
                        // this will hide the menu.
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
                // If the menu header was previously hidden because it was 
                // disabled.
                if (!_headerVisibilityOn)
                {
                    _headerVisibilityOn = true;
                    Items[0].Visible = Visibility.Visible;
                }

                // If neither the header nor an open menu contains the mouse
                // close the menu.
                if (!Panel.Contains(MsRect) && !_menuItemPanel.Contains(MsRect))
                {
                    CloseMenu();
                }

                // updating the menu visibility count.  If zero, the user must 
                // make a mouse down event in order to reopen any menu.  If 
                // greater than zero, the menus will open with a mouse enter 
                // event.
                if (!_fullMenuOpen && _menuVisibilityCounted)
                {
                    _menuVisibilityCounted = false;
                    if (MenuVisibilityCount != 0)
                    {
                        MenuVisibilityCount--;
                    }
                }
                else if (_fullMenuOpen && !_menuVisibilityCounted)
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
                        MenuShouldAutoOpen = false;
                    }
                }

                if (MouseEntered)
                {
                    SetHeaderWidthAndHeight();
                    Background = Brushes.LightGray;
                }
                else if (_fullMenuOpen)
                {
                    /*if (!_menuItemVariablesFinalized)
                    {
                        _menuItemVariablesFinalized = true;

                        CalculateGreatestWidth();
                        SetWidthAndHeight();
                    }*/

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

                if (_fullMenuOpen)
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

                _fullMenuOpen = false;
            }
        }

        /// <summary>
        /// Opens a menu, making all menu items visible.
        /// </summary>
        private void OpenMenu()
        {
            if (!_menuItemVariablesFinalized)
            {
                _menuItemVariablesFinalized = true;
                CalculateGreatestWidth();
                SetWidthAndHeight();
            }

            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Visible = Visibility.Visible;
            }

            _fullMenuOpen = true;
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
        /// Sets the width and height of the header.
        /// </summary>
        private void SetHeaderWidthAndHeight()
        {
            Panel = new Rectangle((int)Items[0].Position.X, (int)Items[0].Position.Y, Items[0].Width + (int)Items[0].Padding.Left + (int)Items[0].Padding.Right, Items[0].Height + (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom);
        }

        /// <summary>
        /// When a font changes from the default, the width and height must be
        /// reset.
        /// </summary>
        private void SetWidthAndHeight()
        {
            int height = (int)Margin.Top + Items[0].Height + (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom;
            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Width = _longestWidth;
                Items[i].Height += (int)Items[i].Padding.Top + (int)Items[i].Padding.Bottom;
                Items[i].Margin = new Thickness(Margin.Left, height, Margin.Right, Margin.Bottom);
                Items[i].IsEnabled = true;
                height += Items[i].Height;
            }

            Panel = new Rectangle((int)Items[0].Position.X, (int)Items[0].Position.Y, Items[0].Width + (int)Items[0].Padding.Left + (int)Items[0].Padding.Right, Items[0].Height + (int)Items[0].Padding.Top + (int)Items[0].Padding.Bottom);
            _menuItemPanel = new Rectangle((int)Position.X, (int)Position.Y + Items[0].Height, _longestWidth, height - (Items[0].Height + (int)Margin.Top));
            _menuItemsDrawPanel = new Rectangle((int)Position.X, (int)Items[1].Position.Y - 1, _longestWidth, height - (int)Items[1].Position.Y + 2);
        }

        /// <summary>
        /// Will highlight the Head of the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XamLiteMenuMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (MenuShouldAutoOpen)
            {
                OpenMenu();
            }
        }

        /// <summary>
        /// Will either open or close a menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XamLiteMenuMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Items[1] != null && Items[1].Visible == Visibility.Hidden)
            {
                OpenMenu();
                MenuShouldAutoOpen = true;
            }
            else
            {
                CloseMenu();
            }
        }
    }
}
