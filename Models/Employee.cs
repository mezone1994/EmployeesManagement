using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class Employee : UserActivity
    {
        public int Id { get; set; }

        [Display(Name = "Employee Number")]
        public string EmpNo { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Middle name cannot be more than 50 characters long.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set;}

        public Country Country { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        public Department Department { get; set; }

        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        public SystemCodeDetail Gender { get; set; }

        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }

        public Designation Designation { get; set; }

        public static string GenerateEmployeeNumber(int count)
        {
            // Increment count and format the employee number
            return $"EMP{count + 1:D3}";
        }
        [Display(Name = "Employee Photo")]
        public string? Photo { get; set; }

    }
}
