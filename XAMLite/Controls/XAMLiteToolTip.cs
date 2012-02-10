using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using System.Collections.Generic;

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
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteToolTip(Game game)
            : base(game)
        {

        }
    }
}
