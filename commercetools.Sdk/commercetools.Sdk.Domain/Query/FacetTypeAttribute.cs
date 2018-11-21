using System;

namespace commercetools.Sdk.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FacetResultTypeAttribute : TypeMarkerAttribute
    {
        public FacetResultTypeAttribute(string facetType)
        {
            this.Value = facetType;
        }
    }
}
