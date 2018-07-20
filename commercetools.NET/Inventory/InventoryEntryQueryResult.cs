using commercetools.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Inventory
{
    /// <summary>
    /// An implementation of PagedQueryResult that provides access to the results as a List of inventory objects.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api.html#pagedqueryresult"/>
    public class InventoryEntryQueryResult : PagedQueryResult
    {
        #region Properties

        /// <summary>
        /// Results
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public List<InventoryEntry> Results { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public InventoryEntryQueryResult(dynamic data)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Results = Helper.GetListFromJsonArray<InventoryEntry>(data.results);
        }

        #endregion
    }
}
