using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// Base class for SuggestTokenizer types.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#suggesttokenizer"/>
    public abstract class SuggestTokenizer
    {
        #region Properties

        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SuggestTokenizer()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected SuggestTokenizer(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Type = data.type;
        }

        #endregion
    }
}
