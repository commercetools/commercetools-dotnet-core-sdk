using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts
{
    /// <summary>
    /// DiscountedLineItemPriceForQuantity
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#discountedlineitempriceforquantity"/>
    public class DiscountedLineItemPriceForQuantity
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
        public DiscountedLineItemPriceForQuantity(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Value = new Money(data.discountedPrice.value);
            this.IncludedDiscounts = Helper.GetListFromJsonArray<DiscountedLineItemPortion>(data.discountedPrice.includedDiscounts);
        }

        #endregion
    }
}
