﻿using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteRectangle : XAMLiteControl
    {
        /// <summary>
        /// 
        /// </summary>
        private Color _fill;

        /// <summary>
        /// 
        /// </summary>
        public Brush Fill
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _fill = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Color _stroke;

        /// <summary>
        /// The border color of the Rectangle.
        /// </summary>
        public Brush Stroke
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _stroke = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// The border width of the rectangle.
        /// </summary>
        public int StrokeThickness { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRectangle(Game game)
            : base(game)
        {
            _fill = Color.Transparent;
            _stroke = Color.Transparent;
            StrokeThickness = 1;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == System.Windows.Visibility.Visible)
            {
                ConfirmHeightAndWidth();
                // Begin.
                SpriteBatch.Begin();
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                SpriteBatch.Draw(Pixel, Panel, _fill * (float)Opacity);
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, StrokeThickness);
                SpriteBatch.Draw(Pixel, Panel, _stroke * (float)Opacity);
                Panel = new Rectangle((int)Position.X, (int)Position.Y + Height - StrokeThickness, Width, StrokeThickness);
                SpriteBatch.Draw(Pixel, Panel, _stroke * (float)Opacity);
                Panel = new Rectangle((int)Position.X, (int)Position.Y, StrokeThickness, Height);
                SpriteBatch.Draw(Pixel, Panel, _stroke * (float)Opacity);
                Panel = new Rectangle((int)Position.X + Width - StrokeThickness, (int)Position.Y, StrokeThickness, Height);
                SpriteBatch.Draw(Pixel, Panel, _stroke * (float)Opacity);

                // End.
                SpriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfirmHeightAndWidth()
        {
            if (Width == 0)
            {
                Width = Viewport.Width - (int)Margin.Left - (int)Margin.Right;
            }

            if (Height == 0)
            {
                Height = Viewport.Height - (int)Margin.Top - (int)Margin.Bottom;
            }
        }
    }
}
