using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// Contains a list of selectable items.
    /// </summary>
    public class XAMLiteListBox : XAMLiteGridNew
    {
        /// <summary>
        /// True when the list box has been selected anywhere within its
        /// boundaries.  If a ListBoxItem has been previously or newly
        /// selected, the highlight brush color will return to its focused
        /// color.  If False, the highlight brush will be changed to the
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
                    ModifyChildFocusAndHighlightColor();
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the item in the current selection or 
        /// returns negative one (-1) if the selection is empty.
        /// </summary>
        public int SelectedIndex { get; set; }

        /// <summary>
        /// True when the control contains the mouse.
        /// </summary>
        public bool IsMouseOver { get; set; }

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
        /// The text color of ListBoxItems when the
        /// ListBoxItem is not individually defined.
        /// </summary>
        public Brush Foreground { get; set; }

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
        /// Remains true, until the Items have been added to the grid and 
        /// therefore, the label now has a height and can be positioned 
        /// inside the ListBox.
        /// </summary>
        private bool _needToUpdate = true;

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

            // If the thickness is uniform, the ListBox only needs one rectangle
            // to define it.
            _rectangle = new XAMLiteRectangleNew(Game)
            {
                Width = Width,
                Height = Height,
                Fill = Background,
                Stroke = BorderBrush,
                StrokeThickness = _isBorderThicknessEqual ? BorderThickness.Left : 0,
                DrawOrder = DrawOrder
            };
            _borderRectangles.Add(_rectangle);

            if (!_isBorderThicknessEqual)
            {
                SetBorders();
            }

            foreach (var borderRectangle in _borderRectangles)
            {
                Children.Add(borderRectangle);
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
        /// Initializes the event handlers.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            LostFocus += OnLostFocus;
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
        /// Updates the highlight color and focus of the selected ListBox item when the
        /// ListBox focus changes.
        /// </summary>
        protected virtual void ModifyChildFocusAndHighlightColor()
        {
            if (IsFocused)
            {
                return;
            }

            foreach (XAMLiteListBoxItem item in Items)
            {
                if (item.IsSelected)
                {
                    item.ModifySelectedBrush(IsFocused);
                }
            }
        }

        /// <summary>
        /// Adds the items to the grid.
        /// </summary>
        private void LoadItems()
        {
            for (var i = _itemsIndex; i < Items.Count; i++)
            {
                Children.Add(Items[i]);
            }

            _needToUpdate = true;
        }

        /// <summary>
        /// Sets margins, height, width, etc., once the item has been added to
        /// the grid.
        /// </summary>
        protected virtual void UpdateItems()
        {
            for (var i = _itemsIndex; i < Items.Count; i++)
            {
                var item = (XAMLiteListBoxItem)Items[i];
                
                var margin = item.Margin;

                double topMargin = 0;
                if (i == 0)
                {
                    topMargin = Items[0].Margin.Top + BorderThickness.Top;
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

                item.UpdateMarginAndWidth(new Thickness(margin.Left, topMargin, margin.Right, margin.Bottom));
            }

            if (!(this is XAMLiteComboBox))
            {
                UpdateHeight();
            }

            UpdateBorders();
            _needToUpdate = false;

            _itemsIndex = Items.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateHeight()
        {
            var h = 0;
            foreach (var item in Items)
            {
                h += item.Height;
            }

            if (h > Height)
            {
                Height = h + (int)BorderThickness.Top + (int)BorderThickness.Bottom;
            }
        }

        /// <summary>
        /// Updates the borders of the ListBox.
        /// </summary>
        private void UpdateBorders()
        {
            if (_borderRectangles.Count == 1)
            {
                _rectangle.Width = Width;
                _rectangle.Height = Height;
            }
            else
            {
                _rectangle.Width = Width;
                _rectangle.Height = Height;
                _borderRectangles[1].Height = Height;
                _borderRectangles[2].Height = Height;
                _borderRectangles[3].Width = Width;
                _borderRectangles[4].Width = Width;
                _borderRectangles[4].Margin = new Thickness(0, Height - _borderRectangles[4].Height, 0, 0);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateWidth(int width)
        {
            Width = width + (int)BorderThickness.Left + (int)BorderThickness.Right;
        }

        /// <summary>
        /// Deselects all Items except for the item listed at the provided 
        /// index.
        /// </summary>
        /// <param name="index"></param>
        protected internal void DeselectAll(int index)
        {
            if (!IsFocused)
            {
                IsFocused = true;
            }

            foreach (XAMLiteListBoxItem item in Items)
            {
                if (item.Index != index)
                {
                    item.IsSelected = false;
                }
            }
        }

        /// <summary>
        /// Sets IsMouseOver to true when the control is entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            IsMouseOver = true;
        }

        /// <summary>
        /// Sets IsMouseOver to false when the control is exited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            IsMouseOver = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnLostFocus(object sender, EventArgs eventArgs)
        {
            if (!IsFocused)
            {
                return;
            }

            SelectedIndex = -1;
            IsFocused = false;
        }

        protected void UpdateRectangleHeights(int height)
        {
            if (_borderRectangles == null)
            {
                return;
            }

            // Adjust the background rectangle and the borders so that they 
            // are the correct height.
            foreach (var rectangle in _borderRectangles)
            {
                if (rectangle.Height > Height)
                {
                    rectangle.Height = Height - height;
                }
            }
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
