using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// The orientation that a scroll wheel is to be displayed.
    /// </summary>
    public enum Orientation
    {
        Horizontal,

        Vertical
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteScrollBar : XAMLiteGridNew
    {
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteScrollBar(Game game)
            : base(game)
        {
            Orientation = Orientation.Vertical;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            var upArrowButton = new XAMLiteImageWithRolloverNew(Game)
                { SourceName = "", RolloverSourceName = "", RenderTransform = RenderTransform.Normal };
            Children.Add(upArrowButton);
        }
    }
}
