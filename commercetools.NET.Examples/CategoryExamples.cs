using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Categories;
using commercetools.Categories.UpdateActions;

namespace commercetools.Examples
{
    /// <summary>
    /// Code snippets for querying, creating, updating, and deleting Categories.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-categories.html"/>
    public class CategoryExamples
    {
        /// <summary>
        /// Run Category example code.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="project">Project</param>
        public async static Task Run(Client client, Project.Project project)
        {
            string language = project.Languages[0];

            /*  GET CATEGORIES
             *  ===================================================================================
             */
            Response<CategoryQueryResult> categoryQueryResponse = await client.Categories().QueryCategoriesAsync();
            List<Category> categories = new List<Category>();

            if (categoryQueryResponse.Success)
            {
                CategoryQueryResult categoryQueryResult = categoryQueryResponse.Result;
                categories = categoryQueryResult.Results;

                Console.WriteLine("Retrieved {0} categories.", categories.Count);
            }
            else
            {
                Helper.WriteError<CategoryQueryResult>(categoryQueryResponse);
            }

            // Get a single category by its ID.
            string categoryId = categories[0].Id;
            Response<Category> categoryResponse = await client.Categories().GetCategoryByIdAsync(categoryId);
            Category category = null;

            if (categoryResponse.Success)
            {
                category = categoryResponse.Result;
                Console.WriteLine("Retrieved category with ID {0}.", category.Id);
            }
            else
            {
                Helper.WriteError<Category>(categoryResponse);
            }

            /*  CREATE CATEGORY
             *  ===================================================================================
             */
            LocalizedString categoryName = new LocalizedString();
            categoryName[language] = "My New Category";

            LocalizedString categorySlug = new LocalizedString();
            categorySlug[language] = "mynewcategory";

            CategoryDraft categoryDraft = new CategoryDraft(categoryName, categorySlug);
            categoryResponse = await client.Categories().CreateCategoryAsync(categoryDraft);

            if (categoryResponse.Success)
            {
                category = categoryResponse.Result;
                Console.WriteLine("Created new category with ID {0}.", category.Id);
                Console.WriteLine("Category name: {0}", category.Name[language]);
                Console.WriteLine("Category slug: {0}", category.Slug[language]);
            }
            else
            {
                Helper.WriteError<Category>(categoryResponse);
            }

            /*  UPDATE A CATEGORY
             *  ===================================================================================
             *  Each change is made using its own update action object which maps to an update 
             *  action call in the API. The list of update action objects are sent to the API using
             *  a single request. If there is an update action in the API that has not yet been 
             *  implemented in the SDK, you can use the GenericAction class to make any request you 
             *  want (as long as it is a valid update action supported by the API).
             */
            categoryName[language] = "Updated Category";
            ChangeNameAction changeNameAction = new ChangeNameAction(categoryName);

            // Here is how you would make the setDescription request using a GenericAction object.
            LocalizedString categoryDescription = new LocalizedString();
            categoryDescription[language] = "This is a new category created by the commercetools.NET SDK.";

            GenericAction setDescriptionAction = new GenericAction("setDescription");
            setDescriptionAction["description"] = categoryDescription;

            List<UpdateAction> actions = new List<UpdateAction>() { changeNameAction, setDescriptionAction };

            categoryResponse = await client.Categories().UpdateCategoryAsync(category, actions);

            if (categoryResponse.Success)
            {
                category = categoryResponse.Result;
                Console.WriteLine("Updated category with ID {0}.", category.Id);
                Console.WriteLine("Updated category name: {0}", category.Name[language]);
                Console.WriteLine("Category description: {0}", category.Description[language]);
            }
            else
            {
                Helper.WriteError<Category>(categoryResponse);
            }

            /*  DELETE A CATEGORY
             *  ===================================================================================
             *  Delete API requests return a generic response, but some return the object that was 
             *  deleted. The Categories delete request returns the full representation.
             */
            categoryResponse = await client.Categories().DeleteCategoryAsync(category);

            if (categoryResponse.Success)
            {
                category = categoryResponse.Result;
                Console.WriteLine("Deleted category with ID {0}.", category.Id);
            }
            else
            {
                Helper.WriteError<Category>(categoryResponse);
            }
        }
    }
}
