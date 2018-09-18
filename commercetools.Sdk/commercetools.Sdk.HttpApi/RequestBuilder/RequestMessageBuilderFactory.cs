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

        public IRequestMessageBuilder GetRequestMessageBuilder<T>(ICommand<T> command)
        {
            Type typeOfCommand = command.GetType();
            //return registeredRequestMessageBuilders.Where(x => x.CommandType == typeOfCommand).FirstOrDefault();
            return null;
        }
    }
}
