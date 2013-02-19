using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
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

        /// <summary>
        /// The original margin of the grid as specified by the alignment, 
        /// prior to being adjusted with user defined margins.
        /// </summary>
        private Thickness _originalGridMargin;

        /// <summary>
        /// The original margin of the child as specified by the alignment, 
        /// prior to being adjusted with user defined margins.
        /// </summary>
        private Thickness[] _originalChildMargin;

        /// <summary>
        /// True when the horizontal alignment should be centered.
        /// </summary>
        private bool[] _isHorCentered;

        /// <summary>
        /// True when the vertical alignment should be centered.
        /// </summary>
        private bool[] _isVerCentered;

        /// <summary>
        /// True when the horizontal alignment should be stretched to the 
        /// grid's width.
        /// </summary>
        private bool[] _isHorStretched;

        /// <summary>
        /// True when the vertical alignment should be stretched to the 
        /// grid's height.
        /// </summary>
        private bool[] _isVerStretched;

        /// <summary>
        /// Holds a record of the child's natural visibility prior to being affected by the grid.
        /// </summary>
        private List<bool> _childVisibility;

        /// <summary>
        /// Holds a record of the child's natural opacity and is used to modify its opacity according to
        /// the opacity of the grid (grid opacity * child opacity).
        /// </summary>
        private float[] _childOpacity;

        /// <summary>
        /// Background color of the Grid.
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// Background color of the Grid.
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                _transparent = value == Brushes.Transparent;
            }
        }

        /// <summary>
        /// true when the background color of the grid is transparent.
        /// </summary>
        private bool _transparent;

        /// <summary>
        /// True when a child of the grid becomes visible after being loaded.
        /// </summary>
        private bool _isVisible;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteGrid(Game game)
            : base(game)
        {
            Children = new List<XAMLiteControl>();
        }

        /// <summary>
        /// Loads the content of the grid.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _originalGridMargin = Margin;
            _originalChildMargin = new Thickness[Children.Count];
            _isHorCentered = new bool[Children.Count];
            _isVerCentered = new bool[Children.Count];
            _isHorStretched = new bool[Children.Count];
            _isVerStretched = new bool[Children.Count];
        }

        /// <summary>
        /// Updates the grid.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_childrenLoaded)
            {
                LoadChildren(gameTime);
            }

            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)Position.X - (int)_originalGridMargin.Left + (int)Margin.Left + (int)_originalGridMargin.Right - (int)Margin.Right,
                    (int)Position.Y - (int)_originalGridMargin.Top + (int)Margin.Top + (int)_originalGridMargin.Bottom - (int)Margin.Bottom,
                    Width,
                    Height);
                ModifyChildren();
            }

            // Update Visibility of Children
            if (VisibilityChanged)
            {
                VisibilityChanged = false;
                UpdateChildVisibility();
            }

            //Update the opacity of the child according to the change in the grid's opacity.
            if (OpacityChanged)
            {
                OpacityChanged = false;
                UpdateChildOpacity();
            }

            // makes sure that if Opacity of child was changed separate from grid after initialization, then
            // it should limit the increase to that of the grid's.
            CheckChildrenOpacity();

            if (_childrenLoaded && !_isVisible && Visible == Visibility.Visible)
            {
                _isVisible = true;

                var index = 0;
                foreach (var t in Children)
                {
                    if (_childVisibility[index])
                    {
                        t.Visible = Visibility.Visible;
                    }

                    index++;
                }
            }
        }

        /// <summary>
        /// Draws the background for the grid, if it exists.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (!_transparent)
                {
                    SpriteBatch.Draw(Pixel, Panel, _backgroundColor * (float)Opacity);
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        /// <param name="gameTime"> </param>
        private void LoadChildren(GameTime gameTime)
        {
            _childrenLoaded = true;

            _childOpacity = new float[Children.Count];

            RecordChildOpacity();
            SaveInitialChildVisibility();
            HideChildren();
            
            Panel = new Rectangle((int)Position.X - (int)_originalGridMargin.Left +
                    (int)Margin.Left + (int)_originalGridMargin.Right - (int)Margin.Right,
                    (int)Position.Y - (int)_originalGridMargin.Top + (int)Margin.Top + (int)_originalGridMargin.Bottom - (int)Margin.Bottom, Width, Height);

            for (var i = 0; i < Children.Count; i++)
            {
                _originalChildMargin[i] = new Thickness(Children[i].Margin.Left, Children[i].Margin.Top,
                    Children[i].Margin.Right, Children[i].Margin.Bottom);
            }

            ModifyChildren();

            // Add the child component to the game with the modified parameters.
            foreach (var t in Children)
            {
                Game.Components.Add(t);
                t.Update(gameTime);
            }
        }

        /// <summary>
        /// Stores the original visibility of the child and initially sets its
        /// visibility to hidden until the grid is fully set up.
        /// </summary>
        private void SaveInitialChildVisibility()
        {
            if (_childVisibility == null)
            {
                _childVisibility = new List<bool>();
            }

            for (var i = 0; i < Children.Count; i++)
            {
                _childVisibility.Add(Children[i].Visible == Visibility.Visible);
            }
        }

        /// <summary>
        /// Modifies the child's margin properties to adhere to the grid
        /// </summary>
        private void ModifyChildren()
        {
            double left = 0;
            double top = 0;
            double right = 0;
            double bottom = 0;

            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Width > Width)
                {
                    Children[i].Width = Width;
                }

                if (Children[i].Height > Height)
                {
                    Children[i].Height = Height;
                }

                switch (Children[i].HorizontalAlignment)
                {
                    // Child component is on the left
                    case HorizontalAlignment.Left:
                        // a cheat to override the nature of our centering process
                        if (!_isHorCentered[i])
                        {
                            left = Panel.X + _originalChildMargin[i].Left;
                        }
                        else if (_isHorStretched[i])
                        {
                            left = Width;
                        }
                        else
                        {
                            left = Panel.X + Width / 2 - Children[i].Width / 2 + _originalChildMargin[i].Left - _originalChildMargin[i].Right;
                        }

                        break;

                    // Child component is on the right
                    case HorizontalAlignment.Right:
                        right = Viewport.Width - (Panel.X + Width) + _originalChildMargin[i].Right;
                        break;

                    // Child component is centered
                    case HorizontalAlignment.Center:
                        _isHorCentered[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        left = Panel.X + (Width / 2) - (Children[i].Width / 2) + _originalChildMargin[i].Left - _originalChildMargin[i].Right;
                        break;

                    case HorizontalAlignment.Stretch:
                        _isHorStretched[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].HorizontalAlignment = HorizontalAlignment.Left;
                        Children[i].Width = Width - (int)_originalChildMargin[i].Left - (int)_originalChildMargin[i].Right;
                        left = Panel.X + _originalChildMargin[i].Left;
                        right = Viewport.Width - (Panel.X + Width) + _originalChildMargin[i].Right;
                        break;
                }

                switch (Children[i].VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        bottom = Viewport.Height - (Panel.Y + Height) + _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Center:
                        _isVerCentered[i] = true;
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        top = Panel.Y + (Height / 2) - (Children[i].Height / 2) + _originalChildMargin[i].Top - _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Stretch:
                        _isVerStretched[i] = true;
                        // a cheat to override the nature of our centering process
                        Children[i].VerticalAlignment = VerticalAlignment.Top;
                        Children[i].Height = Height - (int)_originalChildMargin[i].Top - (int)_originalChildMargin[i].Bottom;
                        top = Panel.Y + _originalChildMargin[i].Top;
                        bottom = Viewport.Height - (Panel.Y + Height) + _originalChildMargin[i].Bottom;
                        break;

                    case VerticalAlignment.Top:
                        if (!_isVerCentered[i])
                        {
                            top = Panel.Y + _originalChildMargin[i].Top;
                        }
                        else if (_isVerStretched[i])
                        {
                            top = Children[i].Height;
                        }
                        else
                        {
                            top = Panel.Y + Height / 2 - Children[i].Height / 2 + _originalChildMargin[i].Top - _originalChildMargin[i].Bottom;
                        }

                        break;
                }

                // Reset Margin
                Children[i].Margin = new Thickness(left, top, right, bottom);
            }
        }

        /// <summary>
        /// This toggles the visibility of the child to Hidden when the grid becomes hidden.  However, if
        /// the grid becomes visible again, the child visibilities are reset to what they were prior.
        /// </summary>
        private void UpdateChildVisibility()
        {
            if (Visible == Visibility.Hidden)
            {
                HideChildren();
            }
            else
            {
                // return the visibility of the child to what it was prior to becoming hidden
                // like its parent.
                if (_childVisibility == null)
                {
                    return;
                }

                for (var i = 0; i < _childVisibility.Count; i++)
                {
                    Children[i].Visible = _childVisibility[i] ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Hides the grid children.
        /// </summary>
        private void HideChildren()
        {
            // change the child visibility to hidden, like the grid.
            foreach (var child in Children)
            {
                child.Visible = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Records the current opacity of the child so that it can be used to modify its
        /// opacity according to the grid's opacity.
        /// </summary>
        private void RecordChildOpacity()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                _childOpacity[i] = (float)Children[i].Opacity;
            }
        }

        /// <summary>
        /// Modifies the opacity of the child according to the opacity of the grid and the opacity
        /// of the child.
        /// </summary>
        private void UpdateChildOpacity()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Opacity = Opacity * _childOpacity[i];
            }
        }

        /// <summary>
        /// This checks to see whether a child's opacity was changed when the grid's opacity
        /// was not.  If it was, a method to modify the specific control according to the
        /// grid's opacity limits is called.
        /// </summary>
        private void CheckChildrenOpacity()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                var previousChildOpacity = _childOpacity[i] * (float)Opacity;

                if (Math.Abs((float)Children[i].Opacity - previousChildOpacity) < 0.01)
                {
                    var opacityDifference = (float)Children[i].Opacity - previousChildOpacity;
                    ModifyChildOpacity(i, opacityDifference);
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
        private void ModifyChildOpacity(int index, float difference)
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

            Children[index].Opacity = _childOpacity[index] * (float)Opacity;
        }
    }
}
