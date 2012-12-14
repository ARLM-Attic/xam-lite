using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XAMLite
{
    /// <summary>
    /// Contains a list of selectable items.
    /// </summary>
    public class XAMLiteListBox : XAMLiteBaseControl
    {
        /// <summary>
        /// True when the list box has been selected anywhere within its
        /// boundaries.  If a ListBoxItem has been previously or newly
        /// selected, the highlight brush color will return to its focused
        /// color.  If False, the highlight brush will be changed for the
        /// unfocused brush color.
        /// </summary>
        public override bool IsFocused
        {
            get
            {
                return base.IsFocused;
            }

            set
            {
                base.IsFocused = value;

                if (Focusable)
                {
                    ModifyChildHighlightColor();
                }
            }
        }

        /// <summary>
        /// True when the control contains the mouse.
        /// </summary>
        public bool IsMouseOver { get; set; }

        /// <summary>
        /// The text color of ListBoxItems when the
        /// ListBoxItem is not individually defined.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// List of Items that make up the content of the control.
        /// </summary>
        public Items Items;

        /// <summary>
        /// When this does not equal Items.Count, a method is called to add the
        /// additional assets to the Game Components.
        /// </summary>
        private int _itemsIndex;

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

                if (_rectangle == null)
                {
                    return;
                }

                _rectangle.Stroke = value;

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
        /// The back ground color of the ListBox.
        /// </summary>
        public override Brush Background
        {
            get
            {
                return base.Background;
            }

            set
            {
                base.Background = value;

                if (_rectangle != null)
                {
                    _rectangle.Fill = value;
                }
            }
        }

        /// <summary>
        /// Although not in WPF, this seems essential to override the default
        /// colors in WPF for highlighting on mouse over or when selected.
        /// </summary>
        public Brush SelectedBackground { get; set; }

        /// <summary>
        /// The brush color of a selected ListBoxItem when the ListBox that 
        /// contains it loses focus.
        /// </summary>
        public Brush UnfocusedSelectedBackground { get; set; }

        /// <summary>
        /// Contains all of the XAMLiteRectangles that make up the border.
        /// </summary>
        private readonly List<XAMLiteRectangleNew> _borderRectangles;

        /// <summary>
        /// Background of the ListBox.
        /// </summary>
        private XAMLiteRectangleNew _rectangle;

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
            SelectedBackground = Brushes.CornflowerBlue;
            UnfocusedSelectedBackground = Brushes.DarkGray;
            Foreground = Brushes.Black;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
            Focusable = true;
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
            _rectangle = new XAMLiteRectangleNew(Game)
            {
                Width = Width,
                Height = Height,
                Fill = Background,
                Stroke = BorderBrush,
                StrokeThickness = _isBorderThicknessEqual ? BorderThickness.Left : 0
            };
            _borderRectangles.Add(_rectangle);

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
                VerticalAlignment = VerticalAlignment.Top,
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

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            MouseUp += OnMouseUp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            IsMouseOver = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            IsMouseOver = false;
        }

        /// <summary>
        /// Updates the control.
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
                foreach (var t in Items)
                {
                    if (t.Height == 0)
                    {
                        _needToUpdate = true;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the highlight color of the selected ListBox item when the
        /// ListBox loses focus.
        /// </summary>
        protected virtual void ModifyChildHighlightColor()
        {
            if (Parent is XAMLiteComboBox)
            {
                foreach (XAMLiteComboBoxItem item in Items)
                {
                    if (item.IsSelected)
                    {
                        item.UpdateSelectedBrush(IsFocused);
                    }
                }
            }
            else
            {
                foreach (XAMLiteListBoxItem item in Items)
                {
                    if (item.IsSelected)
                    {
                        item.UpdateSelectedBrush(IsFocused);
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
                    _grid.Children.Add(Items[i]);
                }

                _itemsIndex = Items.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateHeight()
        {
            var h = BorderThickness.Top + BorderThickness.Bottom;

            if (Items == null)
            {
                return;
            }

            foreach (var item in Items)
            {
                h += item.Height;
            }

            if (Height > (int)h)
            {
                // move the bottom rectangle up.
                if (_borderRectangles.Count > 1)
                {
                    var rect = _borderRectangles[_borderRectangles.Count - 1];
                    rect.Margin = new Thickness(
                        rect.Margin.Left, rect.Margin.Top, rect.Margin.Right, rect.Margin.Bottom + (Height - h));
                }

                Height = (int)h;
                _grid.Height = (int)h;

                foreach (var rectangle in _borderRectangles)
                {
                    if (rectangle.Height > Height)
                    {
                        rectangle.Height = Height;
                    } 
                }
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
                var item = Items[i] is XAMLiteComboBoxItem
                               ? (XAMLiteComboBoxItem)Items[i]
                               : (XAMLiteListBoxItem)Items[i];
                var margin = item.Margin;

                double topMargin = 0;

                if (i == 0)
                {
                    topMargin = BorderThickness.Top;
                }
                else
                {
                    topMargin += Items[i - 1].Margin.Top + Items[i - 1].Height;
                }

                if (item.Background == Brushes.Transparent)
                {
                    item.Background = Background;
                }

                if (item.Foreground == Brushes.Transparent)
                {
                    item.Foreground = Foreground;
                }

                if (item.SelectedBackground == Brushes.Transparent)
                {
                    item.SelectedBackground = SelectedBackground;
                }

                if (item.UnfocusedSelectedBackground == Brushes.Transparent)
                {
                    item.UnfocusedSelectedBackground = UnfocusedSelectedBackground;
                }

                item.UpdateMarginAndWidth(new Thickness(margin.Left + BorderThickness.Left, margin.Top + topMargin, margin.Right, margin.Bottom));

                if (Parent is XAMLiteComboBox)
                {
                    UpdateHeight();
                }
            }

            _needToUpdate = false;
            _itemsUpdated = true;
        }

        /// <summary>
        /// Deselects all Items except for the item listed at the provided 
        /// index.
        /// </summary>
        /// <param name="index"></param>
        protected internal void DeselectAll(int index)
        {
            if (Parent is XAMLiteComboBox)
            {
                foreach (XAMLiteComboBoxItem item in Items)
                {
                    if (item.Index != index)
                    {
                        item.IsSelected = false;
                    }
                }
            }
            else
            {
                foreach (XAMLiteListBoxItem item in Items)
                {
                    if (item.Index != index)
                    {
                        item.IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the focus to true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            IsFocused = true;
        }
    }
}
