using System.ComponentModel.DataAnnotations.Schema;

namespace SystemTech.Core.Messages
{
    public interface IMessage
    {
        public string BusinessKey { get; set; }
        public Exception Exception { get; set; }
    }

    public class MessageModel : IMessage
    {
        public MessageModel()
        {
            BusinessKey = $"BusinessKey_{Guid.NewGuid().ToString()}";
        }
        [NotMapped]
        public string BusinessKey { get; set; }
        [NotMapped]
        public Exception Exception { get; set; }
    }
}