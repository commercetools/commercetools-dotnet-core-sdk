namespace commercetools.Sdk.Domain.Products
{
    public class PublishUpdateAction : UpdateAction<Product>
    {
        public string Action => "publish";
        public PublishScope Scope { get; set; }

    }
}