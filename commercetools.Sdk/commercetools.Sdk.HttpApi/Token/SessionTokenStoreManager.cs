using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.Http;

namespace commercetools.Sdk.HttpApi
{
    /// <summary>
    /// This class blah blah
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.ITokenStoreManager" />
    public class SessionTokenStoreManager : ITokenStoreManager
    {
        private IHttpContextAccessor httpContextAccessor;
        private ISerializerService serializerService;

        public SessionTokenStoreManager(IHttpContextAccessor httpContextAccessor, ISerializerService serializerService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serializerService = serializerService;
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public Token Token
        {
            get
            {
                if (string.IsNullOrEmpty(this.httpContextAccessor.HttpContext.Session.GetString("Token")))
                {
                    return null;
                }

                return this.serializerService.Deserialize<Token>(this.httpContextAccessor.HttpContext.Session.GetString("Token"));
            }

            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("Token", this.serializerService.Serialize(value));
            }
        }

        protected IHttpContextAccessor HttpContextAccessor { get; }
    }
}