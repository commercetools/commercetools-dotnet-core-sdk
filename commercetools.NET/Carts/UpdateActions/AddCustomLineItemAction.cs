using commercetools.Common;
using commercetools.CustomFields;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Adds a CustomLineItem to the cart.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-customlineitem"/>
    public class AddCustomLineItemAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        /// <summary>
        /// Quantity - Defaults to 1
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public Money Money { get; set; }

        /// <summary>
        /// Slug
        /// </summary>
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }

        /// <summary>
        /// Reference to a TaxCategory - Required only for Platform TaxMode 
        /// </summary>
        /// <remarks>
        /// The given tax category will be used to select a tax rate when a cart has the tax mode Platform.
        /// </remarks>
        [JsonProperty(PropertyName = "taxCategory")]
        public Reference TaxCategory { get; set; }

        /// <summary>
        /// An external tax rate can be set if the cart has the External TaxMode.
        /// </summary>
        [JsonProperty(PropertyName = "externalTaxRate")]
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }

        /// <summary>
        /// The custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFieldsDraft Custom { get; set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="money">Money</param>
        /// <param name="slug">Slug</param>
        public AddCustomLineItemAction(LocalizedString name, Money money, string slug)
        {
            this.Action = "addCustomLineItem";
            this.Name = name;
            this.Money = money;
            this.Slug = slug;
        }

        #endregion
    }
}
