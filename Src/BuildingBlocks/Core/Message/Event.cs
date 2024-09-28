using MediatR;

namespace Core.Message
{
    public class Event : Message, INotification//MARCACAO
    {
        public DateTime Timestamp {  get; set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
