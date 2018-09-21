using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class RequestMessageBuilderFactory : IRequestMessageBuilderFactory
    {
        private readonly IEnumerable<IRequestMessageBuilder> registeredRequestMessageBuilders;

        public RequestMessageBuilderFactory(IEnumerable<IRequestMessageBuilder> registeredRequestMessageBuilders)
        {
            this.registeredRequestMessageBuilders = registeredRequestMessageBuilders;
        }

        public T GetRequestMessageBuilder<T>()
        {
            IRequestMessageBuilder requestMessageBuilder = this.registeredRequestMessageBuilders.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
            if (requestMessageBuilder != null)
            {
                return (T)requestMessageBuilder;
            }
            return default(T);
        }
    }
}
