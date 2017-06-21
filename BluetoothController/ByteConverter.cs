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
        public static readonly byte STARTBYTE = 0x00;
        private static readonly byte FCS_BYTES = 2;
        private static readonly byte PACKET_SIZE = 19;
        private static int count = 0;

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
            UInt16 oneComplement = GetCheckSequence(bytes);
            oneComplement += 1;
            byte firstByte = (byte)oneComplement;
            byte secondByte = (byte)(oneComplement >> 8);
            return (firstByte == bytes[bytes.Length - 2] && secondByte == bytes[bytes.Length - 1]);
        }

        /// <summary>
        /// Converts Data From Joystick to bytes
        /// </summary>
        public static byte[] ConvertToByte(params Int16[] args)
        {
            byte[] b = new byte[PACKET_SIZE];
            //byte speed = (byte) args[0];
            byte heightcontrol = 0;
            //int azimuth = Java.Lang.Float.FloatToIntBits(args[1]);
            //int pitch = Java.Lang.Float.FloatToIntBits(args[2]);
            //int roll = Java.Lang.Float.FloatToIntBits(args[3]);
            byte speed = (byte) args[0];
            int azimuth = 0;
            int pitch = 0;
            int roll = 0;
           
            //string str = string.Format("Speed: {0} HeightControl: {1} Azimuth: {2} Pitch: {3} Roll: {4}", speed,
            //        heightcontrol, azimuth, pitch, roll);
            //Console.WriteLine(str);
            int checksum = STARTBYTE;
            checksum ^= (heightcontrol << 8 | speed) & 0xFFFF;
            checksum ^= azimuth;
            checksum ^= pitch;
            checksum ^= roll;

            b[0] = STARTBYTE;

            b[1] = (byte)(heightcontrol & 0xFF);
            b[2] = (byte)(speed & 0xFF);

            b[3] = (byte)((azimuth >> 24) & 0xFF);
            b[4] = (byte)((azimuth >> 16) & 0xFF);
            b[5] = (byte)((azimuth >> 8) & 0xFF);
            b[6] = (byte)(azimuth & 0xFF);

            b[7] = (byte)((pitch >> 24) & 0xFF);
            b[8] = (byte)((pitch >> 16) & 0xFF);
            b[9] = (byte)((pitch >> 8) & 0xFF);
            b[10] = (byte)(pitch & 0xFF);

            b[11] = (byte)((roll >> 24) & 0xFF);
            b[12] = (byte)((roll >> 16) & 0xFF);
            b[13] = (byte)((roll >> 8) & 0xFF);
            b[14] = (byte)(roll & 0xFF);

            b[15] = (byte)((checksum >> 24) & 0xFF);
            b[16] = (byte)((checksum >> 16) & 0xFF);
            b[17] = (byte)((checksum >> 8) & 0xFF);
            b[18] = (byte)(checksum & 0xFF);

            return b;
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
            return position + 1 < binary.Length ? GetOneComplementOfBinary(binary, position + 1, result) : result;
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