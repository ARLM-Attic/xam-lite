// -----------------------------------------------------------------------
// <copyright file="XAMLiteSlider.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace XAMLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Represents a control that lets the user select from a range of values 
    /// by moving a slider control along a track.
    /// </summary>
    public class XAMLiteSlider : XAMLiteGridNew
    {
        /// <summary>
        /// Gets or sets the highest possible Value of the range element.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the Minimum possible Value of the range element.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the current magnitude of the range control.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// True when a mouse down is occurring on the slider bar.
        /// </summary>
        private bool _buttonMouseDown;

        /// <summary>
        /// Initial position when a mouse down event occurs on the scroll bar slider.
        /// </summary>
        private Vector2 _initialClickPosition;

        /// <summary>
        /// The Value when a mouse Down first occurs on the Slider.
        /// </summary>
        private double _initialSliderValue;

        /// <summary>
        /// The total amount of possible change within the minimum and maximum values.
        /// </summary>
        private double _range;

        /// <summary>
        /// Value change per one slider change.
        /// </summary>
        private double _slideAdjuster;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteButtonNew button;
        
        /// <summary>
        /// Represents a control that lets the user select from a range of values 
        /// by moving a slider control along a track.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteSlider(Game game)
            : base(game)
        {
            Height = 23;
            Width = 100;
            Minimum = 0;
            Maximum = Width;
            Value = Minimum;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            var leftSideBar = new XAMLiteImageNew(Game)
                {
                    SourceName = "Icons/slider-bar-left"
                };
            Children.Add(leftSideBar);

            var rightSideBar = new XAMLiteImageNew(Game)
                {
                    SourceName = "Icons/slider-bar-left",
                    RenderTransform = RenderTransform.FlipHorizontal
                };
            Children.Add(rightSideBar);

            var centerBar = new XAMLiteImageNew(Game)
                {
                    SourceName = "Icons/slider-bar-center",
                    Width = Width - 4,
                    Margin = new Thickness(2, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
            Children.Add(centerBar);

            var b = Game.Content.Load<Texture2D>("Icons/slider-button-normal");

            _range = Maximum - Minimum;
            _slideAdjuster = _range / (Width - b.Width);
            _initialSliderValue = (Value - Minimum) / _slideAdjuster;

            button = new XAMLiteButtonNew(Game)
                {
                    SourceName = "Icons/slider-button-normal",
                    RolloverSourceName = "Icons/slider-button-hover",
                    ClickSourceName = "Icons/slider-button-down",
                    Margin = new Thickness(_initialSliderValue, 0, 0, 0)
                };
            button.MouseDown += ButtonOnMouseDown;
            Children.Add(button);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_buttonMouseDown)
            {
                UpdateValue();
            }

            if (_buttonMouseDown && Ms.LeftButton == ButtonState.Released)
            {
                _buttonMouseDown = false;
                _initialClickPosition = Vector2.Zero;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sliderValue"></param>
        private void UpdateSliderBar(double sliderValue)
        {
            button.Margin = new Thickness(_initialSliderValue + sliderValue, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateValue()
        {
            double sliderValue = Ms.X - _initialClickPosition.X;

            if (_initialSliderValue + sliderValue >= _range / _slideAdjuster)
            {
                sliderValue = (_range / _slideAdjuster) - _initialSliderValue;
                Value = Maximum;
            }
            else if ((_initialSliderValue + sliderValue) <= 0)
            {
                sliderValue = -_initialSliderValue;
                Value = Minimum;
            }
            else
            {
                Value = (_initialSliderValue + sliderValue) * _slideAdjuster;
            }

            UpdateSliderBar(sliderValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void ButtonOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _initialSliderValue = button.Margin.Left;
            _initialClickPosition = MousePressPosition;
            
            _buttonMouseDown = true;
        }

        /// <summary>
        /// Disposes of the elements that make up the slider.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (button != null)
            {
                button.MouseDown -= ButtonOnMouseDown;
            }

            foreach (var child in Children)
            {
                child.Dispose();
            }
        }
    }
}
