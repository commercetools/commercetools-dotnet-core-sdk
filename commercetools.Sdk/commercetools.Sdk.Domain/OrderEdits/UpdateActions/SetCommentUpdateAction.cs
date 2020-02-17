namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class SetCommentUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "setComment";
        public string Comment { get; set; }
    }
}