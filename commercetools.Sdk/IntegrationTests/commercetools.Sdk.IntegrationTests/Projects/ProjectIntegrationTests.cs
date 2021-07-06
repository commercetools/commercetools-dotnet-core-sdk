using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.Domain.Projects.UpdateActions;
using Xunit;
using static commercetools.Sdk.IntegrationTests.Projects.ProjectFixture;

namespace commercetools.Sdk.IntegrationTests.Projects
{
    [Collection("Integration Tests")]
    public class ProjectIntegrationTests
    {
        const string skip = "skipped now to solve ci build";
        private readonly IClient client;

        public ProjectIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.client = serviceProviderFixture.GetService<IClient>();
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeName()
        {
            await WithCurrentProject(client,
                async project =>
                {
                    Assert.NotNull(project);
                    var oldName = project.Name;
                    var newName = oldName + "-New";

                    var action = new ChangeNameUpdateAction
                    {
                        Name = newName
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.Equal(newName, updatedProject.Name);

                    // then undo this change again
                    action = new ChangeNameUpdateAction
                    {
                        Name = oldName
                    };
                    var projectWithOriginalName = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.Equal(oldName, projectWithOriginalName.Name);
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeCurrencies()
        {
            var predefinedCurrencies = new List<string> {"EUR"};
            var newCurrencies = new List<string> {"EUR", "USD"};

            await WithCurrentProject(client,
                project => SetProjectCurrencies(project, predefinedCurrencies),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.True(predefinedCurrencies.SequenceEqual(project.Currencies));

                    var action = new ChangeCurrenciesUpdateAction
                    {
                        Currencies = newCurrencies
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.True(newCurrencies.SequenceEqual(updatedProject.Currencies));

                    // then undo this change again
                    action = new ChangeCurrenciesUpdateAction
                    {
                        Currencies = predefinedCurrencies
                    };
                    var projectWithOriginalCurrencies = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.True(predefinedCurrencies.SequenceEqual(projectWithOriginalCurrencies.Currencies));
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeCountries()
        {
            var predefinedCountries = new List<string> {"DE"};
            var newCountries = new List<string> {"DE", "US"};

            await WithCurrentProject(client,
                project => SetProjectCountries(project, predefinedCountries),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.True(predefinedCountries.SequenceEqual(project.Countries));

                    var action = new ChangeCountriesUpdateAction
                    {
                        Countries = newCountries
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.True(newCountries.SequenceEqual(updatedProject.Countries));

                    // then undo this change again
                    action = new ChangeCountriesUpdateAction
                    {
                        Countries = predefinedCountries
                    };
                    var projectWithOriginalCountries = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.True(predefinedCountries.SequenceEqual(projectWithOriginalCountries.Countries));
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeLanguages()
        {
            var predefinedLanguages = new List<string> {"en", "de"};
            var newLanguages = new List<string> {"en", "de", "es"};

            await WithCurrentProject(client,
                project => SetProjectLanguages(project, predefinedLanguages),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.True(predefinedLanguages.SequenceEqual(project.Languages));

                    var action = new ChangeLanguagesUpdateAction
                    {
                        Languages = newLanguages
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.True(newLanguages.SequenceEqual(updatedProject.Languages));

                    // then undo this change again
                    action = new ChangeLanguagesUpdateAction
                    {
                        Languages = predefinedLanguages
                    };
                    var projectWithOriginalLanguages = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.True(predefinedLanguages.SequenceEqual(projectWithOriginalLanguages.Languages));
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeMessagesConfiguration()
        {
            var predefinedMessageConfiguration = new MessagesConfigurationDraft
            {
                Enabled = true,
                DeleteDaysAfterCreation = 15
            };
            var newMessageConfiguration = new MessagesConfigurationDraft
            {
                Enabled = true,
                DeleteDaysAfterCreation = 30
            };

            await WithCurrentProject(client,
                project => SetMessageConfiguration(project, predefinedMessageConfiguration),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.Equal(predefinedMessageConfiguration.DeleteDaysAfterCreation,
                        project.Messages.DeleteDaysAfterCreation);

                    var action = new ChangeMessagesConfigurationUpdateAction
                    {
                        MessagesConfiguration = newMessageConfiguration
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.Equal(newMessageConfiguration.DeleteDaysAfterCreation,
                        updatedProject.Messages.DeleteDaysAfterCreation);

                    // then undo this change again
                    action = new ChangeMessagesConfigurationUpdateAction
                    {
                        MessagesConfiguration = predefinedMessageConfiguration
                    };
                    var projectWithOriginalMessageConfiguration = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    
                    Assert.Equal(predefinedMessageConfiguration.DeleteDaysAfterCreation,
                        projectWithOriginalMessageConfiguration.Messages.DeleteDaysAfterCreation);
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectSetShippingRateInputType()
        {
            var shippingRateInputType = new CartScoreShippingRateInputType();

            await WithCurrentProject(client,
                project => SetShippingRateInputType(project, null),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.Null(project.ShippingRateInputType);

                    var action = new SetShippingRateInputTypeUpdateAction
                    {
                        ShippingRateInputType = shippingRateInputType
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.NotNull(updatedProject.ShippingRateInputType);
                    Assert.IsType<CartScoreShippingRateInputType>(updatedProject.ShippingRateInputType);

                    // then undo this change again
                    action = new SetShippingRateInputTypeUpdateAction
                    {
                        ShippingRateInputType = null
                    };
                    var projectWithNullShippingRateInputType = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    
                    Assert.Null(projectWithNullShippingRateInputType.ShippingRateInputType);
                });
        }

        [Fact(Skip = skip)]
        public async Task UpdateProjectSetExternalOAuth()
        {
            var externalOAuth = new ExternalOAuth
            {
                Url = "http://www.externalOAuth.com/",
                AuthorizationHeader = "QWxhZGRpbjpvcGVuIHNlc2FtZQ=="
            };

            await WithCurrentProject(client,
                project => SetExternalOAuth(project, null),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.Null(project.ExternalOAuth);

                    var action = new SetExternalOAuthUpdateAction
                    {
                        ExternalOAuth = externalOAuth
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.NotNull(updatedProject.ExternalOAuth);
                    Assert.Equal(externalOAuth.Url, updatedProject.ExternalOAuth.Url);

                    // then undo this change again
                    action = new SetExternalOAuthUpdateAction
                    {
                        ExternalOAuth = null
                    };
                    var projectWithNullExternalOAuth = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.Null(projectWithNullExternalOAuth.ExternalOAuth);
                });
        }
        
        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeCartsConfiguration()
        {
            var oldDeleteDaysAfterLastModification = 20;
            var newDeleteDaysAfterLastModification = 80;
            

            await WithCurrentProject(client,
                project => SetProjectCartConfiguration(project, new CartsConfiguration
                {
                    DeleteDaysAfterLastModification = oldDeleteDaysAfterLastModification
                }),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.NotNull(project.Carts);
                    Assert.Equal(oldDeleteDaysAfterLastModification, project.Carts.DeleteDaysAfterLastModification);

                    var action = new ChangeCartsConfigurationUpdateAction
                    {
                        CartsConfiguration = new CartsConfiguration
                        {
                            DeleteDaysAfterLastModification = newDeleteDaysAfterLastModification
                        }
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.NotNull(updatedProject.Carts);
                    Assert.Equal(newDeleteDaysAfterLastModification,updatedProject.Carts.DeleteDaysAfterLastModification);

                    // then undo this change again
                    action = new ChangeCartsConfigurationUpdateAction
                    {
                        CartsConfiguration = new CartsConfiguration()
                    };
                    var projectWithOriginalSettings = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.NotNull(projectWithOriginalSettings.Carts);
                    Assert.Null(projectWithOriginalSettings.Carts.DeleteDaysAfterLastModification);
                });
        }
        
        [Fact(Skip = skip)]
        public async Task UpdateProjectChangeShoppingListsConfiguration()
        {
            var oldDeleteDaysAfterLastModification = 20;
            var newDeleteDaysAfterLastModification = 80;
            

            await WithCurrentProject(client,
                project => SetProjectShoppingListsConfiguration(project, new ShoppingListsConfiguration
                {
                    DeleteDaysAfterLastModification = oldDeleteDaysAfterLastModification
                }),
                async project =>
                {
                    Assert.NotNull(project);
                    Assert.NotNull(project.ShoppingLists);
                    Assert.Equal(oldDeleteDaysAfterLastModification, project.ShoppingLists.DeleteDaysAfterLastModification);

                    var action = new ChangeShoppingListsConfigurationUpdateAction
                    {
                        ShoppingListsConfiguration = new ShoppingListsConfiguration
                        {
                            DeleteDaysAfterLastModification = newDeleteDaysAfterLastModification
                        }
                    };

                    var updatedProject = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());

                    Assert.NotNull(updatedProject.ShoppingLists);
                    Assert.Equal(newDeleteDaysAfterLastModification,updatedProject.ShoppingLists.DeleteDaysAfterLastModification);

                    // then undo this change again
                    action = new ChangeShoppingListsConfigurationUpdateAction
                    {
                        ShoppingListsConfiguration = new ShoppingListsConfiguration()
                    };
                    var projectWithOriginalSettings = await 
                        TryToUpdateCurrentProject(client, project, action.ToList());
                    Assert.NotNull(projectWithOriginalSettings.ShoppingLists);
                    Assert.Null(projectWithOriginalSettings.ShoppingLists.DeleteDaysAfterLastModification);
                });
        }
    }
}