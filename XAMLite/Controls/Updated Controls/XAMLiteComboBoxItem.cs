using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteComboBoxItem : XAMLiteBaseContent
    {
        /// <summary>
        /// True when the Combo Box Item is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// True when the mouse is over a control.
        /// </summary>
        public bool IsMouseOver { get; set; }

        /// <summary>
        /// The border color.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// The border thickness.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// Grid that contains all of the TextBox assets.
        /// </summary>
        private XAMLiteGridNew _grid;

        /// <summary>
        /// Contains all of the XAMLiteRectangles that make up the border.
        /// </summary>
        private List<XAMLiteRectangleNew> _borderRectangles;

        /// <summary>
        /// The text contained in the text box.
        /// </summary>
        private XAMLiteLabelNew _content;
        private bool _isBorderThicknessEqual;

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
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteComboBoxItem(Game game)
            : base(game)
        {
            FontFamily = new FontFamily("Verdana12");
            Spacing = 2;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Center;
            Padding = new Thickness(5, 0, 0, 0);
            BorderBrush = null;
            _borderRectangles = new List<XAMLiteRectangleNew>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // grid that contains all of the UI assets that form a combo box.
            _grid = new XAMLiteGridNew(Game)
            {
                Parent = this,
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment,
                Width = Width,
                Height = Height,
                Margin = Margin,
            };
            Game.Components.Add(_grid);

            var fill = new XAMLiteRectangleNew(Game)
            {
                Fill = Background,
                Width = Width,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            _grid.Children.Add(fill);

            _content = new XAMLiteLabelNew(Game)
            {
                Content = Content,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily,
                Spacing = Spacing,
                Foreground = Foreground,
                Padding = new Thickness(BorderThickness.Left > 1 ? Padding.Left + BorderThickness.Left : Padding.Left,
                    BorderThickness.Top > 1 ? Padding.Top + BorderThickness.Top : Padding.Top, 0, 0),
            };
            _grid.Children.Add(_content);

            if (BorderBrush != null)
            {
                _isBorderThicknessEqual = BorderThickness.Left == BorderThickness.Right
                                          && BorderThickness.Right == BorderThickness.Top
                                          && BorderThickness.Top == BorderThickness.Bottom;
                if (_isBorderThicknessEqual)
                {
                    var border = new XAMLiteRectangleNew(Game)
                        { Stroke = BorderBrush, StrokeThickness = BorderThickness.Left };
                    _borderRectangles.Add(border);
                }
                else
                {
                    SetBorders();
                }

                AdjustControlPositions();

                foreach (var borderRectangle in _borderRectangles)
                {
                    _grid.Children.Add(borderRectangle);
                }
            }
        }

        /// <summary>
        /// Positions the XAMlite Controls according to the Border
        // Thickness when the thickness is greater than 1.
        /// </summary>
        private void AdjustControlPositions()
        {
            if (_isBorderThicknessEqual)
            {
                
            }
            else
            {
                
            }
        }

        /// <summary>
        /// Sets the border colors of the textbox.
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

    }
}
