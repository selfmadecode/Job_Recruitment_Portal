using BigJobbs.Interfaces;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BigJobbs.Services
{
    public class ApplicantDashboardServices : IApplicantDashboardServices
    {
        readonly Context ctx;
        public ApplicantDashboardServices(Context _ctx)
        {
            ctx = _ctx;
        }
        public IQueryable<Applicant> GetAllApplicantApplication(string applicantId) => ctx._dbContext.Applicants
            .Include(i => i.Job)
            .Include(i => i.Job.JobCategory)
            .Include(i => i.Job.JobType)
            .Where(i => i.UserId == applicantId);


    }
}