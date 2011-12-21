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

    public partial class GridChildrenVisibility : Window
    {
        public GridChildrenVisibility ()
        {
            InitializeComponent();
        }

        private void gridCheckBox_Checked (object sender, RoutedEventArgs e)
        {
            catsAndDogsGrid.Visibility = Visibility.Visible;
        }

        private void gridCheckBox_Unchecked (object sender, RoutedEventArgs e)
        {
            catsAndDogsGrid.Visibility = Visibility.Hidden;
        }

        private void catCheckBox_Checked (object sender, RoutedEventArgs e)
        {
            catImage.Visibility = Visibility.Visible;
        }

        private void catCheckBox_Unchecked (object sender, RoutedEventArgs e)
        {
            catImage.Visibility = Visibility.Hidden;
        }

        private void dogCheckBox_Checked (object sender, RoutedEventArgs e)
        {
            dogImage.Visibility = Visibility.Visible;
        }

        private void dogCheckBox_Unchecked (object sender, RoutedEventArgs e)
        {
            dogImage.Visibility = Visibility.Hidden;
        }
    }

}
