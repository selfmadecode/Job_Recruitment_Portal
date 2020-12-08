using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigJobbs.Infrastructure.ViewModels
{
    public class UserAndJobViewModel
    {
        public Job Job { get; set; }
        public Applicant Applicant { get; set; }
    }
}