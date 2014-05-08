using Ignorance.Testing.Data;
using Ignorance.Testing.Data.EntityFramework;
using Ignorance.Testing.Domain;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignorance.Testing.Providers
{
    public class IgnorantDepartmentProvider : Provider<IIgnorantDepartment>
    {
        protected override IIgnorantDepartment CreateInstance(IContext context)
        {
            IWork work = new Ignorance.EntityFramework.Work(new AdventureWorksEntities());
            IStore<Department> store = new Ignorance.EntityFramework.Store<Department>(work);
            return new IgnorantDepartment(work, store);
        }
    }

    public class IgnorantContactProvider : Provider<IIgnorantContact>
    {
        protected override IIgnorantContact CreateInstance(IContext context)
        {
            IWork work = new Ignorance.EntityFramework.Work(new AdventureWorksEntities());
            IStore<Contact> store = new Ignorance.EntityFramework.Store<Contact>(work);
            return new IgnorantContact(work, store);
        }
    }
}
