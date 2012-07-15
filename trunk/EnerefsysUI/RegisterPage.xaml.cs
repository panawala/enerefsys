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
using EnerefsysBLL;
using System.Security.Cryptography;
using System.IO;
using EnerefsysBLL.Utility;

namespace ControlApp
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //淡入效果
            this.Opacity = 0;
            DoubleAnimation OpercityAnimation =
            new DoubleAnimation(0.01, 1.00, new Duration(TimeSpan.FromSeconds(1)));
            this.BeginAnimation(Page.OpacityProperty, OpercityAnimation);

            string machineCodeStr = MachineCode.GetCpuInfo();
            string md5Str = Utility.GetMD5Hash(machineCodeStr);
            mchineCode.Text = Utility.GetBase64Hash(md5Str);
            mmCode.Text = Utility.GetFromBase64Hash(mchineCode.Text);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string test = "ssssssssssssdddddddddddddddfffffffffffaaaaaaaaaaaaaaaadddddddddddd";
            string result = Utility.GetMD5Hash(test);

        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            string result = Utility.GetMD5Hash(mchineCode.Text);
            if (result == mmCode.Text.ToUpper())
            {
                //设置激活成功
                Utility.IniWriteValue("Active", "IsSuccess", "success", Utility.filePath);
                File.SetAttributes(Utility.filePath, FileAttributes.Hidden);
            }
        }
        
    }
}
