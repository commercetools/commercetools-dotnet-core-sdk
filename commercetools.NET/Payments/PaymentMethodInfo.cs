using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments
{
    /// <summary>
    /// PaymentMethodInfo
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#paymentMethodInfo"/>
    public class PaymentMethodInfo
    {
        #region Properties

        [JsonProperty(PropertyName = "paymentInterface")]
        public string PaymentInterface { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PaymentMethodInfo()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PaymentMethodInfo(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.PaymentInterface = data.paymentInterface;
            this.Method = data.method;
            this.Name = new LocalizedString(data.name);
        }

        #endregion
    }
}
