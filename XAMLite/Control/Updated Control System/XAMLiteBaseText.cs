using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteBaseText : XAMLiteBaseContentControl
    {
        /// <summary>
        /// Text contained in the control.
        /// </summary>
        public virtual string Text { get; set; }

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
        /// Determines whether the default text within the text box can be changed.
        /// </summary>
        public bool IsReadOnly;

        /// <summary>
        /// Sets the max number of characters allowed in the text box
        /// </summary>
        public int MaxLength;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseText(Game game) 
            : base(game)
        {
        }

        /// <summary>
        /// Updates the font metrics and recalculates the width of the
        /// text to ensure it fits within the XAMLiteControl.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Update font metrics here to get an accurate string measurement.
            UpdateFontMetrics();

            var stringWidth = SpriteFont.MeasureString(Text).X + Padding.Left;
            if (stringWidth > Width)
            {
                Width = (int)Math.Round(stringWidth);
            }
        }

        /// <summary>
        /// Updates the FontFamily, Spacing, and recalculates the new
        /// Width and Height.
        /// </summary>
        internal override void UpdateFontMetrics()
        {
            base.UpdateFontMetrics();

            RecalculateWidthAndHeight(Text);
        }
    }
}
