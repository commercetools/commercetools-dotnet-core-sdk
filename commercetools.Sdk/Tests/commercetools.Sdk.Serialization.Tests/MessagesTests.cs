using System.Collections.Generic;
using System.IO;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.APIExtensions;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.Messages.Categories;
using commercetools.Sdk.Domain.Messages.Customers;
using commercetools.Sdk.Domain.Messages.Orders;
using commercetools.Sdk.Domain.Messages.Reviews;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;
using commercetools.Sdk.Domain.Subscriptions;
using commercetools.Sdk.Domain.TaxCategories;
using commercetools.Sdk.Registration;
using FluentAssertions.Json;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace commercetools.Sdk.Serialization.Tests
{
    public class MessagesTests : IClassFixture<SerializationFixture>
    {
        private readonly SerializationFixture serializationFixture;

        public MessagesTests(SerializationFixture serializationFixture)
        {
            this.serializationFixture = serializationFixture;
        }

        [Fact]
        public void MessagesDeserializationOfSpecificType()
        {
            //Deserialize 2 of messages of type category to list of Message<Category>
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/CategoryMessages.json");
            var messages = serializerService.Deserialize<List<Message<Category>>>(serialized);
            var categoryCreatedMessage = messages[0] as CategoryCreatedMessage;
            var categorySlugChangedMessage = messages[1] as CategorySlugChangedMessage;
            Assert.NotNull(categoryCreatedMessage);
            Assert.NotNull(categoryCreatedMessage.Category);
            Assert.NotNull(categoryCreatedMessage.Resource);
            Assert.NotNull(categorySlugChangedMessage);
            Assert.NotNull(categorySlugChangedMessage.Slug);
        }

        [Fact]
        public void MessagesDeserializationOfDifferentTypes()
        {
            //Deserialize 3 of messages of different types
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/Messages.json");
            var messages = serializerService.Deserialize<List<Message>>(serialized);
            var categoryCreatedMessage = messages[0] as CategoryCreatedMessage;
            var customerCreatedMessage = messages[1] as CustomerCreatedMessage;
            var lineItemStateTransitionMessage = messages[2] as LineItemStateTransitionMessage;

            Assert.NotNull(categoryCreatedMessage);
            Assert.NotNull(categoryCreatedMessage.Category);

            Assert.NotNull(customerCreatedMessage);
            Assert.NotNull(customerCreatedMessage.Customer);

            Assert.NotNull(lineItemStateTransitionMessage);
        }

        [Fact]
        public void MessagesDeserializationOfReview()
        {
            //Deserialize 1 of messages of type category to list of Message<Review>
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/ReviewMessages.json");
            var messages = serializerService.Deserialize<List<Message>>(serialized);
            Assert.Single(messages);
            var reviewMessage = messages[0];
            Assert.IsAssignableFrom<Message<Review>>(reviewMessage);
            Assert.IsType<ReviewCreatedMessage>(reviewMessage);
        }

        [Fact]
        public void MessagesDeserializationToGeneralMessage()
        {
            //Deserialize 1 of messages as general message (when the message type not in the SDK)
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/ReviewMessages.json");
            var messages = serializerService.Deserialize<List<GeneralMessage>>(serialized);
            Assert.Single(messages);
            var generalMessage = messages[0];
            Assert.NotNull(generalMessage.Type);
        }

        [Fact]
        public void DeserializationOfResourceCreatedPayload()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/ResourceCreatedPayload.json");
            var payload = serializerService.Deserialize<Payload>(serialized);
            Assert.NotNull(payload);
            Assert.NotNull(payload.ResourceUserProvidedIdentifiers);
            Assert.Equal("test-ca15403ea56ec0e51137ff40a6f4607e", payload.ResourceUserProvidedIdentifiers.Key);
            Assert.IsType<ResourceCreatedPayload<Customer>>(payload);
            var customerCreatedPayload = payload as ResourceCreatedPayload<Customer>;
            Assert.NotNull(customerCreatedPayload);
            Assert.NotNull(customerCreatedPayload.Resource);
        }

        [Fact]
        public void DeserializationOfListOfChangeSubscriptionPayloads()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/Payloads.json");
            var payloads = serializerService.Deserialize<List<Payload>>(serialized);
            Assert.NotNull(payloads);
            Assert.Equal(4, payloads.Count);
            var resourceCreatedPayload = payloads[0] as ResourceCreatedPayload<Customer>;
            var resourceUpdatedPayload = payloads[1] as ResourceUpdatedPayload<Category>;
            var resourceDeletedPayload = payloads[2] as ResourceDeletedPayload<Product>;
            var customerCreatedPayload = payloads[3] as MessageSubscriptionPayload<Customer>;

            Assert.NotNull(resourceCreatedPayload);
            Assert.NotNull(resourceUpdatedPayload);
            Assert.NotNull(resourceDeletedPayload);
            Assert.NotNull(customerCreatedPayload);
            Assert.Equal(2, resourceUpdatedPayload.OldVersion);
        }

        [Fact]
        public void DeserializationOfInvalidSubscriptionPayload()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/InvalidPayload.json");
            var exception = Assert.Throws<JsonSerializationException>(() => serializerService.Deserialize<Payload>(serialized));
            Assert.Equal("Unknown subscription payload type: {\"notificationType\":\"ResourceCreated\",\"version\":1}", exception.Message);
        }

        [Fact]
        public void DeserializationOfListOfMessageSubscriptionPayload()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            var expectedCategoryId = "3df866bd-7e5f-47d1-bbe2-ca1d1f39a260";
            string serialized = File.ReadAllText("Resources/Messages/MessageSubscriptionPayload.json");
            var payload = serializerService.Deserialize<Payload>(serialized);
            Assert.NotNull(payload);
            var categoryCreatedPayload = payload as MessageSubscriptionPayload<Category>;
            Assert.NotNull(categoryCreatedPayload);
            Assert.Equal(expectedCategoryId, categoryCreatedPayload.Resource.Id);
            Assert.NotNull(categoryCreatedPayload.Message);
            var categoryCreatedMessage = categoryCreatedPayload.Message as CategoryCreatedMessage;
            Assert.NotNull(categoryCreatedMessage);
            Assert.Equal(expectedCategoryId,categoryCreatedMessage.Category.Id);
        }

        [Fact]
        public void DeserializationOfListOfMessageSubscriptionPayloads()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/MessageSubscriptionPayloads.json");
            var payloads = serializerService.Deserialize<List<Payload>>(serialized);
            Assert.NotNull(payloads);
            Assert.IsType<MessageSubscriptionPayload<Category>>(payloads[0]);
            Assert.IsType<MessageSubscriptionPayload<Customer>>(payloads[1]);
        }

        [Fact]
        public void DeserializeChangeSubscription()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;

            string serialized1 = @"
                {
                    ""ResourceTypeId"": ""cart-discount""
                }
            ";
            string serialized2 = @"
                {
                    ""ResourceTypeId"": ""new-type""
                }
            ";

            var changeSubscription1 = serializerService.Deserialize<ChangeSubscription>(serialized1);
            var changeSubscription2 = serializerService.Deserialize<ChangeSubscription>(serialized2);
            Assert.Equal(SubscriptionResourceTypeId.CartDiscount, changeSubscription1.GetResourceTypeIdAsEnum());
            Assert.Equal(SubscriptionResourceTypeId.Unknown, changeSubscription2.GetResourceTypeIdAsEnum());
        }

        [Fact]
        public void DeserializeOfSubscriptions()
        {
            ISerializerService serializerService = this.serializationFixture.SerializerService;
            string serialized = File.ReadAllText("Resources/Messages/Subscriptions.json");
            var subscriptions = serializerService.Deserialize<List<Subscription>>(serialized);
            Assert.Equal(2,subscriptions.Count);
            Assert.IsType<SqsDestination>(subscriptions[0].Destination);
            Assert.IsType<SnsDestination>(subscriptions[1].Destination);
            var subscription1 = subscriptions[0];
            var subscription2 = subscriptions[1];
            Assert.IsType<PlatformFormat>(subscription1.Format);
            var subscription2Format = subscription2.Format as CloudEventsFormat;
            Assert.NotNull(subscription2Format);
            Assert.Equal("0.1", subscription2Format.CloudEventsVersion);
            Assert.Equal(SubscriptionHealthStatus.Healthy, subscription1.Status);
            Assert.Equal(SubscriptionHealthStatus.TemporaryError, subscription2.Status);
            Assert.Single(subscription1.Messages);
            Assert.Single(subscription2.Changes);
            Assert.Equal(SubscriptionResourceTypeId.Product, subscription1.Messages[0].GetResourceTypeIdAsEnum());
            Assert.Equal(SubscriptionResourceTypeId.Unknown, subscription2.Changes[0].GetResourceTypeIdAsEnum());
        }
    }
}
