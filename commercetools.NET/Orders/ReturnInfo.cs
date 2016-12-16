using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// Stores information about returns connected to this order.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#returninfo"/>
    public class ReturnInfo
    {
        #region Properties

        [JsonProperty(PropertyName = "items")]
        public List<ReturnItem> Items { get; private set; }

        [JsonProperty(PropertyName = "returnTrackingId")]
        public string ReturnTrackingId { get; private set; }

        [JsonProperty(PropertyName = "returnDate")]
        public DateTime? ReturnDate { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ReturnInfo(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Items = Helper.GetListFromJsonArray<ReturnItem>(data.items);
            this.ReturnTrackingId = data.returnTrackingId;
            this.ReturnDate = data.returnDate;
        }

        #endregion
    }
}
