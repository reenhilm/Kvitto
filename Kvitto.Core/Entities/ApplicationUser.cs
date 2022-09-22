using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kvitto.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = default!;
        [PersonalData]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = default!;
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
