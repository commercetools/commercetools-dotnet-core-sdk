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
        private readonly IDictionary<Type, Type> mapping;
        private readonly IEnumerable<IRequestMessageBuilder> registeredRequestMessageBuilders;

        public RequestMessageBuilderFactory(IDictionary<Type, Type> mapping, IEnumerable<IRequestMessageBuilder> registeredRequestMessageBuilders)
        {
            this.registeredRequestMessageBuilders = registeredRequestMessageBuilders;
            this.mapping = mapping;
        }

        public IRequestMessageBuilder GetRequestMessageBuilder<T>(ICommand<T> command)
        {
            Type typeOfCommand = command.GetType().GetGenericTypeDefinition();
            if (this.mapping.ContainsKey(typeOfCommand))
            {
                Type typeOfBuilder = this.mapping[typeOfCommand];
                return this.registeredRequestMessageBuilders.Where(x => x.GetType() == typeOfBuilder).FirstOrDefault();
            }
            return null;
        }
    }
}
