using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using LiteDB;

namespace c3IDE.DaataAccess
{
    public class OptionsRepository : IRepository<ApplicationOptions>
    {
        public string Database { get; set; } = "data.db";
        public string Collection { get; set; } = "Options";
        public void Insert(ApplicationOptions value)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                collection.Insert(value);
            }
        }

        public void Upsert(ApplicationOptions value)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                collection.Upsert(value);
            }
        }

        public void BulkInsert(IEnumerable<ApplicationOptions> values)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                collection.Insert(values);
            }
        }

        public void BulkUpsert(IEnumerable<ApplicationOptions> values)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                collection.Upsert(values);
            }
        }

        public IEnumerable<ApplicationOptions> GetAll()
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                return collection.FindAll();
            }
        }

        public IEnumerable<ApplicationOptions> Get(Expression<Func<ApplicationOptions, bool>> predicate)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                return collection.Find(predicate);
            }
        }

        public void Delete(ApplicationOptions value)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<ApplicationOptions>(Collection);
                collection.Delete(value.Id);
            }
        }

        public void ResetCollection()
        {
            using (var db = new LiteDatabase(Database))
            {
                db.DropCollection(Collection);
            }
        }
    }
}
