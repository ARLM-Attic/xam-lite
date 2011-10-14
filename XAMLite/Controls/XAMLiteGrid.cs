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
        private Rectangle _panel;
        private Texture2D _pixel;
        private Thickness _oldMargin;

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

            _oldMargin = new Thickness(0, 0, 0, 0);
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

            Rectangle msRect = new Rectangle(ms.X, ms.Y, 1, 1);
            if (_panel.Contains(msRect))
            {
                if (!_mouseEnter)
                {
                    _mouseEnter = true;
                    OnMouseEnter();
                }
                if (_mouseDown)
                {
                    _mouseDown = false;

                    OnMouseDown();
                }
            }
            else
            {
                if (_mouseEnter)
                {
                    _mouseEnter = false;
                    OnMouseLeave();
                }
            }
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
                spriteBatch.Draw(_pixel, _panel, (_backgroundColor * (float)Opacity));
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
            _oldMargin = this.Margin;
            this.Margin = new Thickness(left, top, right, bottom);
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
                switch (Children[i].HorizontalAlignment)
                {

                    case HorizontalAlignment.Left:
                        switch (this.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Center:
                                left = viewport.Width / 2 - this.Width / 2 + (int)Children[i].Margin.Left - (int)Children[i].Margin.Right;
                                break;
                            case HorizontalAlignment.Left:
                                left = (int)Children[i].Margin.Left + this.Margin.Left;
                                break;
                            case HorizontalAlignment.Right:
                                left = viewport.Width - this.Width + (int)Children[i].Margin.Left - this.Margin.Right;
                                Console.WriteLine(left);
                                break;
                            default:
                                break;
                        }
                        break;

                    case HorizontalAlignment.Right:
                        switch (this.HorizontalAlignment)
                        {

                            case HorizontalAlignment.Center:
                                right = viewport.Width / 2 - this.Width / 2 + (int)Children[i].Margin.Right - (int)Children[i].Margin.Left + _oldMargin.Right - _oldMargin.Left;
                                break;
                            case HorizontalAlignment.Left:
                                right = viewport.Width - this.Width + (int)Children[i].Margin.Right - this._oldMargin.Left;
                                break;
                            case HorizontalAlignment.Right:
                                right = (int)Children[i].Margin.Right + this.Margin.Right;
                                break;
                            default:
                                break;
                        }
                        break;

                    case HorizontalAlignment.Center:
                        switch (this.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Center:
                                Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                                left = (int)Children[i].Margin.Left - (int)Children[i].Margin.Right + this.Margin.Left - this.Margin.Right + this.Width / 2 - Children[i].Width / 2;
                                break;
                            case HorizontalAlignment.Left:
                                Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                                left = (int)Children[i].Margin.Left - (int)Children[i].Margin.Right + this.Margin.Left + this.Width / 2 - Children[i].Width / 2;
                                break;
                            case HorizontalAlignment.Right:
                                Children[i].HorizontalAlignment = HorizontalAlignment.Right;
                                right = (int)Children[i].Margin.Right - (int)Children[i].Margin.Left + this.Margin.Right + this.Width / 2 - Children[i].Width / 2;
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

                switch (Children[i].VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        switch (this.VerticalAlignment)
                        {
                            case VerticalAlignment.Center:
                                bottom = (viewport.Height / 2 - this.Height / 2) + (int)Children[i].Margin.Bottom - (int)Children[i].Margin.Top;
                                break;
                            case VerticalAlignment.Top:
                                bottom = viewport.Height - this.Height + (int)Children[i].Margin.Bottom - this.Margin.Top;
                                break;
                            case VerticalAlignment.Bottom:
                                Console.WriteLine(this.Margin.Bottom);
                                bottom = (int)Children[i].Margin.Bottom + this.Margin.Bottom;
                                break;
                            default:
                                break;
                        }
                        break;

                    case VerticalAlignment.Center:
                        switch (this.VerticalAlignment)
                        {
                            case VerticalAlignment.Center:
                                Children[i].VerticalAlignment = VerticalAlignment.Top;
                                top = (int)Children[i].Margin.Top - (int)Children[i].Margin.Bottom + this.Margin.Top - this.Margin.Bottom + this.Height / 2 - Children[i].Height / 2;
                                break;
                            case VerticalAlignment.Top:
                                Children[i].VerticalAlignment = VerticalAlignment.Top;
                                top = (int)Children[i].Margin.Top - (int)Children[i].Margin.Bottom + this.Margin.Top + this.Height / 2 - Children[i].Height / 2;
                                break;
                            case VerticalAlignment.Bottom:
                                Children[i].VerticalAlignment = VerticalAlignment.Bottom;
                                bottom = (int)Children[i].Margin.Bottom - (int)Children[i].Margin.Top + this.Margin.Bottom + this.Height / 2 - Children[i].Height / 2;
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
                                top = (viewport.Height / 2 - this.Height / 2) + (int)Children[i].Margin.Top - (int)Children[i].Margin.Bottom + this._oldMargin.Top;
                                break;
                            case VerticalAlignment.Top:
                                top = (int)Children[i].Margin.Top + this.Margin.Top;
                                break;
                            case VerticalAlignment.Bottom:
                                top = viewport.Height - this.Height + (int)Children[i].Margin.Top - this._oldMargin.Bottom;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                Children[i].Margin = new Thickness(left, top, right, bottom);
            }
        }
    }
}
