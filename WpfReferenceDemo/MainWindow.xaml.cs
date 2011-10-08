using System;
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
            Label label = new Label();
            label.Content = "Hello, world!";
            label.Foreground = Brushes.Yellow;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Visibility = Visibility.Visible;
            ComponentsGrid.Children.Add(label);

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
            rectangle2.Margin = new Thickness(10, 10, 10, 10);
            rectangle2.Stroke = Brushes.Pink;
            ComponentsGrid.Children.Add(rectangle2);

        }

    }
}
