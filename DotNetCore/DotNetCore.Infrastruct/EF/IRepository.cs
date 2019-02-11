using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
   public interface IRepository<TEntity, TPrimaryKey> where TEntity: class,IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> Table { get; }

        #region 查询

        TEntity Get(TPrimaryKey key);

        TEntity Get(Expression<Func<TEntity, bool>> fliter);

        bool TryGet(Expression<Func<TEntity, bool>> fliter, out TEntity entity);

        IQueryable<TEntity> GetAllIncluding<TProperty>(Expression<Func<TEntity, TProperty>> propertySelectors);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> GetList(string sql, params object[] parameters);

        IEnumerable<TReturn> GetList<TReturn>(string sql, params object[] parameters) where TReturn : class;

        IEnumerable<TEntity> GetPaging<K>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, K>> orderFiled, int pageSize, int pageNum, out int count, bool isAsc = true);

        #endregion

        #region 增加

        SaveResult Insert(TEntity entity, bool isSaveChange = true);

        SaveResult InsertMany(IEnumerable<TEntity> lst, bool isSaveChange = true);

        #endregion

        #region 编辑       

        SaveResult Update(TEntity entity, bool isSaveChange = true);

        SaveResult Update(TPrimaryKey key, Action<TEntity> change, bool isSaveChange = true);

        SaveResult UpdateProperty(Expression<Func<TEntity, bool>> filter, string filedName, object filedValue, bool isSaveChange = true);

        SaveResult UpdatePropertys(Expression<Func<TEntity, bool>> filter, Dictionary<string, object> fileds, bool isSaveChange = true);

        SaveResult UpdatePropertys(Expression<Func<TEntity, bool>> filter, Action<TEntity> change, bool isSaveChange = true);

        #endregion

        #region 删除

        SaveResult Delete(TPrimaryKey key, bool isSaveChange = true);

        SaveResult Delete(TEntity Entity, bool isSaveChange = true);

        SaveResult Delete(Expression<Func<TEntity, bool>> filter, bool isSaveChange = true);

        #endregion

        #region 其它

        SaveResult ExcuteSqlCommand(string sql, object[] parameters);

        bool IsExist(Expression<Func<TEntity, bool>> filter);

        int Count(Expression<Func<TEntity, bool>> filter);

        #endregion
    }
}
