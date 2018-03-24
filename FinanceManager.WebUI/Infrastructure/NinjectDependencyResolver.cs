using System;
using System.Collections.Generic;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Concrete;
using System.Web.Mvc;
using Ninject;

namespace FinanceManager.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IExpenseRepository>().To<EFExpenseRepository>();
            kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            kernel.Bind<IIncomeRepository>().To<EFIncomeRepository>();
            kernel.Bind<ISavingRepository>().To<EFSavingRepository>();
        }
    }
}