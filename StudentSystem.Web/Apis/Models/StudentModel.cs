namespace StudentSystem.Web.Apis.Models
{
    public class StudentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set;}
        public int? IdentityNumber { get; set;}
        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
