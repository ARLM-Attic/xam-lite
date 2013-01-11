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
    public class XAMLiteMenuItemNew : XAMLiteBaseControl
    {
        /// <summary>
        /// List of menu items, if any. Each item that is added to another menu 
        /// item, is positioned in one of two ways.  If its parent is one of the
        /// menu items who's parent is the menu, it is drawn immediately below
        /// the menu header.  If the parent of its parent is another menu item,
        /// the the item is drawn to the right side of its parent.
        /// </summary>
        public Items Items;

        /// <summary>
        /// The text that makes up the label of the control.
        /// </summary>
        public string Header { get; set; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuItemNew(Game game)
            : base(game)
        {
        }
    }
}
