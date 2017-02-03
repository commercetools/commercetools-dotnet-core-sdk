using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// This action changes the order of enum values in an EnumType attribute definition. It can update an EnumType attribute definition or a Set of EnumType attribute definition.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#change-the-order-of-enumvalues"/>
    public class ChangePlainEnumValueOrderAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Attribute Name
        /// </summary>
        [JsonProperty(PropertyName = "attributeName")]
        public string AttributeName { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        /// <remarks>
        /// The values must be equal to the values of the attribute enum values (except for the order).
        /// </remarks>
        [JsonProperty(PropertyName = "values")]
        public List<PlainEnumValue> Values { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributeName">Attribute Name</param>
        /// <param name="values">Values</param>
        public ChangePlainEnumValueOrderAction(string attributeName, List<PlainEnumValue> values)
        {
            this.Action = "changePlainEnumValueOrder";
            this.AttributeName = attributeName;
            this.Values = values;
        }

        #endregion
    }
}
