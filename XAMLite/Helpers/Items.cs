using System;
using System.Collections.Generic;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Items : List<XAMLiteBaseControl>
    {
        /// <summary>
        /// Parent of the items.
        /// </summary>
        private XAMLiteBaseControl _parent;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Items(XAMLiteBaseControl parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Adds the item to the list of items and sets its parent.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(XAMLiteBaseControl item)
        {
            if (item.Parent == null)
            {
                //item.Index = Count;
                //item.Parent = _parent;
            }

            if (_parent is XAMLiteComboBox && item is XAMLiteComboBoxItem)
            {
                base.Add(item);
            }
            else if (_parent is XAMLiteListBox && item is XAMLiteListBoxItem)
            {
                base.Add(item);
            }
            else
            {
                throw new Exception("Item is of the wrong type.");
            }
        }
    }
}
