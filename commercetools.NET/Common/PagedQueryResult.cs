using System;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// For query responses of requests supporting paging via limit and offset.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api.html#paged-query-result"/>
    public abstract class PagedQueryResult
    {
        #region Properties

        [JsonProperty(PropertyName = "offset")]
        public int? Offset { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int? Total { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PagedQueryResult(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Offset = data.offset;
            this.Count = data.count;
            this.Total = data.total;
        }

        #endregion
    }
}