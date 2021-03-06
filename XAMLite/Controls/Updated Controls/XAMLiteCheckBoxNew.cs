﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteCheckBox class until this class replaces it.
    /// </summary>
    public class XAMLiteCheckBoxNew : XAMLiteGridNew
    {
        /// <summary>
        /// Gets or sets the content for the textual portion of the control.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// The color of the content, whether text or some other object.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets whether the ToggleButton is checked. 
        /// </summary>
        public bool IsChecked { get; set; }

        public override bool IsEnabled
        {
            get
            {
                return base.IsEnabled;
            }

            set
            {
                base.IsEnabled = value;

                if (!value)
                {
                    Opacity = 0.65;
                }
                else
                {
                    Opacity = 1;
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
        /// character spacing
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        protected FontFamily _fontFamily;

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
        /// True when the font family has changed.
        /// </summary>
        protected bool FontFamilyChanged;

        /// <summary>
        /// The padding that surrounds the text within the control.  Note, in 
        /// WPF, only the top and left can be set.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteCheckBoxNew(Game game)
            : base(game)
        {
            Content = "CheckBox";
            IsChecked = false;
            SourceName = "Icons/RadioButton";
            CheckedSourceName = "Icons/RadioButtonSelected";
            FontFamily = new FontFamily("Verdana12");
            Height = 16;
            Spacing = 2;
            Padding = new Thickness(5, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            Debug.Assert((SourceName != null), "Must set CheckBoxSourceName property. This is the image file path, minus the file extension.");
            _texture = Game.Content.Load<Texture2D>(SourceName);

            Debug.Assert((CheckedSourceName != null), "Must set CheckBoxSelectedSourceName property. This is the image file path, minus the file extension.");

            _label = new XAMLiteLabelNew(Game)
            {
                Content = Content,
                FontFamily = FontFamily,
                Foreground = Foreground,
                Spacing = Spacing,
                Margin = new Thickness(_texture.Width + Padding.Left, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                DrawOrder = DrawOrder
            };
            Game.Components.Add(_label);

            // determine width and height
            Width = (int)(_label.MeasureString().X + _texture.Width + Padding.Left + Padding.Right + _label.Margin.Left);
            Height = (int)(_label.MeasureString().Y + Padding.Top + Padding.Bottom);

            Game.Components.Remove(_label);
            Children.Add(_label);

            _uncheckedButton = new XAMLiteImageNew(Game)
            {
                SourceName = SourceName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = !IsChecked ? Visibility.Visible : Visibility.Hidden
            };
            Children.Add(_uncheckedButton);

            if (HoverSourceName != null)
            {
                _uncheckedHoverButton = new XAMLiteImageNew(Game)
                    {
                        SourceName = HoverSourceName,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Visibility = Visibility.Hidden
                    };
                Children.Add(_uncheckedHoverButton);
            }

            _checkedButton = new XAMLiteImageNew(Game)
            {
                SourceName = CheckedSourceName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = IsChecked ? Visibility.Visible : Visibility.Hidden
            };
            Children.Add(_checkedButton);

            if (HoverCheckedSourceName != null)
            {
                _checkedHoverButton = new XAMLiteImageNew(Game)
                    {
                        SourceName = HoverCheckedSourceName,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Visibility = Visibility.Hidden
                    };
                Children.Add(_checkedHoverButton);
            }
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
                    _checkedHoverButton.Visibility = Visibility.Visible;
                    _checkedButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    _uncheckedHoverButton.Visibility = Visibility.Visible;
                    _uncheckedButton.Visibility = Visibility.Hidden;
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
                    _checkedHoverButton.Visibility = Visibility.Hidden;
                    _checkedButton.Visibility = Visibility.Visible;
                }
                else
                {
                    _uncheckedHoverButton.Visibility = Visibility.Hidden;
                    _uncheckedButton.Visibility = Visibility.Visible;
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
                _uncheckedButton.Visibility = Visibility.Hidden;

                if (_checkedHoverButton != null && _uncheckedHoverButton != null)
                {
                    _checkedHoverButton.Visibility = Visibility.Visible;
                    _uncheckedHoverButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    _checkedButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                _checkedButton.Visibility = Visibility.Hidden;

                if (_checkedHoverButton != null && _uncheckedHoverButton != null)
                {
                    _uncheckedHoverButton.Visibility = Visibility.Visible;
                    _checkedHoverButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    _uncheckedButton.Visibility = Visibility.Visible;
                }
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

                ToggleTextures();
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
