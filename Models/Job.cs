using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Models
{
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Job Title is Required")]
        [Display (Name = "Job Title")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Job Description is Required")]
        [Display (Name = "Job Description and Requirements")]
        [AllowHtml]
        public string Description { get; set; }


        [Required(ErrorMessage = "Job Salary is Required")]
        public int Salary { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }


        [Required(ErrorMessage = "Hiring Company Name is Required")]
        [Display(Name = "Hiring Company")]
        public string HiringCompany { get; set; }


        [Required(ErrorMessage = "Job Location is Required")]
        public string Location { get; set; }

        // public string CompanyEmail { get; set; }
        // public string CompanyWebsite { get; set; }

        public JobCategory JobCategory { get; set; }

        [Required(ErrorMessage = "Select a Category")]
        [Display(Name = "Job Category")]
        public int JobCategoryId { get; set; }


        public JobType JobType { get; set; }

        [Required(ErrorMessage = "Select Job Type")]
        [Display (Name = "Job Type")]
        public int JobTypeId { get; set; }

    }
}