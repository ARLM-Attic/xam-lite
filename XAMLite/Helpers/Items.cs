using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Items : List<XAMLiteListBoxItem>
    {
        /// <summary>
        /// Parent of the items.
        /// </summary>
        private XAMLiteListBox _parent;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Items(XAMLiteListBox parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Adds the item to the list of items and sets its parent.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(XAMLiteListBoxItem item)
        {
            if (item.Parent == null)
            {
                item.Index = Count - 1;
                item.Parent = _parent;
            }

            base.Add(item);
        }
    }
}
