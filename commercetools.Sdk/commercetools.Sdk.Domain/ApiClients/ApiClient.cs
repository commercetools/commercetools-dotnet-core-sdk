﻿using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.ApiClients
{
    [Endpoint("api-clients")]
    public class ApiClient: Resource<ApiClient>
    {
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Secret { get; set; }
        public long? AccessTokenValiditySeconds { get; set; }
        public long? RefreshTokenValiditySeconds { get; set; }
    }
}
