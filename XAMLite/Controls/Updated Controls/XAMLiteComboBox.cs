﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    using Microsoft.Xna.Framework.Graphics;
    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XAMLiteComboBox : XAMLiteListBox
    {
        /// <summary>
        /// 
        /// </summary>
        //public bool IsEditable { get; set; }

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

        private XAMLiteImageNew _textBoxHover;

        private XAMLiteImageNew button;

        private XAMLiteImageNew buttonOver;

        /// <summary>
        /// True when the children are visible.
        /// </summary>
        private bool _areItemsVisibile;

        /// <summary>
        /// At start up, the ComboBox should be initially closed,  but the
        /// Items have not yet been added to the grid.  At the first Update
        /// Items call, once the Items have been added, the control visibility
        /// is toggled off.
        /// </summary>
        private bool _isFirstUpdate = true;

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
                Padding = new Thickness(7, 0, 7, 0)
            };
            Children.Add(_textBox);

            //var texture = Game.Content.Load<Texture2D>("Icons/combobox-arrow");
            _textBoxHover = new XAMLiteImageNew(Game, CreateGradientTexture(150))
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = _textBox.Width - (int)_textBox.BorderThickness.Left - (int)_textBox.BorderThickness.Right,
                Height = _textBox.Height - (int)_textBox.BorderThickness.Top - (int)_textBox.BorderThickness.Bottom,
                Margin = new Thickness(_textBox.BorderThickness.Left, _textBox.BorderThickness.Top, 0, 0),
                Background = SelectedBackground,
                DrawOrder = 4999
            };
            Children.Add(_textBoxHover);

            button = new XAMLiteImageNew(Game)
                {
                    SourceName = "Icons/combobox-arrow",
                    Width = 15,
                    Height = 8,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 10, 5, 0),
                    Background = BorderBrush,
                    DrawOrder = 5000
                };
            Children.Add(button);

            buttonOver = new XAMLiteImageNew(Game)
            {
                SourceName = "Icons/combobox-arrow-hover",
                Width = 15,
                Height = 8,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 10, 5, 0),
                Background = SelectedBackground,
                DrawOrder = 5000
            };
            Children.Add(buttonOver);
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
            LostFocus += OnLostFocus;
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
        /// Closes the dialog when focus is lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnLostFocus(object sender, EventArgs eventArgs)
        {
            Close();
        }

        /// <summary>
        /// Calls a method to modify the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        protected override void UpdateItems()
        {
            // The first time this method is called, the visibility of the 
            // Items is set to hidden.
            if (_isFirstUpdate)
            {
                _isFirstUpdate = false;

                HideChildren();
            } 

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
        }

        /// <summary>
        /// Makes visible all of the ComboBoxItems.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void TextBoxOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _areItemsVisibile = !_areItemsVisibile;

            foreach (var child in Children)
            {
                if (!(child is XAMLiteTextBoxNew) && !(child is XAMLiteImageNew))
                {
                    child.Visibility = _areItemsVisibile ? Visibility.Visible : Visibility.Hidden;
                }
            }

            ToggleButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void TextBoxOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            buttonOver.Visibility = Visibility.Visible;
            _textBoxHover.Visibility = Visibility.Visible;
            button.Visibility = Visibility.Hidden;
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
                buttonOver.Visibility = Visibility.Visible;
                _textBoxHover.Visibility = Visibility.Visible;
                button.Visibility = Visibility.Hidden;
            }
            else
            {
                button.Visibility = Visibility.Visible;
                buttonOver.Visibility = Visibility.Hidden;
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
                buttonOver.Visibility = Visibility.Visible;
                _textBoxHover.Visibility = Visibility.Visible;
                button.Visibility = Visibility.Hidden;
            }
            else
            {
                button.Visibility = Visibility.Visible;
                buttonOver.Visibility = Visibility.Hidden;
                _textBoxHover.Visibility = Visibility.Hidden;
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
            Text = content;

            Close();
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
            _textBox.Dispose();
        }
    }
}