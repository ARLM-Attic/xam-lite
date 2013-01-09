using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteComboBox : XAMLiteListBox
    {
        /// <summary>
        /// The text displayed in the ComboBox.
        /// </summary>
        private string _text;

        /// <summary>
        /// The default text set at start.
        /// </summary>
        private string _defaultText;

        /// <summary>
        /// The text to be displayed in the TextBox.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;

                if (_textBox != null)
                {
                    _textBox.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that enables or disables editing of the text 
        /// in text box of the ComboBox.
        /// </summary>
        public bool IsEditable { get; set; }

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
        /// Represents the top portion of the ListBox.
        /// </summary>
        private XAMLiteTextBoxNew _textBox;

        /// <summary>
        /// The gradient color that displays when the combo box has been 
        /// selected.
        /// </summary>
        private XAMLiteImageNew _textBoxHover;

        /// <summary>
        /// The normal state button.
        /// </summary>
        private XAMLiteImageNew _button;

        /// <summary>
        /// The hover state button.
        /// </summary>
        private XAMLiteImageNew _buttonOver;

        /// <summary>
        /// True when the children are visible.
        /// </summary>
        private bool _areItemsVisibile;

        /// <summary>
        /// The height of the control when the items are visible.
        /// </summary>
        private int _openHeight;

        /// <summary>
        /// When true, no other combo boxes may open.
        /// </summary>
        protected static bool IsOpenLock;

        /// <summary>
        /// When true, the mouse down functions will work properly for the 
        /// specific control.  This is set to true when the IsOpenLock is set.
        /// </summary>
        private bool _isOpenLock;

        /// <summary>
        /// When true, the first mouse up will allow the combo box to be opened.
        /// This prevents the combo box from opening when a mouse down occurred
        /// on the control when it first became visible.
        /// </summary>
        private bool _isJustVisible;

        /// <summary>
        /// 
        /// </summary>
        public override Visibility Visibility
        {
            get
            {
                return base.Visibility;
            }

            set
            {
                base.Visibility = value;

                if (_textBox == null)
                {
                    return;
                }

                if (value == Visibility.Visible)
                {
                    _isJustVisible = true;
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteComboBox(Game game)
            : base(game)
        {
            FontFamily = new FontFamily("Verdana12");
            Foreground = Brushes.Black;
            Background = Brushes.White;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _defaultText = Text;

            _textBox = new XAMLiteTextBoxNew(Game)
            {
                IsReadOnly = true,
                Text = _defaultText == string.Empty ? "Add default text" : _defaultText,
                Width = Width,
                IsCursorOveride = true,
                Height = 28,
                FontFamily = FontFamily,
                Foreground = Foreground,
                Background = Background,
                BorderBrush = BorderBrush,
                BorderThickness = BorderThickness,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Thickness(7, 0, 7, 0),
                DrawOrder = DrawOrder
            };
            Children.Add(_textBox);

            _textBoxHover = new XAMLiteImageNew(Game, CreateGradientTexture(150))
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = _textBox.Width - (int)_textBox.BorderThickness.Left - (int)_textBox.BorderThickness.Right,
                Height = _textBox.Height - (int)_textBox.BorderThickness.Top - (int)_textBox.BorderThickness.Bottom,
                Margin = new Thickness(_textBox.BorderThickness.Left, _textBox.BorderThickness.Top, 0, 0),
                Background = SelectedBackground,
                Visibility = Visibility.Hidden,
                DrawOrder = DrawOrder + 1
            };
            Children.Add(_textBoxHover);

            _button = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/combobox-arrow",
                Width = 15,
                Height = 8,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 10, 5, 0),
                Background = BorderBrush,
                DrawOrder = DrawOrder + 1
            };
            Children.Add(_button);

            _buttonOver = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/combobox-arrow-hover",
                Width = 15,
                Height = 8,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 10, 5, 0),
                Background = SelectedBackground,
                DrawOrder = DrawOrder + 1
            };
            Children.Add(_buttonOver);
        }

        /// <summary>
        /// Initializes any event handlers.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _textBox.MouseDown += TextBoxOnMouseDown;
            _textBox.MouseEnter += TextBoxOnMouseEnter;
            _textBox.MouseLeave += TextBoxOnMouseLeave;
            _textBox.MouseUp += TextBoxOnMouseUp;
            MouseLeave += OnMouseLeave;

            // This is messing up Virtual pig once the 
            // gameplay screen is active.
            LostFocus += OnLostFocus;
        }

        /// <summary>
        /// Checks whether the combo box just became visible.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_isJustVisible && Ms.LeftButton == ButtonState.Released)
            {
                _isJustVisible = false;
            }
        }

        /// <summary>
        /// Updates the width of the control according to the largest item's 
        /// width.
        /// </summary>
        private void UpdateWidth()
        {
            var w = _textBox.Width;

            foreach (var item in Items)
            {
                var i = (XAMLiteComboBoxItem)item;
                var width = i.Width + (int)i.Padding.Left + (int)i.Padding.Right + (int)BorderThickness.Left + (int)BorderThickness.Right;
                if (width > w)
                {
                    w = width;
                }
            }

            Width = w;
            _textBox.Width = Width;
            _textBoxHover.Width = Width - (int)BorderThickness.Left - (int)BorderThickness.Right;
        }

        /// <summary>
        /// Calls a method to modify the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        protected override void UpdateItems()
        {
            // Since this class derives from a ListBox, the first Item must be
            // moved downward to accommodate the top portion of the control.
            // The remaining Items are adjusted in the base class.
            if (Items.Count > 0)
            {
                var m = Items[0].Margin;
                Items[0].Margin = new Thickness(m.Left, m.Top + _textBox.Height - BorderThickness.Top, m.Right, m.Bottom);
            }

            UpdateWidth();
            UpdateHeight();

            base.UpdateItems();
        }

        /// <summary>
        /// Modifies the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        private void UpdateHeight()
        {
            var h = BorderThickness.Top + BorderThickness.Bottom;

            if (Items == null)
            {
                return;
            }

            foreach (var item in Items)
            {
                h += item.Height;
            }

            Height = (int)h + _textBox.Height - (int)BorderThickness.Top;

            // set the open height.
            _openHeight = Height;
        }

        /// <summary>
        /// Closes the dialog when focus is lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnLostFocus(object sender, EventArgs eventArgs)
        {
            _textBoxHover.Visibility = Visibility.Hidden;

            Close();

            if (_isOpenLock)
            {
                _isOpenLock = false;
                IsOpenLock = false;
                _textBox.Text = _text;
            }
        }

        /// <summary>
        /// Removes the focus on the text box, thus removing the 
        /// highlighted border color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!_areItemsVisibile)
            {
                _textBox.IsFocused = false;
            }
        }

        /// <summary>
        /// This prevent the text box normal tendency to put an empty string 
        /// in the text box, in case a combo box was selected which was beneath
        /// another open combo box when it was closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void TextBoxOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_isOpenLock && _areItemsVisibile)
            {
                _textBox.IsFocused = true;
                _textBox.Text = string.Empty;

            }
            else if (_isOpenLock && !_areItemsVisibile)
            {
                _textBox.IsFocused = true;
                _textBox.Text = _text;
                _isOpenLock = false;
                IsOpenLock = false;
            }
        }

        /// <summary>
        /// Makes visible/hidden all of the ComboBoxItems.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void TextBoxOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_isJustVisible)
            {
                return;
            }

            if (_isOpenLock || !IsOpenLock)
            {
                IsOpenLock = true;
                _isOpenLock = true;

                _areItemsVisibile = !_areItemsVisibile;

                foreach (var child in Children)
                {
                    if (!(child is XAMLiteTextBoxNew) && !(child is XAMLiteImageNew))
                    {
                        child.Visibility = _areItemsVisibile ? Visibility.Visible : Visibility.Hidden;
                    }
                }

                if (_areItemsVisibile && !IsFocused)
                {
                    IsFocused = true;
                }
                else if (!_areItemsVisibile && IsFocused)
                {
                    IsFocused = false;
                }

                Height = _areItemsVisibile ? _openHeight : _textBox.Height;

                ToggleButtons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void TextBoxOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            _buttonOver.Visibility = Visibility.Visible;
            _textBoxHover.Visibility = Visibility.Visible;
            _button.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void TextBoxOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_areItemsVisibile)
            {
                _buttonOver.Visibility = Visibility.Visible;
                _textBoxHover.Visibility = Visibility.Visible;
                _button.Visibility = Visibility.Hidden;
            }
            else
            {
                _button.Visibility = Visibility.Visible;
                _buttonOver.Visibility = Visibility.Hidden;
                _textBoxHover.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ToggleButtons()
        {
            if (_areItemsVisibile)
            {
                _buttonOver.Visibility = Visibility.Visible;
                _textBoxHover.Visibility = Visibility.Visible;
                _button.Visibility = Visibility.Hidden;
            }
            else
            {
                _button.Visibility = Visibility.Visible;
                _buttonOver.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ModifyChildFocusAndHighlightColor()
        {
            foreach (XAMLiteComboBoxItem item in Items)
            {
                if (item.IsSelected)
                {
                    item.ModifySelectedBrush(IsFocused);
                }
                else
                {
                    item.RemoveHighLight();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveHighLightColor(int index)
        {
            foreach (XAMLiteComboBoxItem item in Items)
            {
                if (item.Index != index)
                {
                    item.RemoveHighLight();
                }
            }
        }

        /// <summary>
        /// Hides all of the ComboBox assets except the Top portion.
        /// </summary>
        private void HideChildren()
        {
            foreach (var child in Children)
            {
                if (!(child is XAMLiteTextBoxNew) && !(child is XAMLiteImageNew))
                {
                    child.Visibility = Visibility.Hidden;
                }

                Height = _textBox.Height;

                ToggleButtons();
            }
        }

        /// <summary>
        /// Hides the Items.
        /// </summary>
        public void Close()
        {
            _areItemsVisibile = false;
            HideChildren();
        }

        /// <summary>
        /// Sets the text and hides the Items.
        /// </summary>
        public void Close(string content)
        {
            if (IsEditable)
            {
                _text = content;
            }

            Close();
        }

        /// <summary>
        /// Overrides the natural grid mode to make every child return
        /// to its prior visibility state when it the grid becomes visible.
        /// Instead, the items will remain closed if !IsFocused.
        /// </summary>
        protected override void UpdateChildVisibility()
        {
            base.UpdateChildVisibility();

            if (!IsFocused)
            {
                Close();

                if (Visibility == Visibility.Hidden)
                {
                    _button.Visibility = Visibility.Hidden;
                    _buttonOver.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Builds the gradient-styled default buttons.
        /// </summary>
        /// <returns></returns>
        private Texture2D CreateGradientTexture(int brightness)
        {
            const int GradientThickness = 3;
            var t = new Texture2D(Game.GraphicsDevice, 55, Height);

            var bgc = new Color[55 * Height];

            for (int i = bgc.Length - 1; i > 0; i--)
            {
                var gradientColor = ((i * 20) / (Height * GradientThickness)) - brightness;
                bgc[i] = new Color(gradientColor, gradientColor, gradientColor, gradientColor);
            }

            t.SetData(bgc);

            return t;
        }

        /// <summary>
        /// Disposes of the text box and its mouse down hook.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _textBox.MouseDown -= TextBoxOnMouseDown;
            _textBox.MouseEnter -= TextBoxOnMouseEnter;
            _textBox.MouseLeave -= TextBoxOnMouseLeave;
            _textBox.MouseUp -= TextBoxOnMouseUp;
            LostFocus -= OnLostFocus;
            MouseLeave -= OnMouseLeave;

            foreach (var item in Items)
            {
                item.Dispose();
            }
        }
    }
}
