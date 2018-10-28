using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.LinqToQueryPredicate;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class RequestBuilderTests
    {
        [Fact]
        public void GetRequestMessageBuilderFromFactory()
        {
            ISerializerService serializerService = TestUtils.GetSerializerService();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            GetRequestMessageBuilder getByIdRequestMessageBuilder = new GetRequestMessageBuilder(clientConfiguration);
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            UpdateRequestMessageBuilder updateByIdRequestMessageBuilder = new UpdateRequestMessageBuilder(serializerService, clientConfiguration);
            DeleteRequestMessageBuilder deleteByIdRequestMessageBuilder = new DeleteRequestMessageBuilder(clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { getByIdRequestMessageBuilder, createRequestMessageBuilder, updateByIdRequestMessageBuilder, deleteByIdRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IRequestMessageBuilder requestMessageBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetRequestMessageBuilder>();
            Assert.Equal(typeof(GetRequestMessageBuilder), requestMessageBuilder.GetType());
        }

        [Fact]
        public void GetHttpApiCommand()
        {
            CreateCommand<Category> createCommand = new CreateCommand<Category>(new CategoryDraft());
            IEnumerable<Type> registeredTypes = new List<Type>() { typeof(CreateHttpApiCommand<>) };
            ISerializerService serializerService = TestUtils.GetSerializerService();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            CreateRequestMessageBuilder createRequestMessageBuilder = new CreateRequestMessageBuilder(serializerService, clientConfiguration);
            IEnumerable<IRequestMessageBuilder> requestMessageBuilders = new List<IRequestMessageBuilder>() { createRequestMessageBuilder };
            IRequestMessageBuilderFactory requestMessageBuilderFactory = new RequestMessageBuilderFactory(requestMessageBuilders);
            IHttpApiCommandFactory httpApiCommandFactory = new HttpApiCommandFactory(registeredTypes, requestMessageBuilderFactory);
            IHttpApiCommand httpApiCommand = httpApiCommandFactory.Create(createCommand);
            Assert.Equal(typeof(CreateHttpApiCommand<Category>), httpApiCommand.GetType());
        }

        [Fact]
        public void GetQueryRequestMessageWithExpand()
        {
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> parentExpansion = new ReferenceExpansion<Category>(c => c.Parent);
            expansions.Add(parentExpansion);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>() { Expand = expansions };
            IEnumerable<Type> registeredTypes = new List<Type>() { typeof(QueryHttpApiCommand<>) };
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            ISortExpressionVisitor sortExpressionVisitor = new SortExpressionVisitor();
            IExpansionExpressionVisitor expansionExpressionVisitor = new ExpansionExpressionVisitor();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(clientConfiguration, queryPredicateExpressionVisitor, expansionExpressionVisitor, sortExpressionVisitor);
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("https://api.sphere.io/portablevendor/categories?expand=parent", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public void GetQueryRequestMessageWithTwoExpands()
        {
            List<Expansion<Category>> expansions = new List<Expansion<Category>>();
            ReferenceExpansion<Category> parentExpansion = new ReferenceExpansion<Category>(c => c.Parent);
            ReferenceExpansion<Category> firstAncestorExpansion = new ReferenceExpansion<Category>(c => c.Ancestors[0]);
            expansions.Add(parentExpansion);
            expansions.Add(firstAncestorExpansion);
            QueryCommand<Category> queryCommand = new QueryCommand<Category>() { Expand = expansions };
            IEnumerable<Type> registeredTypes = new List<Type>() { typeof(QueryHttpApiCommand<>) };
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            ISortExpressionVisitor sortExpressionVisitor = new SortExpressionVisitor();
            IExpansionExpressionVisitor expansionExpressionVisitor = new ExpansionExpressionVisitor();
            IClientConfiguration clientConfiguration = TestUtils.GetClientConfiguration("Client");
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(clientConfiguration, queryPredicateExpressionVisitor, expansionExpressionVisitor, sortExpressionVisitor);
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("https://api.sphere.io/portablevendor/categories?expand=parent&expand=ancestors%5B0%5D", httpRequestMessage.RequestUri.ToString());
        }
    }
}
