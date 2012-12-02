using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteBaseContent : XAMLiteBaseContentControl
    {
        /// <summary>
        /// Object contained in the control, which might include
        /// string, date/time, etc.
        /// </summary>
        public virtual object Content { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseContent(Game game)
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

            RecalculateWidthAndHeight(Content);
        }
    }
}
