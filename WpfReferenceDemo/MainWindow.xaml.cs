﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfReferenceDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        Label _label;

        Image _dogImage;

        /// <summary>
        /// Create some example controls and add them as children to the root XAML grid control.
        /// Note the WPF and XNA difference:
        /// WPF:  ComponentsGrid.Children.Add(label)
        /// XNA:  Components.Add(label)
        /// In this application, we've named the main grid "ComponentsGrid". The idea is that a grid is the UI
        /// control container in WPF/XAML, and the Components collection is the container in XNA. That's why 
        /// we've picked the named "ComponentsGrid" in this WPF reference application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Label example.
            _label = new Label();
            _label.Content = "Hello, world!";
            _label.Foreground = Brushes.Yellow;
            _label.HorizontalAlignment = HorizontalAlignment.Center;
            _label.VerticalAlignment = VerticalAlignment.Bottom;
            _label.Visibility = Visibility.Visible;
            ComponentsGrid.Children.Add(_label);

            // Rectangle example #1.
            Rectangle rectangle1 = new Rectangle();
            rectangle1.Width = 300;
            rectangle1.Height = 100;
            rectangle1.Fill = Brushes.LightGray;
            rectangle1.Stroke = Brushes.Green;
            rectangle1.StrokeThickness = 1;
            rectangle1.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle1.VerticalAlignment = VerticalAlignment.Top;
            rectangle1.Margin = new Thickness(0, 25, 0, 0);
            ComponentsGrid.Children.Add(rectangle1);

            // Rectangle example #2
            Rectangle rectangle2 = new Rectangle();
            rectangle2.Margin = new Thickness(30, 30, 30, 30);
            rectangle2.Stroke = Brushes.Pink;
            ComponentsGrid.Children.Add(rectangle2);

            // Clickable-image example.
            _dogImage = new Image();
            _dogImage.Width = 100;
            _dogImage.Height = 100;
            _dogImage.Source = new BitmapImage(new Uri(@"Content\Dog.png", UriKind.Relative));
            _dogImage.MouseDown +=new MouseButtonEventHandler(dogImage_MouseDown);
            _dogImage.MouseEnter +=new MouseEventHandler(dogImage_MouseEnter);
            _dogImage.MouseLeave += new MouseEventHandler(dogImage_MouseLeave);
            ComponentsGrid.Children.Add(_dogImage);

            var foo = new TextBlock();
            foo.TextWrapping = TextWrapping.Wrap;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dogImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _label.Content = "You clicked the dog!";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dogImage_MouseEnter(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseEnter event was raised!";
            _dogImage.Source = new BitmapImage(new Uri(@"Content\Dog-Bright.jpg", UriKind.Relative));
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dogImage_MouseLeave(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseLeave event was raised!";
            _dogImage.Source = new BitmapImage(new Uri(@"Content\Dog.png", UriKind.Relative));
            
        }

    }
}
