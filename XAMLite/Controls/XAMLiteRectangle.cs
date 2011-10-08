using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    public class XAMLiteRectangle : XAMLiteControl
    {
        #region Fields
        private Rectangle rect;
        private Texture2D pixel;
        public Color Fill { get; set; }
        public Color Stroke { get; set; }
        public int StrokeThickness;

        #endregion

        #region Properties
        #endregion

        public XAMLiteRectangle(Game game)
            : base(game)
        {
            Fill = Color.White;
            Stroke = Color.Transparent;
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
            this.spriteBatch.Draw(pixel, rect, Fill);
            if (StrokeThickness > 0)
            {
                rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, StrokeThickness);
                this.spriteBatch.Draw(pixel, rect, Stroke);
                rect = new Rectangle((int)this.Position.X, ((int)this.Position.Y + this.Height - StrokeThickness), this.Width, StrokeThickness);
                this.spriteBatch.Draw(pixel, rect, Stroke);
                rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, StrokeThickness, this.Height);
                this.spriteBatch.Draw(pixel, rect, Stroke);
                rect = new Rectangle(((int)this.Position.X + this.Width - StrokeThickness), (int)this.Position.Y, StrokeThickness, this.Height);
                this.spriteBatch.Draw(pixel, rect, Stroke);
            }

            // End.
            this.spriteBatch.End();

        }
    }
}
