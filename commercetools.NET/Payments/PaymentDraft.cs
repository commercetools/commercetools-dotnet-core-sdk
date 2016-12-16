using System.Collections.Generic;

using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Payments
{
    /// <summary>
    /// PaymentDraft
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#paymentdraft"/>
    public class PaymentDraft
    {
        #region Properties

        [JsonProperty(PropertyName = "customer")]
        public Reference Customer { get; set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        [JsonProperty(PropertyName = "interfaceId")]
        public string InterfaceId { get; set; }

        [JsonProperty(PropertyName = "amountPlanned")]
        public Money AmountPlanned { get; set; }

        [JsonProperty(PropertyName = "amountAuthorized")]
        public Money AmountAuthorized { get; set; }

        [JsonProperty(PropertyName = "authorizedUntil")]
        public string AuthorizedUntil { get; set; }

        [JsonProperty(PropertyName = "amountPaid")]
        public Money AmountPaid { get; set; }

        [JsonProperty(PropertyName = "amountRefunded")]
        public Money AmountRefunded { get; set; }

        [JsonProperty(PropertyName = "paymentMethodInfo")]
        public PaymentMethodInfo PaymentMethodInfo { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; set; }

        [JsonProperty(PropertyName = "paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }

        [JsonProperty(PropertyName = "transactions")]
        public List<TransactionDraft> Transactions { get; set; }

        [JsonProperty(PropertyName = "interfaceInteractions")]
        public List<CustomFieldsDraft> InterfaceInteractions { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PaymentDraft()
        {
        }

        #endregion
    }
}
