using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.AspNetCore.Http;

namespace commercetools.Sdk.PasswordExample
{
    public class UserCredentialsStoreManager : InMemoryTokenStoreManager, IUserCredentialsStoreManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserCredentialsStoreManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Username => this.httpContextAccessor.HttpContext.Request.Form["username"];

        public string Password => this.httpContextAccessor.HttpContext.Request.Form["password"];
    }
}
