using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        /// When placement mode is not either MousePoint or Mouse, the tool tip
        /// will be drawn according to either the default or user-specified
        /// ToolTipService specifications.
        /// </summary>
        private TimeSpan _visibleTimeSpan;

        /// <summary>
        /// 
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
                    //CalculateWidthAndHeight(value);
                    _textChanged = true;
                }
                base.Text = value;
            }
        }

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
                    //CalculateWidthAndHeight(value);
                    _textChanged = true;
                }
                base.Text = value;
            }
        }

        /// <summary>
        /// True if some change occurred in the text and the tool tip needs to 
        /// be remeasured.
        /// </summary>
        private bool _textChanged;

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
        /// May be used to establish what the tool tip will use to position itself with.
        /// </summary>
        //public DependencyObject Parent { get; }

        /// <summary>
        /// Describes the position of the tool tip releative to the control.
        /// </summary>
        public PlacementMode Placement { get; set; }

        /// <summary>
        /// Determines whether the position of the ToolTIp will be affected by
        /// the position of the PlacementRectangle
        /// </summary>
        private bool _placementRectangleSet;

        /// <summary>
        /// The private PlacementRectangle.
        /// </summary>
        private Rect _placementRect;

        /// <summary>
        /// Gets or sets the rectangular area relative to which the ToolTip 
        /// control is positioned when it opens.  Works in assocation with the
        /// PlacementTarget.
        /// </summary>
        public Rect PlacementRectangle { get { return _placementRect; } set { _placementRect = value; _placementRectangleSet = true; } }

        /// <summary>
        /// Determines whether the position of the ToolTIp will be affected by
        /// the position of the PlacementRectangle
        /// </summary>
        private bool _placementTargetSet;

        /// <summary>
        /// The private PlacementRectangle.
        /// </summary>
        private XAMLiteControl _placementTarget;

        /// <summary>
        /// The control to which the tool tip is assigned.
        /// </summary>
        public XAMLiteControl PlacementTarget { get { return _placementTarget; } set { _placementTarget = value; _placementTargetSet = true; } }

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
        /// True when visible.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// Location of where the text should be drawn.
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

        /// <summary>
        /// The background is set to Transparent
        /// </summary>
        private bool _transparent;

        /// <summary>
        /// The organized text or content after it has gone through being text
        /// wrapped.
        /// </summary>
        private StringBuilder _sb;

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// True when the width of the control was set.
        /// </summary>
        private bool _widthSet;

        /// <summary>
        /// True when the height of the control was set.
        /// </summary>
        private bool _heightSet;

        /// <summary>
        /// True when the text has been wrapped.
        /// </summary>
        private bool _textWrappingSet;

        /// <summary>
        /// True when the width and height of the control have been set.
        /// </summary>
        private bool _widthHeightContainerSet;

        /// <summary>
        /// The position where the tool tip will be drawn.
        /// </summary>
        private Rectangle _drawPosition;

        /// <summary>
        /// Total number of live tooltips instances.
        /// </summary>
        public static int TooltipCount;

        /// <summary>
        /// Approximate width in pixels of the mouse pointer.
        /// </summary>
        //private int _pointerWidth;

        /// <summary>
        /// Approximate height of the mouse pointer.
        /// </summary>
        private int _pointerHeight;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteToolTip(Game game)
            : base(game)
        {
            _pointerHeight = 20;
            //_pointerWidth = 12;

            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            spriteFont = courier10SpriteFont;
            Padding = new Thickness(0, 0, 0, 0);
            Spacing = 2;
            HorizontalOffset = 17;
            VerticalOffset = -Height - 25;
            Placement = PlacementMode.MousePoint;
            IsEnabled = false;
            IsOpen = false;

            ToolTipService = new XAMLiteToolTipService();

            _visibleTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.ShowDuration);
            
            TooltipCount++;

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

            _drawPosition = new Rectangle();
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
                spriteFont.Spacing = Spacing;
                CalculateWidthAndHeight(this.Text);
            }
            if (marginChanged)
            {
                marginChanged = false;
                panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }

            if (_textChanged)
            {
                _textChanged = false;
                if (TextWrapping == TextWrapping.Wrap)
                {
                    _textWrappingSet = false;
                    _widthHeightContainerSet = false;
                    CalculateDrawPosition();
                }
            }

            if ( IsOpen && Placement != PlacementMode.Mouse && Placement != PlacementMode.MousePoint)
            {
                Visible = Visibility.Visible;
                _visibleTimeSpan -= gameTime.ElapsedGameTime;
                if (_visibleTimeSpan <= TimeSpan.Zero)
                {
                    IsOpen = false;
                    Visible = Visibility.Hidden;
                    _visibleTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.ShowDuration);
                }
            }

            if (!IsOpen)
            {
                _placementRectangleSet = false;
                _placementTargetSet = false;
            }
        }

        /// <summary>
        /// Draws the tool tip.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (IsOpen && IsEnabled)
            {
                if (TextWrapping == TextWrapping.Wrap && !_textWrappingSet)
                {
                    _textWrappingSet = true;
                    this.Text = WordWrap(this.Text, Width);
                }

                if (!_widthHeightContainerSet)
                {
                    _widthHeightContainerSet = true;
                    CalculateWidthAndHeight(this.Name + this.Text);
                }

                spriteBatch.Begin();

                // Make sure our height has been calculated before we try to draw anything.
                // Otherwise, that means the height and positioning haven't been fully calculated,  
                // and the text will be displayed at an incorrect position for one frame.
                if (_drawPosition.Height > 0)
                {
                    this.spriteFont.Spacing = this.Spacing;

                    if (!_transparent)
                    {
                        spriteBatch.Draw(pixel, _drawPosition, (this._backgroundColor * (float)Opacity));
                    }

                    if (Name != null && Name != string.Empty)
                    {
                        spriteBatch.DrawString(this.spriteFont, this.Name, paddedPosition, Color.Yellow);
                    }

                    spriteBatch.DrawString(this.spriteFont, this.Text, paddedPosition, this._foregroundColor);
                }

                spriteBatch.End();
            }
        }

        /// <summary>
        /// Determines where the panel and text should be drawn.
        /// </summary>
        private void CalculateDrawPosition()
        {
            // start by simply adding in the Horizontal and Vertical offsets.
            _drawPosition.X = (int)HorizontalOffset;
            _drawPosition.Y = (int)VerticalOffset;

            if (Placement != PlacementMode.Absolute || Placement != PlacementMode.Mouse || Placement != PlacementMode.MousePoint)
            {
                if (!_placementTargetSet)
                {
                    PlacementTarget = new XAMLiteLabel(this.Game);
                    PlacementTarget.Width = viewport.Width;
                    PlacementTarget.Height = viewport.Height;
                }

                // Add the additional positional info for the PlacementTarget.
                _drawPosition.X += (int)PlacementTarget.Position.X;
                _drawPosition.Y += (int)PlacementTarget.Position.Y;
            }

            switch (Placement)
            {
                // Absolute position of the tool tip as specified by the
                // HorizontalOffset and the VerticalOffset was already 
                // established above.
                case PlacementMode.Absolute:
                    break;
                // The center of the tool tip should touch the center of the target.
                case PlacementMode.Center:                
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X + (int)PlacementRectangle.Width / 2;
                        _drawPosition.Y += (int)PlacementRectangle.Y + (int)PlacementRectangle.Height / 2;
                    }
                    else
                    {
                        _drawPosition.X += (int)PlacementTarget.Width / 2;
                        _drawPosition.Y += (int)PlacementTarget.Height / 2;
                    }
                    _drawPosition.X -= panel.Width / 2;
                    _drawPosition.Y -= panel.Height / 2;
                    break;
                // Top right of tool tip should touch top left of target.
                case PlacementMode.Left:
                    _drawPosition.X -= panel.Width;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X;
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }
                    break;
                // Top left of tool tip should touch top right of target.
                case PlacementMode.Right:
                    _drawPosition.X += (int)PlacementTarget.Width;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += -((int)PlacementTarget.Width - ((int)PlacementRectangle.X + (int)PlacementRectangle.Width));
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }
                    break;
                // Bottom left of tool tip should touch the top left of target.
                case PlacementMode.Top:
                    _drawPosition.Y -= panel.Height;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X;
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }
                    break;
                // Top left of tool tip should touch the bottom left of target.
                case PlacementMode.Bottom:
                    _drawPosition.Y += (int)PlacementTarget.Height;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X;
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }
                    break;
                // Top left of tool tip should touch the bottom left of the mouse pointer.
                case PlacementMode.Mouse:
                    _drawPosition.X += msRect.X;
                    _drawPosition.Y += msRect.Y + _pointerHeight;

                    // adjust for screen edges if encountered
                    if (_drawPosition.X < 0)
                    {
                        _drawPosition.X = 0;
                    }
                    else if ((_drawPosition.X + panel.Width) > viewport.Width)
                    {
                        _drawPosition.X = viewport.Width - panel.Width;
                    }
                    if (_drawPosition.Y < 0)
                    {
                        _drawPosition.Y = 0;
                    }
                    else if ((_drawPosition.Y + panel.Height) > viewport.Height)
                    {
                        _drawPosition.Y = viewport.Height - panel.Height;
                    }
                    break;
                // Top left of tool tip should touch the tip of the mouse pointer.
                case PlacementMode.MousePoint:
                    _drawPosition.X += msRect.X;
                    _drawPosition.Y += msRect.Y;

                    // adjust for screen edges if encountered
                    if (_drawPosition.X < 0)
                    {
                        _drawPosition.X = 0;
                    }
                    else if ((_drawPosition.X + panel.Width) > viewport.Width)
                    {
                        _drawPosition.X = viewport.Width - panel.Width;
                    }
                    if (_drawPosition.Y < 0)
                    {
                        _drawPosition.Y = 0;
                    }
                    else if ((_drawPosition.Y + panel.Height) > viewport.Height)
                    {
                        _drawPosition.Y = viewport.Height - panel.Height;
                    }
                    break;
                default:
                    break;
            }

            _drawPosition.Width = panel.Width;
            _drawPosition.Height = panel.Height;

            paddedPosition = new Vector2(_drawPosition.X + (int)Padding.Left, _drawPosition.Y + (int)Padding.Top);
        }

        /// <summary>
        /// Set width and height if not already set or adjusting for noWrap
        /// </summary>
        /// <param name="text"></param>
        private void CalculateWidthAndHeight(string text)
        {
            if (!_widthSet || TextWrapping == TextWrapping.NoWrap)
                this.Width = (int)this.spriteFont.MeasureString(text).X + (int)Padding.Left + (int)Padding.Right;

            if (!_heightSet)
                this.Height = (int)this.spriteFont.MeasureString(text).Y + (int)Padding.Top + (int)Padding.Bottom;
            marginChanged = true;
        }

        // used to break the string into seperate lines of text
        protected const string _newline = "\n";

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
            width -= (int)Padding.Left + (int)Padding.Right;
            // return if string length is less than width of textblock
            if (width > (int)this.spriteFont.MeasureString(text).X)
            {
                return text;
            }

            // just for clarity
            string tempString = Regex.Replace(text, "\n", "");

            float strLenPixels = (int)this.spriteFont.MeasureString(tempString).X;
            int numCharsinString = 0;

            // determining total number of characters in the string 
            for (int i = 0; i < tempString.Length; i++)
                numCharsinString++;

            // Now removing any whitespaces that might be at the end of the original string
            while ((numCharsinString - 1) >= 0 && Char.IsWhiteSpace(text[numCharsinString - 1]))
                numCharsinString--;

            // finding number of pixels per character in string length
            float pxPerChar = strLenPixels / numCharsinString;

            // determining max number of characters per line to fit textblock
            int charsPerLine;

            int paddingAdjust = (int)Padding.Left + (int)Padding.Right;
            if (paddingAdjust < this.Width)
            {
                charsPerLine = (int)((this.Width - ((int)Padding.Left + (int)Padding.Right)) / pxPerChar);
                /*if (((this.Width - ((int)Padding.Left + (int)Padding.Right)) / pxPerChar) - charsPerLine >= 0.35)
                {
                    charsPerLine -= 1;
                }*/
            }
            else
                charsPerLine = 1;

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
