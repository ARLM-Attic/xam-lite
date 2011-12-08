using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteTextBox : XAMLiteControl
    {

        /// <summary>
        /// 
        /// </summary>
        protected Texture2D textBoxTexture;

        protected Rectangle textBoxRectangle;
        protected Vector2 textPosition;
        protected Vector2 cursorPosition;
        protected Vector2 cursorStartPosition;

        protected Keys[] standardInputKeys;
        protected Keys[] specialInputKeys;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName
        {
            get;
            set;
        }

        protected string cursor;

        /// <summary>
        /// The character string within the text box.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.spriteFont != null)
                {
                    this.spriteFont.Spacing = Spacing;
                    RecalculateWidthAndHeight(value);
                }
                base.Text = value;
            }
        }

        /// <summary>
        /// Sets the alignment of the text.
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        private FontFamily _fontFamily;
        private bool fontFamilyChanged; // used in the Update() method

        /// <summary>
        /// Sets the font family for the text inside the text box.
        /// </summary>
        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; fontFamilyChanged = true; }
        }

        /// <summary>
        /// Character spacing for the font.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Sets the padding for the text within the text box.
        /// </summary>
        public Thickness Padding { get; set; }

        Vector2 paddedPosition;

        /// <summary>
        /// 
        /// </summary>
        private Color _foregroundColor;

        /// <summary>
        /// Sets the text color in the text box.
        /// </summary>
        public Brush Foreground
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _foregroundColor = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// Sets the background color of the text box
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                if ((SolidColorBrush)value == Brushes.Transparent)
                    transparent = true;
                else
                    transparent = false;
            }
        }

        private bool transparent;

        /// <summary>
        /// Determines whether the default text within the text box can be changed.
        /// </summary>
        public bool IsReadOnly;

        /// <summary>
        /// Sets the max number of characters allowed in the text box
        /// </summary>
        public int MaxLength;

        /// <summary>
        /// Determines whether the user selected the text box for typing.
        /// </summary>
        private bool _selected;
        private bool initialTyping;
        private bool cursorVisible;
        private bool cursorBlink;
        private TimeSpan cursorBlinkTime;

        private bool keyShift;
        private TimeSpan keyShiftTimer;
        private bool keyShiftTimerStarted;
        private bool capsLockOn;
        private bool backspaceheld;
        private bool deleteNextChar;
        private TimeSpan deleteTimer;
        private int numDeletedKeys;
        private bool standardKeyTyped;

        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;

        public XAMLiteTextBox(Game game)
            : base(game)
        {
            this.Text = string.Empty;
            this._backgroundColor = Color.White;
            this._foregroundColor = Color.Black;
            this.Padding = new Thickness(0, 0, 0, 0);
            this.SourceName = @"Images/textBox";
            this.cursor = "|";
            this.initialTyping = true;
            this.cursorBlinkTime = TimeSpan.FromSeconds(0.5);

            deleteNextChar = true;

            numDeletedKeys = 0;

            specialInputKeys = new Keys[] {
                Keys.D0,
                Keys.D1,
                Keys.D2,
                Keys.D3,
                Keys.D4,
                Keys.D5,
                Keys.D5,
                Keys.D6,
                Keys.D7,
                Keys.D8,
                Keys.D9,
                Keys.OemQuotes,
                Keys.OemTilde,
                Keys.OemComma,
                Keys.OemPeriod,
                Keys.OemSemicolon,
                Keys.OemBackslash,
                Keys.OemCloseBrackets,
                Keys.OemOpenBrackets,
                Keys.OemPlus,
                Keys.OemMinus,
                Keys.OemQuestion,
                Keys.OemPipe
            };

            standardInputKeys = new Keys[] { 
                Keys.A, 
                Keys.B, 
                Keys.C, 
                Keys.D, 
                Keys.E, 
                Keys.F, 
                Keys.G, 
                Keys.H, 
                Keys.I, 
                Keys.J, 
                Keys.K, 
                Keys.L, 
                Keys.M, 
                Keys.N, 
                Keys.O, 
                Keys.P, 
                Keys.Q, 
                Keys.R, 
                Keys.S, 
                Keys.T, 
                Keys.U, 
                Keys.V, 
                Keys.W, 
                Keys.X, 
                Keys.Y, 
                Keys.Z,
                Keys.Space,
                Keys.Enter,
                Keys.OemClear,
                Keys.Decimal,
                Keys.Tab,
                Keys.Add,
                Keys.Subtract,
                Keys.Multiply,
                Keys.Divide,
                Keys.CapsLock,
                //Keys.Home,
                //Keys.End,
                //Keys.Left,
                //Keys.Right,
                //Keys.Up,
                //Keys.Down,
                //Keys.PageUp,
                //Keys.PageDown,
                //Keys.Insert,
                Keys.NumPad0,
                Keys.NumPad1,
                Keys.NumPad2,
                Keys.NumPad3,
                Keys.NumPad4,
                Keys.NumPad5,
                Keys.NumPad6,
                Keys.NumPad7,
                Keys.NumPad8,
                Keys.NumPad9
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="fontName"></param>
        protected override void LoadContent()
        {
            base.LoadContent();

            this.textBoxTexture = Game.Content.Load<Texture2D>(this.SourceName);
            this.Width = textBoxTexture.Width;
            this.Height = textBoxTexture.Height;
            this._panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            this.textPosition = new Vector2(_panel.X + (int)this.Padding.Left, _panel.Y + (int)this.Padding.Top);
            this.cursorPosition = new Vector2(_panel.X + (int)this.Padding.Left, _panel.Y + (int)this.Padding.Top);
            this.cursorStartPosition = new Vector2(_panel.X + (int)this.Padding.Left, _panel.Y + (int)this.Padding.Top);
        }

        public override void Update(GameTime gameTime)
        {
            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                this.spriteFont.Spacing = Spacing;
            }

            // initial text box click where the default text is replaced with just a cursor.
            if (_mouseDown && _panel.Contains(_msRect) && initialTyping)
            {
                _selected = true;
                initialTyping = false;
                this.Text = string.Empty;
                cursorVisible = true;
                cursorBlink = true;
            }
            // user has previously typed something, deselected, and then selected again. 
            else if (_mouseDown && _panel.Contains(_msRect) && !_selected)
            {
                _selected = true;
                cursorVisible = true;
                cursorBlink = true;
            }
            // text box is deselected.
            else if (_mouseDown && !_panel.Contains(_msRect) && _selected)
            {
                _selected = false;
                cursorVisible = false;
                cursorBlink = false;
            }

            // handling the blinky cursor.
            if (cursorVisible)
            {
                cursorBlinkTime -= gameTime.ElapsedGameTime;
                if (cursorBlinkTime <= TimeSpan.Zero)
                { 
                    cursorBlinkTime = TimeSpan.FromSeconds(0.5);
                    if (cursorBlink)
                        cursorBlink = false;
                    else
                        cursorBlink = true;
                }
            }

            if (backspaceheld)
            {
                deleteTimer -= gameTime.ElapsedGameTime;
                if (deleteTimer <= TimeSpan.Zero)
                {
                    deleteNextChar = true;
                }
            }

            currentKeyboardState = Keyboard.GetState();

            if ((currentKeyboardState.IsKeyDown(Keys.RightShift) ||
                currentKeyboardState.IsKeyDown(Keys.LeftShift)) && !keyShiftTimerStarted)
            {
                keyShiftTimer = TimeSpan.FromSeconds(0.1);
                keyShift = true;
            }

            if (keyShift)
            {
                keyShiftTimer -= gameTime.ElapsedGameTime;
                if (keyShiftTimer <= TimeSpan.Zero)
                {
                    keyShift = false;
                    keyShiftTimerStarted = false;
                }
            }

            if (_selected)
                ProcessKeyboard();
            
            base.Update(gameTime);

            lastKeyboardState = currentKeyboardState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(this.textBoxTexture, this._panel, Color.White);
                this.spriteBatch.DrawString(this.spriteFont, this.Text, textPosition, _foregroundColor);
                if(cursorBlink)
                    this.spriteBatch.DrawString(this.spriteFont, this.cursor, cursorPosition, _foregroundColor);
                this.spriteBatch.End();
            }
        }

        private void ProcessKeyboard()
        {
            foreach (Keys key in standardInputKeys)
            {
                if (CheckKey(key))
                {
                    standardKeyTyped = true;
                    AddKeyToText(key);
                    break;
                }
            }

            foreach (Keys key in specialInputKeys)
            {
                if (CheckKey(key))
                {
                    standardKeyTyped = false;
                    AddKeyToText(key);
                    break;
                }
            }
            cursorPosition.X = cursorStartPosition.X + (int)this.spriteFont.MeasureString(this.Text).X + 2;
        }

        private bool CheckKey(Keys key)
        {
            if ((currentKeyboardState.IsKeyDown(Keys.Back) ||
                currentKeyboardState.IsKeyDown(Keys.Delete)) && deleteNextChar)
            {
                startDelete();
            }

            if (currentKeyboardState.IsKeyUp(Keys.Back) && 
                currentKeyboardState.IsKeyUp(Keys.Delete))
            {
                stopDelete();
            }

            return lastKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        private void AddKeyToText(Keys key)
        {
            string newChar = "";

            if (this.Text.Length >= MaxLength && key != Keys.Back && key != Keys.Delete && 
                key != Keys.Tab && key != Keys.Enter && (int)this.spriteFont.MeasureString(this.Text).X >= 
                textBoxTexture.Width - 20)
                return;
            if (standardKeyTyped)
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
                        if (!capsLockOn)
                        {
                            capsLockOn = true;
                        }
                        else
                        {
                            capsLockOn = false;
                        }
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
                        this.Text = "";
                        return;
                    case Keys.Enter:
                    case Keys.Tab:
                        _selected = false;
                        cursorVisible = false;
                        cursorBlink = false;
                        break;
                    default:
                        if (keyShift || capsLockOn)
                            newChar += key;
                        else
                            newChar += key.ToString().ToLower();
                        break;
                }
            }
            else
            {
                if (keyShift)
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
                        default:
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
                        default:
                            break;
                    }
                }
            }
            this.Text += newChar;
            OnKeyDown();
        }

        private void stopDelete()
        {
            backspaceheld = false;
            deleteNextChar = true;
            numDeletedKeys = 0;
            OnKeyDown();
        }

        private void startDelete()
        {
            deleteNextChar = false;
            backspaceheld = true;
            numDeletedKeys++;
            if (this.Text.Length != 0)
                this.Text = Text.Remove(Text.Length - 1);
            if(numDeletedKeys <= 2)
                deleteTimer = TimeSpan.FromSeconds(0.3);
            else if(numDeletedKeys <= 4)
                deleteTimer = TimeSpan.FromSeconds(0.2);
            else
                deleteTimer = TimeSpan.FromSeconds(0.15);
        }
    }
}
