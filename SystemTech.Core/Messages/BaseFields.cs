using System.ComponentModel.DataAnnotations.Schema;

namespace SystemTech.Core.Messages
{
    public class QueryFields
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        
        public int Offset => Page * PageSize;
        
        public int Limit => PageSize;
        public string OrderBy { get; set; }
        public QueryFields() {}
    }

    public class CommandFields
    {
        public CommandFields()
        {
            BusinessKey = $"BusinessKey_{Guid.NewGuid().ToString()}";
        }
        [NotMapped]
        public string BusinessKey { get; set; }
        [NotMapped]
        public Exception Exception { get; set; }
        public string UpdatedBy { get; set; }
    }

    public interface IPermanentDeleteField
    {
        public bool PermanentDelete { get; set; }
    }
}