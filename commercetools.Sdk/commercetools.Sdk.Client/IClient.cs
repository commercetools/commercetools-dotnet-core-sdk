using System;
using System.Threading.Tasks;

namespace commercetools.Sdk.Client
{
    public interface IClient
    {
        string Name { get; set; }

        Task<T> Execute<T>(ICommand command);
    }
}