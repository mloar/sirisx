using System;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace sqsmda
{
	class MainClass
	{
		private static void FindOrCreateSQSQueue(AmazonSQS sqs)
		{
			//Confirming the queue exists
			ListQueuesRequest listQueuesRequest = new ListQueuesRequest();
			ListQueuesResponse listQueuesResponse = sqs.ListQueues(listQueuesRequest);
			
			Console.WriteLine("Printing list of Amazon SQS queues.\n");
			if (listQueuesResponse.IsSetListQueuesResult())
			{
			    ListQueuesResult listQueuesResult = listQueuesResponse.ListQueuesResult;
			    foreach (String queueUrl in listQueuesResult.QueueUrl)
			    {
					Console.WriteLine("  QueueUrl: {0}", queueUrl);
			    }
			}
			Console.WriteLine();
		}
		
		public static int Main(string[] args)
		{
			string awsAccessKeyId, awsSecretAccessKey;
			try
			{
				System.Configuration.AppSettingsReader settings = new System.Configuration.AppSettingsReader();
				awsAccessKeyId = (string)settings.GetValue("AwsAccessKeyId", typeof(string));
				awsSecretAccessKey = (string)settings.GetValue("AwsSecretAccessKey", typeof(string));
			}
			catch (InvalidOperationException)
			{
				Console.Error.WriteLine("Could not read AwsAccessKeyId or AwsSecretAccessKey from the configuration.");
				return 1;
			}
			
			AmazonSQS sqs = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey);
			FindOrCreateSQSQueue(sqs);
			
			Console.WriteLine ("Hello World!");
			return 0;
		}
	}
}
