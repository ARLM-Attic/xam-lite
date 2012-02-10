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
        /// Maintains a list of all controls contained within the grid.
        /// </summary>
        public List<XAMLiteControl> Children { get; set; }

        /// <summary>
        /// Used to determine whether the child has been loaded into the grid.
        /// </summary>
        private bool _childrenLoaded;

        private Thickness _originalGridMargin;

        private Thickness[] _originalChildMargin;
        private bool[] _isHorCentered;
        private bool[] _isVerCentered;
        private bool[] _isHorStretched;
        private bool[] _isVerStretched;

        /// <summary>
        /// Holds a record of the child's natural visibility prior to being affected by the grid.
        /// </summary>
        private bool[] _childVisibility;

        /// <summary>
        /// Holds a record of the child's natural opacity and is used to modify its opacity according to
        /// the opacity of the grid (grid opacity * child opacity).
        /// </summary>
        private float[] _childOpacity;

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
        public override void Initialize()
        {
            base.Initialize();
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

            _originalGridMargin = this.Margin;
            _originalChildMargin = new Thickness[Children.Count];
            _isHorCentered = new bool[Children.Count];
            _isVerCentered = new bool[Children.Count];
            _isHorStretched = new bool[Children.Count];
            _isVerStretched = new bool[Children.Count];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_childrenLoaded)
            {
                loadChildren(gameTime);
            }

            if (marginChanged)
            {
                marginChanged = false;
                panel = new Rectangle((int)this.Position.X - (int)_originalGridMargin.Left +
                    (int)this.Margin.Left + (int)_originalGridMargin.Right - (int)this.Margin.Right,
                    (int)this.Position.Y - (int)_originalGridMargin.Top + (int)this.Margin.Top +
                    (int)_originalGridMargin.Bottom - (int)this.Margin.Bottom, this.Width, this.Height);
                modifyChildren();
            }

            // Update Visibility of Children
            if (visibilityChanged)
            {
                visibilityChanged = false;
                updateChildVisibility();
            }

            //Update the opacity of the child according to the change in the grid's opacity.
            if (opacityChanged)
            {
                opacityChanged = false;
                updateChildOpacity();
            }

            // makes sure that if Opacity of child was changed separate from grid after initialization, then
            // it should limit the increase to that of the grid's.
            checkChildrenOpacity();
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
                    spriteBatch.Draw(pixel, panel, (_backgroundColor * (float)Opacity));
                }
                spriteBatch.End();
            }
            // Begin.
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        /// <param name></param>
        private void loadChildren(GameTime gameTime)
        {
            _childrenLoaded = true;

            _childVisibility = new bool[Children.Count];

            _childOpacity = new float[Children.Count];

            panel = new Rectangle((int)this.Position.X - (int)_originalGridMargin.Left +
                    (int)this.Margin.Left + (int)_originalGridMargin.Right - (int)this.Margin.Right,
                    (int)this.Position.Y - (int)_originalGridMargin.Top + (int)this.Margin.Top +
                    (int)_originalGridMargin.Bottom - (int)this.Margin.Bottom, this.Width, this.Height);

            for (int i = 0; i < Children.Count; i++)
            {
                _originalChildMargin[i] = new Thickness(Children[i].Margin.Left, Children[i].Margin.Top,
                    Children[i].Margin.Right, Children[i].Margin.Bottom);
            }

            modifyChildren();
            recordChildOpacity();
            recordChildVisibility();

            // Add the child component to the game with the modified parameters.
            for (int i = 0; i < Children.Count; i++)
            {
                this.Game.Components.Add(Children[i]);
                Children[i].Update(gameTime);
            }
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
                if (Children[i].Width > this.Width)
                    Children[i].Width = this.Width;
                if (Children[i].Height > this.Height)
                    Children[i].Height = this.Height;

                switch (Children[i].HorizontalAlignment)
                {
                    // Child component is on the left
                    case HorizontalAlignment.Left:
                        if (!_isHorCentered[i]) // a cheat to override the nature of our centering process
                            left = panel.X + _originalChildMargin[i].Left;
                        else if (_isHorStretched[i])
                        {
                            left = this.Width;
                        }
                        else
                        {
                            left = panel.X + this.Width / 2 - Children[i].Width / 2 + _originalChildMargin[i].Left
                                - _originalChildMargin[i].Right;
                        }
                        break;

                    // Child component is on the right
                    case HorizontalAlignment.Right:
                        right = viewport.Width - (panel.X + this.Width) + _originalChildMargin[i].Right;
                        break;

                    // Child component is centered
                    case HorizontalAlignment.Center:
                        _isHorCentered[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        left = panel.X + this.Width / 2 - Children[i].Width / 2 + _originalChildMargin[i].Left - _originalChildMargin[i].Right;
                        break;

                    case HorizontalAlignment.Stretch:
                        _isHorStretched[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        Children[i].Width = this.Width - (int)_originalChildMargin[i].Left - (int)_originalChildMargin[i].Right;
                        left = panel.X + _originalChildMargin[i].Left;
                        right = viewport.Width - (panel.X + this.Width) + _originalChildMargin[i].Right;
                        break;

                    default:
                        break;
                }

                switch (Children[i].VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        bottom = viewport.Height - (panel.Y + this.Height) + _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Center:
                        _isVerCentered[i] = true;
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        top = panel.Y + this.Height / 2 - Children[i].Height / 2 + _originalChildMargin[i].Top - _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Stretch:
                        _isVerStretched[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        Children[i].Height = this.Height - (int)_originalChildMargin[i].Top - (int)_originalChildMargin[i].Bottom;
                        top = panel.Y + _originalChildMargin[i].Top;
                        bottom = viewport.Height - (panel.Y + this.Height) + _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Top:
                        if (!_isVerCentered[i])
                            top = panel.Y + _originalChildMargin[i].Top;
                        else if (_isVerStretched[i])
                        {
                            top = Children[i].Height;
                        }
                        else
                        {
                            top = panel.Y + this.Height / 2 - Children[i].Height / 2 + _originalChildMargin[i].Top - _originalChildMargin[i].Bottom;
                        }
                        break;
                    default:
                        break;
                }

                // Reset Margin
                Children[i].Margin = new Thickness(left, top, right, bottom);
            }
        }

        /// <summary>
        /// Stores the visibilty of the child.
        /// </summary>
        private void recordChildVisibility()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Visible == Visibility.Visible)
                {
                    _childVisibility[i] = true;
                }
                else
                {
                    _childVisibility[i] = false;
                }
            }
        }

        /// <summary>
        /// This toggles the visibilty of the child to Hidden when the grid becomes hidden.  However, if
        /// the grid becomes visible again, the child visibilities are reset to what they were prior.
        /// </summary>
        private void updateChildVisibility()
        {
            if (this.Visible == Visibility.Hidden)
            {
                // before making the child hidden, record its lateset visibility state.
                recordChildVisibility();

                // change the child visibility to hidden, like the grid.
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].Visible = Visibility.Hidden;
                }
            }
            else
            {
                // return the visibility of the child to what it was prior to becoming hidden
                // like its parent.
                for (int i = 0; i < _childVisibility.Length; i++)
                {
                    if (_childVisibility[i])
                    {
                        Children[i].Visible = Visibility.Visible;
                    }
                    else
                    {
                        Children[i].Visible = Visibility.Hidden;
                    }
                }
            }
        }

        /// <summary>
        /// Records the current opacity of the child so that it can be used to modify its
        /// opacity according to the grid's opacity.
        /// </summary>
        private void recordChildOpacity()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                _childOpacity[i] = (float)Children[i].Opacity;
            }
        }

        /// <summary>
        /// Modifies the opacity of the child according to the opacity of the grid and the opacity
        /// of the child.
        /// </summary>
        private void updateChildOpacity()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Opacity = this.Opacity * _childOpacity[i];
            }
        }

        /// <summary>
        /// This checks to see whether a child's opacity was changed when the grid's opacity
        /// was not.  If it was, a method to modify the specific control according to the
        /// grid's opacity limits is called.
        /// </summary>
        private void checkChildrenOpacity()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                float previousChildOpacity = _childOpacity[i] * (float)this.Opacity;

                if ((float)Children[i].Opacity != previousChildOpacity)
                {
                    float opacityDifference = (float)Children[i].Opacity - previousChildOpacity;
                    modifyChildOpacity(i, opacityDifference);
                }
            }
        }

        /// <summary>
        /// If a child's opacity was changed when the grid opacity was not changed, this will modify
        /// the child's opacity according to the grids so that the child's opacity cannot exceed its
        /// parent.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="difference"></param>
        private void modifyChildOpacity(int index, float difference)
        {
            float newChildOpacity = _childOpacity[index] + difference;
            if (newChildOpacity <= 1f && newChildOpacity >= 0f)
            {
                _childOpacity[index] = newChildOpacity;
            }
            else
            {
                if (newChildOpacity > 1f)
                {
                    _childOpacity[index] = 1f;
                }
                else
                {
                    _childOpacity[index] = 0f;
                }
            }
            Children[index].Opacity = _childOpacity[index] * (float)this.Opacity;
        }
    }
}
