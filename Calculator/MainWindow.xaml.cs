using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            foreach (var el in root.Children)
            {
                if (el is Button)
                {
                    if ((string)((Button)el).Content != "=")
                        ((Button)el).Click += Button_Click;
                }
            }
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = this.MaxWidth;
            doubleAnimation.Duration = TimeSpan.FromSeconds(5);
            textBox1.BeginAnimation(TextBox.WidthProperty, doubleAnimation);
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Start();
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            textBox1.BeginAnimation(TextBox.WidthProperty, null);
            timer.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = textBox1.Text == "Error" ? "" : textBox1.Text;
            var str = (string)((Button)sender).Content;
            if (str == "C")
            {
                textBox1.Text = "";
            }
            else if (str == "→")
            {
                var temp = textBox1.Text.Length;
                if (temp >= 2)
                {
                    textBox1.Text = textBox1.Text.Substring(0, temp - 1);
                }
                else if (temp <= 1)
                {
                    textBox1.Text = "";
                }
            }
            else
            {
                textBox1.Text += str;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                    throw new Exception();
                var value = new DataTable().Compute(textBox1.Text, null).ToString();
                textBox1.Text = value;
                MessageBox.Show("Успешный подсчёт, ты молодец!");
            }
            catch
            {
                MessageBox.Show("Где-то ты ошибься!");
                textBox1.Text = "Error";
            }
        }
    }
}
