namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Serialization;
    using Microsoft.AspNetCore.Http;

    public class SessionAnonymousCredentialsStoreManager : SessionTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public SessionAnonymousCredentialsStoreManager(IHttpContextAccessor httpContextAccessor, ISerializerService serializerService) : base(httpContextAccessor, serializerService)
        {
        }

        public string AnonymousId
        {
            get
            {
                return this.HttpContextAccessor.HttpContext.Session.GetString("AnonymousId");
            }
            set
            {
                this.HttpContextAccessor.HttpContext.Session.SetString("AnonymousId", value);
            }
        }
    }
}