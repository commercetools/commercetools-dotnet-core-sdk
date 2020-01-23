using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Types;
using commercetools.Sdk.Domain.Types.FieldTypes;
using Type = commercetools.Sdk.Domain.Types.Type;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.Types
{
    public static class TypesFixture
    {
        #region DraftBuilds
        public static TypeDraft DefaultTypeDraft(TypeDraft typeDraft)
        {
            typeDraft.Key = TestingUtility.RandomString(10);
            typeDraft.Name = new LocalizedString();
            typeDraft.Name.Add("en", TestingUtility.RandomString(10));
            typeDraft.Description = new LocalizedString();
            typeDraft.Description.Add("en", TestingUtility.RandomString(10));
            typeDraft.ResourceTypeIds = new List<ResourceTypeId>() { ResourceTypeId.Category, ResourceTypeId.CustomerGroup, ResourceTypeId.InventoryEntry, ResourceTypeId.Order, ResourceTypeId.LineItem, ResourceTypeId.CustomLineItem, ResourceTypeId.ProductPrice, ResourceTypeId.Asset, ResourceTypeId.Payment, ResourceTypeId.PaymentInterfaceInteraction, ResourceTypeId.Review, ResourceTypeId.CartDiscount, ResourceTypeId.DiscountCode, ResourceTypeId.Channel, ResourceTypeId.ShoppingList, ResourceTypeId.ShoppingListTextLineItem, ResourceTypeId.Customer, ResourceTypeId.OrderEdit };
            typeDraft.FieldDefinitions = new List<FieldDefinition>();
            typeDraft.FieldDefinitions.Add(CreateStringFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateLocalizedStringFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateNumberFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateBooleanFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateEnumFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateLocalizedEnumFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateMoneyFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateDateFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateTimeFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateDateTimeFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateReferenceFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateCustomObjectReferenceFieldDefinition());
            typeDraft.FieldDefinitions.Add(CreateSetFieldDefinition());
            return typeDraft;
        }
        public static TypeDraft DefaultTypeDraftWithKey(TypeDraft draft, string key)
        {
            var customDraft = DefaultTypeDraft(draft);
            customDraft.Key = key;
            return customDraft;
        }

        public static TypeDraft DefaultTypeDraftWithoutFields(TypeDraft draft)
        {
            var customDraft = DefaultTypeDraft(draft);
            customDraft.FieldDefinitions.Clear();
            return customDraft;
        }
        public static TypeDraft DefaultTypeDraftWithOneStringField(TypeDraft draft)
        {
            var customDraft = DefaultTypeDraft(draft);
            customDraft.FieldDefinitions.RemoveAll(field => field.Type.GetType() != typeof(StringFieldType));
            return customDraft;
        }
        #endregion


        #region WithType

        public static async Task WithType( IClient client, Action<Type> func)
        {
            await With(client, new TypeDraft(), DefaultTypeDraft, func);
        }
        public static async Task WithType( IClient client, Func<TypeDraft, TypeDraft> draftAction, Action<Type> func)
        {
            await With(client, new TypeDraft(), draftAction, func);
        }

        public static async Task WithType( IClient client, Func<Type, Task> func)
        {
            await WithAsync(client, new TypeDraft(), DefaultTypeDraft, func);
        }
        public static async Task WithType( IClient client, Func<TypeDraft, TypeDraft> draftAction, Func<Type, Task> func)
        {
            await WithAsync(client, new TypeDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableType

        public static async Task WithUpdateableType(IClient client, Func<Type, Type> func)
        {
            await WithUpdateable(client, new TypeDraft(), DefaultTypeDraft, func);
        }

        public static async Task WithUpdateableType(IClient client, Func<TypeDraft, TypeDraft> draftAction, Func<Type, Type> func)
        {
            await WithUpdateable(client, new TypeDraft(), draftAction, func);
        }

        public static async Task WithUpdateableType(IClient client, Func<Type, Task<Type>> func)
        {
            await WithUpdateableAsync(client, new TypeDraft(), DefaultTypeDraft, func);
        }
        public static async Task WithUpdateableType(IClient client, Func<TypeDraft, TypeDraft> draftAction, Func<Type, Task<Type>> func)
        {
            await WithUpdateableAsync(client, new TypeDraft(), draftAction, func);
        }

        #endregion


        #region HelperFunctions

        public static FieldDefinition CreateStringFieldDefinition()
        {
            return CreateStringFieldDefinition("string-field");
        }

        private static FieldDefinition CreateStringFieldDefinition(string name)
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

        private static FieldDefinition CreateLocalizedStringFieldDefinition()
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

        private static FieldDefinition CreateNumberFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "number-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "number description");
            fieldDefinition.Type = new NumberFieldType();
            return fieldDefinition;
        }

        private static FieldDefinition CreateBooleanFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "boolean-field";
            fieldDefinition.Required = true;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "boolean description");
            fieldDefinition.Type = new BooleanFieldType();
            return fieldDefinition;
        }

        private static FieldDefinition CreateEnumFieldDefinition()
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

        private static FieldDefinition CreateLocalizedEnumFieldDefinition()
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

        private static FieldDefinition CreateMoneyFieldDefinition()
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

        private static FieldDefinition CreateDateTimeFieldDefinition()
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

        private static FieldDefinition CreateDateFieldDefinition()
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

        private static FieldDefinition CreateTimeFieldDefinition()
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

        private static FieldDefinition CreateReferenceFieldDefinition()
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

        private static FieldDefinition CreateCustomObjectReferenceFieldDefinition()
        {
            FieldDefinition fieldDefinition = new FieldDefinition();
            fieldDefinition.Name = "customobjectfield";
            fieldDefinition.Required = false;
            fieldDefinition.Label = new LocalizedString();
            fieldDefinition.Label.Add("en", "reference description");
            ReferenceFieldType fieldType = new ReferenceFieldType();
            fieldType.ReferenceTypeId = ReferenceFieldTypeId.KeyValueDocument;
            fieldDefinition.Type = fieldType;
            return fieldDefinition;
        }

        private static FieldDefinition CreateSetFieldDefinition()
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

        public static Fields CreateNewFields()
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

        #endregion
    }
}
