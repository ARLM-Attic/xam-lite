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
    public class XAMLiteComboBox : XAMLiteBaseText
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// List of Combo Box Items that make up the content of the control.
        /// </summary>
        public List<XAMLiteComboBoxItem> Items; 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteComboBox(Game game)
            : base(game)
        {
            Items = new List<XAMLiteComboBoxItem>();
        }
    }
}
