using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// Button class with rollover and mouse down textures.
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteButton class until this class replaces it.
    /// </summary>
    public class XAMLiteButtonNew : XAMLiteGridNew
    {
        /// <summary>
        /// Object contained in the control, which might include
        /// string, date/time, etc.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Character spacing.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
                FontFamilyChanged = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// True when the font family has changed.
        /// </summary>
        protected bool FontFamilyChanged;

        /// <summary>
        /// The content color.
        /// </summary>
        protected Color ForegroundColor
        {
            get
            {
                var solidBrush = (SolidColorBrush)Foreground;
                var color = solidBrush.Color;
                return new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// The color of the content, whether text or some other object.
        /// </summary>
        public virtual Brush Foreground { get; set; }

        /// <summary>
        /// The 2-D image for the button that is not hovered over nor being
        /// clicked. Used specifically for measuring the grid size before
        /// being passed on to the XAMLiteImage class.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// This is the image file path, minus the file extension for the basic
        /// texture.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension for the Rollover image.
        /// </summary>
        public string RolloverSourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension for the Clicked Button image.
        /// </summary>
        public string ClickSourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension for the basic
        /// default edge texture.
        /// </summary>
        private string _defaultSourceNameEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the 
        /// default Rollover edge texture.
        /// </summary>
        private string _defaultRolloverSourceNameEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the
        /// default clicked edge texture.
        /// </summary>
        private string _defaultClickedSourceNameEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the basic
        /// default top edge texture.
        /// </summary>
        private string _defaultSourceNameTopEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the
        /// default top rollover edge texture.
        /// </summary>
        private string _defaultRolloverSourceNameTopEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the
        /// default top click edge texture.
        /// </summary>
        private string _defaultClickSourceNameTopEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the basic
        /// default bottom edge texture.
        /// </summary>
        private string _defaultSourceNameBottomEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the
        /// default bottom rollover edge texture.
        /// </summary>
        private string _defaultRolloverSourceNameBottomEdge;

        /// <summary>
        /// This is the image file path, minus the file extension for the
        /// default bottom click edge texture.
        /// </summary>
        private string _defaultClickSourceNameBottomEdge;

        /// <summary>
        /// Used for measuring the width of the texture and passed as a 
        /// parameter to the XAMLiteImage constructor so that the content is 
        /// not loaded from disk twice.
        /// </summary>
        private Texture2D _defaultEdgeTexture;

        /// <summary>
        /// Represents the normal state of the button and has no rollover 
        /// state. When a source name is not included, this represents the
        /// normal default state.
        /// </summary>
        private XAMLiteImageNew _mainButton;

        /// <summary>
        /// Represents the normal state of the button and contains a rollover 
        /// state.
        /// </summary>
        private XAMLiteImageWithRolloverNew _mainButtonWithRollover;

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteLabelNew label;

        /// <summary>
        /// Represents the clicked state of a button and has no rollover state.
        /// When no source name was included, this represents the clicked 
        /// default state of the button.
        /// </summary>
        private XAMLiteImageNew _clickedButton;

        /// <summary>
        /// True when no directory path for the Source Name was included when 
        /// the button was instantiated.
        /// </summary>
        private bool _isDefaultTextures;

        /// <summary>
        /// Contains default XAMLite images used for the main body and left and
        /// right edges for the button's normal state.
        /// </summary>
        private List<XAMLiteImageNew> _defaultImages;

        /// <summary>
        /// Contains default XAMLite images used for the main body and left and
        /// right edges for the button's rollover state.
        /// </summary>
        private List<XAMLiteImageNew> _defaultRolloverImages;

        /// <summary>
        /// Contains default XAMLite images used for the main body and left and
        /// right edges for the button's clicked state.
        /// </summary>
        private List<XAMLiteImageNew> _defaultClickImages;

        ///// <summary>
        ///// 
        ///// </summary>
        //public override double Opacity
        //{
        //    get
        //    {
        //        return base.Opacity;
        //    }

        //    set
        //    {
        //        if (label != null)
        //        {
        //            label.Opacity = value;
        //        }
        //        else
        //        {
        //            base.Opacity = value;
        //        }
        //    }
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="XAMLite.XAMLiteButton"/> class. 
        /// </summary>
        /// <param name="game">
        /// Reference to the Game.
        /// </param>
        public XAMLiteButtonNew(Game game)
            : base(game)
        {
            FontFamily = new FontFamily("Arial");
            Focusable = true;
            Spacing = 2;
            Foreground = Brushes.Black;
        }

        /// <summary>
        /// Loads the button content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            if (SourceName == null)
            {
                LoadDefaultTexturePaths();
            }

            if (Content != null)
            {
                label = new XAMLiteLabelNew(Game)
                    {
                        Content = Content,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontFamily = FontFamily,
                        Spacing = Spacing,
                        Foreground = Foreground,
                        Padding = Padding,
                        Visibility = Visibility.Hidden,
                        DrawOrder = DrawOrder
                    };

                Game.Components.Add(label);

                Width = label.Width;
                Height = label.Height;

                Game.Components.Remove(label);
            }

            _texture = Game.Content.Load<Texture2D>(SourceName);

            if (Width == 0)
            {
                Width = _texture.Width;
            }

            if (Height == 0)
            {
                Height = _texture.Height;
            }

            // get a reference to the edge texture and get basic edge width
            if (_isDefaultTextures)
            {
                _defaultEdgeTexture = Game.Content.Load<Texture2D>(_defaultSourceNameEdge);
                Width += _defaultEdgeTexture.Width * 2;
            }

            // load the default XAMLiteObjects if none set by the developer.
            if (_isDefaultTextures)
            {
                LoadDefaultContent();
            }
            else
            {
                if (RolloverSourceName != null)
                {
                    _mainButtonWithRollover = new XAMLiteImageWithRolloverNew(Game, _texture)
                    {
                        Name = "Rollover Image",
                        RolloverSourceName = RolloverSourceName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = Width,
                        Height = Height,
                        Background = Background,
                        DrawOrder = DrawOrder
                    };
                    Children.Add(_mainButtonWithRollover);
                }
                else
                {
                    _mainButton = new XAMLiteImageNew(Game, _texture)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = Width,
                        Height = Height,
                        Background = Background,
                        DrawOrder = DrawOrder
                    };
                    Children.Add(_mainButton);
                }

                if (ClickSourceName != null)
                {
                    _clickedButton = new XAMLiteImageNew(Game)
                    {
                        SourceName = ClickSourceName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = Width,
                        Height = Height,
                        Visibility = Visibility.Hidden,
                        Background = Background,
                        DrawOrder = DrawOrder
                    };
                    Children.Add(_clickedButton);
                }
            }

            if (Content != null)
            {
                Children.Add(label);
            }

            // Set to transparent so that only the button textures get 
            // Colorized.
            Background = Brushes.Transparent;

            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        /// <summary>
        /// Load default content for the button when none provided.
        /// </summary>
        private void LoadDefaultContent()
        {
            if (Background == null)
            {
                Background = Brushes.CornflowerBlue;
            }

            _mainButton = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 3, Height - (4), 0))
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = Width - (_defaultEdgeTexture.Width * 2),
                Height = Height - (4),
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultImages.Add(_mainButton);

            var defaultRolloverButton = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 3, Height - (4), 75))
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = _isDefaultTextures ? Width - (_defaultEdgeTexture.Width * 2) : Width,
                Height = Height - (4),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultRolloverImages.Add(defaultRolloverButton);

            _clickedButton = new XAMLiteImageNew(Game, GradientTextureBuilder.CreateGradientTexture(Game, 3, Height - (4), -75))
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = Width - (_defaultEdgeTexture.Width * 2),
                Height = Height - (4),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultClickImages.Add(_clickedButton);

            var defaultLeftEdge = new XAMLiteImageNew(Game, _defaultEdgeTexture)
            {
                IsEdge = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = Height - 4,
                Margin = new Thickness(0, 0, Width - _defaultEdgeTexture.Width, 0),
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultImages.Add(defaultLeftEdge);

            var defaultRolloverLeftEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultRolloverSourceNameEdge,
                IsEdge = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = Height - 4,
                Margin = new Thickness(0, 0, Width - _defaultEdgeTexture.Width, 0),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultRolloverImages.Add(defaultRolloverLeftEdge);

            var defaultClickLeftEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultClickedSourceNameEdge,
                IsEdge = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = Height - 4,
                Margin = new Thickness(0, 0, Width - _defaultEdgeTexture.Width, 0),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultClickImages.Add(defaultClickLeftEdge);

            var defaultRightEdge = new XAMLiteImageNew(Game, _defaultEdgeTexture)
            {
                IsEdge = true,
                RenderTransform = RenderTransform.FlipHorizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Height - 4,
                Margin = new Thickness(Width - _defaultEdgeTexture.Width, 0, 0, 0),
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultImages.Add(defaultRightEdge);

            var defaultRolloverRightEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultRolloverSourceNameEdge,
                IsEdge = true,
                RenderTransform = RenderTransform.FlipHorizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Height - 4,
                Margin = new Thickness(Width - _defaultEdgeTexture.Width, 0, 0, 0),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultRolloverImages.Add(defaultRolloverRightEdge);

            var defaultClickRightEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultClickedSourceNameEdge,
                IsEdge = true,
                RenderTransform = RenderTransform.FlipHorizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Height - 4,
                Margin = new Thickness(Width - _defaultEdgeTexture.Width, 0, 0, 0),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultClickImages.Add(defaultClickRightEdge);

            var defaultTopEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultSourceNameTopEdge,
                IsEdge = true,
                IsTopEdge = true,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - 4,
                Margin = new Thickness(0, 0, 0, Height - 4),
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultImages.Add(defaultTopEdge);

            var defaultRolloverTopEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultRolloverSourceNameTopEdge,
                IsEdge = true,
                IsTopEdge = true,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - 4,
                Margin = new Thickness(0, 0, 0, Height - 4),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultRolloverImages.Add(defaultRolloverTopEdge);

            var defaultClickTopEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultClickSourceNameTopEdge,
                IsEdge = true,
                IsTopEdge = true,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - 4,
                Margin = new Thickness(0, 0, 0, Height - 4),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultClickImages.Add(defaultClickTopEdge);

            var defaultBottomEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultSourceNameBottomEdge,
                IsEdge = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - 4,
                Margin = new Thickness(0, Height - 4, 0, 0),
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultImages.Add(defaultBottomEdge);

            var defaultRolloverBottomEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultRolloverSourceNameBottomEdge,
                IsEdge = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - 4,
                Margin = new Thickness(0, Height - 4, 0, 0),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultRolloverImages.Add(defaultRolloverBottomEdge);

            var defaultClickBottomEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultClickSourceNameBottomEdge,
                IsEdge = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - 4,
                Margin = new Thickness(0, Height - 4, 0, 0),
                Visibility = Visibility.Hidden,
                Background = Background,
                DrawOrder = DrawOrder
            };
            _defaultClickImages.Add(defaultClickBottomEdge);

            foreach (var image in _defaultImages)
            {
                Children.Add(image);
            }

            foreach (var image in _defaultRolloverImages)
            {
                Children.Add(image);
            }

            foreach (var image in _defaultClickImages)
            {
                Children.Add(image);
            }
        }

        /// <summary>
        /// Loads default textures when the basic texture has not been set.
        /// </summary>
        private void LoadDefaultTexturePaths()
        {
            _isDefaultTextures = true;

            _defaultImages = new List<XAMLiteImageNew>();
            _defaultRolloverImages = new List<XAMLiteImageNew>();
            _defaultClickImages = new List<XAMLiteImageNew>();

            SourceName = "Images/ButtonCenterNormal";
            RolloverSourceName = "Images/ButtonCenterOver";
            ClickSourceName = "Images/ButtonCenterDown";
            _defaultSourceNameEdge = "Images/ButtonEdgeNormal";
            _defaultRolloverSourceNameEdge = "Images/ButtonEdgeOver";
            _defaultClickedSourceNameEdge = "Images/ButtonEdgeDown";
            _defaultSourceNameTopEdge = "Images/ButtonTopNormal";
            _defaultSourceNameBottomEdge = "Images/ButtonBottomNormal";
            _defaultRolloverSourceNameTopEdge = "Images/ButtonTopOver";
            _defaultRolloverSourceNameBottomEdge = "Images/ButtonBottomOver";
            _defaultClickSourceNameTopEdge = "Images/ButtonTopDown";
            _defaultClickSourceNameBottomEdge = "Images/ButtonBottomDown";
        }

        public override void Update(GameTime gameTime)
        {
            // adjust the children in the button grid.
            if (PositionChanged && !_isDefaultTextures)
            {
                foreach (var child in Children)
                {
                    child.Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the child opacity.
        /// </summary>
        protected override void UpdateChildOpacity()
        {
            CheckForNewChildren();

            foreach (var child in Children)
            {
                child.Opacity = Opacity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (_isDefaultTextures)
            {
                foreach (var image in _defaultRolloverImages)
                {
                    image.Visibility = Visibility.Hidden;
                }

                foreach (var image in _defaultClickImages)
                {
                    image.Visibility = Visibility.Hidden;
                }

                foreach (var image in _defaultImages)
                {
                    image.Visibility = Visibility.Visible;
                }
            }
            else
            {
                _mainButtonWithRollover.Visibility = Visibility.Visible;
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (_isDefaultTextures)
            {
                if (!MousePressed)
                {
                    foreach (var image in _defaultClickImages)
                    {
                        image.Visibility = Visibility.Hidden;
                    }
                }

                foreach (var image in _defaultImages)
                {
                    image.Visibility = Visibility.Hidden;
                }

                foreach (var image in _defaultRolloverImages)
                {
                    image.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Updates the visibility between the mouse down and mouse hover images.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (RolloverSourceName != null && ClickSourceName != null)
            {
                _clickedButton.Visibility = Visibility.Hidden;

                if (_isDefaultTextures)
                {
                    foreach (var image in _defaultClickImages)
                    {
                        image.Visibility = Visibility.Hidden;
                    }

                    foreach (var image in _defaultImages)
                    {
                        image.Visibility = Visibility.Hidden;
                    }

                    foreach (var image in _defaultRolloverImages)
                    {
                        image.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    _mainButtonWithRollover.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Updates the visibility between the mouse hover and mouse down images.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (RolloverSourceName != null && ClickSourceName != null)
            {
                _clickedButton.Visibility = Visibility.Visible;

                if (_isDefaultTextures)
                {
                    foreach (var image in _defaultImages)
                    {
                        image.Visibility = Visibility.Hidden;
                    }

                    foreach (var image in _defaultRolloverImages)
                    {
                        image.Visibility = Visibility.Hidden;
                    }

                    foreach (var image in _defaultClickImages)
                    {
                        image.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    _mainButtonWithRollover.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Dispose of the XAMLiteLabel that is used for the Content portion of
        /// the control.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            foreach (var child in Children)
            {
                child.Dispose();
            }
        }
    }
}
