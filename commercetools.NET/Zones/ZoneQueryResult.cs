using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Zones
{
    /// <summary>
    /// An implementation of PagedQueryResult that provides access to the results as a List of Zone objects.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api.html#pagedqueryresult"/>
    public class ZoneQueryResult : PagedQueryResult
    {
        #region Properties

        /// <summary>
        /// Results
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public List<Zone> Results { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ZoneQueryResult(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Results = Helper.GetListFromJsonArray<Zone>(data.results);
        }

        #endregion
    }
}
