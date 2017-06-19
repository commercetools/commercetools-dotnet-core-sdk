using System;
using commercetools.Common;
using Newtonsoft.Json;

namespace commercetools.CartDiscount.UpdateActions
{
    public class SetValidUntil:UpdateAction
    {
        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; }
        public SetValidUntil(DateTime validUntil)
        {
            this.Action = "setValidUntil";
            this.ValidUntil = validUntil;
        }
    }
}
