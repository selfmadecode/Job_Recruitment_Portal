using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigJobbs.Interfaces
{
    public interface IApplicantDashboardServices
    {
        IQueryable<Applicant> GetAllApplicantApplication(string applicantId);
        //Applicant ViewApplicant();
        //// Applicant EditApplicant();
        //Applicant AcceptApplicant();
        //Applicant RejectApplicant();
    }
}
