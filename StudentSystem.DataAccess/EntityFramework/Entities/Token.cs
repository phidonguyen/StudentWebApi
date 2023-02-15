namespace StudentSystem.DataAccess.EntityFramework.Entities
{
    public class Token
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpired { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpired { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual User User { get; set; }
    }
}
