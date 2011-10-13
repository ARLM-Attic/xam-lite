using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteGrid : XAMLiteControl
    {
        public List<XAMLiteControl> Children { get; set; }
        protected Rectangle _panel;
        private Texture2D _pixel;

        /// <summary>
        /// 
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// 
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                if ((SolidColorBrush)value == Brushes.Transparent)
                    transparent = true;
                else
                    transparent = false;

            }
        }

        private bool transparent;

        public XAMLiteGrid(Game game)
            : base(game)
        {
            Children = new List<XAMLiteControl>();

            // for Background Color
            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="fontName"></param>
        protected override void LoadContent()
        {
            base.LoadContent();
            _panel = new Rectangle((int)Position.X, (int)Position.Y, this.Width, this.Height);

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].HorizontalAlignment = this.HorizontalAlignment;
                Children[i].VerticalAlignment = this.VerticalAlignment;
                Children[i].Margin = new System.Windows.Thickness((this.Margin.Left + Children[i].Margin.Left), (this.Margin.Top + Children[i].Margin.Top), (this.Margin.Right + Children[i].Margin.Right), (this.Margin.Bottom + Children[i].Margin.Bottom));
            }

            for (int i = 0; i < Children.Count; i++)
                this.Game.Components.Add(Children[i]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (!transparent)
            {
                spriteBatch.Draw(_pixel, _panel, _backgroundColor);
            }

            spriteBatch.End();

            // Begin.
        }

    }
}
