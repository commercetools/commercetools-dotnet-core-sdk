using System;
using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class GetByIdCommand<T> : ICommand<T>
    {
        public Guid Guid { get; set; }

        public GetByIdCommand(Guid guid)
        {
            this.Guid = guid;
        }
    }
}