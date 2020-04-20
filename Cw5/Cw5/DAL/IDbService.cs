using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DAL
{
    public interface IDbService<T>
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        bool Add(T added);
        bool Update(T updated);
        bool Delete(string studentId);
    }
}