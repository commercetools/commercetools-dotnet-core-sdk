namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Serialization;
    using Microsoft.AspNetCore.Http;

    public class SessionUserCredentialsStoreManager : SessionTokenStoreManager, IUserCredentialsStoreManager
    {
        public SessionUserCredentialsStoreManager(IHttpContextAccessor httpContextAccessor, ISerializerService serializerService) : base(httpContextAccessor, serializerService)
        {
        }

        public string Username
        {
            get
            {
                return this.HttpContextAccessor.HttpContext.Session.GetString("Username");
            }
            set
            {
                this.HttpContextAccessor.HttpContext.Session.SetString("Username", value);
            }
        }

        public string Password
        {
            get
            {
                return this.HttpContextAccessor.HttpContext.Session.GetString("Password");
            }
            set
            {
                this.HttpContextAccessor.HttpContext.Session.SetString("Password", value);
            }
        }
    }
}