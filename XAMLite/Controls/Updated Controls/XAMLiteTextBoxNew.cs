using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace XAMLite
{
    using System.Collections.Generic;

    public class XAMLiteTextBoxNew : XAMLiteBaseText
    {
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
        private XAMLiteLabelNew _text;

        /// <summary>
        /// Default text as designated by the developer.
        /// </summary>
        private string _initialText;

        /// <summary>
        /// Initial Padding as defined by the developer.  Included with
        /// measuring where the cursor is placed.
        /// </summary>
        private double _initialPadding;

        /// <summary>
        /// The label containing the cursor.
        /// </summary>
        private XAMLiteLabelNew _cursor;

        /// <summary>
        /// The current position of the cursor.
        /// </summary>
        protected Vector2 CursorPosition;

        /// <summary>
        /// Starting position, on the X-Axis of where the cursor should
        /// begin from.
        /// </summary>
        protected float CursorStartPosition;

        /// <summary>
        /// Standard upper/lowercase letters and numbers.
        /// </summary>
        protected Keys[] StandardInputKeys;

        /// <summary>
        /// Special characters such as semicolons, brackets, etc.
        /// </summary>
        protected Keys[] SpecialInputKeys;

        /// <summary>
        /// The character '|' that makes the blinking cursor.
        /// </summary>
        protected string TextBoxCursor;
        
        private bool _cursorVisible;
        
        private bool _cursorBlink;
        
        private TimeSpan _cursorBlinkTime;

        private bool _keyShift;
        
        private TimeSpan _keyShiftTimer;
        
        private bool _keyShiftTimerStarted;
        
        private bool _capsLockOn;
        
        private bool _backspaceheld;
        
        private bool _deleteNextChar;
        
        private TimeSpan _deleteTimer;
        
        private int _numDeletedKeys;
        
        private bool _standardKeyTyped;

        private KeyboardState _currentKeyboardState;
        
        private KeyboardState _lastKeyboardState;

        /// <summary>
        /// TODO: Consider creating a base class for complex controls
        /// TODO: so that adding all of these parts are not necessary every time.
        /// </summary>
        /// <summary>
        /// TODO: Consider creating a base class for complex controls
        /// TODO: so that adding all of these parts are not necessary every time.
        /// </summary>
        public override Visibility Visible
        {
            get
            {
                return base.Visible;
            }

            set
            {
                base.Visible = value;

                if (_grid != null)
                {
                    _grid.Visible = value;
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBoxNew(Game game)
            : base(game)
        {
            TextAlignment = TextAlignment.Left;
            FontFamily = new FontFamily("Arial");
            Focusable = true;
            Spacing = 2;
            Width = 120;
            Height = 23;
            Foreground = Brushes.Black;
            Padding = new Thickness(5, 0, 0, 0);
            TextBoxCursor = "|";
            _initialText = string.Empty;
            _cursorBlinkTime = TimeSpan.FromSeconds(0.5);
            BorderBrush = null;
            _borderRectangles = new List<XAMLiteRectangleNew>();

            _deleteNextChar = true;

            _numDeletedKeys = 0;

            SpecialInputKeys = new[]
                {
                    Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
                    Keys.OemQuotes, Keys.OemTilde, Keys.OemComma, Keys.OemPeriod, Keys.OemSemicolon, Keys.OemBackslash,
                    Keys.OemCloseBrackets, Keys.OemOpenBrackets, Keys.OemPlus, Keys.OemMinus, Keys.OemQuestion,
                    Keys.OemPipe
                };

            StandardInputKeys = new[]
                {
                    Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, 
                    Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
                    Keys.Z, Keys.Space, Keys.Enter, Keys.OemClear, Keys.Decimal, Keys.Tab, Keys.Add, Keys.Subtract,
                    Keys.Multiply, Keys.Divide, Keys.CapsLock, //Keys.Home,
                    //Keys.End, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.PageUp, Keys.PageDown, Keys.Insert,
                    Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6,
                    Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
                };
        }

        /// <summary>
        /// Loads the text box content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _initialText = Text;
            _initialPadding = Padding.Left;

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

            _text = new XAMLiteLabelNew(Game)
                {
                    Content = Text,
                    HorizontalAlignment = TextAlignment == TextAlignment.Left || TextAlignment == TextAlignment.Justify ? HorizontalAlignment.Left : TextAlignment == TextAlignment.Center ? HorizontalAlignment.Center : HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontFamily = FontFamily,
                    Spacing = Spacing,
                    Foreground = Foreground,
                    Padding = new Thickness(BorderThickness.Left > 1 ? Padding.Left + BorderThickness.Left : Padding.Left, 
                        BorderThickness.Top > 1 ? Padding.Top + BorderThickness.Top : Padding.Top, 0, 0),
                };
            _grid.Children.Add(_text);

            _cursor = new XAMLiteLabelNew(Game)
                {
                    Content = TextBoxCursor,
                    HorizontalAlignment = _text.HorizontalAlignment,
                    VerticalAlignment = _text.VerticalAlignment,
                    FontFamily = FontFamily,
                    Spacing = Spacing,
                    Foreground = Foreground,
                    Padding = _text.Padding,
                    Visible = Visibility.Hidden
                };
            _grid.Children.Add(_cursor);

            if (BorderBrush == null)
            {
                SetBorders();
            }

            // Create the borders of the control, if they are set.
            if (BorderThickness.Left > 0)
            {
                if (BorderThickness.Left == BorderThickness.Right &&
                   BorderThickness.Right == BorderThickness.Top &&
                    BorderThickness.Top == BorderThickness.Bottom)
                {
                    var border = new XAMLiteRectangleNew(Game)
                        {
                            Stroke = BorderBrush,
                            StrokeThickness = BorderThickness.Left
                        };
                    _borderRectangles.Add(border);
                }
                else
                {
                    SetBorders();
                }

                foreach (var borderRectangle in _borderRectangles)
                {
                    _grid.Children.Add(borderRectangle);
                }
            }

            MouseUp += OnMouseUp;
            MouseLeave += OnMouseLeave;

            Background = Brushes.Transparent;
        }

        /// <summary>
        /// Sets the border colors of the textbox.
        /// </summary>
        private void SetBorders()
        {
            var nullbrush = false;

            // Set this to a defined default value.
            if (BorderBrush == null)
            {
                nullbrush = true;
                BorderThickness = new Thickness(1);
            }

            var leftBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = nullbrush ? Brushes.Black : BorderBrush,
                Opacity = nullbrush ? 0.25f : Opacity,
                Width = (int)BorderThickness.Left,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            _borderRectangles.Add(leftBorder);

            var rightBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = nullbrush ? Brushes.Black : BorderBrush,
                Opacity = nullbrush ? 0.25f : Opacity,
                Width = (int)BorderThickness.Right,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
            };
            _borderRectangles.Add(rightBorder);

            var topBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = nullbrush ? Brushes.Black : BorderBrush,
                Opacity = nullbrush ? 0.5f : Opacity,
                Width = Width - 2,
                Height = (int)BorderThickness.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
            };
            _borderRectangles.Add(topBorder);

            var bottomBorder = new XAMLiteRectangleNew(Game)
            {
                Fill = nullbrush ? Brushes.Black : BorderBrush,
                Opacity = nullbrush ? 0.25f : Opacity,
                Width = Width - 2,
                Height = (int)BorderThickness.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            _borderRectangles.Add(bottomBorder);
        }

        /// <summary>
        /// Updates the text box.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!IsFocused)
            {
                _cursor.Visible = Visibility.Hidden;
                _cursorVisible = false;
                _cursorBlink = false;
                if (_text.Content.ToString() == string.Empty)
                {
                    _text.Content = _initialText;
                }
            }

            // handling the blinky cursor.
            if (_cursorVisible)
            {
                _cursorBlinkTime -= gameTime.ElapsedGameTime;
                if (_cursorBlinkTime <= TimeSpan.Zero)
                {
                    _cursorBlinkTime = TimeSpan.FromSeconds(0.5);
                    _cursorBlink = !_cursorBlink;
                }
            }

            _cursor.Visible = _cursorBlink ? Visibility.Visible : Visibility.Hidden;

            ProcessInput(gameTime);
            
            UpdateTextAndCursor();

            UpdateBorders();

            base.Update(gameTime);

            _lastKeyboardState = _currentKeyboardState;
        }

        /// <summary>
        /// Modifies the border brush when applicable.
        /// </summary>
        private void UpdateBorders()
        {
            // Changes the border brush when the control is hovered over.
            if (MouseEntered && Focusable && !IsFocused)
            {
                foreach (var borderRectangle in _borderRectangles)
                {
                    borderRectangle.Stroke = Brushes.Blue;
                }
            }

            // if a left mouse press occurs when not on the control, focus is 
            // lost and the control border brush should return to its default
            // state.
            if (IsFocused && Ms.LeftButton == ButtonState.Pressed && !MousePressed)
            {
                if (_borderRectangles[0].Stroke != BorderBrush)
                {
                    ResetBorderBrush();
                }
            }
        }

        /// <summary>
        /// Processes keystrokes.
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessInput(GameTime gameTime)
        {
            if (_backspaceheld)
            {
                _deleteTimer -= gameTime.ElapsedGameTime;
                if (_deleteTimer <= TimeSpan.Zero)
                {
                    _deleteNextChar = true;
                }
            }

            _currentKeyboardState = Keyboard.GetState();

            if ((_currentKeyboardState.IsKeyDown(Keys.RightShift) ||
                _currentKeyboardState.IsKeyDown(Keys.LeftShift)) && !_keyShiftTimerStarted)
            {
                _keyShiftTimer = TimeSpan.FromSeconds(0.1);
                _keyShift = true;
            }

            if (_keyShift)
            {
                _keyShiftTimer -= gameTime.ElapsedGameTime;
                if (_keyShiftTimer <= TimeSpan.Zero)
                {
                    _keyShift = false;
                    _keyShiftTimerStarted = false;
                }
            }

            if (IsFocused)
            {
                ProcessKeyboard();
            }
        }

        /// <summary>
        /// Updates the text for the label that is visually
        /// displayed.
        /// </summary>
        private void UpdateTextAndCursor()
        {
            Text = _text.Content.ToString();
            _cursor.Padding = new Thickness(_initialPadding + SpriteFont.MeasureString(Text).X + Spacing, Padding.Top, 0, 0);
        }

        /// <summary>
        /// Handles when the textbox is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            IsFocused = true;

            if ((string)_text.Content == _initialText)
            {
                _text.Content = string.Empty;
            }

            _cursorVisible = true;
        }

        /// <summary>
        /// Changes the border color back to its original border brush color
        /// when the control !IsFocused.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsFocused)
            {
                ResetBorderBrush();
            }
        }

        /// <summary>
        /// Resets the Border Brush for the textbox after the control either 
        /// loses focus, or on a MouseLeave when there was never focus.
        /// </summary>
        private void ResetBorderBrush()
        {
            foreach (var borderRectangle in _borderRectangles)
            {
                borderRectangle.Stroke = BorderBrush;
            }
        }

        /// <summary>
        /// Processes key board input.
        /// </summary>
        private void ProcessKeyboard()
        {
            foreach (var key in StandardInputKeys)
            {
                if (CheckKey(key))
                {
                    _standardKeyTyped = true;
                    AddKeyToText(key);
                    break;
                }
            }

            foreach (var key in SpecialInputKeys)
            {
                if (CheckKey(key))
                {
                    _standardKeyTyped = false;
                    AddKeyToText(key);
                    break;
                }
            }

            CursorPosition.X = CursorStartPosition + (int)SpriteFont.MeasureString(Text).X + 2;
        }

        /// <summary>
        /// Stats or stops key deletion.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CheckKey(Keys key)
        {
            if ((_currentKeyboardState.IsKeyDown(Keys.Back) ||
                _currentKeyboardState.IsKeyDown(Keys.Delete)) && _deleteNextChar)
            {
                StartDelete();
            }

            if (_currentKeyboardState.IsKeyUp(Keys.Back) &&
                _currentKeyboardState.IsKeyUp(Keys.Delete))
            {
                StopDelete();
            }

            return _lastKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Adds characters to the string.
        /// </summary>
        /// <param name="key"></param>
        private void AddKeyToText(Keys key)
        {
            var newChar = "";

            if (_text.Content.ToString().Length >= MaxLength && key != Keys.Back && key != Keys.Delete &&
                key != Keys.Tab && key != Keys.Enter && (int)SpriteFont.MeasureString(_text.Content.ToString()).X >=
                Width - _text.Padding.Left - BorderThickness.Right - BorderThickness.Left - 6)
            {
                return;
            }

            if (_standardKeyTyped)
            {
                switch (key)
                {
                    case Keys.Space:
                        newChar += " ";
                        break;
                    case Keys.Add:
                        newChar += "+";
                        break;
                    case Keys.Subtract:
                        newChar += "-";
                        break;
                    case Keys.Multiply:
                        newChar += "*";
                        break;
                    case Keys.Divide:
                        newChar += "/";
                        break;
                    case Keys.CapsLock:
                        _capsLockOn = !_capsLockOn;
                        break;
                    case Keys.Decimal:
                        newChar += ".";
                        break;
                    case Keys.NumPad7:
                        //case Keys.Home:
                        newChar += "7";
                        break;
                    case Keys.NumPad1:
                        //case Keys.End:
                        newChar += "1";
                        break;
                    case Keys.NumPad4:
                        //case Keys.Left:
                        newChar += "4";
                        break;
                    case Keys.NumPad5:
                        newChar += "5";
                        break;
                    case Keys.NumPad6:
                        //case Keys.Right:
                        newChar += "6";
                        break;
                    case Keys.NumPad8:
                        //case Keys.Up:
                        newChar += "8";
                        break;
                    case Keys.NumPad2:
                        //case Keys.Down:
                        newChar += "2";
                        break;
                    case Keys.NumPad9:
                        //case Keys.PageUp:
                        newChar += "9";
                        break;
                    case Keys.NumPad3:
                        //case Keys.PageDown:
                        newChar += "3";
                        break;
                    case Keys.NumPad0:
                        //case Keys.Insert:
                        newChar += "0";
                        break;
                    case Keys.OemClear:
                        Text = "";
                        return;
                    case Keys.Enter:
                    case Keys.Tab:
                        IsFocused = false;
                        ResetBorderBrush();
                        break;
                    default:
                        if (_keyShift || _capsLockOn)
                        {
                            newChar += key;
                        }
                        else
                        {
                            newChar += key.ToString().ToLower();
                        }

                        break;
                }
            }
            else
            {
                if (_keyShift)
                {
                    switch (key)
                    {
                        case Keys.D1:
                            newChar += "!";
                            break;
                        case Keys.D2:
                            newChar += "@";
                            break;
                        case Keys.D3:
                            newChar += "#";
                            break;
                        case Keys.D4:
                            newChar += "$";
                            break;
                        case Keys.D5:
                            newChar += "%";
                            break;
                        case Keys.D6:
                            newChar += "^";
                            break;
                        case Keys.D7:
                            newChar += "&";
                            break;
                        case Keys.D8:
                            newChar += "*";
                            break;
                        case Keys.D9:
                            newChar += "(";
                            break;
                        case Keys.D0:
                            newChar += ")";
                            break;
                        case Keys.OemQuotes:
                            newChar += "\"";
                            break;
                        case Keys.OemTilde:
                            newChar += "~";
                            break;
                        case Keys.OemComma:
                            newChar += "<";
                            break;
                        case Keys.OemPeriod:
                            newChar += ">";
                            break;
                        case Keys.OemSemicolon:
                            newChar += ":";
                            break;
                        case Keys.OemBackslash:
                            newChar += "|";
                            break;
                        case Keys.OemCloseBrackets:
                            newChar += "}";
                            break;
                        case Keys.OemOpenBrackets:
                            newChar += "{";
                            break;
                        case Keys.OemPlus:
                            newChar += "+";
                            break;
                        case Keys.OemMinus:
                            newChar += "_";
                            break;
                        case Keys.OemQuestion:
                            newChar += "?";
                            break;
                        case Keys.OemPipe:
                            newChar += "|";
                            break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case Keys.D1:
                            newChar += "1";
                            break;
                        case Keys.D2:
                            newChar += "2";
                            break;
                        case Keys.D3:
                            newChar += "3";
                            break;
                        case Keys.D4:
                            newChar += "4";
                            break;
                        case Keys.D5:
                            newChar += "5";
                            break;
                        case Keys.D6:
                            newChar += "6";
                            break;
                        case Keys.D7:
                            newChar += "7";
                            break;
                        case Keys.D8:
                            newChar += "8";
                            break;
                        case Keys.D9:
                            newChar += "9";
                            break;
                        case Keys.D0:
                            newChar += "0";
                            break;
                        case Keys.OemQuotes:
                            newChar += "\'";
                            break;
                        case Keys.OemTilde:
                            newChar += "`";
                            break;
                        case Keys.OemComma:
                            newChar += ",";
                            break;
                        case Keys.OemPeriod:
                            newChar += ".";
                            break;
                        case Keys.OemSemicolon:
                            newChar += ";";
                            break;
                        case Keys.OemBackslash:
                            newChar += "\\";
                            break;
                        case Keys.OemCloseBrackets:
                            newChar += "]";
                            break;
                        case Keys.OemOpenBrackets:
                            newChar += "[";
                            break;
                        case Keys.OemPlus:
                            newChar += "=";
                            break;
                        case Keys.OemMinus:
                            newChar += "-";
                            break;
                        case Keys.OemQuestion:
                            newChar += "/";
                            break;
                        case Keys.OemPipe:
                            newChar += "\\";
                            break;
                    }
                }
            }

            _text.Content += newChar;
            OnKeyDown();
        }

        /// <summary>
        /// Resets delete.
        /// </summary>
        private void StopDelete()
        {
            _backspaceheld = false;
            _deleteNextChar = true;
            _numDeletedKeys = 0;
            OnKeyDown();
        }

        /// <summary>
        /// Deletes text and increases speed the longer the delete key is held.
        /// </summary>
        private void StartDelete()
        {
            _deleteNextChar = false;
            _backspaceheld = true;
            _numDeletedKeys++;
            if (_text.Content.ToString().Length != 0)
            {
                _text.Content = _text.Content.ToString().Remove(Text.Length - 1);
            }

            if (_numDeletedKeys <= 2)
            {
                _deleteTimer = TimeSpan.FromSeconds(0.3);
            }
            else if (_numDeletedKeys <= 4)
            {
                _deleteTimer = TimeSpan.FromSeconds(0.2);
            }
            else
            {
                _deleteTimer = TimeSpan.FromSeconds(0.15);
            }
        }

        /// <summary>
        /// Dispose of the XAMLiteLabel that is used for the Content portion of
        /// the control.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            foreach (var child in _grid.Children)
            {
                child.Dispose();
            }
        }
    }
}
