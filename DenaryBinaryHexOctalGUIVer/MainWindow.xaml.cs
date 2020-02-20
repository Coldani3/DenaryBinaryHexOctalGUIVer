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

        private const string CustomOption = "Custom";

        public MainWindow()
        {
            InitializeComponent();

            foreach (string numBase in ScreenNameToBase.Keys)
            {
                FromBox.Items.Add(numBase);
                ToBox.Items.Add(numBase);
            }

            FromBox.Items.Add(CustomOption);
            ToBox.Items.Add(CustomOption);

            this.FromBox.SelectedIndex = 2;
            this.ToBox.SelectedIndex = 0;
        }

        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckInputAndUpdate();
        }

        /// <summary>
        /// Checks the input box and updates the output box if the input field is valid.
        /// </summary>
        private void CheckInputAndUpdate()
        {
            if (FromBox.SelectedIndex != -1)
            {
                string currInput = InputBox.Text;
                Base fromBase;

                if ((string) FromBox.SelectedItem == CustomOption) fromBase = FromCustomBaseText.Text != "" ? new Base(Convert.ToInt32(FromCustomBaseText.Text)) : Base.DENARY; //Default to 10 if in doubt
                else fromBase = ScreenNameToBase[(string)FromBox.SelectedItem];

                if (!ValidateInput(InputBox.Text, fromBase)) InputBox.Text = "";
                else UpdateOutput(); //if custom base is selected and there is nothing in the custom base box do not update output.
            }
        }

        private void BaseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromBox.SelectedIndex != -1)
            {
                bool toCustomSelected = (string)ToBox.SelectedItem == CustomOption;
                bool fromCustomSelected = (string)FromBox.SelectedItem == CustomOption;

                ToCustomBaseText.IsEnabled = toCustomSelected;
                FromCustomBaseText.IsEnabled = fromCustomSelected;

                if (!fromCustomSelected && !toCustomSelected)
                {
                    if (!Converter.ValidateForBase(InputBox.Text.ToUpper(), ScreenNameToBase[(string)FromBox.SelectedItem])) InputBox.Text = "";
                }

                UpdateOutput();
            }
        }

        /// <summary>
        /// Validates a specified string for the specified base.
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <param name="fromBase">The numeric base to validate input against.</param>
        /// <returns>True if valid, false if not</returns>
        public bool ValidateInput(string input, Base fromBase)
        {
            if (!Converter.ValidateForBase(InputBox.Text.ToUpper(), fromBase))
            {
                MessageBox.Show("Invalid input! It must match the specified base!");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Check if a whole string is made of numbers
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumber(string text)
        {
            foreach (char character in text)
            {
                if (!Char.IsDigit(character)) return false;
            }

            return true;
        }

        /// <summary>
        /// Calculates and updates the output box to match the intended output.
        /// </summary>
        public void UpdateOutput()
        {
            if (FromBox.SelectedIndex != -1 && ToBox.SelectedIndex != -1 && InputBox.Text != "")
            {
                //if the custom base option is selected and the custom base text is not empty and the custom base is a number then use the custom base option
                Base fromBase;
                Base toBase;

                if ((string) FromBox.SelectedItem == CustomOption)
                {
                    if (FromCustomBaseText.Text != "" && IsNumber(FromCustomBaseText.Text)) fromBase = new Base(Convert.ToInt32(FromCustomBaseText.Text));
                    else fromBase = Base.DENARY;
                }
                else fromBase = ScreenNameToBase[(string)FromBox.SelectedItem];

                if ((string)ToBox.SelectedItem == CustomOption)
                {
                    if (ToCustomBaseText.Text != "" && IsNumber(ToCustomBaseText.Text)) toBase = new Base(Convert.ToInt32(ToCustomBaseText.Text));
                    else toBase = Base.DENARY;
                }
                else toBase = ScreenNameToBase[(string)ToBox.SelectedItem];

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

        private void BoxesEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) CheckInputAndUpdate();
        }

        private void SwapBasesButton_Click(object sender, RoutedEventArgs e)
        {
            object fromBase = FromBox.SelectedItem;
            FromBox.SelectedItem = ToBox.SelectedItem;
            ToBox.SelectedItem = fromBase;
            CheckInputAndUpdate();
        }

        private void FromCustomBaseText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
