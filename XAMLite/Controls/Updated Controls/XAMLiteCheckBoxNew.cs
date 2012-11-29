using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    using System.Windows.Input;

    /// <summary>
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteCheckBox class until this class replaces it.
    /// </summary>
    public class XAMLiteCheckBoxNew : XAMLiteBaseContent
    {
        private bool _isChecked;

        /// <summary>
        /// True when the checkbox is checked.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            } 

            set
            {
                if (IsEnabled)
                {
                    _isChecked = value;

                    ToggleTextures();
                }
            }
        }

        /// <summary>
        /// Texture asset for the checkbox when it is unchecked.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// This is the image file path, minus the file extension for an 
        /// unchecked checkbox.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension for when the
        /// checkbox is hovered over but is unchecked.
        /// </summary>
        public string HoverSourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension for when the 
        /// checkbox is checked and not hovered over.
        /// </summary>
        public string CheckedSourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension for when the
        /// checkbox is hovered over and checked.
        /// </summary>
        public string HoverCheckedSourceName { get; set; }

        /// <summary>
        /// Contains all of the components of the check box.
        /// </summary>
        private XAMLiteGridNew _grid;

        /// <summary>
        /// The unchecked button.
        /// </summary>
        private XAMLiteImageNew _uncheckedButton;

        /// <summary>
        /// The unchecked hover button.
        /// </summary>
        private XAMLiteImageNew _uncheckedHoverButton;

        /// <summary>
        /// The checked button.
        /// </summary>
        private XAMLiteImageNew _checkedButton;

        /// <summary>
        /// The checked hover button.
        /// </summary>
        private XAMLiteImageNew _checkedHoverButton;

        /// <summary>
        /// The content portion of the CheckBox.
        /// </summary>
        private XAMLiteLabelNew _label;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteCheckBoxNew(Game game)
            : base(game)
        {
            SourceName = "Icons/RadioButton";
            CheckedSourceName = "Icons/RadioButtonSelected";
            FontFamily = new FontFamily("Arial");
            Spacing = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseDown += OnMouseDown;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            
            Debug.Assert((SourceName != null), "Must set CheckBoxSourceName property. This is the image file path, minus the file extension.");
            _texture = Game.Content.Load<Texture2D>(SourceName);

            UpdateFontMetrics();
            
            Debug.Assert((CheckedSourceName != null), "Must set CheckBoxSelectedSourceName property. This is the image file path, minus the file extension.");
            
            _grid = new XAMLiteGridNew(Game)
            {
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment,
                Width = Width,
                Height = Height,
                Margin = Margin
            };    
            Game.Components.Add(_grid);
            
            _uncheckedButton = new XAMLiteImageNew(Game)
            {
                SourceName = SourceName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Visible = !IsChecked ? Visibility.Visible : Visibility.Hidden
            };
            _grid.Children.Add(_uncheckedButton);

            if (HoverSourceName != null)
            {
                _uncheckedHoverButton = new XAMLiteImageNew(Game)
                    {
                        SourceName = HoverSourceName,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Visible = Visibility.Hidden
                    };
                _grid.Children.Add(_uncheckedHoverButton);
            }

            _checkedButton = new XAMLiteImageNew(Game)
            {
                SourceName = CheckedSourceName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Visible = IsChecked ? Visibility.Visible : Visibility.Hidden
            };    
            _grid.Children.Add(_checkedButton);

            if (HoverCheckedSourceName != null)
            {
                _checkedHoverButton = new XAMLiteImageNew(Game)
                    {
                        SourceName = HoverCheckedSourceName,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Visible = Visibility.Hidden
                    };
                _grid.Children.Add(_checkedHoverButton);
            }

            _label = new XAMLiteLabelNew(Game)
            {
                Content = Content,
                Foreground = Foreground,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                AttachedToGrid = true,
                Padding = new Thickness(5, 0, 0, 0),
                FontFamily = FontFamily,
                Spacing = Spacing,
                Margin = new Thickness(_texture.Width, 0, 0, 0)
            };
            
            _grid.Children.Add(_label);
        }

        /// <summary>
        /// Recalculates the width and height of the control.  If the width or height
        /// as set by the user is greater than those of the assets within the control,
        /// the user defined settings will be maintained.
        /// </summary>
        /// <param name="content"></param>
        protected override void RecalculateWidthAndHeight(object content)
        {
            var w = _texture.Width + (int)Padding.Left + (int)Padding.Right + (int)SpriteFont.MeasureString(Content.ToString()).X;
            Width = Width > w ? Width : w;
            var h = (int)SpriteFont.MeasureString(Content.ToString()).Y + (int)Padding.Top + (int)Padding.Bottom;
            Height = Height > h && Height > _texture.Height ? Height : _texture.Height > h ? _texture.Height : h;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (HoverSourceName == null || HoverCheckedSourceName == null)
            {
                return;
            }

            if (IsEnabled)
            {
                if (IsChecked)
                {
                    _checkedHoverButton.Visible = Visibility.Visible;
                    _checkedButton.Visible = Visibility.Hidden;
                }
                else
                {
                    _uncheckedHoverButton.Visible = Visibility.Visible;
                    _uncheckedButton.Visible = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (HoverSourceName == null || HoverCheckedSourceName == null)
            {
                return;
            }

            if (IsEnabled)
            {
                if (IsChecked)
                {
                    _checkedHoverButton.Visible = Visibility.Hidden;
                    _checkedButton.Visible = Visibility.Visible;
                }
                else
                {
                    _uncheckedHoverButton.Visible = Visibility.Hidden;
                    _uncheckedButton.Visible = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ToggleTextures()
        {
            if (IsChecked)
            {
                _checkedHoverButton.Visible = Visibility.Visible;
                _uncheckedButton.Visible = Visibility.Hidden;
                _uncheckedHoverButton.Visible = Visibility.Hidden;
            }
            else
            {
                _uncheckedHoverButton.Visible = Visibility.Visible;
                _checkedButton.Visible = Visibility.Hidden;
                _checkedHoverButton.Visible = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEnabled)
            {
                IsChecked = !IsChecked;
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
