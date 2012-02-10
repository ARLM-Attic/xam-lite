using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XAMLite
{
    /// <summary>
    /// This is a helper class for XAMLiteToolTip and its purpose is to
    /// designate intervals relating to visibility.
    /// </summary>
    public class XAMLiteToolTipService
    {
        /// <summary>
        /// Gets or sets the length of time before a tooltip opens.
        /// </summary>
        public int InitialShowDelay;

        /// <summary>
        /// Gets or sets the amount of time that a tooltip remains visible.
        /// </summary>
        public int ShowDuration;

        /// <summary>
        /// Gets or sets the maximum time between the display of two tooltips 
        /// where the second tooltip appears without a delay.
        /// </summary>
        public int BetweenShowDelay;
    }
}
