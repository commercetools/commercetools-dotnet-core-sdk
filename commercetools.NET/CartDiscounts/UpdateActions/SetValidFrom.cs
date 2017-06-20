using System;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class SetValidFrom: UpdateAction
    {
        [JsonProperty(PropertyName = "validFrom")]
        public DateTime? ValidFrom { get; }
        public SetValidFrom(DateTime validFrom)
        {
            this.Action = "setValidFrom";
            this.ValidFrom = validFrom;
        }
    }
}
