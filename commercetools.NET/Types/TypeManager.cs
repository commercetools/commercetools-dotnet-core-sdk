using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Types
{
    /// <summary>
    /// Provides access to the functions in the Types section of the API. 
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-types.html"/>
    public class TypeManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/types";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public TypeManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a Type by its ID.
        /// </summary>
        /// <param name="typeId">Type ID</param>
        /// <returns>Type</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#get-type-by-id"/>
        public Task<Response<Type>> GetTypeByIdAsync(string typeId)
        {
            if (string.IsNullOrWhiteSpace(typeId))
            {
                throw new ArgumentException("typeId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", typeId);
            return _client.GetAsync<Type>(endpoint);
        }

        /// <summary>
        /// Queries for Types.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>TypeQueryResult</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#get-type-by-id"/>
        public Task<Response<TypeQueryResult>> QueryTypesAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
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

            return _client.GetAsync<TypeQueryResult>(ENDPOINT_PREFIX, values);
        }

        /// <summary>
        /// Creates a new type.
        /// </summary>
        /// <param name="typeDraft">Type Draft</param>
        /// <returns>Type</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#create-type"/>
        public Task<Response<Type>> CreateTypeAsync(TypeDraft typeDraft)
        {
            if (string.IsNullOrWhiteSpace(typeDraft.Key))
            {
                throw new ArgumentException("Key is required");
            }

            if (typeDraft.Name.IsEmpty())
            {
                throw new ArgumentException("At least one value for Name is required");
            }

            if (typeDraft.ResourceTypeIds.Count < 1)
            {
                throw new ArgumentException("At least one value for ResourceTypeIds is required");
            }

            string payload = JsonConvert.SerializeObject(typeDraft, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return _client.PostAsync<Type>(ENDPOINT_PREFIX, payload);
        }

        /// <summary>
        /// Updates a type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="action">The update action to be performed on the type.</param>
        /// <returns>Type</returns>
        public Task<Response<Type>> UpdateTypeAsync(Type type, UpdateAction action)
        {
            return UpdateTypeByIdAsync(type.Id, type.Version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="actions">The update actions to be performed on the type.</param>
        /// <returns>Type</returns>
        public Task<Response<Type>> UpdateTypeAsync(Type type, List<UpdateAction> actions)
        {
            return UpdateTypeByIdAsync(type.Id, type.Version, actions);
        }

        /// <summary>
        /// Updates a type.
        /// </summary>
        /// <param name="typeId">Type ID</param>
        /// <param name="version">The expected version of the type on which the changes should be applied.</param>
        /// <param name="action">The update action to be performed on the type.</param>
        /// <returns>Type</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#update-type-by-id"/>
        public Task<Response<Type>> UpdateTypeByIdAsync(string typeId, int version, UpdateAction action)
        {
            return UpdateTypeByIdAsync(typeId, version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a type.
        /// </summary>
        /// <param name="typeId">Type ID</param>
        /// <param name="version">The expected version of the type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the type.</param>
        /// <returns>Type</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#update-type-by-id"/>
        public Task<Response<Type>> UpdateTypeByIdAsync(string typeId, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(typeId))
            {
                throw new ArgumentException("typeId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", typeId);
            return UpdateTypeAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Updates a type.
        /// </summary>
        /// <param name="key">Type key</param>
        /// <param name="version">The expected version of the type on which the changes should be applied.</param>
        /// <param name="action">The update action to be performed on the type.</param>
        /// <returns>Type</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#update-type-by-id"/>
        public Task<Response<Type>> UpdateTypeByKeyAsync(string key, int version, UpdateAction action)
        {
            return UpdateTypeByKeyAsync(key, version, new List<UpdateAction> { action });
        }

        /// <summary>
        /// Updates a type.
        /// </summary>
        /// <param name="key">Type key</param>
        /// <param name="version">The expected version of the type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to be performed on the type.</param>
        /// <returns>Type</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#update-type-by-id"/>
        public Task<Response<Type>> UpdateTypeByKeyAsync(string key, int version, List<UpdateAction> actions)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return UpdateTypeAsync(endpoint, version, actions);
        }

        /// <summary>
        /// Private worker method for UpdateProductByIdAsync and UpdateProductByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">The expected version of the type on which the changes should be applied.</param>
        /// <param name="actions">The list of update actions to apply to the type.</param>
        /// <returns>Type</returns>
        private Task<Response<Type>> UpdateTypeAsync(string endpoint, int version, List<UpdateAction> actions)
        {
            if (version < 1)
            {
                throw new ArgumentException("version is required");
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

            return _client.PostAsync<Type>(endpoint, data.ToString());
        }

        /// <summary>
        /// Deletes a type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Response of type JObject</returns>
        public Task<Response<JObject>> DeleteTypeAsync(Type type)
        {
            return DeleteTypeByIdAsync(type.Id, type.Version);
        }

        /// <summary>
        /// Deletes a type.
        /// </summary>
        /// <param name="typeId">Type ID</param>
        /// <param name="version">Type version</param>
        /// <returns>JObject</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#delete-type-by-id"/>
        public Task<Response<JObject>> DeleteTypeByIdAsync(string typeId, int version)
        {
            if (string.IsNullOrWhiteSpace(typeId))
            {
                throw new ArgumentException("typeId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", typeId);
            return DeleteTypeAsync(endpoint, version);
        }

        /// <summary>
        /// Deletes a type.
        /// </summary>
        /// <param name="key">Type key</param>
        /// <param name="version">Type version</param>
        /// <returns>JObject</returns>
        /// <see href="https://dev.commercetools.com/http-api-projects-types.html#delete-type-by-key"/>
        public Task<Response<JObject>> DeleteTypeByKeyAsync(string key, int version)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/key=", key);
            return DeleteTypeAsync(endpoint, version);
        }

        /// <summary>
        /// Private worker method for DeleteTypeByIdAsync and DeleteTypeByKeyAsync.
        /// </summary>
        /// <param name="endpoint">Request endpoint</param>
        /// <param name="version">Type version</param>
        /// <returns>JObject</returns>
        private Task<Response<JObject>> DeleteTypeAsync(string endpoint, int version)
        {
            if (version < 1)
            {
                throw new ArgumentException("version is required");
            }

            var values = new NameValueCollection
            {
                { "version", version.ToString() }
            };

            return _client.DeleteAsync<JObject>(endpoint, values);
        }

        #endregion
    }
}
