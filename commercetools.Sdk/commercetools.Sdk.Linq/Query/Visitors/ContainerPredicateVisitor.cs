namespace commercetools.Sdk.Linq.Query.Visitors
{
    // variants(not(attributes(name="attribute-name")))
    public class ContainerPredicateVisitor : IPredicateVisitor
    {
        public ContainerPredicateVisitor(IPredicateVisitor inner, IPredicateVisitor parent,
            bool renderInnerWithOutParentheses = false)
        {
            this.Inner = inner;
            this.Parent = parent;
            this.RenderInnerWithOutParentheses = renderInnerWithOutParentheses;
        }

        public IPredicateVisitor Parent { get; }

        public IPredicateVisitor Inner { get; }

        public bool RenderInnerWithOutParentheses { get; }

        /// <summary>
        /// Remove Parentheses from Inner if RenderInnerWithOutParentheses is true
        /// </summary>
        /// <returns>expression result as string</returns>
        public string Render()
        {
            string result;

            if (this.RenderInnerWithOutParentheses)
            {
                result = this.Parent != null
                    ? $"{this.Parent.Render()}{this.Inner?.Render()}"
                    : this.Inner?.Render();
            }
            else
            {
                result = this.Parent != null
                    ? $"{this.Parent.Render()}({this.Inner?.Render()})"
                    : this.Inner?.Render();
            }

            return result;
        }
    }
}