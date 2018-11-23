using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TypeFixture : ClientFixture, IDisposable
    {
        public TypeFixture()
        {

        }

        public void Dispose()
        {
            
        }

        // TODO Replace with fake generator
        public TypeDraft CreateTypeDraft()
        {
            TypeDraft typeDraft = new TypeDraft();
            typeDraft.Key = this.RandomString(3);
            typeDraft.Name = new LocalizedString();
            typeDraft.Name.Add("en", this.RandomString(6));
            typeDraft.Description = new LocalizedString();
            typeDraft.Description.Add("en", this.RandomString(10));
            typeDraft.ResourceTypeIds = new List<ResourceTypeId>() { ResourceTypeId.Category };
            typeDraft.FieldDefinitions = new List<FieldDefinition>();
            FieldDefinition stringFieldDefinition = this.CreateStringField();
            FieldDefinition numberFieldDefinition = this.CreateNumberField();
            FieldDefinition booleanFieldDefinition = this.CreateBooleanField();
            FieldDefinition enumFieldDefinition = this.CreateEnumField();
            typeDraft.FieldDefinitions.Add(stringFieldDefinition);
            typeDraft.FieldDefinitions.Add(numberFieldDefinition);
            typeDraft.FieldDefinitions.Add(booleanFieldDefinition);
            typeDraft.FieldDefinitions.Add(enumFieldDefinition);
            return typeDraft;
        }

        private FieldDefinition CreateStringField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "string-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "string description");
            fieldDefinition.InputHint = TextInputHint.SingleLine;
            fieldDefinition.Type = new StringType();
            return fieldDefinition;
        }

        private FieldDefinition CreateNumberField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "number-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "number description");
            fieldDefinition.Type = new NumberType();
            return fieldDefinition;
        }

        private FieldDefinition CreateBooleanField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "boolean-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "boolean description");
            fieldDefinition.Type = new BooleanType();
            return fieldDefinition;
        }

        private FieldDefinition CreateEnumField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "enum-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "boolean description");
            EnumType enumType = new EnumType();
            enumType.Values = new List<EnumValue>();
            enumType.Values.Add(new EnumValue() { Key = "enum-key-1", Label = "enum-label-1" });
            enumType.Values.Add(new EnumValue() { Key = "enum-key-2", Label = "enum-label-2" });
            return fieldDefinition;
        }
    }
}
