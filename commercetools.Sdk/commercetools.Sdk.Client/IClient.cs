using commercetools.Sdk.Domain;
using System;
using System.Threading.Tasks;

namespace commercetools.Sdk.Client
{
    public interface IClient
    {
        string Name { get; set; }

        Task<T> Execute<T>(Command<T> command);

        //Task<T> Get<T>(Guid id);
        //Task<T> CreateAsync<T>(IDraft<T> draft);
        //Task<PagedQueryResult<T>> Query<T>(QueryPredicate<T> queryPredicate, Sort<T> sort, Expansion expand, int limit, int offset);
    }
}