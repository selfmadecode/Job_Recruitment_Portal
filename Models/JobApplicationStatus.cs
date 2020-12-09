using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigJobbs.Models
{
    public static class JobApplicationStatus
    {
        // Status of application
        // Admin controller uses this class to query the database for application status
        public const string pending = "Pending";
        public const string rejected = "Rejected";
        public const string accepted = "Accepted";
    }
}