using System.ComponentModel.DataAnnotations.Schema;

namespace SystemTech.Core.Messages
{
    public interface IResults
    {
        public Exception Error { get; set; }
    }

    public class ResourceResults
    {
        public ResourceResults()
        {
        }
        [NotMapped]
        public string BusinessKey { get; set; }
        [NotMapped]
        public Exception Exception { get; set; }
        public Exception Error { get; set; }
    }
}