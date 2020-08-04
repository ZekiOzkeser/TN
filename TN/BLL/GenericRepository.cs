using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TN.Models;

namespace TN.BLL
{
    public class GenericRepository<T> : IDisposable, IRepository<T> where T : ModelBase
    {
        DbContext ctx;
        DbSet<T> dbset;

        public GenericRepository(DbContext ctx)
        {
            this.ctx = ctx;
            this.dbset = ctx.Set<T>();
        }
        public bool Create(T model)
        {
            dbset.Add(model);
            return ctx.SaveChanges() > 0;
        }

        public bool Edit(T model)
        {
            ctx.Entry(model).State = EntityState.Modified;
            return ctx.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            dbset.Remove(dbset.Find(id));
            return ctx.SaveChanges() > 0;
        }

        public IEnumerable<T> Listele() => dbset.ToList();

        public IQueryable<T> Sort() => dbset;


        public T Bring(int id) => dbset.Find(id);
       

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ctx.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GenericRepository()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }


        #endregion
    }
}