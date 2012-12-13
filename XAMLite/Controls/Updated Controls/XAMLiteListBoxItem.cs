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
        //private bool _isBorderThicknessEqual;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteGridNew _grid;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteLabelNew _listBoxContent;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteRectangleNew _background;

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
        /// 
        /// </summary>
        private Brush _selectedBackground;

        /// <summary>
        /// Although not in WPF, this seems essential to override the default
        /// colors in WPF for highlighting on mouse over or when selected.  
        /// If this is not explicitly set, it will receive the brush color
        /// as specified by the ListBox that contains it.
        /// </summary>
        public Brush SelectedBackground 
        { 
            get
            {
                return _selectedBackground;
            } 

            set
            {
                _selectedBackground = value;
            } 
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteListBoxItem(Game game)
            : base(game)
        {
            Background = Brushes.Transparent;
            SelectedBackground = Brushes.Transparent;
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

            _grid = new XAMLiteGridNew(Game)
                {
                    Parent = this,
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment,
                    Margin = Margin
                };
            Game.Components.Add(_grid);

            _background = new XAMLiteRectangleNew(Game)
                {
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = Brushes.CornflowerBlue   
                };
            _grid.Children.Add(_background);

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
            _grid.Children.Add(_listBoxContent);
        }

        /// <summary>
        /// Updates the margins and widths that make up the control.
        /// </summary>
        /// <param name="margin"></param>
        internal void UpdateMarginAndWidth(Thickness margin)
        {
            // set margins
            Margin = margin;
            _grid.Margin = Margin;
            _background.Margin = Margin;
            _listBoxContent.Margin = Margin;

            // set Widths.
            var par = (XAMLiteListBox)Parent;
            Width = par.Width - (int)par.BorderThickness.Right - (int)par.BorderThickness.Left;
            _grid.Width = Width;
            _background.Width = Width;

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            _background.Fill = Background;
            _background.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            _background.Fill = SelectedBackground;
            _background.Visibility = Visibility.Visible;
        }
    }
}
