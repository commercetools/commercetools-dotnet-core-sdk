using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.HttpApi.RequestBuilders
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
            IRequestMessageBuilder requestMessageBuilder = this.registeredRequestMessageBuilders.FirstOrDefault(x => x.GetType() == typeof(T));
            if (requestMessageBuilder != null)
            {
                return (T)requestMessageBuilder;
            }
            return default(T);
        }
    }
}