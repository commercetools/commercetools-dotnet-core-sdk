using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.DiscountCodes.UpdateActions
{
    public class ChangeCartDiscountsAction : UpdateAction
    {
        [JsonProperty(PropertyName = "cartDiscounts")]
        public List<Reference> CartDiscounts { get; private set; }

        public ChangeCartDiscountsAction(List<Reference> cartDiscounts)
        {
            if (cartDiscounts == null || cartDiscounts.Count == 0)
            {
                throw new ArgumentException($"{nameof(cartDiscounts)} cannot be null or empty");
            }

            this.Action = "changeCartDiscounts";
            this.CartDiscounts = cartDiscounts;
        }
    }
}
