namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    public class SetDeleteDaysAfterLastModificationUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setDeleteDaysAfterLastModification";
        public int? DeleteDaysAfterLastModification { get; set; }
    }
}
