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
using System.Windows.Shapes;

namespace WpfReferenceDemo
{
    /// <summary>
    /// Interaction logic for GridExample.xaml
    /// </summary>
    public partial class GridExample : Window
    {
        public GridExample ()
        {
            InitializeComponent();
        }

        private void Window_Loaded (object sender, RoutedEventArgs e)
        {
            
            // Green grid. Dynamic size based on window size.
            var grid = new Grid();
            grid.Margin = new Thickness(20, 20, 20, 20);
            grid.Background = Brushes.LightGreen;
            ComponentsGrid.Children.Add(grid);

            // Dog image. Bottom-center, with a little margin. Like button placement.
            var dogImage = new Image();
            dogImage.Width = 100;
            dogImage.Height = 100;
            dogImage.HorizontalAlignment = HorizontalAlignment.Center;
            dogImage.VerticalAlignment = VerticalAlignment.Bottom;
            dogImage.Margin = new Thickness(0, 0, 0, 10);
            dogImage.Source = new BitmapImage(new Uri(@"Content\Dog.png", UriKind.Relative));
            grid.Children.Add(dogImage);

        }
    }
}
