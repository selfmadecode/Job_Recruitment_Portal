using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigJobbs.Models
{
    public class Applicant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email Address Required")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }


        [Required(ErrorMessage = "Phone Number Required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string JobApplicationStatus { get; set; } = "Pending";

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public Job Job { get; set; }
        public int JobId { get; set; }

    }
}