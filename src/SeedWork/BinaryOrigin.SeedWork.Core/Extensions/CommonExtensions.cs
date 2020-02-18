using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Common extensions
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// Is null or default
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="value">Value to evaluate</param>
        /// <returns>Result</returns>
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static bool IsNullOrDefault<T>(this T value) where T : struct
        {
            return default(T).Equals(value);
        }
    }
}