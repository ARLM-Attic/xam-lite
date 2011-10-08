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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Label example.
            Label label = new Label();
            label.Content = "Hello";
            label.Foreground = Brushes.Red;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Visibility = Visibility.Visible;

            // Add label to window grid.
            this.grid1.Children.Add(label);

            // Rectangle example.
            Rectangle rectangle = new Rectangle();
            rectangle.Margin = new Thickness(10, 10, 50, 50);
            rectangle.Stroke = Brushes.Green;
            
            // Add rectangle to window grid.
            this.grid1.Children.Add(rectangle);

        }

    }
}
