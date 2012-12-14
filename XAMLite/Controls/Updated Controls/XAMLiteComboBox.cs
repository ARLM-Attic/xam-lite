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
        /// The initial text to be displayed in the TextBox.
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

                if (textBox != null)
                {
                    textBox.Text = value;
                }   
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        private XAMLiteTextBoxNew textBox;

        private bool _firstUpdate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteComboBox(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _defaultText = Text;

            textBox = new XAMLiteTextBoxNew(Game)
            {
                Text = _defaultText == string.Empty ? "Combo Box" : _defaultText,
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
            Grid.Children.Add(textBox);
        }

        public override void Initialize()
        {
            base.Initialize();

            textBox.MouseDown += TextBoxOnMouseDown;
        }

        /// <summary>
        /// Calls a method to modify the height of the control so that it is 
        /// only as tall as the list of Items.
        /// </summary>
        protected override void UpdateItems()
        {
            if (Items[0].Visibility == Visibility.Visible && _firstUpdate)
            {
                _firstUpdate = true;

                HideChildren();
            } 

            if (Items.Count > 0)
            {
                var m = Items[0].Margin;
                Items[0].Margin = new Thickness(m.Left, m.Top + textBox.Height, m.Right, m.Bottom);
            }

            base.UpdateItems();

            UpdateHeight();
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

            Height = (int)h;
        }

        /// <summary>
        /// Makes visible all of the ComboBoxItems.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void TextBoxOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            foreach (var child in Grid.Children)
            {
                if (!(child is XAMLiteTextBoxNew))
                {
                    child.Visibility = Visibility.Visible;
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
        /// Hides the Items portion of the combo box when selected.
        /// </summary>
        public void Close(string content)
        {
            Text = content;

            HideChildren();
        }

        /// <summary>
        /// Disposes of the text box and its mouse down hook.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            textBox.MouseDown -= TextBoxOnMouseDown;
            textBox.Dispose();
        }
    }
}
