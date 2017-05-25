using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes.UpdateActions
{
    /// <summary>
    /// Set key
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-productTypes.html#set-key"/>
    public class SetKeyAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Key
        /// </summary>
        /// <remarks>
        /// If key is absent or null, this field is removed if it exists.
        /// </remarks>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetKeyAction()
        {
            this.Action = "setKey";
        }

        #endregion
    }
}
