using BigJobbs.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Controllers
{
    public class ApplicantDashboardController : Controller
    {
        IApplicantDashboardServices applicantDashboard;
        public ApplicantDashboardController(IApplicantDashboardServices applicantDashboardServices)
        {
            applicantDashboard = applicantDashboardServices;
        }

        // GET: ApplicantDashboard
        [Authorize]
        public ActionResult Index()
        {
            string applicantId = User.Identity.GetUserId();

            var ApplicantApplications = applicantDashboard.GetAllApplicantApplication(applicantId);
            return View(ApplicantApplications);
        }
    }
}