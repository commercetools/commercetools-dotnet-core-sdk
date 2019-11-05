using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ClientFixture
    {
        public ServiceProviderFixture ServiceProviderFixture { get; }

        public ClientFixture(ServiceProviderFixture serviceProviderFixture)
        {
            this.ServiceProviderFixture = serviceProviderFixture;
        }

        public T GetService<T>()
        {
            return this.ServiceProviderFixture.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.ServiceProviderFixture.GetClientConfiguration(name);
        }
        public async Task<T> TryDeleteResource<T>(IVersioned<T> toBeDeleted)
        {
            var commerceToolsClient = this.GetService<IClient>();

            var deletedResource = default(T);

            try
            {
                deletedResource = await commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<T>(toBeDeleted.Id, toBeDeleted.Version));
            }
            catch (ConcurrentModificationException concurrentModificationException)
            {
                var currentVersion = concurrentModificationException.GetCurrentVersion();
                if (currentVersion.HasValue)
                {
                    deletedResource = await commerceToolsClient
                        .ExecuteAsync(new DeleteByIdCommand<T>(toBeDeleted.Id, currentVersion.Value));
                }
            }
            catch (NotFoundException exception)
            {
            }

            return deletedResource;
        }

    }
}
