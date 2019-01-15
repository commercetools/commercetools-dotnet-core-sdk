namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Category>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}