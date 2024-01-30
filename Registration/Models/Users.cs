using System.ComponentModel.DataAnnotations;

namespace Registration.Models
{
    public class Users
    {
        [Key]
        
        public int UserID    { get; set; }
        [Display(Name="Email")]
        [Required]
        public string EmailID   { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string Password  { get; set; }
        public string RegistrationType { get; set; }
        [Display(Name = "Status")]
        public bool    IsActive { get; set; }
        [Display(Name = "Email Verified")]
        public bool EmailVerified { get; set; }
        public DateTime AddedDate {  get; set; }
        [Display(Name = "User Type")]
        [Required]
        public string UserType { get; set;}

    }
}
