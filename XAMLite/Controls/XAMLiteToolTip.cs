﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using System.Collections.Generic;
using System.Text;

namespace XAMLite
{
    /// <summary>
    /// Represents a control that creates a pop-up window that displays 
    /// information for an element in the interface.
    /// </summary>
    public class XAMLiteToolTip : XAMLiteControl
    {
        /// <summary>
        /// This is a helper class for XAMLiteToolTip and its purpose is to
        /// designate intervals relating to visibility.
        /// </summary>
        public XAMLiteToolTipService ToolTipService;

        /// <summary>
        /// This just duplicates the Text property but is here since XAML developer will expect to be able
        /// to set the Content property of a label. Note: This Content property shouldn't be confused with 
        /// XNA's concept of Content (i.e. textures and models, etc).
        /// </summary>
        public string Content
        {
            get
            {
                return this.Text;
            }

            set
            {
                this.Text = value;
                if (this.spriteFont != null)
                {
                    this.spriteFont.Spacing = Spacing;
                    RecalculateWidthAndHeight(value);
                }
                base.Text = value;
            }
        }

        /// <summary>
        /// Family the font belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// True when the font family has been changed.
        /// </summary>
        private bool fontFamilyChanged;

        /// <summary>
        /// Family the font belongs to.
        /// </summary>
        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; fontFamilyChanged = true; }
        }

        /// <summary>
        /// character spacing
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the control has a drop 
        /// shadow.
        /// </summary>
        public bool HasDropShadow { get; set; }

        /// <summary>
        /// Describes the position of the tool tip releative to the control.
        /// </summary>
        public PlacementMode Placement { get; set; }

        /// <summary>
        /// Gets or sets the rectangular area relative to which the ToolTip 
        /// control is positioned when it opens.
        /// </summary>
        public Rect PlacementRectangle { get; set; }

        /// <summary>
        /// Get or sets the horizontal distance between the target origin and 
        /// the popup alignment point.
        /// </summary>
        public double HorizontalOffset { get; set; }

        /// <summary>
        /// Get or sets the vertical distance between the target origin and the 
        /// popup alignment point.
        /// </summary>
        public double VerticalOffset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Vector2 paddedPosition;

        /// <summary>
        /// Font color.
        /// </summary>
        private Color _foregroundColor;

        /// <summary>
        /// Font color.
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
        /// Background color of the tool tip
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// Background color of the tool tip
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                if ((SolidColorBrush)value == Brushes.Transparent)
                    _transparent = true;
                else
                    _transparent = false;
            }
        }

        private bool _transparent;

        private StringBuilder _sb;

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        public TextAlignment TextAlignment { get; set; }

        private bool _widthSet;
        private bool _heightSet;

        private bool _textWrappingSet;
        private bool _widthHeightContainerSet;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteToolTip(Game game)
            : base(game)
        {
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            spriteFont = courier10SpriteFont;
            Padding = new Thickness(0, 0, 0, 0);
            Spacing = 2;
        }

        /// <summary>
        /// Loads the tool tip content.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="fontName"></param>
        protected override void LoadContent()
        {
            base.LoadContent();

            // triggers whether Width and Height of textblock were set by user
            if (this.Width == 0)
                _widthSet = false;
            else
                _widthSet = true;
            if (this.Height == 0)
                _heightSet = false;
            else
                _heightSet = true;
        }

        /// <summary>
        /// Updates the tool tip.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fontFamilyChanged)
            {
                fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                this.spriteFont.Spacing = Spacing;
                RecalculateWidthAndHeight(this.Text);
            }
            if (marginChanged)
            {
                marginChanged = false;
                panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }
        }

        /// <summary>
        /// Draws the tool tip.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                if (TextWrapping == TextWrapping.Wrap && !_textWrappingSet)
                {
                    _textWrappingSet = true;
                    this.Text = WordWrap(this.Text, (int)this.spriteFont.MeasureString(this.Text).X);
                }

                if (!_widthHeightContainerSet)
                {
                    _widthHeightContainerSet = true;
                    CalculateWidthAndHeight(this.Text);
                }

                paddedPosition = new Vector2(panel.X + (int)Padding.Left, panel.Y + (int)Padding.Top);

                spriteBatch.Begin();

                this.spriteFont.Spacing = this.Spacing;
                if (!_transparent)
                {
                    spriteBatch.Draw(pixel, panel, (this._backgroundColor * (float)Opacity));
                }

                spriteBatch.DrawString(this.spriteFont, this.Text, paddedPosition, (this._foregroundColor * (float)Opacity));
                
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Set width and height if not already set or adjusting for noWrap
        /// </summary>
        /// <param name="text"></param>
        private void CalculateWidthAndHeight(string text)
        {
            if (!_widthSet || TextWrapping == TextWrapping.NoWrap)
                this.Width = (int)this.spriteFont.MeasureString(text).X + (int)Padding.Left + (int)Padding.Right;

            if (!_heightSet || TextWrapping == TextWrapping.NoWrap)
                this.Height = (int)this.spriteFont.MeasureString(text).Y + (int)Padding.Top + (int)Padding.Bottom;
            marginChanged = true;
        }

        // used to break the string into seperate lines of text
        protected const string _newline = "\r\n";

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in pixels, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        /// 
        public string WordWrap(string text, int width)
        {
            // return if string length is less than width of textblock
            if (this.Width > width)
                return text;

            // just for clarity
            float strLenPixels = width;
            int numCharsinString = 0;

            // determining total number of characters in the string 
            for (int i = 0; i < text.Length; i++)
                numCharsinString++;

            // Now removing any whitespaces that might be at the end of the string
            while ((numCharsinString - 1) >= 0 && Char.IsWhiteSpace(text[numCharsinString - 1]))
                numCharsinString--;

            // finding number of pixels per character in string length
            float pxPerChar = strLenPixels / numCharsinString;

            // determining max number of characters per line to fit textblock
            int charsPerLine;

            int paddingAdjust = (int)Padding.Left + (int)Padding.Right;
            if (paddingAdjust < this.Width)
                charsPerLine = this.Width / (int)pxPerChar;
            else
                charsPerLine = 1;

            Console.WriteLine(text);
            _sb = new StringBuilder();
            int pos, next;
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(_newline, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + _newline.Length;

                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > charsPerLine)
                            len = BreakLine(text, pos, charsPerLine);
                        _sb.Append(text, pos, len);

                        // update position
                        pos += len;

                        // "if" statement prevents extra line being added at end of text for drawing the background block
                        if (pos != text.Length)
                            _sb.Append(_newline);

                        // Trim whitespace following break
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;

                    } while (eol > pos);
                }
                else _sb.Append(_newline); // Empty line
            }

            return _sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;
            if (i < 0)
                return max; // No whitespace found; break at maximum length
            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;
            // Return length of text before whitespace
            return i + 1;
        }
    }
}
