using System;
using System.Threading;
using System.Xml;

namespace SiriSX.Analyzer
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            //MessageRetrievalInterface mailbox = new AmazonSQSMessageRetrievalInterface();
            MessageRetrievalInterface mailbox = new MaildirMessageRetrievalInterface();
            for (;;)
            {
                AlertMessage m;
                if ((m = mailbox.GetNextMessage()) != null)
                {
                    Console.WriteLine(m.Subject);
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
