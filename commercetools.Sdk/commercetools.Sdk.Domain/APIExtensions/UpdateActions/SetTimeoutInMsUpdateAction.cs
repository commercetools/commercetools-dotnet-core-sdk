namespace commercetools.Sdk.Domain.APIExtensions.UpdateActions
{
    public class SetTimeoutInMsUpdateAction : UpdateAction<Extension>
    {
        public string Action => "setTimeoutInMs";
        public long TimeoutInMs { get; set; }
    }
}
