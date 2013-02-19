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
        /// <summary>
        /// Gets or sets whether the ToggleButton is checked. 
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets the content for the textual portion of the control.
        /// </summary>
        public object Content { get; set; }

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
        /// The unchecked radio button.
        /// </summary>
        private XAMLiteImageNew _uncheckedBox;

        /// <summary>
        /// The checked radio button.
        /// </summary>
        private XAMLiteImageNew _checkedBox;

        /// <summary>
        /// Initializes a new instance of the XAMLiteRadioButton class.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRadioButtonNew(Game game)
            : base(game)
        {
            Content = "CheckBox";
            IsChecked = false;
            RadioButtonSourceName = "Icons/RadioButton";
            RadioButtonSelectedSourceName = "Icons/RadioButtonSelected";
            Height = 16;
            Padding = new Thickness(5, 0, 0, 0);
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            var checkBoxAsset = Game.Content.Load<Texture2D>(RadioButtonSourceName);

            var label = new XAMLiteLabelNew(Game)
                {
                    Content = Content,
                    FontFamily = FontFamily,
                    Foreground = Foreground,
                    Spacing = Spacing,
                    Margin = new Thickness(checkBoxAsset.Width + Padding.Left, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    DrawOrder = DrawOrder
                };
            Game.Components.Add(label);

            // determine width and height
            Width = (int)(label.MeasureString().X + checkBoxAsset.Width + Padding.Left + Padding.Right + label.Margin.Left);
            Height = (int)(label.MeasureString().Y + Padding.Top + Padding.Bottom);

            Game.Components.Remove(label);
            Children.Add(label);

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
            IsChecked = !IsChecked;

            SetRadioButtonIsChecked();
        }

        /// <summary>
        /// Visual changes the art assets based on whether IsChecked = true.
        /// </summary>
        private void SetRadioButtonIsChecked()
        {
            _checkedBox.Visibility = IsChecked ? Visibility.Visible : Visibility.Hidden;
            _uncheckedBox.Visibility = !IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Disposes of the XAMLite objects that make up the radio button 
        /// class.
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
