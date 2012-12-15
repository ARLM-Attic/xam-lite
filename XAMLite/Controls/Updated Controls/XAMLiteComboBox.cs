using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
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
        /// Represents the top portion of the ListBox.
        /// </summary>
        private XAMLiteTextBoxNew _textBox;

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
                Text = _defaultText == string.Empty ? "Add default text" : _defaultText,
                Width = Width,
                IsCursorOveride = true,
                Height = 28,
                FontFamily = new FontFamily("Verdana12"),
                Foreground = Brushes.Black,
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Thickness(7, 0, 7, 0)
            };
            Grid.Children.Add(_textBox);
        }

        /// <summary>
        /// Initializes any event handlers.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _textBox.MouseDown += TextBoxOnMouseDown;
            LostFocus += OnLostFocus;
        }

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
                Items[0].Margin = new Thickness(m.Left, m.Top + _textBox.Height, m.Right, m.Bottom);
            }

            base.UpdateItems();

            //UpdateWidth();
            UpdateHeight();
        }

        //private void UpdateWidth()
        //{

        //    var w = Width;

        //    foreach (var item in Items)
        //    {
        //        if (item.Width > w)
        //        {
        //            w = item.Width;
        //        }
        //    }

        //    Width = w;
        //}

        /// <summary>
        /// Modifies the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        private void UpdateHeight()
        {
            var h = BorderThickness.Top;

            if (Items == null)
            {
                return;
            }

            foreach (var item in Items)
            {
                h += item.Height;
            }

            Height = (int)h;
        }

        /// <summary>
        /// Makes visible all of the ComboBoxItems.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void TextBoxOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _areItemsVisibile = !_areItemsVisibile;


            foreach (var child in Grid.Children)
            {
                if (!(child is XAMLiteTextBoxNew))
                {
                    child.Visibility = _areItemsVisibile ? Visibility.Visible : Visibility.Hidden;
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
            foreach (var child in Grid.Children)
            {
                if (!(child is XAMLiteTextBoxNew))
                {
                    child.Visibility = Visibility.Hidden;
                }
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
