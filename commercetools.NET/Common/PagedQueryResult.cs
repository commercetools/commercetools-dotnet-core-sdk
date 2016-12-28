using System;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// For query responses of requests supporting paging via limit and offset.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api.html#pagedqueryresult"/>
    public abstract class PagedQueryResult
    {
        #region Properties

        /// <summary>
        /// The offset supplied by the client or the server default. It is the number of elements skipped, not a page number.
        /// </summary>
        [JsonProperty(PropertyName = "offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// The actual number of results returned.
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

        /// <summary>
        /// The total number of results matching the query.
        /// </summary>
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
