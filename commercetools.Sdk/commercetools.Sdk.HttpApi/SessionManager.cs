using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace commercetools.Sdk.HttpApi
{
    public class SessionManager : ISessionManager
    {
        private IHttpContextAccessor httpContextAccessor;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Username
        {
            get
            {
                return this.httpContextAccessor.HttpContext.Session.GetString("Username");
            }
            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("Username", value);
            }
        }

        public string Password
        {
            get
            {
                return this.httpContextAccessor.HttpContext.Session.GetString("Password");
            }
            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("Password", value);
            }
        }

        public string AnonymousId
        {
            get
            {
                return this.httpContextAccessor.HttpContext.Session.GetString("AnonymousId");
            }
            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("AnonymousId", value);
            }
        }

        public string ClientName
        {
            get
            {
                return this.httpContextAccessor.HttpContext.Session.GetString("ClientName");
            }
            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("ClientName", value);
            }
        }

        public TokenFlow TokenFlow
        {
            get
            {
                TokenFlow tokenFlow;
                Enum.TryParse(this.httpContextAccessor.HttpContext.Session.GetString("TokenFlow"), out tokenFlow);
                return tokenFlow;
            }
            set
            {
                this.httpContextAccessor.HttpContext.Session.SetString("TokenFlow", value.ToString());
            }
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