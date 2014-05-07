using Ignorance.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignorance.Testing.AdventureWorksProvider
{
    public interface IWorkAdventureWork : IWorkEntityFramework
    {
    }

    public class WorkAdventureWork : Ignorance.EntityFramework.Work, IWorkAdventureWork
    {
        public WorkAdventureWork(DbContext context)
            : base(context)
        {
        }
    }
}
