using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XmlDataExtractManager.Helpers
{
    public static class ExtractionMethods
    {
        public static string CleanUpString(this String strInput)
        {
            return (!String.IsNullOrWhiteSpace(strInput))
                       ? strInput.Trim().Replace(" ", "").Replace("-", "").Replace("/", "").Replace(",", "").ToUpper()
                       : null;
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static T GetAs<T>(this XElement element, T defaultValue = default(T))
        {
            T ret = defaultValue;

            if (!string.IsNullOrEmpty(element?.Value))
            {
                // Cast to Return Data Type 
                // NOTE: ChangeType does NOT work with Nullable Types
                ret = (T)Convert.ChangeType(element.Value, typeof(T));
            }
            return ret;
        }

        public static string ToNullableString(this String str, int iMaxLength = 0)
        {
            string strBuffer = String.IsNullOrWhiteSpace(str) ? null : str.Trim();

            if (iMaxLength > 0 && strBuffer != null && strBuffer.Length > iMaxLength)
                strBuffer = strBuffer.Substring(0, iMaxLength);
            return strBuffer;
        }

        public static DateTime? ToNullableDateTime(this String str)
        {
            if (str == null || str.Equals(String.Empty))
                return null;
            try
            {
                return Convert.ToDateTime(str);
            }
            catch
            {
                return null;
            }
        }

        public static int? ToNullableInt(this String str)
        {
            if (str == null || str.Equals(String.Empty))
                return null;
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
                return null;
            }
        }

        public static decimal? ToNullableDecimal(this String str)
        {
            if (str == null || str.Equals(String.Empty))
                return null;
            try
            {
                if (str.Contains(",")) str = str.Replace(".", "").Replace(",", ".");
                return Convert.ToDecimal(str, NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                return null;
            }
        }

        public static void PropertyMap<T, U>(T source, U destination)
            where T : class, new()
            where U : class, new()
        {
            List<PropertyInfo> sourceProperties = source.GetType().GetProperties().ToList<PropertyInfo>();
            List<PropertyInfo> destinationProperties = destination.GetType().GetProperties().ToList<PropertyInfo>();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);

                if (destinationProperty != null)
                {
                    try
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }
    }
}
