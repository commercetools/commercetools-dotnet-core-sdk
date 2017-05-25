using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Carts;
using commercetools.Customers;
using commercetools.Customers.UpdateActions;
using commercetools.Messages;
using commercetools.Project;
using commercetools.Types;

using NUnit.Framework;

using Type = commercetools.Types.Type;
using Newtonsoft.Json.Linq;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the TypeManager class.
    /// </summary>
    [TestFixture]
    public class TypeManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<Type> _testTypes;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());

            Task<Response<Project.Project>> projectTask = _client.Project().GetProjectAsync();
            projectTask.Wait();
            Assert.IsTrue(projectTask.Result.Success);
            _project = projectTask.Result.Result;

            Assert.IsTrue(_project.Languages.Count > 0);

            CustomerDraft customerDraft = Helper.GetTestCustomerDraft();
            Task<Response<CustomerCreatedMessage>> customerTask = _client.Customers().CreateCustomerAsync(customerDraft);
            customerTask.Wait();
            Assert.IsTrue(customerTask.Result.Success);

            _testTypes = new List<Type>();

            for (int i = 0; i < 5; i++)
            {
                TypeDraft typeDraft = Helper.GetTypeDraft(_project);
                Task<Response<Type>> typeTask = _client.Types().CreateTypeAsync(typeDraft);
                typeTask.Wait();
                Assert.IsTrue(typeTask.Result.Success);

                Type type = typeTask.Result.Result;
                Assert.NotNull(type.Id);

                _testTypes.Add(type);

            }
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            Task task;

            foreach (Type type in _testTypes)
            {
                task = _client.Types().DeleteTypeAsync(type);
                task.Wait();
            }
        }

        /// <summary>
        /// Tests the TypeManager.GetTypeByIdAsync method.
        /// </summary>
        /// <see cref="TypeManager.GetTypeByIdAsync"/>
        [Test]
        public async Task ShouldGetTypeByIdAsync()
        {
            Response<Type> response = await _client.Types().GetTypeByIdAsync(_testTypes[0].Id);
            Assert.IsTrue(response.Success);

            Type type = response.Result;
            Assert.NotNull(type.Id);
            Assert.AreEqual(type.Id, _testTypes[0].Id);
        }

        /// <summary>
        /// Tests the TypeManager.QueryTypesAsync method.
        /// </summary>
        /// <see cref="TypeManager.QueryTypesAsync"/>
        [Test]
        public async Task ShouldQueryShippingMethodsAsync()
        {
            Response<TypeQueryResult> response = await _client.Types().QueryTypesAsync();
            Assert.IsTrue(response.Success);

            TypeQueryResult typeQueryResult = response.Result;
            Assert.NotNull(typeQueryResult.Results);
            Assert.GreaterOrEqual(typeQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.Types().QueryTypesAsync(limit: limit);
            Assert.IsTrue(response.Success);

            typeQueryResult = response.Result;
            Assert.NotNull(typeQueryResult.Results);
            Assert.LessOrEqual(typeQueryResult.Results.Count, limit);
        }

        /// <summary>
        /// Tests the TypeManager.CreateTypeAsync and TypeManager.DeleteTypeAsync methods.
        /// </summary>
        /// <see cref="TypeManager.CreateTypeAsync"/>
        /// <seealso cref="TypeManager.DeleteTypeAsync(commercetools.Types.Type)"/>
        [Test]
        public async Task ShouldCreateAndDeleteTypeAsync()
        {
            TypeDraft typeDraft = Helper.GetTypeDraft(_project);
            Response<Type> typeResponse = await _client.Types().CreateTypeAsync(typeDraft);
            Assert.IsTrue(typeResponse.Success);

            Type type = typeResponse.Result;
            Assert.NotNull(type.Id);

            string deletedTypeId = type.Id;

            Response<JObject> deleteResponse = await _client.Types().DeleteTypeAsync(type);
            Assert.IsTrue(deleteResponse.Success);

            typeResponse = await _client.Types().GetTypeByIdAsync(deletedTypeId);
            Assert.IsFalse(typeResponse.Success);
        }

        /// <summary>
        /// Tests the TypeManager.UpdateTypeAsync method.
        /// </summary>
        /// <see cref="TypeManager.UpdateTypeAsync(commercetools.Types.Type, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateTypeAsync()
        {
            string randomPostfix = Helper.GetRandomString(10);
            LocalizedString newName = new LocalizedString();
            LocalizedString newDescription = new LocalizedString();

            foreach (string language in _project.Languages)
            {
                newName[language] = string.Concat("Test Type ", language, " ", randomPostfix);
                newDescription[language] = string.Concat("Test Description ", language, " ", randomPostfix);
            }

            GenericAction changeNameAction = new GenericAction("changeName");
            changeNameAction.SetProperty("name", newName);

            GenericAction setDescriptionAction = new GenericAction("setDescription");
            setDescriptionAction.SetProperty("description", newDescription);

            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeNameAction);
            actions.Add(setDescriptionAction);

            Response<Type> response = await _client.Types().UpdateTypeAsync(_testTypes[0], actions);
            Assert.IsTrue(response.Success);

            _testTypes[0] = response.Result;
            Assert.NotNull(_testTypes[0].Id);

            foreach (string language in _project.Languages)
            {
                Assert.AreEqual(_testTypes[0].Name[language], newName[language]);
                Assert.AreEqual(_testTypes[0].Description[language], newDescription[language]);
            }
        }
    }
}
