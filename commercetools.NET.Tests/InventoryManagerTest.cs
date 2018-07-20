using commercetools.Carts;
using commercetools.Categories;
using commercetools.Channels;
using commercetools.Common;
using commercetools.Inventory;
using commercetools.Inventory.UpdateActions;
using commercetools.Project;
using commercetools.Types;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace commercetools.Tests
{

    /// <summary>
    /// Test the API methods in the InventoryManager class.
    /// </summary>
    [TestFixture]
    public class InventoryManagerTest
    {
        private Client _client;
        private Project.Project _project;
        private List<InventoryEntry> _testInventories;
        
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

            _testInventories = new List<InventoryEntry>();

            for (int i = 0; i < 5; i++)
            {
                InventoryEntryDraft inventoryDraft = Helper.GetTestInventoryDraft(_project);

                Task<Response<InventoryEntry>> inventoryTask = _client.Inventories().CreateInventoryEntryAsync(inventoryDraft);
                inventoryTask.Wait();
                Assert.IsTrue(inventoryTask.Result.Success);

                InventoryEntry inventory = inventoryTask.Result.Result;
                Assert.NotNull(inventory.Id);

                _testInventories.Add(inventory);
                
            }            

        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
            foreach (InventoryEntry inventory in _testInventories)
            {
                Task<Response<InventoryEntry>> inventoryTask = _client.Inventories().DeleteInventoryEntryAsync(inventory);
                inventoryTask.Wait();
            }
        }

        /// <summary>
        /// Tests the InventoryManager.GetInventoryEntryByIdAsync method.
        /// </summary>
        /// <see cref="InventoryEntryManager.GetInventoryByIdAsync"/>
        [Test]
        public async Task ShouldGetInventoryByIdAsync()
        {
            Response<InventoryEntry> response = await _client.Inventories().GetInventoryEntryByIdAsync(_testInventories[0].Id);
            Assert.IsTrue(response.Success);

            InventoryEntry inventory = response.Result;
            Assert.NotNull(inventory.Id);
            Assert.AreEqual(inventory.Id, _testInventories[0].Id);
        }

        /// <summary>
        /// Tests the InventoryManager.QueryInventoriesAsync method.
        /// </summary>
        /// <see cref="InventoryManager.QueryInventoriesAsync"/>
        [Test]
        public async Task ShouldQueryInventoriesAsync()
        {
            Response<InventoryEntryQueryResult> response = await _client.Inventories().QueryInventoryAsync();
            Assert.IsTrue(response.Success);

            InventoryEntryQueryResult inventoryQueryResult = response.Result;
            Assert.NotNull(inventoryQueryResult.Results);
            Assert.GreaterOrEqual(inventoryQueryResult.Results.Count, 1);

            int limit = 2;
            response = await _client.Inventories().QueryInventoryAsync(limit: limit);
            Assert.IsTrue(response.Success);

            inventoryQueryResult = response.Result;
            Assert.NotNull(inventoryQueryResult.Results);
            Assert.LessOrEqual(inventoryQueryResult.Results.Count, limit);
        }

        /// <summary>
        /// Tests the InventoryManager.CreateInventoryAsync and InventoryManager.DeleteInventoryAsync methods.
        /// </summary>
        /// <see cref="InventoryManager.CreateInventoryAsync"/>
        /// <seealso cref="InventoryManager.DeleteInventoryAsync(commercetools.Inventories.Inventory)"/>
        [Test]
        public async Task ShouldCreateAndDeleteInventoryAsync()
        {
            InventoryEntryDraft inventoryDraft = Helper.GetTestInventoryDraft(_project);
            Response<InventoryEntry> response = await _client.Inventories().CreateInventoryEntryAsync(inventoryDraft);
            Assert.IsTrue(response.Success);

            InventoryEntry inventory = response.Result;
            Assert.NotNull(inventory.Id);

            string deletedInventoryId = inventory.Id;

            response = await _client.Inventories().DeleteInventoryEntryAsync(inventory);
            Assert.IsTrue(response.Success);

            response = await _client.Inventories().GetInventoryEntryByIdAsync(deletedInventoryId);
            Assert.IsFalse(response.Success);
        }

        /// <summary>
        /// Tests the InventoryManager.ShouldUpdateInventoryAndChangeQuantityAsync method.
        /// </summary>
        /// <see cref="InventoryManager.UpdateInventoryEntryAsync(commercetools.Inventories.Inventory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldUpdateInventoryAndChangeQuantityAsync()
        {
            int newQuantity = Helper.GetRandomNumber(10,20);
            ChangeQuantityAction changeQuantityAction = new ChangeQuantityAction(newQuantity);
            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(changeQuantityAction);

            Response<InventoryEntry> response = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], actions);

            Assert.IsTrue(response.Success);
            Assert.NotNull(response.Result.Id);

            _testInventories[0] = response.Result;

            Assert.AreEqual(_testInventories[0].QuantityOnStock, newQuantity);
            
        }

        /// <summary>
        /// Tests the InventoryManager.ShouldRemoveQuantityAsync method.
        /// </summary>
        /// <see cref="InventoryManager.UpdateInventoryEntryAsync(commercetools.Inventories.Inventory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldRemoveQuantityAsync()
        {
            int quantityToRemove = Helper.GetRandomNumber(1, 10);
            int currentInventoryQuantity = _testInventories[0].AvailableQuantity;
            RemoveQuantityAction removeQuantityAction = new RemoveQuantityAction(quantityToRemove);
            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(removeQuantityAction);

            Response<InventoryEntry> response = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], actions);

            Assert.IsTrue(response.Success);
            Assert.NotNull(response.Result);

            _testInventories[0] = response.Result;

            Assert.AreEqual(_testInventories[0].AvailableQuantity, currentInventoryQuantity - quantityToRemove);

        }
           

        /// <summary>
        /// Tests the InventoryManager.ShouldSetExpectedDeliveryAsync method.
        /// </summary>
        /// <see cref="InventoryManager.UpdateInventoryEntryAsync(commercetools.Inventories.Inventory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldSetExpectedDeliveryAsync()
        {
            // Arrange
            System.DateTime dateExpectedDelivery = System.DateTime.UtcNow;
            SetExpectedDeliveryAction setExpectedDeliveryAction = new SetExpectedDeliveryAction(dateExpectedDelivery); 
            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(setExpectedDeliveryAction);

            //Act
            Response<InventoryEntry> response = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], actions);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.NotNull(response.Result);

            _testInventories[0] = response.Result;

            Assert.AreEqual(_testInventories[0].ExpectedDelivery.ToString(), dateExpectedDelivery.ToString());

        }


        /// <summary>
        /// Tests the InventoryManager.ShouldSetRestockableInDaysAsync method.
        /// </summary>
        /// <see cref="InventoryManager.UpdateInventoryAsync(commercetools.Inventories.Inventory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldSetRestockableInDaysAsync()
        {
            // Arrange
            int restockableInDays = Helper.GetRandomNumber(1, 5);
            SetRestockableInDaysAction setRestockableInDaysAction = new SetRestockableInDaysAction(restockableInDays);
            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(setRestockableInDaysAction);

            //Act
            Response<InventoryEntry> response = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], actions);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.NotNull(response.Result);

            _testInventories[0] = response.Result;

            Assert.AreEqual(_testInventories[0].RestockableInDays, restockableInDays);

        }

        /// <summary>
        /// Tests the InventoryManager.ShouldSetSupplyChannelAsync method.
        /// </summary>
        /// <see cref="InventoryManager.UpdateInventoryAsync(commercetools.Inventories.Inventory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldSetSupplyChannelAsync()
        {
            // Arrange
            ChannelDraft channelDraft = Helper.GetTestChannelDraft(_project);
            Task<Response<Channel>> channelTask = _client.Channels().CreateChannelAsync(channelDraft);
            channelTask.Wait();
            Assert.IsTrue(channelTask.Result.Success);

            Channel channel = channelTask.Result.Result;

            Reference channelReference = new Reference();
            channelReference.Id = channel.Id;
            channelReference.ReferenceType = Common.ReferenceType.Channel;

            SetSupplyChannelAction setSupplyChannelAction = new SetSupplyChannelAction(channelReference);
            List<UpdateAction> actions = new List<UpdateAction>();
            actions.Add(setSupplyChannelAction);

            //Act
            Response<InventoryEntry> response = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], actions);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.NotNull(response.Result);

            _testInventories[0] = response.Result;

            Assert.AreEqual(_testInventories[0].SupplyChannel.Id, channel.Id);

        }

        /// <summary>
        /// Tests the InventoryManager.ShouldSetCustomTypeAndSetCustomFieldAsync method.
        /// </summary>
        /// <see cref="InventoryManager.UpdateInventoryAsync(commercetools.Inventories.Inventory, System.Collections.Generic.List{commercetools.Common.UpdateAction})"/>
        [Test]
        public async Task ShouldSetCustomTypeAndSetCustomFieldAsync()
        {
            // Arrange, create a type
            TypeDraft typeDraft = Helper.GetTypeDraftForInventory(_project);
            Task<Response<Type>> typeTask = _client.Types().CreateTypeAsync(typeDraft);
            typeTask.Wait();
            Assert.IsTrue(typeTask.Result.Success, "CreateType failed");
            Type testType = typeTask.Result.Result;
            
            ResourceIdentifier typeResourceIdentifier = new ResourceIdentifier
            {
                Id = testType.Id,
                TypeId = commercetools.Common.ReferenceType.Type
            };

            string fieldName = testType.FieldDefinitions[0].Name;

            JObject fields = new JObject();
            fields.Add(fieldName, Helper.GetRandomString(10));

            SetCustomTypeAction setCustomTypeAction = new SetCustomTypeAction
            {
                Type = typeResourceIdentifier,
                Fields = fields,
            };

            // Act 1, try set custom type
            Response<InventoryEntry> responseCustomType = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], setCustomTypeAction);

            // Assert
            Assert.IsTrue(responseCustomType.Success);
            _testInventories[0] = responseCustomType.Result;

            Assert.NotNull(_testInventories[0].Custom.Fields);
            Assert.AreEqual(fields[fieldName], _testInventories[0].Custom.Fields[fieldName]);

            // Arrange, change value
            fields[fieldName] = Helper.GetRandomString(10);

            SetCustomFieldAction setCustomFieldAction = new SetCustomFieldAction(fieldName);
            setCustomFieldAction.Value = fields[fieldName];

            // Act 2, try update custom field
            Response<InventoryEntry> cartResponse = await _client.Inventories().UpdateInventoryEntryAsync(_testInventories[0], setCustomFieldAction);

            // Assert
            Assert.IsTrue(cartResponse.Success);
            _testInventories[0] = cartResponse.Result;

            Assert.NotNull(_testInventories[0].Custom.Fields);
            Assert.AreEqual(fields[fieldName], _testInventories[0].Custom.Fields[fieldName]);
        }
    }
}