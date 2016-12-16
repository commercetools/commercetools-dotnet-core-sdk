using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Categories.UpdateActions
{
    /// <summary>
    /// Change name 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#change-name"/>
    public class ChangeNameAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public LocalizedString Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name</param>
        public ChangeNameAction(LocalizedString name)
        {
            this.Action = "changeName";
            this.Name = name;
        }

        #endregion
    }
}
