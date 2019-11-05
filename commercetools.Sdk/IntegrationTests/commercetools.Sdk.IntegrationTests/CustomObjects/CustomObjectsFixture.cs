using System;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.CustomObject;
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
        
        public static CustomObjectDraft<T> DefaultCustomObjectDraftWithKey<T>(CustomObjectDraft<T> draft, T value, string key)
        {
            var customObjectDraft = DefaultCustomObjectDraft(draft, value);
            customObjectDraft.Key = key;
            return customObjectDraft;
        }
       
        #endregion

        #region Create&Delete_CustomObject
        public static async Task<CustomObject<T>> CreateCustomObject<T>(IClient client, IDraft<CustomObject<T>> buildDraft)
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
        
        #endregion

        #region WithUpdateableCustomObject

        

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