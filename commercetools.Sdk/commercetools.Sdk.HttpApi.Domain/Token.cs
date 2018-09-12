namespace commercetools.Sdk.HttpApi.Domain
{
    using System;

    public class Token
    {
        public Token()
        {
            this.CreationDate = DateTime.Now;
        }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public long ExpiresIn { get; set; }

        public string Scope { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreationDate { get; private set; }

        public bool Expired
        {
            get
            {
                if (CreationDate.AddSeconds(ExpiresIn) < DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }
    }
}