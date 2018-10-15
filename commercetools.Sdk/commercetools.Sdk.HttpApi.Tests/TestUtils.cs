using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.LinqToQueryPredicate;
using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TestUtils
    {
        public static ClientConfiguration GetClientConfiguration(string clientSettings)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return config.GetSection(clientSettings).Get<ClientConfiguration>();
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static IClient SetupClient()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null, null, null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager, serializerService);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
            ILogger logger = (new LoggerFactory()).CreateLogger("somecategory");
            TimestampHandler timestampHandler = new TimestampHandler(logger);
            CorrelationIdHandler correlationIdHandler = new CorrelationIdHandler(logger);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler, timestampHandler, correlationIdHandler);
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            IExpansionExpressionVisitor expansionExpressionVisitor = new ExpansionExpressionVisitor();
            ISortExpressionVisitor sortExpressionVisitor = new SortExpressionVisitor();
            GetByIdRequestMessageBuilder getByIdRequestMessageBuilder = new GetByIdRequestMessageBuilder(clientConfiguration);
            GetByKeyRequestMessageBuilder getByKeyRequestMessageBuilder = new GetByKeyRequestMessageBuilder(clientConfiguration);
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateByIdRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateByIdRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateByKeyRequestMessageBuilder updateByKeyRequestMessageBuilder = new UpdateByKeyRequestMessageBuilder(serializerService, clientConfiguration);
            DeleteByIdRequestMessageBuilder deleteByIdRequestMessageBuilder = new DeleteByIdRequestMessageBuilder(clientConfiguration);
            DeleteByKeyRequestMessageBuilder deleteByKeyRequestMessageBuilder = new DeleteByKeyRequestMessageBuilder(clientConfiguration);
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(clientConfiguration, queryPredicateExpressionVisitor, expansionExpressionVisitor, sortExpressionVisitor);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder, getByKeyRequestMessageBuilder, createRequestMessageBuilder, updateByIdRequestMessageBuilder, updateByKeyRequestMessageBuilder, deleteByKeyRequestMessageBuilder, deleteByIdRequestMessageBuilder, queryRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IEnumerable<Type> registeredHttpApiCommandTypes = new List<Type>() { typeof(CreateHttpApiCommand<>), typeof(QueryHttpApiCommand<>), typeof(GetByIdHttpApiCommand<>), typeof(GetByKeyHttpApiCommand<>), typeof(UpdateByKeyHttpApiCommand<>), typeof(UpdateByIdHttpApiCommand<>), typeof(DeleteByKeyHttpApiCommand<>), typeof(DeleteByIdHttpApiCommand<>) };
            IHttpApiCommandFactory httpApiCommandFactory = new HttpApiCommandFactory(registeredHttpApiCommandTypes, requestMessageBuilderFactory);

            IClient commerceToolsClient = new Client(httpClientFactory, httpApiCommandFactory, serializerService);
            return commerceToolsClient;
        }

        public static ISerializerService GetSerializerService()
        {
            IEnumerable<ICustomConverter<Sdk.Domain.Attribute>> customAttributeConverters = new List<ICustomConverter<Sdk.Domain.Attribute>>()
            {
                new MoneyAttributeConverter(),
                new TextAttributeConverter(),
                new LocalizedTextAttributeConverter(),
                new BooleanAttributeConverter(),
                new NumberAttributeConverter(),
                new DateTimeAttributeConverter(),
                new TimeAttributeConverter(),
                new DateAttributeConverter(),
                new EnumAttributeConverter(),
                new LocalizedEnumAttributeConverter()
            };
            IEnumerable<ICustomConverter<Money>> customMoneyConverters = new List<ICustomConverter<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            ErrorConverter errorConverter = new ErrorConverter();
            AttributeConverter attributeConverter = new AttributeConverter(customAttributeConverters, moneyConverter);
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter, errorConverter };
            
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService;
        }
    }
}