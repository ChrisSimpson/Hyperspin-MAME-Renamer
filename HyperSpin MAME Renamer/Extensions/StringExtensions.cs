using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Renamer.Extensions
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        #region Private Fields

        private static IEnumerable<String> _ignoreWords = new string[] { "THE", "AND", "&", "OF", "IN", "TO", String.Empty };

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Clean a string
        /// </summary>
        /// <param name="value">The value to be cleaned</param>
        /// <returns>The cleaned string value</returns>
        public static string Clean(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = Regex.Replace(value, @"\(.*?\)", String.Empty);
            value = Regex.Replace(value, @"\[.*?\]", String.Empty);
            value = Regex.Replace(value, @"\s{2,}", " ");
            value = value.Replace("+", " ");

            var punctuation = value.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = value.Split().Select(w => w.Trim(punctuation).ToUpperInvariant()).Where(w => !_ignoreWords.Contains(w)).ToArray();

            for (int word = 0; word < words.Count(); word++)
            {
                if (Regex.IsMatch(words[word], @"^\d+$"))
                {
                    words[word] = words[word].ToRoman();
                }
            }

            return String.Join(" ", words);
        }

        /// <summary>
        /// Calculate the distance between two strings
        /// </summary>
        /// <param name="value">The first string to be compared</param>
        /// <param name="compareValue">The second string to be compared</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Algorithm requires a matrix.")]
        public static int Distance(this string value, string compareValue)
        {
            if (String.IsNullOrEmpty(value))
            {
                return (compareValue ?? String.Empty).Length;
            }

            if (String.IsNullOrEmpty(compareValue))
            {
                return value.Length;
            }

            var bounds = new { Height = value.Length + 1, Width = compareValue.Length + 1 };
            var matrix = new int[bounds.Height, bounds.Width];

            for (var height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (var width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (var height = 1; height < bounds.Height; height++)
            {
                for (var width = 1; width < bounds.Width; width++)
                {
                    var cost = (value[height - 1] == compareValue[width - 1]) ? 0 : 1;
                    var insertion = matrix[height, width - 1] + 1;
                    var deletion = matrix[height - 1, width] + 1;
                    var substitution = matrix[height - 1, width - 1] + cost;

                    var distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && value[height - 1] == compareValue[width - 2] && value[height - 2] == compareValue[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }

        /// <summary>
        /// Get the last directory name from a path
        /// </summary>
        /// <param name="value">The path to get the last directory name from</param>
        /// <returns>The last directory name from the path</returns>
        public static string GetLastDirectoryName(this string value)
        {
            return new DirectoryInfo(value).Name;
        }

        /// <summary>
        /// Convert a numeric string to Roman numerals
        /// </summary>
        /// <param name="value">The string to be converted</param>
        /// <returns>The Roman numeral representation of the string</returns>
        public static string ToRoman(this string value)
        {
            if (String.IsNullOrWhiteSpace(value) || value.Length > 4)
            {
                return value;
            }

            var romanNumerals = new string[][]
            {
                new string[] {"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"},
                new string[] {"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"},
                new string[] {"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"},
                new string[] {"", "M", "MM", "MMM", "MV", "V", "VM", "VMM", "VMMM", "MX"}
            };

            var digits = value.Reverse().ToArray();
            var returnValue = String.Empty;
            var digit = digits.Length;

            while (digit-- > 0)
            {
                returnValue += romanNumerals[digit][Int32.Parse(digits[digit].ToString(), CultureInfo.InvariantCulture)];
            }

            return returnValue;
        }

        #endregion Public Methods
    }
}