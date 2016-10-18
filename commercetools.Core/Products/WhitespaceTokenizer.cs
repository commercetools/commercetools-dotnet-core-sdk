using System;

using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// WhitespaceTokenizer
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#suggesttokenizer"/>
    public class WhitespaceTokenizer : SuggestTokenizer
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public WhitespaceTokenizer(dynamic data = null)
            : base((object)data)
        {
        }

        #endregion
    }
}