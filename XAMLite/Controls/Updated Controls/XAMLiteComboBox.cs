using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteComboBox : XAMLiteListBox
    {
        /// <summary>
        /// 
        /// </summary>
        //public bool IsEditable { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteComboBox(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Calls a method to modify the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        protected override void UpdateItems()
        {
            base.UpdateItems();

            UpdateHeight();
        }

        /// <summary>
        /// Modifies the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        private void UpdateHeight()
        {
            var h = BorderThickness.Top + BorderThickness.Bottom;

            if (Items == null)
            {
                return;
            }

            foreach (var item in Items)
            {
                h += item.Height;
            }

            Height = (int)h;
        }

        /// <summary>
        /// Hides the Items portion of the combo box when selected.
        /// </summary>
        public void Close()
        {
            Visibility = Visibility.Hidden;
        }
    }
}
