using System;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscounts.UpdateActions
{
    public class SetValidUntilAction:UpdateAction
    {
        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; }
        public SetValidUntilAction(DateTime validUntil)
        {
            this.Action = "setValidUntil";
            this.ValidUntil = validUntil;
        }
    }
}
