namespace StudentSystem.DataAccess.EntityFramework.Entities
{
    public partial class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set;}
        public int? IdentityNumber { get; set;}
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
