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
            if (_parent is XAMLiteMenuNew && item is XAMLiteMenuItemNew)
            {
                Console.WriteLine("Pare: " + _parent);
                var i = item as XAMLiteMenuItemNew;
                i.ItemIndex = Count;
                base.Add(item);
            }
            else if (_parent is XAMLiteComboBox && item is XAMLiteComboBoxItem)
            {
                var i = item as XAMLiteComboBoxItem;
                i.ItemIndex = Count;
                base.Add(item);
            }
            else if (_parent is XAMLiteListBox && item is XAMLiteListBoxItem)
            {
                var i = item as XAMLiteListBoxItem;
                i.ItemIndex = Count;
                base.Add(item);
            }
            else
            {
                throw new Exception("Item is of the wrong type.");
            }
        }
    }
}
