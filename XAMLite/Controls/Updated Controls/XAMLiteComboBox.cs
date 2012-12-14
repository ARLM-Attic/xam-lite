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

            if (Height > (int)h)
            {
                //// move the bottom rectangle up.
                //if (_borderRectangles.Count > 1)
                //{
                //    var rect = _borderRectangles[_borderRectangles.Count - 1];
                //    rect.Margin = new Thickness(
                //        rect.Margin.Left, rect.Margin.Top, rect.Margin.Right, rect.Margin.Bottom + (Height - h));
                //}

                Height = (int)h;
                //_grid.Height = (int)h;

                //foreach (var rectangle in _borderRectangles)
                //{
                //    if (rectangle.Height > Height)
                //    {
                //        rectangle.Height = Height;
                //    }
                //}
            }
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
