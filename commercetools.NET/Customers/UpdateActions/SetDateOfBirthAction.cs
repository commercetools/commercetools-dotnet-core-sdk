using System;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers.UpdateActions
{
    /// <summary>
    /// SetDateOfBirthAction
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#set-date-of-birth"/>
    public class SetDateOfBirthAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// DateOfBirth
        /// </summary>
        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetDateOfBirthAction()
        {
            this.Action = "setDateOfBirth";
        }

        #endregion
    }
}
