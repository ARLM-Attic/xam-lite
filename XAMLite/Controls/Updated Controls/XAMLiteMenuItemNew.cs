﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteMenuItemNew : XAMLiteGridNew
    {
        /// <summary>
        /// List of menu items, if any. Each item that is added to another menu 
        /// item, is positioned in one of two ways.  If its parent is one of the
        /// menu items who's parent is the menu, it is drawn immediately below
        /// the menu header.  If the parent of its parent is another menu item,
        /// the the item is drawn to the right side of its parent.
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
        /// When true, the menu item is able to be interacted with.
        /// </summary>
        public override bool IsEnabled
        {
            get
            {
                return base.IsEnabled;
            }

            set
            {
                base.IsEnabled = value;

                if (_label != null)
                {
                    _label.Opacity = value ? 1f : 0.55f;
                }
            }
        }

        /// <summary>
        /// When true, a menu item can have a state of IsChecked or !IsChecked.
        /// </summary>
        public bool IsCheckable;

        /// <summary>
        /// When true, a check mark is visible to the left of the menu item.
        /// </summary>
        public bool IsChecked;

        /// <summary>
        /// The text that makes up the label of the control.
        /// </summary>
        private string _header;

        /// <summary>
        /// The text that makes up the label of the control.
        /// </summary>
        public string Header
        {
            get
            {
                if (_label == null)
                {
                    return _header;
                }

                return _label.Content.ToString();
            }

            set
            {
                _header = value;

                if (_label != null)
                {
                    _label.Content = value;
                }
            }
        }

        /// <summary>
        /// Character spacing.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        protected FontFamily _fontFamily;

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
                FontFamilyChanged = true;
            }
        }

        /// <summary>
        /// True when the font family has changed.
        /// </summary>
        protected bool FontFamilyChanged;

        /// <summary>
        /// The Text color of the Menu Item.
        /// </summary>
        private Brush _foreground;

        /// <summary>
        /// The Text color of the Menu Item.
        /// </summary>
        public Brush Foreground
        {
            get
            {
                return _foreground;
            }

            set
            {
                _foreground = value;

                if (_label == null)
                {
                    return;
                }

                _label.Foreground = value;
            }
        }

        /// <summary>
        /// The default background brush color for all items when a menu is
        /// open.  if a particular menu item has its Background set, this will 
        /// override it.
        /// </summary>
        public Brush ItemsBackground { get; set; }

        /// <summary>
        /// The spacing between the edge of the control and where the text
        /// starts.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// The text portion of the control.
        /// </summary>
        private XAMLiteLabelNew _label;

        /// <summary>
        /// When true, the menu item parent is a XAMLiteMenu.
        /// </summary>
        internal bool IsMenuHead;

        /// <summary>
        /// Index of object in Items as opposed to its grid index.
        /// </summary>
        internal int ItemIndex;

        /// <summary>
        /// The Border color.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// The thickness of the border.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// The background of the control that changes colors when hovered.
        /// </summary>
        private XAMLiteImageNew _highlightedBackground;

        /// <summary>
        /// Displayed when a menu item has a submenu.
        /// </summary>
        private XAMLiteImageNew _arrow;

        /// <summary>
        /// Displayed when a menu item IsCheckable and IsChecked.
        /// </summary>
        private XAMLiteImageNew _checkmark;

        /// <summary>
        /// A specific backdrop color, if one is assigned.
        /// </summary>
        //private XAMLiteRectangleNew _background;

        /// <summary>
        /// The backdrop color for all items in a Menu.
        /// </summary>
        private XAMLiteRectangleNew _backdrop;

        /// <summary>
        /// The drop shadow for an open menu.
        /// </summary>
        private XAMLiteRectangleNew _dropShadow;

        /// <summary>
        /// Although not in WPF, this seems essential to override the default
        /// colors in WPF for highlighting on mouse over or when selected.  
        /// If this is not explicitly set, it will receive the brush color
        /// as specified by the ListBox that contains it.
        /// </summary>
        public Brush HoverBrush { get; set; }

        /// <summary>
        /// The edges of the highlight color.
        /// </summary>
        private List<XAMLiteImageNew> _highlightEdgesHover;

        /// <summary>
        /// When this does not equal Items.Count, a method is called to add the
        /// additional assets to the Game Components.
        /// </summary>
        private int _itemsIndex;

        /// <summary>
        /// Remains true, until the Items have been added to the grid and 
        /// therefore, the label now has a height and can be positioned 
        /// inside the ListBox.
        /// </summary>
        private bool _needToUpdate = true;

        /// <summary>
        /// When true, the items for a menu item are visible.
        /// </summary>
        internal bool IsMenuOpen;

        /// <summary>
        /// Specifically manages the highlights on menu items that have Items.
        /// For example, when its list of Items is visible, the highlight should
        /// always be visible.
        /// </summary>
        private bool _isHighlighted;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuItemNew(Game game)
            : base(game)
        {
            Items = new Items(this);

            Background = Brushes.Transparent;
            ItemsBackground = Brushes.Gainsboro;
            Foreground = Brushes.Transparent;
            HoverBrush = Brushes.Transparent;
            FontFamily = new FontFamily("Verdana14");
            Spacing = 2;
            Padding = new Thickness(10, 4, 10, 5);
            Focusable = true;
            IsEnabled = true;

            _highlightEdgesHover = new List<XAMLiteImageNew>();
        }

        /// <summary>
        /// Loads the content of the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            if (HasItems)
            {
                _dropShadow = new XAMLiteRectangleNew(Game)
                {
                    Width = Width,
                    Height = Height,
                    Fill = Brushes.Black,
                    Visibility = Visibility.Hidden,
                    Opacity = 0.15f
                };
                Children.Add(_dropShadow);

                _backdrop = new XAMLiteRectangleNew(Game)
                {
                    Width = Width,
                    Height = Height,
                    Fill = ItemsBackground,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Visibility = Visibility.Hidden
                };
                Children.Add(_backdrop);
            }

            _label = new XAMLiteLabelNew(Game)
            {
                Content = Header,
                Foreground = Foreground == Brushes.Transparent ? Brushes.Black : Foreground,
                FontFamily = FontFamily,
                Spacing = Spacing,
                Padding = Padding,
                HorizontalAlignment = HorizontalAlignment.Left,
                Opacity = IsEnabled ? 1f : 0.55f
            };
            Game.Components.Add(_label);

            var w = _label.MeasureString().X + Padding.Left + Padding.Right;
            var h = _label.MeasureString().Y + Padding.Top + Padding.Bottom;

            if (w > Width)
            {
                Width = (int)w;
            }

            if (h > Height)
            {
                Height = (int)h;
            }

            Game.Components.Remove(_label);

            // Max BorderBrush size in WPF is 1.
            if (BorderBrush != Brushes.Transparent)
            {
                BorderThickness = new Thickness(1);
            }

            var topMargin = 0;
            //var gradientLevel = 1;
            if (Parent is XAMLiteMenuNew)
            {
                var m = Margin;
                topMargin = (Parent.Height - Height) / 2;
                Margin = new Thickness(m.Left, topMargin, m.Right, m.Bottom);
                //gradientLevel = (Parent as XAMLiteMenuNew).UpperGradientBrightness;
            }

            if (HoverBrush != Brushes.Transparent)
            {
                var isBright = ColorHelper.Brightness(HoverBrush) > (IsMenuHead ? ColorHelper.Brightness(Parent.Background) : ColorHelper.Brightness(ItemsBackground));

                _highlightedBackground = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 5, Height, !isBright ? 55 : 100))
                {
                    RenderTransform = isBright ? RenderTransform.FlipVertical : RenderTransform.Normal,
                    Background = HoverBrush,
                    Width = Width,
                    Height = Height - 2,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 0.45f,
                    DrawOrder = Parent.DrawOrder,
                    Visibility = Visibility.Hidden
                };
                Children.Add(_highlightedBackground);
            }

            LoadHighlightEdges();

            if (HasItems && !IsMenuHead)
            {
                _arrow = new XAMLiteImageNew(Game)
                {
                    SourceName = "Icons/menu-item-arrow",
                    Background = _label.Foreground,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 15, 0),
                    IsColorized = true
                };
                Children.Add(_arrow);
            }

            if (IsCheckable)
            {
                _checkmark = new XAMLiteImageNew(Game)
                {
                    SourceName = "Icons/checkmark",
                    Background = _label.Foreground,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(12, 0, 0, 0),
                    Visibility = IsChecked ? Visibility.Visible : Visibility.Hidden,
                    IsColorized = true
                };
                Children.Add(_checkmark);
            }

            Children.Add(_label);
        }

        /// <summary>
        /// Loads the hover edges of the control.
        /// </summary>
        private void LoadHighlightEdges()
        {
            var texture = Game.Content.Load<Texture2D>("Icons/menu-highlight-top");
            var isBright = ColorHelper.Brightness(HoverBrush) > 0.75f;
            var top = new XAMLiteImageNew(Game, texture)
            {
                Width = Width - 4,
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(top);

            var bottom = new XAMLiteImageNew(Game, texture)
            {
                Width = Width - 4,
                RenderTransform = RenderTransform.FlipVertical,
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(bottom);

            texture = Game.Content.Load<Texture2D>("Icons/menu-highlight-side");

            var left = new XAMLiteImageNew(Game, texture)
            {
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                Height = Height - 4,
                Margin = Parent is XAMLiteMenuNew ? new Thickness() : new Thickness(5, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(left);

            var right = new XAMLiteImageNew(Game, texture)
            {
                RenderTransform = RenderTransform.FlipHorizontal,
                Height = Height - 4,
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                Margin = Parent is XAMLiteMenuNew ? new Thickness() : new Thickness(0, 0, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(right);

            texture = Game.Content.Load<Texture2D>("Icons/menu-highlight-corner");
            var tlCorner = new XAMLiteImageNew(Game, texture)
            {
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = Parent is XAMLiteMenuNew ? new Thickness(1, 1, 0, 0) : new Thickness(6, 1, 0, 0),
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(tlCorner);

            var trCorner = new XAMLiteImageNew(Game, texture)
            {
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = Parent is XAMLiteMenuNew ? new Thickness(0, 1, 1, 0) : new Thickness(0, 1, 6, 0),
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(trCorner);

            var blCorner = new XAMLiteImageNew(Game, texture)
            {
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = Parent is XAMLiteMenuNew ? new Thickness(1, 0, 0, 1) : new Thickness(6, 0, 0, 1),
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(blCorner);

            var brCorner = new XAMLiteImageNew(Game, texture)
            {
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = Parent is XAMLiteMenuNew ? new Thickness(0, 0, 1, 1) : new Thickness(0, 0, 6, 1),
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(brCorner);

            foreach (var image in _highlightEdgesHover)
            {
                Children.Add(image);
            }
        }

        /// <summary>
        /// Initializes the event hooks.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;

            if (IsMenuHead || !HasItems)
            {
                MouseDown += OnMouseDown;
            }
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
                SetNewItems();
            }

            HandleHighlights();

            HandleCheckMarks();
        }

        /// <summary>
        /// Manages the visibility of checkmarks when the Menu Item is visible.
        /// </summary>
        private void HandleCheckMarks()
        {
            if (Visibility == Visibility.Visible)
            {
                if (IsCheckable && IsChecked && _checkmark.Visibility == Visibility.Hidden)
                {
                    _checkmark.Visibility = _checkmark.Visibility = Visibility.Visible;
                }
                else if (IsCheckable && !IsChecked && _checkmark.Visibility == Visibility.Visible)
                {
                    _checkmark.Visibility = _checkmark.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Specifically manages the highlights on menu items that have Items.
        /// For example, when its list of Items is visible, the highlight should
        /// always be visible.
        /// </summary>
        private void HandleHighlights()
        {
            if (IsMenuHead && Panel.Contains(Ms.X, Ms.Y))
            {
                return;
            }

            if (HasItems && Items[0].Visibility == Visibility.Visible && !_isHighlighted)
            {
                ToggleHighlight();
            }
            else if (HasItems && Items[0].Visibility == Visibility.Hidden && _isHighlighted)
            {
                ToggleHighlight();
            }
        }

        /// <summary>
        /// Toggles the highlight on a menu item.
        /// </summary>
        private void ToggleHighlight()
        {
            _isHighlighted = !_isHighlighted;

            if (_isHighlighted)
            {
                Highlight();
            }
            else
            {
                RemoveHighlight();
            }
        }

        /// <summary>
        /// Highlights a menu item without needing to consider toggling.
        /// </summary>
        private void Highlight()
        {
            foreach (var image in _highlightEdgesHover)
            {
                image.Visibility = Visibility.Visible;
            }

            if (_highlightedBackground != null)
            {
                _highlightedBackground.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Removes the Highlight on a menu item without needing to consider 
        /// toggling.
        /// </summary>
        private void RemoveHighlight()
        {
            foreach (var image in _highlightEdgesHover)
            {
                image.Visibility = Visibility.Hidden;
            }

            if (_highlightedBackground != null)
            {
                _highlightedBackground.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Sets the visiblity of Items when this menu item Has Items.
        /// </summary>
        private void UpdateVisibility()
        {
            if (HasItems)
            {
                foreach (XAMLiteMenuItemNew item in Items)
                {
                    item.Visibility = IsMenuOpen ? item.Visibility = Visibility.Visible : item.Visibility = Visibility.Hidden;
                }

                _backdrop.Visibility = IsMenuOpen ? _backdrop.Visibility = Visibility.Visible : _backdrop.Visibility = Visibility.Hidden;
                _dropShadow.Visibility = IsMenuOpen ? _backdrop.Visibility = Visibility.Visible : _backdrop.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Adds items to the grid.
        /// </summary>
        private void LoadItems()
        {
            for (var i = _itemsIndex; i < Items.Count; i++)
            {
                Items[i].Visibility = Visibility.Hidden;
                Children.Add(Items[i]);
            }

            _needToUpdate = true;
        }

        /// <summary>
        /// Sets the positions of the item margins and widths when they change.
        /// </summary>
        private void SetNewItems()
        {
            if (!HasItems)
            {
                return;
            }

            UpdateWidth();
            UpdatePadding();

            for (var i = _itemsIndex; i < Items.Count; i++)
            {
                var item = (XAMLiteMenuItemNew)Items[i];
                var p = item.Parent as XAMLiteMenuItemNew;

                var m = item.Margin;

                double leftMargin = 0;
                double topMargin = 0;
                if (i == 0)
                {
                    leftMargin = (p != null && p.IsMenuHead) ? 0 : p.Width + m.Left - 4; // +BorderThickness.Left;
                    topMargin = (p.IsMenuHead) ? p.Height + BorderThickness.Top + 2 : 0;
                }
                else
                {
                    leftMargin += Items[i - 1].Margin.Left;
                    topMargin += Items[i - 1].Margin.Top + Items[i - 1].Height + 1;
                }

                item.UpdateMargin(new Thickness(leftMargin, topMargin, m.Right, m.Bottom));
            }

            _needToUpdate = false;

            _itemsIndex = Items.Count;

            SetBackground();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetBackground()
        {
            if (!HasItems)
            {
                return;
            }

            var m = Items[0].Margin;
            _backdrop.Margin = new Thickness(m.Left, m.Top - BorderThickness.Top - 2, m.Right, m.Bottom);
            _backdrop.Width = Items[0].Width;
            _dropShadow.Margin = _backdrop.Margin;
            _dropShadow.Width = _backdrop.Width + 3;

            var h = 0;
            foreach (var item in Items)
            {
                h += item.Height + 1;
            }

            _backdrop.Height = h + (int)BorderThickness.Top + (int)BorderThickness.Bottom + 4;
            _dropShadow.Height = _backdrop.Height + 3;
        }

        /// <summary>
        /// Updates the padding of the control.
        /// </summary>
        private void UpdatePadding()
        {
            foreach (var item in Items)
            {
                var i = item as XAMLiteMenuItemNew;
                var p = (item as XAMLiteMenuItemNew)._label.Padding;

                i._label.Padding = new Thickness(p.Left + 25, p.Top, p.Right, p.Bottom);
            }
        }

        /// <summary>
        /// Margin as set by a menu, if the Menu Item's parent is a menu.
        /// </summary>
        /// <param name="margin"></param>
        internal void UpdateMargin(Thickness margin)
        {
            Margin = margin;

            var m = _label.Margin;
            _label.Margin = new Thickness(m.Left, m.Top, m.Right, m.Bottom);
        }

        /// <summary>
        /// Updates the width of the control according to the largest item's 
        /// width.
        /// </summary>
        private void UpdateWidth()
        {
            var w = 0;

            if (Parent is XAMLiteMenuNew)
            {
                w = Width;
            }

            // determine which item has the greatest width
            foreach (var item in Items)
            {
                var i = (XAMLiteMenuItemNew)item;
                var width = (int)Math.Round(i._label.MeasureString().X) + (int)i.Padding.Left + (int)i.Padding.Right;
                if (i.HasItems)
                {
                    width += 20;
                }

                if (width > w)
                {
                    w = width;
                }
            }

            w += 20;

            // set all widths to the size of the greatest width.
            foreach (var item in Items)
            {
                item.Width = w + 10;
                var i = (XAMLiteMenuItemNew)item;
                i._highlightedBackground.Width = w;
                i._highlightEdgesHover[0].Width = w - 4;
                i._highlightEdgesHover[1].Width = w - 4;
            }
        }

        /// <summary>
        /// Handles Mouse Down events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (IsMenuHead)
            {
                IsMenuOpen = !IsMenuOpen;
                var p = (XAMLiteMenuNew)Parent;
                p.IsMenuOpen = IsMenuOpen;
                UpdateVisibility();
            }
            else
            {
                if (!HasItems)
                {
                    if (IsCheckable)
                    {
                        IsChecked = !IsChecked;
                    }

                    IsMenuOpen = false;

                    if (Parent is XAMLiteMenuNew)
                    {
                        var p = (XAMLiteMenuNew)Parent;
                        if (p.IsMenuOpen)
                        {
                            p.IsMenuOpen = false;

                            UpdateVisibility();
                        }
                    }
                    else
                    {
                        var p = (XAMLiteMenuItemNew)Parent;
                        p.IsMenuOpen = false;
                        p.CloseAll();
                    }
                }
            }
        }

        /// <summary>
        /// Handles Mouse Leave events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!HasItems || Items[0].Visibility == Visibility.Hidden)
            {
                RemoveHighlight();
            }
        }

        /// <summary>
        /// Handles Mouse Enter events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            Highlight();

            if (Parent is XAMLiteMenuNew)
            {
                var p = (XAMLiteMenuNew)Parent;
                if (p.IsMenuOpen && !IsMenuOpen)
                {
                    IsMenuOpen = !IsMenuOpen;

                    p.CloseOtherMenus(ItemIndex);
                    UpdateVisibility();
                }
            }
            else
            {
                var p = (XAMLiteMenuItemNew)Parent;
                if (p.IsMenuOpen)
                {
                    IsMenuOpen = true;

                    p.CloseOtherMenus(ItemIndex);
                    UpdateVisibility();
                }
            }
        }

        /// <summary>
        /// Closes all menus except the the one passed as a parameter.
        /// </summary>
        /// <param name="itemIndex"></param>
        private void CloseOtherMenus(int itemIndex)
        {
            foreach (XAMLiteMenuItemNew item in Items)
            {
                if (item.ItemIndex != itemIndex)
                {
                    item.Close();
                }
            }
        }

        /// <summary>
        /// Closes each layer of items until it reaches a menu head.
        /// </summary>
        public void CloseAll()
        {
            if (Parent != null)
            {
                if (Parent is XAMLiteMenuItemNew)
                {
                    var p = (XAMLiteMenuItemNew)Parent;
                    p.IsMenuOpen = false;
                    p.CloseAll();
                }
                else
                {
                    var p = (XAMLiteMenuNew)Parent;
                    p.IsMenuOpen = false;
                    p.CloseOtherMenus(-1);
                }

                UpdateVisibility();
            }
        }

        /// <summary>
        /// Call to close a menu and its items.
        /// </summary>
        public void Close()
        {
            if (IsMenuOpen)
            {
                IsMenuOpen = !IsMenuOpen;
                UpdateVisibility();
            }
        }

        /// <summary>
        /// Disposes of the XAMLite objects that make up the control.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (IsMenuHead || !HasItems)
            {
                MouseDown -= OnMouseDown;
            }

            MouseEnter -= OnMouseEnter;
            MouseLeave -= OnMouseLeave;

            foreach (var child in Children)
            {
                child.Dispose();
            }

            foreach (var item in Items)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FindMouseDownAndClose()
        {
            // recursively find the deepest open menu.
            foreach (XAMLiteMenuItemNew item in Items)
            {
                if (item.HasItems && item.IsMenuOpen)
                {
                    item.FindMouseDownAndClose();
                    return;
                }
            }

            // abort if a mouse down occurred on the control.
            foreach (XAMLiteMenuItemNew item in Items)
            {
                if (item.Panel.Contains(Ms.X, Ms.Y) && !item.HasItems)
                {
                    return;
                }
            }

            if (!Panel.Contains(Ms.X, Ms.Y))
            {
                IsMenuOpen = false;

                if (Parent is XAMLiteMenuNew)
                {
                    var p = (XAMLiteMenuNew)Parent;
                    if (p.IsMenuOpen)
                    {
                        p.IsMenuOpen = false;

                        UpdateVisibility();
                    }
                }
                else
                {
                    var p = (XAMLiteMenuItemNew)Parent;
                    p.IsMenuOpen = false;
                    p.CloseAll();
                }
            }
        }
    }
}
