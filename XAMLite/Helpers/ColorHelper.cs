using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XAMLite
{
    using System.Windows.Media;
    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// Contains helper methods for processing colors.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Returns a value between 0 and 1 measuring the brightness of a 
        /// Brush, where zero is full dark and 1 is full bright.
        /// </summary>
        /// <param name="brush">The brush color.</param>
        /// <returns></returns>
        public static float Brightness(Brush brush)
        {
            var solidBrush = (SolidColorBrush)brush;
            var color = solidBrush.Color;
            var r = (color.R / 255f) / 3f;
            var g = (color.G / 255f) / 3f;
            var b = (color.B / 255f) / 3f;
            
            return r + g + b;
        }
    }
}
