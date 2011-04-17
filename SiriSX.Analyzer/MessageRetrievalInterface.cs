using System;

namespace SiriSX.Analyzer
{
    class AlertMessage
    {
        public string Subject;
        public string Body;
        public DateTime Date;
    }

    interface MessageRetrievalInterface
    {
        AlertMessage GetNextMessage();
    }
}
