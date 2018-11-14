using System;

namespace commercetools.Sdk.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FacetTypeAttribute : System.Attribute
    {
        private string facetType;

        public FacetTypeAttribute(string facetType)
        {
            this.facetType = facetType;
        }

        public string FacetType
        {
            get
            {
                return this.facetType;
            }
        }
    }
}
