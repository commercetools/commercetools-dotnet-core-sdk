using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace commercetools.Sdk.HttpApi
{
    public class SessionTokenStoreManager : ITokenStoreManager
    {
        private IHttpContextAccessor httpContextAccessor;

        protected IHttpContextAccessor HttpContextAccessor { get; }

        public SessionTokenStoreManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Token Token
        {
            get
            {
                if (string.IsNullOrEmpty(this.httpContextAccessor.HttpContext.Session.GetString("Token")))
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<Token>(this.httpContextAccessor.HttpContext.Session.GetString("Token"));
            }
            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("Token", JsonConvert.SerializeObject(value));
            }
        }
    }
}