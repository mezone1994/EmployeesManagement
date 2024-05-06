using EmployeesManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.ViewModels
{
    public class ProfileViewModel
    {
        public ICollection<SystemProfile> Profiles { get; set; }

        public ICollection<int> RolesRightsIds { get; set; }

        public int[] Ids {  get; set; }


        [Display(Name ="Role")]
        public string RoleId { get; set; }

        [Display(Name = "System Task")]
        public int TaskId { get; set; }


    }
}
