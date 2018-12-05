namespace commercetools.Sdk.Client
{
    using System;
    using Domain;

    // TODO Create request builder for this
    public class UploadProductImage : Command<Product>
    {
        public override System.Type ResourceType => typeof(Product);

        public Guid Id { get; set; }

        // TODO See if this should be moved to domain project
        public int Variant { get; set; }

        public string Sku { get; set; }

        public string Filename { get; set; }

        public bool Staged { get; set; }
    }
}
