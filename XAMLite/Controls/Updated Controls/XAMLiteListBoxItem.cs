using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{ 
    /// <summary>
    /// Represents a selectable item in a ListBox.
    /// </summary>
    public class XAMLiteListBoxItem : XAMLiteBaseContent
    {
        /// <summary>
        /// True when the ListBoxItem is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// True when the ListBoxItem is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;

                if (!_isSelected)
                {
                    BackgroundPanel.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// The Border color.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// The thickness of the border.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected internal XAMLiteGridNew Grid;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteLabelNew _listBoxContent;

        /// <summary>
        /// The background of the control that changes colors when selected.
        /// </summary>
        protected internal XAMLiteRectangleNew BackgroundPanel;

        /// <summary>
        /// Reference to the specific class that the parent represents.
        /// </summary>
        private XAMLiteListBox _parent;

        /// <summary>
        /// Modifies the visibility of the grid, and therefore all of the 
        /// controls contained within it.
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

                if (Grid != null)
                {
                    Grid.Visibility = value;
                }
            }
        }

        /// <summary>
        /// Sets the Text color of the ListBoxItem.
        /// </summary>
        public override Brush Foreground
        {
            get
            {
                return base.Foreground;
            }

            set
            {
                base.Foreground = value;

                if (_listBoxContent == null)
                {
                    return;
                }

                _listBoxContent.Foreground = value;
            }
        }

        /// <summary>
        /// Although not in WPF, this seems essential to override the default
        /// colors in WPF for highlighting on mouse over or when selected.  
        /// If this is not explicitly set, it will receive the brush color
        /// as specified by the ListBox that contains it.
        /// </summary>
        public Brush SelectedBackground { get; set; }

        /// <summary>
        /// The brush color of a selected ListBoxItem when the ListBox that 
        /// contains it loses focus.
        /// </summary>
        public Brush UnfocusedSelectedBackground { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteListBoxItem(Game game)
            : base(game)
        {
            Background = Brushes.Transparent;
            SelectedBackground = Brushes.Transparent;
            UnfocusedSelectedBackground = Brushes.Transparent;
            Foreground = Brushes.Transparent;
            BorderBrush = Brushes.White;
            FontFamily = new FontFamily("Arial");
            BorderThickness = new Thickness(1);
            Padding = new Thickness(4, 0, 0, 0);
            Focusable = true;
        }

        /// <summary>
        /// Loads the content of the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            UpdateFontMetrics();

            var w = (int)SpriteFont.MeasureString(Content.ToString()).X;
            var h = (int)SpriteFont.MeasureString(Content.ToString()).Y;

            Grid = new XAMLiteGridNew(Game)
                {
                    Parent = this,
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment,
                    Margin = Margin
                };
            Game.Components.Add(Grid);

            BackgroundPanel = new XAMLiteRectangleNew(Game)
                {
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top 
                };
            Grid.Children.Add(BackgroundPanel);

            _listBoxContent = new XAMLiteLabelNew(Game)
                {
                    Content = Content,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontFamily = FontFamily,
                    Spacing = Spacing,
                    Padding = Padding,
                    Foreground = Foreground
                };
            Grid.Children.Add(_listBoxContent);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _parent = (XAMLiteListBox)Parent;
        }

        /// <summary>
        /// Updates the margins and widths that make up the control.
        /// </summary>
        /// <param name="margin"></param>
        internal void UpdateMarginAndWidth(Thickness margin)
        {
            // set margins
            Margin = margin;
            Grid.Margin = Margin;
            var m = new Thickness(margin.Left + _parent.BorderThickness.Left, margin.Top, margin.Right, margin.Bottom);
            BackgroundPanel.Margin = m;
            _listBoxContent.Margin = m;

            // set Widths.
            Width = _parent.Width;
            Grid.Width = Width;
            BackgroundPanel.Width = Width - (int)_parent.BorderThickness.Right - (int)_parent.BorderThickness.Left;

            MouseDown += OnMouseDown;
        }

        /// <summary>
        /// Sets the selected color and calls its parent to deselect the other
        /// Items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            IsSelected = true;
            IsFocused = true;

            _parent.DeselectAll(Index);
            BackgroundPanel.Fill = SelectedBackground;
            BackgroundPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When the ListBox containing the ListBoxItem loses focus, the brush
        /// color of the selected item changes to an unfocused color.
        /// </summary>
        public void UnfocusSelectedBrush(bool isFocused)
        {
            IsFocused = false;
            
            BackgroundPanel.Fill = UnfocusedSelectedBackground;
            BackgroundPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            MouseDown -= OnMouseDown;
            foreach (var child in Grid.Children)
            {
                child.Dispose();
            }
        }
    }
}
