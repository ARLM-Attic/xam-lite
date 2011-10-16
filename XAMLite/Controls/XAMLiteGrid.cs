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

        /// <summary>
        /// 
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return base.Margin;
            }
            set
            {
                base.Margin = value;
                gridMarginChanged = true;
            }
        }

        private Thickness[] _originalMargin;
        private bool[] _isHorCentered;
        private bool[] _isVerCentered;

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
        private bool gridMarginChanged = true;

        public XAMLiteGrid(Game game)
            : base(game)
        {
            Children = new List<XAMLiteControl>();
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

            for (int i = 0; i < Children.Count; i++)
            {
                this.Game.Components.Add(Children[i]);
            }

            _originalMargin = new Thickness[Children.Count];
            _isHorCentered = new bool[Children.Count];
            _isVerCentered = new bool[Children.Count];
            for (int i = 0; i < Children.Count; i++)
            {
                _originalMargin[i] = new Thickness(Children[i].Margin.Left, Children[i].Margin.Top, Children[i].Margin.Right, Children[i].Margin.Bottom);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gridMarginChanged)
            {
                gridMarginChanged = false;
                _panel = new Rectangle((int)this.Position.X - (int)this.Margin.Right + (int)this.Margin.Left, (int)this.Position.Y + (int)this.Margin.Top - (int)this.Margin.Bottom, this.Width, this.Height);
                modifyChildren();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                spriteBatch.Begin();

                if (!transparent)
                {
                    spriteBatch.Draw(_pixel, _panel, (_backgroundColor * (float)Opacity));
                }

                spriteBatch.End();
            }

            // Begin.
        }

        /// <summary>
        /// Modifies the child's margin properties to adhere to the grid
        /// </summary>
        /// <param name></param>
        private void modifyChildren()
        {
            double left = 0;
            double top = 0;
            double right = 0;
            double bottom = 0;

            for (int i = 0; i < Children.Count; i++)
            {
                switch (Children[i].HorizontalAlignment)
                {
                    // Child component is on the left
                    case HorizontalAlignment.Left:
                        if (!_isHorCentered[i]) // a cheat to override the nature of our centering process
                            left = _panel.X + _originalMargin[i].Left;
                        else
                        {
                            left = _panel.X + this.Width / 2 - Children[i].Width / 2 + _originalMargin[i].Left - _originalMargin[i].Right;

                        }
                        break;

                    // Child component is on the right
                    case HorizontalAlignment.Right:
                        right = viewport.Width - (_panel.X + this.Width) + _originalMargin[i].Right;
                        break;

                    // Child component is centered
                    case HorizontalAlignment.Center:
                        _isHorCentered[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        left = _panel.X + this.Width / 2 - Children[i].Width / 2 + _originalMargin[i].Left - _originalMargin[i].Right;
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
                        bottom = viewport.Height - (_panel.Y + this.Height) + _originalMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Center:
                        _isVerCentered[i] = true;
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        top = _panel.Y + this.Height / 2 - Children[i].Height / 2 + _originalMargin[i].Top - _originalMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Stretch:
                        Children[i].Height = this.Height;
                        break;

                    case VerticalAlignment.Top:
                        if (!_isVerCentered[i])
                            top = _panel.Y + _originalMargin[i].Top;
                        else
                        {
                            top = _panel.Y + this.Height / 2 - Children[i].Height / 2 + _originalMargin[i].Top - _originalMargin[i].Bottom;
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
