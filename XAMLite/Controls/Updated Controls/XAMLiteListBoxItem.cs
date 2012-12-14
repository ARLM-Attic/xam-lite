﻿using System;
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
                    BackgroundRectangle.Visibility = Visibility.Hidden;
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
        protected internal XAMLiteRectangleNew BackgroundRectangle;

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

            BackgroundRectangle = new XAMLiteRectangleNew(Game)
                {
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top 
                };
            Grid.Children.Add(BackgroundRectangle);

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
        /// Updates the margins and widths that make up the control.
        /// </summary>
        /// <param name="margin"></param>
        internal void UpdateMarginAndWidth(Thickness margin)
        {
            // set margins
            Margin = margin;
            Grid.Margin = Margin;
            BackgroundRectangle.Margin = Margin;
            _listBoxContent.Margin = Margin;

            // set Widths.
            var par = (XAMLiteListBox)Parent;
            Width = par.Width - (int)par.BorderThickness.Right - (int)par.BorderThickness.Left;
            Grid.Width = Width;
            BackgroundRectangle.Width = Width;

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
            var par = (XAMLiteListBox)Parent;
            par.DeselectAll(Index);
            BackgroundRectangle.Fill = SelectedBackground;
            BackgroundRectangle.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When the ListBox containing the ListBoxItem loses focus, the brush
        /// color of the selected item changes.
        /// </summary>
        public void UpdateSelectedBrush(bool isFocused)
        {
            IsFocused = isFocused;

            BackgroundRectangle.Fill = isFocused ? SelectedBackground : UnfocusedSelectedBackground;
            
            BackgroundRectangle.Visibility = Visibility.Visible;
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
