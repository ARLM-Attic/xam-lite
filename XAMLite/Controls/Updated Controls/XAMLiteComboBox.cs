using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using System.Windows.Media;

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
            Height = 23;
            Width = 120;
            BorderBrush = Brushes.Black;
            Background = Brushes.White;
        }

        /// <summary>
        /// Loads the content of the Combo Box.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
