namespace StudentSystem.DataAccess.EntityFramework.Entities
{
    public partial class User
    {
        public User()
        {
            HistoryLogins = new HashSet<HistoryLogin>();
            Tokens = new HashSet<Token>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<HistoryLogin> HistoryLogins { get; set; }
    }
}
