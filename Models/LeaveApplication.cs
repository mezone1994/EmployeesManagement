using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class LeaveApplication: ApprovalActivity
    {
        public int Id { get; set; }
        [Display(Name ="Employee Name")]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [Display(Name = "Number of Days")]
        public int NoOfDays { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Duration")]
        public int DurationID { get; set; }

        public SystemCodeDetail Duration { get; set; }

        [Display(Name = "Leave Type")]
        public int LeaveTypeID { get; set; }

        public LeaveType LeaveType { get; set; }

        public string? Attachment { get; set; }
        [Display(Name = "Notes")]

        public string Description { get; set; }

        [Display(Name = "Approval Notes")]

        public string ApprovalNotes { get; set; }

        [Display(Name = "Status")]
        public int StatusID { get; set; }

        public SystemCodeDetail Status { get; set; }


    }
}
