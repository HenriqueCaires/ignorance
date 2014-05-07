using Ignorance.Testing.Data.EntityFramework;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignorance.Testing.AdventureWorksProvider
{
    public class AdventureWorkProvider : Provider<IWorkAdventureWork>
    {
        protected override IWorkAdventureWork CreateInstance(IContext context)
        {
            return new WorkAdventureWork(new AdventureWorksEntities());
        }
    }
}
