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
        /// <summary>
        /// Holds the list of all controls attached to the grid.
        /// WARNING: In applications where LoadContent(), Update(), Draw(), etc., are explicitly called,
        /// the grid MUST be added as a component to the game after all of its children have been added
        /// as children.
        /// </summary>
        public List<XAMLiteControl> Children { get; set; }

        private Thickness _originalGridMargin;

        private Thickness[] _originalChildMargin;
        private bool[] _isHorCentered;
        private bool[] _isVerCentered;
        private bool[] _isHorStretched;
        private bool[] _isVerStretched;

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

            _originalGridMargin = this.Margin;
            _originalChildMargin = new Thickness[Children.Count];
            _isHorCentered = new bool[Children.Count];
            _isVerCentered = new bool[Children.Count];
            _isHorStretched = new bool[Children.Count];
            _isVerStretched = new bool[Children.Count];
            for (int i = 0; i < Children.Count; i++)
            {
                _originalChildMargin[i] = new Thickness(Children[i].Margin.Left, Children[i].Margin.Top, 
                    Children[i].Margin.Right, Children[i].Margin.Bottom);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (marginChanged)
            {
                marginChanged = false;
                _panel = new Rectangle((int)this.Position.X - (int)_originalGridMargin.Left + 
                    (int)this.Margin.Left + (int)_originalGridMargin.Right - (int)this.Margin.Right, 
                    (int)this.Position.Y - (int)_originalGridMargin.Top + (int)this.Margin.Top + 
                    (int)_originalGridMargin.Bottom - (int)this.Margin.Bottom, this.Width, this.Height);
                modifyChildren();
            }

            if (_visibilityChanged)
            {
                _visibilityChanged = false;

                // Update Visibility of Children
                updateChildVisibility();  
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
                            left = _panel.X + _originalChildMargin[i].Left;
                        else if(_isHorStretched[i])
                        {
                            left = this.Width;
                        }
                        else
                        {
                            left = _panel.X + this.Width / 2 - Children[i].Width / 2 + _originalChildMargin[i].Left 
                                - _originalChildMargin[i].Right;
                        }
                        break;

                    // Child component is on the right
                    case HorizontalAlignment.Right:
                        right = viewport.Width - (_panel.X + this.Width) + _originalChildMargin[i].Right;
                        break;

                    // Child component is centered
                    case HorizontalAlignment.Center:
                        _isHorCentered[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        left = _panel.X + this.Width / 2 - Children[i].Width / 2 + _originalChildMargin[i].Left - _originalChildMargin[i].Right;
                        break;

                    case HorizontalAlignment.Stretch:
                        _isHorStretched[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        Children[i].Width = this.Width - (int)_originalChildMargin[i].Left - (int)_originalChildMargin[i].Right;
                        left = _panel.X + _originalChildMargin[i].Left;
                        right = viewport.Width - (_panel.X + this.Width) + _originalChildMargin[i].Right;
                        break;

                    default:
                        break;
                }

                switch (Children[i].VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        bottom = viewport.Height - (_panel.Y + this.Height) + _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Center:
                        _isVerCentered[i] = true;
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        top = _panel.Y + this.Height / 2 - Children[i].Height / 2 + _originalChildMargin[i].Top - _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Stretch:
                        _isVerStretched[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        Children[i].Height = this.Height - (int)_originalChildMargin[i].Top - (int)_originalChildMargin[i].Bottom;
                        top = _panel.Y + _originalChildMargin[i].Top;
                        bottom = viewport.Height - (_panel.Y + this.Height) + _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Top:
                        if (!_isVerCentered[i])
                            top = _panel.Y + _originalChildMargin[i].Top;
                        else if(_isVerStretched[i]) 
                        {
                            top = Children[i].Height;
                        }
                        else
                        {
                            top = _panel.Y + this.Height / 2 - Children[i].Height / 2 + _originalChildMargin[i].Top - _originalChildMargin[i].Bottom;
                        }
                        break;
                    default:
                        break;
                }

                // Reset Margin
                Children[i].Margin = new Thickness(left, top, right, bottom);

                
                
            }
        }

        private void updateChildVisibility()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Visible = this.Visible;
            }
        }
    }
}
