using commercetools.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace commercetools.Channels
{

    /// <summary>
    /// An implementation of PagedQueryResult that provides access to the results as a List of channel objects.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api.html#pagedqueryresult"/>
    public class ChannelQueryResult : PagedQueryResult
    {
        #region Properties

        /// <summary>
        /// Results
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public List<Channel> Results { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ChannelQueryResult(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Results = Helper.GetListFromJsonArray<Channel>(data.results);
        }

        #endregion
    }

}
