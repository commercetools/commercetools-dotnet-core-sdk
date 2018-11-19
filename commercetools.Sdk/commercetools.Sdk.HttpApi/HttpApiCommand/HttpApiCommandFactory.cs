using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using commercetools.Sdk.Client;
using commercetools.Sdk.Extensions;

namespace commercetools.Sdk.HttpApi
{
    public class HttpApiCommandFactory : IHttpApiCommandFactory
    {
        private IEnumerable<Type> registeredHttpApiCommandTypes;
        private IRequestMessageBuilderFactory requestMessageBuilderFactory;

        public HttpApiCommandFactory(IRegisteredTypeRetriever registeredTypeRetriever, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> registeredHttpApiCommandTypes = registeredTypeRetriever.GetRegisteredTypes<IHttpApiCommand>();
            this.registeredHttpApiCommandTypes = registeredHttpApiCommandTypes;
            this.requestMessageBuilderFactory = requestMessageBuilderFactory;
        }

        public IHttpApiCommand Create<T>(Command<T> command)
        {
            // retrieve the type of T
            // TODO Find a neater way to find the generic type
            // Add a get type property to commands
            Type typeOfGeneric;
            if (command.GetType().IsGenericType)
            {
                typeOfGeneric = command.GetType().GetGenericArguments().FirstOrDefault();
            }
            else
            {
                typeOfGeneric = command.GetType().BaseType.GetGenericArguments().FirstOrDefault();
            }
            Type httApiCommandType = null;

            foreach (Type type in this.registeredHttpApiCommandTypes)
            {
                // retrieving the command type from IHttpApiCommand, e.g. GetHttpApiCommand<T>: IHttpApiCommand<GetCommand<T>, T>
                var httpApiCommandGenericType = type.GetInterfaces().First().GetGenericArguments().First();
                // IsAssignableFrom does not work on open generic types <>, which means the type of T needs to be passed first
                if (httpApiCommandGenericType.GetGenericTypeDefinition().MakeGenericType(typeOfGeneric).IsAssignableFrom(command.GetType()))
                {
                    httApiCommandType = type;
                    break;
                }
            }

            // TODO Replace with compiled lamba expression
            return Activator.CreateInstance(httApiCommandType.MakeGenericType(typeOfGeneric), command, requestMessageBuilderFactory) as IHttpApiCommand;
        }
    }
}
