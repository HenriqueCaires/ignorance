using System;
using System.Linq;
using Ignorance.Domain;
using Ignorance.Testing.Data;
using System.Collections.Generic;

namespace Ignorance.Testing.Domain
{
    public interface IIgnorantDepartment : IService<Department>, IDisposable
    {
        void DeleteAndSave(Department entity);
        void UpdateAndSave(Department entity);
        void AddAndSave(Department entity);
        IEnumerable<Department> All();
        void Save();
        Department FindByName(string name);
        Department Find(short id);
        Department GetFirst();
        Department GetByID(short id);
    }

    public class IgnorantDepartment : Service<Department>, IIgnorantDepartment
    {
        public IgnorantDepartment(IWork work, IStore<Department> store) : base(work, store) { }

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

        public void Save()
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

        public void DeleteAndSave(Department entity)
        {
            Delete(entity);
            Work.Save();
        }
        
        public void UpdateAndSave(Department entity)
        {
            Update(entity);
            Work.Save();
        }

        public void AddAndSave(Department entity)
        {
            Add(entity);
            Work.Save();
        }

        public void Dispose()
        {
            this.Work.Dispose();
        }
    }
}
