using System;
using System.Collections.Generic;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts
{
    public class AbsoluteCartDiscountValue : CartDiscountValue
    {
        [JsonProperty(PropertyName = "money")]
        public List<Money> Money { get; private set; }

        public AbsoluteCartDiscountValue(List<Money> money) : base(CartDiscountType.Absolute)
        {
            if (money == null || money.Count == 0)
            {
                throw new ArgumentException($"{nameof(money)} cannot be null or empty.");
            }

            this.Money = money;
        }
        
        public AbsoluteCartDiscountValue(dynamic data) : base((object)data)
        {
            List<Money> money =  Helper.GetListFromJsonArray<Money>(data.money);

            if (money == null || money.Count == 0)
            {
                throw new ArgumentException($"{nameof(money)} cannot be null or empty.");
            }

            this.Money = money;
        }
    }
}
