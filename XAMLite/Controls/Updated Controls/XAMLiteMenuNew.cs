using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{ 
    /// <summary>
    /// TODO: This probably should be either a XAMLiteImage or a XAMLiteGrid.
    /// </summary>
    public class XAMLiteMenuNew : XAMLiteGridNew
    {
        /// <summary>
        /// List of menu items, if any.  Each item that is added to the menu 
        /// class, is positioned horizontally across, starting from the left
        /// and makes up the different selectable menus for the menu bar.
        /// </summary>
        public Items Items;

        /// <summary>
        /// The brightness of the Upper portion of the menu bar.
        /// </summary>
        public int UpperGradientBrightness;

        /// <summary>
        /// The brightness of the lower portion of the menu bar.
        /// </summary>
        public int LowerGradientBrightness;

        /// <summary>
        /// The top highlight portion of the background.
        /// </summary>
        private XAMLiteImageNew _gradientTop;

        /// <summary>
        /// The bottom highlight portion of the background.
        /// </summary>
        private XAMLiteImageNew _gradientBottom;

        /// <summary>
        /// The background of the menu that the gradient then gets placed over.
        /// </summary>
        private XAMLiteRectangleNew _background;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteMenuNew(Game game)
            : base(game)
        {
            Width = 200;
            Height = 28;
            Background = Brushes.DarkGray;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            UpperGradientBrightness = 175;
            LowerGradientBrightness = 175;
        }

        /// <summary>
        /// Loads all of the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _background = new XAMLiteRectangleNew(Game)
                {
                    Fill = Background,
                    Width = Width,
                    Height = Height
                };
            Children.Add(_background);

            _gradientTop = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 5, Height, UpperGradientBrightness))
                {
                    Width = Width,
                    Height = Height,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    RenderTransform = RenderTransform.FlipVertical
                };
            Children.Add(_gradientTop);

            _gradientBottom = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 5, Height, LowerGradientBrightness))
            {
                Width = Width,
                Height = Height,
                Background = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Children.Add(_gradientBottom);
        }
    }
}
