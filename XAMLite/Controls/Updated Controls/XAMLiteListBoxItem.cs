using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// Represents a selectable item in a ListBox.
    /// </summary>
    public class XAMLiteListBoxItem : XAMLiteGridNew
    {
        /// <summary>
        /// True when the ListBoxItem is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// True when the ListBoxItem is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;

                if (!_isSelected)
                {
                    BackgroundPanel.Visibility = Visibility.Hidden;
                }
            }
        }

        public override Brush Background
        {
            get
            {
                return BackgroundPanel.Background;
            }

            set
            {
                if (BackgroundPanel != null)
                BackgroundPanel.Background = value;
            }
        }

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
        /// 
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// The Border color.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// The thickness of the border.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteLabelNew _listBoxContent;

        /// <summary>
        /// The background of the control that changes colors when selected.
        /// </summary>
        protected internal XAMLiteRectangleNew BackgroundPanel;

        /// <summary>
        /// Reference to the specific class that the parent represents.
        /// </summary>
        private XAMLiteListBox _parent;

        private Brush _foreground;

        /// <summary>
        /// Sets the Text color of the ListBoxItem.
        /// </summary>
        public Brush Foreground
        {
            get
            {
                return _foreground;
            }

            set
            {
                _foreground = value;

                if (_listBoxContent == null)
                {
                    return;
                }

                _listBoxContent.Foreground = value;
            }
        }

        /// <summary>
        /// Although not in WPF, this seems essential to override the default
        /// colors in WPF for highlighting on mouse over or when selected.  
        /// If this is not explicitly set, it will receive the brush color
        /// as specified by the ListBox that contains it.
        /// </summary>
        public Brush SelectedBackground { get; set; }

        /// <summary>
        /// The brush color of a selected ListBoxItem when the ListBox that 
        /// contains it loses focus.
        /// </summary>
        public Brush UnfocusedSelectedBackground { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteListBoxItem(Game game)
            : base(game)
        {
            Background = Brushes.Transparent;
            SelectedBackground = Brushes.Transparent;
            UnfocusedSelectedBackground = Brushes.Transparent;
            Foreground = Brushes.Transparent;
            BorderBrush = Brushes.Transparent;
            FontFamily = new FontFamily("Arial");
            BorderThickness = new Thickness(1);
            Padding = new Thickness(4, 0, 0, 0);
            Focusable = true;
        }

        /// <summary>
        /// Loads the content of the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _listBoxContent = new XAMLiteLabelNew(Game)
            {
                Content = Content,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = FontFamily,
                Spacing = Spacing,
                Padding = Padding,
                Foreground = Foreground,
                Visibility = Visibility.Hidden
            };
            Game.Components.Add(_listBoxContent);

            var w = _listBoxContent.MeasureString().X + Padding.Left + Padding.Right;
            var h = _listBoxContent.MeasureString().Y + Padding.Top + Padding.Bottom;

            if (w > Width)
            {
                Width = (int)w;
            }
            
            if (h > Height)
            {
                Height = (int)h;
            }

            var width = _listBoxContent.Width + (int)Padding.Left + (int)Padding.Right;
            if (Parent.Width < width)
            {
                var parent = (XAMLiteListBox)Parent;
                parent.UpdateWidth(width);
            }

            Game.Components.Remove(_listBoxContent);

            BackgroundPanel = new XAMLiteRectangleNew(Game)
                {
                    Width = Width,
                    Height = Height,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(BorderThickness.Left, 0, BorderThickness.Top, BorderThickness.Bottom),
                    Opacity = 0.45f
                };
            Children.Add(BackgroundPanel);

            Children.Add(_listBoxContent);
        }

        /// <summary>
        /// Sets a reference to the specific class that the parent represents.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _parent = (XAMLiteListBox)Parent;
        }

        /// <summary>
        /// Updates the margins and widths that make up the control.
        /// </summary>
        /// <param name="margin"></param>
        internal void UpdateMarginAndWidth(Thickness margin)
        {
            // set margins
            Margin = margin;

            // set Widths.
            Width = _parent.Width;
            BackgroundPanel.Width = Width - (int)_parent.BorderThickness.Right - (int)_parent.BorderThickness.Left;
            var m = BackgroundPanel.Margin;
            BackgroundPanel.Margin = new Thickness(_parent.BorderThickness.Left, m.Top, m.Right, m.Bottom);
            m = _listBoxContent.Margin;
            _listBoxContent.Margin = new Thickness(_parent.BorderThickness.Left, m.Top, m.Right, m.Bottom);

            MouseDown += OnMouseDown;
        }

        /// <summary>
        /// Sets the selected color and calls its parent to deselect the other
        /// Items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            IsSelected = true;
            IsFocused = true;

            _parent.DeselectAll(Index);
            BackgroundPanel.Fill = SelectedBackground;
            BackgroundPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When the ListBox containing the ListBoxItem loses focus, the brush
        /// color of the selected item changes to an unfocused color.
        /// </summary>
        public void UnfocusSelectedBrush(bool isFocused)
        {
            IsFocused = false;
            
            BackgroundPanel.Fill = UnfocusedSelectedBackground;
            BackgroundPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            MouseDown -= OnMouseDown;
            foreach (var child in Children)
            {
                child.Dispose();
            }
        }

        /// <summary>
        /// Modifies the width when necessary.
        /// </summary>
        public void ModifyWidth(int width)
        {
            Width = width;
            _listBoxContent.Width = width;
            BackgroundPanel.Width = width - (int)BorderThickness.Left - (int)BorderThickness.Right;
        }
    }
}
