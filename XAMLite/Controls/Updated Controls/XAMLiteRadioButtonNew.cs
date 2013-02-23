using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{ 
    /// <summary>
    /// Represents a button that can be selected, but not cleared, by a user. 
    /// The IsChecked property of a XAMLiteRadioButton can be set by clicking 
    /// it.
    /// </summary>
    public class XAMLiteRadioButtonNew : XAMLiteGridNew
    {
        private bool _isChecked;

        /// <summary>
        /// Gets or sets whether the ToggleButton is checked. 
        /// </summary>
        public bool IsChecked 
        { 
            get
            {
                return _isChecked;
            } 
            
            set
            {
                _isChecked = value;

                if (Children == null || Children.Count == 0)
                {
                    return;
                }

                AddRemoveCheckMark();

                if (value)
                {
                    DeselectOtherRadioButtons();
                }
            } 
        }

        /// <summary>
        /// Enables/Disables the control.
        /// </summary>
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

        private object _content;

        /// <summary>
        /// Gets or sets the content for the textual portion of the control.
        /// </summary>
        public object Content
        {
            get
            {
                return _content;
            } 
            
            set
            {
                _content = value;

                if (_label != null)
                {
                    _label.Content = value;
                    Width = (int)(_label.MeasureString().X + _checkedBox.Width + Padding.Left + Padding.Right + _label.Margin.Left);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Thickness _originalMargin;

        public override Thickness Margin
        {
            get
            {
                return base.Margin;
            }

            set
            {
                base.Margin = value;

                if (Children.Count > 0)
                {
                    var om = _originalMargin;

                    var l = value.Left - om.Left;
                    var t = value.Top - om.Top;
                    var r = value.Right - om.Right;
                    var b = value.Bottom - om.Bottom;

                    foreach (var child in Children)
                    {
                        child.Margin = child == _label ? new Thickness(l + _uncheckedBox.Width + Padding.Left, t, r, b) : new Thickness(l, t, r, b);
                    }
                }
                else
                {
                    _originalMargin = value;
                }
            }
        }

        /// <summary>
        /// character spacing
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string RadioButtonSourceName { get; set; }

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string RadioButtonSelectedSourceName { get; set; }

        /// <summary>
        /// Gets or sets the name that specifies which XAMLiteRadioButton 
        /// controls are mutually exclusive.
        /// </summary>
        public string GroupName { get; set; }

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

                if (_label != null)
                {
                    _label.FontFamily = value;
                }
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
        /// The color of the content, whether text or some other object.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// The text portion of the radio button
        /// </summary>
        private XAMLiteLabelNew _label;

        /// <summary>
        /// The unchecked radio button.
        /// </summary>
        private XAMLiteImageNew _uncheckedBox;

        /// <summary>
        /// The checked radio button.
        /// </summary>
        private XAMLiteImageNew _checkedBox;

        /// <summary>
        /// List of every radio button in the UI.
        /// </summary>
        internal static List<XAMLiteRadioButtonNew> RadioButtonList;

        /// <summary>
        /// Initializes a new instance of the XAMLiteRadioButton class.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRadioButtonNew(Game game)
            : base(game)
        {
            Content = "RadioButton";
            IsChecked = false;
            RadioButtonSourceName = "Icons/RadioButton";
            RadioButtonSelectedSourceName = "Icons/RadioButtonSelected";
            Height = 16;
            Padding = new Thickness(5, 0, 0, 0);
            if (RadioButtonList == null)
            {
                RadioButtonList = new List<XAMLiteRadioButtonNew>();
            }
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            var checkBoxAsset = Game.Content.Load<Texture2D>(RadioButtonSourceName);

            _label = new XAMLiteLabelNew(Game)
                {
                    Content = Content,
                    FontFamily = FontFamily,
                    Foreground = Foreground,
                    Spacing = Spacing,
                    Margin = new Thickness(checkBoxAsset.Width + Padding.Left, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = Padding,
                    DrawOrder = DrawOrder
                };
            Game.Components.Add(_label);

            // determine width and height
            Width = (int)(_label.MeasureString().X + checkBoxAsset.Width + Padding.Left + Padding.Right + _label.Margin.Left);
            Height = (int)(_label.MeasureString().Y + Padding.Top + Padding.Bottom);

            Game.Components.Remove(_label);
            Children.Add(_label);

            _uncheckedBox = new XAMLiteImageNew(Game, checkBoxAsset)
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Visibility = !IsChecked ? Visibility.Visible : Visibility.Hidden
                };
            Children.Add(_uncheckedBox);

            _checkedBox = new XAMLiteImageNew(Game)
            {
                SourceName = RadioButtonSelectedSourceName,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Visibility = IsChecked ? Visibility.Visible : Visibility.Hidden
            };
            Children.Add(_checkedBox);

            RadioButtonList.Add(this);
        }

        /// <summary>
        /// Initializes the event hook for MouseUp.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseUp += OnMouseUp;
        }

        /// <summary>
        /// Toggles the state of the radio button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            IsChecked = true;
        }

        /// <summary>
        /// Visually changes the art assets based on whether IsChecked = true.
        /// </summary>
        internal void AddRemoveCheckMark()
        {
            if (Visibility == Visibility.Hidden)
            {
                return;
            }

            _checkedBox.Visibility = IsChecked ? Visibility.Visible : Visibility.Hidden;
            _uncheckedBox.Visibility = !IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Deselects other radio buttons with the same group.
        /// </summary>
        private void DeselectOtherRadioButtons()
        {
            foreach (var radioButton in RadioButtonList)
            {
                if (radioButton.IsEnabled)
                {
                    if (this != radioButton && radioButton.GroupName == GroupName && radioButton.IsChecked)
                    {
                        radioButton.IsChecked = false;
                    }
                }
            }
        }

        /// <summary>
        /// Overrides the typical characteristics of this method.
        /// </summary>
        protected override void UpdateChildVisibility()
        {
            if (Visibility == Visibility.Hidden)
            {
                base.UpdateChildVisibility();
            }
            else
            {
                AddRemoveCheckMark();
                _label.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Disposes of the XAMLite objects that make up the radio button 
        /// class.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (RadioButtonList != null)
            {
                RadioButtonList.Clear();
                RadioButtonList = null;
            }

            foreach (var child in Children)
            {
                child.Dispose();
            }
        }
    }
}
