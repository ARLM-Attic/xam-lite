using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace XAMLite
{
    public class XAMLiteTextBoxNew : XAMLiteGridNew
    {
        /// <summary>
        /// Adjusts the width of the TextBox.
        /// </summary>
        public override int Width
        {
            get
            {
                return base.Width;
            }

            set
            {
                base.Width = value;

                if (fill != null)
                {
                    fill.Width = Width;
                }
            }
        }

        private string _text;

        /// <summary>
        /// Sets the content of the label within the text box.
        /// </summary>
        public string Text
        {
            get
            {
                if (_textLabel != null)
                {
                    return _textLabel.Content.ToString();
                }

                return _text;
            }

            set
            {
                _text = value;

                if (_textLabel != null)
                {
                    _textLabel.Content = value;
                }
            }
        }

        /// <summary>
        /// Character spacing.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        protected FontFamily _fontFamily;

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
                FontFamilyChanged = true;
            }
        }

        /// <summary>
        /// True when the font family has changed.
        /// </summary>
        protected bool FontFamilyChanged;

        /// <summary>
        /// The padding that surrounds the text within the control.  Note, in 
        /// WPF, only the top and left can be set.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// The color of the content, whether text or some other object.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// When true, the text is not editable and the blinking cursor will not appear.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Sets the alignment of the text.
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// The border color.
        /// </summary>
        public virtual Brush BorderBrush { get; set; }

        /// <summary>
        /// The border thickness.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// Sets the max number of characters allowed in the text box
        /// </summary>
        public int MaxLength;

        /// <summary>
        /// The background of the text box.
        /// </summary>
        private XAMLiteRectangleNew fill;

        /// <summary>
        /// Contains all of the XAMLiteRectangles that make up the border.
        /// </summary>
        private List<XAMLiteRectangleNew> _borderRectangles;

        /// <summary>
        /// The text contained in the text box.
        /// </summary>
        private XAMLiteLabelNew _textLabel;

        /// <summary>
        /// Default text as designated by the developer.
        /// </summary>
        internal string InitialText;

        /// <summary>
        /// Initial Padding as defined by the developer.  Included with
        /// measuring where the cursor is placed.
        /// </summary>
        private double _initialPadding;

        /// <summary>
        /// The label containing the cursor.
        /// </summary>
        internal XAMLiteLabelNew TextCursor;

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
        /// When true, the cursor is overridden.
        /// </summary>
        internal bool IsCursorOveride;

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
        /// 
        /// </summary>
        private static bool _locked;

        /// <summary>
        /// 
        /// </summary>
        private bool _isLocked;

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
            InitialText = string.Empty;
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

            InitialText = Text;
            _initialPadding = Padding.Left;

            _textLabel = new XAMLiteLabelNew(Game)
            {
                Content = Text,
                HorizontalAlignment = TextAlignment == TextAlignment.Left 
                    || TextAlignment == TextAlignment.Justify ? 
                    HorizontalAlignment.Left : TextAlignment == TextAlignment.Center ? 
                    HorizontalAlignment.Center : HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily,
                Spacing = Spacing,
                Foreground = Foreground,
                Padding = new Thickness(BorderThickness.Left > 1 ? Padding.Left + BorderThickness.Left : Padding.Left,
                    BorderThickness.Top > 1 ? Padding.Top + BorderThickness.Top : Padding.Top, 0, 0),
                DrawOrder = DrawOrder + 2,
                Visibility = Visibility.Hidden
            };

            // the developer did not set a specific Width and therefore, the 
            // control needs to be quickly added to get a measurement.  Then 
            // it is removed and added to the grid.
            if (_textLabel.Width == 0)
            {
                Game.Components.Add(_textLabel);
            }

            // get the width and height of the text label to make sure the 
            // control will be large enough to contain it.
            if (Width < (int)_textLabel.MeasureString().X + (int)_textLabel.Padding.Left + (int)_textLabel.Padding.Right)
            {
                var pl = 0;

                if (_textLabel.Padding.Right == 0)
                {
                    pl = _textLabel.Padding.Left > 0 ? (int)_textLabel.Padding.Left : 5;
                }

                Width = (int)_textLabel.MeasureString().X + (int)_textLabel.Padding.Left + (int)_textLabel.Padding.Right + pl;
            }

            if (Height < (int)_textLabel.MeasureString().Y + (int)_textLabel.Padding.Top + (int)_textLabel.Padding.Bottom)
            {
                Height = (int)_textLabel.MeasureString().Y + (int)_textLabel.Padding.Top + (int)_textLabel.Padding.Bottom;
            }

            // If it was added as a component already, then remove it so it can 
            // be added to the grid.
            if (Game.Components.Contains(_textLabel))
            {
                Game.Components.Remove(_textLabel);
            }

            fill = new XAMLiteRectangleNew(Game)
            {
                Fill = Background,
                Width = Width,
                Height = Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
            };

            Children.Add(fill);

            Children.Add(_textLabel);

            if (!IsReadOnly)
            {
                TextCursor = new XAMLiteLabelNew(Game)
                    {
                        Content = TextBoxCursor,
                        HorizontalAlignment = _textLabel.HorizontalAlignment,
                        VerticalAlignment = _textLabel.VerticalAlignment,
                        FontFamily = FontFamily,
                        Spacing = Spacing,
                        Foreground = Foreground,
                        Padding = _textLabel.Padding,
                        Visibility = Visibility.Hidden,
                        DrawOrder = DrawOrder
                    };
                Children.Add(TextCursor);
            }

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
                            StrokeThickness = BorderThickness.Left,
                            DrawOrder = DrawOrder
                        };
                    _borderRectangles.Add(border);
                }
                else
                {
                    SetBorders();
                }

                foreach (var borderRectangle in _borderRectangles)
                {
                    Children.Add(borderRectangle);
                }
            }

            Background = Brushes.Transparent;
        }

        /// <summary>
        /// Initializes the event handlers.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseEnter += OnMouseEnter;
            MouseUp += OnMouseUp;
            MouseLeave += OnMouseLeave;
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
                VerticalAlignment = VerticalAlignment.Top,
                DrawOrder = DrawOrder
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
                DrawOrder = DrawOrder
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
                DrawOrder = DrawOrder
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
                DrawOrder = DrawOrder
            };

            _borderRectangles.Add(bottomBorder);
        }

        /// <summary>
        /// Updates the text box.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this is XAMLitePasswordBox && (_textLabel.Content.ToString() != string.Empty && _textLabel.Content.ToString() != InitialText))
            {
                _textLabel.Visibility = Visibility.Hidden;
            }

            if (!_isLocked)
            {
                if (_textLabel.Content.ToString() == string.Empty && Parent != null && !(Parent is XAMLiteComboBox))
                {
                    _textLabel.Content = InitialText;
                }
            }

            if (CurrentTabIndex == TabIndex && !_locked && !_isLocked && Ms.LeftButton == ButtonState.Released)
            {
                _isLocked = true;

                foreach (var borderRectangle in _borderRectangles)
                {
                    borderRectangle.Stroke = Brushes.Blue;
                }

                if ((string)_textLabel.Content == InitialText)
                {
                    _textLabel.Content = string.Empty;
                }

                _cursorVisible = true;

                IsFocused = true;
                _locked = true;
            }

            if (!IsFocused && Ms.LeftButton == ButtonState.Released)
            {
                if (_isLocked)
                {
                    _isLocked = false;
                }

                if (!IsReadOnly)
                {
                    TextCursor.Visibility = Visibility.Hidden;
                    _cursorVisible = false;
                    _cursorBlink = false;
                }

                if (_textLabel.Content != null && _textLabel.Content.ToString() == string.Empty)
                {
                    if (Parent != null && !(Parent is XAMLiteComboBox))
                    {
                        _textLabel.Content = InitialText;
                        _textLabel.Visibility = Visibility.Visible;
                    }

                    if (_borderRectangles[0].Stroke != BorderBrush)
                    {
                        ResetBorderBrush();
                    }
                }
            }

            if (!IsReadOnly)
            {
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

                TextCursor.Visibility = _cursorBlink ? Visibility.Visible : Visibility.Hidden;

                ProcessInput(gameTime);

                UpdateTextAndCursor();
            }

            UpdateBorders();

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

                IsFocused = false;
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
        /// Updates the cursor position.
        /// </summary>
        private void UpdateTextAndCursor()
        {
            if (!(this is XAMLitePasswordBox))
            {
                TextCursor.Padding = new Thickness(
                    _initialPadding + _textLabel.MeasureString().X + Spacing, Padding.Top, 0, 0);
            }
        }

        /// <summary>
        /// Changes the default cursor to the IBeam.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsCursorOveride)
            {
                Cursor.Cursors = Cursors.IBeam;
            }
        }

        /// <summary>
        /// Handles when the textbox is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (Parent != null && Parent is XAMLiteComboBox)
            {
                return;
            }

            _locked = true;
            IsFocused = true;
            _isLocked = true;

            if ((string)_textLabel.Content == InitialText)
            {
                _textLabel.Content = string.Empty;
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

            Cursor.Cursors = Cursors.Arrow;
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

            if (!(this is XAMLitePasswordBox))
            {
                CursorPosition.X = CursorStartPosition + _textLabel.MeasureString().X + 2;
            }
        }

        /// <summary>
        /// Starts or stops key deletion.
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

            if (_textLabel.Content.ToString().Length >= MaxLength && key != Keys.Back && key != Keys.Delete &&
                key != Keys.Tab && key != Keys.Enter && _textLabel.MeasureString().X >=
                Width - _textLabel.Padding.Left - BorderThickness.Right - BorderThickness.Left - 6)
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
                        _locked = false;
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

            _textLabel.Content += newChar;
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
            if (_textLabel.Content.ToString().Length != 0)
            {
                _textLabel.Content = _textLabel.Content.ToString().Remove(_textLabel.Content.ToString().Length - 1);
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

            foreach (var child in Children)
            {
                child.Dispose();
            }
        }
    }
}
