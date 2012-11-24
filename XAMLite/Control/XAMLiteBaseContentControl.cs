using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    using System.Windows.Media;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteBaseContentControl : XAMLiteBaseControl
    {
        /// <summary>
        /// Font texture.
        /// </summary>
        protected SpriteFont SpriteFont;

        /// <summary>
        /// Arial 10 pt font
        /// </summary>
        protected static SpriteFont ArialSpriteFont { get; private set; }

        /// <summary>
        /// Courier 10 pt font
        /// </summary>
        protected static SpriteFont Courier10SpriteFont { get; private set; }

        /// <summary>
        /// Courier 20pt font.
        /// </summary>
        protected static SpriteFont Courier20SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 10 pt font.
        /// </summary>
        protected static SpriteFont Verdana10SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 10 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana10BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 11 pt font.
        /// </summary>
        protected static SpriteFont Verdana11SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 11 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana11BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 12 pt font.
        /// </summary>
        protected static SpriteFont Verdana12SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 12 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana12BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 13 pt font.
        /// </summary>
        protected static SpriteFont Verdana13SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 13 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana13BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 14 pt font.
        /// </summary>
        protected static SpriteFont Verdana14SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 14 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana14BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 15 pt font.
        /// </summary>
        protected static SpriteFont Verdana15SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 16 pt font.
        /// </summary>
        protected static SpriteFont Verdana16SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 16 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana16BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 20 pt font.
        /// </summary>
        protected static SpriteFont Verdana20SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 20 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana20BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 24 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana24BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 60 pt font.
        /// </summary>
        protected static SpriteFont Verdana60SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 60 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana60BoldSpriteFont { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseContentControl(Game game)
            : base(game)
        {    
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            ArialSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
            Courier10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
            Courier20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier20");
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

            //Default this controls font
            SpriteFont = Courier10SpriteFont;
        }

        /// <summary>
        /// Updates the font family.
        /// </summary>
        /// <param name="fontFamily"></param>
        protected void UpdateFontFamily(FontFamily fontFamily)
        {
            switch (fontFamily.ToString())
            {
                case "Arial":
                    SpriteFont = ArialSpriteFont;
                    break;
                case "Courier20":
                    SpriteFont = Courier20SpriteFont;
                    break;
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
                    SpriteFont = Courier10SpriteFont;
                    break;
            }
        }

        /// <summary>
        /// Recalculate the width and height of the control.
        /// </summary>
        protected virtual void RecalculateWidthAndHeight()
        {
        }
    }
}
