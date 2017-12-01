using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Carts.UpdateActions
{
    /// <summary>
    /// Adds all LineItems of a ShoppingList to the cart.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-carts.html#add-shopping-list"/>
    public class AddShoppingListAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// ResourceIdentifier of a ShoppingList
        /// </summary>
        [JsonProperty(PropertyName = "shoppingList")]
        public ResourceIdentifier ShoppingList { get; set; }

        /// <summary>
        /// ResourceIdentifier of a Channel 
        /// </summary>
        /// <remarks>
        /// If present a Reference to the channel will be set for all LineItems that are added to the cart. The channel must have the InventorySupply role.
        /// </remarks>
        [JsonProperty(PropertyName = "supplyChannel")]
        public ResourceIdentifier SupplyChannel { get; set; }

        /// <summary>
        /// ResourceIdentifier of a Channel 
        /// </summary>
        /// <remarks>
        /// If present a Reference to the channel will be set for all LineItems that are added to the cart. The channel must have the role ProductDistribution.
        /// </remarks>
        [JsonProperty(PropertyName = "distributionChannel")]
        public ResourceIdentifier DistributionChannel { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shoppingList">ResourceIdentifier of a ShoppingList</param>
        public AddShoppingListAction(ResourceIdentifier shoppingList)
        {
            this.Action = "addShoppingList";
            this.ShoppingList = shoppingList;
        }

        #endregion
    }
}
