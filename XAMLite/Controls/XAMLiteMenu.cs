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
    public class XAMLiteMenu : XAMLiteControl
    {
        /// <summary>
        /// Holds a list of all menu items specific to this menu.
        /// </summary>
        public List<XAMLiteMenuItem> Items;
        private int lastItemsCount;

        private int longestWidth;

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
                    _transparent = true;
                else
                    _transparent = false;
            }
        }

        private bool _transparent;

        private bool _setMenuItems;

        private bool _mouseReleased;
        private bool _closeMenu;

        private bool _fullMenuIsVisible;
        private bool _menuVisibilityCounted;

        BrushConverter bc;

        Rectangle _menuItemPanel;

        public XAMLiteMenu(Game game)
            : base(game)
        {
            Items = new List<XAMLiteMenuItem>();
            bc = new System.Windows.Media.BrushConverter();
            longestWidth = 0;
            _mouseReleased = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // setting up the menu
            if (!_setMenuItems)
                setMenuItems(gameTime);
            
            if(_closeMenu)
                closeMenu();

            // Sets visibility to hidden for menu items when the mouse is not over the menu.
            if (_panel.Contains(_msRect) || (_menuItemPanel.Contains(_msRect) && _fullMenuIsVisible) || (_fullMenuIsVisible && _subMenuSelected))
            {
                if (_menuSelected)
                {
                    openMenu();
                }
            }
            else
            {
                closeMenu();
            }

            // handles mouse clicks on the head of the menu, making the menu visible or
            // hidden.
            if (_mouseDown && _panel.Contains(_msRect) && _mouseReleased)
            {
                _mouseReleased = false;
                
                if (Items[1] != null && Items[1].Visible == Visibility.Hidden)
                {
                    openMenu();
                    _menuSelected = true;
                }
                else
                {
                    closeMenu();
                }
            }
       
            // notifies that a full click has occurred and allows the menu to be selected again, thus making
            // it visibile or hidden.
            if (!_mouseReleased && _mouseUp)
                _mouseReleased = true;

            // Notifies that the menu must be closed.  Visibility not immediately changed here because it would
            // cause XAMLiteControl to believe that a hidden menu was selected on _mouseDown, thus changing the
            // mouse down event to false before it is fired.
            if (_mouseDown && _menuItemPanel.Contains(_msRect) && !_subMenuSelected)
            {
                _closeMenu = true;
            }

            // updating the menu visibility count.  If zero, the user must make a mouse down event
            // in order to reopen any menu.  If greater than zero, the menus will open with a mouse
            // enter event.
            if (!_fullMenuIsVisible && _menuVisibilityCounted)
            {
                _menuVisibilityCounted = false;
                _menuVisibilityCount--;
            }

            else if (_fullMenuIsVisible && !_menuVisibilityCounted)
            {
                _menuVisibilityCounted = true;
                _menuVisibilityCount++;
            }

            else if (_menuVisibilityCount == 0)
                _menuSelected = false;

            if (_fullMenuIsVisible)
            {
                Items[0].Background = (System.Windows.Media.Brush)bc.ConvertFrom("#cccccc");
            }
            else
                Items[0].Background = Brushes.Transparent;
        }

        /// <summary>
        /// 
        /// </summary>
        private void closeMenu()
        {
            _closeMenu = false;
            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Visible = Visibility.Hidden;
            }
            _fullMenuIsVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void openMenu()
        {
            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Visible = Visibility.Visible;
            }
            _fullMenuIsVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                if (!_transparent)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(_pixel, _panel, (_backgroundColor * (float)Opacity));
                    spriteBatch.End();
                }
            }
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        /// <param name></param>
        protected void setMenuItems(GameTime gameTime)
        {
            _setMenuItems = true;
            lastItemsCount = Items.Count;

            // add the menu items to the game components
            for (int i = 0; i < Items.Count; i++)
            {
                this.Game.Components.Add(Items[i]);
                // Add the child component to the game with the modified parameters.
                if (Items[i].Items.Count > 0)
                    _allSubMenuTitles.Add(Items[i].Header);
            }

            this.Width = Items[0].Width + 20;
            this.Height = Items[0].Height + 20;
            Items[0].Width = this.Width;

            // if it's a submenu, it will need room for the white arrow, so the width must be increased to accommodate
            for (int i = 1; i < Items.Count; i++)
            {
                // adding space for the arrow to denote that a submenu is available.
                if (_allSubMenuTitles.Contains(Items[i].Header))
                {
                    Items[i].Width += 20;
                }
            }

            // determining what the width for all the menu items should be, not including the header of
            // the menu.
            for (int i = 1; i < Items.Count; i++)
            {
                if (longestWidth <= Items[i].Width)
                    longestWidth = Items[i].Width;
            }

            // setting the width for all the menu items, not including the header
            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Width = longestWidth + 40;
                Items[i].Height = Items[0].Height;
            } 
            
            // adding the head of the menu to the list of menus
            _allMenuTitles.Add(Items[0].Header);

            // setting basic parameters of the menu items
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Padding = new Thickness(30, 0, 10, 0);
                Items[i].HorizontalAlignment = this.HorizontalAlignment;
                Items[i].VerticalAlignment = this.VerticalAlignment;

                if (i == 0)
                {
                    Items[i].Margin = new Thickness(this.Margin.Left + Items[0].Padding.Left, this.Margin.Top + Items[0].Padding.Top, this.Margin.Right + Items[0].Padding.Right, this.Margin.Bottom + Items[0].Padding.Bottom);
                    Items[i].Background = Brushes.Transparent;
                }
                else
                {
                    Items[i].Margin = new Thickness(this.Margin.Left + Items[0].Padding.Left, (this.Margin.Top + Items[i].Height * i) + Items[0].Padding.Top, Items[i].Margin.Right + Items[0].Padding.Right, this.Margin.Bottom + Items[0].Padding.Bottom);
                    if(!_allMenuTitles.Contains(Items[i].Header))
                        Items[i].Background = Brushes.Black;
                    Items[i].Visible = Visibility.Hidden;
                }
            }

            // creating the rectangles for determining mouse activities
            _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, Items[0].Height);
           _menuItemPanel = new Rectangle((int)this.Position.X, (int)this.Position.Y + Items[0].Height, longestWidth + 10, Items[0].Height * (Items.Count - 1));
        }
    }
}
