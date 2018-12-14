using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using commercetools.Sdk.Domain.Products.Attributes;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class ProductTypeFixture : ClientFixture, IDisposable
    {
        public List<ProductType> ProductTypesToDelete;

        public ProductTypeFixture()
        {
            this.ProductTypesToDelete = new List<ProductType>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.ProductTypesToDelete.Reverse();
            foreach (ProductType type in this.ProductTypesToDelete)
            {
                ProductType deletedType = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<ProductType>(new Guid(type.Id), type.Version)).Result;
            }
        }

        public ProductType CreateProductType()
        {
            ProductTypeDraft productTypeDraft = this.CreateProductTypeDraft();
            IClient commerceToolsClient = this.GetService<IClient>();
            ProductType createdType = commerceToolsClient.ExecuteAsync(new CreateCommand<ProductType>(productTypeDraft)).Result;
            return createdType;
        }

        public ProductTypeDraft CreateProductTypeDraft()
        {
            ProductTypeDraft productTypeDraft = new ProductTypeDraft();
            productTypeDraft.Name = this.RandomString(5);
            productTypeDraft.Description = this.RandomString(4);
            productTypeDraft.Attributes = new List<AttributeDefinitionDraft>();
            productTypeDraft.Attributes.Add(this.CreateTextAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateLocalizedTextAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateBooleanAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateNumberAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateDateAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateDateTimeAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateTimeAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateMoneyAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateReferenceAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateSetAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateEnumAttributeDefinitionDraft());
            productTypeDraft.Attributes.Add(this.CreateLocalizedEnumAttributeDefinitionDraft());
            return productTypeDraft;
        }

        public AttributeDefinitionDraft CreateTextAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "text-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "text-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new TextAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateLocalizedTextAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "localized-text-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "localized-text-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new LocalizableTextAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateBooleanAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "boolean-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "boolean-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new BooleanAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateNumberAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "number-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "number-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new NumberAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateDateAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "date-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "date-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new DateAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateDateTimeAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "date-time-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "date-time-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new DateTimeAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateTimeAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "time-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "time-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new TimeAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateReferenceAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "reference-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "reference-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            ReferenceAttributeType referenceAttributeType = new ReferenceAttributeType();
            referenceAttributeType.ReferenceTypeId = ReferenceFieldTypeId.Category;
            attributeDefinitionDraft.Type = referenceAttributeType;
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateEnumAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "enum-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "enum-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            EnumAttributeType enumAttributeType = new EnumAttributeType();
            enumAttributeType.Values = new List<PlainEnumValue>();
            enumAttributeType.Values.Add(new PlainEnumValue() { Key = "enum-key-1", Label = "enum-label-1" });
            enumAttributeType.Values.Add(new PlainEnumValue() { Key = "enum-key-2", Label = "enum-label-2" });
            attributeDefinitionDraft.Type = enumAttributeType;
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateLocalizedEnumAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "localized-enum-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "localized-enum-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            LocalizableEnumAttributeType localizableEnumAttributeType = new LocalizableEnumAttributeType();
            localizableEnumAttributeType.Values = new List<LocalizedEnumValue>();
            localizableEnumAttributeType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-1", Label = new LocalizedString() { { "en", "enum-label-1" } } });
            localizableEnumAttributeType.Values.Add(new LocalizedEnumValue() { Key = "enum-key-2", Label = new LocalizedString() { { "en", "enum-label-2" } } });
            attributeDefinitionDraft.Type = localizableEnumAttributeType;
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateMoneyAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "money-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "money-attribute-label" } };
            attributeDefinitionDraft.IsRequired = true;
            attributeDefinitionDraft.Type = new MoneyAttributeType();
            return attributeDefinitionDraft;
        }

        public AttributeDefinitionDraft CreateSetAttributeDefinitionDraft()
        {
            AttributeDefinitionDraft attributeDefinitionDraft = new AttributeDefinitionDraft();
            attributeDefinitionDraft.Name = "set-text-attribute-name";
            attributeDefinitionDraft.Label = new LocalizedString() { { "en", "set-text-attribute-label" } };
            SetAttributeType setAttributeType = new SetAttributeType();
            setAttributeType.ElementType = new TextAttributeType();
            attributeDefinitionDraft.Type = setAttributeType;
            return attributeDefinitionDraft;
        }
    }
}
