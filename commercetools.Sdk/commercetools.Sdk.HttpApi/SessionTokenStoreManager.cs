namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Serialization;
    using Microsoft.AspNetCore.Http;

    public class SessionTokenStoreManager : ITokenStoreManager
    {
        private IHttpContextAccessor httpContextAccessor;
        private ISerializerService serializerService;

        protected IHttpContextAccessor HttpContextAccessor { get; }

        public SessionTokenStoreManager(IHttpContextAccessor httpContextAccessor, ISerializerService serializerService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serializerService = serializerService;
        }

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
    }
}