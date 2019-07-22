namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetAddressUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setAddress";
        public Address Address { get; set; }
    }
}
