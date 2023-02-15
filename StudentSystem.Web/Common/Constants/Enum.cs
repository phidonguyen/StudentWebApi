using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Web.Common.Constants
{
    public enum UserStatusEnum
    {
        None = 0,
        [Display(Name = "Đang chờ")] Waiting = 1,
        [Display(Name = "Tạm khoá")] Banned = 2,
        [Display(Name = "Hoạt động")] Activated = 3
    }
}

