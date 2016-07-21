using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BluetoothController
{
    public class ByteConverter
    {
        
        public static Int16[] ConvertFromByte(byte[] bytes)
        {
            Int16[] numbers = new Int16[bytes.Length / 2];
            int count = 0;

            for (int i = 0; i < bytes.Length; i += 2)
            {
                UInt16 number = GetDecimal(GetBinary("", bytes[i + 1]) + GetBinary("", bytes[i]), 0, 0);
                numbers[count] = (Int16)(number > Int16.MaxValue ? (UInt16.MaxValue - number + 1) * -1 : number);
                Console.WriteLine(numbers[count]);
                count++;
            }

            return numbers;
        }

        public static byte[] ConvertToByte(params Int16[] args)
        {
            byte[] bytes = new byte[8];
            int posi = 0;
            for (int i = 0; i < args.Length; i++)
            {
                short s = args[i];
                byte x = (byte)s;
                bytes[posi++] = x;

                short s2 = (short)(s >> 8);
                byte x2 = (byte)s2;
                bytes[posi++] = x2;
            }
            return bytes;
        }

        public static string GetBinary(string binary, int number)
        {
            if (number != 0)
            {
                binary = (number % 2) + binary;
                return number / 2 == 1 ? "1" + binary : GetBinary(binary, number / 2);
            }
            return "0";
        }

        private static UInt16 GetDecimal(string binary, int position, UInt16 result)
        {
            result += (UInt16)(binary[binary.Length - position - 1] == '1' ? Math.Pow(2, position) : 0);
            return position + 1 < binary.Length ? GetDecimal(binary, position + 1, result) : result;
        }
    }
}