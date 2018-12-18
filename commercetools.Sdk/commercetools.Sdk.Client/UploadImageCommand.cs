using System;
using System.IO;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class UploadImageCommand<T> : Command<T>
    {
        protected UploadImageCommand(Guid id, Stream image, string contentType)
        {
            this.Init(id, image, contentType);
        }

        protected UploadImageCommand(Guid id, Stream image, string contentType, IAdditionalParameters<T> additionalParameters)
        {
            this.Init(id, image, contentType);
            this.AdditionalParameters = additionalParameters;
        }

        public Guid Id { get; private set; }

        public IUploadImageParameters<T> Parameters { get; set; }

        public Stream Image { get; set; }

        public string ContentType { get; set; }

        public override System.Type ResourceType => typeof(T);

        private void Init(Guid id, Stream image, string contentType)
        {
            this.Id = id;
            this.Image = image;
            this.ContentType = contentType;
        }
    }
}
