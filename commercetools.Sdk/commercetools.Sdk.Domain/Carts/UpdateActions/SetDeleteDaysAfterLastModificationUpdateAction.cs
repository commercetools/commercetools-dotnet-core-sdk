namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetDeleteDaysAfterLastModificationUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setDeleteDaysAfterLastModification";
        public int DeleteDaysAfterLastModification { get; set; }
    }
}