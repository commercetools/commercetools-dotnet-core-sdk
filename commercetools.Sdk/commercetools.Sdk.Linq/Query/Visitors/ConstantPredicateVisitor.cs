namespace commercetools.Sdk.Linq.Query.Visitors
{
    // name
    // "Peter"
    public class ConstantPredicateVisitor : IPredicateVisitor
    {
        public ConstantPredicateVisitor(string constant, bool isCaseSensitive = false)
        {
            this.Constant = constant;
            this.IsCaseSensitive = isCaseSensitive;
        }

        /// <summary>
        /// Set true when you want to return constant as it's it, else it will return as camel case
        /// Attribute names must be case sensitive
        /// </summary>
        public bool IsCaseSensitive { get; }

        public string Constant { get; }

        public string Render()
        {
            return IsCaseSensitive ? $"{this.Constant}" : $"{this.Constant.ToCamelCase()}";
        }
    }
}
