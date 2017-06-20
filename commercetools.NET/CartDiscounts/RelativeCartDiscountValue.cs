using Newtonsoft.Json;

namespace commercetools.CartDiscounts
{
    public class RelativeCartDiscountValue : CartDiscountValue
    {
        [JsonProperty(PropertyName = "permyriad")]
        public int? Permyriad { get; private set; }

        public RelativeCartDiscountValue(int permyriad) : base(CartDiscountType.Relative)
        {
            Permyriad = permyriad;
        }

        public RelativeCartDiscountValue(dynamic data) : base((object)data)
        {
            this.Permyriad = data.permyriad;
        }
    }
}
