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
        /// Builds the gradient-styled default buttons.
        /// When using this method, pass in the actual height for the control.  
        /// For gradientContrast, the lower the number, the less extreme the 
        /// contrast between the colors that make up the gradient.
        /// For brightness, the higher the number, the brighter the control.
        /// Numbers for brightness may be positive or negative.
        /// </summary>
        /// <returns></returns>
        public static Texture2D CreateGradientTexture(Game game, int gradientContrast, int height, int brightness)
        {
            const int GradientThickness = 3;
            var t = new Texture2D(game.GraphicsDevice, gradientContrast, height);

            var bgc = new Color[gradientContrast * height];

            for (int i = bgc.Length - 1; i > 0; i--)
            {
                var gradientColor = ((i * 20) / (height * GradientThickness)) - brightness;
                bgc[i] = new Color(gradientColor, gradientColor, gradientColor, gradientColor);
            }

            t.SetData(bgc);

            return t;
        }
    }
}
