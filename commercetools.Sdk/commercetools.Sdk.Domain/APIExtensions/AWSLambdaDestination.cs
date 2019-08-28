namespace commercetools.Sdk.Domain.APIExtensions
{
    [TypeMarker("AWSLambda")]
    public class AwsLambdaDestination : Destination
    {
        /// <summary>
        /// The ARN of the Lambda function
        /// </summary>
        public string Arn { get; set; }

        public string AccessKey { get; set; }

        public string AccessSecret { get; set; }
    }
}
