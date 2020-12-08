using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Controllers
{
    // to avoid break in code when the role name is changed
    [Authorize(Roles = BigJobbs.Models.Roles.admin)]
    public class DashboardController : Controller
    {
        IAdminDashboardServices adminDS;

        public DashboardController(IAdminDashboardServices iAdS)
        {
            adminDS = iAdS;
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            int NumberOfJobs = adminDS.NumberOfJobs();
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
            if (!ModelState.IsValid)
            {
                var job = new JobViewModel
                {
                    job = newJob.job,
                    jobCategory = adminDS.GetAllJobCategories(),
                    jobType = adminDS.GetAllJobTypes()

                };
                return View("JobForm", job);
            }
            if(newJob.job.Id == 0)
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
    }
}