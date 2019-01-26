using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    ///     Common extensions of <see cref="byte" />.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        ///     Convert specified byte to an <see cref="Enum" /> value.
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="value">The byte value.</param>
        /// <returns>The converted <see cref="Enum" /> value.</returns>
        public static T ToEnum<T>(this byte value) where T : struct
        {
            return (T) Enum.ToObject(typeof(T), value);
        }


        public static string AsByteString(this IEnumerable<byte> bytes, string prefix = "0x")
        {
            return string.Join(",", bytes.Select(b => prefix + b.ToString("X2")));
        }
    }
}