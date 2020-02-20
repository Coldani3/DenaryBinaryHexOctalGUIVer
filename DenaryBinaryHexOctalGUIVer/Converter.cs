using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenaryBinaryHexOctalGUIVer
{
    public class Converter
    {
        /// <summary>
        /// Converts a string input from a specified base to another specified base.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>The converted answer.</returns>
        public static string ConvertToBase(string input, Base from, Base to)
        {
            //The value converted to base 10
            int raw = 0;
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char letter = input[input.Length - i - 1];
                raw += ((Convert.ToInt32(letter) - (Char.IsDigit(letter) ? 48 : 55)) * (int)Math.Pow((int)from, i));
            }

            bool passedBeginningZeros = false;
            for (int i = 32; i >= 0; i--)
            {
                //How many times the current raw number divides by
                int divided = (int)Math.Floor(raw / Math.Pow((int)to, i));

                //Stop it turning into 0000000000000000001010 or something similar
                if (divided > 0) passedBeginningZeros = true;

                if (divided > 9) output.Append((char) ((int) divided + 55)); //append appropriate letter for > base 10 using the fact that A (10) is ASCII code 55 + 10 = 65 TODO: Figure out what to do for > base 36
                else if (passedBeginningZeros) output.Append(divided);

                //reduce the raw value
                raw -= (int)Math.Pow((int)to, i) * divided;
            }

            return output.ToString();
        }

        public static bool ValidateForBase(string input, Base numBase)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char letter = input.ToUpper()[i];
                if (Char.IsLetterOrDigit(letter))
                {
                    if (Char.IsDigit(letter))
                    {
                        if (Convert.ToInt32(letter) - 49 > ((int) numBase) - 1)
                        {
                            return false;
                        }
                    }
                    else if (Char.IsLetter(letter) && Convert.ToInt32(letter) - 49 > (65 + (int) numBase) /* 'F'*/)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Base
    {
        public static readonly Base BINARY = new Base(2);
        public static readonly Base OCTAL = new Base(8);
        public static readonly Base DENARY = new Base(10);
        public static readonly Base HEXADECIMAL = new Base(16);

        public int BaseNumber;

        public Base(int baseNum)
        {
            BaseNumber = baseNum;
        }

        public static explicit operator int(Base baseType) => baseType.BaseNumber;
    }
}
