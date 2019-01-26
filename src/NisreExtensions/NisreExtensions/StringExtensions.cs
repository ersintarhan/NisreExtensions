using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        public static DateTime DateTimeDefault = System.DateTime.Parse("1/1/1753 12:00:00");

        /// <summary>
        ///     Splits string to enumerable collection and yields the results;
        /// </summary>
        /// <param name="str">splited string</param>
        /// <param name="delimiter">delimiter</param>
        /// <returns>string iterative collection</returns>
        public static IEnumerable<string> SplitExt(this string str, char delimiter)
        {
            if (string.IsNullOrEmpty(str))
                yield break;

            int idx, startIdx = 0;
            do
            {
                idx = str.IndexOf(delimiter, startIdx);

                var len = idx >= 0 ? idx - startIdx : str.Length - startIdx;

                if (len > 0)
                    yield return str.Substring(startIdx, len);

                startIdx = idx + 1;
            } while (idx >= 0);
        }

        /// <summary>
        ///     Splits items of string collection to enumerable collection and yields the results
        /// </summary>
        /// <param name="list">item collections</param>
        /// <param name="delimiter">delimiter</param>
        /// <returns>string iterative collection</returns>
        public static IEnumerable<string> SplitExt(this IEnumerable<string> list, char delimiter)
        {
            if (list == null)
                yield break;

            foreach (var str in list)
            foreach (var item in str.SplitExt(delimiter))
                yield return item;
        }

        /// <summary>
        ///     Concatenates collection elemens to string using delimiter
        /// </summary>
        /// <typeparam name="T">collection items type</typeparam>
        /// <param name="list">collection</param>
        /// <param name="delimiter">delimiter</param>
        /// <returns>joined string</returns>
        public static string Join2String<T>(this IEnumerable<T> list, char delimiter)
        {
            return Join2String(list, delimiter.ToString());
        }

        /// <summary>
        ///     Concatenates collection elemens to string using delimiter
        /// </summary>
        /// <typeparam name="T">collection items type</typeparam>
        /// <param name="list">collection</param>
        /// <param name="delimiter">delimiter</param>
        /// <returns>joined string</returns>
        public static string Join2String<T>(this IEnumerable<T> list, string delimiter)
        {
            if (list == null)
                return string.Empty;


            var sb = new StringBuilder();
            foreach (var item in list)
            {
                if (item == null)
                    continue;

                sb.Append(item);
                sb.Append(delimiter);
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        ///     string.IsNullOrEmpty(value) counterpart
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>true -  IsNullOrEmpty</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Splits string to iterative collection by delimiter
        /// </summary>
        /// <param name="str">splited string</param>
        /// <param name="delimiter">delimiter</param>
        /// <param name="trim">true : trim values</param>
        /// <returns>string iterative collection</returns>
        public static IEnumerable<string> SplitExt(this string str, string delimiter, bool trim = true)
        {
            if (string.IsNullOrEmpty(str))
                yield break;

            int idx, startIdx = 0;
            do
            {
                idx = str.IndexOf(delimiter, startIdx, StringComparison.Ordinal);

                var len = idx >= 0 ? idx - startIdx : str.Length - startIdx;

                if (len > 0) yield return trim ? str.Substring(startIdx, len).Trim() : str.Substring(startIdx, len);

                startIdx = idx + delimiter.Length;
            } while (idx >= 0);
        }

        /// <summary>
        ///     Splits string to dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of dictionary key</typeparam>
        /// <typeparam name="TVal">Type of dictionary value</typeparam>
        /// <param name="str">string to split</param>
        /// <param name="entryDelimiter">delimiter of dictionary entries</param>
        /// <param name="keyValueDelimiter">delmiter of key|value</param>
        /// <param name="keySelector">key selector</param>
        /// <param name="valueSelector">value selector</param>
        /// <param name="trim">trim keys and value</param>
        /// <returns>dictionary</returns>
        public static IDictionary<TKey, TVal> SplitToDictionaryExt<TKey, TVal>(this string str,
            string entryDelimiter,
            string keyValueDelimiter,
            Func<string, TKey> keySelector,
            Func<string, TVal> valueSelector,
            bool trim = true
        )
        {
            IDictionary<TKey, TVal> result = new Dictionary<TKey, TVal>();

            str.SplitExt(entryDelimiter, trim).ForEach(entry =>
            {
                var keyValue = entry.SplitExt(keyValueDelimiter, trim).ToArray();
                result[keySelector(keyValue[0])] = valueSelector(keyValue[1]);
            });

            return result;
        }

        /// <summary>
        ///     Splits string to dictionary
        /// </summary>
        /// <typeparam name="TVal">Type of dictionary value</typeparam>
        /// <param name="str">string to split</param>
        /// <param name="entryDelimiter">delimiter of dictionary entries</param>
        /// <param name="keyValueDelimiter">delmiter of key|value</param>
        /// <param name="valueSelector">value selector</param>
        /// <param name="trim">trim keys and value</param>
        /// <returns>dictionary</returns>
        public static IDictionary<string, TVal> SplitToDictionaryExt<TVal>(this string str,
            string entryDelimiter,
            string keyValueDelimiter,
            Func<string, TVal> valueSelector,
            bool trim = true
        )
        {
            return str.SplitToDictionaryExt(entryDelimiter, keyValueDelimiter, x => x, valueSelector, trim);
        }

        /// <summary>
        ///     Splits string to dictionary
        /// </summary>
        /// <param name="str">string to split</param>
        /// <param name="entryDelimiter">delimiter of dictionary entries</param>
        /// <param name="keyValueDelimiter">delmiter of key|value</param>
        /// <param name="trim">trim keys and value</param>
        /// <returns>dictionary</returns>
        public static IDictionary<string, string> SplitToDictionaryExt(this string str,
            string entryDelimiter,
            string keyValueDelimiter,
            bool trim = true
        )
        {
            return str.SplitToDictionaryExt(entryDelimiter, keyValueDelimiter, x => x, x => x, trim);
        }


        public static DateTime DateTime(this object val)
        {
            return val.IsNull() ? DateTimeDefault
                : val is SqlDateTime ? ((SqlDateTime) val).Value
                : Convert.ToDateTime(val);
        }

        public static DateTime? DateTimeNull(this object val)
        {
            if (val.IsNull()) return null;

            if (val is SqlDateTime) return ((SqlDateTime) val).Value;

            return Convert.ToDateTime(val);
        }


        /// <summary>
        ///     Parses string to nullable int (Int32).
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>int (Int32) value if parse succeeds otherwise null.</returns>
        public static int? TryParseInt32(this string input)
        {
            int outValue;
            return int.TryParse(input, out outValue) ? (int?) outValue : null;
        }

        /// <summary>
        ///     Parses string to nullable long (Int64).
        /// </summary>
        /// <param name="input">Source string.</param>
        /// <returns>long (Int64) value if parse succeeds otherwise null.</returns>
        public static long? TryParseInt64(this string input)
        {
            long outValue;
            return long.TryParse(input, out outValue) ? (long?) outValue : null;
        }


        /// <summary>
        ///     Answers true if this String is neither null or empty.
        /// </summary>
        /// <param name="input">The string to check.</param>
        public static bool HasValue(this string input)
        {
            return !IsNullOrEmpty(input);
        }


        public static string ToAscii(this string unicode)
        {
            if (string.IsNullOrWhiteSpace(unicode)) return "";
            unicode = unicode.ToLower();
            unicode = Regex.Replace(unicode, "[-\\s+/]+", "-", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[áàảãạăắằẳẵặâấầẩẫậ]", "a", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[óòỏõọôồốổỗộơớờởỡợ]", "o", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[éèẻẽẹêếềểễệ]", "e", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[íìỉĩịIÍĨÌỈĨ]", "i", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[úùủũụưứừửữự]", "u", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[ýỳỷỹỵ]", "y", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "[đ]", "d", RegexOptions.Compiled);
            unicode = Regex.Replace(unicode, "\\W+", "-", RegexOptions.Compiled);
            return unicode;
        }

        /// <summary>
        ///     Creates URL / Html friendly slug
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="maxLength"></param>
        /// <param name="wordSeparator"></param>
        /// <returns></returns>
        public static string GenerateSlug(this string phrase, int maxLength = 50, string wordSeparator = "-")
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return string.Empty;

            // seperate words in camel case
            var charList = new List<char>();
            for (var pos = 0; pos < phrase.Length; pos++)
            {
                var ch1 = phrase[pos];
                charList.Add(ch1);

                if (pos < phrase.Length - 1)
                {
                    var ch2 = phrase[pos + 1];

                    if (char.IsLower(ch1) && (char.IsUpper(ch2) || char.IsNumber(ch2)) ||
                        char.IsUpper(ch1) && (char.IsNumber(ch2) || char.IsUpper(ch2)) ||
                        char.IsNumber(ch1) && (char.IsLower(ch2) || char.IsUpper(ch2)))
                        charList.Add('-');
                }
            }

            // remove accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(charList.ToArray());
            var str = Encoding.ASCII.GetString(bytes);
            // to lower case
            str = str.ToLowerInvariant();
            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // replace spaces with hyphens
            str = Regex.Replace(str, @"\s", "-");
            str = str.Replace("-", wordSeparator);
            // cut and trim it to maxLenght
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();

            return str;
        }

        public static string GenerateSlugWithoutHyphens(this string phrase, int maxLength = 50)
        {
            return phrase.GenerateSlug(maxLength, string.Empty);
        }


        /// <summary>
        ///     Returns the specified length of characters starting from the right
        /// </summary>
        /// <param name="intialString"></param>
        /// <param name="length">Length of characters to return</param>
        /// <returns></returns>
        public static string Right(this string intialString, int length)
        {
            //Check if the value is valid
            if (string.IsNullOrEmpty(intialString))
                intialString = string.Empty;
            else if (intialString.Length > length && length > 0)
                intialString = intialString.Substring(intialString.Length - length, length);

            //Return the string
            return intialString;
        }

        /// <summary>
        ///     Returns the base64encoded text
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="text">Text to be encoded</param>
        /// <returns></returns>
        public static string ToBase64(this Encoding encoding, string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        /// <summary>
        ///     Checks whether the string is null, string.Empty or whitespace
        /// </summary>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }


        /// <summary>
        ///     Checks whether the string can parse into a guid
        /// </summary>
        public static bool IsGuid(this string text)
        {
            Guid convertedGuid;
            return Guid.TryParse(text, out convertedGuid);
        }

        /// <summary>
        ///     Tries to parse string to Guid
        /// </summary>
        /// <exception cref="InvalidCastException">Failed to cast string</exception>
        public static Guid ToGuid(this string text)
        {
            Guid convertedGuid;
            var success = Guid.TryParse(text, out convertedGuid);
            if (!success) throw new InvalidCastException($"Failed casting {text} to Guid");
            return convertedGuid;
        }

        /// <summary>
        ///     Performs a super fast case incensitive string replace
        /// </summary>
        /// <param name="original">The original string</param>
        /// <param name="pattern">The string to search for</param>
        /// <param name="replacement">The string that will replace the search string</param>
        /// <returns>The original text with all replacements done</returns>
        public static string ReplaceCI(this string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            var upperString = original.ToUpper();
            var upperPattern = pattern.ToUpper();
            var inc = original.Length / pattern.Length * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];

            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (var i = position0; i < position1; ++i) chars[count++] = original[i];
                for (var i = 0; i < replacement.Length; ++i) chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }

            if (position0 == 0) return original;
            for (var i = position0; i < original.Length; ++i) chars[count++] = original[i];
            return new string(chars, 0, count);
        }


        /// <summary>
        ///     Converts the decimal to a currency string.
        ///     eg:12345.234  = R 12 345.23
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="symbol">Symbol to prepend</param>
        /// <returns></returns>
        public static string ToCurrency(this decimal value, string symbol = "$", bool prependSymbol = true)
        {
            var item = Math.Round(value, 2);

            var splitup = item.ToString().Split('.');

            var result = "";

            var count = 0;
            foreach (var character in splitup.FirstOrDefault().Reverse())
            {
                if (count++ % 3 == 0)
                    result += " ";

                result += character;
            }

            var chars = result.Reverse().ToArray();
            result = new string(chars).Trim();

            if (splitup.Count() > 1) result += "." + splitup.Last();

            return prependSymbol ? $"{symbol} {result}" : $"{result} {symbol}";
        }

        public static string TruncateLeft(this string value, int maxLength)
        {
            var startIndex = value.Length - maxLength;

            if (startIndex < 1) return value;

            return value.Substring(startIndex, maxLength);
        }

        public static string TruncateRight(this string value, int maxLength)
        {
            if (value.Length < maxLength) return value;

            return value.Substring(0, maxLength);
        }

        /// <summary>
        ///     Convert specified string to an <see cref="Enum" /> value.
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="value">The string to test.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>The converted <see cref="Enum" /> value.</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentException("Must specify valid information for parsing in the string.", nameof(value));
            var t = typeof(T);
            if (!t.IsEnum) throw new ArgumentException("Type provided must be an Enum.", "T");
            return (T) Enum.Parse(t, value, ignoreCase);
        }
        
        
        /// <summary>
        /// Applies the Levenshtein in Distance Algorithim, to find the proximity of two strings
        /// </summary>
        /// <param name="s">The first string to be compared</param>
        /// <param name="t">The other string to be compared</param>
        /// <returns>An integer representing how many characters have to be changed to string s be equal to t</returns>
        public static int LevenshteinDistance(this string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }
        
        /// <summary>
        ///     Converts string to its boolean equivalent
        /// </summary>
        /// <param name="value">string to convert</param>
        /// <returns>boolean equivalent</returns>
        /// <remarks>
        ///     <exception cref="ArgumentException">
        ///         thrown in the event no boolean equivalent found or an empty or whitespace
        ///         string is passed
        ///     </exception>
        /// </remarks>
        public static bool ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value");
            }
            string val = value.ToLower().Trim();
            switch (val)
            {
                case "false":
                    return false;
                case "f":
                    return false;
                case "true":
                    return true;
                case "t":
                    return true;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    throw new ArgumentException("Invalid boolean");
            }
        }

    }
}