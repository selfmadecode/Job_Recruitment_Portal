using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Services
{
    public class JobServices : IJobServices
    {
        readonly Context Ctx;
        IMail _mail;

        public JobServices(Context _ctx, IMail mail)
        {
            Ctx = _ctx;
            _mail = mail;
        }

        public IEnumerable<Job> GetAllJobs() => Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).ToList();

        public IEnumerable<JobCategory> GetAllJobCategorries() => Ctx._dbContext.JobCategories.ToList();

        public IEnumerable<JobType> GetAllJobTypes() => Ctx._dbContext.JobTypes.ToList();

        public IEnumerable<Job> GetJobsByCategory(int id)
        {
            return Ctx._dbContext.Jobs.Where(j => j.JobCategoryId == id).Include(j => j.JobType).ToList();
        }
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


        public void SaveImage(Applicant appplicantDetails)
        {
            // map the file path in DB to the image location(JobImages)
            string fileName = Path.GetFileNameWithoutExtension(appplicantDetails.PassportFile.FileName);
            string extension = Path.GetExtension(appplicantDetails.PassportFile.FileName);

            fileName += DateTime.Now.ToString("yymmssfff") + extension;
            appplicantDetails.PassportPath = "~/Content/JobImages/" + fileName;
            fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/JobImages/"), fileName);

            appplicantDetails.PassportFile.SaveAs(fileName);
        }


        public void SavePdf(Applicant appplicantDetails)
        {
            // map the file path in DB to the image location(Pdf)
            string fileName = Path.GetFileNameWithoutExtension(appplicantDetails.PdfFile.FileName);
            string extension = Path.GetExtension(appplicantDetails.PdfFile.FileName);

            fileName += DateTime.Now.ToString("yymmssfff") + extension;
            appplicantDetails.PdfPath = "~/Content/Pdf/" + fileName;
            fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Pdf/"), fileName);

            appplicantDetails.PdfFile.SaveAs(fileName);
        }


        public bool SaveApplication(UserAndJobViewModel userAndJobViewModel)
        {

            if (userAndJobViewModel.Applicant.JobApplicationStatus != JobApplicationStatus.pending)
            {
                return true;
            }

            SaveImage(userAndJobViewModel.Applicant);
            SavePdf(userAndJobViewModel.Applicant);

            // Save User Application
            var applicant = new Applicant
            {
                FirstName = userAndJobViewModel.Applicant.FirstName,
                LastName = userAndJobViewModel.Applicant.LastName,
                EmailAddress = userAndJobViewModel.Applicant.EmailAddress,
                PhoneNumber = userAndJobViewModel.Applicant.PhoneNumber,
                UserId = userAndJobViewModel.Applicant.UserId,
                JobId = userAndJobViewModel.Job.Id,
                PassportPath = userAndJobViewModel.Applicant.PassportPath,
                PdfPath = userAndJobViewModel.Applicant.PdfPath
            };
            

            Ctx._dbContext.Applicants.Add(applicant);
            Ctx._dbContext.SaveChanges();

            //send mail with the applicant info
            var emailBody = $"{userAndJobViewModel.Applicant.FirstName +" "+ userAndJobViewModel.Applicant.LastName} applied as a/an: {userAndJobViewModel.Job.Name}" +
                $"in {userAndJobViewModel.Job.HiringCompany};";

            NotifyAdmin(emailBody);

            return false;
        }


        public void NotifyAdmin(string emailBody)
        {
            string adminEmail = AdminEmailForNotification.adminEmailAddress;

            _mail.SendMail(adminEmail, "Application Notification", emailBody);
        }

        public UserAndJobViewModel EditApplication(int jobId, string currentUserId)
        {
            var jobInDb = GetJobDetails(jobId);

            //GET the applicant application where applicant (jobId, UserId) Value is (jobId, currentUserId)
            var applicantApplication = GetApplicantApplication(currentUserId, jobId);

            if (jobInDb == null)
                return null;

            // Applicant did not apply for the job he/she wants to edit
            if (applicantApplication == null)
                return null;

            // If the application has been processes and user tries to pull the record for update application
            if (applicantApplication.JobApplicationStatus != JobApplicationStatus.pending)
            {
                return null;
            }

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


        public bool UpdateApplication(UserAndJobViewModel userAndJobViewModel, string currentUserId)
        {

            // Get applicant application from DB and update the details
            var applicationToUpdate = GetApplicantApplication(currentUserId, userAndJobViewModel.Job.Id);

            if (applicationToUpdate.JobApplicationStatus != JobApplicationStatus.pending)
            {
                return true;
            }

            SaveImage(userAndJobViewModel.Applicant);
            SavePdf(userAndJobViewModel.Applicant);


            applicationToUpdate.FirstName = userAndJobViewModel.Applicant.FirstName;
            applicationToUpdate.LastName = userAndJobViewModel.Applicant.LastName;
            applicationToUpdate.EmailAddress = userAndJobViewModel.Applicant.EmailAddress;
            applicationToUpdate.PhoneNumber = userAndJobViewModel.Applicant.PhoneNumber;
            applicationToUpdate.UserId = userAndJobViewModel.Applicant.UserId;
            applicationToUpdate.JobId = userAndJobViewModel.Job.Id;
            applicationToUpdate.Job = userAndJobViewModel.Job;
            applicationToUpdate.User = userAndJobViewModel.Applicant.User;
            applicationToUpdate.Id = userAndJobViewModel.Applicant.Id;
            applicationToUpdate.PassportPath = userAndJobViewModel.Applicant.PassportPath;
            applicationToUpdate.PdfPath = userAndJobViewModel.Applicant.PdfPath;

            Ctx._dbContext.SaveChanges();


            var emailBody = $"{applicationToUpdate.FirstName + applicationToUpdate.LastName} updated application" +
                $"in {applicationToUpdate.Job.HiringCompany};";


            NotifyAdmin(emailBody);
            return false;
        }
    }
}