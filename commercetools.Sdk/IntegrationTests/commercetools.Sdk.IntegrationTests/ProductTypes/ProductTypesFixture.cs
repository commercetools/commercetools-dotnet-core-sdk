using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Domain.Types;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.ProductTypes
{
    public static class ProductTypesFixture
    {
        #region DraftBuilds

        public static ProductTypeDraft DefaultProductTypeDraft(ProductTypeDraft productTypeDraft)
        {
            productTypeDraft.Key = TestingUtility.RandomString(10);
            productTypeDraft.Name = TestingUtility.RandomString(10);
            productTypeDraft.Description = TestingUtility.RandomString(10);
            productTypeDraft.Attributes = new List<AttributeDefinitionDraft>();
            productTypeDraft.Attributes.Add(CreateTextAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateLocalizedTextAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateBooleanAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateNumberAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateDateAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateDateTimeAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateTimeAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateMoneyAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateReferenceAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateSetAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateEnumAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(CreateLocalizedEnumAttributeDefinitionDraft());
            return productTypeDraft;
        }

        public static ProductTypeDraft DefaultProductTypeDraftWithKey(ProductTypeDraft draft, string key)
        {
            var productTypeDraft = DefaultProductTypeDraft(draft);
            productTypeDraft.Key = key;
            return productTypeDraft;
        }

        public static ProductTypeDraft DefaultProductTypeDraftWithEmptyAttributes(ProductTypeDraft draft)
        {
            var productTypeDraft = DefaultProductTypeDraft(draft);
            productTypeDraft.Attributes.Clear();
            return productTypeDraft;
        }
        public static ProductTypeDraft DefaultProductTypeDraftWithOnlyTextAttribute(ProductTypeDraft draft)
        {
            var productTypeDraft = DefaultProductTypeDraft(draft);
            productTypeDraft.Attributes.RemoveAll(attribute => attribute.Type.GetType() != typeof(TextAttributeType));
            return productTypeDraft;
        }

        public static ProductTypeDraft DefaultProductTypeDraftWithOnlyTextAttributeWithConstraint(ProductTypeDraft draft, AttributeConstraint constraint)
        {
            var productTypeDraft = DefaultProductTypeDraft(draft);
            productTypeDraft.Attributes.RemoveAll(attribute => attribute.Type.GetType() != typeof(TextAttributeType));
            productTypeDraft.Attributes[0].AttributeConstraint = constraint;
            return productTypeDraft;
        }

        #endregion

        #region WithProductType

        public static async Task WithProductType( IClient client, Action<ProductType> func)
        {
            await With(client, new ProductTypeDraft(), DefaultProductTypeDraft, func);
        }
        public static async Task WithProductType( IClient client, Func<ProductTypeDraft, ProductTypeDraft> draftAction, Action<ProductType> func)
        {
            await With(client, new ProductTypeDraft(), draftAction, func);
        }

        public static async Task WithProductType( IClient client, Func<ProductType, Task> func)
        {
            await WithAsync(client, new ProductTypeDraft(), DefaultProductTypeDraft, func);
        }
        public static async Task WithProductType( IClient client, Func<ProductTypeDraft, ProductTypeDraft> draftAction, Func<ProductType, Task> func)
        {
            await WithAsync(client, new ProductTypeDraft(), draftAction, func);
        }
        #endregion

        #region WithUpdateableProductType

        public static async Task WithUpdateableProductType(IClient client, Func<ProductType, ProductType> func)
        {
            await WithUpdateable(client, new ProductTypeDraft(), DefaultProductTypeDraft, func);
        }

        public static async Task WithUpdateableProductType(IClient client, Func<ProductTypeDraft, ProductTypeDraft> draftAction, Func<ProductType, ProductType> func)
        {
            await WithUpdateable(client, new ProductTypeDraft(), draftAction, func);
        }

        public static async Task WithUpdateableProductType(IClient client, Func<ProductType, Task<ProductType>> func)
        {
            await WithUpdateableAsync(client, new ProductTypeDraft(), DefaultProductTypeDraft, func);
        }
        public static async Task WithUpdateableProductType(IClient client, Func<ProductTypeDraft, ProductTypeDraft> draftAction, Func<ProductType, Task<ProductType>> func)
        {
            await WithUpdateableAsync(client, new ProductTypeDraft(), draftAction, func);
        }

        #endregion


        #region HelperFunctions

        public static AttributeDefinitionDraft CreateTextAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "text-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "text-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new TextAttributeType();
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateLocalizedTextAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "localized-text-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "localized-text-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new LocalizableTextAttributeType();
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateBooleanAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "boolean-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "boolean-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new BooleanAttributeType();
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateNumberAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "number-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "number-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new NumberAttributeType();
            attributeDefinitionDraft.IsSearchable = true;
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateDateAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "date-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "date-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new DateAttributeType();
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateDateTimeAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "date-time-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "date-time-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new DateTimeAttributeType();
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateTimeAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "time-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "time-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new TimeAttributeType();
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateReferenceAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "reference-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "reference-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            ReferenceAttributeType referenceAttributeType = new ReferenceAttributeType();
            referenceAttributeType.ReferenceTypeId = ReferenceFieldTypeId.ProductType;
            attributeDefinitionDraft.Type = referenceAttributeType;
            attributeDefinitionDraft.IsSearchable = true;
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateEnumAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "enum-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "enum-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            EnumAttributeType enumAttributeType = new EnumAttributeType();
            enumAttributeType.Values = new List<PlainEnumValue>();
            enumAttributeType.Values.Add(new PlainEnumValue() { Key = "enum-key-1", Label = "enum-label-1" });
            enumAttributeType.Values.Add(new PlainEnumValue() { Key = "enum-key-2", Label = "enum-label-2" });
            enumAttributeType.Values.Add(new PlainEnumValue() { Key = "enum-key-3", Label = "enum-label-3" });
            attributeDefinitionDraft.Type = enumAttributeType;
            attributeDefinitionDraft.IsSearchable = true;
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateLocalizedEnumAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "localized-enum-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "localized-enum-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            LocalizableEnumAttributeType localizableEnumAttributeType = new LocalizableEnumAttributeType();
            localizableEnumAttributeType.Values = new List<LocalizedEnumValue>();
            localizableEnumAttributeType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-1", Label = new LocalizedString() { { "en", "enum-label-1" } } });
            localizableEnumAttributeType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-2", Label = new LocalizedString() { { "en", "enum-label-2" } } });
            attributeDefinitionDraft.Type = localizableEnumAttributeType;
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateMoneyAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "money-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "money-attribute-label" } };
            attributeDefinitionDraft.IsRequired = false;
            attributeDefinitionDraft.Type = new MoneyAttributeType();
            attributeDefinitionDraft.IsSearchable = true;
            return attributeDefinitionDraft;
        }

        private static AttributeDefinitionDraft CreateSetAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "set-text-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "set-text-attribute-label" } };
            SetAttributeType setAttributeType = new SetAttributeType();
            setAttributeType.ElementType = new TextAttributeType();
            attributeDefinitionDraft.Type = setAttributeType;
            return attributeDefinitionDraft;
        }

        #endregion
    }
}
