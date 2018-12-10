namespace commercetools.Sdk.Domain.Carts
{ 
    public class DeleteCartAdditionalParameters : IAdditionalParameters<Cart>
    {
        public bool? DataErasure { get; set; }
    }
}