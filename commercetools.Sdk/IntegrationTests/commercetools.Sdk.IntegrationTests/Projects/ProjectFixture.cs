using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Projects;
using commercetools.Sdk.Domain.Projects.UpdateActions;
using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.IntegrationTests.Projects
{
    public static class ProjectFixture
    {
        public static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1,1);
        
        /// <summary>
        /// Get Current Project Languages
        /// </summary>
        /// <returns></returns>
        public static List<string> GetProjectLanguages(IClient client)
        {
            var command = new GetProjectCommand();
            var project = client.ExecuteAsync(command).Result;
            return project.Languages.ToList();
        }

        #region SetupProject

        public static async Task<Project> SetupProject(IClient client, Project project, List<UpdateAction<Project>> updateActions)
        {
           var updatedProject = await TryToUpdateCurrentProject(client, project, updateActions);
           return updatedProject;
        }
        public static async Task<Project> TryToUpdateCurrentProject(IClient client, Project project, List<UpdateAction<Project>> updateActions)
        {
            while (true)
            {
                try
                {
                    var updatedProject = await client
                        .ExecuteAsync(new UpdateProjectCommand(project.Version, updateActions));
                    return updatedProject;
                }
                catch (ConcurrentModificationException)
                {
                    project = await client.ExecuteAsync(new GetProjectCommand());
                }
            }
        }

        public static List<UpdateAction<Project>> SetProjectCurrencies(Project project, List<string> currencies)
        {
            var actions = new List<UpdateAction<Project>>();
            actions.AddUpdate(new ChangeCurrenciesUpdateAction
            {
                Currencies = currencies
            });
            return actions;
        }
        public static List<UpdateAction<Project>> SetProjectCountries(Project project, List<string> countries)
        {
            var actions = new List<UpdateAction<Project>>();
            actions.AddUpdate(new ChangeCountriesUpdateAction
            {
                Countries = countries
            });
            return actions;
        }
        
        public static List<UpdateAction<Project>> SetProjectLanguages(Project project, List<string> languages)
        {
            var actions = new List<UpdateAction<Project>>();
            actions.AddUpdate(new ChangeLanguagesUpdateAction
            {
                Languages = languages
            });
            return actions;
        }
        
        public static List<UpdateAction<Project>> SetMessageConfiguration(Project project, MessagesConfigurationDraft messagesConfiguration)
        {
            var actions = new List<UpdateAction<Project>>();
            actions.AddUpdate(new ChangeMessagesConfigurationUpdateAction
            {
                MessagesConfiguration = messagesConfiguration
            });
            return actions;
        }
        
        public static List<UpdateAction<Project>> SetShippingRateInputType(Project project, ShippingRateInputType shippingRateInputType)
        {
            var actions = new List<UpdateAction<Project>>();
            actions.AddUpdate(new SetShippingRateInputTypeUpdateAction
            {
                ShippingRateInputType = shippingRateInputType
            });
            return actions;
        }
        public static List<UpdateAction<Project>> SetExternalOAuth(Project project, ExternalOAuth externalOAuth)
        {
            var actions = new List<UpdateAction<Project>>();
            actions.AddUpdate(new SetExternalOAuthUpdateAction
            {
                ExternalOAuth = externalOAuth
            });
            return actions;
        }

        #endregion
        

        #region WithCurrentProject

        public static async Task WithCurrentProject(
            IClient client,
            Func<Project, Task<Project>> func
        )
        {
            await WithCurrentProject(
                client,
                project => new List<UpdateAction<Project>>(),
                func
            );
        }
        
        public static async Task WithCurrentProject(
            IClient client,
            Func<Project, List<UpdateAction<Project>>> setupUpdateActions,
            Func<Project, Task<Project>> func,
            Func<IClient, Project, List<UpdateAction<Project>>, Task<Project>> setupFunc = null
        )
        {
            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await SemaphoreSlim.WaitAsync();

            try
            {
                setupFunc = setupFunc ?? SetupProject;

                var currentProject = await client.ExecuteAsync(new GetProjectCommand());

                var setupActions = setupUpdateActions.Invoke(currentProject);

                var updatedProject = setupActions.Count > 0
                    ? await setupFunc(client, currentProject, setupActions)
                    : currentProject;

                await func(updatedProject);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
        
        public static async Task WithCurrentProject(
            IClient client,
            Action<Project> func
        )
        {
            await WithCurrentProject(
                client,
                project => new List<UpdateAction<Project>>(),
                func
            );
        }
        
        public static async Task WithCurrentProject(
            IClient client,
            Func<Project, List<UpdateAction<Project>>> setupUpdateActions,
            Action<Project> func,
            Func<IClient, Project, List<UpdateAction<Project>>, Task<Project>> setupFunc = null
        )
        {
            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await SemaphoreSlim.WaitAsync();
            try
            {
                setupFunc = setupFunc ?? SetupProject;

                var currentProject = await client.ExecuteAsync(new GetProjectCommand());

                var setupActions = setupUpdateActions.Invoke(currentProject);

                var updatedProject = setupActions.Count > 0
                    ? await setupFunc(client, currentProject, setupActions)
                    : currentProject;
                func(updatedProject);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
        #endregion
    }
}
