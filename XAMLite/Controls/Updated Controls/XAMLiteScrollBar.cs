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
        /// The grid that contains all of the assets which define the scroll 
        /// bar.
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
        /// The normal and hover states of the Up Arrow Button.
        /// </summary>
        private XAMLiteImageWithRolloverNew _upArrowButton;

        /// <summary>
        /// The Mouse Down state of the Up Arrow Button.
        /// </summary>
        private XAMLiteImageNew _upArrowButtonMouseDown;

        /// <summary>
        /// The normal and hover states of the Down Arrow Button.
        /// </summary>
        private XAMLiteImageWithRolloverNew _downArrowButton;

        /// <summary>
        /// The Mouse Down state of the Down Arrow Button.
        /// </summary>
        private XAMLiteImageNew _downArrowButtonMouseDown;

        /// <summary>
        /// True when the left mouse button is pressed on the Up Arrow button.
        /// </summary>
        private bool _mouseDownUpArrow;

        /// <summary>
        /// True when the left mouse button is pressed on the Up Arrow button.
        /// </summary>
        private bool _mouseDownDownArrow;

        /// <summary>
        /// Timer for determining when to next scroll the text.
        /// </summary>
        private TimeSpan _scrollTimer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteScrollBar(Game game)
            : base(game)
        {
            Orientation = Orientation.Vertical;
            _scrollTimer = TimeSpan.Zero;
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

            SetInitialScrollValues();
        }

        /// <summary>
        /// Sets the Minimum, Maximum, and initial Value.
        /// </summary>
        private void SetInitialScrollValues()
        {
            Value = 0;
            Minimum = 0;

            switch (Orientation)
            {
                case Orientation.Vertical:
                    var child = Child as XAMLiteTextBlockNew;
                    if (child != null)
                    {
                        var textHeight = child.MeasureText().Y;
                        Maximum = Math.Abs(Height - textHeight);
                    }
          
                    break;
                case Orientation.Horizontal:
                    // TODO: Still need to implement.
                    // Maximum = Width - Child.Width;
                    break;
            }
        }

        /// <summary>
        /// Handles the different means by which the text can be scrolled.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_mouseDownUpArrow)
            {
                _scrollTimer -= gameTime.ElapsedGameTime;
                if (_scrollTimer <= TimeSpan.Zero)
                {
                    UpdateMouseArrowUp();
                    _scrollTimer = TimeSpan.FromSeconds(0.1);
                }
            }
            else if (_mouseDownDownArrow)
            {
                _scrollTimer -= gameTime.ElapsedGameTime;
                if (_scrollTimer <= TimeSpan.Zero)
                {
                    UpdateMouseArrowDown();
                    _scrollTimer = TimeSpan.FromSeconds(0.1);
                }
            }

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
        /// Moves the text up as the text is scrolled to its end.
        /// </summary>
        private void UpdateMouseArrowDown()
        {
            if (Value >= Maximum)
            {
                return;
            }

            Value += 10;

            var m = Child.Margin;

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Child.Margin = new Thickness(m.Left, m.Top - 10, m.Right, m.Bottom);
                    break;
                case Orientation.Horizontal:
                    Child.Margin = new Thickness(m.Left - 10, m.Top, m.Right, m.Bottom);
                    break;
            }
        }

        /// <summary>
        /// Moves the text down as the text is scrolled to its beginning.
        /// </summary>
        private void UpdateMouseArrowUp()
        {
            if (Value <= Minimum)
            {
                return;
            }

            Value -= 10;

            var m = Child.Margin;

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Child.Margin = new Thickness(m.Left, m.Top + 10, m.Right, m.Bottom);
                    break;
                case Orientation.Horizontal:
                    Child.Margin = new Thickness(m.Left + 10, m.Top, m.Right, m.Bottom);
                    break;
            }
        }

        /// <summary>
        /// TODO: Still need to implement.
        /// </summary>
        private void ModifyChildMargin()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Adjust the padding of the child so that the scroll bar does not
        /// cover what it will be scrolling.
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
        /// Handles when the Up Arrow Button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void UpArrowButtonOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _upArrowButton.Visibility = Visibility.Hidden;
            _upArrowButtonMouseDown.Visibility = Visibility.Visible;

            _scrollTimer = TimeSpan.Zero;
            _mouseDownUpArrow = true;
        }

        /// <summary>
        /// Handles when the Down Arrow Button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void DownArrowButtonOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _downArrowButton.Visibility = Visibility.Hidden;
            _downArrowButtonMouseDown.Visibility = Visibility.Visible;

            _scrollTimer = TimeSpan.Zero;
            _mouseDownDownArrow = true;
        }

        /// <summary>
        /// Handles when the Up Arrow Button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void UpArrowButtonMouseDownOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _upArrowButtonMouseDown.Visibility = Visibility.Hidden;
            _upArrowButton.Visibility = Visibility.Visible;

            _mouseDownUpArrow = false;
        }

        /// <summary>
        /// Handles when the Down Arrow Button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void DownArrowButtonMouseDownOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _downArrowButtonMouseDown.Visibility = Visibility.Hidden;
            _downArrowButton.Visibility = Visibility.Visible;

            _mouseDownDownArrow = false;
        }
    }
}
