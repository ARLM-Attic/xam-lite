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

            adjustGridMargins();

            modifyChildren();
            
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

        private void adjustGridMargins()
        {
            double left = this.Margin.Left;
            double top = this.Margin.Top;
            double right = this.Margin.Right;
            double bottom = this.Margin.Bottom;

            if ((this.Margin.Left + this.Margin.Right + this.Width) < viewport.Width)
            {
                int differenceInWidth = viewport.Width - this.Width - ((int)this.Margin.Left + (int)this.Margin.Right);

                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        left += differenceInWidth / 2;
                        right += differenceInWidth / 2;
                        break;

                    case HorizontalAlignment.Left:
                        right += differenceInWidth;
                        break;

                    case HorizontalAlignment.Right:
                        left += differenceInWidth;
                        break;

                    case HorizontalAlignment.Stretch:
                        //this.Width = this.viewport.Width;
                        break;

                    default:
                        break;
                }
            }

            if ((this.Margin.Top + this.Margin.Bottom + this.Height) < viewport.Height)
            {
                int differenceInHeight = viewport.Height - this.Height - ((int)this.Margin.Top + (int)this.Margin.Bottom);

                switch (VerticalAlignment)
                {

                    case VerticalAlignment.Bottom:
                        top += differenceInHeight;
                        break;

                    case VerticalAlignment.Center:
                        top += differenceInHeight / 2;
                        bottom += differenceInHeight / 2;
                        break;

                    case VerticalAlignment.Stretch:
                        // this.Height = this.viewport.Height;
                        break;

                    case VerticalAlignment.Top:
                        bottom += differenceInHeight;
                        break;

                    default:
                        break;
                }

            }
            this.Margin = new Thickness(left, top, right, bottom);
            Console.WriteLine("Margin of grid: " + this.Margin.Left + ", " + this.Margin.Top + ", " + this.Margin.Right + ", " + this.Margin.Bottom);
            
        }

        /// <summary>
        /// Modifies the child's margin properties to adhere to the grid
        /// </summary>
        /// <param name></param>
        private void modifyChildren()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                double left = Children[i].Margin.Left;
                double right = Children[i].Margin.Right;
                double top = Children[i].Margin.Top;
                double bottom = Children[i].Margin.Bottom;

                if (Children[i].HorizontalAlignment != HorizontalAlignment.Center)
                {
                    left += this.Margin.Left;
                    right += this.Margin.Right;
                }
                else
                {
                    left += this.Margin.Left;
                    right += this.Margin.Right;
                }
                if (Children[i].VerticalAlignment != VerticalAlignment.Center)
                {
                    top += this.Margin.Top;
                    bottom += this.Margin.Top;
                }
                else
                {
                    top += this.Margin.Top;
                    bottom += this.Margin.Top;
                }
                Children[i].Margin = new Thickness(left, top, right, bottom);
                Console.WriteLine("Margin of image: " + Children[i].Margin.Left + ", " + Children[i].Margin.Top + ", " + Children[i].Margin.Right + ", " + Children[i].Margin.Bottom);
            }
        }
    }
}
