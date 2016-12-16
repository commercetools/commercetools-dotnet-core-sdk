using Newtonsoft.Json;

namespace commercetools.Messages
{
    /// <summary>
    /// This object is used to to configure the parameters of the Messages feature for the project. It is stored in the Project's messages field.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#messages-configuration"/>
    public class MessagesConfiguration
    {
        #region Properties
        
        /// <summary>
        /// Enabled
        /// </summary>
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public MessagesConfiguration(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Enabled = data.enabled;
        }

        #endregion
    }
}
