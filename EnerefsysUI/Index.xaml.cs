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
using System.Windows.Media.Animation;
using System.IO;
using BLL;

namespace ControlApp
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        public Index()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("EngineInputPage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("ShowPage.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //淡入效果
            this.Opacity = 0;
            DoubleAnimation OpercityAnimation =
            new DoubleAnimation(0.01, 1.00, new Duration(TimeSpan.FromSeconds(1)));
            this.BeginAnimation(Page.OpacityProperty, OpercityAnimation);



            if (File.Exists(Utility.filePath))
            {
                string isSucess = Utility.IniReadValue("Active", "IsSuccess", Utility.filePath);
                if (isSucess == "failed")
                {
                    NavigationService.Navigate(new Uri("RegisterPage.xaml", UriKind.Relative));
                }
            }
            
           


        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("RegisterPage.xaml", UriKind.Relative));
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("ImportPage.xaml", UriKind.Relative));
        }
    }
}
