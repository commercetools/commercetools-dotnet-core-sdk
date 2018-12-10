namespace commercetools.Sdk.Domain.Orders
{
    public abstract class ReturnItemDraft
    {
        public double Quantity { get; set; }
        public string Comment { get; set; }
        public ReturnShipmentState ShipmentState { get; set; }
    }
}