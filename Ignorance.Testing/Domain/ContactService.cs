using System;
using System.Collections.Generic;
using System.Linq;
using Ignorance.Domain;
using Ignorance.Testing.Data;
using Ignorance.Testing.AdventureWorksProvider;

namespace Ignorance.Testing.Domain
{
    public interface IContactService : IService<Contact>
    {
        IEnumerable<Department> All();
        Department First();
        void Delete(Department entity);
        void DeleteAndSave(Department entity);
        Department FindByName(string name);
        void SaveChanges();
    }

    public class ContactService : Service<Contact>, IContactService
    {
        public ContactService(IWorkAdventureWork work) : base(work, new Ignorance.EntityFramework.Store<Contact>(work)) { }

        public ContactService(IWorkAdventureWork work, IStore<Contact> store) : base(work, store) { }

        protected override void OnCreated(Contact entity)
        {
            entity.FirstName = "New";
            entity.LastName = "Guy";
            entity.Suffix = "Esq.";
            entity.NameStyle = true;
            entity.PasswordHash = "Hash!";
            entity.PasswordSalt = "Pepper.";
        }

        protected override void OnSaving(Contact entity)
        {
            if (!entity.EmailAddress.Contains("@"))
                throw new ApplicationException("Invalid email address.");
        }

        protected override void OnDeleting(Contact entity)
        {
            if (entity.LastName == "Fazzaro")
                throw new ApplicationException("Don't delete me or my relatives!");
        }

        public IEnumerable<Contact> GetByLastName(string lastName)
        {
            return this.Store.Some(p => 
                p.LastName.Equals(lastName, 
                    StringComparison.InvariantCultureIgnoreCase))
                .AsEnumerable();
        }

        public IEnumerable<Contact> GetContactsWithSuffixesBecauseTheyAreAwesome()
        {
            return this.Store.Some(p => p.Suffix != null);
        }

        public void JustGoAheadAndMakeSureEveryoneHasSuffixesNow()
        {
            var unsuffixedPeeps = this.Store.Some(p => p.Suffix == null);
            
            foreach (var peep in unsuffixedPeeps)
            {
                peep.Suffix = "Esq.";
            }
        }

        public int GetContactCount()
        {
            return this.Store.All().Count();
        }

        public IEnumerable<Department> All()
        {
            throw new NotImplementedException();
        }

        public Department First()
        {
            throw new NotImplementedException();
        }

        public void Delete(Department entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAndSave(Department entity)
        {
            throw new NotImplementedException();
        }

        public Department FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
