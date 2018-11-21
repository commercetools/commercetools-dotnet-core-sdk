using System.Linq;

namespace commercetools.Sdk.Domain
{
    public abstract class FieldType
    {
        public string Name
        {
            get => this.GetName();
        }

        private string GetName()
        {
            FieldTypeAttribute fieldTypeAttribute = this.GetType().GetCustomAttributes(typeof(FieldTypeAttribute), true).FirstOrDefault() as FieldTypeAttribute;
            if (fieldTypeAttribute != null)
            {
                return fieldTypeAttribute.Value;
            }
            return null;
        }
    }
}