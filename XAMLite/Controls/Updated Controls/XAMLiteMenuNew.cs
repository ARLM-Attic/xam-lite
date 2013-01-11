using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// TODO: This probably should be either a XAMLiteImage or a XAMLiteGrid.
    /// </summary>
    public class XAMLiteMenuNew : XAMLiteGridNew
    {
        /// <summary>
        /// List of menu items, if any.  Each item that is added to the menu 
        /// class, is positioned horizontally across, starting from the left
        /// and makes up the different selectable menus for the menu bar.
        /// </summary>
        public Items Items;

        /// <summary>
        /// The background of the control that holds all of the menu item headers.
        /// </summary>
        private XAMLiteImageNew _background;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuNew(Game game)
            : base(game)
        {
            Width = 200;
            Height = 23;
        }

        /// <summary>
        /// Loads all of the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _background = new XAMLiteImageNew(Game);
            Game.Components.Add(_background);
        }
    }
}
