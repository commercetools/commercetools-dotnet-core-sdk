using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.CommandBuilders;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;

namespace commercetools.Sdk.IntegrationTests.CommandBuilders
{
    [Collection("Integration Tests")]
    public class CommandBuildersIntegrationTests
    {
        private readonly IClient client;
        private readonly ServiceProviderFixture serviceProviderFixture;

        public CommandBuildersIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.serviceProviderFixture = serviceProviderFixture;
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public void Test()
        {
            var b = new CommandBuilder();
            
            //var getByIdInStore = b.Customers().GetById("whatever").InStore("string").Build();
            //var getByKetInStore = b.Customers().GetByKey("whatever").InStore("string").Build();

            //b.Customers().Query().Where()
            /*
            var c1 = b.Customers().Query().Build().Where(customer => customer.Locale == "DE").Where(customer => customer.Email == "foo");
            var c2 = b.Customers().Query().Build().Where(customer => customer.Locale == "DE").Where(customer => customer.Email == "bar");

            var builder = b.Customers().Query().InStore("");//.Where(customer => customer.Locale == "DE");
            var c3 = builder.Build().Where(customer => customer.Email == "foo");
            var c4 = builder.Build().Where(customer => customer.Email == "bar");
            */
            
            //            var ub = b.Customers().UpdateById("whatever").AddAction<AddDeliveryUpdateAction>(
//                action =>
//                {
//                    action.Address = new Address();
//                    return action;
//                }).Build();
        }
        
        /*
        [Fact]
        public async Task GetCategoryById()
        {
            var key = $"GetCategoryById-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    var retrievedCategory = await client.Categories()
                        .GetById(category.Id).Build().ExecuteAsync();
                    
                    Assert.Equal(key, retrievedCategory.Key);
                });
        }
        
        [Fact]
        public async Task GetCategoryByKey()
        {
            var key = $"GetCategoryByKey-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    var retrievedCategory = await client.Categories()
                        .GetByKey(category.Key).Build().ExecuteAsync();
                        
                    Assert.Equal(key, retrievedCategory.Key);
                });
        }
        
        
        [Fact]
        public async Task GetCustomerInStoreById()
        {
            //Get customer in specific store
            await WithStore(client, async store1 =>
            {
                var stores = new List<IReferenceable<Store>>
                {
                    store1.ToKeyResourceIdentifier(),
                };

                await WithCustomer(
                    client, customerDraft => DefaultCustomerDraftInStores(customerDraft, stores),
                    async customer =>
                    {
                        Assert.NotNull(customer);
                        Assert.Single(customer.Stores);

                        //using extension method
                        var retrievedCustomer =
                            await client.GetRequestBuilder<Customer>().GetInStoreById(customer.Id, store1.Key);

                        Assert.NotNull(retrievedCustomer);
                        Assert.Equal(customer.Id, retrievedCustomer.Id);
                        Assert.Single(retrievedCustomer.Stores);
                        Assert.Equal(store1.Key, retrievedCustomer.Stores[0].Key);
                    });
            });
        }
        */
        
    }
}