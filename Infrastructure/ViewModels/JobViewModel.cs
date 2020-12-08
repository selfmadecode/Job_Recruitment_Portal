using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigJobbs.Infrastructure.ViewModels
{
    public class JobViewModel
    {
        public Job job { get; set; }
        public IEnumerable<JobType>jobType { get; set; }
        public IEnumerable<JobCategory> jobCategory { get; set; }
    }
}