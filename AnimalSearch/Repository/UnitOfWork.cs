using AnimalSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Repository
{
    
    public class UnitOfWork : IDisposable //uses to help us to work with repository and helps to use only one data context
    {
        private AnimalContext db = new AnimalContext();
        public AnimalRepository animalRepository;
        public UnitOfWork(AnimalContext context)
        {
            db = context;
        }
        //saves changes
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public AnimalRepository Animals
        {
            get //setting data
            {
                if (animalRepository == null)
                    animalRepository = new AnimalRepository(db);
                return animalRepository;
            }
        }
        //save data
        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();// Free any other managed objects here.
                }
                this.disposed = true;
            }
        }
        //garbage collector 
        public void Dispose()
        {
            Dispose(true);// dispose unmanaged resources
            GC.SuppressFinalize(this);
        }
    }

}