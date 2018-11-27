namespace commercetools.Sdk.Domain.Categories
{
    public class ChangeParentUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeParent";
        public ResourceIdentifier Slug { get; set; }
    }
}