using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.IntegrationTests
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

        public static async Task<T> CreateResource<T>(IClient client, IDraft<T> buildDraft) where T : Resource<T>
        {
            return await client
                .ExecuteAsync(new CreateCommand<T>(buildDraft));
        }


        public static async Task WithAsync<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Func<T, Task> func,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null
            ) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);
            var resource = await createFunc(client, buildDraft);

            try
            {
                await func(resource);
            }
            finally
            {
                await deleteFunc(client, resource);
            }
        }
        
        public static async Task WithListAsync<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Func<List<T>, Task> func,
            int count = 2,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null
        ) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;
            
            var resourcesList = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var buildDraft = draftAction.Invoke(draft);
                var resource = await createFunc(client, buildDraft);
                resourcesList.Add(resource);
            }

            try
            {
                await func(resourcesList);
            }
            finally
            {
                foreach (var resource in resourcesList)
                {
                    await deleteFunc(client, resource);   
                }
            }
        }

        public static async Task WithAsync<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Func<T, TDraft, Task> func,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null
        ) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);
            var resource = await createFunc(client, buildDraft);

            try
            {
                await func(resource, buildDraft);
            }
            finally
            {
                await deleteFunc(client, resource);
            }
        }

        public static async Task ImportWithAsync<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Func<T, TDraft, Task> func,
            Func<IClient, IImportDraft<T>, Task<T>> importFunc,
            Func<IClient, T, Task> deleteFunc = null
        ) where T : Resource<T> where TDraft : IImportDraft<T>
        {
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);
            var resource = await importFunc(client, buildDraft);

            try
            {
                await func(resource, buildDraft);
            }
            finally
            {
                await deleteFunc(client, resource);
            }
        }

        public static async Task WithUpdateableAsync<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Func<T, Task<T>> func,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);

            var resource = await createFunc(client, buildDraft);

            var updatedResource = default(T);

            try
            {
                updatedResource = await func(resource);
            }
            finally
            {
                await deleteFunc(client, updatedResource ?? resource);
            }
        }


        public static async Task With<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Action<T> func,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);

            var resource = await createFunc(client, buildDraft);

            try
            {
                func(resource);
            }
            finally
            {
                await deleteFunc(client, resource);
            }
        }

        public static async Task With<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Action<T, TDraft> func,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);

            var resource = await createFunc(client, buildDraft);

            try
            {
                func(resource, buildDraft);
            }
            finally
            {
                await deleteFunc(client, resource);
            }
        }

        public static async Task ImportWith<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Action<T, TDraft> func,
            Func<IClient, IImportDraft<T>, Task<T>> importFunc,
            Func<IClient, T, Task> deleteFunc = null) where T : Resource<T> where TDraft : IImportDraft<T>
        {
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);

            var resource = await importFunc(client, buildDraft);

            try
            {
                func(resource, buildDraft);
            }
            finally
            {
                await deleteFunc(client, resource);
            }
        }


        public static async Task WithUpdateable<T, TDraft>(
            IClient client,
            TDraft draft,
            Func<TDraft, TDraft> draftAction,
            Func<T, T> func,
            Func<IClient, IDraft<T>, Task<T>> createFunc = null,
            Func<IClient, T, Task> deleteFunc = null) where T : Resource<T> where TDraft : IDraft<T>
        {
            createFunc = createFunc ?? CreateResource;
            deleteFunc = deleteFunc ?? DeleteResource;

            var buildDraft = draftAction.Invoke(draft);

            var resource = await createFunc(client, buildDraft);

            var updatedResource = default(T);

            try
            {
                updatedResource = func(resource);
            }
            finally
            {
                await deleteFunc(client, updatedResource ?? resource);
            }
        }


        public static async Task AssertEventually(Func<Task> runnableBlock, int maxWaitTimeSecond = 180,
            int waitBeforeRetryMilliseconds = 100)
        {
            var maxWaitTime = TimeSpan.FromSeconds(maxWaitTimeSecond);
            var waitBeforeRetry = TimeSpan.FromMilliseconds(waitBeforeRetryMilliseconds);
            await AssertEventually(maxWaitTime, waitBeforeRetry, runnableBlock);
        }

        private static async Task AssertEventually(TimeSpan maxWaitTime, TimeSpan waitBeforeRetry, Func<Task> runnableBlock)
        {
            long timeOutAt = (int) DateTime.Now.TimeOfDay.TotalMilliseconds + (int) maxWaitTime.TotalMilliseconds;
            while (true)
            {
                try
                {
                    await runnableBlock.Invoke();
                    // the block executed without throwing an exception, return
                    return;
                }
                catch (Exception ex)
                {
                    if ((int) DateTime.Now.TimeOfDay.TotalMilliseconds > timeOutAt) //if it's timeout
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
                    Task.Delay((int) waitBeforeRetry.TotalMilliseconds).Wait();
                }
                catch (ThreadInterruptedException e)
                {
                    throw new SystemException(e.Message);
                }
            }
        }

    }
}
