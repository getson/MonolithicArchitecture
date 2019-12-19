using BinaryOrigin.SeedWork.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BinaryOrigin.SeedWork.Core.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Get all the interface in the specified assembly that match the predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T[] GetTypesByInterface<T>(Assembly assembly, Func<Type, bool> predicate)
        {
            if (assembly == null) return new T[0];

            return assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(predicate))
                .Select(type => (T)Activator.CreateInstance(type))
                .ToArray();
        }

        /// <summary>
        /// Get all the types in the specified assembly that match the predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T[] GetTypes<T>(Assembly assembly, Func<Type, bool> predicate)
        {
            return assembly.GetTypes()
                           .Where(predicate)
                           .Select(type => (T)Activator.CreateInstance(type))
                           .ToArray();
        }

        /// <summary>
        /// Sets a property on an object to a value.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ValidationException(nameof(instance));
            if (propertyName == null) throw new ValidationException(nameof(propertyName));

            var instanceType = instance.GetType();
            var pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new GeneralException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            if (!pi.CanWrite)
                throw new GeneralException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = TypeConverterHelper.To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        /// <summary>
        /// Get private fields property value
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="fieldName">Field name</param>
        /// <returns>Value</returns>
        public static object GetPrivateFieldValue(object target, string fieldName)
        {
            if (target == null)
            {
                throw new ValidationException(nameof(target), "The assignment target cannot be null.");
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("The field name cannot be null or empty.", nameof(fieldName));
            }

            var t = target.GetType();
            FieldInfo fi = null;

            while (t != null)
            {
                fi = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fi != null) break;

                t = t.BaseType;
            }

            if (fi == null)
            {
                throw new GeneralException($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi.GetValue(target);
        }

        /// <summary>
        /// Find classes of type
        /// </summary>
        /// <param name="assignTypeFrom">Assign type from</param>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">A value indicating whether to find only concrete classes</param>
        /// <param name="ignoreReflectionErrors"></param>
        /// <returns>Result</returns>
        public static IEnumerable<Type> GetClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true, bool ignoreReflectionErrors = false)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    var types = GetTypesInAssembly(a, ignoreReflectionErrors);
                    if (types != null)
                    {
                        result.AddRange(GetClassesOfType(types, assignTypeFrom, onlyConcreteClasses));
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = ex.LoaderExceptions.Aggregate(string.Empty, (current, exception) => current + (exception.Message + Environment.NewLine));

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }

        /// <summary>
        /// Does type implement generic?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        public static bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        #region Private Methods

        private static IEnumerable<Type> GetClassesOfType(IEnumerable<Type> types, Type assignTypeFrom, bool onlyConcreteClasses)
        {
            var result = new List<Type>();
            foreach (var t in types)
            {
                if (!assignTypeFrom.IsAssignableFrom(t) && (!assignTypeFrom.IsGenericTypeDefinition ||
                                                            !DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                {
                    continue;
                }

                if (t.IsInterface)
                {
                    continue;
                }

                if (!onlyConcreteClasses)
                {
                    result.Add(t);
                }
                else
                {
                    if (t.IsClass && !t.IsAbstract)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }

        private static IEnumerable<Type> GetTypesInAssembly(Assembly a, bool ignoreReflectionErrors)
        {
            Type[] types = null;
            try
            {
                types = a.GetTypes();
            }
            catch
            {
                if (!ignoreReflectionErrors)
                {
                    throw;
                }
            }

            return types;
        }

        #endregion Private Methods
    }
}