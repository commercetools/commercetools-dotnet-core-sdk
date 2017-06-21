using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes
{
    public class DiscountCodeQueryResult : PagedQueryResult
    {
        #region Properties

        /// <summary>
        /// Results
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public List<DiscountCode> Results { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public DiscountCodeQueryResult(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Results = Helper.GetListFromJsonArray<DiscountCode>(data.results);
        }

        #endregion
    }
}
