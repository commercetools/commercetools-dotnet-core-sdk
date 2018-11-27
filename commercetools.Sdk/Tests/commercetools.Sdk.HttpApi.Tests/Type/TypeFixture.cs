using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TypeFixture : ClientFixture, IDisposable
    {
        public List<Type> TypesToDelete;

        public TypeFixture()
        {
            this.TypesToDelete = new List<Type>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.TypesToDelete.Reverse();
            foreach (Type type in this.TypesToDelete)
            {
                Type deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<Type>(new Guid(type.Id), type.Version)).Result;
            }
        }

        public Type CreateType()
        {
            TypeDraft typeDraft = this.CreateTypeDraft();
            IClient commerceToolsClient = this.GetService<IClient>();
            Type createdType = commerceToolsClient.ExecuteAsync(new CreateCommand<Type>(typeDraft)).Result;
            return createdType;
        }

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
            FieldDefinition localizedStringFieldDefinition = this.CreateLocalizedStringField();
            FieldDefinition numberFieldDefinition = this.CreateNumberField();
            FieldDefinition booleanFieldDefinition = this.CreateBooleanField();
            FieldDefinition enumFieldDefinition = this.CreateEnumField();
            FieldDefinition localizedEnumFieldDefinition = this.CreateLocalizedEnumField();
            FieldDefinition moneyFieldDefinition = this.CreateMoneyField();
            FieldDefinition dateFieldDefinition = this.CreateDateField();
            FieldDefinition timeFieldDefinition = this.CreateTimeField();
            FieldDefinition dateTimeFieldDefinition = this.CreateDateTimeField();
            FieldDefinition referenceFieldDefinition = this.CreateReferenceField();
            FieldDefinition setFieldDefinition = this.CreateSetField();
            typeDraft.FieldDefinitions.Add(stringFieldDefinition);
            typeDraft.FieldDefinitions.Add(localizedStringFieldDefinition);
            typeDraft.FieldDefinitions.Add(numberFieldDefinition);
            typeDraft.FieldDefinitions.Add(booleanFieldDefinition);
            typeDraft.FieldDefinitions.Add(enumFieldDefinition);
            typeDraft.FieldDefinitions.Add(localizedEnumFieldDefinition);
            typeDraft.FieldDefinitions.Add(moneyFieldDefinition);
            typeDraft.FieldDefinitions.Add(dateFieldDefinition);
            typeDraft.FieldDefinitions.Add(timeFieldDefinition);
            typeDraft.FieldDefinitions.Add(dateTimeFieldDefinition);
            typeDraft.FieldDefinitions.Add(referenceFieldDefinition);
            typeDraft.FieldDefinitions.Add(setFieldDefinition);
            return typeDraft;
        }

        public FieldDefinition CreateNewStringField()
        {
            return this.CreateStringField("new-string-field");
        }

        private FieldDefinition CreateStringField()
        {
            return this.CreateStringField("string-field");
        }

        private FieldDefinition CreateStringField(string name)
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = name;
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "string description");
            fieldDefinition.InputHint = TextInputHint.SingleLine;
            fieldDefinition.Type = new StringType();
            return fieldDefinition;
        }

        private FieldDefinition CreateLocalizedStringField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "localized-string-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "localized string description");
            fieldDefinition.InputHint = TextInputHint.MultiLine;
            fieldDefinition.Type = new LocalizedStringType();
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
            fieldDefinition.Label.Add("en", "enum description");
            EnumType enumType = new EnumType();
            enumType.Values = new List<EnumValue>();
            enumType.Values.Add(new EnumValue() { Key = "enum-key-1", Label = "enum-label-1" });
            enumType.Values.Add(new EnumValue() { Key = "enum-key-2", Label = "enum-label-2" });
            fieldDefinition.Type = enumType;
            return fieldDefinition;
        }

        private FieldDefinition CreateLocalizedEnumField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "localized-enum-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "localized enum description");
            LocalizedEnumType localizedEnumType = new LocalizedEnumType();
            localizedEnumType.Values = new List<LocalizedEnumValue>();
            localizedEnumType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-1", Label = new LocalizedString() { { "en", "enum-label-1" } } });
            localizedEnumType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-2", Label = new LocalizedString() { { "en", "enum-label-2" } } });
            fieldDefinition.Type = localizedEnumType;
            return fieldDefinition;
        }

        private FieldDefinition CreateMoneyField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "money-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "money description");
            MoneyType moneyType = new MoneyType();
            fieldDefinition.Type = moneyType;
            return fieldDefinition;
        }

        private FieldDefinition CreateDateTimeField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "date-time-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "date time description");
            DateTimeType dateTimeType = new DateTimeType();
            fieldDefinition.Type = dateTimeType;
            return fieldDefinition;
        }

        private FieldDefinition CreateDateField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "date-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "date description");
            DateType dateType = new DateType();
            fieldDefinition.Type = dateType;
            return fieldDefinition;
        }

        private FieldDefinition CreateTimeField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "time-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "time description");
            TimeType timeType = new TimeType();
            fieldDefinition.Type = timeType;
            return fieldDefinition;
        }

        private FieldDefinition CreateReferenceField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "reference-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "reference description");
            ReferenceFieldType fieldType = new ReferenceFieldType();
            fieldType.ReferenceTypeId = ReferenceFieldTypeId.Category;
            fieldDefinition.Type = fieldType;
            return fieldDefinition;
        }

        private FieldDefinition CreateSetField()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "set-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "set description");
            SetType fieldType = new SetType();
            fieldType.ElementType = new StringType();
            fieldDefinition.Type = fieldType;
            return fieldDefinition;
        }
    }
}