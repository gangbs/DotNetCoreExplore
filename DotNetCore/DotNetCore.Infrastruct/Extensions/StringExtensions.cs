using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.Extensions
{
   public static class StringExtensions
    {
        public static string Byte2String(this byte[] array)
        {
            //string str = System.Text.UTF8Encoding.Default.GetString(array);
            return Encoding.UTF8.GetString(array);

        }

        public static byte[] String2Byte(this string str)
        {
           return Encoding.UTF8.GetBytes(str);
        }
    }
}
