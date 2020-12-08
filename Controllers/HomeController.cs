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
            var jobs = JobService.GetAllJobs();

            if (User.IsInRole(Roles.admin))
                return RedirectToAction("index", "Dashboard");
            else
                return View(jobs);
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