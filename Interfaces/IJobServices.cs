using BigJobbs.Infrastructure.ViewModels;
using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BigJobbs.Interfaces
{
    public interface IJobServices
    {
        IEnumerable<Job> GetAllJobs();
        IEnumerable<Job> GetJobsByCategory(int id);
        IEnumerable<JobCategory> GetAllJobCategorries();
        IEnumerable<JobType> GetAllJobTypes();

        Job GetJobDetails(int id);
        UserAndJobViewModel GetJobAndApplicantDetails(int id, string userId);
        UserAndJobViewModel GetOnlyJobDetails(int id);
        UserAndJobViewModel ApplyForJob(int jobId, string userId);
        UserAndJobViewModel EditApplication(int jobId, string userId);


        ApplicationUser GetUser(string id);

        bool SaveApplication(UserAndJobViewModel userAndJobViewModel);
        bool UpdateApplication(UserAndJobViewModel userAndJobViewModel, string currentUserId);
        string SaveFile(HttpPostedFileBase file, string folderToSaveFile);
    }
}
