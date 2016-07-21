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
        public static readonly byte STARTBYTE = 10;
        private static readonly byte FCS_BYTES = 2;

        /// <summary>
        /// Converts byte array to 16 bit integers
        /// </summary>
        public static Int16[] ConvertFromByte(byte[] bytes)
        {
            // checking if fcs is right otherwise the frame will be thrown
            if (CheckFcsEquality(bytes) == true)
            {
                Int16[] numbers = new Int16[bytes.Length / 2];
                int count = 0;

                // convert bytes to data
                for (int i = 1; i < bytes.Length - FCS_BYTES; i += 2)
                {
                    // Converts 2 bytes into unsigned int16
                    UInt16 number = GetDecimal(GetBinary("", bytes[i + 1]) + GetBinary("", bytes[i]), 0, 0);
                    // if the unsigned int is greater than the max value of signed int
                    // then the number is negative
                    numbers[count] = (Int16)(number > Int16.MaxValue ? (UInt16.MaxValue - number + 1) * -1 : number);
                    count++;
                }
                return numbers;
            }
            return null;
        }

        /// <summary>
        /// Calculates FCS and compares with sended FCS
        /// </summary>
        private static bool CheckFcsEquality(byte[] bytes)
        {
            UInt16 calculatedFcs = GetOneComplementOfBinary(GetBinary("", GetCheckSequence(bytes)), 0, 0);
            UInt16 messageFcs = GetDecimal(GetBinary("", bytes[bytes.Length - 1]) + GetBinary("", bytes[bytes.Length - 2]), 0, 0);
            return calculatedFcs == messageFcs;
        }

        /// <summary>
        /// Converts Data From Joystick to bytes
        /// </summary>
        public static byte[] ConvertToByte(params Int16[] args)
        {
            byte[] bytes = new byte[11];
            // Set the start byte
            bytes[0] = STARTBYTE;

            // Convert joystick data to bytes
            int posi = 1;
            for (int i = 0; i < args.Length; i++)
            {
                short s = args[i];
                byte x = (byte)s;
                bytes[posi++] = x;

                short s2 = (short)(s >> 8);
                byte x2 = (byte)s2;
                bytes[posi++] = x2;
            }

            // Calculate Frame Check Sequence (2 Bytes)
            UInt16 fcs = GetOneComplementOfBinary(GetBinary("", GetCheckSequence(bytes)), 0, 0);
            bytes[9] = (byte)fcs;
            bytes[10] = (byte)(fcs >> 8);

            return bytes;
        }

        /// <summary>
        /// Calculate 1 complement of all bytes (only 8 bit) in array
        /// </summary>
        private static UInt16 GetCheckSequence(byte[] bytes)
        {
            UInt16 number = 0;

            for (int i = 1; i < bytes.Length - FCS_BYTES; i++)
            {
                number += (UInt16)(255 - bytes[i]);
            }

            return number;
        }

        /// <summary>
        /// Calculates 1 Complement of binary number
        /// </summary>
        /// <param name="binary">Binary number in string format</param>
        /// <param name="position">Has to be 0</param>
        /// <param name="result">Has to be 0</param>
        /// <returns>16 bit integer</returns>
        public static UInt16 GetOneComplementOfBinary(string binary, int position, UInt16 result)
        {
            result += (UInt16)(binary[binary.Length - position - 1] == '0' ? Math.Pow(2, position) : 0);
            return position + 1 < binary.Length ? GetDecimal(binary, position + 1, result) : result;
        }

        /// <summary>
        /// Returns binary number in string format
        /// </summary>
        /// <param name="binary">Has to be ""</param>
        /// <param name="number">Positive Decimal</param>
        public static string GetBinary(string binary, int number)
        {
            if (number != 0)
            {
                binary = (number % 2) + binary;
                return number / 2 == 1 ? "1" + binary : GetBinary(binary, number / 2);
            }
            return "".PadLeft(8, '0');
        }

        /// <summary>
        /// Calculates 16 bit integer of binary
        /// </summary>
        /// <param name="binary">binary in string format (max 16 chars)</param>
        /// <param name="position">has to be 0</param>
        /// <param name="result">has to be 0</param>
        /// <returns></returns>
        private static UInt16 GetDecimal(string binary, int position, UInt16 result)
        {
            result += (UInt16)(binary[binary.Length - position - 1] == '1' ? Math.Pow(2, position) : 0);
            return position + 1 < binary.Length ? GetDecimal(binary, position + 1, result) : result;
        }
    }
}