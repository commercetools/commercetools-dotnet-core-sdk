using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using commercetools.Sdk.Registration;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class HttpApiCommandFactory : IHttpApiCommandFactory
    {
        private readonly Dictionary<Type, ObjectActivator> activators;
        private readonly IEnumerable<Type> registeredHttpApiCommandTypes;
        private readonly IRequestMessageBuilderFactory requestMessageBuilderFactory;

        public HttpApiCommandFactory(ITypeRetriever typeRetriever, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.registeredHttpApiCommandTypes = typeRetriever.GetTypes<IHttpApiCommand>();
            this.requestMessageBuilderFactory = requestMessageBuilderFactory;
            this.activators = new Dictionary<Type, ObjectActivator>();
        }

        private delegate object ObjectActivator(params object[] args);

        public IHttpApiCommand Create<T>(Command<T> command)
        {
            // Retrieve the type of T; for CreateCommand<Category>, Category is retrieved
            Type typeOfGeneric = command.ResourceType;
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

            if (httApiCommandType == null)
            {
                throw new ArgumentException();
            }

            // CreateHttpApiCommand<T> => CreateHttpApiCommand<Category>
            Type requestedType = httApiCommandType.MakeGenericType(typeOfGeneric);
            ObjectActivator createdActivator;
            if (this.activators.ContainsKey(requestedType))
            {
                createdActivator = this.activators[requestedType];
            }
            else
            {
                ConstructorInfo ctor = requestedType.GetConstructors().First();
                createdActivator = GetActivator(ctor);
                activators[requestedType] = createdActivator;
            }

            object instance = createdActivator(command, requestMessageBuilderFactory);
            return instance as IHttpApiCommand;
        }

        // TODO Move this to a different class perhaps
        private ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            ObjectActivator compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }
    }
}