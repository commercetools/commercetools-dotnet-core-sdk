using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Carts;
using commercetools.Carts.UpdateActions;
using commercetools.Products;

namespace commercetools.Examples
{
    /// <summary>
    /// Code snippets for creating, querying, updating, and deleting Carts.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-carts.html"/>
    public class CartExamples
    {
        /// <summary>
        /// Run Cart example code.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="project">Project</param>
        public async static Task Run(Client client, Project.Project project)
        {
            string currency = project.Currencies[0];
            string country = project.Countries[0];

            /*  CREATE CART
             *  ===================================================================================
             */
            CartDraft cartDraft = new CartDraft(currency);
            Response<Cart> cartResponse = await client.Carts().CreateCartAsync(cartDraft);
            Cart cart = null;
            
            if (cartResponse.Success)
            {
                cart = cartResponse.Result;
                Console.WriteLine("Created new cart with ID {0}.", cart.Id);
            }
            else
            {
                Helper.WriteError<Cart>(cartResponse);
            }

            /*  GET CARTS
             *  ===================================================================================
             */
            Response<CartQueryResult> cartQueryResponse = await client.Carts().QueryCartsAsync();
            List<Cart> carts = new List<Cart>();

            if (cartQueryResponse.Success)
            {
                CartQueryResult cartQueryResult = cartQueryResponse.Result;
                carts = cartQueryResult.Results;

                Console.WriteLine("Retrieved {0} carts.", carts.Count);
            }
            else
            {
                Helper.WriteError<CartQueryResult>(cartQueryResponse);
            }

            // Get a single cart by its ID.
            string cartId = carts[0].Id;
            cartResponse = await client.Carts().GetCartByIdAsync(cartId);

            if (cartResponse.Success)
            {
                cart = cartResponse.Result;
                Console.WriteLine("Retrieved cart with ID {0}.", cart.Id);
            }
            else
            {
                Helper.WriteError<Cart>(cartResponse);
            }

            /*  UPDATE A CART
             *  ===================================================================================
             *  Each change is made using its own update action object which maps to an update 
             *  action call in the API. The list of update action objects are sent to the API using
             *  a single request. If there is an update action in the API that has not yet been 
             *  implemented in the SDK, you can use the GenericAction class to make any request you 
             *  want (as long as it is a valid update action supported by the API).
             */
            SetCountryAction setCountryAction = new SetCountryAction();
            setCountryAction.Country = country;

            // Here is how you would make the setCustomerEmail request using a GenericAction object.
            GenericAction setCustomerEmailAction = new GenericAction("setCustomerEmail");
            setCustomerEmailAction["email"] = "customer@example.com";

            //List<UpdateAction> actions = new List<UpdateAction>() { setCountryAction, setCustomerEmailAction };
            List<UpdateAction> actions = new List<UpdateAction>() { setCustomerEmailAction };

            cartResponse = await client.Carts().UpdateCartAsync(cart, actions);

            if (cartResponse.Success)
            {
                cart = cartResponse.Result;
                Console.WriteLine("Updated cart with ID {0}.", cart.Id);
                Console.WriteLine("Cart country: {0}", cart.Country);
                Console.WriteLine("Cart customer email: {0}", cart.CustomerEmail);
            }
            else
            {
                Helper.WriteError<Cart>(cartResponse);
            }

            /*  ADDING, CHANGING AND REMOVING LINE ITEMS
             *  ===================================================================================
             *  Making line item requests is done using update actions.
             */

            // Find a product to add to the cart as a line item.
            Response<ProductQueryResult> productQueryResponse = await client.Products().QueryProductsAsync();
            Product product = null;

            if (productQueryResponse.Success)
            {
                ProductQueryResult productQueryResult = productQueryResponse.Result;
                product = productQueryResult.Results[0];
                Console.WriteLine("Retrieved product with ID {0}.", product.Id);
            }
            else
            {
                Helper.WriteError<ProductQueryResult>(productQueryResponse);
            }

            // Add the line item to a cart.
            AddLineItemAction addLineItemAction = new AddLineItemAction(product.Id, product.MasterData.Current.MasterVariant.Id);
            addLineItemAction.Quantity = 1;

            Console.WriteLine("addLineItemAction.Action: {0}", addLineItemAction.Action);
            Console.WriteLine("addLineItemAction.ProductId: {0}", addLineItemAction.ProductId);
            Console.WriteLine("addLineItemAction.VariantId: {0}", addLineItemAction.VariantId);
            Console.WriteLine("addLineItemAction.Quantity: {0}", addLineItemAction.Quantity);

            Console.WriteLine("cart.Id: {0}", cart.Id);
            Console.WriteLine("cart.Version: {0}", cart.Version);

            cartResponse = await client.Carts().UpdateCartAsync(cart, addLineItemAction);

            if (cartResponse.Success)
            {
                cart = cartResponse.Result;
                Console.WriteLine("Added line item to cart with ID {0}.", cart.Id);
                Console.WriteLine("Line item product ID: {0}.", cart.LineItems[0].ProductId);
                Console.WriteLine("Line item quantity: {0}.", cart.LineItems[0].Quantity);
            }
            else
            {
                Helper.WriteError<Cart>(cartResponse);
            }

            // Use another update action to change the quantity of the line item in the cart to 2.
            if (cart.LineItems.Count > 0)
            {
                ChangeLineItemQuantityAction changeLineItemQuantityAction = new ChangeLineItemQuantityAction(cart.LineItems[0].Id, 2);
                cartResponse = await client.Carts().UpdateCartAsync(cart, changeLineItemQuantityAction);

                if (cartResponse.Success)
                {
                    cart = cartResponse.Result;
                    Console.WriteLine("Changed line item quanity in cart with ID {0}.", cart.Id);
                    Console.WriteLine("Line item product ID: {0}.", cart.LineItems[0].ProductId);
                    Console.WriteLine("Line item quantity: {0}.", cart.LineItems[0].Quantity);
                }
                else
                {
                    Helper.WriteError<Cart>(cartResponse);
                }
            }

            // Finally, remove the line item from the cart.
            if (cart.LineItems.Count > 0)
            {
                RemoveLineItemAction removeLineItemAction = new RemoveLineItemAction(cart.LineItems[0].Id);
                cartResponse = await client.Carts().UpdateCartAsync(cart, removeLineItemAction);

                if (cartResponse.Success)
                {
                    cart = cartResponse.Result;
                    Console.WriteLine("Removed line item from cart with ID {0}.", cart.Id);
                    Console.WriteLine("Line items in cart: {0}.", cart.LineItems.Count);
                }
                else
                {
                    Helper.WriteError<Cart>(cartResponse);
                }
            }

            /*  DELETE A CART
             *  ===================================================================================
             *  Delete API requests return a generic response, but some return the object that was 
             *  deleted. The Carts delete request returns the full representation.
             */
            cartResponse = await client.Carts().DeleteCartAsync(cart);

            if (cartResponse.Success)
            {
                cart = cartResponse.Result;
                Console.WriteLine("Deleted cart with ID {0}.", cart.Id);
            }
            else
            {
                Helper.WriteError<Cart>(cartResponse);
            }
        }
    }
}
