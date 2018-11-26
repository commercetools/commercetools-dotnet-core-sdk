using System;

namespace commercetools.Sdk.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FieldTypeAttribute : TypeMarkerAttribute
    {
        public FieldTypeAttribute(string fieldType)
        {
            this.Value = fieldType;
        }
    }
}
