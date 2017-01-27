namespace commercetools.Types
{
    /// <summary>
    /// MoneyType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-types.html#moneytype"/>
    public class MoneyType : FieldType
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public MoneyType()
            : base()
        {
            this.Name = "Money";
        }

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public MoneyType(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
