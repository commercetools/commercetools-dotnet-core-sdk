using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using commercetools.Carts;
using commercetools.Common;
using commercetools.Common.UpdateActions;
using commercetools.Orders;
using commercetools.Orders.UpdateActions;
using commercetools.ProductProjections;

using Newtonsoft.Json.Linq;

namespace commercetools.Examples
{
    /// <summary>
    /// Code snippets for querying, creating, updating, and deleting Orders.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html"/>
    public class OrderExamples
    {
        /// <summary>
        /// Run Order example code.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="project">Project</param>
        public async static Task Run(Client client, Project.Project project)
        {
            string currency = project.Currencies[0];
            string country = project.Countries[0];
            string language = project.Languages[0];

            /*  CREATE ORDER
             *  ===================================================================================
             *  An order is created from an existing cart.  To create an order from a cart, the 
             *  cart cannot be empty and it must have a shipping address set.
             */

            // Retrieve 2 products to add to a cart.
            Response<ProductProjectionQueryResult> productProjectionQueryResponse = await client.ProductProjections().QueryProductProjectionsAsync(limit: 2);

            List<ProductProjection> products = new List<ProductProjection>();
            List<LineItemDraft> lineItemDrafts = new List<LineItemDraft>();

            if (productProjectionQueryResponse.Success)
            {
                ProductProjectionQueryResult productProjectionQueryResult = productProjectionQueryResponse.Result;
                Console.WriteLine("Retrieved {0} products.", productProjectionQueryResult.Results.Count);

                foreach (ProductProjection product in productProjectionQueryResult.Results)
                {
                    LineItemDraft lineItemDraft = new LineItemDraft(product.Id, product.MasterVariant.Id)
                    {
                        Quantity = 1
                    };

                    lineItemDrafts.Add(lineItemDraft);
                }
            }
            else
            {
                Helper.WriteError<ProductProjectionQueryResult>(productProjectionQueryResponse);
            }

            // Create a shipping address.
            Address shippingAddress = new Address();
            shippingAddress.FirstName = "John";
            shippingAddress.LastName = "Doe";
            shippingAddress.StreetNumber = "123";
            shippingAddress.StreetName = "Main St.";
            shippingAddress.PostalCode = "11111";
            shippingAddress.City = "City";
            shippingAddress.Country = country;

            // Create the cart.
            Cart cart = null;

            if (lineItemDrafts.Count > 0)
            {
                CartDraft cartDraft = new CartDraft(currency)
                {
                    ShippingAddress = shippingAddress,
                    LineItems = lineItemDrafts
                };

                Response<Cart> cartResponse = await client.Carts().CreateCartAsync(cartDraft);

                if (cartResponse.Success)
                {
                    cart = cartResponse.Result;
                    Console.WriteLine("Created new cart with ID {0}.", cart.Id);
                }
                else
                {
                    Helper.WriteError<Cart>(cartResponse);
                }
            }

            // Create the order from the cart.
            Response<Order> orderResponse = null;
            Order order = null;

            if (cart != null)
            {
                OrderFromCartDraft orderFromCartDraft = new OrderFromCartDraft(cart);
                orderResponse = await client.Orders().CreateOrderFromCartAsync(orderFromCartDraft);

                if (orderResponse.Success)
                {
                    order = orderResponse.Result;
                    Console.WriteLine("Created new order with ID {0}.", order.Id);

                    foreach (LineItem lineItem in order.LineItems)
                    {
                        Console.WriteLine("Order has a line iten named '{0}' with an ID of {1}.", lineItem.Name[language], lineItem.Id);
                    }
                }
                else
                {
                    Helper.WriteError<Order>(orderResponse);
                }
            }

            /*  GET ORDERS
             *  ===================================================================================
             */

            // Get 3 most recent orders.
            Response<OrderQueryResult> orderQueryResponse = await client.Orders().QueryOrdersAsync(sort: "createdAt desc", limit: 3);
            List<Order> orders = new List<Order>();

            if (orderQueryResponse.Success)
            {
                OrderQueryResult orderQueryResult = orderQueryResponse.Result;
                orders = orderQueryResult.Results;

                Console.WriteLine("Retrieved {0} orders.", orders.Count);
            }
            else
            {
                Helper.WriteError<OrderQueryResult>(orderQueryResponse);
            }

            // Get a single order by its ID.
            if (orders.Count > 0)
            {
                string orderId = orders[0].Id;
                orderResponse = await client.Orders().GetOrderByIdAsync(orderId);

                if (orderResponse.Success)
                {
                    order = orderResponse.Result;
                    Console.WriteLine("Retrieved order with ID {0}.", order.Id);
                }
                else
                {
                    Helper.WriteError<Order>(orderResponse);
                }
            }

            /*  UPDATE AN ORDER
             *  ===================================================================================
             *  Each change is made using its own update action object which maps to an update 
             *  action call in the API. The list of update action objects are sent to the API using
             *  a single request. If there is an update action in the API that has not yet been 
             *  implemented in the SDK, you can use the GenericAction class to make any request you 
             *  want (as long as it is a valid update action supported by the API).
             */
            if (order != null)
            {
                ChangeOrderStateAction changeOrderStateAction = new ChangeOrderStateAction(OrderState.Confirmed);

                // Here is how you would make the setDescription request using a GenericAction object.
                GenericAction setOrderNumberAction = new GenericAction("setOrderNumber");
                setOrderNumberAction["orderNumber"] = "ABC123";

                List<UpdateAction> actions = new List<UpdateAction>() { changeOrderStateAction, setOrderNumberAction };

                orderResponse = await client.Orders().UpdateOrderAsync(order, actions);

                if (orderResponse.Success)
                {
                    order = orderResponse.Result;
                    Console.WriteLine("Updated order with ID {0}.", order.Id);
                    Console.WriteLine("Order state: {0}", order.OrderState);
                    Console.WriteLine("Order number: {0}", order.OrderNumber);
                }
                else
                {
                    Helper.WriteError<Order>(orderResponse);
                }
            }

            /*  DELETE AN ORDER
             *  ===================================================================================
             *  Delete API requests return a generic response, but some return the object that was 
             *  deleted. The Orders delete request returns a generic response.
             */
            if (order != null)
            {
                Response<JObject> deleteResponse = await client.Orders().DeleteOrderAsync(order);

                if (deleteResponse.Success)
                {
                    Console.WriteLine("Deleted order with ID {0}.", order.Id);
                }
                else
                {
                    Helper.WriteError<JObject>(deleteResponse);
                }
            }
        }
    }
}
