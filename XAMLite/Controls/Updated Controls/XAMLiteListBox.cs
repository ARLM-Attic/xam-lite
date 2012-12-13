using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Contains a list of selectable items.
    /// </summary>
    public class XAMLiteListBox : XAMLiteBaseControl
    {
        /// <summary>
        /// The text color of ListBoxItems when the
        /// ListBoxItem is not individually defined.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// List of XAMLiteListBoxItems that make up the content of the 
        /// ListBox.
        /// </summary>
        public Items Items;

        /// <summary>
        /// When this does not equal Items.Count, a method is called to add the
        /// additional assets to the Game Components.
        /// </summary>
        private int _itemsIndex;

        /// <summary>
        /// The Border color.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// The thickness of the border.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// True when the border thickness is of a uniform size.
        /// </summary>
        private bool _isBorderThicknessEqual;

        /// <summary>
        /// Defines the position of ListBoxItems within the ListBox.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// Contains all of the XAMLiteRectangles that make up the border.
        /// </summary>
        private readonly List<XAMLiteRectangleNew> _borderRectangles;

        /// <summary>
        /// TODO: Consider creating a base class for complex controls
        /// TODO: so that adding all of these parts are not necessary every time.
        /// </summary>
        public override Visibility Visibility
        {
            get
            {
                return base.Visibility;
            }

            set
            {
                base.Visibility = value;

                if (_grid != null)
                {
                    _grid.Visibility = value;
                }
            }
        }

        /// <summary>
        /// The grid that contains all of the XAMLite objects which define the 
        /// ListBox.
        /// </summary>
        private XAMLiteGridNew _grid;

        /// <summary>
        /// Remains true, until the Items have been added to the grid and 
        /// therefore, the label now has a height and can be positioned 
        /// inside the ListBox.
        /// </summary>
        private bool _needToUpdate;

        /// <summary>
        /// When true, the update method stops looking at the Items to see if 
        /// any have 0 heights.
        /// </summary>
        private bool _itemsUpdated;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteListBox(Game game)
            : base(game)
        {
            Height = 100;
            Width = 120;
            Background = Brushes.White;
            Foreground = Brushes.Black;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
            Items = new Items(this);
            _borderRectangles = new List<XAMLiteRectangleNew>();
        }

        /// <summary>
        /// Loads the content of the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // determines whether all borders are of uniform size.
            _isBorderThicknessEqual = BorderThickness.Left == BorderThickness.Right
                                          && BorderThickness.Right == BorderThickness.Top
                                          && BorderThickness.Top == BorderThickness.Bottom;

            _grid = new XAMLiteGridNew(Game)
                {
                    Width = Width,
                    Height = Height,
                    HorizontalAlignment = HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment,
                    Margin = Margin
                };
            Game.Components.Add(_grid);

            // If the thickness is uniform, the ListBox only needs one rectangle
            // to define it.
            var rectangle = new XAMLiteRectangleNew(Game)
            {
                Width = Width,
                Height = Height,
                Fill = Background,
                Stroke = BorderBrush,
                StrokeThickness = _isBorderThicknessEqual ? BorderThickness.Left : 0
            };
            _borderRectangles.Add(rectangle);

            if (!_isBorderThicknessEqual)
            {
                SetBorders();
            }

            foreach (var borderRectangle in _borderRectangles)
            {
                _grid.Children.Add(borderRectangle);
            }
        }

        /// <summary>
        /// When the border thickness are not of uniform size, each is added as
        /// a separate rectangle.
        /// </summary>
        private void SetBorders()
        {
            var leftBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = (int)BorderThickness.Left,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            _borderRectangles.Add(leftBorder);

            var rightBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = (int)BorderThickness.Right,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
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
            };
            _borderRectangles.Add(topBorder);

            var bottomBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = BorderBrush,
                Opacity = Opacity,
                Width = Width - 2,
                Height = (int)BorderThickness.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            _borderRectangles.Add(bottomBorder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            LoadItems();

            base.Update(gameTime);

            if (_needToUpdate)
            {
                UpdateItems();
            }

            if (!_itemsUpdated)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Height == 0)
                    {
                        _needToUpdate = true;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the items to the grid.
        /// </summary>
        private void LoadItems()
        {
            if (_itemsIndex != Items.Count && Items.Count > 0)
            {
                for (var i = _itemsIndex; i < Items.Count; i++)
                {
                    Console.WriteLine(Items[i].ToString());
                    _grid.Children.Add(Items[i]);
                }

                _itemsIndex = Items.Count;
                Console.WriteLine(Items.Count);
            }
        }

        /// <summary>
        /// Sets margins, height, width, etc., once the item has been added to
        /// the grid.
        /// </summary>
        private void UpdateItems()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var margin = Items[i].Margin;

                double topMargin = 0;

                if (i == 0)
                {
                    topMargin = BorderThickness.Top;
                }
                else
                {
                    topMargin += Items[i - 1].Margin.Top + Items[i - 1].Height;
                }

                //Items[i].Width = Width;
                Items[i].UpdateMargin(new Thickness(margin.Left + BorderThickness.Left, margin.Top + topMargin, margin.Right, margin.Bottom));
            }

            _needToUpdate = false;
            _itemsUpdated = true;
        }
    }
}
