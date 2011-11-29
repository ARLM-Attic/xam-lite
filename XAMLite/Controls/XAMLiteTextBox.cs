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
                Keys.Back,
                Keys.Delete,
                Keys.Tab
                
                //Keys.Home,
                //Keys.End,
                //Keys.Add,
                //Keys.Subtract,
                //Keys.Multiply,
                //Keys.Divide,
                //Keys.Left,
                //Keys.Right,
                //Keys.Up,
                //Keys.Down,                
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
            //base.Update(gameTime);

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

            currentKeyboardState = Keyboard.GetState();

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
            return lastKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        private void AddKeyToText(Keys key)
        {
            string newChar = "";
            keyShift = false;
            if (currentKeyboardState.IsKeyDown(Keys.RightShift) ||
                currentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                keyShift = true;
            }
            if (this.Text.Length >= MaxLength && key != Keys.Back && (int)this.spriteFont.MeasureString(this.Text).X >= textBoxTexture.Width - 20)
                return;
            if (standardKeyTyped)
            {
                switch (key)
                {
                    case Keys.Space:
                        newChar += " ";
                        break;
                    case Keys.Enter:
                    case Keys.Tab:
                        _selected = false;
                        cursorVisible = false;
                        cursorBlink = false;
                        break;
                    case Keys.Delete:
                    case Keys.Back:
                        if (this.Text.Length != 0)
                            this.Text = Text.Remove(Text.Length - 1);
                        return;
                    default:
                        if (keyShift)
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
                        default:
                            break;
                    }
                }
            }
            this.Text += newChar;
        }
    }
}
