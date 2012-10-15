#region License
/*
Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
using System;
using System.Security.Cryptography;

namespace RusticiSoftware.TCAPIClientLibrary.Helper
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
