using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Categories
{
    /// <summary>
    /// Provides access to the functions in the Categories section of the API.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html"/>
    public class CategoryManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/categories";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public CategoryManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a category by its ID.
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#get-category-by-id"/>
        public Task<Response<Category>> GetCategoryByIdAsync(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentException("categoryId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", categoryId);
            return _client.GetAsync<Category>(endpoint);
        }

        /// <summary>
        /// Queries categories.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>CategoryQueryResult</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#query-categories"/>
        public Task<Response<CategoryQueryResult>> QueryCategoriesAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
        {
            NameValueCollection values = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(where))
            {
                values.Add("where", where);
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                values.Add("sort", sort);
            }

            if (limit > 0)
            {
                values.Add("limit", limit.ToString());
            }

            if (offset >= 0)
            {
                values.Add("offset", offset.ToString());
            }

            return _client.GetAsync <CategoryQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDraft">CategoryDraft object</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#create-a-category"/>
        public Task<Response<Category>> CreateCategoryAsync(CategoryDraft categoryDraft)
        {
            if (categoryDraft == null)
            {
                throw new ArgumentException("categoryDraft cannot be null");
            }

            if (categoryDraft.Name.IsEmpty())
            {
                throw new ArgumentException("Category name is required");
            }

            if (categoryDraft.Slug.IsEmpty())
            {
                throw new ArgumentException("Category slug is required");
            }

            string payload = JsonConvert.SerializeObject(categoryDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<Category>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="action">The update action to be performed on the category.</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#update-category"/>
        public Task<Response<Category>> UpdateCategoryAsync(Category category, UpdateAction action)
        {
            return UpdateCategoryAsync(category.Id, category.Version, new List<UpdateAction> { action } );
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="actions">The list of update actions to be performed on the category.</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#update-category"/>
        public Task<Response<Category>> UpdateCategoryAsync(Category category, List<UpdateAction> actions)
        {
            return UpdateCategoryAsync(category.Id, category.Version, actions);
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="categoryId">ID of the category</param>
        /// <param name="version">The expected version of the category on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on
        /// the category.</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#update-category"/>
        public Task<Response<Category>> UpdateCategoryAsync(string categoryId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentException("Category ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            if (actions == null || actions.Count < 1)
            {
                throw new ArgumentException("One or more update actions is required");
            }

            JObject data = JObject.FromObject(new
            {
                version = version,
                actions = JArray.FromObject(actions, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore })
            });

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", categoryId);
            return _client.PostAsync<Category>(endpoint, data.ToString());
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#delete-category"/>
        public Task<Response<Category>> DeleteCategoryAsync(Category category)
        {
            return DeleteCategoryAsync(category.Id, category.Version);
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="categoryId">Caregory ID</param>
        /// <param name="version">Caregory version</param>
        /// <returns>Category</returns>
        /// <see href="http://dev.commercetools.com/http-api-projects-categories.html#delete-category"/>
        public Task<Response<Category>> DeleteCategoryAsync(string categoryId, int version)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentException("Category ID is required");
            }

            if (version < 1)
            {
                throw new ArgumentException("Version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", categoryId);
            return _client.DeleteAsync<Category>(endpoint, values);
        }

        #endregion
    }
}
