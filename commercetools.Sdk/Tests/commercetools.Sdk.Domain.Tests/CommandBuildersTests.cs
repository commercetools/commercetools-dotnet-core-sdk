using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Customers.UpdateActions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.CommandBuilders;
using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class CommandBuildersTests
    {
        [Fact]
        public void IsGetByIdCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers().GetById("customerId").Build();
            Assert.IsType<GetByIdCommand<Customer>>(command);
        }

        [Fact]
        public void IsGetByKeyCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers().GetByKey("customerKey").Build();
            Assert.IsType<GetByKeyCommand<Customer>>(command);
        }
        
        [Fact]
        public void IsGetByCustomerIdCommand()
        {
            var builder = new CommandBuilder();
            var customerId = "customerId";
            var command = builder.Carts().GetByCustomerId(customerId).Build();
            Assert.IsType<GetCartByCustomerIdCommand>(command);
            Assert.Equal(customerId, command.ParameterValue);
        }
        
        [Fact]
        public void IsGetByCustomerIdInStoreCommand()
        {
            var builder = new CommandBuilder();
            var customerId = "customerId";
            var command = builder.Carts().GetByCustomerId(customerId).InStore("storeKey").Build();
            
            Assert.NotNull(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var getByCustomerIdCommand = innerCommand as GetCartByCustomerIdCommand;
            Assert.NotNull(getByCustomerIdCommand);
            Assert.Equal("customerId", getByCustomerIdCommand.ParameterValue);
        }

        [Fact]
        public void IsDeleteByIdCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers().DeleteById("customerId", 2).Build();
            Assert.IsType<DeleteByIdCommand<Customer>>(command);
            Assert.Equal("customerId", command.ParameterValue);
            Assert.Equal(2, command.Version);
        }

        [Fact]
        public void IsDeleteByKeyCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers().DeleteByKey("customerKey", 2).Build();
            Assert.IsType<DeleteByKeyCommand<Customer>>(command);
            Assert.Equal("customerKey", command.ParameterValue);
            Assert.Equal(2, command.Version);
        }

        [Fact]
        public void IsGetByIdInStoreCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers().GetById("customerId").InStore("storeKey").Build();
            Assert.NotNull(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var getByIdCommand = innerCommand as GetByIdCommand<Customer>;
            Assert.NotNull(getByIdCommand);
            Assert.Equal("customerId", getByIdCommand.ParameterValue);
        }
        
        [Fact]
        public void IsGetByIdInStoreCommand2()
        {
            var builder = new CommandBuilder();
            var command = builder
                .Customers()
                .InStore("storeKey")
                .GetById("customerId")
                .Build();
                            
            Assert.NotNull(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var getByIdCommand = innerCommand as GetByIdCommand<Customer>;
            Assert.NotNull(getByIdCommand);
            Assert.Equal("customerId", getByIdCommand.ParameterValue);
        }
        
        [Fact]
        public void IsGetByKeyInStoreCommand()
        {
            var builder = new CommandBuilder();
            var command = builder
                .Customers()
                .InStore("storeKey")
                .GetByKey("customerKey")
                .Build();
                            
            Assert.NotNull(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var getByKeyCommand = innerCommand as GetByKeyCommand<Customer>;
            Assert.NotNull(getByKeyCommand);
            Assert.Equal("customerKey", getByKeyCommand.ParameterValue);
        }

        [Fact]
        public void IsDeleteByIdInStoreCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers()
                .DeleteById("customerId", 2).InStore("storeKey").Build();
            Assert.NotNull(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var deleteByIdCommand = innerCommand as DeleteByIdCommand<Customer>;
            Assert.NotNull(deleteByIdCommand);
            Assert.Equal("customerId", deleteByIdCommand.ParameterValue);
        }

        [Fact]
        public void IsQueryCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers()
                .Query()
                .Where(c => c.Key == "cusKey")
                .Expand(c => c.CustomerGroup)
                .Sort(c => c.LastName, SortDirection.Descending)
                .SetLimit(3)
                .SetOffset(5)
                .Build();
            Assert.IsType<QueryCommand<Customer>>(command);
            var queryParams = command.QueryParameters as QueryCommandParameters;
            Assert.NotNull(queryParams);
            Assert.Single(queryParams.Where);
            Assert.Equal("key = \"cusKey\"", queryParams.Where[0]);
            Assert.Single(queryParams.Expand);
            Assert.Equal("customerGroup", queryParams.Expand[0]);
            Assert.Single(queryParams.Sort);
            Assert.Equal("lastName desc", queryParams.Sort[0]);
            Assert.Equal(3, queryParams.Limit);
            Assert.Equal(5, queryParams.Offset);
        }

        [Fact]
        public void IsQueryCommandInStore()
        {
            var builder = new CommandBuilder();

            var command1 = builder.Customers()
                .Query().Where(c => c.Key == "cusKey")
                .InStore("storeKey").Build();
            
            var command2 = builder.Customers()
                .Query().InStore("storeKey")
                .Build().Where(c => c.Key == "cusKey");

            Assert.IsType<InStoreCommand<PagedQueryResult<Customer>>>(command1);
            Assert.IsType<InStoreCommand<PagedQueryResult<Customer>>>(command2);

            var innerCommand1 = command1.InnerCommand as QueryCommand<Customer>;
            var innerCommand2 = command2.InnerCommand as QueryCommand<Customer>;

            Assert.NotNull(innerCommand1);
            Assert.NotNull(innerCommand2);

            var queryParams1 = innerCommand1.QueryParameters as QueryCommandParameters;
            var queryParams2 = innerCommand2.QueryParameters as QueryCommandParameters;

            Assert.NotNull(queryParams1);
            Assert.NotNull(queryParams2);

            Assert.Single(queryParams1.Where);
            Assert.Single(queryParams2.Where);

            Assert.Equal("key = \"cusKey\"", queryParams1.Where[0]);
            Assert.Equal("key = \"cusKey\"", queryParams2.Where[0]);
        }

        [Fact]
        public void IsUpdateByIdCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers()
                .UpdateById("customerId", 2)
                .AddAction(new SetKeyUpdateAction()
                {
                    Key = "newKey"
                })
                .Build();

            Assert.IsType<UpdateByIdCommand<Customer>>(command);
            Assert.Equal("customerId", command.ParameterValue);
            Assert.Equal(2, command.Version);
            Assert.Single(command.UpdateActions);
            Assert.IsType<SetKeyUpdateAction>(command.UpdateActions[0]);
        }

        [Fact]
        public void IsUpdateByIdInStoreCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers()
                .UpdateById("customerId", 2)
                .AddAction(new SetFirstNameUpdateAction {FirstName = "FirstName"})
                .AddAction(() => new SetLastNameUpdateAction {LastName = "LastName"})
                .InStore("storeKey")
                .Build();

            Assert.IsType<InStoreCommand<Customer>>(command);
            Assert.Equal("storeKey", command.StoreKey);

            var innerCommand = command.InnerCommand as UpdateByIdCommand<Customer>;
            Assert.NotNull(innerCommand);
            Assert.Equal("customerId", innerCommand.ParameterValue);
            Assert.Equal(2, innerCommand.UpdateActions.Count);
            var action1 = innerCommand.UpdateActions[0] as SetFirstNameUpdateAction;
            Assert.NotNull(action1);
            Assert.Equal("FirstName", action1.FirstName);
        }

        [Fact]
        public void IsUpdateByKeyCommand()
        {
            var builder = new CommandBuilder();
            var command = builder.Customers()
                .UpdateByKey("customerKey", 2)
                .AddAction(new SetKeyUpdateAction()
                {
                    Key = "newKey"
                })
                .Build();

            Assert.IsType<UpdateByKeyCommand<Customer>>(command);
            Assert.Equal("customerKey", command.ParameterValue);
            Assert.Equal(2, command.Version);
            Assert.Single(command.UpdateActions);
            Assert.IsType<SetKeyUpdateAction>(command.UpdateActions[0]);
        }

        [Fact]
        public void IsCreateCommand()
        {
            var builder = new CommandBuilder();
            var categoryDraft = new CategoryDraft
            {
                Name = new LocalizedString {{"en", "CatName"}},
                Slug = new LocalizedString {{"en", "CatSlug"}}
            };
            var command = builder.Categories().Create(categoryDraft).Build();
            Assert.IsType<CreateCommand<Category>>(command);
            Assert.NotNull(command.Entity);
        }

        [Fact]
        public void IsSignUpCommand()
        {
            var builder = new CommandBuilder();
            var customerDraft = new CustomerDraft
            {
                Email = "customer@email.com",
                Password = "pass"
            };
            var command = builder.Customers().SignUp(customerDraft).Build();
            Assert.IsType<SignUpCustomerCommand>(command);
            Assert.NotNull(command.Entity);
        }

        [Fact]
        public void IsSignUpInStoreCommand()
        {
            var builder = new CommandBuilder();
            var customerDraft = new CustomerDraft
            {
                Email = "customer@email.com",
                Password = "pass"
            };
            var command = builder.Customers()
                .SignUp(customerDraft)
                .InStore("storeKey").Build();

            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<SignInResult<Customer>>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var signUpCustomerCommand = innerCommand as SignUpCustomerCommand;
            Assert.NotNull(signUpCustomerCommand);
            Assert.NotNull(signUpCustomerCommand.Entity);
            var customerDraftEntity = signUpCustomerCommand.Entity as CustomerDraft;
            Assert.NotNull(customerDraftEntity);
            Assert.Equal(customerDraft.Email, customerDraftEntity.Email);
            Assert.Equal(customerDraft.Password, customerDraftEntity.Password);
        }

        [Fact]
        public void IsLoginCommand()
        {
            var builder = new CommandBuilder();
            var email = "customer@email.com";
            var password = "pass";
            var command = builder.Customers().Login(email, password).Build();
            Assert.IsType<LoginCustomerCommand>(command);
            Assert.Equal(email, command.Email);
            Assert.Equal(password, command.Password);
        }

        [Fact]
        public void IsLoginInStoreCommand()
        {
            var builder = new CommandBuilder();
            var email = "customer@email.com";
            var password = "pass";

            var command = builder.Customers()
                .Login(email, password)
                .InStore("storeKey").Build();

            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<SignInResult<Customer>>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var loginCustomerCommand = innerCommand as LoginCustomerCommand;
            Assert.NotNull(loginCustomerCommand);
            Assert.Equal(email, loginCustomerCommand.Email);
            Assert.Equal(password, loginCustomerCommand.Password);
        }

        [Fact]
        public void IsChangePasswordCommand()
        {
            var builder = new CommandBuilder();
            var currentPassword = "current";
            var newPassword = "new";
            var command = builder.Customers()
                .ChangePassword("customerId", 2, currentPassword, newPassword)
                .Build();
            Assert.IsType<ChangeCustomerPasswordCommand>(command);
            Assert.Equal("customerId", command.Id);
            Assert.Equal(2, command.Version);
            Assert.Equal(currentPassword, command.CurrentPassword);
            Assert.Equal(newPassword, command.NewPassword);
        }
        
        [Fact]
        public void IsCreateTokenForPasswordResettingCommand()
        {
            var builder = new CommandBuilder();
            var email = "customer@email.com";
            var command = builder.Customers()
                .CreateTokenForPasswordResetting(email)
                .Build();
            Assert.IsType<CreateTokenForCustomerPasswordResetCommand>(command);
            Assert.Equal(email, command.Email);
        }
        
        [Fact]
        public void IsCreateTokenForPasswordResettingInStoreCommand()
        {
            var builder = new CommandBuilder();
            var email = "customer@email.com";
            var command = builder.Customers()
                .CreateTokenForPasswordResetting(email)
                .InStore("storeKey")
                .Build();
            
            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<Token<Customer>>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var createTokenCommand = innerCommand as CreateTokenForCustomerPasswordResetCommand;
            Assert.NotNull(createTokenCommand);
            Assert.Equal(email, createTokenCommand.Email);
        }
        
        [Fact]
        public void IsCreateTokenForVerifyingEmailCommand()
        {
            var builder = new CommandBuilder();
            var id = "customerId";
            var timeToLiveMinutes = 10;
            var version = 2;
            var command = builder.Customers()
                .CreateTokenForEmailVerification(id, timeToLiveMinutes, version)
                .Build();
            Assert.IsType<CreateTokenForCustomerEmailVerificationCommand>(command);
            Assert.Equal(id, command.Id);
            Assert.Equal(timeToLiveMinutes, command.TimeToLiveMinutes);
            Assert.Equal(version, command.Version);
        }
        
        [Fact]
        public void IsCreateTokenForVerifyingEmailInStoreCommand()
        {
            var builder = new CommandBuilder();
            var id = "customerId";
            var timeToLiveMinutes = 10;
            var version = 2;
            var command = builder.Customers()
                .CreateTokenForEmailVerification(id, timeToLiveMinutes, version)
                .InStore("storeKey")
                .Build();
            
            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<Token<Customer>>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var createTokenCommand = innerCommand as CreateTokenForCustomerEmailVerificationCommand;
            Assert.NotNull(createTokenCommand);
            Assert.Equal(id, createTokenCommand.Id);
            Assert.Equal(version, createTokenCommand.Version);
            Assert.Equal(timeToLiveMinutes, createTokenCommand.TimeToLiveMinutes);
        }

        [Fact]
        public void IsGetByPasswordTokenCommand()
        {
            var builder = new CommandBuilder();
            var passwordToken = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            
            var command = builder.Customers()
                .GetByPasswordToken(passwordToken)
                .Build();
            Assert.IsType<GetCustomerByPasswordTokenCommand>(command);
            Assert.Equal(passwordToken, command.ParameterValue);
        }
        
        [Fact]
        public void IsGetByPasswordTokenCommand2()
        {
            var builder = new CommandBuilder();
            var email = "customer@email.com";
            var command = builder.Customers()
                .CreateTokenForPasswordResetting(email)
                .GetByPasswordToken()
                .Build();
            Assert.IsType<GetCustomerByPasswordTokenCommand>(command);
        }
        
        [Fact]
        public void IsGetByPasswordTokenInStoreCommand()
        {
            var builder = new CommandBuilder();
            var passwordToken = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            
            var command = builder.Customers()
                .GetByPasswordToken(passwordToken)
                .InStore("storeKey")
                .Build();
            
            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<Customer>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var getByCustomerTokenCommand = innerCommand as GetCustomerByPasswordTokenCommand;
            Assert.NotNull(getByCustomerTokenCommand);
            Assert.Equal(passwordToken, getByCustomerTokenCommand.ParameterValue);
        }
        
        [Fact]
        public void IsGetByEmailTokenCommand()
        {
            var builder = new CommandBuilder();
            var emailToken = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            
            var command = builder.Customers()
                .GetByEmailToken(emailToken)
                .Build();
            Assert.IsType<GetCustomerByEmailTokenCommand>(command);
            Assert.Equal(emailToken, command.ParameterValue);
        }
        
        [Fact]
        public void IsGetByEmailTokenInStoreCommand()
        {
            var builder = new CommandBuilder();
            var emailToken = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            
            var command = builder.Customers()
                .GetByEmailToken(emailToken)
                .InStore("storeKey")
                .Build();
           
            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<Customer>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var getByCustomerTokenCommand = innerCommand as GetCustomerByEmailTokenCommand;
            Assert.NotNull(getByCustomerTokenCommand);
            Assert.Equal(emailToken, getByCustomerTokenCommand.ParameterValue);
        }
        
        
        [Fact]
        public void IsResetCustomerPasswordCommand()
        {
            var builder = new CommandBuilder();
            var tokenValue = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            var newPassword = "newPassword";
            
            
            var command = builder.Customers()
                .ResetPassword(tokenValue, newPassword)
                .Build();
            Assert.IsType<ResetCustomerPasswordCommand>(command);
            Assert.Equal(tokenValue, command.TokenValue);
            Assert.Equal(newPassword, command.NewPassword);
        }
        
        [Fact]
        public void IsResetCustomerPasswordInStoreCommand()
        {
            var builder = new CommandBuilder();
            var tokenValue = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            var newPassword = "newPassword";
            
            
            var command = builder.Customers()
                .ResetPassword(tokenValue, newPassword)
                .InStore("storeKey")
                .Build();
            
            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<Customer>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var resetCustomerPasswordCommand = innerCommand as ResetCustomerPasswordCommand;
            Assert.NotNull(resetCustomerPasswordCommand);
            Assert.Equal(tokenValue, resetCustomerPasswordCommand.TokenValue);
            Assert.Equal(newPassword, resetCustomerPasswordCommand.NewPassword);
        }
        
        [Fact]
        public void IsVerifyCustomerEmailCommand()
        {
            var builder = new CommandBuilder();
            var tokenValue = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            
            var command = builder.Customers()
                .VerifyEmail(tokenValue)
                .Build();
            Assert.IsType<VerifyCustomerEmailCommand>(command);
            Assert.Equal(tokenValue, command.TokenValue);
        }
        
        [Fact]
        public void IsVerifyCustomerEmailInStoreCommand()
        {
            var builder = new CommandBuilder();
            var tokenValue = "3spe4Xdtw_8QItIghMjkO4XAs9EgO40V9aN6c1EE";
            
            var command = builder.Customers()
                .VerifyEmail(tokenValue)
                .InStore("storeKey")
                .Build();
            
            Assert.NotNull(command);
            Assert.IsType<InStoreCommand<Customer>>(command);
            Assert.Equal("storeKey", command.StoreKey);
            var innerCommand = command.InnerCommand;
            var verifyCustomerEmailCommand = innerCommand as VerifyCustomerEmailCommand;
            Assert.NotNull(verifyCustomerEmailCommand);
            Assert.Equal(tokenValue, verifyCustomerEmailCommand.TokenValue);
        }
    }
}