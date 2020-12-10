using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Controllers
{
    public class JobController : Controller
    {
        IJobServices JobService;
        public JobController(IJobServices jobService)
        {
            JobService = jobService;
        }
        // GET: Job
        public ActionResult Index()
        {
            return View();
        }
        // [Authorize]
        public ActionResult JobDetails(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(currentUserId))
            {
                var jobDetails = JobService.GetOnlyJobDetails(id);

                if (jobDetails.Job == null)
                    return HttpNotFound("No Available job matching the Id");

                return View(jobDetails);
            }
            else
            {
                var jobAndApplicantDetails = JobService.GetJobAndApplicantDetails(id, currentUserId);

                if (jobAndApplicantDetails.Job == null)
                    return HttpNotFound("No Available job matching the Id");

                return View(jobAndApplicantDetails);
            }
        }
        [Authorize]
        public ActionResult ApplyForJob(int jobId)
        {
            var currentUserId = User.Identity.GetUserId();

            var jobDetails = JobService.ApplyForJob(jobId, currentUserId);

            if (jobDetails == null)
                return HttpNotFound("No Available job matching the Id");

            return View(jobDetails);
        }

        public ActionResult EditApplication(int jobId)
        {
            var currentUserId = User.Identity.GetUserId();

            var jobDetails = JobService.EditApplication(jobId, currentUserId);

            if (jobDetails == null)
                return HttpNotFound("No Available job matching this Id");

            return View("ApplyForJob", jobDetails);
        }
        [HttpPost]
        public ActionResult SaveApplication(UserAndJobViewModel userAndJobViewModel)
        {
            var currentUserId = User.Identity.GetUserId();

            string passportExtension = Path.GetExtension(userAndJobViewModel.Applicant.PassportFile.FileName);
            string pdfExtension = Path.GetExtension(userAndJobViewModel.Applicant.PdfFile.FileName);

            //Validate applicant passport...returns the view if the passport file is not in the correct format
            if(!(passportExtension.Contains(".jpg") || passportExtension.Contains(".jpeg") || passportExtension.Contains(".png")))
            {
                ViewBag.PassportError = "Only .jpeg, .png or .jpg files allowed";

                return View("ApplyForJob", userAndJobViewModel);
            }

            //Validate applicant pdf...returns the view if the pdf file is not in the correct format
            if (!pdfExtension.Contains(".pdf"))
            {
                ViewBag.PdfError = "Only .pdf files allowed";

                return View("ApplyForJob", userAndJobViewModel);
            }

            // if the model is not in the right format
            if (!ModelState.IsValid)
            {
                var userAndJob = new UserAndJobViewModel
                {
                    Applicant = userAndJobViewModel.Applicant,
                    Job = userAndJobViewModel.Job
                };
                return View("ApplyForJob", userAndJob);
            }

            try
            {
                if(userAndJobViewModel.Applicant.Id == 0)
                    JobService.SaveApplicantion(userAndJobViewModel);
                else
                    JobService.UpdateApplication(userAndJobViewModel, currentUserId);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            ViewBag.ApplicationStatus = "Congrats! Application Done";
            // add a page for successfully applied
            return View("ApplicationStatus", userAndJobViewModel);
        }
    }
}