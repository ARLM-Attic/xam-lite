using System;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{  
    /// <summary>
    /// Verdana Fonts.
    /// </summary>
    public partial class XAMLiteBaseContentControl
    {
        /// <summary>
        /// Verdana 10 point font.
        /// </summary>
        protected static SpriteFont Verdana10SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 10 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana10BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 11 point font.
        /// </summary>
        protected static SpriteFont Verdana11SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 11 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana11BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 12 point font.
        /// </summary>
        protected static SpriteFont Verdana12SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 12 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana12BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 13 point font.
        /// </summary>
        protected static SpriteFont Verdana13SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 13 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana13BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 14 point font.
        /// </summary>
        protected static SpriteFont Verdana14SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 14 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana14BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 15 point font.
        /// </summary>
        protected static SpriteFont Verdana15SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 16 point font.
        /// </summary>
        protected static SpriteFont Verdana16SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 16 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana16BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 20 point font.
        /// </summary>
        protected static SpriteFont Verdana20SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 20 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana20BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 24 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana24BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 60 point font.
        /// </summary>
        protected static SpriteFont Verdana60SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 60 point Bold font.
        /// </summary>
        protected static SpriteFont Verdana60BoldSpriteFont { get; private set; }

        /// <summary>
        /// Loads the Verdana fonts.
        /// </summary>
        private void LoadVerdanaFonts()
        { 
            Verdana10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana10");
            Verdana10BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana10Bold");
            Verdana11SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana11");
            Verdana11BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana11Bold");
            Verdana12SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana12");
            Verdana12BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana12Bold");
            Verdana13SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana13");
            Verdana13BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana13Bold");
            Verdana14SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana14");
            Verdana14BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana14Bold");
            Verdana15SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana15");
            Verdana16SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana16");
            Verdana16BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana16Bold");
            Verdana20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana20");
            Verdana20BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana20Bold");
            Verdana24BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana24Bold");
            Verdana60SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60");
            Verdana60BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60Bold");
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateVerdanaFontFamily()
        {
            switch (FontFamily.ToString())
            {
                case "Verdana10":
                    SpriteFont = Verdana10SpriteFont;
                    break;
                case "Verdana10Bold":
                    SpriteFont = Verdana10BoldSpriteFont;
                    break;
                case "Verdana11":
                    SpriteFont = Verdana11SpriteFont;
                    break;
                case "Verdana11Bold":
                    SpriteFont = Verdana11BoldSpriteFont;
                    break;
                case "Verdana12":
                    SpriteFont = Verdana12SpriteFont;
                    break;
                case "Verdana12Bold":
                    SpriteFont = Verdana12BoldSpriteFont;
                    break;
                case "Verdana13":
                    SpriteFont = Verdana13SpriteFont;
                    break;
                case "Verdana13Bold":
                    SpriteFont = Verdana13BoldSpriteFont;
                    break;
                case "Verdana14":
                    SpriteFont = Verdana14SpriteFont;
                    break;
                case "Verdana14Bold":
                    SpriteFont = Verdana14BoldSpriteFont;
                    break;
                case "Verdana15":
                    SpriteFont = Verdana15SpriteFont;
                    break;
                case "Verdana16":
                    SpriteFont = Verdana16SpriteFont;
                    break;
                case "Verdana16Bold":
                    SpriteFont = Verdana16BoldSpriteFont;
                    break;
                case "Verdana20":
                    SpriteFont = Verdana20SpriteFont;
                    break;
                case "Verdana20Bold":
                    SpriteFont = Verdana20BoldSpriteFont;
                    break;
                case "Verdana24Bold":
                    SpriteFont = Verdana24BoldSpriteFont;
                    break;
                case "Verdana60":
                    SpriteFont = Verdana60SpriteFont;
                    break;
                case "Verdana60Bold":
                    SpriteFont = Verdana60BoldSpriteFont;
                    break;
                default:
                    throw new Exception("Verdana font not supported.");
            }
        }
    }
}
