using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Interfaces;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BigJobbs.Services
{
    public class AdminDashboardServices : IAdminDashboardServices
    {
        readonly Context Ctx;
        IMail _mail;
        public AdminDashboardServices(Context ctx, IMail mail)
        {
            Ctx = ctx;
            _mail = mail;
        }

        public IEnumerable<JobCategory> GetAllJobCategories() => Ctx._dbContext.JobCategories.ToList();

        public IEnumerable<Job> GetAllJobs() => Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).ToList();


        public IEnumerable<JobType> GetAllJobTypes() => Ctx._dbContext.JobTypes.ToList();

        public int NumberOfJobs()
        {
            return Ctx._dbContext.Jobs.Include(c => c.JobCategory).Include(t => t.JobType).Count();
        }
        public void SaveImage(Job newJob)
        {
            // map the file path in DB to the image location(JobImages)
            string fileName = Path.GetFileNameWithoutExtension(newJob.ImageFile.FileName);
            string extension = Path.GetExtension(newJob.ImageFile.FileName);

            fileName += DateTime.Now.ToString("yymmssfff") + extension;
            newJob.ImagePath = "~/Content/JobImages/" + fileName;
            fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/JobImages/"), fileName);

            newJob.ImageFile.SaveAs(fileName);
        }
        public void SaveJob(Job newJob)
        {
            SaveImage(newJob);

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
            SaveImage(job);

            var jobToUpdate = Ctx._dbContext.Jobs.SingleOrDefault(i => i.Id == job.Id);

            jobToUpdate.DatePosted = job.DatePosted;
            jobToUpdate.Description = job.Description;
            jobToUpdate.HiringCompany = job.HiringCompany;
            jobToUpdate.JobCategory = job.JobCategory;
            jobToUpdate.Name = job.Name;
            jobToUpdate.Location = job.Location;
            jobToUpdate.Salary = job.Salary;
            jobToUpdate.JobType = job.JobType;
            jobToUpdate.ImagePath = job.ImagePath;

            Ctx._dbContext.SaveChanges();
        }

        public IEnumerable<Applicant> GetAllApplicants() => Ctx._dbContext.Applicants.Include(j => j.Job).Include(j => j.Job.JobType).ToList();

        public IEnumerable<Applicant> GetApplications(string condition) => Ctx._dbContext.Applicants.Include(j => j.Job)
            .Where(i => i.JobApplicationStatus == condition)
            .Include(j => j.Job.JobType).ToList();

        public NumberOfApplication NumberOfApplication()
        {
            var totalJobs = NumberOfJobs();
            var pendingApplications = GetApplications(JobApplicationStatus.pending).Count();
            var acceptedApplications = GetApplications(JobApplicationStatus.accepted).Count();
            var rejectedApplications = GetApplications(JobApplicationStatus.rejected).Count();

            var applications = new NumberOfApplication
            {
                TotalJobs = totalJobs,
                PendingApplications = pendingApplications,
                AcceptedApplications = acceptedApplications,
                RejectedApplications = rejectedApplications
            };

            return applications;
        }

        public Applicant GetJobAndApplicantDetails(int applicantId, int jobId) => Ctx._dbContext.Applicants
            .Include(j => j.Job)
            .Include(j => j.Job.JobType)
            .FirstOrDefault(i => i.Id == applicantId && i.Job.Id == jobId);

        public bool ProcessApplication(int applicantId, int jobId, string condition)
        {
            var pending = JobApplicationStatus.pending;

            var application = GetJobAndApplicantDetails(applicantId, jobId);

            //If the application has already been worked on
            //If the application status is not pending (i.e it has been processed before)
            if (application.JobApplicationStatus != pending)
                return true;
            else
            {
                application.JobApplicationStatus = condition;

                Ctx._dbContext.SaveChanges();
                NotifyApplicant(application, condition);

                return false;
            }

            
        }
        public void NotifyApplicant(Applicant applicant, string condition)
        {
            //send mail with the applicant info
            var messageBody = $"Your application has been processed!," +
                $" You applied for a/an {applicant.Job.Name} position in {applicant.Job.HiringCompany};" +
                $" your application has been {condition}";

            _mail.SendMail(applicant.EmailAddress, "Application Notification", messageBody);

        }
    }
    
}