using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = Microsoft.Xna.Framework.Color;
using FontFamily = System.Windows.Media.FontFamily;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace XAMLite
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class XAMLiteBaseContentControl : XAMLiteBaseControl
    {
        /// <summary>
        /// Font texture.
        /// </summary>
        internal SpriteFont SpriteFont;

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
        /// Position of the content within the existing XAMLite control.
        /// </summary>
        protected virtual Vector2 ContentPosition
        {
            get
            {
                return new Vector2((float)(Position.X + Padding.Left), (float)(Position.Y + Padding.Top));
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// True when the font family has changed.
        /// </summary>
        protected bool FontFamilyChanged;

        /// <summary>
        /// The content color.
        /// </summary>
        protected Color ForegroundColor
        {
            get
            {
                var solidBrush = (SolidColorBrush)Foreground;
                var color = solidBrush.Color;
                return new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// The color of the content, whether text or some other object.
        /// </summary>
        public virtual Brush Foreground { get; set; }

        /// <summary>
        /// Character spacing.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseContentControl(Game game)
            : base(game)
        {
            Foreground = Brushes.Black;
            Padding = new Thickness();
            Spacing = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            LoadArialFonts();
            LoadCourierFonts();
            LoadVerdanaFonts();

            //Default this controls font
            SpriteFont = Courier10SpriteFont;
        }

        /// <summary>
        /// Updates the Button.
        /// </summary>
        /// <param name="gameTime">Reference to the GameTime.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (FontFamilyChanged)
            {
                UpdateFontMetrics();
            }
        }

        /// <summary>
        /// Updates the spacing, font family, and retakes the string 
        /// measurements
        /// </summary>
        internal virtual void UpdateFontMetrics()
        {
            UpdateFontFamily(_fontFamily);
            SpriteFont.Spacing = Spacing;
            FontFamilyChanged = false;
        }

        /// <summary>
        /// Updates the font family.
        /// </summary>
        /// <param name="fontFamily"></param>
        protected void UpdateFontFamily(FontFamily fontFamily)
        {
            var font = fontFamily.ToString();

            if (font.Contains("Arial"))
            {
                UpdateArialFontFamily();
            }
            else if (font.Contains("Courier"))
            {
                UpdateCourierFontFamily();
            }
            else if (font.Contains("Verdana"))
            {
                UpdateVerdanaFontFamily();
            }
            else
            {
                throw new Exception("Font type not supported.");
            }
        }

        /// <summary>
        /// Recalculate the width and height of the control.  If the user 
        /// defined width or height is greater than the measurement of the 
        /// string the user defined settings will be maintained.
        /// </summary>
        protected virtual void RecalculateWidthAndHeight(object content)
        {
            if (content != null)
            {
                var w = (int)SpriteFont.MeasureString(content.ToString()).X + (int)Padding.Left + (int)Padding.Right;
                Width = Width > w ? Width : w;
                var h = (int)SpriteFont.MeasureString(content.ToString()).Y + (int)Padding.Top + (int)Padding.Bottom;
                Height = Height > h ? Height : h;
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
    }
}
