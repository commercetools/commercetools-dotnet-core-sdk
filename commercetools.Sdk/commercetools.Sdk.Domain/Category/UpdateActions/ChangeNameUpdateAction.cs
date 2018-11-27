namespace commercetools.Sdk.Domain.Categories
{
    public class ChangeNameUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeName";
        public LocalizedString Name { get; set; }
    }
}