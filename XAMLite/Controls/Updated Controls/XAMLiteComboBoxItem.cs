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
    public class XAMLiteComboBoxItem : XAMLiteListBoxItem
    {
        /// <summary>
        /// Parent of the this control.
        /// </summary>
        private XAMLiteComboBox _parent;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteComboBoxItem(Game game)
            : base(game)
        {
            Padding = new Thickness(7, 2, 4, 2);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _parent = (XAMLiteComboBox)Parent;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseEnter += OnMouseEnter;
            MouseDown += OnMouseDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (Visibility == Visibility.Visible)
            {
                _parent.SelectedIndex = ItemIndex;
                _parent.RemoveHighLightColor(Index);
                BackgroundPanel.Fill = SelectedBackground;
                BackgroundPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Tells its parent to close the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (IsEnabled)
            {
                IsSelected = true;
                IsFocused = false;
                _parent.Close((string)Content);
            }
        }

        /// <summary>
        /// When the ListBox containing the ListBoxItem loses focus, the brush
        /// color of the selected item changes to an unfocused color.
        /// </summary>
        public override void ModifySelectedBrush(bool isFocused)
        {
            BackgroundPanel.Fill = SelectedBackground;
            BackgroundPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveHighLight()
        {
            if (BackgroundPanel != null)
            {
                BackgroundPanel.Fill = Brushes.Transparent;
                BackgroundPanel.Visibility = Visibility.Hidden;
            }
        }
    }
}
