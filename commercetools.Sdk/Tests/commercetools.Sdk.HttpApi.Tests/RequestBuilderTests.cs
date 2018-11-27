using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class RequestBuilderTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public RequestBuilderTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void GetHttpApiCommand()
        {
            CreateCommand<Category> createCommand = new CreateCommand<Category>(new CategoryDraft());
            IHttpApiCommandFactory httpApiCommandFactory = this.clientFixture.GetService<IHttpApiCommandFactory>();
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
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IClientConfiguration>(),
                this.clientFixture.GetService<IQueryPredicateExpressionVisitor>(),
                this.clientFixture.GetService<IExpansionExpressionVisitor>(),
                this.clientFixture.GetService<ISortExpressionVisitor>());
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
            QueryRequestMessageBuilder queryRequestMessageBuilder = new QueryRequestMessageBuilder(
                this.clientFixture.GetService<IClientConfiguration>(),
                this.clientFixture.GetService<IQueryPredicateExpressionVisitor>(),
                this.clientFixture.GetService<IExpansionExpressionVisitor>(),
                this.clientFixture.GetService<ISortExpressionVisitor>());
            HttpRequestMessage httpRequestMessage = queryRequestMessageBuilder.GetRequestMessage(queryCommand);
            Assert.Equal("https://api.sphere.io/portablevendor/categories?expand=parent&expand=ancestors%5B0%5D", httpRequestMessage.RequestUri.ToString());
        }

        [Fact]
        public void GetRequestMessageBuilderFromFactory()
        {
            IRequestMessageBuilderFactory requestMessageBuilderFactory = this.clientFixture.GetService<IRequestMessageBuilderFactory>();
            IRequestMessageBuilder requestMessageBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetRequestMessageBuilder>();
            Assert.Equal(typeof(GetRequestMessageBuilder), requestMessageBuilder.GetType());
        }
    }
}