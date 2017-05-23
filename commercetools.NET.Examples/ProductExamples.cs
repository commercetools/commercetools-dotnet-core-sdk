using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Products;
using commercetools.Products.UpdateActions;
using commercetools.ProductTypes;

namespace commercetools.Examples
{
    /// <summary>
    /// Code snippets for querying, creating, updating, and deleting Products.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-products.html"/>
    public class ProductExamples
    {
        /// <summary>
        /// Run Product example code.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="project">Project</param>
        public async static Task Run(Client client, Project.Project project)
        {
            string language = project.Languages[0];

            /*  GET PRODUCTS
             *  ===================================================================================
             */
            Response<ProductQueryResult> productQueryResponse = await client.Products().QueryProductsAsync();
            List<Product> products = new List<Product>();

            if (productQueryResponse.Success)
            {
                ProductQueryResult productQueryResult = productQueryResponse.Result;
                products = productQueryResult.Results;

                Console.WriteLine("Retrieved {0} products.", products.Count);
            }
            else
            {
                Helper.WriteError<ProductQueryResult>(productQueryResponse);
            }

            // Get a single product by its ID.
            string productId = products[0].Id;
            Response<Product> productResponse = await client.Products().GetProductByIdAsync(productId);
            Product product = null;

            if (productResponse.Success)
            {
                product = productResponse.Result;
                Console.WriteLine("Retrieved product with ID {0}.", product.Id);
            }
            else
            {
                Helper.WriteError<Product>(productResponse);
            }

            /*  CREATE PRODUCT
             *  ===================================================================================
             *  You will need to specify the type of product you are creating using a 
             *  resource identifier that references an existing product type in your commercetools 
             *  project. So before creating the product, we will retrieve a ProductType from the
             *  API and use it to create a ResourceIdentifier object.
             */
            Response<ProductTypeQueryResult> productTypeResponse = await client.ProductTypes().QueryProductTypesAsync();
            ProductType productType = null;

            if (productTypeResponse.Success)
            {
                ProductTypeQueryResult productTypeQueryResult = productTypeResponse.Result;
                productType = productTypeQueryResult.Results[0];
                Console.WriteLine("Retrieved product type with ID {0}.", productType.Id);
            }
            else
            {
                Helper.WriteError<ProductTypeQueryResult>(productTypeResponse);
                return;
            }

            ResourceIdentifier resourceIdentifier = new ResourceIdentifier();
            resourceIdentifier.TypeId = Common.ReferenceType.ProductType;
            resourceIdentifier.Id = productType.Id;

            LocalizedString productName = new LocalizedString();
            productName[language] = "My New Product";

            LocalizedString productSlug = new LocalizedString();
            productSlug[language] = "mynewproduct";

            ProductDraft productDraft = new ProductDraft(productName, resourceIdentifier, productSlug);
            productResponse = await client.Products().CreateProductAsync(productDraft);

            if (productResponse.Success)
            {
                product = productResponse.Result;
                Console.WriteLine("Created new product with ID {0}.", product.Id);
                Console.WriteLine("Product name: {0}", product.MasterData.Staged.Name[language]);
                Console.WriteLine("Product slug: {0}", product.MasterData.Staged.Slug[language]);
            }
            else
            {
                Helper.WriteError<Product>(productResponse);
            }

            /*  UPDATE A PRODUCT
             *  ===================================================================================
             *  Each change is made using its own update action object which maps to an update 
             *  action call in the API. The list of update action objects are sent to the API using
             *  a single request. If there is an update action in the API that has not yet been 
             *  implemented in the SDK, you can use the GenericAction class to make any request you 
             *  want (as long as it is a valid update action supported by the API).
             */
            SetKeyAction setKeyAction = new SetKeyAction();
            setKeyAction.Key = "ABC123";

            // Here is how you would make the changeName request using a GenericAction object.
            productName[language] = "My New Name";

            GenericAction changeNameAction = new GenericAction("changeName");
            changeNameAction["name"] = productName;
            changeNameAction["staged"] = true;

            List<UpdateAction> actions = new List<UpdateAction>() { setKeyAction, changeNameAction };

            productResponse = await client.Products().UpdateProductAsync(product, actions);

            if (productResponse.Success)
            {
                product = productResponse.Result;
                Console.WriteLine("Updated product with ID {0}.", product.Id);
                Console.WriteLine("Product key: {0}", product.Key);
                Console.WriteLine("Updated product name: {0}", product.MasterData.Staged.Name[language]);
            }
            else
            {
                Helper.WriteError<Product>(productResponse);
            }

            /*  DELETE A PRODUCT
             *  ===================================================================================
             *  Delete API requests return a generic response, but some return the object that was 
             *  deleted. The Products delete request returns the full representation.
             */
            productResponse = await client.Products().DeleteProductAsync(product);

            if (productResponse.Success)
            {
                product = productResponse.Result;
                Console.WriteLine("Deleted product with ID {0}.", product.Id);
            }
            else
            {
                Helper.WriteError<Product>(productResponse);
            }
        }
    }
}
