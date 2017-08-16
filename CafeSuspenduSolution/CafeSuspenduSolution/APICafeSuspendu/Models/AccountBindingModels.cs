using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace APICafeSuspendu.Models
{
    // Models used as parameters to AccountController actions.
    public class CreateUserBindingModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Cafe name")]
        public string CafeName { get; set; }

        [Display(Name = "Street")]
        [MaxLength(50)]
        public string Street { get; set; }

        [Display(Name = "Number")]
        [MaxLength(5)]
        public string Number { get; set; }

        [Display(Name = "Nb coffee required for promotion")]
        public int? NbCoffeeRequiredForPromotion { get; set; }

        [Display(Name = "Promotion Value")]
        public double PromotionValue { get; set; }

        [Display(Name = "Bookings")]
        public ICollection<Booking> Bookings { get; set; }

        //[Required]
        [Display(Name = "Charities")]
        public ICollection<Charity> Charities { get; set; }
        [Display(Name = "Time Tables")]
        public RegistrationTimeTable[] TimeTables { get; set; }

        //person

        [Display(Name = "First Name")]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Roles")]
        public string[] Roles { get; set; }



    }

     public class AddExternalLoginBindingModel
     {
         [Required]
         [Display(Name = "External access token")]
         public string ExternalAccessToken { get; set; }
     }

     public class ChangePasswordBindingModel
     {
         [Required]
         [DataType(DataType.Password)]
         [Display(Name = "Current password")]
         public string OldPassword { get; set; }

         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "New password")]
         public string NewPassword { get; set; }

         [DataType(DataType.Password)]
         [Display(Name = "Confirm new password")]
         [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
         public string ConfirmPassword { get; set; }
     }

     public class RegisterBindingModel
     {
         [Required]
         [Display(Name = "Email")]
         public string Email { get; set; }

         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "Password")]
         public string Password { get; set; }

         [DataType(DataType.Password)]
         [Display(Name = "Confirm password")]
         [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
         public string ConfirmPassword { get; set; }
     }

     public class RegisterExternalBindingModel
     {
         [Required]
         [Display(Name = "Email")]
         public string Email { get; set; }
     }

     public class RemoveLoginBindingModel
     {
         [Required]
         [Display(Name = "Login provider")]
         public string LoginProvider { get; set; }

         [Required]
         [Display(Name = "Provider key")]
         public string ProviderKey { get; set; }
     }

     public class SetPasswordBindingModel
     {
         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "New password")]
         public string NewPassword { get; set; }

         [DataType(DataType.Password)]
         [Display(Name = "Confirm new password")]
         [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
         public string ConfirmPassword { get; set; }
     }
}
