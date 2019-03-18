using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace c3IDE.DataAccess
{
    public interface IRepository<T>
    {
        string Database { get; set; }
        string Collection { get; set; }
        string Path { get; }
        void Insert(T value);
        void Upsert(T value);
        void BulkInsert(IEnumerable<T> values);
        void BulkUpsert(IEnumerable<T> values);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Delete(T value);
        void ResetCollection();
    }
}
