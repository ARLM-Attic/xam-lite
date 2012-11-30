using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

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
        public Brush BorderBrush { get; set; }

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
        /// Updates the FontFamily, Spacing, and recalculates the new
        /// Width and Height.
        /// </summary>
        protected override void UpdateFontMetrics()
        {
            base.UpdateFontMetrics();

            RecalculateWidthAndHeight(Text);
        }
    }
}
