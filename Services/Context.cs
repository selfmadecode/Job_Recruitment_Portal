using BigJobbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigJobbs.Services
{
    public class Context: Controller
    {
        public ApplicationDbContext _dbContext;
        public Context()
        {
            _dbContext = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }
    }
}