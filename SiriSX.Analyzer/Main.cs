using System;
using System.Threading;
using System.Xml;

namespace SiriSX.Analyzer
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            MessageRetrievalInterface mailbox = new AmazonSQSMessageRetrievalInterface();
            for (;;)
            {
                if (mailbox.GetNextMessage() != null)
                {
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }

            return 0;
        }
    }
}
