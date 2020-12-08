using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Services
{
    public class JobServices : IJobServices
    {
        readonly Context Ctx;

        public JobServices(Context _ctx)
        {
            Ctx = _ctx;
        }

        public IEnumerable<Job> GetAllJobs() => Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).ToList();

        public IEnumerable<JobCategory> GetAllJobCategorries() => Ctx._dbContext.JobCategories.ToList();

        public IEnumerable<JobType> GetAllJobTypes() => Ctx._dbContext.JobTypes.ToList();

        public Job GetJobDetails(int id)
        {
            return Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).FirstOrDefault(j => j.Id == id);
        }

        public UserAndJobViewModel ApplyForJob(int jobId, string currentUserId)
        {
            var jobInDb = GetJobDetails(jobId);

            var user = GetUser(currentUserId);

            if (jobInDb == null)
                return null;

            var applicant = new Applicant
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
                UserId = user.Id
            };

            var applicanVm = new UserAndJobViewModel
            {
                Job = jobInDb,
                Applicant = applicant

            };

            return applicanVm;

        }

        public ApplicationUser GetUser(string currentUserId) => Ctx._dbContext.Users.SingleOrDefault(i => i.Id == currentUserId);

        public void SaveApplicantion(UserAndJobViewModel userAndJobViewModel)
        {
            var appplicant = new Applicant
            {
                FirstName = userAndJobViewModel.Applicant.FirstName,
                LastName = userAndJobViewModel.Applicant.LastName,
                EmailAddress = userAndJobViewModel.Applicant.EmailAddress,
                PhoneNumber = userAndJobViewModel.Applicant.PhoneNumber,
                UserId = userAndJobViewModel.Applicant.UserId,
                JobId = userAndJobViewModel.Job.Id
            };

            Ctx._dbContext.Applicants.Add(appplicant);
            Ctx._dbContext.SaveChanges();
        }
        public UserAndJobViewModel EditApplication(int jobId, string currentUserId)
        {
            var jobInDb = GetJobDetails(jobId);
            var applicant = GetApplicantApplication(currentUserId, jobId);

            if (jobInDb == null)
                return null;

            var jobApplicant = new Applicant
            {
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                EmailAddress = applicant.EmailAddress,
                UserId = applicant.UserId,
                PhoneNumber = applicant.PhoneNumber,
                Id =applicant.Id,
                JobApplicationStatus = applicant.JobApplicationStatus,
                Job = applicant.Job,
                JobId = applicant.JobId,
                User = applicant.User
                
            };

            var applicanVm = new UserAndJobViewModel
            {
                Job = jobInDb,
                Applicant = jobApplicant

            };
            return applicanVm;
        }
        public Applicant GetApplicantApplication(string currentUserId, int jobId) => Ctx._dbContext.Applicants.SingleOrDefault(i => i.UserId == currentUserId && i.JobId == jobId);

        public void UpdateApplication(UserAndJobViewModel userAndJobViewModel, string currentUserId)
        {
            var applicationToUpdate = GetApplicantApplication(currentUserId, userAndJobViewModel.Job.Id);

            applicationToUpdate.FirstName = userAndJobViewModel.Applicant.FirstName;
            applicationToUpdate.LastName = userAndJobViewModel.Applicant.LastName;
            applicationToUpdate.EmailAddress = userAndJobViewModel.Applicant.EmailAddress;
            applicationToUpdate.PhoneNumber = userAndJobViewModel.Applicant.PhoneNumber;
            applicationToUpdate.UserId = userAndJobViewModel.Applicant.UserId;
            applicationToUpdate.JobId = userAndJobViewModel.Job.Id;
            applicationToUpdate.Job = userAndJobViewModel.Job;
            applicationToUpdate.User = userAndJobViewModel.Applicant.User;
            applicationToUpdate.Id = userAndJobViewModel.Applicant.Id;

            Ctx._dbContext.SaveChanges();
        }
    }
}