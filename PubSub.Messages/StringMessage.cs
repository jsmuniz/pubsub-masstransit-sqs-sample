namespace PubSub.Messages
{
    public class StringMessage
    {
        public string Message { get; set; }

        public StringMessage(string message)
        {
            Message = message;
        }
    }
}
