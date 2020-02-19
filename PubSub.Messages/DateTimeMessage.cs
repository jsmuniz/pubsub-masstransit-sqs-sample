using System;

namespace PubSub.Messages
{
    public class DateTimeMessage
    {
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
