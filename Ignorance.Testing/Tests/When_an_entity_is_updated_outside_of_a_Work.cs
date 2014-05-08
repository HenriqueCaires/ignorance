using Ignorance.Testing.Data.EntityFramework;
using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Ignorance.Testing
{
    [TestClass]
    public class When_an_entity_is_updated_outside_of_a_Work : ServiceBehaviorTest
    {
        const string new_department_name = "Fred";
        [TestMethod]
        public void Service_Update_should_apply_the_changes()
        {
            // change the entity outside a working context
            this.Dept.Name = new_department_name;

            // use Update to re-attach and save the changes
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                ignorant.UpdateAndSave(this.Dept);
            }

            // verify the changes using EF
            using (var ignorant = kernel.Get<IIgnorantDepartment>())
            {
                var d = ignorant.Find(this.Dept.DepartmentID);
                Assert.AreEqual(new_department_name, d.Name, "Changes from outside of Work were not applied.");
            }
        }
    }
}
