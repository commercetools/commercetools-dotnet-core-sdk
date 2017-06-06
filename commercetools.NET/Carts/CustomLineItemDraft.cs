using System.Collections.Generic;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// API representation for creating a new CustomLineItem.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#customlineitemdraft"/>
    public class CustomLineItemDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; set; }

        [JsonProperty(PropertyName = "money")]
        public Money Money { get; set; }

        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }

        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomLineItemDraft(LocalizedString name)
        {
            this.Name = name;
        }

        #endregion
    }
}
