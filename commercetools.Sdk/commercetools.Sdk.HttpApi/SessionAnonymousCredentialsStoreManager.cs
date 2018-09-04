using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace commercetools.Sdk.HttpApi
{
    public class SessionAnonymousCredentialsStoreManager : SessionTokenStoreManager, IAnonymousCredentialsStoreManager
    {
        public SessionAnonymousCredentialsStoreManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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