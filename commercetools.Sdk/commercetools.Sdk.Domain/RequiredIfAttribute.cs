using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class RequiredIfAttribute : ValidationAttribute  
    {  
        public string PropertyName { get; set; }  
        public object Value { get; set; }  
  
        public RequiredIfAttribute(string propertyName, object value, string errorMessage = "")  
        {  
            PropertyName = propertyName;  
            ErrorMessage = errorMessage;  
            Value = value;  
        }  
  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)  
        {  
            var instance = validationContext.ObjectInstance;  
            var type = instance.GetType();  
            var proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);  
            if (proprtyvalue.ToString() == Value.ToString() && value == null)  
            {  
                return new ValidationResult(ErrorMessage);  
            }  
            return ValidationResult.Success;  
        }  
    }  
}