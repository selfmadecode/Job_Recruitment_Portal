using BigJobbs.Interfaces;
using BigJobbs.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BigJobbs.Infrastructure
{
    public class NinjectResolver : IDependencyResolver
    {
        private IKernel _kernel;
        public NinjectResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBinding();
        }

        private void AddBinding()
        {
             _kernel.Bind<IJobServices>().To<JobServices>();
             _kernel.Bind<IApplicantDashboardServices>().To<ApplicantDashboardServices>();
             _kernel.Bind<IAdminDashboardServices>().To<AdminDashboardServices>();

        }

        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);


        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);

    }
}