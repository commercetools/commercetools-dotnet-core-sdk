using System;
using System.IO;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class UploadImageCommand<T> : Command<T>
    {
        protected UploadImageCommand(Guid id, Stream image, string contentType)
        {
            this.Id = id;
            this.Image = image;
            this.ContentType = contentType;
        }

        public Guid Id { get; }

        public IUploadImageParameters<T> Parameters { get; set; }

        public Stream Image { get; set; }

        public string ContentType { get; set; }

        public override System.Type ResourceType => typeof(T);
    }
}
