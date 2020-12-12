using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Controllers
{
    // to avoid break in code when the role isn't spelt correctly
    [Authorize(Roles = BigJobbs.Models.Roles.admin)]
    public class DashboardController : Controller
    {
        IAdminDashboardServices adminDS;
        IFile fileExtension;

        public DashboardController(IAdminDashboardServices iAdS, IFile _file)
        {
            adminDS = iAdS;
            fileExtension = _file;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            var NumberOfJobs = adminDS.NumberOfApplication();

            return View(NumberOfJobs);
        }


        [HttpGet]
        public ActionResult AddJob()
        {
            var jobCategories = adminDS.GetAllJobCategories();
            var jobTypes = adminDS.GetAllJobTypes();

            var vm = new JobViewModel
            {
                jobCategory = jobCategories,
                jobType = jobTypes,
                job = new Job()
            };
            
            return View("JobForm", vm);
        }


        [HttpPost]
        public ActionResult SaveJob(JobViewModel newJob)
        {

            string imageExtenstion = Path.GetExtension(newJob.job.ImageFile.FileName);

            var isJpeg = fileExtension.IsJpegOrPng(imageExtenstion);

            if (!ModelState.IsValid || isJpeg == false)
            {
                var job = new JobViewModel
                {
                    job = newJob.job,
                    jobCategory = adminDS.GetAllJobCategories(),
                    jobType = adminDS.GetAllJobTypes()

                };


                if (isJpeg == false)
                    ViewBag.ImageError = "Only .jpeg, .png or .jpg files allowed";


                return View("JobForm", job);
            }  

            if (newJob.job.Id == 0)
            {
                adminDS.SaveJob(newJob.job);

                return RedirectToAction("Index");
            }
            else
            {
                adminDS.UpdateJobInDb(newJob.job);

                return RedirectToAction("Index");
            }
        }


        public ActionResult GetAllJobs()
        {
            var jobs = adminDS.GetAllJobs();

            return View(jobs);
        }


        public ActionResult EditJob(int id)
        {
            var jobInDb = adminDS.EditJob(id);


            if (jobInDb == null)
                return HttpNotFound("No job matched the Id");


            return View("JobForm", jobInDb);
        }


        public ActionResult GetAllApplicants()
        {
            var allApplicants = adminDS.GetAllApplicants();

            return View("AllApplicants", allApplicants);
        }


        public ActionResult GetPendingApplications()
        {
            var pendingApplications = adminDS.GetApplications(JobApplicationStatus.pending);
            
            ViewBag.ApplicationStatus = "Pending";
            return View("PendingApplications", pendingApplications);
        }


        public ActionResult GetRejectedApplications()
        {
            var rejectedApplications = adminDS.GetApplications(JobApplicationStatus.rejected);

            ViewBag.ApplicationStatus = "Rejected";
            return View("ProcessedApplications", rejectedApplications);
        }


        public ActionResult GetAcceptedApplications()
        {
            var acceptedApplications = adminDS.GetApplications(JobApplicationStatus.accepted);

            ViewBag.ApplicationStatus = "Accepted";
            return View("ProcessedApplications", acceptedApplications);
        }


        public ActionResult ApplicationDetails(int applicantId, int jobId)
        {
            var application = adminDS.GetJobAndApplicantDetails(applicantId, jobId);

            if (application == null)
                return HttpNotFound("No application matching the Id");

            return View(application);
        }


        public ActionResult AcceptApplication(int applicantId, int jobId)
        {
            var alreadyProcessed = adminDS.ProcessApplication(applicantId, jobId, JobApplicationStatus.accepted);

            if (alreadyProcessed == true)
                return HttpNotFound("Application Already processed!");

            return RedirectToAction("GetPendingApplications");
        }


        public ActionResult RejectApplication(int applicantId, int jobId)
        {
            var alreadyProcessed = adminDS.ProcessApplication(applicantId, jobId, JobApplicationStatus.rejected);

            if (alreadyProcessed == true)
                return HttpNotFound("Application Already processed!");

            return RedirectToAction("GetPendingApplications");
        }
    }
}