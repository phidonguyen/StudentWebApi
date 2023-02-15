namespace StudentSystem.DataAccess.EntityFramework.Entities
{
    public class HistoryLogin
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public bool IsSuccess { get; set; }
        public string TokenId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
