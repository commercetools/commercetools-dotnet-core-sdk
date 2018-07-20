using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Inventory.UpdateActions
{
    /// <summary>
    /// This action sets, overwrites or removes any existing custom field for an existing inventory.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-inventory.html#set-customfield"/>
    public class SetCustomFieldAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Field name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        /// <remarks>
        /// If absent or null, this field is removed if it exists. If value is provided, set the value of the field defined by the name.
        /// </remarks>
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Field name</param>
        public SetCustomFieldAction(string name)
        {
            this.Action = "setCustomField";
            this.Name = name;
        }

        #endregion
    }
}
