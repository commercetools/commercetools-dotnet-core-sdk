using System;

namespace commercetools.Products
{
    /// <summary>
    /// SuggestTokenizerFactory
    /// </summary>
    public static class SuggestTokenizerFactory
    {
        /// <summary>
        /// Creates a SuggestTokenizer using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from SuggestTokenizer, or null</returns>
        public static SuggestTokenizer Create(dynamic data)
        {
            if (data == null || data.type == null)
            {
                return null;
            }

            switch ((string)data.type)
            {
                case "whitespace":
                    return new WhitespaceTokenizer(data);
                case "custom":
                    return new CustomTokenizer(data);
            }

            return null;
        }
    }
}
