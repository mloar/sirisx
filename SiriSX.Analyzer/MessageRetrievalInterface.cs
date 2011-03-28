namespace SiriSX.Analyzer
{
    struct Message
    {
        string Subject;
        string Body;
        DateTime Date;
    }

    interface MessageRetrievalInterface
    {
        public Message GetNextMessage();
    }
}
