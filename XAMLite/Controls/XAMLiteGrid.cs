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
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            if ((this.Margin.Left + this.Margin.Right + this.Width) < viewport.Width)
            {
                int differenceInWidth = viewport.Width - this.Width;

                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        left = differenceInWidth / 2 + (int)this.Margin.Left - (int)this.Margin.Right;
                        break;

                    case HorizontalAlignment.Left:
                        left = (int)this.Margin.Left;
                        right = differenceInWidth - left;
                        break;

                    case HorizontalAlignment.Right:
                        right = (int)this.Margin.Right;
                        left = differenceInWidth - right;
                        break;

                    case HorizontalAlignment.Stretch:
                        this.Width = this.viewport.Width;
                        break;

                    default:
                        break;
                }
            }

            if ((this.Margin.Top + this.Margin.Bottom + this.Height) < viewport.Height)
            {
                int differenceInHeight = viewport.Height - this.Height;

                switch (VerticalAlignment)
                {

                    case VerticalAlignment.Bottom:
                        bottom = (int)this.Margin.Bottom;
                        top = differenceInHeight - bottom;
                        break;

                    case VerticalAlignment.Center:
                        top = differenceInHeight / 2 + (int)this.Margin.Top - (int)this.Margin.Bottom;
                        break;

                    case VerticalAlignment.Stretch:
                        this.Height = this.viewport.Height;
                        break;

                    case VerticalAlignment.Top:
                        top = (int)this.Margin.Top;
                        bottom = differenceInHeight - top;
                        break;

                    default:
                        break;
                }

            }
            _panel = new Rectangle((int)left, (int)top, this.Width, this.Height);
            this.Margin = new Thickness(left, top, right, bottom);
            Console.WriteLine("Margin of grid: " + this.Margin.Left + ", " + this.Margin.Top + ", " + this.Margin.Right + ", " + this.Margin.Bottom);
        }

        /// <summary>
        /// Modifies the child's margin properties to adhere to the grid
        /// </summary>
        /// <param name></param>
        private void modifyChildren()
        {
            double left = this.Margin.Left;
            double top = this.Margin.Top;
            double right = this.Margin.Right;
            double bottom = this.Margin.Bottom;

            for (int i = 0; i < Children.Count; i++)
            {
                    int differenceInWidth = this.Width - Children[i].Width;

                    switch (Children[i].HorizontalAlignment)
                    {
                        case HorizontalAlignment.Center:
                            switch (this.HorizontalAlignment)
                            {
                                case HorizontalAlignment.Center:
                                    break;
                                case HorizontalAlignment.Left:
                                    break;
                                case HorizontalAlignment.Right:
                                    break;
                                default: 
                                    break;
                            }
                            break;

                        case HorizontalAlignment.Left:
                            switch (this.HorizontalAlignment)
                            {
                                case HorizontalAlignment.Center:
                                    left = (viewport.Width / 2 - this.Width / 2) + (int)Children[i].Margin.Left - (int)Children[i].Margin.Right;
                                    break;
                                case HorizontalAlignment.Left:
                                    left = (int)Children[i].Margin.Left;
                                    break;
                                case HorizontalAlignment.Right:
                                    left = viewport.Width - this.Width + (int)Children[i].Margin.Left;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case HorizontalAlignment.Right:
                            switch (this.HorizontalAlignment)
                            {
                                case HorizontalAlignment.Center:
                                    right = (viewport.Width / 2 - this.Width / 2) + (int)Children[i].Margin.Right - (int)Children[i].Margin.Left;
                                    break;
                                case HorizontalAlignment.Left:
                                    right = viewport.Width - this.Width + (int)Children[i].Margin.Right;
                                    break;
                                case HorizontalAlignment.Right:
                                    right = (int)Children[i].Margin.Right;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case HorizontalAlignment.Stretch:
                            Children[i].Width = this.Width;
                            break;

                        default:
                            break;
                    }

                    int differenceInHeight = this.Height - Children[i].Height;

                    switch (Children[i].VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            switch (this.VerticalAlignment)
                            {
                                case VerticalAlignment.Center:
                                    bottom = (viewport.Height / 2 - this.Height / 2) + (int)Children[i].Margin.Bottom - (int)Children[i].Margin.Top;
                                    break;
                                case VerticalAlignment.Top:
                                    bottom = viewport.Height - this.Height + (int)Children[i].Margin.Bottom;
                                    break;
                                case VerticalAlignment.Bottom:
                                    bottom = (int)Children[i].Margin.Bottom;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case VerticalAlignment.Center:
                            top = differenceInHeight / 2 + (int)Children[i].Margin.Top - (int)Children[i].Margin.Bottom;
                            switch (this.VerticalAlignment)
                            {
                                case VerticalAlignment.Center:
                                    break;
                                case VerticalAlignment.Top:
                                    break;
                                case VerticalAlignment.Bottom:
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case VerticalAlignment.Stretch:
                            Children[i].Height = this.Height;
                            break;

                        case VerticalAlignment.Top:
                            switch (this.VerticalAlignment)
                            {
                                case VerticalAlignment.Center:
                                    top = (viewport.Height / 2 - this.Height / 2) + (int)Children[i].Margin.Top - (int)Children[i].Margin.Bottom;
                                    break;
                                case VerticalAlignment.Top:
                                    top = (int)Children[i].Margin.Top;
                                    break;
                                case VerticalAlignment.Bottom:
                                    top = viewport.Height - this.Height + (int)Children[i].Margin.Top;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }

                Children[i].Margin = new Thickness(left, top, right, bottom);
                Console.WriteLine("Margin of image: " + Children[i].Margin.Left + ", " + Children[i].Margin.Top + ", " + Children[i].Margin.Right + ", " + Children[i].Margin.Bottom);
            }
        }

        /// <summary>
        /// Modifies the child's margin properties to adhere to the grid
        /// </summary>
        /// <param name></param>
        /*private void modifyChildren()
        {
            double left = this.Margin.Left;
            double top = this.Margin.Top;
            double right = this.Margin.Right;
            double bottom = this.Margin.Bottom;

            for (int i = 0; i < Children.Count; i++)
            {
                if (this.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    if (Children[i].HorizontalAlignment == HorizontalAlignment.Center)
                    {
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        left += Children[i].Margin.Left + this.Width / 2 - Children[i].Width / 2;
                    }
                    else
                    {
                        left += Children[i].Margin.Left;
                        right += Children[i].Margin.Right;
                    }
                }
                else if (this.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    if (Children[i].HorizontalAlignment == HorizontalAlignment.Center)
                    {
                        Children[i].HorizontalAlignment = HorizontalAlignment.Right;
                        right += Children[i].Margin.Right + this.Width / 2 - Children[i].Width / 2;
                        //left += Children[i].Margin.Left;
                    }
                    else
                    {
                        left += Children[i].Margin.Left;
                        right += Children[i].Margin.Right;
                    }
                }
                else if (this.HorizontalAlignment == HorizontalAlignment.Center)
                {
                    left += Children[i].Margin.Left;
                    right += Children[i].Margin.Right; // working
                }

                if (this.VerticalAlignment == VerticalAlignment.Top)
                {
                    Children[i].VerticalAlignment = VerticalAlignment.Top;

                    if (Children[i].VerticalAlignment == VerticalAlignment.Center)
                    {
                        //Console.WriteLine("Inhere!!");
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        top += Children[i].Margin.Top + this.Height / 2 - Children[i].Height / 2;
                        //bottom += Children[i].Margin.Bottom;
                    }
                    else
                    {
                        
                        top += Children[i].Margin.Top;
                        bottom += Children[i].Margin.Bottom;
                    }
                }
                else if (this.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    Children[i].VerticalAlignment = VerticalAlignment.Bottom;
                    if (Children[i].VerticalAlignment == VerticalAlignment.Center)
                    {
                        
                        bottom += Children[i].Margin.Bottom + this.Height / 2 - Children[i].Height / 2;
                        //top += Children[i].Margin.Top;
                    }
                    else
                    {
                        Console.WriteLine("Inhere!!");
                        top += Children[i].Margin.Top;
                        bottom += Children[i].Margin.Bottom;
                    }
                }
                else if (this.VerticalAlignment == VerticalAlignment.Center)
                {
                    top += Children[i].Margin.Top;
                    bottom += Children[i].Margin.Bottom; // working
                }
                

                Children[i].Margin = new Thickness(left, top, right, bottom);
                    
                Console.WriteLine("Margin of image: " + Children[i].Margin.Left + ", " + Children[i].Margin.Top + ", " + Children[i].Margin.Right + ", " + Children[i].Margin.Bottom);
            }
        }*/
    }
}
