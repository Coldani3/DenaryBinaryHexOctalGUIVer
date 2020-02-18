using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DenaryBinaryHexOctalGUIVer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Base> ScreenNameToBase = new Dictionary<string, Base>
        {
            { "Binary (Base 2)", Base.BINARY },
            { "Octal (Base 8)", Base.OCTAL },
            { "Denary (Base 10)", Base.DENARY },
            { "Hexadecimal (Base 16)", Base.HEXADECIMAL }
        };

        public MainWindow()
        {
            InitializeComponent();

            //BitmapImage image = new BitmapImage(new Uri(@"icon.png", UriKind.RelativeOrAbsolute));
            //this.Icon = image;

            foreach (string numBase in ScreenNameToBase.Keys)
            {
                FromBox.Items.Add(numBase);
                ToBox.Items.Add(numBase);
            }

            this.FromBox.SelectedIndex = 2;
            this.ToBox.SelectedIndex = 0;
        }

        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckInputAndUpdate();
        }

        private void CheckInputAndUpdate()
        {
            if (FromBox.SelectedIndex != -1)
            {
                string currInput = InputBox.Text;

                if (!ValidateInput(InputBox.Text, ScreenNameToBase[(string)FromBox.SelectedItem])) InputBox.Text = "";
                else UpdateOutput();
            }
        }

        private void BaseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromBox.SelectedIndex != -1)
            {
                if (!Converter.ValidateForBase(InputBox.Text.ToUpper(), ScreenNameToBase[(string)FromBox.SelectedItem])) InputBox.Text = "";
                UpdateOutput();
            }
        }

        public bool ValidateInput(string input, Base fromBase)
        {
            if (!Converter.ValidateForBase(InputBox.Text.ToUpper(), fromBase))
            {
                MessageBox.Show("Invalid input! It must match the specified base!");
                return false;
            }

            return true;
        }

        public void UpdateOutput()
        {
            if (FromBox.SelectedIndex != -1 && ToBox.SelectedIndex != -1 && InputBox.Text != "")
            {
                Base fromBase = ScreenNameToBase[(string)FromBox.SelectedItem];
                Base toBase = ScreenNameToBase[(string)ToBox.SelectedItem];

                OutputBox.Text = Converter.ConvertToBase(InputBox.Text, fromBase, toBase);
            }
            else OutputBox.Text = "";
        }

        private void Window_UnfocusOnClick(object sender, MouseButtonEventArgs e)
        {
            ((UIElement) sender).Focus();
        }

        private void ForceCalcButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInputAndUpdate();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckInputAndUpdate();
            }
        }

        private void SwapBasesButton_Click(object sender, RoutedEventArgs e)
        {
            object fromBase = FromBox.SelectedItem;
            FromBox.SelectedItem = ToBox.SelectedItem;
            ToBox.SelectedItem = fromBase;
            CheckInputAndUpdate();
        }
    }
}
