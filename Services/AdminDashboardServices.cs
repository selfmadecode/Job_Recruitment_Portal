using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BigJobbs.Services
{
    public class AdminDashboardServices : IAdminDashboardServices
    {
        readonly Context Ctx;
        public AdminDashboardServices(Context ctx)
        {
            Ctx = ctx;
        }

        public IEnumerable<JobCategory> GetAllJobCategories() => Ctx._dbContext.JobCategories.ToList();

        public IEnumerable<Job> GetAllJobs() => Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).ToList();


        public IEnumerable<JobType> GetAllJobTypes() => Ctx._dbContext.JobTypes.ToList();

        public int NumberOfJobs()
        {
            return Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).Count();
        }
        public void SaveJob(Job newJob)
        {
            Ctx._dbContext.Jobs.Add(newJob);
            Ctx._dbContext.SaveChanges();
        }

        public JobViewModel EditJob(int id)
        {
            var jobInDb = Ctx._dbContext.Jobs.SingleOrDefault(i => i.Id == id);

            if (jobInDb == null)
                return null;

            var jobViewModel = new JobViewModel
            {
                job = jobInDb,
                jobCategory = GetAllJobCategories(),
                jobType = GetAllJobTypes()
            };
            return jobViewModel;
        }

        public void UpdateJobInDb(Job job)
        {
            var jobToUpdate = Ctx._dbContext.Jobs.SingleOrDefault(i => i.Id == job.Id);

            jobToUpdate.DatePosted = job.DatePosted;
            jobToUpdate.Description = job.Description;
            jobToUpdate.HiringCompany = job.HiringCompany;
            jobToUpdate.JobCategory = job.JobCategory;
            jobToUpdate.Name = job.Name;
            jobToUpdate.Location = job.Location;
            jobToUpdate.Salary = job.Salary;
            jobToUpdate.JobType = job.JobType;

            Ctx._dbContext.SaveChanges();
        }
    }
}