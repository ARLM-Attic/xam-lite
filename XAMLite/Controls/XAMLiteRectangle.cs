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
        private Rectangle _rect;
        private Texture2D _pixel;
        private Color _fill;
        private Color _stroke;

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

        public int StrokeThickness { get; set; }

        #endregion

        public XAMLiteRectangle(Game game)
            : base(game)
        {
            _fill = Color.Transparent;
            _stroke = Color.Transparent;
            StrokeThickness = 1;
            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
            this.Width = 0;
            this.Height = 0;
        }

        ~XAMLiteRectangle()
        {
            _pixel.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            ConfirmHeightAndWidth();
            // Begin.
            this.spriteBatch.Begin();
            _rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            this.spriteBatch.Draw(_pixel, _rect, _fill);
            _rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, StrokeThickness);
            this.spriteBatch.Draw(_pixel, _rect, _stroke);
            _rect = new Rectangle((int)this.Position.X, ((int)this.Position.Y + this.Height - StrokeThickness), this.Width, StrokeThickness);
            this.spriteBatch.Draw(_pixel, _rect, _stroke);
            _rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, StrokeThickness, this.Height);
            this.spriteBatch.Draw(_pixel, _rect, _stroke);
            _rect = new Rectangle(((int)this.Position.X + this.Width - StrokeThickness), (int)this.Position.Y, StrokeThickness, this.Height);
            this.spriteBatch.Draw(_pixel, _rect, _stroke);

            // End.
            this.spriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        private void ConfirmHeightAndWidth()
        {
            if(this.Width == 0)
                this.Width = Game.GraphicsDevice.Viewport.Width - (int)this.Margin.Left - (int)this.Margin.Right;

            if(this.Height == 0)
                this.Height = Game.GraphicsDevice.Viewport.Height - (int)this.Margin.Top - (int)this.Margin.Bottom;
        }
    }
}
