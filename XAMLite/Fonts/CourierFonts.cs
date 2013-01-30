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
        /// Courier 10 point Bold font
        /// </summary>
        protected static SpriteFont Courier10BoldSpriteFont { get; private set; }

        /// <summary>
        /// Courier 10 point Italic font
        /// </summary>
        protected static SpriteFont Courier10ItalicSpriteFont { get; private set; }

        /// <summary>
        /// Courier 10 point Bold Italic font
        /// </summary>
        protected static SpriteFont Courier10BoldItalicSpriteFont { get; private set; }

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
        /// Courier 20 point Bold font.
        /// </summary>
        protected static SpriteFont Courier20BoldSpriteFont { get; private set; }

        /// <summary>
        /// Courier 20 point Italic font.
        /// </summary>
        protected static SpriteFont Courier20ItalicSpriteFont { get; private set; }

        /// <summary>
        /// Courier 20 point Bold Italic font.
        /// </summary>
        protected static SpriteFont Courier20BoldItalicSpriteFont { get; private set; }

        /// <summary>
        /// Loads the Courier fonts.
        /// </summary>
        private void LoadCourierFonts()
        {
            Courier10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
            Courier10BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10Bold");
            Courier10ItalicSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10Italic");
            Courier10BoldItalicSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10BoldItalic");
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
                case "Courier10Bold":
                    SpriteFont = Courier10BoldSpriteFont;
                    break;
                case "Courier10Italic":
                    SpriteFont = Courier10ItalicSpriteFont;
                    break;
                case "Courier10BoldItalic":
                    SpriteFont = Courier10BoldItalicSpriteFont;
                    break;
                case "Courier12":
                    SpriteFont = Courier12SpriteFont;
                    break;
                case "Courier12Bold":
                    SpriteFont = Courier12BoldSpriteFont;
                    break;
                case "Courier12Italic":
                    SpriteFont = Courier12ItalicSpriteFont;
                    break;
                case "Courier12BoldItalic":
                    SpriteFont = Courier12BoldItalicSpriteFont;
                    break;
                case "Courier20":
                    SpriteFont = Courier20SpriteFont;
                    break;
                case "Courier20Bold":
                    SpriteFont = Courier20BoldSpriteFont;
                    break;
                case "Courier20Italic":
                    SpriteFont = Courier20ItalicSpriteFont;
                    break;
                case "Courier20BoldItalic":
                    SpriteFont = Courier20BoldItalicSpriteFont;
                    break;
                default:
                    throw new Exception("Courier font not supported.");
            }
        }
    }
}
