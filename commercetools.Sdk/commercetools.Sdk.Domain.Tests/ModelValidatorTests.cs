using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain.Validation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class ModelValidatorTests
    {
        public ModelValidatorTests()
        {
            // Validation attributes use service locator to get the implementation objects.
            var services = new ServiceCollection();
            services.UseDomain();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [Fact]
        public void IsValidTextSearchModelInvalidLocale()
        {
            IModelValidator modelValidator = new ModelValidator();
            ReviewDraft reviewDraft = new ReviewDraft
            {
                Locale = "en-ZZ"
            };
            var result = modelValidator.IsValid(reviewDraft);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void IsValidTextSearchModelValidLocale()
        {
            IModelValidator modelValidator = new ModelValidator();
            ReviewDraft reviewDraft = new ReviewDraft
            {
                Locale = "en-US"
            };
            var result = modelValidator.IsValid(reviewDraft);
            Assert.Empty(result);
        }
    }
}
