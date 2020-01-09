using BinaryOrigin.SeedWork.Core.Exceptions;
using System;
using System.Diagnostics;

namespace BinaryOrigin.SeedWork.Core.Extensions
{
    [DebuggerStepThrough]
    public static class GuardExtensions
    {
        public static void ThrowIfNullOrEmpty(this string value, string argument)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new GeneralException(argument);
        }

        public static void ThrowIfNullOrEmpty(this string value, string argument, string message)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new GeneralException(argument, message);
        }

        public static void ThrowIfNull(this object @object, string argument)
        {
            if (@object == null) throw new GeneralException(argument);
        }

        public static void ThrowIfNull(this object @object, string argument, string message)
        {
            if (@object == null) throw new GeneralException(argument, message);
        }

        public static void ThrowIfNotAllowed<T>(this T @value, T notAllowedValue, string argument, string message)
            where T : struct
        {
            if (@value.Equals(notAllowedValue))
            {
                throw new ArgumentException(argument, message);
            }
        }
    }
}