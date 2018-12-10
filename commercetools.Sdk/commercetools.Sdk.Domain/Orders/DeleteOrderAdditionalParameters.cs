namespace commercetools.Sdk.Domain.Orders
{ 
    public class DeleteOrderAdditionalParameters : IAdditionalParameters<Order>
    {
        public bool? DataErasure { get; set; }
    }
}