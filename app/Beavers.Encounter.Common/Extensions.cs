using System;
using System.Collections;
using System.IO;
using System.Reflection;
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

        #region Exception
        
        public static string Expand(this Exception e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Exception scanned_exeption = e;
            Exception ex = new Exception(e.Message, e.InnerException);

            while (scanned_exeption != null)
            {
                try
                {
                    // Для удобочитаемоси выделяем тип исключения
                    stringBuilder.Append("---------- ");
                    stringBuilder.Append(scanned_exeption.GetType().ToString());
                    stringBuilder.AppendLine(" ----------");

                    PropertyInfo[] props = scanned_exeption.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    foreach (PropertyInfo pi in props)
                    {
                        // Выводим значения не только строковых полей, но и знечения других полезных свойств-значений.
                        if (pi.PropertyType.IsAssignableFrom(typeof(string)) || pi.PropertyType.IsValueType)
                        {
                            try
                            {
                                object value = pi.GetValue(scanned_exeption, null).ToString();
                                stringBuilder.AppendLine(String.Format("{0}={1}", pi.Name, (value == null ? "" : value.ToString())));
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (scanned_exeption.Data != null && scanned_exeption.Data.Count > 0)
                    {
                        stringBuilder.AppendLine("Data items:");
                        foreach (DictionaryEntry dataItem in scanned_exeption.Data)
                        {
                            stringBuilder.AppendLine(String.Format("  key: {0}, value: {1}", dataItem.Key, dataItem.Value));
                        }
                    }
                }
                catch
                {
                    stringBuilder.AppendLine(string.Format("********** Исключительная ситуация при обработке исключительной ситуации. *********** {0}", e.GetType().ToString()));
                }
                scanned_exeption = scanned_exeption.InnerException;

                // Добавляем разрыв для отделения вложенных исключений друг от друга.
                stringBuilder.AppendLine();
            }
            try
            {
                stringBuilder.Append("Метод:" + ex.TargetSite.ReflectedType + "." + ex.TargetSite.Name);
            }
            catch
            {
                // Здесь иногда водникает исключение - просто глушим его.
            }

            stringBuilder.AppendLine("Пользователь: " + System.Threading.Thread.CurrentPrincipal.Identity.Name);

            return stringBuilder.ToString(); 
        }

        #endregion Exception
    }
}
