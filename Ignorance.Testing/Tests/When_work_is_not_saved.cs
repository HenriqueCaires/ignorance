using System;
using System.Linq;
using Ignorance.Testing.Data;
using Ignorance.Testing.Data.EntityFramework;
using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ignorance.Testing.Providers;

namespace Ignorance.Testing
{
    [TestClass]
    public class When_work_is_not_saved
    {
        public short TestDepartmentID { get; set; }

        private IKernel kernel;
        
        [TestInitialize]
        public void SetUp()
        {
            kernel = new StandardKernel();

            //kernel.Bind<IIgnorantContact>().ToProvider<IgnorantContactProvider>();
            kernel.Bind<IIgnorantDepartment>().ToProvider<IgnorantDepartmentProvider>();

            // create a record using straight EF
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = new Department() { 
                     Name = "Ignorance",
                     GroupName = "Information Technology",
                     ModifiedDate = DateTime.Today
                };
                ignorant.AddAndSave(d);

                this.TestDepartmentID = d.DepartmentID;
            }
        }


        [TestMethod]
        public void added_entities_should_not_persist()
        {
            var guidName = Guid.NewGuid().ToString();
            // using Service API, Add a record
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.Create();
                d.Name = guidName;
                ignorant.Add(d);

                // but do NOT call Save on Work.
            }

            // test (using straight EF) that the record still exists.
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.FindByName(guidName);
                Assert.IsNull(d, "Add was persisted without Work.Save().");
            }
        }

        [TestMethod]
        public void changes_to_entities_should_not_persist()
        {
            // using Service API, call Update on the record
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.GetByID(this.TestDepartmentID);
                d.Name = "Updated";
                ignorant.Update(d);

                // but do NOT call Save on Work.
            }

            // test (using straight EF) that the record still exists.
            using (var db = new AdventureWorksEntities())
            {
                var d = db.Departments.Find(this.TestDepartmentID);
                Assert.AreEqual("Ignorance", d.Name, "Update was persisted without Work.Save().");
            }
        }

        [TestMethod]
        public void entities_should_not_be_deleted_from_storage()
        {
            // using Service API, call Delete on the record
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.GetByID(this.TestDepartmentID);
                ignorant.Delete(d);

                // but do NOT call Save on Work.
            }

            // test (using straight EF) that the record still exists.
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.Find(this.TestDepartmentID);
                Assert.IsNotNull(d, "Changes were persisted without Work.Save().");
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            // delete the test record using straight EF
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.Find(this.TestDepartmentID);
                ignorant.DeleteAndSave(d);
            }
        }
    }
}
