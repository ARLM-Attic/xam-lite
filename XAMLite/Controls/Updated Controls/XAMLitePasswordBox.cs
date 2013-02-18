using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>Represents a control designed for entering and handling passwords.
    /// </summary>
    public class XAMLitePasswordBox : XAMLiteTextBoxNew
    {
        /// <summary>
        /// Gets or sets the password currently held by the XAMLitePasswordBox.
        /// </summary>
        public string Password
        {
            get
            {
                return Text;
            }

            set
            {
                Text = value;
            }
        }

        /// <summary>
        /// The previous text by which it is determined that the text has changed.
        /// </summary>
        private string _previousText;

        /// <summary>
        /// The filled circles that mask the actual password text.
        /// </summary>
        private XAMLiteLabelNew _passwordMask;

        /// <summary>
        /// The only visible character in the string.
        /// </summary>
        private XAMLiteLabelNew _currentCharacter;

        /// <summary>
        /// Gets or sets the masking character for the XAMLitePasswordBox.
        /// </summary>
        public char PasswordChar { get; set; }

        /// <summary>
        /// The count down from when a character displays briefly on the screen 
        /// to when it becomes masked.
        /// </summary>
        private TimeSpan _maskCharacter;

        /// <summary>
        /// Initializes a control designed for entering and handling passwords.
        /// </summary>
        /// <param name="game"></param>
        public XAMLitePasswordBox(Game game)
            : base(game)
        {
            PasswordChar = 'n';
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _previousText = Text;

            _passwordMask = new XAMLiteLabelNew(Game)
                {
                    FontFamily = new FontFamily("Webdings08"),
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(BorderThickness.Left > 1 ? Padding.Left + BorderThickness.Left : Padding.Left,
                    BorderThickness.Top > 1 ? Padding.Top + BorderThickness.Top : Padding.Top, 0, 0),
                    HorizontalAlignment = TextAlignment == TextAlignment.Left
                    || TextAlignment == TextAlignment.Justify ?
                    HorizontalAlignment.Left : TextAlignment == TextAlignment.Center ?
                    HorizontalAlignment.Center : HorizontalAlignment.Right,
                    Spacing = Spacing
                };
            Children.Add(_passwordMask);

            _currentCharacter = new XAMLiteLabelNew(Game)
                {
                    FontFamily = FontFamily,
                    Spacing = Spacing,
                    Content = " ",
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(BorderThickness.Left > 1 ? Padding.Left + BorderThickness.Left : Padding.Left,
                    BorderThickness.Top > 1 ? Padding.Top + BorderThickness.Top : Padding.Top, 0, 0),
                    HorizontalAlignment = TextAlignment == TextAlignment.Left
                    || TextAlignment == TextAlignment.Justify ?
                    HorizontalAlignment.Left : TextAlignment == TextAlignment.Center ?
                    HorizontalAlignment.Center : HorizontalAlignment.Right,
                    Visibility = Visibility.Hidden
                };
            Children.Add(_currentCharacter);

            _previousText = Text;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // If the text is the initial text, hide the mask.
            if (Text == InitialText && _passwordMask.Visibility == Visibility.Visible)
            {
                _passwordMask.Visibility = Visibility.Hidden;
            }
            else if (Text != _previousText && Text.Length < _previousText.Length)
            {
                // If password character have been deleted.
                // set the previous text to the new text.
                _previousText = Text;
                // update the mask and cursor positions.
                UpdateMask(Text.Length);
                _passwordMask.Visibility = Visibility.Visible;

                UpdateCursor(_passwordMask.MeasureString().X);
            }
            else if (Text != _previousText && Text != string.Empty && Text != InitialText)
            {
                // if password characters have increased...
                // mask all of the text with filled characters, except the most 
                // recently pressed key.
                UpdateMask(Text.Length - 1);
                _passwordMask.Visibility = Visibility.Visible;

                var pos = _passwordMask.MeasureString().X + Spacing;

                // make the most recently pressed key visible.
                _currentCharacter.Visibility = Visibility.Visible;
                _currentCharacter.Content = Text == string.Empty
                                                ? ""
                                                : Text[Text.Length - 1].ToString(CultureInfo.InvariantCulture);
                // update the exposed character position.
                _currentCharacter.Margin = new Thickness(pos, 0, 0, 0);

                // update the cursor position.
                UpdateCursor(pos + _currentCharacter.MeasureString().X);
                
                // set the timer for when to replace the exposed character with the mask.
                _maskCharacter = TimeSpan.FromMilliseconds(800);

                // set the previous text to the new text.
                _previousText = Text;
            }
            else
            {
                // if the most recently pressed key is exposed, the timer will count down.
                if (_currentCharacter.Visibility == Visibility.Visible)
                {
                    _maskCharacter -= gameTime.ElapsedGameTime;

                    // When the timer reaches zero
                    if (_maskCharacter <= TimeSpan.Zero)
                    {
                        // hide the character and mask it.
                        _currentCharacter.Visibility = Visibility.Hidden;

                        // Update the mask and cursor positions.
                        UpdateMask(Text.Length);
                        _passwordMask.Visibility = Visibility.Visible;
                        UpdateCursor(_passwordMask.MeasureString().X);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the cursor position.
        /// </summary>
        /// <param name="leftPadding"></param>
        private void UpdateCursor(double leftPadding)
        {
            TextCursor.Padding = new Thickness(Padding.Left + leftPadding + Spacing, Padding.Top, 0, 0);
        }

        /// <summary>
        /// Updates the mask that covers the actual password text.
        /// </summary>
        /// <param name="length"></param>
        private void UpdateMask(int length)
        {
            _passwordMask.Content = "";
            for (var i = 0; i < length; i++)
            {
                _passwordMask.Content += "n";
            }
        }
    }
}
