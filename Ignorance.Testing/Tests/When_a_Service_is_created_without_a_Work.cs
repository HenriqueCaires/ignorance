using Ignorance.Testing.AdventureWorksProvider;
using Ignorance.Testing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Ignorance.Testing
{
    [TestClass]
    public class When_a_Service_is_created_without_a_Work
    {
        private IKernel kernel;

        [TestInitialize]
        public void SetUp()
        {
            kernel = new StandardKernel();

            kernel.Bind<IWorkAdventureWork>().ToProvider<AdventureWorkProvider>();

        }

        [TestMethod]
        public void it_should_still_be_able_to_query_data()
        {
            using (var work = kernel.Get<IWorkAdventureWork>())
            {
                var s = new DepartmentService(work);
                Assert.IsNotNull(s.GetFirst(), "Service did not create its own work when initialized without one.");
            }
        }
    }
}
