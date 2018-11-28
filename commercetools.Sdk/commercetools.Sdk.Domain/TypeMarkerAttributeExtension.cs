namespace commercetools.Sdk.Domain
{
    using System.Linq;

    public static class TypeMarkerAttributeExtension
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
    }
}