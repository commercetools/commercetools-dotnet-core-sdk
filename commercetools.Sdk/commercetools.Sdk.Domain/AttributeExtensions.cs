namespace commercetools.Sdk.Domain
{
    using System.Linq;

    public static class AttributeExtensions
    {
        public static string GetTypeMarkerAttributeValue(this System.Type type)
        {
            TypeMarkerAttribute typeMarkerAttribute = type.GetCustomAttributes(typeof(TypeMarkerAttribute), true).FirstOrDefault() as TypeMarkerAttribute;
            if (typeMarkerAttribute != null)
            {
                return typeMarkerAttribute.Value;
            }
            return null;
        }

        public static string GetEndpointValue(this System.Type type)
        {
            EndpointAttribute endpointAttribute = type.GetCustomAttributes(typeof(EndpointAttribute), true).FirstOrDefault() as EndpointAttribute;
            if (endpointAttribute != null)
            {
                return endpointAttribute.Value;
            }
            return null;
        }
    }
}