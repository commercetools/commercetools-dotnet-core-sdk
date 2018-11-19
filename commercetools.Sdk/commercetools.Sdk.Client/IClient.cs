using commercetools.Sdk.Domain;
using System;
using System.Threading.Tasks;

namespace commercetools.Sdk.Client
{
    public interface IClient
    {
        string Name { get; set; }

        Task<T> ExecuteAsync<T>(Command<T> command);
    }
}