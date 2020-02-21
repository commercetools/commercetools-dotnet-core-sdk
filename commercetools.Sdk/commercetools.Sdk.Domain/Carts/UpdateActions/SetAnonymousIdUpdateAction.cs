namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetAnonymousIdUpdateAction : CartUpdateAction
    {
        public override string Action => "setAnonymousId";
        public string AnonymousId { get; set; }
    }
}