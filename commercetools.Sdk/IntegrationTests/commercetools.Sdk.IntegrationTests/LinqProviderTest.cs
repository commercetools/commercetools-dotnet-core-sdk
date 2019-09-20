using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.ProductProjections;
using Xunit;

namespace commercetools.Sdk.IntegrationTests
{
    [Collection("Integration Tests")]
    public class LinqProviderTest
    {
        private readonly IClient client;

        public LinqProviderTest(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task QueryTakeSkipTake()
        {
            var query = from c in this.client.Query<Category>()
                where c.Key == "category-foo"
                select c;

            query = query.Take(3).Skip(1).Take(4);

            var parameters = (query.Provider as ClientQueryProvider<Category>)?.Command.QueryParameters as QueryCommandParameters;

            Assert.Equal(1, parameters?.Offset);
            Assert.Equal(2, parameters?.Limit);
        }

        [Fact]
        public async Task QuerySkipTakeSkip()
        {
            var query = from c in this.client.Query<Category>()
                where c.Key == "category-foo"
                select c;

            query = query.Skip(4).Take(3).Skip(1);  // that should yield 3 items from offset 5

            var parameters = (query.Provider as ClientQueryProvider<Category>)?.Command.QueryParameters as QueryCommandParameters;

            Assert.Equal(5, parameters?.Offset);
            Assert.Equal(2, parameters?.Limit);

        }

        [Fact]
        public async Task QueryTakeSkip()
        {
            var query = from c in this.client.Query<Category>()
                where c.Key == "category-foo"
                select c;

            query = query.Take(3).Skip(2);          // that should yield 1 item from offset 2

            var parameters = (query.Provider as ClientQueryProvider<Category>)?.Command.QueryParameters as QueryCommandParameters;

            Assert.Equal(2, parameters?.Offset);
            Assert.Equal(1, parameters?.Limit);
        }

        [Fact]
        public async Task SearchTakeSkipTake()
        {
            var query = from c in this.client.SearchProducts()
                where c.Key == "category-foo"
                select c;

            query = query.Take(3).Skip(1).Take(4);

            var parameters = (query.Provider as ClientProductProjectionSearchProvider)?.Command.SearchParameters as ProductProjectionSearchParameters;

            Assert.Equal(1, parameters?.Offset);
            Assert.Equal(2, parameters?.Limit);
        }

        [Fact]
        public async Task SearchSkipTakeSkip()
        {
            var query = from c in this.client.SearchProducts()
                where c.Key == "category-foo"
                select c;

            query = query.Skip(4).Take(3).Skip(1);  // that should yield 3 items from offset 5

            var parameters = (query.Provider as ClientProductProjectionSearchProvider)?.Command.SearchParameters as ProductProjectionSearchParameters;

            Assert.Equal(5, parameters?.Offset);
            Assert.Equal(2, parameters?.Limit);

        }

        [Fact]
        public async Task SearchTakeSkip()
        {
            var query = from c in this.client.SearchProducts()
                where c.Key == "category-foo"
                select c;

            query = query.Take(3).Skip(2);          // that should yield 1 item from offset 2

            var parameters = (query.Provider as ClientProductProjectionSearchProvider)?.Command.SearchParameters as ProductProjectionSearchParameters;

            Assert.Equal(2, parameters?.Offset);
            Assert.Equal(1, parameters?.Limit);
        }
    }
}
