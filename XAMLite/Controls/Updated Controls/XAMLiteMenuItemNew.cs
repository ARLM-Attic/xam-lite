using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Microsoft.Xna.Framework.Graphics;

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
        /// 
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
        /// 
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

        // TODO: A Menu Item needs a background color when it
        // TODO: is not the head of a menu and it needs a highlight color
        // TODO: when it is being hovered over.
        ///// <summary>
        ///// 
        ///// </summary>
        //public override Brush Background
        //{
        //    get
        //    {
        //        if (BackgroundPanel != null)
        //        {
        //            return BackgroundPanel.Background;
        //        }

        //        return Brushes.Transparent;
        //    }

        //    set
        //    {
        //        if (BackgroundPanel != null)
        //        {
        //            BackgroundPanel.Background = value;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        private Brush _foreground;

        /// <summary>
        /// Sets the Text color of the ListBoxItem.
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
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// The text portion of the control.
        /// </summary>
        private XAMLiteLabelNew _label;

        ///// <summary>
        ///// Parent of the this control.
        ///// </summary>
        //private XAMLiteMenuNew _parent;

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
        /// Although not in WPF, this seems essential to override the default
        /// colors in WPF for highlighting on mouse over or when selected.  
        /// If this is not explicitly set, it will receive the brush color
        /// as specified by the ListBox that contains it.
        /// </summary>
        public Brush HoverBrush { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private List<XAMLiteImageNew> _highlightEdgesHover; 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuItemNew(Game game)
            : base(game)
        {
            Background = Brushes.Transparent;
            //SelectedBackground = Brushes.Transparent;
            //UnfocusedSelectedBackground = Brushes.Transparent;
            Foreground = Brushes.Transparent;
            HoverBrush = Brushes.Transparent;
            //BorderBrush = Brushes.Transparent;
            FontFamily = new FontFamily("Verdana14");
            Spacing = 2;
            //BorderThickness = new Thickness(1);
            Padding = new Thickness(10, 4, 10, 5);
            Focusable = true;
            IsEnabled = true;

            _highlightEdgesHover = new List<XAMLiteImageNew>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _label = new XAMLiteLabelNew(Game)
                {
                    Content = Header,
                    Foreground = Foreground == Brushes.Transparent ? Brushes.Black : Foreground,
                    FontFamily = FontFamily,
                    Spacing = Spacing,
                    Padding = Padding,
                    Opacity = IsEnabled ? 1f : 0.75f
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
                var isBright = ColorHelper.Brightness(HoverBrush) > 0.5f;
                
                _highlightedBackground = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 5, Height, !isBright ? 55 : 100))
                    {
                        RenderTransform = isBright ? RenderTransform.FlipVertical : RenderTransform.Normal,
                        Background = HoverBrush,
                        Width = Width - 2,
                        Height = Height,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        //Margin = new Thickness(BorderThickness.Left, BorderThickness.Top, 0, 0),
                        Opacity = 0.45f,
                        DrawOrder = Parent.DrawOrder,
                        Visibility = Visibility.Hidden
                    };
                Children.Add(_highlightedBackground);
            }

            if (Parent is XAMLiteMenuNew)
            {
                LoadHighlightEdges();
            }

            Children.Add(_label);
        }

        public override void Initialize()
        {
            base.Initialize();
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
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

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
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
        /// Loads the hover edges of the control.
        /// </summary>
        private void LoadHighlightEdges()
        {
            var texture = Game.Content.Load<Texture2D>("Icons/menu-highlight-top");
            var isBright = ColorHelper.Brightness(HoverBrush) > 0.75f;
            var top = new XAMLiteImageNew(Game, texture)
                {
                    Width = Width,
                    Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Hidden
                };
            _highlightEdgesHover.Add(top);

            var bottom = new XAMLiteImageNew(Game, texture)
            {
                Width = Width,
                RenderTransform = RenderTransform.FlipVertical,
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(bottom);

            texture = Game.Content.Load<Texture2D>("Icons/menu-highlight-side");

            var left = new XAMLiteImageNew(Game, texture)
            {
                Background = HoverBrush == Brushes.Transparent ? Brushes.DarkGray : !isBright ? HoverBrush : Brushes.DarkGray,
                Height = Height - 4,
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
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden
            };
            _highlightEdgesHover.Add(right);

            foreach (var image in _highlightEdgesHover)
            {
                Children.Add(image);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="margin"></param>
        internal void UpdateMarginAndWidth(Thickness margin)
        {
            // set margins
            Margin = margin;

            //Console.WriteLine("Parent: " + _parent);
            // set Widths.
            //Width = _parent.Width;
            //BackgroundPanel.Width = Width - (int)_parent.BorderThickness.Right - (int)_parent.BorderThickness.Left;
            //var m = Margin;
            //var m = BackgroundPanel.Margin;
            //BackgroundPanel.Margin = new Thickness(_parent.BorderThickness.Left, m.Top, m.Right, m.Bottom);
            var m = _label.Margin;
            _label.Margin = new Thickness(m.Left, m.Top, m.Right, m.Bottom);

        }
    }
}
