namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetDeleteDaysAfterLastModificationUpdateAction : CartUpdateAction
    {
        public override string Action => "setDeleteDaysAfterLastModification";
        public int? DeleteDaysAfterLastModification { get; set; }
    }
}
