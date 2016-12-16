using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// ChangeEmailAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#change-email"/>
    public class ChangeEmailAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="email">Email</param>
        public ChangeEmailAction(string email)
        {
            this.Action = "changeEmail";
            this.Email = email;
        }

        #endregion
    }
}
