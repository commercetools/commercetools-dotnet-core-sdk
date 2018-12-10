namespace commercetools.Sdk.Domain.Carts
{ 
    public class GetCartByCustomerIdAdditionalParameters : IAdditionalParameters<Cart>
    {
        public string CustomerId { get; set; }
    }
}