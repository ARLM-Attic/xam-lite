using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// The orientation of the control, whether horizontal or vertical.
    /// </summary>
    public enum Orientation
    {
        Horizontal,

        Vertical
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteScrollBar : XAMLiteGridNew
    {
        /// <summary>
        /// Gets or sets whether the ScrollBar is displayed horizontally or vertically.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets the current magnitude of the range control. 
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the highest possible Value of the range element.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the Minimum possible Value of the range element.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteImageWithRolloverNew _upArrowButton;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteImageNew _upArrowButtonMouseDown;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteImageWithRolloverNew _downArrowButton;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteImageNew _downArrowButtonMouseDown;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteScrollBar(Game game)
            : base(game)
        {
            Orientation = Orientation.Vertical;
            Height = 100;
            Width = 21;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            BackgroundColor = Color.Red;
            var h = Height;

            if (Orientation == Orientation.Horizontal)
            {
                Width = Parent.Width;
            }

            var backDrop = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollBackDrop",
                //RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateClockwise90,
                Height = Height,
                Width = Width
            };
            Children.Add(backDrop);

            _upArrowButton = new XAMLiteImageWithRolloverNew(Game)
                {
                    SourceName = "Icons/ArrowButton", 
                    RolloverSourceName = "Icons/ArrowButtonHover", 
                    RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
            Children.Add(_upArrowButton);
            _upArrowButton.MouseDown += UpArrowButtonOnMouseDown;

            _upArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden
            };
            Children.Add(_upArrowButtonMouseDown);
            _upArrowButtonMouseDown.MouseUp += UpArrowButtonMouseDownOnMouseUp;

            _downArrowButton = new XAMLiteImageWithRolloverNew(Game)
            {
                SourceName = "Icons/ArrowButton",
                RolloverSourceName = "Icons/ArrowButtonHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            //Children.Add(_downArrowButton);
            _downArrowButton.MouseDown += DownArrowButtonOnMouseDown;

            _downArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden
            };
            //Children.Add(_downArrowButtonMouseDown);
            _downArrowButtonMouseDown.MouseUp += DownArrowButtonMouseDownOnMouseUp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void UpArrowButtonOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _upArrowButton.Visibility = Visibility.Hidden;
            _upArrowButtonMouseDown.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void DownArrowButtonOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _downArrowButton.Visibility = Visibility.Hidden;
            _downArrowButtonMouseDown.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void UpArrowButtonMouseDownOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _upArrowButtonMouseDown.Visibility = Visibility.Hidden;
            _upArrowButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void DownArrowButtonMouseDownOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _downArrowButtonMouseDown.Visibility = Visibility.Hidden;
            _downArrowButton.Visibility = Visibility.Visible;
        }

    }
}
