using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.Domain.OrderEdits.StagedActions;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Orders.UpdateActions;
using static commercetools.Sdk.IntegrationTests.GenericFixture;

namespace commercetools.Sdk.IntegrationTests.OrderEdits
{
    public class OrderEditsFixture
    {
        #region DraftBuilds

        public static OrderEditDraft DefaultOrderEditDraft(OrderEditDraft orderEditDraft,
            Order order)
        {
            var randomInt = TestingUtility.RandomInt();
            orderEditDraft.Key = $"OrderEdit_{randomInt}";
            orderEditDraft.Resource = order.ToReference();
            orderEditDraft.StagedActions = new List<IStagedOrderUpdateAction>();
            return orderEditDraft;
        }

        public static OrderEditDraft DefaultOrderEditDraftWithStagedAction(OrderEditDraft draft,
            Order order)
        {
            var orderEditDraft = DefaultOrderEditDraft(draft, order);
            orderEditDraft.StagedActions.Add(
                new SetCustomerEmailUpdateAction
                {
                    Email = "john.doe@commercetools.de"
                });
            return orderEditDraft;
        }

        #endregion

        #region WithOrderEdit

        public static async Task WithOrderEdit(IClient client, Func<OrderEditDraft, OrderEditDraft> draftAction,
            Action<OrderEdit> func)
        {
            await With(client, new OrderEditDraft(), draftAction, func);
        }

        public static async Task WithOrderEdit(IClient client, Func<OrderEditDraft, OrderEditDraft> draftAction,
            Func<OrderEdit, Task> func)
        {
            await WithAsync(client, new OrderEditDraft(), draftAction, func);
        }

        #endregion
        
        #region WithUpdateableOrderEdits
        
        public static async Task WithUpdateableOrderEdit(IClient client, Func<OrderEditDraft, OrderEditDraft> draftAction,
            Func<OrderEdit, OrderEdit> func)
        {
            await WithUpdateable(client, new OrderEditDraft(), draftAction, func);
        }

        public static async Task WithUpdateableOrderEdit(IClient client, Func<OrderEditDraft, OrderEditDraft> draftAction,
            Func<OrderEdit, Task<OrderEdit>> func)
        {
            await WithUpdateableAsync(client, new OrderEditDraft(), draftAction, func);
        }
        
        #endregion
    }
}