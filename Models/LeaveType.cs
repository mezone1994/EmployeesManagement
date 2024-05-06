using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class LeaveType : UserActivity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Code field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Code must be between 2 and 50 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The Name must be between 5 and 100 characters.")]
        public string Name { get; set; }
    }
}
