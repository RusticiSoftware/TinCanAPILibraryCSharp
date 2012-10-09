using System;
using System.Security.Cryptography;

namespace RusticiSoftware.TinCanAPILibrary.Helper
{
    public class Encryption
    {
        /// <summary>
        /// // Hash an input string using SHA-1, return bytes
        /// </summary>
        /// <param name="input">The input string to hash</param>
        /// <returns></returns>
        public static byte[] GetSha1Hash(byte[] input)
        {
            SHA1 hasher = SHA1.Create();
            return hasher.ComputeHash(input);
        }
    }
}
