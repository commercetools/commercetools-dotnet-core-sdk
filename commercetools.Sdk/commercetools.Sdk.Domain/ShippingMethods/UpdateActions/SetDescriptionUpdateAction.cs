using System;

namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    [Obsolete("Use SetLocalizedDescriptionUpdateAction instead")]
    public class SetDescriptionUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}