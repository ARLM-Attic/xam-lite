using Microsoft.Xna.Framework;

namespace XAMLite
{  
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
        internal override void UpdateFontMetrics()
        {
            base.UpdateFontMetrics();

            RecalculateWidthAndHeight(Content);
        }
    }
}
