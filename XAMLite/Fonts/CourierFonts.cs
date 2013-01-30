using System;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{   
    /// <summary>
    /// Courier Fonts
    /// </summary>
    public partial class XAMLiteBaseContentControl
    {
        /// <summary>
        /// Courier 10 point font
        /// </summary>
        protected static SpriteFont Courier10SpriteFont { get; private set; }

        /// <summary>
        /// Courier 12 point font
        /// </summary>
        protected static SpriteFont Courier12SpriteFont { get; private set; }

        /// <summary>
        /// Courier 12 point Bold font
        /// </summary>
        protected static SpriteFont Courier12BoldSpriteFont { get; private set; }

        /// <summary>
        /// Courier 12 point Italic font
        /// </summary>
        protected static SpriteFont Courier12ItalicSpriteFont { get; private set; }

        /// <summary>
        /// Courier 12 point Bold Italic font
        /// </summary>
        protected static SpriteFont Courier12BoldItalicSpriteFont { get; private set; }

        /// <summary>
        /// Courier 20 point font.
        /// </summary>
        protected static SpriteFont Courier20SpriteFont { get; private set; }

        /// <summary>
        /// Loads the Courier fonts.
        /// </summary>
        private void LoadCourierFonts()
        {
            Courier10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
            Courier20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier20");
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCourierFontFamily()
        {
            switch (FontFamily.ToString())
            {
                case "Courier10":
                    SpriteFont = Courier10SpriteFont;
                    break;
                case "Courier20":
                    SpriteFont = Courier20SpriteFont;
                    break;
                default:
                    throw new Exception("Courier font not supported.");
            }
        }
    }
}
