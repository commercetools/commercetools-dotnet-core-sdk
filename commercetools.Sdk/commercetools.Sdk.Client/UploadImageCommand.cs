using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class UploadImageCommand<T> : Command<T>
    {
        public override System.Type ResourceType => typeof(T);

        public IUploadImageParameters<T> UploadImageParameters { get; set; }
    }
}
