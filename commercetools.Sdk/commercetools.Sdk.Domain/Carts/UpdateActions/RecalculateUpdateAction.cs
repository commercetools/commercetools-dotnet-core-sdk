namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RecalculateUpdateAction : UpdateAction<Cart>
    {
        public string Action => "recalculate";
        public bool UpdateProductData { get; set; }
    }
}