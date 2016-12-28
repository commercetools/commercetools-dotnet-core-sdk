using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Carts
{
    /// <summary>
    /// DiscountCodeInfo
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html#discountcodeinfo"/>
    public class DiscountCodeInfo
    {
        #region Properties

        [JsonProperty(PropertyName = "discountCode")]
        public Reference DiscountCode { get; private set; }

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DiscountCodeState? State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public DiscountCodeInfo(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            DiscountCodeState? state;

            this.DiscountCode = data.discountCode != null ? new Reference(data.discountCode) : null;
            this.State = Helper.TryGetEnumByEnumMemberAttribute<DiscountCodeState?>((string)data.state, out state) ? state : null;
        }

        #endregion
    }
}
