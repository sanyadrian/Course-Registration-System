using System.ComponentModel.DataAnnotations;
using Azure.Core.Pipeline;
namespace Lab6.DataAccess;


    public class EmployeeMetadata
    {
        [Required(ErrorMessage = "Employee name is required. ")]
        [RegularExpression(@"[a-zA-Z]+\s+[a-zA-Z]+", 
        ErrorMessage = "Must be in the form of the first name followed by the last name")]
        [Display(Name="Employee Name")]

        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "User name is required")]
        [StringLength(30, MinimumLength = 3,
        ErrorMessage ="User name length should be more than 3 characters. ")]
        [Display(Name="Network ID")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 5,
        ErrorMessage = "Password length should be more than 5 characters.")]
        public string Password { get; set; } = null!;
        [Display(Name = "Job Title(s)")]
        public string Roles{get; set;}
    }

