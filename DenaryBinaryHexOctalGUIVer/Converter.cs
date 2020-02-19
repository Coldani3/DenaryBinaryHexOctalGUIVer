using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenaryBinaryHexOctalGUIVer
{
    public class Converter
    {
        static Dictionary<int, char> HexConversions = new Dictionary<int, char>
        {
            { 10, 'A' },
            { 11, 'B' },
            { 12, 'C' },
            { 13, 'D' },
            { 14, 'E' },
            { 15, 'F' }
        };

        public static string ConvertToBase(string input, Base from, Base to)
        {
            int raw = 0;
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                raw += (Convert.ToInt32(input[input.Length - i - 1]) - 48) * (int)Math.Pow((int)from, i);
            }

            bool passedBeginningZeros = false;
            for (int i = 32; i >= 0; i--)
            {
                int divided = (int)Math.Floor(raw / Math.Pow((int)to, i));

                if (divided > 0) passedBeginningZeros = true;

                if (divided > 9) output.Append((char) ((int) divided + 55)/*HexConversions[divided]*/);
                else if (passedBeginningZeros) output.Append(divided);

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
                    else if (Char.IsLetter(letter) && Convert.ToInt32(letter) - 49 > 'F')
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
