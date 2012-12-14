using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows;
    using System.Windows.Input;

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
        /// Hides the Items portion of the combo box when selected.
        /// </summary>
        public void Close()
        {
            Visibility = Visibility.Hidden;
        }
    }
}
