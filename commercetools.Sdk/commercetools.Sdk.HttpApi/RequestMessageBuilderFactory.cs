using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class RequestMessageBuilderFactory : IRequestMessageBuilderFactory
    {
        private readonly IDictionary<Type, Type> registeredMessageBuilderTypes;

        public RequestMessageBuilderFactory(IDictionary<Type, Type> registeredMessageBuilderTypes)
        {
            this.registeredMessageBuilderTypes = registeredMessageBuilderTypes;
        }

        public IRequestMessageBuilder GetRequestMessageBuilder(ICommand command)
        {
            Type typeOfCommand = command.GetType();
            if (registeredMessageBuilderTypes.ContainsKey(typeOfCommand))
            {
                Type typeOfBuilder = registeredMessageBuilderTypes[typeOfCommand];
                IRequestMessageBuilder builder = Activator.CreateInstance(typeOfBuilder, command) as IRequestMessageBuilder;
                return builder;
            }
            return null;
        }
    }
}
