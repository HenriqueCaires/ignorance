using System;
using System.Linq;
using Ignorance.Testing.Data;
using Ignorance.Testing.Data.EntityFramework;
using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ignorance.EntityFramework;
using Ignorance.Testing.AdventureWorksProvider;

namespace Ignorance.Testing
{
    [TestClass]
    public class ServiceBehaviorTest
    {
        //public string GuidDeptName { get; set; }
        protected Department Dept { get; set; }
        protected IKernel kernel;

        [TestInitialize]
        public void SetUp()
        {
            kernel = new StandardKernel();

            kernel.Bind<IWorkAdventureWork>().ToProvider<AdventureWorkProvider>();

            // create the test entity, using a guid string for its name
            using (var work = kernel.Get<IWorkAdventureWork>())
            {
                var service = new DepartmentService(work);
                
                this.Dept = service.Create();
                this.Dept.Name = Guid.NewGuid().ToString();
                this.Dept.GroupName = Guid.NewGuid().ToString();
                service.Add(this.Dept);
                service.SaveChanges();
            }
        }
        
        [TestCleanup]
        public void TearDown()
        {
            // delete the test entity
            using (var work = kernel.Get<IWorkAdventureWork>())
            {
                var service = new DepartmentService(work);
                var d = service.FindByName(Dept.Name);
                if (d != null)
                {
                    service.DeleteAndSave(d);
                }

            }
        }
    }
}
