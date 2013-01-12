﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{ 
    /// <summary>
    /// TODO: This probably should be either a XAMLiteImage or a XAMLiteGrid.
    /// </summary>
    public class XAMLiteMenuNew : XAMLiteGridNew
    {
        /// <summary>
        /// List of menu items, if any.  Each item that is added to the menu 
        /// class, is positioned horizontally across, starting from the left
        /// and makes up the different selectable menus for the menu bar.
        /// </summary>
        public Items Items;

        /// <summary>
        /// Returns whether there are Items in the list of Items.
        /// </summary>
        public bool HasItems
        {
            get
            {
                return Items.Count > 0;
            }
        }

        /// <summary>
        /// When this does not equal Items.Count, a method is called to add the
        /// additional assets to the Game Components.
        /// </summary>
        private int _itemsIndex;

        /// <summary>
        /// The brightness of the Upper portion of the menu bar.
        /// </summary>
        public int UpperGradientBrightness;

        /// <summary>
        /// The brightness of the lower portion of the menu bar.
        /// </summary>
        public int LowerGradientBrightness;

        /// <summary>
        /// The top highlight portion of the background.
        /// </summary>
        private XAMLiteImageNew _gradientTop;

        /// <summary>
        /// The bottom highlight portion of the background.
        /// </summary>
        private XAMLiteImageNew _gradientBottom;

        /// <summary>
        /// The background of the menu that the gradient then gets placed over.
        /// </summary>
        private XAMLiteRectangleNew _background;

        /// <summary>
        /// Remains true, until the Items have been added to the grid and 
        /// therefore, the label now has a height and can be positioned 
        /// inside the ListBox.
        /// </summary>
        private bool _needToUpdate = true;

        /// <summary>
        /// True when the control contains the mouse.
        /// </summary>
        public bool IsMouseOver { get; set; }

        /// <summary>
        /// The private border color.
        /// </summary>
        private Brush _borderBrush;

        /// <summary>
        /// The Border color.
        /// </summary>
        public Brush BorderBrush
        {
            get
            {
                return _borderBrush;
            }

            set
            {
                _borderBrush = value;

                if (_background == null)
                {
                    return;
                }

                _background.Stroke = value;

                if (_borderRectangles.Count <= 1)
                {
                    return;
                }

                for (var i = 1; i < _borderRectangles.Count; i++)
                {
                    _borderRectangles[i].Fill = value;
                }
            }
        }

        /// <summary>
        /// Contains all of the XAMLiteRectangles that make up the border.
        /// </summary>
        private readonly List<XAMLiteRectangleNew> _borderRectangles;

        /// <summary>
        /// The thickness of the border.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// True when the border thickness is of a uniform size.
        /// </summary>
        private bool _isBorderThicknessEqual;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuNew(Game game)
            : base(game)
        {
            Items = new Items(this);
            Width = 200;
            Height = 28;
            Background = Brushes.DarkGray;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            UpperGradientBrightness = 175;
            LowerGradientBrightness = 175;
            BorderBrush = Brushes.Black;

            _borderRectangles = new List<XAMLiteRectangleNew>();
        }

        /// <summary>
        /// Loads all of the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // determines whether all borders are of uniform size.
            _isBorderThicknessEqual = BorderThickness.Left == BorderThickness.Right
                                          && BorderThickness.Right == BorderThickness.Top
                                          && BorderThickness.Top == BorderThickness.Bottom;
            _background = new XAMLiteRectangleNew(Game)
                {
                    Width = Width,
                    Height = Height,
                    Fill = Background,
                    Stroke = BorderBrush,
                    StrokeThickness = _isBorderThicknessEqual ? BorderThickness.Left : 0,
                    DrawOrder = DrawOrder
                };
            _borderRectangles.Add(_background);

            if (!_isBorderThicknessEqual)
            {
                SetBorders();
            }

            foreach (var borderRectangle in _borderRectangles)
            {
                Children.Add(borderRectangle);
            }

            var m = Margin;
            _gradientTop = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 5, Height, Background == Brushes.DarkGray ? 180 : UpperGradientBrightness))
                {
                    Width = Width,
                    Height = Height,
                    Margin = new Thickness(m.Left + BorderThickness.Left, m.Top + BorderThickness.Top, m.Right, m.Bottom),
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    RenderTransform = RenderTransform.FlipVertical
                };
            Children.Add(_gradientTop);

            _gradientBottom = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 5, Height, Background == Brushes.DarkGray ? 165 : LowerGradientBrightness))
            {
                Width = Width - (int)BorderThickness.Left - (int)BorderThickness.Right,
                Height = Height - (int)BorderThickness.Top - (int)BorderThickness.Bottom,
                Margin = new Thickness(m.Left + BorderThickness.Left, m.Top + BorderThickness.Top, m.Right, m.Bottom),
                Background = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Children.Add(_gradientBottom);
        }

        private void SetBorders()
        {
            var leftBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = (int)BorderThickness.Left,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
            };
            _borderRectangles.Add(leftBorder);

            var rightBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = (int)BorderThickness.Right,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
            };
            _borderRectangles.Add(rightBorder);

            var topBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = Width - 2,
                Height = (int)BorderThickness.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
            };
            _borderRectangles.Add(topBorder);

            var bottomBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = Width - 2,
                Height = (int)BorderThickness.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, Height, 0, 0),
                DrawOrder = DrawOrder
            };
            _borderRectangles.Add(bottomBorder);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_itemsIndex != Items.Count)
            {
                LoadItems();
            }

            base.Update(gameTime);

            if (_needToUpdate)
            {
                UpdateItems();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadItems()
        {
            for (var i = _itemsIndex; i < Items.Count; i++)
            {
                Console.WriteLine("Adding Child: " + i);
                Children.Add(Items[i]);
            }

            _needToUpdate = true;
        }

        private void UpdateItems()
        {
            for (var i = _itemsIndex; i < Items.Count; i++)
            {
                var item = (XAMLiteMenuItemNew)Items[i];

                var margin = item.Margin;

                double leftMargin = 0;
                if (i == 0)
                {
                    leftMargin = Items[0].Margin.Left + BorderThickness.Left;
                }
                if (i > 0)
                {
                    leftMargin += Items[i - 1].Margin.Left + Items[i - 1].Width;
                }

                item.UpdateMarginAndWidth(new Thickness(leftMargin, margin.Top, margin.Right, margin.Bottom));
                Console.WriteLine("Setting margin: " + i);
            }

            _needToUpdate = false;

            _itemsIndex = Items.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            foreach (var child in Children)
            {
                child.Dispose();
            }
        }
    }
}
