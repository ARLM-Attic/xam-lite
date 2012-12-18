using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework;

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
        public Children Children { get; set; }

        /// <summary>
        /// Used to determine whether the child has been loaded into the grid.
        /// </summary>
        //private bool _childrenLoaded;

        /// <summary>
        /// Holds a record of the child's natural visibility prior to being affected by the grid.
        /// </summary>
        private List<bool> _childVisibility;

        /// <summary>
        /// 
        /// </summary>
        private List<bool> _currentChildVisibility;

        /// <summary>
        /// Holds a record of the child's natural opacity and is used to modify its opacity according to
        /// the opacity of the grid (grid opacity * child opacity).
        /// </summary>
        private List<float> _childOpacity;

        /// <summary>
        /// 
        /// </summary>
        private int _gridCount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteGridNew(Game game)
            : base(game)
        {
            Children = new Children(this);
            BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Load the content of the Grid.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // set the internal Window to the Panel.
            Window = Panel;

            // If the grid is a part of a XAMLite component that is attached to 
            // a grid then it, itself, is attached to this same grid and its 
            // position should be set to the same as its parent's.
            if (IsAttachedToGrid)
            {
                ModifyGridPosition();
            }
        }

        /// <summary>
        /// Updates the grid.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_gridCount != Children.Count)
            {
                LoadChildren();
            }
            //if (!_childrenLoaded)
            //{
            //    LoadChildren();
            //}

            // Update Visibility of Children according to the grid's visibility.
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

            //for (var i = 0; i < Children.Count; i++)
            //{
            //    if (Visibility == Visibility.Visible)
            //    {
            //        if (_childVisibility[i] && Children[i].Visibility == Visibility.Hidden)
            //        {
            //            _childVisibility[i] = false;
            //        }
            //        else
            //        {
            //            _childVisibility[i] = true;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            //SpriteBatch.Draw(Pixel, Panel, BackgroundColor);
            SpriteBatch.End();
        }

        /// <summary>
        /// Loads the children once the grid has been set up.
        /// </summary>
        private void LoadChildren()
        {
            SaveChildOpacity();
            SaveInitialChildVisibility();
            HideChildren();

            // Add the child component to the game with the modified parameters.
            for (var i = _gridCount; i < Children.Count; i++)
            {
                AddChild(Children[i]);
            }

            _gridCount = Children.Count;
            ModifyChildPositionAndWidth();
        }

        /// <summary>
        /// Sets some parameters and adds the child to the Game.
        /// </summary>
        /// <param name="child"></param>
        private void AddChild(XAMLiteBaseControl child)
        {
            try
            {
                if (!Game.Components.Contains(child))
                {
                    Game.Components.Add(child);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Modifies a grid's position when it is a component of a complex 
        /// XAMLite object that is embedded in another grid.
        /// </summary>
        private void ModifyGridPosition()
        {
            var parentWindow = Parent.Window;

            var x = (float)parentWindow.X;
            var y = (float)parentWindow.Y;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    x += ((float)(parentWindow.Width - Width) / 2) + (float)Margin.Left - (float)Margin.Right;
                    break;

                case HorizontalAlignment.Left:
                    x += Window.X;
                    break;

                case HorizontalAlignment.Right:
                    x += parentWindow.Width - Width - (float)Margin.Right;
                    break;

                case HorizontalAlignment.Stretch:
                    x = Window.X + (int)Margin.Left;
                    Width = parentWindow.Width - (int)(Margin.Left + Margin.Right);
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    y += parentWindow.Height - Height - (float)Margin.Bottom;
                    break;

                case VerticalAlignment.Center:
                    y += ((float)(parentWindow.Height - Height) / 2) + (float)Margin.Top - (float)Margin.Bottom;
                    break;

                case VerticalAlignment.Stretch:
                    y += Window.Y + (int)Margin.Top;
                    Height = parentWindow.Height - (int)(Margin.Top + Margin.Bottom);
                    break;

                case VerticalAlignment.Top:
                    y += Window.Y;
                    break;
            }

            Window = new Rectangle((int)x, (int)y, Width, Height);
        }

        /// <summary>
        /// Adjust the children position and width according to the grid location.
        /// </summary>
        private void ModifyChildPositionAndWidth()
        {
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

                child.Window = Panel;

                child.PositionChanged = true;
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
                _currentChildVisibility = new List<bool>();
            }

            for (var i = _gridCount; i < Children.Count; i++)
            {
                _childVisibility.Add(Children[i].Visibility == Visibility.Visible);
            }
        }

        /// <summary>
        /// At start, hides the children until the grid is fully set up.
        /// </summary>
        private void HideChildren()
        {
            foreach (var child in Children)
            {
                child.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// This toggles the visibility of the child to Hidden when the grid becomes hidden.  However, if
        /// the grid becomes visible again, the child visibilities are reset to what they were prior.
        /// </summary>
        private void UpdateChildVisibility()
        {
            if (Visibility == Visibility.Hidden)
            {
                // change the child visibility to hidden, like the grid.
                HideChildren();
            }
            else
            {
                // this means a new child was added later.
                CheckForNewChildren();

                // return the visibility of the child to what it was prior to becoming hidden
                // like its parent.
                for (var i = 0; i < _childVisibility.Count; i++)
                {
                    Children[i].Visibility = _childVisibility[i] ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Records the current opacity of the child so that it can be used to modify its
        /// opacity according to the grid's opacity.
        /// </summary>
        private void SaveChildOpacity()
        {
            if (_childOpacity == null)
            {
                _childOpacity = new List<float>(); 
            }

            for (var i = _gridCount; i < Children.Count; i++)
            {
                _childOpacity.Add((float)Children[i].Opacity);
            }
        }

        /// <summary>
        /// Modifies the opacity of the child according to the opacity of the grid and the opacity
        /// of the child.
        /// </summary>
        private void UpdateChildOpacity()
        {
            CheckForNewChildren();

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Opacity = Opacity * _childOpacity[i];
                
            }
        }

        private void CheckForNewChildren()
        {
            if (_gridCount == Children.Count && Children.Count == _childOpacity.Count && _childOpacity.Count == _childVisibility.Count && _childVisibility.Count == Children.Count)
            {
                return;
            }

            // this means that a new child was added later.
            if (_childOpacity.Count != Children.Count)
            {
                Console.WriteLine("Opacity count not the same");
                for (int i = _childOpacity.Count; i < Children.Count; i++)
                {
                    _childOpacity.Add((float)Children[i].Opacity);
                }

                Console.WriteLine("Grid Count: " + _gridCount);
                Console.WriteLine("Child count: " + _childOpacity.Count);
                Console.WriteLine("Children count: " + Children.Count);
            }

            if (_childVisibility.Count != Children.Count)
            {
                for (var i = _childVisibility.Count; i < Children.Count; i++)
                {
                    _childVisibility.Add(Children[i].Visibility == Visibility.Visible);
                    AddChild(Children[i]);
                }
            }
        }

        /// <summary>
        /// This checks to see whether a child's opacity was changed when the grid's opacity
        /// was not.  If it was, a method to modify the specific control according to the
        /// grid's opacity limits is called.
        /// </summary>
        private void CheckChildrenOpacity()
        {
            CheckForNewChildren();

            for (var i = 0; i < Children.Count; i++)
            {
                var previousChildOpacity = _childOpacity[i];

                if (Math.Abs((float)Children[i].Opacity - previousChildOpacity) > 0.01)
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
            Console.WriteLine("Opacity changed.");
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

        /// <summary>
        /// When a child is being removed from the list of Children, the
        /// child Visibility and child Opacity counts are updated.
        /// </summary>
        /// <param name="index"></param>
        internal void DecreaseChildrenLists(int index)
        {
            _childVisibility.RemoveAt(index);
            _childOpacity.RemoveAt(index);
        }
    }
}
