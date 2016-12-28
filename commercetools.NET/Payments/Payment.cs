using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Payments
{
    /// <summary>
    /// Payments hold information about the current state of receiving and/or refunding money.
    /// </summary>
    /// <remarks>
    /// A payment represents one or a logically connected series of financial transactions like reserving money, charging money or refunding money. They serve as a representation of the current state of the payment and can also be used to trigger new transactions. The actual financial process is not done by the commercetools™ platform but usually by a PSP (Payment Service Provider), which is connected via PSP-specific integration implementation. The Payment representation does not contain payment method-specific fields. These are added as CustomFields via a payment method-specific payment type.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#payment"/>
    public class Payment
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "customer")]
        public Reference Customer { get; private set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; private set; }

        [JsonProperty(PropertyName = "interfaceId")]
        public string InterfaceId { get; private set; }

        [JsonProperty(PropertyName = "amountPlanned")]
        public Money AmountPlanned { get; private set; }

        [JsonProperty(PropertyName = "amountAuthorized")]
        public Money AmountAuthorized { get; private set; }

        [JsonProperty(PropertyName = "authorizedUntil")]
        public string AuthorizedUntil { get; private set; }

        [JsonProperty(PropertyName = "amountPaid")]
        public Money AmountPaid { get; private set; }

        [JsonProperty(PropertyName = "amountRefunded")]
        public Money AmountRefunded { get; private set; }

        [JsonProperty(PropertyName = "paymentMethodInfo")]
        public PaymentMethodInfo PaymentMethodInfo { get; private set; }

        [JsonProperty(PropertyName = "paymentStatus")]
        public PaymentStatus PaymentStatus { get; private set; }

        [JsonProperty(PropertyName = "transactions")]
        public List<Transaction> Transactions { get; private set; }

        [JsonProperty(PropertyName = "interfaceInteractions")]
        public List<CustomFields.CustomFields> InterfaceInteractions { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Payment(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.Customer = new Reference(data.customer);
            this.ExternalId = data.externalId;
            this.InterfaceId = data.interfaceId;
            this.AmountPlanned = new Money(data.amountPlanned);
            this.AmountAuthorized = new Money(data.amountAuthorized);
            this.AuthorizedUntil = data.authorizedUntil;
            this.AmountPaid = new Money(data.amountPaid);
            this.AmountRefunded = new Money(data.amountRefunded);
            this.PaymentMethodInfo = new PaymentMethodInfo(data.paymentMethodInfo);
            this.PaymentStatus = new PaymentStatus(data.paymentStatus);
            this.Transactions = Helper.GetListFromJsonArray<Transaction>(data.transactions);
            this.InterfaceInteractions = Helper.GetListFromJsonArray<CustomFields.CustomFields>(data.interfaceInteractions);
            this.Custom = new CustomFields.CustomFields(data.custom);
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Payment payment = obj as Payment;

            if (payment == null)
            {
                return false;
            }

            return payment.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
