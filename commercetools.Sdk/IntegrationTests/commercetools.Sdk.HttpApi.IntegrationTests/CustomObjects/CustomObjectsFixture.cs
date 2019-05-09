using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.CustomObject;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.Domain.Zones;
using commercetools.Sdk.Registration;
using Xunit.Abstractions;

namespace commercetools.Sdk.HttpApi.IntegrationTests.CustomObjects
{
    public class CustomObjectsFixture : ClientFixture, IDisposable
    {
        public List<CustomObjectBase> CustomObjectsToDelete { get; private set; }

        public CustomObjectsFixture(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            this.CustomObjectsToDelete = new List<CustomObjectBase>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.CustomObjectsToDelete.Reverse();

            foreach (var customObject in this.CustomObjectsToDelete)
            {
                CustomObject deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<CustomObject>(new Guid(customObject.Id), customObject.Version)).Result;
            }
        }

        /// <summary>
        /// Get CustomObject Draft
        /// </summary>
        /// <returns></returns>

        public CustomObjectDraft<T> GetCustomObjectDraft<T>(string container, string key,T value, int? version = null)
        {
            CustomObjectDraft<T> draft = new CustomObjectDraft<T>
            {
                Container = container,
                Key = key,
                Value = value,
                Version = version
            };
            return draft;
        }

        public CustomObjectDraft<FooBar> GetFooBarCustomObjectDraft(string container, string key, FooBar fooBar)
        {
            var draft = GetCustomObjectDraft(container, key, fooBar);
            return draft;
        }
        public CustomObjectDraft<Foo> GetFooCustomObjectDraft(string container, string key, Foo foo)
        {
            var draft = GetCustomObjectDraft(container, key, foo);
            return draft;
        }

        public CustomObject<FooBar> CreateFooBarCustomObject(string container = TestingUtility.DefaultContainerName, string key = null, FooBar fooBar = null)
        {
            key = key ?? TestingUtility.RandomString(10);
            fooBar = fooBar ?? new FooBar();

            var draft = GetFooBarCustomObjectDraft(container, key, fooBar);
            return CreateCustomObject(draft);
        }

        /// <summary>
        /// Create FooBar CustomObjects inside FooBarContainer
        /// </summary>
        public List<CustomObject<FooBar>> CreateMultipleFooBarCustomObjects(string container = TestingUtility.DefaultContainerName, int count = 3)
        {
            List<CustomObject<FooBar>> customObjects = new List<CustomObject<FooBar>>();
            for (var i = 1; i <= count; i++)
            {
                var fooBar = new FooBar($"FooBar_{i}");
                var fooBarDraft = GetFooBarCustomObjectDraft(container, TestingUtility.RandomString(10), fooBar);
                var fooBarCustomObject=CreateCustomObject(fooBarDraft);
                customObjects.Add(fooBarCustomObject);
            }

            return customObjects;
        }
        /// <summary>
        /// Create 2 custom objects with different types
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public List<CustomObjectBase> CreateCustomObjectsWithDifferentTypes(string container = TestingUtility.DefaultContainerName)
        {
            List<CustomObjectBase> customObjects = new List<CustomObjectBase>();

            //Add Foo Custom Object
            var fooDraft = GetFooCustomObjectDraft(container, TestingUtility.RandomString(10), new Foo());
            var fooCustomObject=CreateCustomObject(fooDraft);
            customObjects.Add(fooCustomObject);

            //Add FooBar Custom Object
            var fooBarDraft = GetFooBarCustomObjectDraft(container, TestingUtility.RandomString(10), new FooBar());
            var fooBarCustomObject=CreateCustomObject(fooBarDraft);
            customObjects.Add(fooBarCustomObject);

            return customObjects;
        }

        public CustomObject<T> CreateCustomObject<T>(CustomObjectDraft<T> customObjectDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            var customObject = commerceToolsClient.ExecuteAsync(new CustomObjectUpsertCommand<T>(customObjectDraft)).Result;
            return customObject;
        }



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
