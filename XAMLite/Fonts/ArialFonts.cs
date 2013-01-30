using System;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// Arial Fonts.
    /// </summary>
    public partial class XAMLiteBaseContentControl
    {
        /// <summary>
        /// Arial 10 point font
        /// </summary>
        protected static SpriteFont ArialSpriteFont { get; private set; }

        /// <summary>
        /// Loads the Arial fonts.
        /// </summary>
        private void LoadArialFonts()
        {
            ArialSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateArialFontFamily()
        {
            switch (FontFamily.ToString())
            {
                case "Arial":
                    SpriteFont = ArialSpriteFont;
                    break;
                default:
                    throw new Exception("Arial font not supported.");
            }
        }
    }
}
