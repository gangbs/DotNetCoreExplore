using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Infrastruct.Redis
{
    public class RedisHashCache : RedisBase
    {
        public RedisHashCache(int dbnum, string prefix, string connectionString = null) : base(dbnum, prefix, connectionString)
        {
        }

        public string HashGet(string key, string field)
        {
            key = this.GenRealKey(key);
            var val = this.DbHandler(db => db.HashGet(key, field));
            return val;
        }

        public T HashGet<T>(string key, string field)
        {
            key = this.GenRealKey(key);
            var val = this.DbHandler(db => db.HashGet(key, field));
            return this.ConvertObj<T>(val);
        }        

        public List<HashEntry> HashGetAll(string key)
        {
            var dic = new Dictionary<string, object>();
            key = this.GenRealKey(key);
            return this.DbHandler(db => db.HashGetAll(key)).ToList();
        }

        public List<string> AllField(string key)
        {
            key = this.GenRealKey(key);
            var fields = this.DbHandler(db => db.HashKeys(key));
            var lstField = (from f in fields
                            select f.ToString()).ToList();
            return lstField;
        }

        public long FieldCount(string key)
        {
            key = this.GenRealKey(key);
            return this.DbHandler(db => db.HashLength(key));
        }

        public bool HashSet<T>(string key, string field, T obj)
        {
            key = this.GenRealKey(key);
            string json = ConvertJson<T>(obj);
            return this.DbHandler(db => db.HashSet(key, field, json));
        }       

        public void HashSet(string key, HashEntry[] arr)
        {
            key = this.GenRealKey(key);
            this.DbHandler(db => db.HashSet(key, arr));
        }

        public bool HashExist(string key, string field)
        {
            return this.DbHandler(db => db.HashExists(key, field));
        }

        public bool HashDelete(string key, string field)
        {
            return this.DbHandler(db => db.HashDelete(key, field));
        }

        public long HashDelete(string key, IEnumerable<string> fields)
        {
            var hashFields = from f in fields
                             select (RedisValue)f;
            return this.DbHandler<long>(db => db.HashDelete(key, hashFields.ToArray()));
        }


        #region 异步

        public async Task<string> HashGetAsync(string key, string field)
        {
            key = this.GenRealKey(key);
            var val = await this.DbHandler(db => db.HashGetAsync(key, field));
            return val;
        }

        public async Task<T> HashGetAsync<T>(string key, string field)
        {
            key = this.GenRealKey(key);
            var val = await this.DbHandler(db => db.HashGetAsync(key, field));
            return this.ConvertObj<T>(val);
        }

        public async Task< List<HashEntry>> HashGetAllAsync(string key)
        {
            var dic = new Dictionary<string, object>();
            key = this.GenRealKey(key);
            var arr= await this.DbHandler(db => db.HashGetAllAsync(key));
            return arr.ToList();
        }

        public async Task<List<string>> AllFieldAsync(string key)
        {
            key = this.GenRealKey(key);
            var fields = await this.DbHandler(db => db.HashKeysAsync(key));
            var lstField = (from f in fields
                            select f.ToString()).ToList();
            return lstField;
        }

        public async Task<long>  FieldCountAsync(string key)
        {
            key = this.GenRealKey(key);
            return await this.DbHandler(db => db.HashLengthAsync(key));
        }

        public async Task<bool> HashSetAsync<T>(string key, string field, T obj)
        {
            key = this.GenRealKey(key);
            string json = ConvertJson<T>(obj);
            return await this.DbHandler(db => db.HashSetAsync(key, field, json));
        }

        public async Task HashSetAsync(string key, HashEntry[] arr)
        {
            key = this.GenRealKey(key);
            await this.DbHandler(db => db.HashSetAsync(key, arr));
        }

        public async Task<bool> HashExistsAsync(string key, string field)
        {
            return await this.DbHandler(db => db.HashExistsAsync(key, field));
        }

        public async Task<bool> HashDeleteAsync(string key, string field)
        {
            return await this.DbHandler(db => db.HashDeleteAsync(key, field));
        }

        public async Task<long> HashDeleteAsync(string key, IEnumerable<string> fields)
        {
            var hashFields = from f in fields
                             select (RedisValue)f;
            return await this.DbHandler<Task<long>>(db => db.HashDeleteAsync(key, hashFields.ToArray()));
        }

        #endregion

    }
}
