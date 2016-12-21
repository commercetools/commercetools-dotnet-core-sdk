using commercetools.Common;
using commercetools.Types;

using Newtonsoft.Json;

namespace commercetools.Products.UpdateActions
{
    /// <summary>
    /// Adds / Removes / Changes a custom attribute in all variants at the same time (it can be helpful to set attribute values that are constrained with SameForAll).
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#set-attribute-in-all-variants"/>
    public class SetAttributeInAllVariantsAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public FieldType Value { get; set; }

        /// <summary>
        /// Staged
        /// </summary>
        [JsonProperty(PropertyName = "staged")]
        public bool Staged { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        public SetAttributeInAllVariantsAction(string name)
        {
            this.Action = "setAttributeInAllVariants";
            this.Name = name;
        }

        #endregion
    }
}
