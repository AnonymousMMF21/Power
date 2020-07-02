using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KeLi.Power.Tool.Extensions
{
    /// <summary>
    ///     Linq extension.
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        ///     Clones the specified object by deeply for entity model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeeplyClone(this object obj)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (obj is null)
                return null;

            var type = obj.GetType();

            if (type.IsAbstract)
                return obj;

            if (type.IsValueType || type == typeof(string))
                return obj;

            #region Not Support Types

            var interfaces = type.GetInterfaces();

            if (obj.GetType().Name.Contains("Tuple"))
                throw new NotSupportedException(typeof(Tuple).Name);

            var isKeyValuePair = interfaces.Where(w => w.IsGenericType).Any(a => a.GetGenericTypeDefinition() == typeof(KeyValuePair<,>));

            if (isKeyValuePair)
                throw new NotSupportedException(typeof(KeyValuePair<,>).Name);

            var isLookup = interfaces.Where(w => w.IsGenericType).Any(a => a.GetGenericTypeDefinition() == typeof(ILookup<,>));

            if (isLookup)
                throw new NotSupportedException(typeof(ILookup<,>).Name);

            #endregion Not Support Types

            #region Clones IList

            if (interfaces.Contains(typeof(IList)) && !interfaces.Contains(typeof(IDictionary)))
            {
                var items = obj as IList;

                // Must new object.
                var results = Activator.CreateInstance(obj.GetType()) as IList;

                if (results is null)
                    throw new NullReferenceException(nameof(results));

                if (items is null)
                    return results;

                foreach (var item in items)
                    results.Add(item.DeeplyClone());

                return results;
            }

            #endregion

            #region Clones IDictionary

            if (interfaces.Contains(typeof(IDictionary)))
            {
                var items = obj as IDictionary;

                // Must new object.
                var results = Activator.CreateInstance(obj.GetType()) as IDictionary;

                if (items is null)
                    return results;

                if (results is null)
                    throw new NullReferenceException(nameof(results));

                foreach (var key in items.Keys)
                {
                    if (results.Contains(key))
                        results[key] = items[key].DeeplyClone();

                    else
                        results.Add(key.DeeplyClone(), items[key].DeeplyClone());
                }

                return results;
            }

            #endregion Clones IDictionary

            var constructors = type.GetConstructors(flags);
            var constructor = constructors.FirstOrDefault();

            if (constructor != null)
            {
                #region Inits object

                var constructorParms = constructor.GetParameters();
                object[] parmValues = null;

                if (constructorParms.Length > 0)
                {
                    var tmpParmValues = new List<object>();

                    foreach (var constructorParm in constructorParms)
                    {
                        var parmType = constructorParm.ParameterType;
                        var parmValue = parmType.IsValueType ? Activator.CreateInstance(parmType) : null;

                        tmpParmValues.Add(parmValue);
                    }

                    parmValues = tmpParmValues.ToArray();
                }

                var result = Activator.CreateInstance(type, flags, null, parmValues, null);

                #endregion Inits object

                #region Sets Fields

                var fields = type.GetFields(flags);

                foreach (var field in fields)
                {
                    var fieldValue = field.GetValue(obj);

                    if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
                        field.SetValue(result, fieldValue);

                    else
                        field.SetValue(result, DeeplyClone(fieldValue));
                }

                #endregion Sets Fields

                #region Sets Properties

                var properties = type.GetProperties(flags);

                foreach (var property in properties)
                {
                    if (!property.CanWrite)
                        continue;

                    if (!property.CanRead)
                        continue;

                    var propertyValue = property.GetValue(obj, null);

                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                        property.SetValue(result, propertyValue, null);

                    else
                        property.SetValue(result, DeeplyClone(propertyValue), null);
                }

                #endregion Sets Properties

                return result;
            }

            return null;
        }

        /// <summary>
        ///     Returns the min element of a sequence.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static TSource MinSource<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source is null)
                return default;

            if (selector is null)
                return default;

            return source.MinSourceWithKey(selector, comparer).Value;
        }

        /// <summary>
        ///     Returns the min KeyValuePair of a sequence.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TSource> MinSourceWithKey<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source is null)
                return default;

            if (selector is null)
                return default;

            var minItem = source.FirstOrDefault();

            if (minItem == null)
                return default;

            var minKey = selector.Invoke(minItem);

            comparer = comparer ?? Comparer<TKey>.Default;

            foreach (var item in source)
            {
                var key = selector.Invoke(item);

                if (comparer.Compare(minKey, key) > 0)
                {
                    minItem = item;
                    minKey = key;
                }
            }

            return new KeyValuePair<TKey, TSource>(minKey, minItem);
        }

        /// <summary>
        ///     Returns the max element of a sequence.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static TSource MaxSource<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source is null)
                return default;

            if (selector is null)
                return default;

            return source.MaxSourceWithKey(selector, comparer).Value;
        }

        /// <summary>
        ///     Returns the max KeyValuePair of a sequence.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TSource> MaxSourceWithKey<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source is null)
                return default;

            if (selector is null)
                return default;

            var maxItem = source.FirstOrDefault();

            if (maxItem == null)
                return default;

            var maxKey = selector.Invoke(maxItem);

            comparer = comparer ?? Comparer<TKey>.Default;

            foreach (var item in source)
            {
                var key = selector.Invoke(item);

                if (comparer.Compare(maxKey, key) < 0)
                {
                    maxItem = item;
                    maxKey = key;
                }
            }

            return new KeyValuePair<TKey, TSource>(maxKey, maxItem);
        }
    }
}