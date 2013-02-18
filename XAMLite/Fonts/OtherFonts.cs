using System;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// Other Fonts.
    /// </summary>
    public partial class XAMLiteBaseContentControl
    {
        /// <summary>
        /// Webdings 8 point font
        /// </summary>
        protected static SpriteFont Webdings08SpriteFont { get; private set; }

        /// <summary>
        /// Webdings 10 point font
        /// </summary>
        protected static SpriteFont Webdings10SpriteFont { get; private set; }

        /// <summary>
        /// Webdings 12 point font
        /// </summary>
        protected static SpriteFont Webdings12SpriteFont { get; private set; }

        /// <summary>
        /// Webdings 14 point font
        /// </summary>
        protected static SpriteFont Webdings14SpriteFont { get; private set; }

        /// <summary>
        /// Loads the Webdings fonts.
        /// </summary>
        private void LoadOtherFonts()
        {
            Webdings08SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Webdings08");
            Webdings10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Webdings10");
            Webdings12SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Webdings12");
            Webdings14SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Webdings14");
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateOtherFontFamily()
        {
            switch (FontFamily.ToString())
            {
                case "Webdings08":
                    SpriteFont = Webdings08SpriteFont;
                    break;
                case "Webdings10":
                    SpriteFont = Webdings10SpriteFont;
                    break;
                case "Webdings12":
                    SpriteFont = Webdings12SpriteFont;
                    break;
                case "Webdings14":
                    SpriteFont = Webdings14SpriteFont;
                    break;
                default:
                    throw new Exception("Font not supported.");
            }
        }
    }
}
