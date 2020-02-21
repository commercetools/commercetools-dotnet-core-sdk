using System.Collections.Generic;
using System.IO;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Domain.OrderEdits.StagedActions;
using commercetools.Sdk.Domain.Orders.UpdateActions;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class OrderEditsTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public OrderEditsTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }
        
        [Fact]
        public void DeserializationOfOrderEditPreviewSuccess()
        {
            //Deserialize OrderEditResult (OrderEditPreviewSuccess)
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/OrderEdits/OrderEditPreviewSuccess.json");
            var result = serializerService.Deserialize<OrderEditResult>(serialized);
            Assert.IsType<OrderEditPreviewSuccess>(result);
            var orderEditPreviewSuccess = result as OrderEditPreviewSuccess;
            Assert.NotNull(orderEditPreviewSuccess);
            Assert.True(orderEditPreviewSuccess.Preview.LineItems.Count > 0);
            Assert.True(orderEditPreviewSuccess.MessagePayloads.Count > 0);
        }
        
        [Fact]
        public void SerializeStagedOrderUpdateAction()
        {
            //Deserialize OrderEditResult (OrderEditPreviewSuccess)
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var setCustomerEmailAction = new SetCustomerEmailUpdateAction {
                Email = "TestEmail"
            };
            var stagedActions = new List<IStagedOrderUpdateAction>
            {
                setCustomerEmailAction
            };
            var serialized = serializerService.Serialize(stagedActions);
            
            Assert.NotEmpty(serialized);
        }

        [Fact]
        public void DeserializeStageOrderUpdateAction()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var serialized = "{\"action\":\"setCustomerEmail\",\"email\":\"TestEmail\"}";
            var result = serializerService.Deserialize<IStagedOrderUpdateAction>(serialized);
            Assert.IsType<SetCustomerEmailUpdateAction>(result);
        }
    }
}