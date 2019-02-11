using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Table
        {
            get
            {
                return this._dbSet.AsNoTracking();
            }
        }

        #region 查询

        public TEntity Get(TPrimaryKey key)
        {
            TEntity obj = _dbSet.Find(key);

            if (obj != null)
            {
                _context.Entry<TEntity>(obj).State = EntityState.Detached;
            }

            return obj;
        }
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Where(filter).AsNoTracking().FirstOrDefault();
        }
        public bool TryGet(Expression<Func<TEntity, bool>> filter, out TEntity entity)
        {
            entity = this.Get(filter);
            return entity != null ? true : false;
        }
        public IQueryable<TEntity> GetAllIncluding<TProperty>(Expression<Func<TEntity, TProperty>> propertySelectors)
        {
            return this._dbSet.Include<TEntity, TProperty>(propertySelectors);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return this._dbSet.AsNoTracking();
        }
        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Where(filter).AsNoTracking();
        }
        public IEnumerable<TEntity> GetList(string sql, params object[] parameters)
        {
            return this._dbSet.FromSql(sql, parameters).AsNoTracking();
        }
        public IEnumerable<TReturn> GetList<TReturn>(string sql, params object[] parameters)where TReturn:class
        {
           return this._context.Query<TReturn>().FromSql(sql,parameters).AsEnumerable();
            //return this._context.Database.<TReturn>(sql, parameters);
        }
        public IEnumerable<TEntity> GetPaging<K>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, K>> orderFiled, int pageSize, int pageNum, out int count, bool isAsc = true)
        {
            count = _dbSet.Count(filter);
            IEnumerable<TEntity> lstReturn;

            if (isAsc)
            {
                lstReturn = _dbSet.Where(filter).OrderBy(orderFiled).Skip(pageSize * (pageNum - 1)).Take(pageSize).AsNoTracking();
            }
            else
            {
                lstReturn = _dbSet.Where(filter).OrderByDescending(orderFiled).Skip(pageSize * (pageNum - 1)).Take(pageSize).AsNoTracking();
            }
            return lstReturn;
        }

        #endregion

        #region 增加

        public SaveResult Insert(TEntity entity, bool isSaveChange = true)
        {
            ////第一种方法
            //_dbSet.Attach(entity);
            //_context.Entry<TEntity>(entity).State = EntityState.Added;

            //第二种方法
            _dbSet.Add(entity); //EntityState.Detached

            return isSaveChange ? this.Save() : null;
        }
        public SaveResult InsertMany(IEnumerable<TEntity> lst, bool isSaveChange = true)
        {
            _dbSet.AddRange(lst);

            return isSaveChange ? this.Save() : null;
        }
        

        #endregion

        #region 编辑

        public SaveResult Update(TEntity entity, bool isSaveChange = true)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return isSaveChange ? this.Save() : null;
        }

        public SaveResult Update(TPrimaryKey key, Action<TEntity> change, bool isSaveChange = true)
        {
            throw new NotImplementedException();
        }

        public SaveResult UpdateProperty(Expression<Func<TEntity, bool>> filter, string filedName, object filedValue, bool isSaveChange = true)
        {
            var lstEntity = this._dbSet.Where(filter);
            try
            {
                foreach (var entity in lstEntity)
                {
                    typeof(TEntity).GetProperty(filedName).SetValue(entity, filedValue);
                }
            }
            catch
            {
                return null;
            }
            return isSaveChange ? this.Save() : null;
        }

        public SaveResult UpdatePropertys(Expression<Func<TEntity, bool>> filter, Dictionary<string, object> fileds, bool isSaveChange = true)
        {
            var lstEntity = this._dbSet.Where(filter);
            try
            {
                foreach (var entity in lstEntity)
                {
                    foreach (KeyValuePair<string, object> kv in fileds)
                    {
                        typeof(TEntity).GetProperty(kv.Key).SetValue(entity, kv.Value);
                    }
                }
            }
            catch
            {
                return null;
            }
            return isSaveChange ? this.Save() : null;
        }

        public SaveResult UpdatePropertys(Expression<Func<TEntity, bool>> filter, Action<TEntity> change, bool isSaveChange = true)
        {
            var lstEntity = this._dbSet.Where(filter);
            foreach (var entity in lstEntity)
            {
                change(entity);
            }
            return isSaveChange ? this.Save() : null;
        }

        #endregion

        #region 删除

        public SaveResult Delete(TPrimaryKey key,bool isSaveChange = true)
        {
            TEntity entity = this._dbSet.Find(key);
            return this.Delete(entity, isSaveChange);
        }

        public SaveResult Delete(TEntity entity, bool isSaveChange = true)
        {
            //_dbSet.Remove(entity);
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            return isSaveChange ? this.Save() : null;
        }

        public SaveResult Delete(Expression<Func<TEntity, bool>> filter, bool isSaveChange = true)
        {
            var lst = this._dbSet.Where(filter);
            _dbSet.RemoveRange(lst);
            return isSaveChange ? this.Save() : null;
        }

        #endregion

        #region 保存变更

        protected SaveResult Save()
        {
            SaveResult r;
            try
            {
                int count = _context.SaveChanges();
                r = new SaveResult { Success = true, Rows = count };
            }
            catch (Exception exp)
            {
                r = new SaveResult { Success = false, Message = exp.Message };
            }
            return r;
        }

        #endregion

        #region 其它

        public SaveResult ExcuteSqlCommand(string sql, object[] parameters)
        {
            SaveResult r;
            try
            {
                int count = this._context.Database.ExecuteSqlCommand(sql, parameters);
                r = new SaveResult { Success = true, Rows = count };
            }
            catch (Exception exp)
            {
                r = new SaveResult { Success = false, Message = exp.Message };
            }
            return r;
        }

        public bool IsExist(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Any(filter);
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return this._dbSet.Count(filter);
        }

        #endregion
    }
}
