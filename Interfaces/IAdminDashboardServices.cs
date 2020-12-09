using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigJobbs.Interfaces
{
    public interface IAdminDashboardServices
    {
        NumberOfApplication NumberOfApplication();
        IEnumerable<JobCategory> GetAllJobCategories();
        IEnumerable<JobType> GetAllJobTypes();
        void SaveJob(Job job);
        void UpdateJobInDb(Job job);
        IEnumerable<Job> GetAllJobs();
        JobViewModel EditJob(int id);
        IEnumerable<Applicant> GetAllApplicants();
        IEnumerable<Applicant> GetApplications(string condition);
        Applicant GetJobAndApplicantDetails(int applicantId, int jobId);
        void ProcessApplication(int applicantId, int jobId, string condition);
    }
}
