using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace commercetools.Sdk.HttpApi
{
    public class SessionUserCredentialsStoreManager : SessionTokenStoreManager, IUserCredentialsStoreManager
    {   
        public SessionUserCredentialsStoreManager(IHttpContextAccessor httpContextAccessor) : base (httpContextAccessor)
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