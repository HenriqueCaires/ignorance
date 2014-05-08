using System;
using System.Linq;
using Ignorance.Testing.Data;
using Ignorance.Testing.Data.EntityFramework;
using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ignorance.EntityFramework;
using Ignorance.Testing.Providers;

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

            kernel.Bind<IIgnorantDepartment>().ToProvider<IgnorantDepartmentProvider>();

            // create the test entity, using a guid string for its name
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                this.Dept = ignorant.Create();
                this.Dept.Name = Guid.NewGuid().ToString();
                this.Dept.GroupName = Guid.NewGuid().ToString();
                ignorant.Add(this.Dept);
                ignorant.Save();
            }
        }
        
        [TestCleanup]
        public void TearDown()
        {
            // delete the test entity
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.FindByName(Dept.Name);
                if (d != null)
                {
                    ignorant.DeleteAndSave(d);
                }

            }
        }
    }
}
