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
using Type = System.Type;

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
            IFilterExpressionVisitor filterExpressionVisitor = new FilterExpressionVisitor();
            GetRequestMessageBuilder getByIdRequestMessageBuilder = new GetRequestMessageBuilder(clientConfiguration);
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateRequestMessageBuilder(serializerService, clientConfiguration);
            DeleteRequestMessageBuilder deleteByIdRequestMessageBuilder = new DeleteRequestMessageBuilder(clientConfiguration);
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(clientConfiguration, queryPredicateExpressionVisitor, expansionExpressionVisitor, sortExpressionVisitor);
            SearchRequestMessageBuilder searchRequestMessageBuilder = new SearchRequestMessageBuilder(clientConfiguration, filterExpressionVisitor);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder, createRequestMessageBuilder, updateByIdRequestMessageBuilder, deleteByIdRequestMessageBuilder, queryRequestMessageBuilder, searchRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IEnumerable<Type> registeredHttpApiCommandTypes = new List<Type>() { typeof(SearchHttpApiCommand<>), typeof(CreateHttpApiCommand<>), typeof(QueryHttpApiCommand<>), typeof(GetHttpApiCommand<>), typeof(UpdateHttpApiCommand<>), typeof(DeleteHttpApiCommand<>) };
            IHttpApiCommandFactory httpApiCommandFactory = new HttpApiCommandFactory(registeredHttpApiCommandTypes, requestMessageBuilderFactory);

            IClient commerceToolsClient = new Client(httpClientFactory, httpApiCommandFactory, serializerService);
            return commerceToolsClient;
        }

        public static ISerializerService GetSerializerService()
        {
            IEnumerable<ICustomJsonMapper<Sdk.Domain.Attribute>> customAttributeConverters = new List<ICustomJsonMapper<Sdk.Domain.Attribute>>()
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
            IEnumerable<ICustomJsonMapper<Money>> customMoneyConverters = new List<ICustomJsonMapper<Money>>()
            {
                new CentPrecisionMoneyConverter(),
                new HighPrecisionMoneyConverter()
            };
            MoneyConverter moneyConverter = new MoneyConverter(customMoneyConverters);
            ErrorConverter errorConverter = new ErrorConverter();
            FacetResultConverter facetResultConverter = new FacetResultConverter();
            AttributeConverter attributeConverter = new AttributeConverter(customAttributeConverters);
            IEnumerable<JsonConverter> registeredConverters = new List<JsonConverter>() { moneyConverter, attributeConverter, errorConverter, facetResultConverter };
            
            CustomContractResolver customContractResolver = new CustomContractResolver(registeredConverters);
            JsonSerializerSettingsFactory jsonSerializerSettingsFactory = new JsonSerializerSettingsFactory(customContractResolver);
            ISerializerService serializerService = new SerializerService(jsonSerializerSettingsFactory);
            return serializerService;
        }
    }
}