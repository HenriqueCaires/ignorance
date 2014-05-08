using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;

namespace Ignorance.Testing
{
    [TestClass]
    public class When_a_Service_deletes_an_entity : ServiceBehaviorTest
    {
        [TestMethod]
        public void it_can_come_from_the_current_Work()
        {
            // delete the test entity
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.GetByID(this.Dept.DepartmentID);
                ignorant.DeleteAndSave(d);
            }

            // verify its deletion
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.Find(this.Dept.DepartmentID);
                Assert.IsNull(d, "Entity was not deleted.");
            }
        }

        [TestMethod]
        public void it_can_come_from_outside_the_current_Work()
        {
            // delete the test entity

            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var v = ignorant.All();
                foreach (var item in v)
                {
                    var abc = item;
                }
            }

            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                ignorant.DeleteAndSave(this.Dept);
            }

            // verify its deletion
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.Find(this.Dept.DepartmentID);
                Assert.IsNull(d, "Entity was not deleted.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "OnDeleting was not called when deleting the entity.")]
        public void it_should_call_OnDeleting_first()
        {
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.GetByID(this.Dept.DepartmentID);

                try
                {
                    // update the entity to something that will trip the OnDeleting rules
                    d.Name = "Department of Important Things DO NOT DELETE";
                    ignorant.Save();

                    // now try to delete it
                    ignorant.DeleteAndSave(d);
                }
                finally
                {
                    // change it back so that tear down works
                    d.Name = this.Dept.Name;
                    ignorant.Save();
                }
            }
        }
    }
}
