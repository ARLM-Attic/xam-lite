using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// Button class with rollover and mouse down textures.
    /// 
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteButton class until this class replaces it.
    /// </summary>
    public class XAMLiteButtonNew : XAMLiteBaseContent
    {
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
        /// Used for measuring the width of the texture and passed as a 
        /// parameter to the XAMLiteImage constructor so that the content is 
        /// not loaded from disk twice.
        /// </summary>
        private Texture2D _edgeTexture;

        /// <summary>
        /// Contains all of the assets that represent the XAMLiteButton.
        /// </summary>
        private XAMLiteGridNew _grid;

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

            _texture = Game.Content.Load<Texture2D>(SourceName);

            if (Content != null)
            {
                UpdateFontMetrics();
            }
            
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
                _edgeTexture = Game.Content.Load<Texture2D>(_defaultSourceNameEdge);
                Width += _edgeTexture.Width * 2;
            }

            // then load the grid.
            _grid = new XAMLiteGridNew(Game)
                {
                    Parent = this,
                    Width = Width,
                    Height = Height,
                    HorizontalAlignment = HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment,
                    Margin = Margin
                };
            Game.Components.Add(_grid);

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
                        RolloverSourceName = RolloverSourceName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = Width,
                        Height = Height,
                        Background = Background
                    };
                    _grid.Children.Add(_mainButtonWithRollover);
                }
                else
                {
                    _mainButton = new XAMLiteImageNew(Game, _texture)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = Width,
                        Height = Height,
                        Background = Background
                    };
                    _grid.Children.Add(_mainButton);
                }

                if (ClickSourceName != null)
                {
                    _clickedButton = new XAMLiteImageNew(Game)
                    {
                        SourceName = ClickSourceName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = Width,
                        Height = Height,
                        Visible = Visibility.Hidden,
                        Background = Background
                    };
                    _grid.Children.Add(_clickedButton);
                }
            }
            
            if (Content != null)
            {
                var label = new XAMLiteLabelNew(Game)
                    {
                        Content = Content,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontFamily = FontFamily,
                        Spacing = Spacing,
                        Foreground = Foreground,
                        Padding = Padding
                    };
                _grid.Children.Add(label);
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
            _mainButton = new XAMLiteImageNew(Game, CreateGradientTexture(Height))
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - (_edgeTexture.Width * 2),
                Height = Height,
                Background = Background
            };
            _defaultImages.Add(_mainButton);

            var defaultRolloverButton = new XAMLiteImageNew(Game)
            {
                SourceName = RolloverSourceName,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = _isDefaultTextures ? Width - (_edgeTexture.Width * 2) : Width,
                Height = Height,
                Visible = Visibility.Hidden,
                Background = Background
            };
            _defaultRolloverImages.Add(defaultRolloverButton);

            _clickedButton = new XAMLiteImageNew(Game)
            {
                SourceName = ClickSourceName,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = Width - (_edgeTexture.Width * 2),
                Height = Height,
                Visible = Visibility.Hidden,
                Background = Background
            };
            _defaultClickImages.Add(_clickedButton);

            var defaultLeftEdge = new XAMLiteImageNew(Game, _edgeTexture)
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = Height,
                Margin = new Thickness(0, 0, Width - _edgeTexture.Width, 0),
                Background = Background
            };
            _defaultImages.Add(defaultLeftEdge);

            var defaultRolloverLeftEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultRolloverSourceNameEdge,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = Height,
                Margin = new Thickness(0, 0, Width - _edgeTexture.Width, 0),
                Visible = Visibility.Hidden,
                Background = Background
            };
            _defaultRolloverImages.Add(defaultRolloverLeftEdge);

            var defaultClickLeftEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultClickedSourceNameEdge,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = Height,
                Margin = new Thickness(0, 0, Width - _edgeTexture.Width, 0),
                Visible = Visibility.Hidden,
                Background = Background
            };
            _defaultClickImages.Add(defaultClickLeftEdge);

            var defaultRightEdge = new XAMLiteImageNew(Game, _edgeTexture)
            {
                RenderTransform = new ScaleTransform(-1, 0, 0.5, 0.5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Height,
                Margin = new Thickness(Width - _edgeTexture.Width, 0, 0, 0),
                Background = Background
            };
            _defaultImages.Add(defaultRightEdge);

            var defaultRolloverRightEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultRolloverSourceNameEdge,
                RenderTransform = new ScaleTransform(-1, 0, 0.5, 0.5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Height,
                Margin = new Thickness(Width - _edgeTexture.Width, 0, 0, 0),
                Visible = Visibility.Hidden,
                Background = Background
            };
            _defaultRolloverImages.Add(defaultRolloverRightEdge);

            var defaultClickRightEdge = new XAMLiteImageNew(Game)
            {
                SourceName = _defaultClickedSourceNameEdge,
                RenderTransform = new ScaleTransform(-1, 0, 0.5, 0.5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = Height,
                Margin = new Thickness(Width - _edgeTexture.Width, 0, 0, 0),
                Visible = Visibility.Hidden,
                Background = Background
            };
            _defaultClickImages.Add(defaultClickRightEdge);

            foreach (var image in _defaultImages)
            {
                _grid.Children.Add(image);
            }

            foreach (var image in _defaultRolloverImages)
            {
                _grid.Children.Add(image);
            }

            foreach (var image in _defaultClickImages)
            {
                _grid.Children.Add(image);
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        private Texture2D CreateGradientTexture(int height)
        {
            int gradientThickness = 2;
            int gradientColor = 255;
            Texture2D t = new Texture2D(Game.GraphicsDevice, Width, Height);
            Color[] bgc = new Color[Width * Height];

            for (int i = 0; i < bgc.Length; i++)
            {
                gradientColor = ((i * 20) / (Height * gradientThickness));
                bgc[i] = new Color(gradientColor, gradientColor, gradientColor, gradientColor);
            }

            t.SetData(bgc);

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_isDefaultTextures)
            {
                foreach (var image in _defaultRolloverImages)
                {
                    image.Visible = Visibility.Hidden;
                }

                foreach (var image in _defaultClickImages)
                {
                    image.Visible = Visibility.Hidden;
                }

                foreach (var image in _defaultImages)
                {
                    image.Visible = Visibility.Visible;
                }
            }
            else
            {
                _mainButtonWithRollover.Visible = Visibility.Visible;
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_isDefaultTextures)
            {
                if (!MousePressed)
                {
                    foreach (var image in _defaultClickImages)
                    {
                        image.Visible = Visibility.Hidden;
                    }
                }

                foreach (var image in _defaultImages)
                {
                    image.Visible = Visibility.Hidden;
                }

                foreach (var image in _defaultRolloverImages)
                {
                    image.Visible = Visibility.Visible;
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
            if (RolloverSourceName != null && ClickSourceName != null)
            {
                _clickedButton.Visible = Visibility.Hidden;

                if (_isDefaultTextures)
                {
                    foreach (var image in _defaultClickImages)
                    {
                        image.Visible = Visibility.Hidden;
                    }

                    foreach (var image in _defaultImages)
                    {
                        image.Visible = Visibility.Hidden;
                    }

                    foreach (var image in _defaultRolloverImages)
                    {
                        image.Visible = Visibility.Visible;
                    }
                }
                else
                {
                    _mainButtonWithRollover.Visible = Visibility.Visible;
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
            if (RolloverSourceName != null && ClickSourceName != null)
            {
                _clickedButton.Visible = Visibility.Visible;

                if (_isDefaultTextures)
                {
                    foreach (var image in _defaultImages)
                    {
                        image.Visible = Visibility.Hidden;
                    }

                    foreach (var image in _defaultRolloverImages)
                    {
                        image.Visible = Visibility.Hidden;
                    } 
                    
                    foreach (var image in _defaultClickImages)
                    {
                        image.Visible = Visibility.Visible;
                    }
                }
                else
                {
                    _mainButtonWithRollover.Visible = Visibility.Hidden;
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

            foreach (var child in _grid.Children)
            {
                child.Dispose();
            }
        }
    }
}
