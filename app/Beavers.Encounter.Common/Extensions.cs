using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Beavers.Encounter.Common
{
    public static class Extensions
    {
        #region File

        public static void WriteToFile(this Stream stream, string path)
        {
            FileHelper.WriteStream(stream, path);
        }

        #endregion

        #region Security

        /// <summary>
        /// Hash utility - pass the hash algorithm name as a string i.e. SHA1, MD5 etc.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algorithm"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string HashIt(this string input, string algorithm, bool upperCase)
        {
            return SecurityHelper.HashIt(input, algorithm, upperCase);
        }

        /// <summary>
        /// Hash utility - pass the hash algorithm name as a string i.e. SHA1, MD5 etc.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algorithm"></param>
        /// <returns>Hashed String</returns>
        public static string HashIt(this string input, string algorithm)
        {
            return SecurityHelper.HashIt(input, algorithm);
        }
        #endregion Security
    }
}
