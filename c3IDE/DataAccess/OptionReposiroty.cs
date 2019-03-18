using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using c3IDE.Models;
using LiteDB;

namespace c3IDE.DataAccess
{
    //options repo only supports upsert and delete, and get
    //there should only be one option record at any given time,
    //there is a hardcoded key guid on the options poco object
    public class OptionRepository : IRepository<Options>
    {
        public string Database { get; set; } = "data.db";
        public string Collection { get; set; } = "Options";
        public string Path => System.IO.Path.Combine(App.DataFolder, Database);

        public void Insert(Options value)
        {
            throw new NotImplementedException();
        }

        public void Upsert(Options value)
        {
            using (var db = new LiteDatabase(Path))
            {
                var collection = db.GetCollection<Options>(Collection);
                collection.Upsert(value);
            }
        }

        public void BulkInsert(IEnumerable<Options> values)
        {
            throw new NotImplementedException();
        }

        public void BulkUpsert(IEnumerable<Options> values)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Options> GetAll()
        {
            using (var db = new LiteDatabase(Path))
            {
                var collection = db.GetCollection<Options>(Collection);
                return collection.FindAll();
            }
        }

        public IEnumerable<Options> Get(Expression<Func<Options, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Delete(Options value)
        {
            using (var db = new LiteDatabase(Path))
            {
                var collection = db.GetCollection<Options>(Collection);
                collection.Delete(value.Key);
            }
        }

        public void ResetCollection()
        {
            throw new NotImplementedException();
        }
    }
}
