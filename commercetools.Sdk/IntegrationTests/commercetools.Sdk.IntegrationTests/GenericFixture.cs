using System;
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

    }
}
