using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Categories.CategoriesFixture;
using static commercetools.Sdk.IntegrationTests.Types.TypesFixture;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.Categories.UpdateActions.SetDescriptionUpdateAction;

namespace commercetools.Sdk.IntegrationTests.Categories
{
    [Collection("Integration Tests")]
    public class CategoriesIntegrationTests
    {
        private readonly IClient client;
        private readonly ServiceProviderFixture serviceProviderFixture;

        public CategoriesIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.serviceProviderFixture = serviceProviderFixture;
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact]
        public async Task CreateCategory()
        {
            var key = $"CreateCategory-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                category => { Assert.Equal(key, category.Key); });
        }

        [Fact]
        public async Task GetCategoryById()
        {
            var key = $"GetCategoryById-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    var retrievedCategory = await client
                        .ExecuteAsync(category.ToIdResourceIdentifier().GetById());
                    Assert.Equal(key, retrievedCategory.Key);
                });
        }

        [Fact]
        public async Task GetCategoryByKey()
        {
            var key = $"GetCategoryByKey-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    var retrievedCategory = await client
                        .ExecuteAsync(category.ToKeyResourceIdentifier().GetByKey());
                    Assert.Equal(key, retrievedCategory.Key);
                });
        }

        [Fact]
        public async Task QueryCategories()
        {
            var key = $"QueryCategories-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    var queryCommand = new QueryCommand<Category>();
                    queryCommand.Where(c => c.Key == category.Key.valueOf());
                    var returnedSet = await client.ExecuteAsync(queryCommand);
                    Assert.Single(returnedSet.Results);
                    Assert.Equal(key, returnedSet.Results[0].Key);
                });
        }

        [Fact]
        public async Task QueryCategoryByParentAndExpandIt()
        {
            await WithCategory(client, async parentCategory =>
            {
                await WithCategory(
                    client, categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory),
                    async category =>
                    {
                        var queryCommand = new QueryCommand<Category>();
                        queryCommand.Where(c => c.Parent.Id == parentCategory.Id.valueOf());
                        var returnedSet = await client.ExecuteAsync(queryCommand.Expand(c => c.Parent));
                        Assert.Single(returnedSet.Results);
                        var retrievedCategory = returnedSet.Results[0];
                        Assert.Equal(category.Key, retrievedCategory.Key);
                        Assert.NotNull(retrievedCategory.Parent);
                        Assert.Equal(parentCategory.Id, retrievedCategory.Parent.Id);
                    });
            });
        }

        [Fact]
        public async Task QueryCategoriesByParentAndSort()
        {
            var count = 3;
            await WithCategory(client, async parentCategory =>
            {
                await WithListOfCategories(
                    client, categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory), count,
                    async categoriesList =>
                    {
                        Assert.Equal(count, categoriesList.Count);
                        var orderedCategoriesNames =
                            categoriesList.OrderBy(c => c.Name["en"]).Select(c => c.Name["en"]).ToList();
                        var queryCommand = new QueryCommand<Category>();
                        queryCommand.Where(c => c.Parent.Id == parentCategory.Id.valueOf());
                        queryCommand.Expand(c => c.Parent);
                        queryCommand.Sort(c => c.Name["en"]);
                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        var categoriesResult = returnedSet.Results;
                        Assert.Equal(count, categoriesResult.Count);
                        Assert.NotNull(categoriesList[0].Parent);
                        Assert.Equal(parentCategory.Id, categoriesResult[0].Parent.Id);
                        var returnedCategoriesNames = categoriesResult.Select(c => c.Name["en"]).ToList();
                        Assert.True(returnedCategoriesNames.SequenceEqual(orderedCategoriesNames));
                    });
            });
        }

        [Fact]
        public async Task QueryCategoriesByParentAndSortDescending()
        {
            var count = 3;
            await WithCategory(client, async parentCategory =>
            {
                await WithListOfCategories(
                    client, categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory), count,
                    async categoriesList =>
                    {
                        Assert.Equal(count, categoriesList.Count);
                        var orderedCategoriesNames = categoriesList.OrderByDescending(c => c.Name["en"])
                            .Select(c => c.Name["en"]).ToList();
                        var queryCommand = new QueryCommand<Category>();
                        queryCommand.Where(c => c.Parent.Id == parentCategory.Id.valueOf());
                        queryCommand.Expand(c => c.Parent);
                        queryCommand.Sort(c => c.Name["en"], SortDirection.Descending);
                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        var categoriesResult = returnedSet.Results;
                        Assert.Equal(count, categoriesResult.Count);
                        Assert.NotNull(categoriesList[0].Parent);
                        Assert.Equal(parentCategory.Id, categoriesResult[0].Parent.Id);
                        var returnedCategoriesNames = categoriesResult.Select(c => c.Name["en"]).ToList();
                        Assert.True(returnedCategoriesNames.SequenceEqual(orderedCategoriesNames));
                    });
            });
        }

        [Fact]
        public async Task QueryCategoriesByParentAndLimit()
        {
            var count = 3;
            await WithCategory(client, async parentCategory =>
            {
                await WithListOfCategories(
                    client, categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory), count,
                    async categoriesList =>
                    {
                        var limit = count - 1;
                        Assert.Equal(count, categoriesList.Count);
                        var queryCommand = new QueryCommand<Category>();
                        queryCommand.Where(c => c.Parent.Id == parentCategory.Id.valueOf());
                        queryCommand.Expand(c => c.Parent);
                        queryCommand.SetLimit(limit);
                        queryCommand.SetWithTotal(true);

                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        Assert.Equal(limit, returnedSet.Count);
                        Assert.Equal(limit, returnedSet.Limit);
                        Assert.Equal(count, returnedSet.Total);

                        var categoriesResult = returnedSet.Results;
                        Assert.NotNull(categoriesList[0].Parent);
                        Assert.Equal(parentCategory.Id, categoriesResult[0].Parent.Id);
                    });
            });
        }

        [Fact]
        public async Task QueryCategoriesByParentAndOffset()
        {
            var count = 3;
            await WithCategory(client, async parentCategory =>
            {
                await WithListOfCategories(
                    client, categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory), count,
                    async categoriesList =>
                    {
                        var offset = count - 1;
                        Assert.Equal(count, categoriesList.Count);
                        var queryCommand = new QueryCommand<Category>();
                        queryCommand.Where(c => c.Parent.Id == parentCategory.Id.valueOf());
                        queryCommand.SetOffset(offset);
                        queryCommand.SetWithTotal(true);

                        var returnedSet = await client.ExecuteAsync(queryCommand);
                        Assert.Equal(count - offset, returnedSet.Results.Count);
                        Assert.Equal(count - offset, returnedSet.Count);
                        Assert.Equal(offset, returnedSet.Offset);
                        Assert.Equal(count, returnedSet.Total);
                    });
            });
        }

        [Fact]
        public async void QueryContextLinqProvider()
        {
            await WithCategory(client, async parentCategory =>
            {
                await WithCategory(client,
                    categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory),
                    category =>
                    {
                        var query = from c in client.Query<Category>()
                            where c.Key == category.Key.valueOf()
                            orderby c.Key descending
                            select c;

                        query.Expand(c => c.Parent).Expand(c => c.Ancestors.ExpandAll());

                        var command = ((ClientQueryProvider<Category>) query.Provider).Command;
                        if (command.QueryParameters is QueryCommandParameters commandParams)
                        {
                            Assert.Equal($"key = \"{category.Key}\"", string.Join(", ", commandParams.Where));
                            Assert.Equal("key desc", string.Join(", ", commandParams.Sort));
                            Assert.Equal("parent, ancestors[*]", string.Join(", ", commandParams.Expand));
                        }

                        var categories = query.ToList();
                        Assert.Single(categories);
                        Assert.Equal(category.Key, categories.First().Key);
                        Assert.Equal(category.Parent.Id, categories.First().Parent.Obj.Id);
                        Assert.Equal(category.Parent.Id, categories.First().Ancestors.First().Id);
                    });
            });
        }

        [Fact]
        public void UseLinqProvider()
        {
            var search = from p in client.SearchProducts()
                where p.Categories.Any(reference => reference.Id == "abc")
                select p;

            search.Expand(p => p.ProductType)
                .Expand(p => p.TaxCategory)
                .Filter(p =>
                    p.Variants.Any(v => v.Attributes.Any(a => a.Name == "color" && ((TextAttribute) a).Value == "red")))
                .FilterQuery(p =>
                    p.Variants.Any(v => v.Attributes.Any(a => a.Name == "size" && ((TextAttribute) a).Value == "48")))
                .TermFacet(projection => projection.Key);

            var command = ((ClientProductProjectionSearchProvider) search.Provider).Command;
            var commandFactory = this.serviceProviderFixture.GetService<IHttpApiCommandFactory>();
            ;
            var httpApiCommand = commandFactory.Create(command);

            var request = httpApiCommand.HttpRequestMessage;
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal(
                "filter=variants.attributes.color%3A%22red%22&filter.query=categories.id%3A%22abc%22&filter.query=variants.attributes.size%3A%2248%22&facet=key&expand=productType&expand=taxCategory&withTotal=false",
                request.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task DeleteCategoryById()
        {
            var key = $"DeleteCategoryById-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    await client.ExecuteAsync(category.DeleteById());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Category>(category))
                    );
                });
        }

        [Fact]
        public async Task DeleteCategoryByKey()
        {
            var key = $"DeleteCategoryByKey-{TestingUtility.RandomString()}";
            await WithCategory(
                client, categoryDraft => DefaultCategoryDraftWithKey(categoryDraft, key),
                async category =>
                {
                    await client.ExecuteAsync(category.DeleteByKey());
                    await Assert.ThrowsAsync<NotFoundException>(
                        () => client.ExecuteAsync(new GetByIdCommand<Category>(category))
                    );
                });
        }

        [Fact]
        public async Task DeleteCategoryByIdAndExpandParent()
        {
            var key = $"DeleteCategoryById-{TestingUtility.RandomString()}";
            await WithCategory(client, async parentCategory =>
            {
                await WithCategory(
                    client, categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory),
                    async category =>
                    {
                        var deletedCategory = await client.ExecuteAsync(category.DeleteById().Expand(c => c.Parent));
                        Assert.NotNull(deletedCategory.Parent.Obj);
                        await Assert.ThrowsAsync<NotFoundException>(
                            () => client.ExecuteAsync(new GetByIdCommand<Category>(deletedCategory))
                        );
                    });
            });
        }

        #region UpdateActions

        [Fact]
        public async Task UpdateCategoryByKeyChangeName()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var name = TestingUtility.RandomString();
                var action = new ChangeNameUpdateAction
                {
                    Name = new LocalizedString {{"en", name}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(name, updatedCategory.Name["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategoryByIdSetKeyAndExpandParent()
        {
            await WithCategory(client, async parentCategory =>
            {
                await WithUpdateableCategory(client,
                    categoryDraft => DefaultCategoryDraftWithParent(categoryDraft, parentCategory)
                    , async category =>
                    {
                        var key = TestingUtility.RandomString();
                        var action = new SetKeyUpdateAction {Key = key};

                        var updatedCategory = await client
                            .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action))
                                .Expand(c => c.Parent));

                        Assert.Equal(key, updatedCategory.Key);
                        Assert.NotNull(updatedCategory.Parent.Obj);
                        return updatedCategory;
                    });
            });
        }

        [Fact]
        public async Task UpdateCategoryByKeyChangeSlug()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var slug = TestingUtility.RandomString();
                var action = new ChangeSlugUpdateAction
                {
                    Slug = new LocalizedString {{"en", slug}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateByKey(actions => actions.AddUpdate(action)));

                Assert.Equal(slug, updatedCategory.Slug["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetDescription()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var description = TestingUtility.RandomString();
                var action = new SetDescriptionUpdateAction
                {
                    Description = new LocalizedString {{"en", description}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(description, updatedCategory.Description["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategoryChangeParent()
        {
            await WithCategory(client, async parentCategory =>
            {
                await WithUpdateableCategory(client, async category =>
                {
                    Assert.Null(category.Parent);
                    var action = new ChangeParentUpdateAction
                    {
                        Parent = parentCategory.ToKeyResourceIdentifier()
                    };

                    var updatedCategory = await client
                        .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.NotNull(updatedCategory.Parent);
                    Assert.Equal(parentCategory.Id, updatedCategory.Parent.Id);
                    return updatedCategory;
                });
            });
        }

        [Fact]
        public async Task UpdateCategoryChangeOrderHint()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var orderHint = TestingUtility.RandomSortOrder();
                var action = new ChangeOrderHintUpdateAction
                {
                    OrderHint = orderHint
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(orderHint, updatedCategory.OrderHint);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetExternalId()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var externalId = TestingUtility.RandomString();
                var action = new SetExternalIdUpdateAction
                {
                    ExternalId = externalId
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(externalId, updatedCategory.ExternalId);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetMetaTitle()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var metaTitle = TestingUtility.RandomString();
                var action = new SetMetaTitleUpdateAction
                {
                    MetaTitle = new LocalizedString {{"en", metaTitle}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(metaTitle, updatedCategory.MetaTitle["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetMetaDescription()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var metaDescription = TestingUtility.RandomString();
                var action = new SetMetaDescriptionUpdateAction
                {
                    MetaDescription = new LocalizedString {{"en", metaDescription}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(metaDescription, updatedCategory.MetaDescription["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetMetaKeywords()
        {
            await WithUpdateableCategory(client, async category =>
            {
                var metaKeywords = TestingUtility.RandomString();
                var action = new SetMetaKeywordsUpdateAction
                {
                    MetaKeywords = new LocalizedString {{"en", metaKeywords}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Equal(metaKeywords, updatedCategory.MetaKeywords["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableCategory(client,
                    async category =>
                    {
                        var action = new SetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields
                        };

                        var updatedCategory = await client
                            .ExecuteAsync(category.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(type.Id, updatedCategory.Custom.Type.Id);
                        return updatedCategory;
                    });
            });
        }

        [Fact]
        public async Task UpdateCategorySetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableCategory(client,
                    categoryDraft => DefaultCategoryDraftWithCustomType(categoryDraft, type, fields),
                    async category =>
                    {
                        var action = new SetCustomFieldUpdateAction()
                        {
                            Name = "string-field", Value = newValue
                        };

                        var updatedCategory = await client
                            .ExecuteAsync(category.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Equal(newValue, updatedCategory.Custom.Fields["string-field"]);
                        return updatedCategory;
                    });
            });
        }

        [Fact]
        public async Task UpdateCategoryAddAsset()
        {
            await WithUpdateableCategory(client, async category =>
            {
                Assert.Empty(category.Assets);
                var assetDraft = TestingUtility.GetAssetDraft();
                var action = new AddAssetUpdateAction
                {
                    Asset = assetDraft
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCategory.Assets);
                Assert.Equal(assetDraft.Key, updatedCategory.Assets[0].Key);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategoryRemoveAsset()
        {
            await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset, async category =>
            {
                Assert.Single(category.Assets);
                var asset = category.Assets[0];
                var action = new RemoveAssetUpdateAction
                {
                    AssetKey = asset.Key
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Empty(updatedCategory.Assets);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetAssetKey()
        {
            await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset, async category =>
            {
                Assert.Single(category.Assets);
                var newKey = TestingUtility.RandomString();
                var action = new SetAssetKeyUpdateAction
                {
                    AssetId = category.Assets[0].Id,
                    AssetKey = newKey
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCategory.Assets);
                Assert.Equal(newKey, updatedCategory.Assets[0].Key);
                return updatedCategory;
            });
        }


        [Fact]
        public async Task UpdateCategoryChangeAssetOrder()
        {
            await WithUpdateableCategory(client,
                categoryDraft => DefaultCategoryDraftWithMultipleAssets(categoryDraft, 3),
                async category =>
                {
                    Assert.Equal(3, category.Assets.Count);
                    var assets = category.Assets;
                    assets.Reverse();
                    var reversedAssetsOrder = assets.Select(a => a.Id).ToList();
                    var action = new ChangeAssetOrderUpdateAction
                    {
                        AssetOrder = reversedAssetsOrder
                    };

                    var updatedCategory = await client
                        .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                    Assert.Equal(3, updatedCategory.Assets.Count);
                    var newAssetsOrder = updatedCategory.Assets.Select(a => a.Id).ToList();
                    Assert.Equal(reversedAssetsOrder, newAssetsOrder);
                    return updatedCategory;
                });
        }

        [Fact]
        public async Task UpdateCategoryChangeAssetName()
        {
            await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset, async category =>
            {
                Assert.Single(category.Assets);
                var newName = TestingUtility.RandomString();
                var action = new ChangeAssetNameUpdateAction
                {
                    AssetId = category.Assets[0].Id,
                    Name = new LocalizedString {{"en", newName}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCategory.Assets);
                Assert.Equal(newName, updatedCategory.Assets[0].Name["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetAssetDescription()
        {
            await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset, async category =>
            {
                Assert.Single(category.Assets);
                var newDescription = TestingUtility.RandomString();
                var action = new SetAssetDescriptionUpdateAction
                {
                    AssetId = category.Assets[0].Id,
                    Description = new LocalizedString {{"en", newDescription}}
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCategory.Assets);
                Assert.Equal(newDescription, updatedCategory.Assets[0].Description["en"]);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetAssetTags()
        {
            await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset, async category =>
            {
                Assert.Single(category.Assets);
                var tags = new List<string> {"Tag1"};
                var action = new SetAssetTagsUpdateAction
                {
                    AssetId = category.Assets[0].Id,
                    Tags = tags
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCategory.Assets);
                Assert.True(tags.SequenceEqual(updatedCategory.Assets[0].Tags));
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetAssetResources()
        {
            await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset, async category =>
            {
                Assert.Single(category.Assets);
                Assert.Single(category.Assets[0].Sources);

                var assetSources = TestingUtility.GetListOfAssetSource();
                var action = new SetAssetSourcesUpdateAction
                {
                    AssetId = category.Assets[0].Id,
                    Sources = assetSources
                };

                var updatedCategory = await client
                    .ExecuteAsync(category.UpdateById(actions => actions.AddUpdate(action)));

                Assert.Single(updatedCategory.Assets);
                Assert.Equal(2, updatedCategory.Assets[0].Sources.Count);
                Assert.Equal(assetSources[0].Key, updatedCategory.Assets[0].Sources[0].Key);
                return updatedCategory;
            });
        }

        [Fact]
        public async Task UpdateCategorySetAssetCustomType()
        {
            var fields = CreateNewFields();

            await WithType(client, async type =>
            {
                await WithUpdateableCategory(client, DefaultCategoryDraftWithAsset,
                    async category =>
                    {
                        Assert.Single(category.Assets);
                        var action = new SetAssetCustomTypeUpdateAction
                        {
                            Type = type.ToKeyResourceIdentifier(),
                            Fields = fields,
                            AssetId = category.Assets[0].Id
                        };

                        var updatedCategory = await client
                            .ExecuteAsync(category.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Single(updatedCategory.Assets);
                        Assert.Equal(type.Id, updatedCategory.Assets[0].Custom.Type.Id);
                        return updatedCategory;
                    });
            });
        }

        [Fact]
        public async Task UpdateCategorySetAssetCustomField()
        {
            var fields = CreateNewFields();
            var newValue = TestingUtility.RandomString(10);

            await WithType(client, async type =>
            {
                await WithUpdateableCategory(client,
                    categoryDraft => DefaultCategoryDraftWithAssetWithCustomType(categoryDraft, type, fields),
                    async category =>
                    {
                        Assert.Single(category.Assets);
                        var action = new SetAssetCustomFieldUpdateAction
                        {
                            Name = "string-field", Value = newValue, AssetId = category.Assets[0].Id
                        };

                        var updatedCategory = await client
                            .ExecuteAsync(category.UpdateByKey(actions => actions.AddUpdate(action)));

                        Assert.Single(updatedCategory.Assets);
                        Assert.Equal(newValue, updatedCategory.Assets[0].Custom.Fields["string-field"]);
                        return updatedCategory;
                    });
            });
        }

        #endregion
    }
}