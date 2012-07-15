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
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using BLL;
using System.IO;

namespace ControlApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private DateTime startTime;
        private DateTime endTime;
        public Window1()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            startTime = System.DateTime.Now;
            e.Handled = true;


            //MyRectangle.Name = "MyRectangle";  
            //NameScope.SetNameScope(this, new NameScope());
            //this.RegisterName(MyRectangle.Name, MyRectangle);

            //DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            //myDoubleAnimation.From = 0;
            //myDoubleAnimation.To = 1;
            //myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            //Storyboard.SetTargetName(myDoubleAnimation,MyRectangle.Name);
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Border.OpacityProperty));

            //Storyboard myStoryboard = new Storyboard();
            //myStoryboard.Children.Add(myDoubleAnimation);

            //myStoryboard.Begin(this);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ButtonState!=MouseButtonState.Released)
                this.DragMove();
        }


        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            endTime = DateTime.Now;
            TimeSpan ts = endTime - startTime;
            if (ts.Seconds >= 1)
                this.Close();
            else
                pageFrame.Source = new Uri("Index.xaml", UriKind.Relative);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Utility.filePath))
            {
                int remainCount = Convert.ToInt32(Utility.IniReadValue("Active", "Count", Utility.filePath));
                string isSucess = Utility.IniReadValue("Active", "IsSuccess", Utility.filePath);
                if (isSucess == "failed")
                {
                    MessageBox.Show("请联系管理员激活！");
                    pageFrame.Source = new Uri("RegisterPage.xaml", UriKind.Relative);
                }
                if (remainCount < 1)
                {
                    MessageBox.Show("软件使用次数已经达到，请联系软件制作商！\n联系人：王祥；电话：13761775505");
                    this.Close();
                }
                remainCount--;
                Utility.IniWriteValue("Active", "Count", remainCount.ToString(), Utility.filePath);
            }
            else
            {
                Utility.IniWriteValue("Active", "Count", "50", Utility.filePath);
                Utility.IniWriteValue("Active", "IsSuccess", "failed", Utility.filePath);
            }
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
