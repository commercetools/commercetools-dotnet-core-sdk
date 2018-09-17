using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByIdRequestMessageBuilder : IRequestMessageBuilder
    {
        public static Type CommandType = typeof(UpdateByIdCommand);

        private UpdateByIdCommand command;

        public UpdateByIdRequestMessageBuilder(UpdateByIdCommand command)
        {
            this.command = command;
        }

        public HttpMethod HttpMethod => HttpMethod.Post;

        public string RequestUriEnd => $"/{this.command.Guid}";

        public object RequestBody
        {
            get
            {
                return new
                {
                    Version = this.command.Version,
                    Actions = this.command.UpdateActions
                };
            }
        }
    }
}
