using Newtonsoft.Json;

namespace commercetools.ProductProjectionSearch
{
    /// <summary>
    /// Range facets provide statistical data over values for date, time, datetime, number and money type fields in a Facet range type.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-products-search.html#range"/>
    public class Range
    {
        #region Properties

        [JsonProperty(PropertyName = "from")]
        public int From { get; private set; }

        [JsonProperty(PropertyName = "fromStr")]
        public string FromStr { get; private set; }

        [JsonProperty(PropertyName = "to")]
        public int To { get; private set; }

        [JsonProperty(PropertyName = "toStr")]
        public string ToStr { get; private set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; private set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; private set; }

        [JsonProperty(PropertyName = "min")]
        public int Min { get; private set; }

        [JsonProperty(PropertyName = "max")]
        public int Max { get; private set; }

        [JsonProperty(PropertyName = "mean")]
        public double Mean { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Range(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.From = data.from;
            this.FromStr = data.fromStr;
            this.To = data.to;
            this.ToStr = data.toStr;
            this.Count = data.count;
            this.Total = data.total;
            this.Min = data.min;
            this.Max = data.max;
            this.Mean = data.mean;
        }

        #endregion
    }
}
