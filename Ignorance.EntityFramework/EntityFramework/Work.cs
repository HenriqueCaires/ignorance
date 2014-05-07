using System.Data.Entity;
using System.Linq;

namespace Ignorance.EntityFramework
{
    public interface IWorkEntityFramework : IWork { }
    public class Work : Ignorance.Work, IWorkEntityFramework
    {
        public DbContext DataContext { get; set; }

        public Work(DbContext context)
        {
            this.DataContext = context;
        }

        protected override void Commit()
        {
            this.DataContext.SaveChanges();
        }

        public override void Dispose()
        {
            this.DataContext.Dispose();
        }

        public override System.Collections.ICollection Added
        {
            get
            {
                return (from p in this.DataContext.ChangeTracker.Entries()
                        where p.State == EntityState.Added
                        select p.Entity).ToList();
            }
        }

        public override System.Collections.ICollection Updated
        {
            get
            {
                return (from p in this.DataContext.ChangeTracker.Entries()
                        where p.State == EntityState.Modified
                        select p.Entity).ToList();
            }
        }

        public override System.Collections.ICollection Deleted
        {
            get
            {
                return (from p in this.DataContext.ChangeTracker.Entries()
                        where p.State == EntityState.Deleted
                        select p.Entity).ToList();
            }
        }
    }
}
