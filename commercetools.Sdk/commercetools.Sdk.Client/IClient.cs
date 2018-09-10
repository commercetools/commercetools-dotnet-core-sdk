using commercetools.Sdk.Domain;
using System;
using System.Threading.Tasks;

namespace commercetools.Sdk.Client
{
    public interface IClient
    {
        string Name { get; set; }

        Task<T> GetByIdAsync<T>(Guid id);
        Task<T> GetByKeyAsync<T>(string key);
    }
}