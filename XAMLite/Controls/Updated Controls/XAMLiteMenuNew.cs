using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteMenuNew : XAMLiteBaseControl
    {
        /// <summary>
        /// List of menu items, if any.  Each item that is added to the menu 
        /// class, is positioned horizontally across, starting from the left
        /// and makes up the different selectable menus for the menu bar.
        /// </summary>
        public Items Items;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuNew(Game game)
            : base(game)
        {
        }
    }
}
