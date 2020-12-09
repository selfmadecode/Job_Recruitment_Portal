using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigJobbs.Infrastructure.ViewModels
{
    public class NumberOfApplication
    {
        public int TotalJobs { get; set; }
        public int PendingApplications { get; set; }
        public int AcceptedApplications { get; set; }
        public int RejectedApplications { get; set; }
    }
}