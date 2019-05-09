﻿using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using Xunit.Abstractions;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class TypeFixture : ClientFixture, IDisposable
    {
        public List<Type> TypesToDelete;

        public TypeFixture(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
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
            typeDraft.Key = TestingUtility.RandomString(10);
            typeDraft.Name = new LocalizedString();
            typeDraft.Name.Add("en", TestingUtility.RandomString(10));
            typeDraft.Description = new LocalizedString();
            typeDraft.Description.Add("en", TestingUtility.RandomString(10));
            typeDraft.ResourceTypeIds = new List<ResourceTypeId>() { ResourceTypeId.Category, ResourceTypeId.CustomerGroup, ResourceTypeId.InventoryEntry, ResourceTypeId.Order, ResourceTypeId.LineItem, ResourceTypeId.CustomLineItem, ResourceTypeId.ProductPrice, ResourceTypeId.Asset };
            typeDraft.FieldDefinitions = new List<FieldDefinition>();
            typeDraft.FieldDefinitions.Add(this.CreateStringFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateLocalizedStringFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateNumberFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateBooleanFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateEnumFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateLocalizedEnumFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateMoneyFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateDateFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateTimeFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateDateTimeFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateReferenceFieldDefinition());
            typeDraft.FieldDefinitions.Add(this.CreateSetFieldDefinition());
            return typeDraft;
        }

        public FieldDefinition CreateNewStringFieldDefinition()
        {
            return this.CreateStringFieldDefinition("new-string-field");
        }

        private FieldDefinition CreateStringFieldDefinition()
        {
            return this.CreateStringFieldDefinition("string-field");
        }

        private FieldDefinition CreateStringFieldDefinition(string name)
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = name;
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "string description");
            fieldDefinition.InputHint = TextInputHint.SingleLine;
            fieldDefinition.Type = new StringFieldType();
            return fieldDefinition;
        }

        private FieldDefinition CreateLocalizedStringFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "localized-string-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "localized string description");
            fieldDefinition.InputHint = TextInputHint.MultiLine;
            fieldDefinition.Type = new LocalizedStringFieldType();
            return fieldDefinition;
        }

        private FieldDefinition CreateNumberFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "number-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "number description");
            fieldDefinition.Type = new NumberFieldType();
            return fieldDefinition;
        }

        private FieldDefinition CreateBooleanFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "boolean-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "boolean description");
            fieldDefinition.Type = new BooleanFieldType();
            return fieldDefinition;
        }

        private FieldDefinition CreateEnumFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "enum-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "enum description");
            EnumFieldType enumType = new EnumFieldType();
            enumType.Values = new List<EnumValue>();
            enumType.Values.Add(new EnumValue() { Key = "enum-key-1", Label = "enum-label-1" });
            enumType.Values.Add(new EnumValue() { Key = "enum-key-2", Label = "enum-label-2" });
            fieldDefinition.Type = enumType;
            return fieldDefinition;
        }

        private FieldDefinition CreateLocalizedEnumFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "localized-enum-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "localized enum description");
            LocalizedEnumFieldType localizedEnumType = new LocalizedEnumFieldType();
            localizedEnumType.Values = new List<LocalizedEnumValue>();
            localizedEnumType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-1", Label = new LocalizedString() { { "en", "enum-label-1" } } });
            localizedEnumType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-2", Label = new LocalizedString() { { "en", "enum-label-2" } } });
            fieldDefinition.Type = localizedEnumType;
            return fieldDefinition;
        }

        private FieldDefinition CreateMoneyFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "money-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "money description");
            MoneyFieldType moneyType = new MoneyFieldType();
            fieldDefinition.Type = moneyType;
            return fieldDefinition;
        }

        private FieldDefinition CreateDateTimeFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "date-time-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "date time description");
            DateTimeFieldType dateTimeType = new DateTimeFieldType();
            fieldDefinition.Type = dateTimeType;
            return fieldDefinition;
        }

        private FieldDefinition CreateDateFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "date-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "date description");
            DateFieldType dateType = new DateFieldType();
            fieldDefinition.Type = dateType;
            return fieldDefinition;
        }

        private FieldDefinition CreateTimeFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "time-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "time description");
            TimeFieldType timeType = new TimeFieldType();
            fieldDefinition.Type = timeType;
            return fieldDefinition;
        }

        private FieldDefinition CreateReferenceFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "reference-field";
            fieldDefinition.Required = false;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "reference description");
            ReferenceFieldType fieldType = new ReferenceFieldType();
            fieldType.ReferenceTypeId = ReferenceFieldTypeId.Category;
            fieldDefinition.Type = fieldType;
            return fieldDefinition;
        }

        private FieldDefinition CreateSetFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "set-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "set description");
            SetFieldType fieldType = new SetFieldType();
            fieldType.ElementType = new StringFieldType();
            fieldDefinition.Type = fieldType;
            return fieldDefinition;
        }

        public Fields CreateNewFields()
        {
            Fields fields = new Fields();
            fields.Add("string-field", "test");
            fields.Add("localized-string-field", new LocalizedString() { { "en", "localized-string-field-value" } });
            fields.Add("enum-field", "enum-key-1");
            fields.Add("localized-enum-field", "enum-key-1");
            fields.Add("number-field", 3);
            fields.Add("boolean-field", true);
            fields.Add("date-field", new DateTime(2018, 11, 28));
            fields.Add("date-time-field", new DateTime(2018, 11, 28, 11, 01, 00));
            fields.Add("time-field", new TimeSpan(11, 01, 00));
            fields.Add("money-field", new Money() { CentAmount = 1800, CurrencyCode = "EUR" });
            fields.Add("set-field", new FieldSet<string>() { "test1", "test2" });
            return fields;
        }
    }
}
