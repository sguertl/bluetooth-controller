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
        
        public static void ConvertFromByte(byte[] bytes)
        {
            
        }

        public static byte[] ConvertToByte(params Int16[] args)
        {
            byte[] bytes = new byte[4];
            int posi = 0;

            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    string binaryString = GetBinary("", args[i]);
            //    Int16 binary = Convert.ToInt16(binaryString.PadLeft(16 - binaryString.Length, '0'));
            //    byte currentByte = (byte) binary;
            //    bytes[i] = currentByte;
            //    i++;
            //    currentByte <<= args[i];
            //    bytes[i] = currentByte;
            //}

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

        private static string GetBinary(string binary, int number)
        {
            binary = (number % 2) + binary;
            return number / 2 == 1 ? "1" + binary : GetBinary(binary, number / 2);
        }
    }
}