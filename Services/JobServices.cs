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
            var jobInDb = Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).FirstOrDefault(j => j.Id == id);
            
            if (jobInDb == null)
                return null;
            else
                return jobInDb;
        }
        public UserAndJobViewModel GetOnlyJobDetails(int id)
        {
            var onlyJobDetails = GetJobDetails(id);


            var onlyJob = new UserAndJobViewModel
            {
                Job = onlyJobDetails,
                Applicant = new Applicant()
            };
            return onlyJob;
        }
        public UserAndJobViewModel GetJobAndApplicantDetails(int id, string userId)
        {
            var applicant = Ctx._dbContext.Applicants.FirstOrDefault(j => j.Job.Id == id && j.UserId == userId);
            
            var job = GetJobDetails(id);

            var application = new UserAndJobViewModel
            {
                Applicant = applicant,
                Job = job
            };
            return application;
        }

        public UserAndJobViewModel ApplyForJob(int jobId, string currentUserId)
        {
            var jobInDb = GetJobDetails(jobId);

            var user = GetUser(currentUserId);

            if (jobInDb == null)
                return null;

            // fills the job form with the following properties from the user table for the user
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
            // Save User Application
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

            //GET the applicant application where applicant (jobId, UserId) Value is (jobId, currentUserId)
            var applicantApplication = GetApplicantApplication(currentUserId, jobId);

            if (jobInDb == null)
                return null;

            var jobApplicant = new Applicant
            {
                FirstName = applicantApplication.FirstName,
                LastName = applicantApplication.LastName,
                EmailAddress = applicantApplication.EmailAddress,
                UserId = applicantApplication.UserId,
                PhoneNumber = applicantApplication.PhoneNumber,
                Id = applicantApplication.Id,
                JobApplicationStatus = applicantApplication.JobApplicationStatus,
                Job = applicantApplication.Job,
                JobId = applicantApplication.JobId,
                User = applicantApplication.User
                
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
            // Get applicant application from DB and update the details
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