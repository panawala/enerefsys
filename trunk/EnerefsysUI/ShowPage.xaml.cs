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
using Microsoft.Win32;
using BLL;
using System.Windows.Media.Animation;

namespace ControlApp
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class ShowPage : Page
    {
        public ShowPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //淡入效果
            this.Opacity = 0; 
            DoubleAnimation OpercityAnimation =
            new DoubleAnimation(0.01, 1.00,new Duration(TimeSpan.FromSeconds(1)));
            this.BeginAnimation(Page.OpacityProperty, OpercityAnimation);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double temperature = Convert.ToDouble(Temperature.Text);

            SoluteResult sr = EngineManager.GetReslutByTemByWeight(temperature, Convert.ToInt32(Load.Text));
            MinFlow.Text = sr.Solute.ToString();
            EngineW.Text = "最低为：F(" + temperature + ")=" + sr.Result;
        }

        private void Temperature_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
        }

        private void Temperature_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (txt.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
