using Amazon.SQS;
using Amazon.SQS.Model;

namespace SiriSX.Analyzer
{
    class AmazonSQSMessageRetrievalInterface : MessageRetrievalInterface
    {
        AmazonSQS sqs;
        string queueUrl;

        public AmazonSQSMessageRetrievalInterface()
        {
            string awsAccessKeyId, awsSecretAccessKey, queueUrl;
            try
            {
                System.Configuration.AppSettingsReader settings = new System.Configuration.AppSettingsReader();
                awsAccessKeyId = (string)settings.GetValue("AwsAccessKeyId", typeof(string));
                awsSecretAccessKey = (string)settings.GetValue("AwsSecretAccessKey", typeof(string));
                queueUrl = (string)settings.GetValue("QueueUrl", typeof(string));
            }
            catch (InvalidOperationException)
            {
                throw new ApplicationException(
                    "Could not read AwsAccessKeyId or AwsSecretAccessKey from the configuration."
                    );
            }

            sqs = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey);
        }

        public Message GetNextMessage()
        {
            //Receiving a message
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = queueUrl;
            ReceiveMessageResponse receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);
            if (receiveMessageResponse.IsSetReceiveMessageResult())
            {
                Console.WriteLine("Printing received message.\n");
                ReceiveMessageResult receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                foreach (Message message in receiveMessageResult.Message)
                {
                    Message m;
                    Console.WriteLine("  Message");
                    if (message.IsSetMessageId())
                    {
                        Console.WriteLine("    MessageId: {0}", message.MessageId);
                    }
                    if (message.IsSetReceiptHandle())
                    {
                        Console.WriteLine("    ReceiptHandle: {0}", message.ReceiptHandle);
                    }
                    if (message.IsSetMD5OfBody())
                    {
                        Console.WriteLine("    MD5OfBody: {0}", message.MD5OfBody);
                    }
                    if (message.IsSetBody())
                    {
                        m.Body = message.Body;
                        Console.WriteLine("    Body: {0}", message.Body);
                    }
                    foreach (Amazon.SQS.Model.Attribute attribute in message.Attribute)
                    {
                        Console.WriteLine("  Attribute");
                        if (attribute.IsSetName() && attribute.Name == "Subject")
                        {
                            if (attribute.IsSetValue())
                            {
                                m.Subject = attribute.Value;
                            }
                        }
                        if (attribute.IsSetValue())
                        {
                            Console.WriteLine("    Value: {0}", attribute.Value);
                        }
                    }

                    // Deleting a message
                    Console.WriteLine("Deleting the message.\n");
                    DeleteMessageRequest deleteRequest = new DeleteMessageRequest();
                    deleteRequest.QueueUrl = queueUrl;
                    deleteRequest.ReceiptHandle = message.ReceiptHandle;
                    sqs.DeleteMessage(deleteRequest);

                    return m;
                }
            }
            else
            {
                return null;
            }
        }
