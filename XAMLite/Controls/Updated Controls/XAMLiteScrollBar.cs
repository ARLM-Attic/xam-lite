using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    /// TODO: Spend some serious time optimizing all of the state changes of the various buttons and sliders.
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
        /// The Mouse Down state of the Up Arrow Button.
        /// </summary>
        private XAMLiteImageNew _upArrowButtonMouseDown;

        /// <summary>
        /// The Mouse Down state of the Down Arrow Button.
        /// </summary>
        private XAMLiteImageNew _downArrowButtonMouseDown;

        /// <summary>
        /// Grid that contains all of the XAMLiteImages for creating the 
        /// scrollable bar.
        /// </summary>
        private XAMLiteGridNew _scrollBar;

        /// <summary>
        /// The arrow used to view text that is above the window.
        /// </summary>
        private XAMLiteGridNew _upArrow;

        /// <summary>
        /// The arrow used to view text that is below the window.
        /// </summary>
        private XAMLiteGridNew _downArrow;

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
        /// Contains all XAMLite objects that make up the normal state scroll 
        /// bar.
        /// </summary>
        private List<XAMLiteImageNew> _scrollBarNormal;

        /// <summary>
        /// Contains all XAMLite objects that make up the hover state scroll 
        /// bar.
        /// </summary>
        private List<XAMLiteImageNew> _scrollBarHover;

        /// <summary>
        /// Contains all XAMLite objects that make up the mouse down state 
        /// scroll bar.
        /// </summary>
        private List<XAMLiteImageNew> _scrollBarMouseDown;

        /// <summary>
        /// The height of the text, which may be different than the height of 
        /// the Child, when a scroll bar is necessary.
        /// </summary>
        private float _childTextHeight;

        /// <summary>
        /// Initial position when a mouse down event occurs on the scroll bar slider.
        /// </summary>
        private Vector2 _initialClickPosition;

        /// <summary>
        /// True when a mouse down occurs on the slider
        /// </summary>
        private bool _scrollSliderMouseDown;

        /// <summary>
        /// The Value when a mouse Down first occurs on the Slider.
        /// </summary>
        private double _initialSliderValue;

        /// <summary>
        /// This is used to compare against the current scroll wheel value.  
        /// When they are different, the child will scroll.
        /// </summary>
        private int _previousScrollWheelValue;

        /// <summary>
        /// The maximum height that the scrollable bar can have.
        /// </summary>
        private double _maxScrollBarHeight;

        /// <summary>
        /// The minimum height that a scrollable bar can have.
        /// </summary>
        private double _minScrollBarHeight;

        /// <summary>
        /// True when the text height minus the control height + the minimum 
        /// scrollable bar height is greater than the maximum scrollable bar 
        /// height.
        /// </summary>
        private bool _isLargeTextBlock;

        /// <summary>
        /// This determines the amount the scrollable bar should adjust in 
        /// relation to the text adjustments made by one of the arrow buttons.
        /// </summary>
        private double _scrollValueAdjuster = 1;

        /// <summary>
        /// This determines the amount the text should adjust in relation to 
        /// the scrollable bar being moved.
        /// </summary>
        private double _textValueAdjuster = 1;

        /// <summary>
        /// When true, a mouse down is still active on the Up Arrow, but a 
        /// Mouse Leave occurred.  The text should not scroll while this is true.
        /// </summary>
        private bool _upArrowHasLeftWhilePressed;

        /// <summary>
        /// When true, a mouse down is still active on the Down Arrow, but a 
        /// Mouse Leave occurred.  The text should not scroll while this is true.
        /// </summary>
        private bool _downArrowHasLeftWhilePressed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteScrollBar(Game game)
            : base(game)
        {
            Orientation = Orientation.Vertical;
            _scrollTimer = TimeSpan.Zero;
            _initialClickPosition = Vector2.Zero;
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
                DrawOrder = Child.DrawOrder + 1;
                var m = Child.Margin;
                _grid = new XAMLiteGridNew(Game)
                    {
                        Width = Child.Width,
                        Height = Child.Height - 2,
                        Margin = new Thickness(m.Left, m.Top + 1, m.Right + 1, m.Bottom + 1),
                        HorizontalAlignment = Child.HorizontalAlignment,
                        VerticalAlignment = Child.VerticalAlignment,
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
                Width = Width,
                DrawOrder = DrawOrder
            };
            Children.Add(backDrop);

            var t = Game.Content.Load<Texture2D>("Icons/ArrowButton");

            var upArrowNormalButton = new XAMLiteImageNew(Game, t)
            {
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
            };   

            var downArrowNormalButton = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButton",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                DrawOrder = DrawOrder
            };

            var c = Child as XAMLiteTextBlockNew;

            if (c == null)
            {
                return;
            }

            c.MouseDown += ChildOnMouseDown;
            // if the text height is less than the height of the block, do not load
            // scroll bar nor set the event handlers.

            if (Child is XAMLiteRichTextBlock)
            {
                var rtb = Child as XAMLiteRichTextBlock;
                _childTextHeight = rtb.MeasureTextHeight();
            }
            else
            {
                _childTextHeight = c.MeasureText().Y;
            }

            if (_childTextHeight <= Child.Height)
            {
                Children.Add(upArrowNormalButton);
                Children.Add(downArrowNormalButton);

                return;
            }

            _upArrow = new XAMLiteGridNew(Game)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = t.Width,
                Height = t.Height,
                DrawOrder = DrawOrder
            };    
            Children.Add(_upArrow);

            _upArrow.Children.Add(upArrowNormalButton);

            var upArrowHoverButton = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _upArrow.Children.Add(upArrowHoverButton);

            _upArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _upArrow.Children.Add(_upArrowButtonMouseDown);

            _downArrow = new XAMLiteGridNew(Game)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = t.Width,
                Height = t.Height,
                DrawOrder = DrawOrder
            };
            Children.Add(_downArrow);

            _downArrow.Children.Add(downArrowNormalButton);

            var downArrowHoverButton = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _downArrow.Children.Add(downArrowHoverButton);

            _downArrowButtonMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ArrowButtonMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.FlipVertical : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _downArrow.Children.Add(_downArrowButtonMouseDown);

            // load the event handlers for the arrow buttons.
            _upArrow.MouseEnter += UpArrowOnMouseEnter;
            _upArrow.MouseLeave += UpArrowOnMouseLeave;
            _upArrow.MouseDown += UpArrowOnMouseDown;

            _downArrow.MouseEnter += DownArrowOnMouseEnter;
            _downArrow.MouseLeave += DownArrowOnMouseLeave;
            _downArrow.MouseDown += DownArrowOnMouseDown;

            _scrollBarNormal = new List<XAMLiteImageNew>();
            _scrollBarHover = new List<XAMLiteImageNew>();
            _scrollBarMouseDown = new List<XAMLiteImageNew>();

            // percent of the text height versus the child height.
            var childToTextRatio = Child.Height / _childTextHeight;
            
            // the difference between the text height and the height of its 
            // container.
            var textHeightToChildHeightDifference = _childTextHeight - Child.Height;

            // the maximum scroll bar height
            _maxScrollBarHeight = Child.Height - (t.Height * 2) + 1;

            // the minimum scroll bar height.
            _minScrollBarHeight = 10;

            var scrollHeight = 0;
            if (textHeightToChildHeightDifference + _minScrollBarHeight < _maxScrollBarHeight)
            {
                scrollHeight = (int)(_maxScrollBarHeight - textHeightToChildHeightDifference - 2);
            }
            else
            {
                _isLargeTextBlock = true;
                var difference = textHeightToChildHeightDifference + _minScrollBarHeight + 1 - _maxScrollBarHeight;
                _scrollValueAdjuster = difference / textHeightToChildHeightDifference;
                scrollHeight = (int)_minScrollBarHeight;
                _textValueAdjuster = (_childTextHeight - Child.Height) / (_maxScrollBarHeight - scrollHeight);
            }

            // TODO: this is not truly set up yet.  Focus is currently on vertical scrolling.
            var scrollWidth = Orientation == Orientation.Vertical ? Width : (int)((Width - (t.Width * 2)) * childToTextRatio);

            SetInitialScrollValues(new Vector2(scrollWidth, scrollHeight));

            _scrollBar = new XAMLiteGridNew(Game)
            {
                Width = scrollWidth,
                Height = scrollHeight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = Orientation == Orientation.Vertical ? new Thickness(0, t.Height, 0, 0) : new Thickness(t.Width, 0, 0, 0),
                DrawOrder = DrawOrder
            };
            Children.Add(_scrollBar);
            _scrollBar.MouseEnter += ScrollBarOnMouseEnter;
            _scrollBar.MouseLeave += ScrollBarOnMouseLeave;
            _scrollBar.MouseDown += ScrollBarOnMouseDown;

            var tTop = Game.Content.Load<Texture2D>("Icons/ScrollButtonTopNoHover");
            var tBottom = Game.Content.Load<Texture2D>("Icons/ScrollButtonBottomNoHover");

            var scrollBarTopNoHover = new XAMLiteImageNew(Game, tTop)
            {
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
            };    
            _scrollBar.Children.Add(scrollBarTopNoHover);
            _scrollBarNormal.Add(scrollBarTopNoHover);

            var scrollBarTopHover = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonTopHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarTopHover);
            _scrollBarHover.Add(scrollBarTopHover);

            var scrollBarTopMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonTopMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarTopMouseDown);
            _scrollBarMouseDown.Add(scrollBarTopMouseDown);

            var scrollBarBodyNoHover = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonBodyNoHover",
                Width = Orientation == Orientation.Vertical ? _scrollBar.Width : _scrollBar.Width - tTop.Width - tBottom.Width,
                Height = Orientation == Orientation.Vertical ? _scrollBar.Height - tTop.Height - tBottom.Height : _scrollBar.Height,
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarBodyNoHover);
            _scrollBarNormal.Add(scrollBarBodyNoHover);

            var scrollBarBodyHover = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonBodyHover",
                Width = Orientation == Orientation.Vertical ? _scrollBar.Width : _scrollBar.Width - tTop.Width - tBottom.Width,
                Height = Orientation == Orientation.Vertical ? _scrollBar.Height - tTop.Height - tBottom.Height : _scrollBar.Height,
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarBodyHover);
            _scrollBarHover.Add(scrollBarBodyHover);

            var scrollBarBodyMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonBodyMouseDown",
                Width = Orientation == Orientation.Vertical ? _scrollBar.Width : _scrollBar.Width - tTop.Width - tBottom.Width,
                Height = Orientation == Orientation.Vertical ? _scrollBar.Height - tTop.Height - tBottom.Height : _scrollBar.Height,
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateCounterClockwise90,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarBodyMouseDown);
            _scrollBarMouseDown.Add(scrollBarBodyMouseDown);

            var scrollBarBottomNoHover = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonBottomNoHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarBottomNoHover);
            _scrollBarNormal.Add(scrollBarBottomNoHover);

            var scrollBarBottomHover = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonBottomHover",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarBottomHover);
            _scrollBarHover.Add(scrollBarBottomHover);

            var scrollBarBottomMouseDown = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/ScrollButtonBottomMouseDown",
                RenderTransform = Orientation == Orientation.Vertical ? RenderTransform.Normal : RenderTransform.RotateClockwise90,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder
            };
            _scrollBar.Children.Add(scrollBarBottomMouseDown);
            _scrollBarMouseDown.Add(scrollBarBottomMouseDown);
        }

        /// <summary>
        /// Sets the Minimum, Maximum, and initial Value.
        /// </summary>
        private void SetInitialScrollValues(Vector2 widthHeight)
        {
            Value = 0;
            Minimum = 0;

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Maximum = Math.Abs(_childTextHeight - Height) + 2;
                    break;

                case Orientation.Horizontal:
                    Maximum = Width - (_upArrow.Width * 2) - widthHeight.X;
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

            if (_scrollSliderMouseDown)
            {
                UpdateScrollSliderValue();
            }

            HandleMouseUp();
            
            if (_mouseDownUpArrow && !_upArrowHasLeftWhilePressed)
            {
                _scrollTimer -= gameTime.ElapsedGameTime;
                if (_scrollTimer <= TimeSpan.Zero)
                {
                    UpdateMouseArrowUpValue();
                    _scrollTimer = TimeSpan.FromSeconds(0.1);
                }
            }
            else if (_mouseDownDownArrow && !_downArrowHasLeftWhilePressed)
            {
                _scrollTimer -= gameTime.ElapsedGameTime;
                if (_scrollTimer <= TimeSpan.Zero)
                {
                    UpdateMouseArrowDownValue();
                    _scrollTimer = TimeSpan.FromSeconds(0.1);
                }
            }

            if (Child == null || !Child.IsFocused)
            {
                return;
            }

            if (Ms.LeftButton == ButtonState.Pressed && !Child.Panel.Contains(Ms.X, Ms.Y))
            {
                Child.IsFocused = false;
            }

            if (Ms.ScrollWheelValue != _previousScrollWheelValue)
            {
                if (Ms.ScrollWheelValue > _previousScrollWheelValue)
                {
                    UpdateMouseArrowUpValue();
                }
                else
                {
                    UpdateMouseArrowDownValue();
                }

                _previousScrollWheelValue = Ms.ScrollWheelValue;
            }
        }

        /// <summary>
        /// Handles whenever a Mouse Up event occurs, regardless of whether it was released over the control.
        /// </summary>
        private void HandleMouseUp()
        {
            if (_scrollSliderMouseDown && Ms.LeftButton == ButtonState.Released)
            {
                SliderReleased();
                CheckForMouseEnter();
            }
            else if (_mouseDownUpArrow && Ms.LeftButton == ButtonState.Released)
            {
                UpArrowReleased();
                CheckForMouseEnter();
            }
            else if (_mouseDownDownArrow && Ms.LeftButton == ButtonState.Released)
            {
                DownArrowReleased();
                CheckForMouseEnter();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckForMouseEnter()
        {
            if (_scrollBar.Panel.Contains(Ms.X, Ms.Y))
            {
                for (var i = 0; i < _scrollBarNormal.Count; i++)
                {
                    _scrollBarNormal[i].Visibility = Visibility.Hidden;
                    _scrollBarHover[i].Visibility = Visibility.Visible;
                }
            }
            else if (_upArrow.Panel.Contains(Ms.X, Ms.Y))
            {
                _upArrow.Children[0].Visibility = Visibility.Hidden;
                _upArrow.Children[1].Visibility = Visibility.Visible;
            }
            else if (_downArrow.Panel.Contains(Ms.X, Ms.Y))
            {
                _downArrow.Children[0].Visibility = Visibility.Hidden;
                _downArrow.Children[1].Visibility = Visibility.Visible;
            }

            _upArrowHasLeftWhilePressed = false;
            _downArrowHasLeftWhilePressed = false;
        }

        /// <summary>
        /// Moves the text down as the text is scrolled to its beginning.
        /// </summary>
        private void UpdateMouseArrowUpValue()
        {
            Value -= 10;

            if (Value <= Minimum)
            {
                Value = Minimum;
            }

            UpdateChildPosition(Value);
            UpdateScrollSliderPosition(_isLargeTextBlock ? Value * (1 - _scrollValueAdjuster) : Value);
        }

        /// <summary>
        /// Moves the text up as the text is scrolled to its end.
        /// </summary>
        private void UpdateMouseArrowDownValue()
        {
            Value += 10;

            if (Value >= Maximum)
            {
                Value = Maximum;
            }

            UpdateChildPosition(Value);
            UpdateScrollSliderPosition(_isLargeTextBlock ? Value * (1 - _scrollValueAdjuster) : Value);
        }

        /// <summary>
        /// Calculates the new Slider position and then makes a method call to
        /// visually modify the text position and slider position.
        /// </summary>
        private void UpdateScrollSliderValue()
        {
            var scrollBarValue = Orientation == Orientation.Vertical ? _initialSliderValue + (Ms.Y - _initialClickPosition.Y) : _initialSliderValue + (Ms.X - _initialClickPosition.X);

            Value = scrollBarValue * _textValueAdjuster;
            
            if (Value <= Minimum)
            {
                Value = Minimum;
                scrollBarValue = Minimum * (1 - _scrollValueAdjuster);
            }
            else if (Value >= Maximum)
            {
                Value = Maximum;
                scrollBarValue = _isLargeTextBlock ? Maximum * (1 - _scrollValueAdjuster) : Maximum;
            }

            UpdateChildPosition(Value);
            UpdateScrollSliderPosition(scrollBarValue);
        }

        /// <summary>
        /// Updates the position of the slider.
        /// </summary>
        /// <param name="value"></param>
        private void UpdateScrollSliderPosition(double value)
        {
            var z = Orientation == Orientation.Vertical ? new Thickness(0, _upArrow.Height + value, 0, 0) : new Thickness(value < Maximum ? _upArrow.Width + value : _upArrow.Width + value - 2, 0, 0, 0);
            _scrollBar.Margin = z;

            var m = Orientation == Orientation.Vertical ? new Thickness(0, value, 0, 0) : new Thickness(value < Maximum ? value : value - 2, 0, 0, 0);

            _scrollBarNormal[0].Margin = m;
            _scrollBarHover[0].Margin = m;
            _scrollBarMouseDown[0].Margin = m;
            
            _scrollBarNormal[1].Margin = m;
            _scrollBarHover[1].Margin = m;
            _scrollBarMouseDown[1].Margin = m;

            m = Orientation == Orientation.Vertical ? new Thickness(0, 0, 0, -value) : new Thickness(0, 0, value < Maximum ? -value : -value + 2, 0);
            _scrollBarNormal[2].Margin = m;
            _scrollBarHover[2].Margin = m;
            _scrollBarMouseDown[2].Margin = m;
        }

        /// <summary>
        /// Sets the mouse down state to hidden.
        /// </summary>
        private void UpArrowReleased()
        {
            if (_upArrow.Panel.Contains(Ms.X, Ms.Y))
            {
                _upArrow.Children[0].Visibility = Visibility.Hidden;
                _upArrow.Children[2].Visibility = Visibility.Hidden;
                _upArrow.Children[1].Visibility = Visibility.Visible;
            }
            else
            {
                _upArrow.Children[1].Visibility = Visibility.Hidden;
                _upArrow.Children[2].Visibility = Visibility.Hidden;
                _upArrow.Children[0].Visibility = Visibility.Visible;
            }

            _mouseDownUpArrow = false;
        }

        /// <summary>
        /// Sets the mouse down state to hidden.
        /// </summary>
        private void DownArrowReleased()
        {
            if (_downArrow.Panel.Contains(Ms.X, Ms.Y))
            {
                _downArrow.Children[0].Visibility = Visibility.Hidden;
                _downArrow.Children[2].Visibility = Visibility.Hidden;
                _downArrow.Children[1].Visibility = Visibility.Visible;
            }
            else
            {
                _downArrow.Children[1].Visibility = Visibility.Hidden;
                _downArrow.Children[2].Visibility = Visibility.Hidden;
                _downArrow.Children[0].Visibility = Visibility.Visible;
            }

            _mouseDownDownArrow = false;
        }

        /// <summary>
        /// Updates the visibility of the scroll bar on mouse up.
        /// </summary>
        private void SliderReleased()
        {
            _scrollSliderMouseDown = false;
            _initialClickPosition = Vector2.Zero;

            if (_scrollBar.Panel.Contains(Ms.X, Ms.Y))
            {
                for (var i = 0; i < _scrollBarNormal.Count; i++)
                {
                    _scrollBarNormal[i].Visibility = Visibility.Hidden;
                    _scrollBarMouseDown[i].Visibility = Visibility.Hidden;
                    _scrollBarHover[i].Visibility = Visibility.Visible;
                }
            }
            else
            {
                for (var i = 0; i < _scrollBarNormal.Count; i++)
                {
                    _scrollBarHover[i].Visibility = Visibility.Hidden;
                    _scrollBarMouseDown[i].Visibility = Visibility.Hidden;
                    _scrollBarNormal[i].Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Updates the position of the child.
        /// </summary>
        private void UpdateChildPosition(double value)
        {
            var m = Child.Margin;

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Child.Margin = new Thickness(m.Left, -value, m.Right, m.Bottom);
                    break;

                case Orientation.Horizontal:
                    Child.Margin = new Thickness(-value, m.Top, m.Right, m.Bottom);
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
        /// Gives focus to the child when a mouse down occurs on it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void ChildOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!Child.IsFocused)
            {
                Child.IsFocused = true;
            }
        }

        /// <summary>
        /// Sets the hover state to visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void UpArrowOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_scrollSliderMouseDown || _mouseDownDownArrow)
            {
                return;
            }

            _upArrow.Children[0].Visibility = Visibility.Hidden;

            if (_mouseDownUpArrow)
            {
                _upArrow.Children[1].Visibility = Visibility.Hidden;
                _upArrow.Children[2].Visibility = Visibility.Visible;
                _upArrowHasLeftWhilePressed = false;
            }
            else
            {
                _upArrow.Children[2].Visibility = Visibility.Hidden;
                _upArrow.Children[1].Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the hover state to hidden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void UpArrowOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_mouseDownDownArrow || _scrollSliderMouseDown)
            {
                return;
            }

            _upArrow.Children[2].Visibility = Visibility.Hidden;

            if (_mouseDownUpArrow)
            {
                _upArrow.Children[0].Visibility = Visibility.Hidden;
                _upArrow.Children[1].Visibility = Visibility.Visible;
                _upArrowHasLeftWhilePressed = true;
            }
            else
            {
                _upArrow.Children[1].Visibility = Visibility.Hidden;
                _upArrow.Children[0].Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the mouse down state to visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void UpArrowOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_scrollSliderMouseDown || _mouseDownDownArrow)
            {
                return;
            }

            _upArrow.Children[0].Visibility = Visibility.Hidden;
            _upArrow.Children[1].Visibility = Visibility.Hidden;
            _upArrow.Children[2].Visibility = Visibility.Visible;

            _scrollTimer = TimeSpan.Zero;
            _mouseDownUpArrow = true;
        }

        /// <summary>
        /// Sets the hover state to visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void DownArrowOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_scrollSliderMouseDown || _mouseDownUpArrow)
            {
                return;
            }

            _downArrow.Children[0].Visibility = Visibility.Hidden;

            if (_mouseDownDownArrow)
            {
                _downArrow.Children[1].Visibility = Visibility.Hidden;
                _downArrow.Children[2].Visibility = Visibility.Visible;
                _downArrowHasLeftWhilePressed = false;
            }
            else
            {
                _downArrow.Children[2].Visibility = Visibility.Hidden;
                _downArrow.Children[1].Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the hover state to hidden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void DownArrowOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_mouseDownUpArrow || _scrollSliderMouseDown)
            {
                return;
            }

            _downArrow.Children[2].Visibility = Visibility.Hidden;

            if (_mouseDownDownArrow)
            {
                _downArrow.Children[0].Visibility = Visibility.Hidden;
                _downArrow.Children[1].Visibility = Visibility.Visible;
                _downArrowHasLeftWhilePressed = true;
            }
            else
            {
                _downArrow.Children[1].Visibility = Visibility.Hidden;
                _downArrow.Children[0].Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the mouse down state to visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void DownArrowOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_scrollSliderMouseDown || _mouseDownUpArrow)
            {
                return;
            }

            _downArrow.Children[0].Visibility = Visibility.Hidden;
            _downArrow.Children[1].Visibility = Visibility.Hidden;
            _downArrow.Children[2].Visibility = Visibility.Visible;

            _scrollTimer = TimeSpan.Zero;
            _mouseDownDownArrow = true;
        }

        /// <summary>
        /// Updates the visibility of the scroll bar on mouse down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void ScrollBarOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_mouseDownDownArrow || _mouseDownUpArrow)
            {
                return;
            }

            for (var i = 0; i < _scrollBarNormal.Count; i++)
            {
                _scrollBarNormal[i].Visibility = Visibility.Hidden;
                _scrollBarHover[i].Visibility = Visibility.Hidden;
                _scrollBarMouseDown[i].Visibility = Visibility.Visible;
            }

            _initialSliderValue = _isLargeTextBlock ? Value * (1 - _scrollValueAdjuster) : Value;
            _initialClickPosition = MousePressPosition;
            _scrollSliderMouseDown = true;
        }

        /// <summary>
        /// Updates the visibility of the scroll bar on mouse leave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void ScrollBarOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (Ms.LeftButton == ButtonState.Released) 
            {
                for (var i = 0; i < _scrollBarNormal.Count; i++)
                {
                    _scrollBarHover[i].Visibility = Visibility.Hidden;
                    _scrollBarMouseDown[i].Visibility = Visibility.Hidden;
                    _scrollBarNormal[i].Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Updates the visibility of the scroll bar on mouse enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void ScrollBarOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_mouseDownDownArrow || _mouseDownUpArrow)
            {
                return;
            }

            if (!_scrollSliderMouseDown) 
            {
                for (var i = 0; i < _scrollBarNormal.Count; i++)
                {
                    _scrollBarNormal[i].Visibility = Visibility.Hidden;
                    _scrollBarMouseDown[i].Visibility = Visibility.Hidden;
                    _scrollBarHover[i].Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Disposes of the XAMLite Objects.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Child != null && _childTextHeight > Child.Height)
            {
                _upArrow.MouseEnter -= UpArrowOnMouseEnter;
                _upArrow.MouseLeave -= UpArrowOnMouseLeave;
                _upArrow.MouseDown -= UpArrowOnMouseDown;

                _downArrow.MouseEnter -= DownArrowOnMouseEnter;
                _downArrow.MouseLeave -= DownArrowOnMouseLeave;
                _downArrow.MouseDown -= DownArrowOnMouseDown;

                _scrollBar.MouseEnter -= ScrollBarOnMouseEnter;
                _scrollBar.MouseLeave -= ScrollBarOnMouseLeave;
                _scrollBar.MouseDown -= ScrollBarOnMouseDown;
            }

            foreach (var child in Children)
            {
                child.Dispose();
            }

            if (_scrollBar == null)
            {
                return;
            }

            foreach (var child in _scrollBar.Children)
            {
                child.Dispose();
            }

            foreach (var child in _downArrow.Children)
            {
                child.Dispose();
            }

            foreach (var child in _upArrow.Children)
            {
                child.Dispose();
            }

            //_scrollBarNormal = null;
            //_scrollBarHover = null;
            //_scrollBarMouseDown = null;
        }
    }
}
