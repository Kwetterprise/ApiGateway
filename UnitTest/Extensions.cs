using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest
{
    using System.Collections;
    using System.Linq;
    using Xunit;

    public static class Extensions
    {
        public static string ToJson(params (string Key, string Value)[] data)
        {
            var keyValuePairs = data.Select(x => new KeyValuePair<string, string>(x.Key, x.Value));
            return new Dictionary<string, string>(keyValuePairs).ToJson();
        }

        public static Dictionary<string, string> ToJson(this Dictionary<string, Dictionary<string, string>> dict)
        {
            return dict.ToDictionary(x => x.Key, x => x.Value.ToJson());
        }

        public static string ToJson(this Dictionary<string, string> dict)
        {
            return System.Text.Json.JsonSerializer.Serialize(dict);
        }

        #region Asserting

        public static T AssertSingle<T>(this IEnumerable<T> collection)
        {
            return Assert.Single(collection);
        }

        public static T AssertSingle<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            return Assert.Single(collection, predicate);
        }

        public static TOut AssertIsType<TOut>(this object @in)
        {
            return Assert.IsType<TOut>(@in);
        }

        public static void AssertEqual<T>(this T actual, T expected)
        {
            Assert.Equal(expected, actual);
        }

        public static T? NullableSingleOrDefault<T>(this IEnumerable<T> source, Predicate<T> predicate)
            where T: struct
        {
            T? found = null;
            foreach (var variable in source)
            {
                if (predicate(variable))
                {
                    if (found != null)
                    {
                        throw new InvalidOperationException($"More than one element satisfies the condition in {nameof(predicate)}.");
                    }

                    found = variable;
                }
            }

            if (found == null)
            {
                throw new InvalidOperationException($"No element satisfies the condition in {nameof(predicate)}.");
            }

            return found;
        }

        public static void AssertEmpty(this IEnumerable source)
        {
            Assert.Empty(source);
        }

        #endregion
    }
}
