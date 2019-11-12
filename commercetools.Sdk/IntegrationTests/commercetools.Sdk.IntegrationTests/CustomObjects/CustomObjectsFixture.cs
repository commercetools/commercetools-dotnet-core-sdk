using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.CustomObjects;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.CustomObjects
{
    public class CustomObjectsFixture
    {
        #region DraftBuilds

        public static CustomObjectDraft<T> DefaultCustomObjectDraft<T>(CustomObjectDraft<T> customObjectDraft, T value)
        {
            var randomInt = TestingUtility.RandomInt();
            customObjectDraft.Key = $"Key{randomInt}";
            customObjectDraft.Container = TestingUtility.DefaultContainerName;
            customObjectDraft.Value = value;
            return customObjectDraft;
        }
        
        public static CustomObjectDraft<T> DefaultCustomObjectDraftWithContainerName<T>(CustomObjectDraft<T> draft, string containerName)
        {
            var value = Activator.CreateInstance<T>();
            var customObjectDraft = DefaultCustomObjectDraft(draft, value);
            customObjectDraft.Container = containerName;
            return customObjectDraft;
        }

        public static CustomObjectDraft<T> DefaultCustomObjectDraftWithKey<T>(CustomObjectDraft<T> draft, T value,
            string key)
        {
            var customObjectDraft = DefaultCustomObjectDraft(draft, value);
            customObjectDraft.Key = key;
            return customObjectDraft;
        }

        #endregion

        #region Create&Delete_CustomObject

        public static async Task<CustomObject<T>> CreateCustomObject<T>(IClient client,
            IDraft<CustomObject<T>> buildDraft)
        {
            return await client
                .ExecuteAsync(new CustomObjectUpsertCommand<T>(buildDraft));
        }

        public static async Task DeleteCustomObject(IClient client, CustomObjectBase obj)
        {
            try
            {
                await client.ExecuteAsync(new DeleteByIdCommand<CustomObject>(obj.Id, obj.Version));
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
                        .ExecuteAsync(new DeleteByIdCommand<CustomObjectBase>(obj.Id, currentVersion.Value));
                }
            }
        }

        #endregion

        #region WithCustomObject

        public static async Task WithCustomObject<T>(
            IClient client,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Action<CustomObject<T>> func
        )
        {
            await WithCustomObject(client, new CustomObjectDraft<T>(), draftAction, func);
        }

        public static async Task WithCustomObject<T>(
            IClient client,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Func<CustomObject<T>, Task> func
        )
        {
            await WithCustomObjectAsync(client, new CustomObjectDraft<T>(), draftAction, func);
        }
        
        public static async Task WithListOfCustomObject<T>(
            IClient client,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            int count,
            Func<List<CustomObject<T>>, Task> func
        )
        {
            await WithListOfCustomObjectsAsync(client, new CustomObjectDraft<T>(), draftAction, func, count);
        }

        public static async Task WithCustomObject<T>(
            IClient client,
            CustomObjectDraft<T> draft,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Action<CustomObject<T>> func
        )
        {
            var buildDraft = draftAction.Invoke(draft);
            var resource = await CreateCustomObject(client, buildDraft);

            try
            {
                func(resource);
            }
            finally
            {
                await DeleteCustomObject(client, resource);
            }
        }

        public static async Task WithCustomObjectAsync<T>(
            IClient client,
            CustomObjectDraft<T> draft,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Func<CustomObject<T>, Task> func
        )
        {
            var buildDraft = draftAction.Invoke(draft);
            var resource = await CreateCustomObject(client, buildDraft);

            try
            {
                await func(resource);
            }
            finally
            {
                await DeleteCustomObject(client, resource);
            }
        }

        public static async Task WithListOfCustomObjectsAsync<T>(
            IClient client,
            CustomObjectDraft<T> draft,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Func<List<CustomObject<T>>, Task> func,
            int count = 2
        )
        {
            var resourcesList = new List<CustomObject<T>>();
            for (int i = 1; i <= count; i++)
            {
                var buildDraft = draftAction.Invoke(draft);
                var resource = await CreateCustomObject(client, buildDraft);
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
                    await DeleteCustomObject(client, resource);
                }
            }
        }

        #endregion

        #region WithUpdateableCustomObject

        public static async Task WithUpdateableCustomObjectAsync<T>(
            IClient client,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Func<CustomObject<T>, CustomObjectDraft<T>, Task<CustomObject<T>>> func
        )
        {
            await WithUpdateableCustomObjectAsync(client, new CustomObjectDraft<T>(), draftAction, func);
        }

        public static async Task WithUpdateableCustomObjectAsync<T>(
            IClient client,
            CustomObjectDraft<T> draft,
            Func<CustomObjectDraft<T>, CustomObjectDraft<T>> draftAction,
            Func<CustomObject<T>, CustomObjectDraft<T>, Task<CustomObject<T>>> func
        )
        {
            var buildDraft = draftAction.Invoke(draft);
            var resource = await CreateCustomObject(client, buildDraft);
            var updatedResource = default(CustomObject<T>);

            try
            {
                updatedResource = await func(resource, buildDraft);
            }
            finally
            {
                await DeleteCustomObject(client, updatedResource ?? resource);
            }
        }

        #endregion
    }

    //Custom Types for CustomObjects
    /// <summary>
    /// A demo class for a value of a custom object
    /// </summary>
    public class FooBar
    {
        public string Bar { get; set; }

        public FooBar()
        {
            this.Bar = "bar";
        }

        public FooBar(string bar)
        {
            this.Bar = bar;
        }
    }

    public class Foo
    {
        public string Value { get; set; }

        public Foo()
        {
            this.Value = "foo";
        }

        public Foo(string value)
        {
            this.Value = value;
        }
    }
}