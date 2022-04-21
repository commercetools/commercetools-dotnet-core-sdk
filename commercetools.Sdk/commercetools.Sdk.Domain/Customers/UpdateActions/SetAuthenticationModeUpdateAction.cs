namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetAuthenticationModeUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setAuthenticationMode";
        public AuthenticationMode AuthMode { get; set; }
        [RequiredIf(nameof(AuthMode), AuthenticationMode.Password,"The 'password' is required when authMode is 'Password'")]
        public string Password { get; set; }
    }
}
