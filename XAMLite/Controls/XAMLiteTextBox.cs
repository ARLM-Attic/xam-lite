using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;
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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
    }
}
