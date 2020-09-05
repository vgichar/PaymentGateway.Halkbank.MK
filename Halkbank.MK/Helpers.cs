using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Halkbank.MK
{
    internal static class Helpers
    {
        /// <summary>
        /// Halkbank encoding
        /// </summary>
        public static string HalkbankEncoding => "ISO-8859-9";

        /// <summary>
        /// Generate random string with specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandom(int length)
        {
            string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
            var bytes = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
        }

        /// <summary>
        /// Encrypt imput bytes using SHA1 algorithm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ToSha1(byte[] input)
        {
            SHA1 sha = SHA1.Create();
            byte[] hash = sha.ComputeHash(input);

            return hash;
        }

        /// <summary>
        /// Encode string using prvided encoding code
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encode(string input, string encoding)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(encoding).GetBytes(input);
        }


        /// <summary>
        /// Encode string using prvided encoding
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encode(string input, Encoding encoding)
        {
            return encoding.GetBytes(input);
        }


        /// <summary>
        /// Convert bytes to base64 string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToBase64String(byte[] input)
        {

            return Convert.ToBase64String(input);
        }
    }
}
