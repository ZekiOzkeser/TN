using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TN.Models;

namespace TN.BLL
{
    public interface  IRepository<T> where T:ModelBase
    {
        bool Create(T model);
        bool Edit(T model);
        bool Delete(int id);
        IEnumerable<T> Listele();
        T Bring(int id);
    }
}