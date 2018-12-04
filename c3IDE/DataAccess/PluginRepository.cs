using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using c3IDE.Framework;
using c3IDE.PluginModels;
using LiteDB;

namespace c3IDE.DataAccess
{
    public class PluginRepository : IRepository<C3Plugin>
    {
        public string Database { get; set; } = "data.db";
        public string Collection { get; set; } = "Plugins";

        public PluginRepository()
        {
            BsonMapper.Global.RegisterType<Image>
            (
                serialize: (img) => img.ImageToBase64(),
                deserialize: (bson) =>
                {
                    var str = bson.ToString().Replace("\"", string.Empty);
                    return str.Base64ToImage();
                });
        }

        public void Insert(C3Plugin value)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
                collection.Insert(value);
            }
        }

        public void Upsert(C3Plugin value)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
                collection.Upsert(value);
            }
        }

        public void BulkInsert(IEnumerable<C3Plugin> values)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
                collection.Insert(values);
            }
        }

        public void BulkUpsert(IEnumerable<C3Plugin> values)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
                collection.Upsert(values);
            }
        }

        public IEnumerable<C3Plugin> GetAll()
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
                return collection.FindAll();
            }
        }

        public IEnumerable<C3Plugin> Get(Expression<Func<C3Plugin, bool>> predicate)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
                return collection.Find(predicate);
            }
        }

        public void Delete(C3Plugin value)
        {
            using (var db = new LiteDatabase(Database))
            {
                var collection = db.GetCollection<C3Plugin>(Collection);
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
