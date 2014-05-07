using System;
using System.Linq;
using Ignorance.Domain;
using Ignorance.Testing.Data;
using System.Collections.Generic;
using Ignorance.Testing.AdventureWorksProvider;

namespace Ignorance.Testing.Domain
{
    public interface IDepartmentService : IService<Department>
    {
        IEnumerable<Department> All();
        Department First();
        void Delete(Department entity);
        void DeleteAndSave(Department entity);
        Department FindByName(string name);
        Department Find(short id);
        void SaveChanges();
    }

    public class DepartmentService : Service<Department>, IDepartmentService
    {
        public DepartmentService(IWorkAdventureWork work) : base(work, new Ignorance.EntityFramework.Store<Department>(work)) { }

        public DepartmentService(IWorkAdventureWork work, IStore<Department> store) : base(work, store) { }

        protected override void OnDeleting(Department entity)
        {
            if (entity.Name.Contains("DO NOT DELETE"))
                throw new ApplicationException("Um.");
        }

        protected override void OnSaving(Department entity)
        {
            if (entity.Name.Contains("Torture"))
                throw new ApplicationException("That's not cool.");
        }

        protected override void OnCreated(Department entity)
        {
            entity.Name = "New Department";
            entity.ModifiedDate = DateTime.Today;
        }

        public Department GetByID(short id)
        {
            return this.Store.One(p => p.DepartmentID == id);
        }

        public Department GetFirst()
        {
            return this.Store.All().First();
        }

        public IEnumerable<Department> All()
        {
            return this.Store.All().AsEnumerable();
        }

        public Department First()
        {
            return this.Store.All().AsEnumerable().First();
        }

        public void Delete(Department entity)
        {
            this.Store.Remove(entity);
        }

        public void DeleteAndSave(Department entity)
        {
            this.Store.Attach(entity);
            this.Store.Remove(entity);
            SaveChanges();
        }

        public void SaveChanges()
        {
            this.Work.Save();
        }

        public Department FindByName(string name)
        {
            return this.Store.All().FirstOrDefault(p => p.Name == name);
        }

        public Department Find(short id)
        {
            return this.Store.All().FirstOrDefault(p => p.DepartmentID == id);
        }
    }
}
