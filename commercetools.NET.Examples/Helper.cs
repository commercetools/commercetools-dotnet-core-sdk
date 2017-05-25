using System;
using System.Linq;

using commercetools.Common;

namespace commercetools.Examples
{
    public class Helper
    {
        private static Random _random = new Random();

        /// <summary>
        /// Gets a random string.
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <returns>Random string</returns>
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets a random integer.
        /// </summary>
        /// <param name="minValue">Min. value</param>
        /// <param name="maxValue">Max. value</param>
        /// <returns>Random integer between min. value and max. value</returns>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Writes an API error and nested messages to the console.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="response">Response</param>
        public static void WriteError<T>(Response<T> response)
        {
            Console.WriteLine("Error: {0} - {1}", response.StatusCode, response.ReasonPhrase);

            foreach (ErrorMessage errorMessage in response.Errors)
            {
                Console.WriteLine("\t{0}: {1}", errorMessage.Code, errorMessage.Message);
            }
        }
    }
}
