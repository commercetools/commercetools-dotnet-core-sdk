using System.Collections.Generic;

using Newtonsoft.Json;

using commercetools.Common;

namespace commercetools.Carts
{
    /// <summary>
    /// DiscountedLineItemPrice
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#discountedlineitemprice"/>
    public class DiscountedLineItemPrice
    {
        #region Properties

        [JsonProperty(PropertyName = "value")]
        public Money Value { get; private set; }

        [JsonProperty(PropertyName = "includedDiscounts")]
        public List<DiscountedLineItemPortion> IncludedDiscounts { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public DiscountedLineItemPrice(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Value = new Money(data.value);
            this.IncludedDiscounts = Helper.GetListFromJsonArray<DiscountedLineItemPortion>(data.includedDiscounts);
        }

        #endregion
    }
}
