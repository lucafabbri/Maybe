using System.Collections.Generic;
using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Provides Maybe-based wrapper methods for safe collection access operations.
/// </summary>
public static class CollectionToolkit
{
    /// <summary>
    /// Attempts to get a value from a dictionary by key, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
    /// <param name="dictionary">The dictionary to access.</param>
    /// <param name="key">The key to look up.</param>
    /// <returns>A Maybe containing the value if found or a CollectionError.</returns>
    public static Maybe<TValue, CollectionError> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary == null)
        {
            return new CollectionError(key!, new ArgumentNullException(nameof(dictionary)), "Dictionary cannot be null");
        }

        if (key == null)
        {
            return new CollectionError(key!, new ArgumentNullException(nameof(key)), "Key cannot be null");
        }

        try
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return Maybe<TValue, CollectionError>.Some(value);
            }
            else
            {
                return new CollectionError(key, new KeyNotFoundException($"Key '{key}' was not found in the dictionary"), $"Key '{key}' was not found in the dictionary");
            }
        }
        catch (ArgumentException ex)
        {
            return new CollectionError(key, ex, $"Invalid key type for dictionary: {key}");
        }
        catch (Exception ex)
        {
            return new CollectionError(key, ex, $"Unexpected error accessing dictionary with key: {key}");
        }
    }

    /// <summary>
    /// Attempts to get a value from a read-only dictionary by key, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
    /// <param name="dictionary">The read-only dictionary to access.</param>
    /// <param name="key">The key to look up.</param>
    /// <returns>A Maybe containing the value if found or a CollectionError.</returns>
    public static Maybe<TValue, CollectionError> TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary == null)
        {
            return new CollectionError(key!, new ArgumentNullException(nameof(dictionary)), "Dictionary cannot be null");
        }

        if (key == null)
        {
            return new CollectionError(key!, new ArgumentNullException(nameof(key)), "Key cannot be null");
        }

        try
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return Maybe<TValue, CollectionError>.Some(value);
            }
            else
            {
                return new CollectionError(key, new KeyNotFoundException($"Key '{key}' was not found in the dictionary"), $"Key '{key}' was not found in the dictionary");
            }
        }
        catch (ArgumentException ex)
        {
            return new CollectionError(key, ex, $"Invalid key type for dictionary: {key}");
        }
        catch (Exception ex)
        {
            return new CollectionError(key, ex, $"Unexpected error accessing dictionary with key: {key}");
        }
    }

    /// <summary>
    /// Attempts to get an element from a list by index, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="list">The list to access.</param>
    /// <param name="index">The index to access.</param>
    /// <returns>A Maybe containing the element if found or a CollectionError.</returns>
    public static Maybe<T, CollectionError> TryGetAt<T>(this IList<T> list, int index)
    {
        if (list == null)
        {
            return new CollectionError(index, new ArgumentNullException(nameof(list)), "List cannot be null");
        }

        try
        {
            if (index < 0 || index >= list.Count)
            {
                return new CollectionError(index, new ArgumentOutOfRangeException(nameof(index)), $"Index {index} is out of range for list of length {list.Count}");
            }

            var element = list[index];
            return Maybe<T, CollectionError>.Some(element);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return new CollectionError(index, ex, $"Index {index} is out of range for list of length {list.Count}");
        }
        catch (Exception ex)
        {
            return new CollectionError(index, ex, $"Unexpected error accessing list at index: {index}");
        }
    }

    /// <summary>
    /// Attempts to get an element from a read-only list by index, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="list">The read-only list to access.</param>
    /// <param name="index">The index to access.</param>
    /// <returns>A Maybe containing the element if found or a CollectionError.</returns>
    public static Maybe<T, CollectionError> TryGetAt<T>(this IReadOnlyList<T> list, int index)
    {
        if (list == null)
        {
            return new CollectionError(index, new ArgumentNullException(nameof(list)), "List cannot be null");
        }

        try
        {
            if (index < 0 || index >= list.Count)
            {
                return new CollectionError(index, new ArgumentOutOfRangeException(nameof(index)), $"Index {index} is out of range for list of length {list.Count}");
            }

            var element = list[index];
            return Maybe<T, CollectionError>.Some(element);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return new CollectionError(index, ex, $"Index {index} is out of range for list of length {list.Count}");
        }
        catch (Exception ex)
        {
            return new CollectionError(index, ex, $"Unexpected error accessing list at index: {index}");
        }
    }

    /// <summary>
    /// Attempts to get an element from an array by index, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="array">The array to access.</param>
    /// <param name="index">The index to access.</param>
    /// <returns>A Maybe containing the element if found or a CollectionError.</returns>
    public static Maybe<T, CollectionError> TryGetAt<T>(this T[] array, int index)
    {
        if (array == null)
        {
            return new CollectionError(index, new ArgumentNullException(nameof(array)), "Array cannot be null");
        }

        try
        {
            if (index < 0 || index >= array.Length)
            {
                return new CollectionError(index, new ArgumentOutOfRangeException(nameof(index)), $"Index {index} is out of range for array of length {array.Length}");
            }

            var element = array[index];
            return Maybe<T, CollectionError>.Some(element);
        }
        catch (IndexOutOfRangeException ex)
        {
            return new CollectionError(index, ex, $"Index {index} is out of range for array of length {array.Length}");
        }
        catch (Exception ex)
        {
            return new CollectionError(index, ex, $"Unexpected error accessing array at index: {index}");
        }
    }

    /// <summary>
    /// Attempts to get the first element from a sequence, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the sequence elements.</typeparam>
    /// <param name="source">The sequence to access.</param>
    /// <returns>A Maybe containing the first element if found or a CollectionError.</returns>
    public static Maybe<T, CollectionError> TryFirst<T>(this IEnumerable<T> source)
    {
        if (source == null)
        {
            return new CollectionError("first", new ArgumentNullException(nameof(source)), "Source cannot be null");
        }

        try
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return Maybe<T, CollectionError>.Some(enumerator.Current);
            }
            else
            {
                return new CollectionError("first", new InvalidOperationException("Sequence contains no elements"), "Sequence contains no elements");
            }
        }
        catch (InvalidOperationException ex)
        {
            return new CollectionError("first", ex, "Sequence contains no elements");
        }
        catch (Exception ex)
        {
            return new CollectionError("first", ex, "Unexpected error getting first element from sequence");
        }
    }

    /// <summary>
    /// Attempts to get the last element from a sequence, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the sequence elements.</typeparam>
    /// <param name="source">The sequence to access.</param>
    /// <returns>A Maybe containing the last element if found or a CollectionError.</returns>
    public static Maybe<T, CollectionError> TryLast<T>(this IEnumerable<T> source)
    {
        if (source == null)
        {
            return new CollectionError("last", new ArgumentNullException(nameof(source)), "Source cannot be null");
        }

        try
        {
            if (source is IList<T> list)
            {
                if (list.Count > 0)
                {
                    return Maybe<T, CollectionError>.Some(list[list.Count - 1]);
                }
            }
            else
            {
                using var enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                {
                    return new CollectionError("last", new InvalidOperationException("Sequence contains no elements"), "Sequence contains no elements");
                }

                T last = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    last = enumerator.Current;
                }
                return Maybe<T, CollectionError>.Some(last);
            }

            return new CollectionError("last", new InvalidOperationException("Sequence contains no elements"), "Sequence contains no elements");
        }
        catch (InvalidOperationException ex)
        {
            return new CollectionError("last", ex, "Sequence contains no elements");
        }
        catch (Exception ex)
        {
            return new CollectionError("last", ex, "Unexpected error getting last element from sequence");
        }
    }
}