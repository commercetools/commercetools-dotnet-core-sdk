using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Sets deleteDaysAfterLastModification. 
    /// </summary>
    /// <remarks>
    /// The cart will be deleted automatically if it hasn’t been modified for the specified amount of days and it is in the Active CartState. If a ChangeSubscription for carts exists, a ResourceDeleted notification will be sent.
    /// </remarks>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#set-deletedaysafterlastmodification-beta"/>
    public class SetDeleteDaysAfterLastModificationAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Delete days after last modification
        /// </summary>
        [JsonProperty(PropertyName = "deleteDaysAfterLastModification")]
        public int? DeleteDaysAfterLastModification { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetDeleteDaysAfterLastModificationAction()
        {
            this.Action = "setDeleteDaysAfterLastModification";
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="deleteDaysAfterLastModification">The cart will be deleted automatically if it hasn’t been modified for the specified amount of days and it is in the Active CartState. If a ChangeSubscription for carts exists, a ResourceDeleted notification will be sent.</param>
        public SetDeleteDaysAfterLastModificationAction(int? deleteDaysAfterLastModification) : this()
        {
            this.DeleteDaysAfterLastModification = deleteDaysAfterLastModification;
        }
        #endregion
    }
}
