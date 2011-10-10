using System;
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
    public class XAMLiteRectangle : XAMLiteControl
    {
        #region Fields
        private Rectangle rect;
        private Texture2D pixel;
        private Color _fill;
        private Color _stroke;
        public int StrokeThickness { get; set; }

        #endregion

        #region Properties
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
        public Brush Stroke
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _stroke = new Color(color.R, color.G, color.B, color.A);
            }
        }
        #endregion

        public XAMLiteRectangle(Game game)
            : base(game)
        {
            Fill = Brushes.Transparent;
            Stroke = Brushes.Transparent;
            StrokeThickness = 0;
            pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        ~XAMLiteRectangle()
        {
            pixel.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Begin.
            this.spriteBatch.Begin();
            rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            this.spriteBatch.Draw(pixel, rect, _fill);
            if (StrokeThickness > 0)
            {
                rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, StrokeThickness);
                this.spriteBatch.Draw(pixel, rect, _stroke);
                rect = new Rectangle((int)this.Position.X, ((int)this.Position.Y + this.Height - StrokeThickness), this.Width, StrokeThickness);
                this.spriteBatch.Draw(pixel, rect, _stroke);
                rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, StrokeThickness, this.Height);
                this.spriteBatch.Draw(pixel, rect, _stroke);
                rect = new Rectangle(((int)this.Position.X + this.Width - StrokeThickness), (int)this.Position.Y, StrokeThickness, this.Height);
                this.spriteBatch.Draw(pixel, rect, _stroke);
            }

            // End.
            this.spriteBatch.End();

        }
    }
}
