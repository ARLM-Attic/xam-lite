using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class GradientTextureBuilder
    {
        /// <summary>
        /// Builds gradient-styled textures.
        /// When using this method, pass in the actual height for the control.  
        /// For gradientContrast, the lower the number, the less extreme the 
        /// contrast between the colors that make up the gradient.
        /// For brightness, the higher the number, the brighter the control.
        /// Numbers for brightness may be positive or negative.
        /// </summary>
        /// <param name="game">Reference to the game.</param>
        /// <param name="gradientLevel">Controls how extreme the gradient 
        /// texture will becomes.  The lower the number, the more extreme.
        /// A good starting place is a setting of 3.</param>
        /// <param name="height">The height of the control that the texture is
        /// being created for.</param>
        /// <param name="transparencyLevel">The higher the number, the brighter the 
        /// control. Numbers may also be negative.  The higher the number, 
        /// the greater the amount of transparency.</param>
        /// <returns></returns>
        public static Texture2D CreateGradientTexture(Game game, int gradientLevel, int height, int transparencyLevel)
        {
            //This number is multiplied against the Height to build the gradient color array.
            const int GradientWidth = 55;
            var t = new Texture2D(game.GraphicsDevice, GradientWidth, height);

            var bgc = new Color[GradientWidth * height];
            
            for (var i = bgc.Length - 1; i > 0; i--)
            {
                var gradientColor = ((i * 20) / (height * gradientLevel)) - transparencyLevel;
                bgc[i] = new Color(gradientColor, gradientColor, gradientColor, gradientColor);
            }

            t.SetData(bgc);

            return t;
        }
    }
}
