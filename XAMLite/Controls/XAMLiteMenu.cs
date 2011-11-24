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
                    transparent = true;
                else
                    transparent = false;
            }
        }

        private bool transparent;

        private bool _setMenuItems;

        private bool alreadyDown;

        private bool fullMenuIsVisible;

        BrushConverter bc;

        Rectangle _menuItemPanel;

        public XAMLiteMenu(Game game)
            : base(game)
        {
            Items = new List<XAMLiteMenuItem>();
            bc = new System.Windows.Media.BrushConverter();
            longestWidth = 0;
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

            if (!_setMenuItems)
                setMenuItems(gameTime);

            if (_mouseEnter)
            {
                Items[0].Background = (System.Windows.Media.Brush)bc.ConvertFrom("#cccccc");

            }

            else if ((_menuItemPanel.Contains(_msRect) || _subMenuSelected) && fullMenuIsVisible)
            {
                Items[0].Background = (System.Windows.Media.Brush)bc.ConvertFrom("#cccccc");
            }
            // this is the problem area here, causing the menu to sometimes shrink up...
            else
            {
                if (alreadyDown == true)
                {
                    _menuSelected = true;
                }
                else
                {
                    _menuSelected = false;
                    fullMenuIsVisible = false;
                }

                alreadyDown = false;

                Items[0].Background = Brushes.Transparent;
                if (Items.Count > 0)
                {
                    this.Height = Items[0].Height;
                    for (int i = 1; i < Items.Count; i++)
                    {
                        Items[i].Visible = Visibility.Hidden;
                    }
                }

                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }

            if ((_mouseEnter && _mouseDown && !alreadyDown) || (_mouseEnter && _menuSelected))
            {
                _menuSelected = true;
                alreadyDown = true;
                fullMenuIsVisible = true;
                for (int i = 1; i < Items.Count; i++)
                {
                    Items[i].Visible = Visibility.Visible;
                }
                this.Height = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    this.Height += Items[i].Height;
                }
                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }
            /*else if (_mouseDown && alreadyDown)
            {
                _menuSelected = false;
                alreadyDown = false;
                fullMenuIsVisible = false;
                for (int i = 1; i < Items.Count; i++)
                {
                    Items[i].Visible = Visibility.Hidden;
                }
                this.Height = Items[0].Height;
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                if (!transparent)
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
        private void setMenuItems(GameTime gameTime)
        {
            _setMenuItems = true;
            lastItemsCount = Items.Count;

            // Add the child component to the game with the modified parameters.
            for (int i = 0; i < Items.Count; i++)
            {
                this.Game.Components.Add(Items[i]);

                // if it is a sub menu, add it to the list of all sub menus
                if (Items[i].Items.Count > 0)
                    _allSubMenuTitles.Add(Items[i]);

                Items[i].Update(gameTime);
            }

            this.Width = Items[0].Width + 20;
            this.Height = Items[0].Height + 20;
            Items[0].Width = this.Width;

            for (int i = 0; i < Items.Count; i++)
            {
                if (_allSubMenuTitles.Contains(Items[i]))
                {
                    Items[i].Width += 20;
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (longestWidth <= Items[i].Width)
                    longestWidth = Items[i].Width;
            }

            for (int i = 1; i < Items.Count; i++)
            {
                Items[i].Width = longestWidth + 20;
                Items[i].Height = Items[0].Height;
            } 
            
            _allMenuTitles.Add(Items[0]);

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Padding = new Thickness(10, 0, 10, 0);
                Items[i].HorizontalAlignment = this.HorizontalAlignment;
                Items[i].VerticalAlignment = this.VerticalAlignment;
                if (i == 0)
                    Items[i].Margin = new Thickness(this.Margin.Left + Items[0].Padding.Left, this.Margin.Top + Items[0].Padding.Top, this.Margin.Right + Items[0].Padding.Right, this.Margin.Bottom + Items[0].Padding.Bottom);
                else
                {
                    Items[i].Margin = new Thickness(this.Margin.Left + Items[0].Padding.Left, (this.Margin.Top + Items[i].Height * i) + Items[0].Padding.Top, Items[i].Margin.Right + Items[0].Padding.Right, this.Margin.Bottom + Items[0].Padding.Bottom);
                }
            }

            _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, Items[0].Height);
           _menuItemPanel = new Rectangle((int)this.Position.X, (int)this.Position.Y + Items[0].Height, longestWidth + 10, this.Height);
        }
    }
}
