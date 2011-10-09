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
    /// Interaction logic for XamlMarkup.xaml
    /// </summary>
    public partial class XamlMarkup : Window
    {



        public XamlMarkup()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void dogImage_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void dogImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.imageLabel.Content = "Image (clicked)";
        }

        private void dogImage_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void dogImage_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }


    }
}
