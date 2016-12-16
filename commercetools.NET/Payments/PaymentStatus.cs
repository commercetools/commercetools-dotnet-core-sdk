using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments
{
    /// <summary>
    /// PaymentStatus
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#paymentstatus"/>
    public class PaymentStatus
    {
        #region Properties

        [JsonProperty(PropertyName = "interfaceCode")]
        public string InterfaceCode { get; private set; }

        [JsonProperty(PropertyName = "interfaceText")]
        public string InterfaceText { get; private set; }

        [JsonProperty(PropertyName = "state")]
        public Reference State { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PaymentStatus()
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public PaymentStatus(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.InterfaceCode = data.interfaceCode;
            this.InterfaceText = data.interfaceText;
            this.State = new Reference(data.state);
        }

        #endregion
    }
}
