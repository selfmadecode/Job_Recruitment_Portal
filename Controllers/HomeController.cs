using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Controllers
{
    public class HomeController : Controller
    {
        IJobServices JobService;
        public HomeController(IJobServices jobService)
        {
            JobService = jobService;
        }
        public ActionResult Index()
        {
            var jobs = JobService.GetAllJobs().Take(5);

            if (User.IsInRole(Roles.admin))
                return RedirectToAction("index", "Dashboard");
            else
                return View(jobs);
        }

        public ActionResult AllJobs(int id)
        {
            if(id <= 0)
            {
                var jobs = JobService.GetAllJobs();
                return View("Jobs", jobs);
            }
            else
            {
                var jobByCategory = JobService.GetJobsByCategory(id);
                return View("Jobs", jobByCategory);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}