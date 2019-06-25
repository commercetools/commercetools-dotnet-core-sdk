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

            return deletedResource;
        }

        public async void AssertEventually(TimeSpan maxWaitTime, TimeSpan waitBeforeRetry, Action runnableBlock)
        {
            long timeOutAt = (int)DateTime.Now.TimeOfDay.TotalMilliseconds + (int)maxWaitTime.TotalMilliseconds;
            while (true)
            {
                try
                {
                    runnableBlock.Invoke();
                    // the block executed without throwing an exception, return
                    return;
                }
                catch (Exception ex)
                {
                    if ((int)DateTime.Now.TimeOfDay.TotalMilliseconds > timeOutAt) //if it's timeout
                    {
                        throw;
                    }

                    if (ex is ErrorResponseException errorResponseException &&
                        errorResponseException.ErrorResponse.Errors.Any(err =>
                            err.Code.Equals("SearchFacetPathNotFound")))
                    {
                        throw;
                    }
                }

                try
                {
                    await Task.Delay((int) waitBeforeRetry.TotalMilliseconds).ConfigureAwait(false);
                }
                catch (ThreadInterruptedException e)
                {
                    throw new SystemException(e.Message);
                }
            }
        }

    }
}
