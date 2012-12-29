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
    /// TODO: Remove color from textures so that the assets can be colorized to any color.
    /// </summary>
    public class XAMLiteScrollBar : XAMLiteGridNew
    {
        /// <summary>
        /// The child that reacts to user interaction of the scroll bar.
        /// </summary>
        public XAMLiteBaseControl Child;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteGridNew _grid;

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
        /// 
        /// </summary>
        private object _content;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteScrollBar(Game game)
            : base(game)
        {
            Orientation = Orientation.Vertical;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Width = 21;
                    Height = Child != null && Height == 0 ? Child.Height : 100;
                    break;
                case Orientation.Horizontal:
                    Width = Child != null && Width == 0 ? Child.Width : 100;
                    Height = 21;
                    break;
            }

            // If there is a child linked to the Scroll Bar, set up a grid that imitates the
            // child's location so that the scroll bar will be positioned within its child's
            // boundaries. Also, modify the padding of the child to accommodate the space
            // that the scroll bar needs.
            if (Child != null)
            {
                _grid = new XAMLiteGridNew(Game)
                    {
                        Width = Child.Width,
                        Height = Child.Height,
                        Margin = Child.Margin,
                        HorizontalAlignment = Child.HorizontalAlignment,
                        VerticalAlignment = Child.VerticalAlignment
                    };
                Game.Components.Add(_grid);
                _grid.Children.Add(this);

                if (Child is XAMLiteTextBlockNew)
                {
                    var child = (XAMLiteTextBlockNew)Child;

                    _content = child.Text;

                    ModifyChildPadding(child);
                }
                else
                {
                    ModifyChildMargin();
                }
            }

            var backDrop = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollBackDrop",
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
            Children.Add(_downArrowButton);
            _downArrowButton.MouseDown += DownArrowButtonOnMouseDown;

            _downArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden
            };
            Children.Add(_downArrowButtonMouseDown);
            _downArrowButtonMouseDown.MouseUp += DownArrowButtonMouseDownOnMouseUp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (_scrollUpButton.State == ButtonStates.Down)
            //{
            //    _currentTimeToScroll += dt;
            //    if (_currentTimeToScroll > TimePerScroll)
            //    {
            //        _currentTimeToScroll = 0;
            //        _currentScroll = (int)MathHelper.Clamp(_currentScroll + ScrollDownSpeed, _minScroll, 0);
            //    }
            //}
            //else if (_scrollDownButton.State == ButtonStates.Down)
            //{
            //    _currentTimeToScroll += dt;
            //    if (_currentTimeToScroll > TimePerScroll)
            //    {
            //        _currentTimeToScroll = 0;
            //        _currentScroll = (int)MathHelper.Clamp(_currentScroll - ScrollDownSpeed, _minScroll, 0);
            //    }
            //}
            //else
            //{
            //    _currentTimeToScroll = 0;
            //}

            //if (_scrollUpButton.HandleInput(dt))
            //{
            //    return true;
            //}

            //if (_scrollDownButton.HandleInput(dt))
            //{
            //    return true;
            //}

            //if (Bounds.Contains(Input.X, Input.Y))
            //{
            //    if (Input.DeltaWheel != 0)
            //    {
            //        _currentScroll = (int)MathHelper.Clamp(_currentScroll + ((Input.DeltaWheel * 0.01f) * ScrollDownSpeed), _minScroll, 0);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void ModifyChildMargin()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ModifyChildPadding(XAMLiteTextBlockNew child)
        {
            var p = child.Padding;
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            child.Padding = new Thickness(p.Left, p.Top + Height, p.Right, p.Bottom);
                            break;
                        case VerticalAlignment.Bottom:
                            child.Padding = new Thickness(p.Left, p.Top, p.Right, p.Bottom + Height);
                            break;
                    }

                    break;

                case Orientation.Vertical:
                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            child.Padding = new Thickness(p.Left + Width, p.Top, p.Right, p.Bottom);
                            break;
                        case HorizontalAlignment.Right:
                            child.Padding = new Thickness(p.Left, p.Top, p.Right + Width, p.Bottom);
                            break;
                    }

                    break;
            }
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
