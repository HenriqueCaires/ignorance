using Ignorance.Testing.AdventureWorksProvider;
using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Ignorance.Testing
{
    [TestClass]
    public class When_a_service_creates_an_entity
    {
        private IKernel kernel;

        [TestInitialize]
        public void SetUp()
        {
            kernel = new StandardKernel();

            kernel.Bind<IWorkAdventureWork>().ToProvider<AdventureWorkProvider>();

        }

        [TestMethod]
        public void the_entity_should_not_be_null()
        {
            using (var work = kernel.Get<IWorkAdventureWork>())
            {
                var s = new DepartmentService(work);
                var d = s.Create();
                Assert.IsNotNull(d, "The entity was not created.");
            }
        }

        [TestMethod]
        public void the_entity_should_be_initialized_with_default_values()
        {
            using (var work = kernel.Get<IWorkAdventureWork>())
            {
                var s = new DepartmentService(work);
                var d = s.Create();
                Assert.AreEqual("New Department", d.Name, "OnCreate was not called.");
            }
        }

    }
}
