using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteGrid class until this class replaces it.
    /// </summary>
    public class XAMLiteGridNew : XAMLiteBaseControl
    {
        /// <summary>
        /// Maintains a list of all controls contained within the grid.
        /// </summary>
        public List<XAMLiteBaseControl> Children { get; set; }

        /// <summary>
        /// Used to determine whether the child has been loaded into the grid.
        /// </summary>
        private bool _childrenLoaded;

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
        /// True when a child of the grid becomes visible after being loaded.
        /// </summary>
        private bool _isVisible;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteGridNew(Game game)
            : base(game)
        {
            Children = new List<XAMLiteBaseControl>();
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
                LoadChildren();
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

            if (_childrenLoaded && !_isVisible)
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
        ///  Used only for debugging purposes.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            // For debugging: Draw a dot in the corners and center of the grid.
            //SpriteBatch.Draw(Pixel, new Rectangle((int)TopLeftCorner.X, (int)TopLeftCorner.Y, 1, 1), Color.Aquamarine);
            //SpriteBatch.Draw(Pixel, new Rectangle((int)BottomLeftCorner.X, (int)BottomLeftCorner.Y, 1, 1), Color.Aquamarine);
            //SpriteBatch.Draw(Pixel, new Rectangle((int)TopRightCorner.X, (int)TopRightCorner.Y, 1, 1), Color.Aquamarine);
            //SpriteBatch.Draw(Pixel, new Rectangle((int)BottomRightCorner.X, (int)BottomRightCorner.Y, 1, 1), Color.Aquamarine);
            //SpriteBatch.Draw(Pixel, new Rectangle((int)Center.X, (int)Center.Y, 1, 1), Color.Aquamarine);
            SpriteBatch.End();
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        private void LoadChildren()
        {
            _childrenLoaded = true;

            _childVisibility = new bool[Children.Count];
            _childOpacity = new float[Children.Count];

            RecordChildOpacity();
            RecordChildVisibility();

            // Add the child component to the game with the modified parameters.
            foreach (var child in Children)
            {
                child.Visible = Visibility.Hidden;
                child.AttachedToGrid = true;
            }

            double left = 0;
            double top = 0;
            double right = 0;
            double bottom = 0;

            // Add the child component to the game with the modified parameters.
            foreach (var child in Children)
            {
                // if the child is larger than the grid, resize the object
                // to the grid dimensions.
                if (child.Width > Width)
                {
                    child.Width = Width;
                }

                if (child.Height > Height)
                {
                    child.Height = Height;
                }

                var th = child.Margin;

                var difference = th.Left - th.Right;
                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        var viewPortCenter = Viewport.X + (Viewport.Width / 2);
                        var centerDifference = viewPortCenter - Center.X;
                        left = -centerDifference + difference;
                        break;
                    case HorizontalAlignment.Left:
                        left = TopLeftCorner.X + th.Left;
                        break;
                    case HorizontalAlignment.Right:
                        right = Viewport.Width - TopRightCorner.X + th.Right;
                        break;
                    case HorizontalAlignment.Stretch:
                        child.HorizontalAlignment = HorizontalAlignment.Left;
                        child.Width = (Width - (int)difference) > 0 ? Width - (int)difference : 0;
                        left = TopLeftCorner.X + th.Left;
                        break;
                }

                difference = th.Top - th.Bottom;
                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Center:
                        var viewPortCenter = Viewport.Y + (Viewport.Height / 2);
                        var centerDifference = viewPortCenter - Center.Y;
                        top = -centerDifference + difference;
                        break;
                    case VerticalAlignment.Top:
                        top = TopLeftCorner.Y + th.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        bottom = Viewport.Height - BottomLeftCorner.Y + th.Bottom;
                        break;
                    case VerticalAlignment.Stretch:
                        child.VerticalAlignment = VerticalAlignment.Top;
                        child.Height = (Height - (int)difference) > 0 ? Height - (int)difference : 0;
                        top = TopLeftCorner.Y + th.Top;
                        break;
                }

                Game.Components.Add(child);
                child.Margin = new Thickness(left, top, right, bottom);
            }
        }

        /// <summary>
        /// Stores the visibility of the child.
        /// </summary>
        private void RecordChildVisibility()
        {
            for (var i = 0; i < Children.Count; i++)
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
        /// This toggles the visibility of the child to Hidden when the grid becomes hidden.  However, if
        /// the grid becomes visible again, the child visibilities are reset to what they were prior.
        /// </summary>
        private void UpdateChildVisibility()
        {
            if (Visible == Visibility.Hidden)
            {
                // before making the child hidden, record its lateset visibility state.
                RecordChildVisibility();

                // change the child visibility to hidden, like the grid.
                foreach (var t in Children)
                {
                    t.Visible = Visibility.Hidden;
                }
            }
            else
            {
                // return the visibility of the child to what it was prior to becoming hidden
                // like its parent.
                for (var i = 0; i < _childVisibility.Length; i++)
                {
                    Children[i].Visible = _childVisibility[i] ? Visibility.Visible : Visibility.Hidden;
                }
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
