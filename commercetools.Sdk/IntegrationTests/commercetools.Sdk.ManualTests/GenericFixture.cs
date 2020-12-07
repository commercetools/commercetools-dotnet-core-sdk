using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Products.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.ManualTests
{
    public static class GenericFixture
    {
        public static async Task DeleteResource<T>(IClient client, T obj) where T : Resource<T>
        {
            try
            {
                await client.ExecuteAsync(new DeleteByIdCommand<T>(obj));
            }
            catch (NotFoundException)
            {
            }
            catch (ConcurrentModificationException concurrentModificationException)
            {
                var currentVersion = concurrentModificationException.GetCurrentVersion();
                if (currentVersion.HasValue)
                {
                    await client
                        .ExecuteAsync(new DeleteByIdCommand<T>(obj.Id, currentVersion.Value));
                }
            }
        }
        
        public static async Task DeleteResources<T>(IClient client) where T : Resource<T>
        {
            try
            {
                var returnedSet = await client.ExecuteAsync(new QueryCommand<T>());
                foreach (var resource in returnedSet.Results)
                {
                    await DeleteResource(client, resource);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        public static async Task DeleteProducts(IClient client)
        {
            try
            {
                var returnedSet = await client.ExecuteAsync(new QueryCommand<Product>());
                foreach (var product in returnedSet.Results)
                {
                    var toBeDeleted = product;
                    if (product.MasterData.Published)
                    {
                        toBeDeleted = await Unpublish(client, product);
                    }

                    await DeleteResource(client, toBeDeleted);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        public static async Task<Product> Unpublish(IClient client, Product product)
        {
            var updateActions = new List<UpdateAction<Product>>
            {
                new UnpublishUpdateAction()
            };
            var retrievedProduct = await client.ExecuteAsync(new UpdateByIdCommand<Product>(product, updateActions));
            return retrievedProduct;
        }
    }
}