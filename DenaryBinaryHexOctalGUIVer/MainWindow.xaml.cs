using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DenaryBinaryHexOctalGUIVer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Dictionary<string, Base> ScreenNameToBase = new Dictionary<string, Base>
        {
            { "Binary (Base 2)", Base.Binary },
            { "Octal (Base 8)", Base.Octal },
            { "Denary (Base 10)", Base.Denary },
            { "Hexadecimal (Base 16)", Base.Hexadecimal }
        };
        private const string CustomOption = "Custom";

        private static bool WarningShown = false;

        public MainWindow()
        {
            InitializeComponent();

            //Add default options to the base selection boxes
            foreach (string numBase in ScreenNameToBase.Keys)
            {
                this.FromBox.Items.Add(numBase);
                this.ToBox.Items.Add(numBase);
            }

            this.FromBox.Items.Add(CustomOption);
            this.ToBox.Items.Add(CustomOption);

            this.FromBox.SelectedIndex = 2;
            this.ToBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Checks the input box and updates the output box if the input field is valid.
        /// </summary>
        private void CheckInputAndUpdate()
        {
            if (this.FromBox.SelectedIndex != -1)
            {
                string currInput = this.InputBox.Text;
                Base fromBase;

                if ((string)this.FromBox.SelectedItem == CustomOption) fromBase = this.FromCustomBaseText.Text != "" ? new Base(Convert.ToInt32(this.FromCustomBaseText.Text)) : Base.Denary; //Default to 10 if in doubt
                else fromBase = ScreenNameToBase[(string)this.FromBox.SelectedItem];

                //only update the output if the base is valid
                if (!ValidateInput(this.InputBox.Text, fromBase)) this.InputBox.Text = "";
                else UpdateOutput();
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
            if (!Converter.ValidateForBase(this.InputBox.Text.ToUpper(), fromBase))
            {
                MessageBox.Show("Invalid input! It must match the specified base!");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Check if a whole string is made of numbers
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>True if the string is made up of nothing but numbers.</returns>
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
            if (this.FromBox.SelectedIndex != -1 && this.ToBox.SelectedIndex != -1 && this.InputBox.Text != "")
            {
                //if the custom base option is selected and the custom base text is not empty and the custom base is a number then use the custom base option
                Base fromBase;
                Base toBase;

                if ((string)this.FromBox.SelectedItem == CustomOption)
                {
                    if (this.FromCustomBaseText.Text != "" && IsNumber(this.FromCustomBaseText.Text))
                    {
                        int baseNum = Convert.ToInt32(this.FromCustomBaseText.Text);
                        fromBase = new Base(baseNum);

                        if (baseNum > 36 && !WarningShown)
                        {
                            MessageBox.Show("Warning! Bases past 36 are currently not supported as there are no more letters to use to represent numbers past this point! Currently, as we use letters to support higher than 9 (and implementing the (x > 9) system is challenging), there are currently no letters left to support anything past 36, and so for now you must resort to the characters after capital Z in the ASCII system. Anything higher than the base notation equivalent of 42 will likely loop back to A.");
                            WarningShown = true;
                        }
                    }
                    else fromBase = Base.Denary;
                }
                else fromBase = ScreenNameToBase[(string)this.FromBox.SelectedItem];

                if ((string)this.ToBox.SelectedItem == CustomOption)
                {
                    if (this.ToCustomBaseText.Text != "" && IsNumber(this.ToCustomBaseText.Text))
                    {
                        int baseNum = Convert.ToInt32(this.ToCustomBaseText.Text);
                        toBase = new Base(baseNum);

                        if (baseNum > 36 && !WarningShown)
                        {
                            MessageBox.Show("Warning! Bases past 36 are currently not supported as there are no more letters to use to represent numbers past this point! Currently, as we use letters to support higher than 9 (and implementing the (x > 9) system is challenging), there are currently no letters left to support anything past 36, and so for now you must resort to the characters after capital Z in the ASCII system. Anything higher than the base notation equivalent of 42 will likely loop back to A.");
                            WarningShown = true;
                        }
                    }
                    else toBase = Base.Denary;
                }
                else toBase = ScreenNameToBase[(string)this.ToBox.SelectedItem];

                this.OutputBox.Text = Converter.ConvertToBase(this.InputBox.Text, fromBase, toBase);
            }
            else this.OutputBox.Text = "";
        }
        #region Events
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
            string customFromBase = FromCustomBaseText.Text;
            FromBox.SelectedItem = ToBox.SelectedItem;
            ToBox.SelectedItem = fromBase;
            FromCustomBaseText.Text = ToCustomBaseText.Text;
            ToCustomBaseText.Text = customFromBase;
            CheckInputAndUpdate();
        }

        private void BaseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromBox.SelectedIndex != -1)
            {
                bool toCustomSelected = (string)ToBox.SelectedItem == CustomOption;
                bool fromCustomSelected = (string)FromBox.SelectedItem == CustomOption;

                ToCustomBaseText.IsEnabled = toCustomSelected;
                FromCustomBaseText.IsEnabled = fromCustomSelected;

                if (!fromCustomSelected && !toCustomSelected && !Converter.ValidateForBase(InputBox.Text.ToUpper(), ScreenNameToBase[(string)FromBox.SelectedItem])) InputBox.Text = "";

                UpdateOutput();
            }
        }

        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckInputAndUpdate();
        }
        #endregion
    }
}
