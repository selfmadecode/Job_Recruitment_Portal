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
    [Authorize]
    public class JobController : Controller
    {
        IJobServices JobService;
        IFile fileExtension;


        public JobController(IJobServices jobService, IFile _file)
        {
            JobService = jobService;
            fileExtension = _file;
        }


        // GET: Job
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
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
            var isJpeg = fileExtension.IsJpegOrPng(passportExtension);


            if(isJpeg == false)
            {
                ViewBag.PassportError = "Only .jpeg, .png or .jpg files allowed";

                return View("ApplyForJob", userAndJobViewModel);
            }


            var isPdf = fileExtension.IsPdf(pdfExtension);

            //Validate applicant pdf...returns the view if the pdf file is not in the correct format
            if(isPdf == false)
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
            return View("ApplicationStatus", userAndJobViewModel);
        }
    }
}