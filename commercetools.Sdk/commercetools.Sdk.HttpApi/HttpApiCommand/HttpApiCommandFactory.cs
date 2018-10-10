using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi
{
    public class HttpApiCommandFactory : IHttpApiCommandFactory
    {
        private IEnumerable<Type> registeredTypes;
        private IRequestMessageBuilderFactory requestMessageBuilderFactory;

        public HttpApiCommandFactory(IEnumerable<Type> registeredTypes, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.registeredTypes = registeredTypes;
            this.requestMessageBuilderFactory = requestMessageBuilderFactory;
        }

        public IHttpApiCommand Create<T>(Command<T> command)
        {
            Type typeOfCommand = command.GetType().GetGenericTypeDefinition();
            Type typeOfGeneric = command.GetType().GetGenericArguments().FirstOrDefault();
            Type typeOfHttApiCommand = null;

            foreach (Type type in this.registeredTypes)
            {
                //Type typeOfInteface = type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestable<>)).FirstOrDefault();
                //if (typeOfInteface.GetGenericArguments().Any(x => x.GetGenericTypeDefinition() == typeOfCommand))
                //{ 
                //    typeOfHttApiCommand = type;
                //}
                
                var t = type.GetInterfaces().First().GetGenericArguments().First();
                if (EqualityComparer<Type>.Default.Equals(t.GetGenericTypeDefinition(), typeOfCommand))
                {
                    typeOfHttApiCommand = type;
                    break;
                }

            }

            // TODO Replace with compiled lamba expression
            return Activator.CreateInstance(typeOfHttApiCommand.MakeGenericType(typeOfGeneric), command, requestMessageBuilderFactory) as IHttpApiCommand;
        }
    }
}
