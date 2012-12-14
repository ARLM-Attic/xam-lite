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
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _parent = (XAMLiteComboBox)Parent;

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
            _parent.DeselectAll(Index);
            BackgroundPanel.Fill = SelectedBackground;
            BackgroundPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Tells its parent to close the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _parent.Close();
        }

    }
}
