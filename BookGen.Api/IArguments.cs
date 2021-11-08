using System;
using System.Collections.Generic;

namespace BookGen.Api
{
    /// <summary>
    /// Represents Arguments and values that are passed to the Script.
    /// Argument names are case insensitive.
    /// </summary>
    public interface IArguments : IReadOnlyCollection<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Argument names -> all lowercase
        /// </summary>
        IEnumerable<string> ArgumentNames { get; }

        /// <summary>
        /// Checks if the specified argument name is in the collection or not.
        /// </summary>
        /// <param name="name">Argument name to search</param>
        /// <returns>true, if argument name can be found, otherwise false</returns>
        bool HasArgument(string name);

        /// <summary>
        /// Get argument based on its index
        /// </summary>
        /// <param name="index">Index of argument to get</param>
        /// <returns>The argument value at the specified index</returns>
        string this[int index] { get; }

        /// <summary>
        /// Tries to get argument value and convert it to type.
        /// If argument not found fallback value will be returned
        /// </summary>
        /// <typeparam name="T">Specifies conversion target type. Must implement IConvertible</typeparam>
        /// <param name="argument">Argument to get</param>
        /// <param name="fallback">Fallback value if argument not found</param>
        /// <returns>Argument value converted to type if found, otherwise the fallback value</returns>
        T GetArgumentOrFallback<T>(string argument, T fallback) where T : IConvertible;

        /// <summary>
        /// Get argument value and convert it to type.
        /// If argument not found an exception will be thrown
        /// </summary>
        /// <typeparam name="T">Specifies conversion target type. Must implement IConvertible</typeparam>
        /// <param name="argument">Argument to get</param>
        /// <returns>Argument value converted to type.</returns>
        /// <exception cref="ArgumentException"></exception>
        T GetArgumentOrThrow<T>(string argument) where T : IConvertible;
    }
}
