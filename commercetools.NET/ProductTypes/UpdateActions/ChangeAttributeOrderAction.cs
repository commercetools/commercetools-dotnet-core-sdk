using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Changes the order of AttributeDefinitions.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#change-the-order-of-attributedefinitions"/>
    public class ChangeAttributeOrderAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Attributes
        /// </summary>
        /// <remarks>
        /// The attributes must be equal to the product type attributes (except for the order).
        /// </remarks>
        [JsonProperty(PropertyName = "attributes")]
        public List<AttributeDefinition> Attributes { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributes">Attributes</param>
        public ChangeAttributeOrderAction(List<AttributeDefinition> attributes)
        {
            this.Action = "changeAttributeOrder";
            this.Attributes = attributes;
        }

        #endregion
    }
}
