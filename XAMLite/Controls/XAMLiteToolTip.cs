using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// Represents a control that creates a pop-up window that displays 
    /// information for an element in the interface.
    /// </summary>
    public class XAMLiteToolTip : XAMLiteControl
    {
        /// <summary>
        /// Approximate height of the mouse pointer.
        /// </summary>
        private readonly int _pointerHeight;

        /// <summary>
        /// This is a helper class for XAMLiteToolTip and its purpose is to
        /// designate intervals relating to visibility.
        /// </summary>
        public XAMLiteToolTipService ToolTipService;

        /// <summary>
        /// The duration a tool tip will be displayed as specified by the 
        /// ToolTipService.ShowDuration.  
        /// </summary>
        private TimeSpan _visibleTimeSpan;

        /// <summary>
        /// The duration a tool tip must wait to become Visible as specified
        /// by the user as specified by ToolTipService.InitialShowDelay.  
        /// Default is 0 milliseconds.
        /// </summary>
        private TimeSpan _visibleDelayTimeSpan;

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
                if (SpriteFont != null)
                {
                    SpriteFont.Spacing = Spacing;
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
                return Text;
            }

            set
            {
                Text = value;
                if (SpriteFont != null)
                {
                    SpriteFont.Spacing = Spacing;
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
        private bool _fontFamilyChanged;

        /// <summary>
        /// Family the font belongs to.
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
                _fontFamilyChanged = true;
            }
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
        /// PlacementTarget. Not applicable when Placement = Absolute, Mouse, 
        /// MousePoint.
        /// </summary>
        public Rect PlacementRectangle
        {
            get
            {
                return _placementRect;
            }

            set
            {
                _placementRect = value;
                _placementRectangleSet = true;
            }
        }

        /// <summary>
        /// Determines whether the position of the ToolTIp will be affected by
        /// the position of the PlacementRectangle
        /// </summary>
        private bool _placementTargetSet;

        /// <summary>
        /// The private PlacementTarget.
        /// </summary>
        private XAMLiteControl _placementTarget;

        /// <summary>
        /// The control to which the tool tip is assigned. If not assigned,
        /// the target becomes the screen.  Not applicable when Placement = 
        /// Absolute, Mouse, MousePoint.
        /// </summary>
        public XAMLiteControl PlacementTarget
        {
            get
            {
                return _placementTarget;
            }

            set
            {
                _placementTarget = value;
                _placementTargetSet = true;
            }
        }

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
        /// When true, control becomes visible.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// Location of where the text should be drawn.
        /// </summary>
        private Vector2 _textPos;

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

                _transparent = value == Brushes.Transparent;
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
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteToolTip(Game game)
            : base(game)
        {
            _pointerHeight = 20;

            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            _foregroundColor = Color.Black;
            _backgroundColor = Color.Transparent;
            SpriteFont = Courier10SpriteFont;
            Padding = new Thickness(0, 0, 0, 0);
            Spacing = 2;
            Placement = PlacementMode.MousePoint;
            Visible = Visibility.Hidden;
            IsEnabled = false;
            IsOpen = false;

            ToolTipService = new XAMLiteToolTipService();

            _visibleTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.ShowDuration);
            _visibleDelayTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.InitialShowDelay);

            TooltipCount++;
        }

        /// <summary>
        /// Loads the tool tip content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // triggers whether Width and Height of textblock were set by user
            _widthSet = Width != 0;

            _heightSet = Height != 0;

            _drawPosition = new Rectangle();
        }

        /// <summary>
        /// Updates the tool tip.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (TextWrapping == TextWrapping.Wrap && !_textWrappingSet)
            {
                _textWrappingSet = true;
                Text = WordWrap(Text, Width);
            }

            if (!_widthHeightContainerSet)
            {
                _widthHeightContainerSet = true;
                CalculateWidthAndHeight(Name + Text);
            }

            if (_fontFamilyChanged)
            {
                _fontFamilyChanged = false;
                UpdateFontFamily(_fontFamily);
                SpriteFont.Spacing = Spacing;
                CalculateWidthAndHeight(Text);
            }

            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }

            if (_textChanged)
            {
                _textChanged = false;
                if (TextWrapping == TextWrapping.Wrap)
                {
                    Text = WordWrap(Text, Width);
                    CalculateWidthAndHeight(Name + Text);
                    CalculateDrawPosition();
                }
            }

            // This is providing enough time for the dynamic tool tip text
            // to calculate its size prior to rendering.  Without, the text will
            // flash outside of its tool tip container and then the container
            // resizes.
            if (IsOpen)
            {
                if (_visibleDelayTimeSpan > TimeSpan.Zero)
                {
                    _visibleDelayTimeSpan -= gameTime.ElapsedGameTime;
                }
                else
                {
                    _visibleDelayTimeSpan = TimeSpan.Zero;
                    Visible = Visibility.Visible;
                }
            }

            // When the placement is not mouse-related, it runs a timer
            // that determines when it should be hidden again.
            if (Visible == Visibility.Visible && Placement != PlacementMode.Mouse && Placement != PlacementMode.MousePoint)
            {
                _visibleTimeSpan -= gameTime.ElapsedGameTime;
                if (_visibleTimeSpan <= TimeSpan.Zero)
                {
                    IsOpen = false;
                    Visible = Visibility.Hidden;
                    _visibleTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.ShowDuration);
                    _visibleDelayTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.InitialShowDelay);
                }
            }

            if (!IsOpen)
            {
                Visible = Visibility.Hidden;
                _placementRectangleSet = false;
                _placementTargetSet = false;
                _visibleDelayTimeSpan = TimeSpan.FromMilliseconds(ToolTipService.InitialShowDelay);
            }
        }

        /// <summary>
        /// Draws the tool tip.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible && IsOpen)
            {
                SpriteBatch.Begin();

                // Make sure our height has been calculated before we try to draw anything.
                // Otherwise, that means the height and positioning haven't been fully calculated,  
                // and the text will be displayed at an incorrect position for one frame.
                if (_drawPosition.Height > 0)
                {
                    SpriteFont.Spacing = Spacing;

                    if (!_transparent)
                    {
                        SpriteBatch.Draw(Pixel, _drawPosition, _backgroundColor * (float)Opacity);
                    }

                    if (!string.IsNullOrEmpty(Name))
                    {
                        SpriteBatch.DrawString(SpriteFont, Name, _textPos, Color.Yellow);
                    }

                    SpriteBatch.DrawString(SpriteFont, Text, _textPos, _foregroundColor);
                }

                SpriteBatch.End();
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

            if (Placement != PlacementMode.Absolute && Placement != PlacementMode.Mouse && Placement != PlacementMode.MousePoint)
            {
                // if no target is set, the screen becomes the control.
                if (!_placementTargetSet)
                {
                    PlacementTarget = new XAMLiteLabel(Game) { Width = Viewport.Width, Height = Viewport.Height };
                }

                // Add the additional positional info for the PlacementTarget.
                _drawPosition.X += (int)PlacementTarget.Position.X;
                _drawPosition.Y += (int)PlacementTarget.Position.Y;
            }

            // Make adjustments to the draw position based on the Placement.
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
                        _drawPosition.X += (int)PlacementRectangle.X + ((int)PlacementRectangle.Width / 2);
                        _drawPosition.Y += (int)PlacementRectangle.Y + ((int)PlacementRectangle.Height / 2);
                    }
                    else
                    {
                        _drawPosition.X += PlacementTarget.Width / 2;
                        _drawPosition.Y += PlacementTarget.Height / 2;
                    }

                    _drawPosition.X -= Panel.Width / 2;
                    _drawPosition.Y -= Panel.Height / 2;
                    break;
                // Top right of tool tip should touch top left of target.
                case PlacementMode.Left:
                    _drawPosition.X -= Panel.Width;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X;
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }

                    break;
                // Top left of tool tip should touch top right of target.
                case PlacementMode.Right:
                    _drawPosition.X += PlacementTarget.Width;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += -(PlacementTarget.Width - ((int)PlacementRectangle.X + (int)PlacementRectangle.Width));
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }

                    break;
                // Bottom left of tool tip should touch the top left of target.
                case PlacementMode.Top:
                    _drawPosition.Y -= Panel.Height;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X;
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }

                    break;
                // Top left of tool tip should touch the bottom left of target.
                case PlacementMode.Bottom:
                    _drawPosition.Y += PlacementTarget.Height;
                    if (_placementRectangleSet)
                    {
                        _drawPosition.X += (int)PlacementRectangle.X;
                        _drawPosition.Y += (int)PlacementRectangle.Y;
                    }

                    break;
                // Top left of tool tip should touch the bottom left of the mouse pointer.
                case PlacementMode.Mouse:
                    _drawPosition.X += MsRect.X;
                    _drawPosition.Y += MsRect.Y + _pointerHeight;
                    break;
                // Top left of tool tip should touch the tip of the mouse pointer.
                case PlacementMode.MousePoint:
                    _drawPosition.X += MsRect.X;
                    _drawPosition.Y += MsRect.Y;
                    break;
            }

            // check whether a screen edge is encountered.
            if (ScreenEdgeDetected())
            {
                // in which case, adjust where the panel is drawn.
                AdjustForScreenEdge();
            }

            // set the width
            _drawPosition.Width = Panel.Width;
            _drawPosition.Height = Panel.Height;

            // set where the text will be drawn within the tool tip.
            _textPos = new Vector2(_drawPosition.X + (int)Padding.Left, _drawPosition.Y + (int)Padding.Top);

            Panel.X = _drawPosition.X;
            Panel.Y = _drawPosition.Y;
        }

        /// <summary>
        /// Returns true if a screen edge is detected where the tool tip is to
        /// be drawn.
        /// </summary>
        /// <returns></returns>
        private bool ScreenEdgeDetected()
        {
            if (_drawPosition.X < 0 || (_drawPosition.X + Panel.Width) > Viewport.Width || _drawPosition.Y < 0 || (_drawPosition.Y + Panel.Height) > Viewport.Height)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Adjusts the X or Y position of the tool tip when a screen edge is
        /// detected.
        /// </summary>
        private void AdjustForScreenEdge()
        {
            switch (Placement)
            {
                case PlacementMode.Absolute:
                    break;
                case PlacementMode.MousePoint:
                case PlacementMode.Mouse:
                    if (_drawPosition.X < 0)
                    {
                        _drawPosition.X = 0;
                    }
                    else if ((_drawPosition.X + Panel.Width) > Viewport.Width)
                    {
                        _drawPosition.X = Viewport.Width - Panel.Width;
                    }

                    if (_drawPosition.Y < 0)
                    {
                        _drawPosition.Y = 0;
                    }
                    else if ((_drawPosition.Y + Panel.Height) > Viewport.Height)
                    {
                        _drawPosition.Y = Viewport.Height - Panel.Height;
                    }

                    break;
            }
        }

        /// <summary>
        /// Set width and height if not already set or adjusting for noWrap
        /// </summary>
        /// <param name="text"></param>
        private void CalculateWidthAndHeight(string text)
        {
            if (!_widthSet || TextWrapping == TextWrapping.NoWrap)
            {
                Width = (int)SpriteFont.MeasureString(text).X + (int)Padding.Left + (int)Padding.Right;
            }

            if (!_heightSet)
            {
                Height = (int)SpriteFont.MeasureString(text).Y + (int)Padding.Top + (int)Padding.Bottom;
            }

            MarginChanged = true;
        }

        // used to break the string into seperate lines of text
        protected const string Newline = "\n";

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in pixels, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        /// 
        private string WordWrap(string text, int width)
        {
            width -= (int)Padding.Left + (int)Padding.Right;
            // return if string length is less than width of textblock
            if (width > (int)SpriteFont.MeasureString(text).X)
            {
                return text;
            }

            // just for clarity
            string tempString = Regex.Replace(text, "\n", "");

            float strLenPixels = (int)SpriteFont.MeasureString(tempString).X;
            int numCharsinString = 0;

            // determining total number of characters in the string 
            for (var i = 0; i < tempString.Length; i++)
            {
                numCharsinString++;
            }

            // Now removing any whitespaces that might be at the end of the original string
            while ((numCharsinString - 1) >= 0 && char.IsWhiteSpace(text[numCharsinString - 1]))
            {
                numCharsinString--;
            }

            // finding number of pixels per character in string length
            float pxPerChar = strLenPixels / numCharsinString;

            // determining max number of characters per line to fit textblock
            int charsPerLine;

            int paddingAdjust = (int)Padding.Left + (int)Padding.Right;
            if (paddingAdjust < width)
            {
                charsPerLine = (int)(width / pxPerChar);
            }
            else
            {
                charsPerLine = 1;
            }

            _sb = new StringBuilder();
            int pos, next;
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Newline, pos, StringComparison.Ordinal);
                if (eol == -1)
                {
                    next = eol = text.Length;
                }
                else
                {
                    next = eol + Newline.Length;
                }

                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;
                        if (len > charsPerLine)
                        {
                            len = BreakLine(text, pos, charsPerLine);
                        }

                        _sb.Append(text, pos, len);

                        // update position
                        pos += len;

                        // "if" statement prevents extra line being added at end of text for drawing the background block
                        if (pos != text.Length)
                        {
                            _sb.Append(Newline);
                        }

                        // Trim whitespace following break
                        while (pos < eol && char.IsWhiteSpace(text[pos]))
                        {
                            pos++;
                        }
                    }
                    while (eol > pos);
                }
                else
                {
                    _sb.Append(Newline); // Empty line
                }
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
        private int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max - 1;
            while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            if (i < 0)
            {
                return max; // No whitespace found; break at maximum length
            }
            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }
            // Return length of text before whitespace
            return i + 1;
        }
    }
}
