using commercetools.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Inventory.UpdateActions
{
    /// <summary>
    /// Set Restockable in Days
    /// </summary>   
    /// <see href="http://docs.commercetools.com/http-api-projects-inventory.html#set-restockableindays"/>
    public class SetRestockableInDaysAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// Days.
        /// </summary>
        [JsonProperty(PropertyName = "restockableInDays")]
        public int RestockableInDays { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="restockableInDays">Restockable in Days</param>
        public SetRestockableInDaysAction(int restockableInDays)
        {
            this.Action = "setRestockableInDays";
            this.RestockableInDays = restockableInDays;
        }

        #endregion
    }
}
