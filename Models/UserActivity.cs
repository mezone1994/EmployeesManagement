namespace EmployeesManagement.Models
{
    public class UserActivity
    {
        public string CreatedByID { get; set; }

        public DateTime CreatedOn { get; set; }


        public string ModifiedByID { get; set; }

        public DateTime ModifiedOn { get; set; }



    }

    public class ApprovalActivity : UserActivity
    {
        public string ApprovedByID { get; set; }

        public DateTime ApprovedOn { get; set; }

    }
}
