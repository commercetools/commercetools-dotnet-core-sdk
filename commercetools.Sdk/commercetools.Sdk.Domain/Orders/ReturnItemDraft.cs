namespace commercetools.Sdk.Domain.Orders
{
    public abstract class ReturnItemDraft
    {
        public long Quantity { get; set; }
        public string Comment { get; set; }
        public ReturnShipmentState ShipmentState { get; set; }
        public CustomFields Custom { get; set; }
    }
}
