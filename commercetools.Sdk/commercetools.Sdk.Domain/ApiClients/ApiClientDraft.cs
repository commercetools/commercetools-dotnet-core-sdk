namespace commercetools.Sdk.Domain.ApiClients
{
    public class ApiClientDraft: IDraft<ApiClient>
    {
        public string Name { get; set; }
        public string Scope { get; set; }
        public long? DeleteDaysAfterCreation { get; set; }
        public long? AccessTokenValiditySeconds { get; set; }
        public long? RefreshTokenValiditySeconds { get; set; }
    }
}