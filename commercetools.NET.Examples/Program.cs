using System;
using System.Configuration;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Project;

using Configuration = commercetools.Common.Configuration;

namespace commercetools.Examples
{
    /// <summary>
    /// These examples are meant to run against a commercetools project that contains some data;
    /// at minimum, the sample data that is included by default when creating a new project should
    /// be present so that there are at least one currency, language, and product type to work 
    /// with, along with a few products and categories.  All of these examples should successfully
    /// run against the sample data set that is included when you create a new commercetools 
    /// project.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">Arguments</param>
        public static void Main(string[] args)
        {
            new Program().Run().Wait();
        }

        /// <summary>
        /// Run all examples.
        /// </summary>
        /// <returns>Task</returns>
        private async Task Run()
        {
            /*  CONFIGURE CLIENT
             *  ===================================================================================
             *  Set up the Configuration object with your project information and use it to create
             *  and instance of the Client object.
             */
            Configuration configuration = new Configuration(
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.OAuthUrl"]),
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ApiUrl"]),
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ProjectKey"]),
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ClientID"]),
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["commercetools.ClientSecret"]),
                ProjectScope.ManageProject);
            
            Client client = new Client(configuration);            


            /*  GET PROJECT
             *  ===================================================================================
             *  The project reference has information about the project that is used in subsequent
             *  examples, like the project currencies and languages.
             */
            Response<Project.Project> projectResponse = await client.Project().GetProjectAsync();
            Project.Project project = null;

            if (projectResponse.Success)
            {
                project = projectResponse.Result;
            }
            else
            {
                Helper.WriteError<Project.Project>(projectResponse);
                return;
            }

            await CartExamples.Run(client, project);
            await CategoryExamples.Run(client, project);
            await OrderExamples.Run(client, project);
            await ProductExamples.Run(client, project);
                        

           
        }
    }
}
