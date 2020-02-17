using System;
using commercetools.Sdk.Client;

namespace commercetools.Sdk.HttpApi
{
    public interface IHttpApiCommandFactory
    {
        IHttpApiCommand Create<T>(ICommand<T> command);
    }
}