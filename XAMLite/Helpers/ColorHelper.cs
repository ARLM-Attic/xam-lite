using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// Contains helper methods for processing colors.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Returns a value between 0 and 1 measuring the brightness of a 
        /// color, where zero is full dark and 1 is full bright.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float Brightness(Color color)
        {
            var avg = (color.R + color.G + color.G) / 3f;

            return avg / 255;
        }
    }
}
