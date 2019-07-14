using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using MyApp.Core.Extensions;

namespace MyApp.Core.Helpers
{
    public static class TypeConverterHelper
    {
        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Convert enum string to Enum Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static T GetEnumValue<T>(string enumName)
        {
           
            var values = Enum.GetNames(typeof(T));

            var selected = values.FirstOrDefault(value => value.EqualsIgnoreCase(enumName));

            if (selected == null)
                throw new ArgumentException($"Cannot Parse value '{enumName}' in {typeof(T).FullName} ");

            return (T)Enum.Parse(typeof(T), selected);

        }
    }
}