using System;
using System.Collections.Generic;

namespace Ignorance.Domain
{
    public interface IService<T>
     where T : class
    {
        void Add(System.Collections.Generic.IEnumerable<T> entities);
        void Add(T entity);
        T Create();
        void Delete(System.Collections.Generic.IEnumerable<T> entities);
        void Delete(T entity);
        void Update(System.Collections.Generic.IEnumerable<T> entities);
        void Update(T entity);
    }

    /// <summary>
    /// A partial implementation of a Domain Service. A class implementing this abstract 
    /// governs business rules around an entity of type T.
    /// </summary>
    /// <typeparam name="T">The data entity type to be serviced.</typeparam>
    public abstract class Service<T> : IService<T>
        where T : class
    {
        /// <summary>
        /// Creates a new service instance, with the given 
        /// unit of work. If no UoW is given, a 
        /// new UoW is created.
        /// </summary>
        public Service(IWork work, IStore<T> store)
        {
            if (work == null)
                throw new NullReferenceException("work");

            if (store == null)
                throw new NullReferenceException("store");

            this.Work = work;
            this.Store = store;

            WireUpValidators();
        }

        protected virtual void WireUpValidators()
        {
            this.Work.Adding += (object sender, EntityEventArgs e) =>
            {
                if (e.Entity is T)
                    OnSaving(e.Entity as T);
            };

            this.Work.Updating += (object sender, EntityEventArgs e) =>
            {
                if (e.Entity is T)
                    OnSaving(e.Entity as T);
            };

            this.Work.Deleting += (object sender, EntityEventArgs e) =>
            {
                if (e.Entity is T)
                    OnDeleting(e.Entity as T);
            };
        }

        /// <summary>
        /// Called before an entity is deleted from storage. 
        /// This is the place to have a really good think about what 
        /// data you are totally cool with losing forever. 
        /// </summary>
        protected abstract void OnDeleting(T entity);

        /// <summary>
        /// Called before the given entity is persisted to storage. 
        /// This is a good place to enforce some standards around what
        /// sort of riff-raff you allow near your database. 
        /// 
        /// Excuse me, data storage implementation.
        /// </summary>
        protected abstract void OnSaving(T entity);

        /// <summary>
        /// Gets or sets the unit of work
        /// for the service being performed.
        /// </summary>
        protected IWork Work { get; set; }

        /// <summary>
        /// Gets or sets a Store for the service's entity type,
        /// connected to the service's unit of work.
        /// </summary>
        /// <returns></returns>
        protected IStore<T> Store { get; set; }

        /// <summary>
        /// Called after the given entity has been instantiated. 
        /// Use this method to apply defaults for the entity.
        /// </summary>
        protected abstract void OnCreated(T entity);

        /// <summary>
        /// Creates an instance of an entity, 
        /// and calls the OnCreated method so 
        /// you can start hosing it up almost 
        /// immediately.
        /// </summary>
        /// <returns></returns>
        public virtual T Create()
        {
            var entity = System.Activator.CreateInstance<T>();
            OnCreated(entity);
            return entity;
        }

        /// <summary>
        /// Adds the logic from OnSaving to a list of things 
        /// the Work needs to do before it can save the given
        /// entity, and then makes really solid plans to save 
        /// said entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            this.Store.Add(entity);
        }

        /// <summary>
        /// For each of the entities in the given list, 
        /// adds the logic from OnSaving to a list of things 
        /// the Work needs to do before it can save them, and 
        /// then makes really solid plans to save them.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (var e in entities) Add(e);
        }

        /// <summary>
        /// Attaches the given entity to its store, 
        /// making really solid plans to save said entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            this.Store.Attach(entity);
        }

        /// <summary>
        /// For each of the entities in the given list, 
        /// attaches them to its store, and makes really 
        /// solid plans to save save them.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (var e in entities) Update(e);
        }

        /// <summary>
        /// Makes really solid plans to delete the given entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            this.Store.Attach(entity);
            this.Store.Remove(entity);
        }

        /// <summary>
        /// Makes really solid plans to delete each 
        /// of the entities in the given list.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (var e in entities) Delete(e);
        }
    }
}
