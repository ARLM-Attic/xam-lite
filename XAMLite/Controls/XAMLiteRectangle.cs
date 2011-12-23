﻿using System;
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
            pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            this.Width = 0;
            this.Height = 0;
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
            if (Visible == System.Windows.Visibility.Visible)
            {

                ConfirmHeightAndWidth();
                // Begin.
                this.spriteBatch.Begin();
                panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
                this.spriteBatch.Draw(pixel, panel, (_fill * (float)Opacity));
                panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, StrokeThickness);
                this.spriteBatch.Draw(pixel, panel, (_stroke * (float)Opacity));
                panel = new Rectangle((int)this.Position.X, ((int)this.Position.Y + this.Height - StrokeThickness), this.Width, StrokeThickness);
                this.spriteBatch.Draw(pixel, panel, (_stroke * (float)Opacity));
                panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, StrokeThickness, this.Height);
                this.spriteBatch.Draw(pixel, panel, (_stroke * (float)Opacity));
                panel = new Rectangle(((int)this.Position.X + this.Width - StrokeThickness), (int)this.Position.Y, StrokeThickness, this.Height);
                this.spriteBatch.Draw(pixel, panel, (_stroke * (float)Opacity));

                // End.
                this.spriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        private void ConfirmHeightAndWidth()
        {
            if(this.Width == 0)
                this.Width = this.viewport.Width - (int)this.Margin.Left - (int)this.Margin.Right;

            if(this.Height == 0)
                this.Height = this.viewport.Height - (int)this.Margin.Top - (int)this.Margin.Bottom;
        }
    }
}
