using Kingsun.Framework.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SqlSugar;
using Kingsun.Core.Utils;

namespace Kingsun.Framework.Manager
{
    public class SqlSugarManager
    {
        public string OperateError = "";
        public SqlSugarManager() { }
        SqlsugarRepository repository = new SqlsugarRepository();

        public bool Delete<T>(T entity) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                int i = db.Deleteable<T>().Where(entity).ExecuteCommand();
                OperateError = repository._operatorError;
                return i > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public bool Delete<T>(object id) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                int i = db.Deleteable<T>(id).ExecuteCommand();
                OperateError = repository._operatorError;
                return i > 0;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">主键编号数组</param>
        /// <returns></returns>
        public bool Delete<T>(object[] ids) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                int i = db.Deleteable<T>(ids).ExecuteCommand();
                OperateError = repository._operatorError;
                return i > 0;
            }
        }



        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(object id) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                T entity = db.Queryable<T>().InSingle(id);
                OperateError = repository._operatorError;
                return entity;
            }
        }

        public T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> expr) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                T entity = db.Queryable<T>().Where(expr).First();
                OperateError = repository._operatorError;
                return entity;
            }
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> expr) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                List<T> list = db.Queryable<T>().Where(expr).ToList();
                OperateError = repository._operatorError;
                return list;
            }
        }

        public List<T> GetList<T>(List<Expression<Func<T, bool>>> exprs) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                var queryable = db.Queryable<T>();
                if (exprs != null)
                {
                    foreach (var where in exprs)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                List<T> list = queryable.ToList();
                OperateError = repository._operatorError;
                return list;
            }
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                int i = db.Insertable<T>(entity).ExecuteCommand();
                OperateError = repository._operatorError;
                return i;
            }
        }

        public bool Update<T>(T entity) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                int i = db.Updateable<T>(entity).ExecuteCommand();
                OperateError = repository._operatorError;
                return i > 0;
            }
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <typeparam name="T">更新的实体</typeparam>
        /// <param name="entity">更新的对象</param>
        /// <param name="expr">需要更新的字段</param>
        /// <returns></returns>
        public bool Update<T>(T entity, Expression<Func<T, object>> expr) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                int i = db.Updateable(entity).UpdateColumns(expr).ExecuteCommand();
                OperateError = repository._operatorError;
                return i > 0;
            }
        }

        public List<T> GetAll<T>() where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                return db.Queryable<T>().ToList();
            }

        }



        public List<T> GetPageList<T>(int pageIndex, int pageSize,
            Expression<Func<T, object>> orderby, int orderType,
            Expression<Func<T, bool>> where, ref int totalCount) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                List<T> list = db.Queryable<T>().OrderBy(orderby, (OrderByType)orderType).Where(where).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                return list;
            }
        }

        public List<T> GetPageList<T>(int pageIndex, int pageSize, string orderBy, Expression<Func<T, bool>> where, ref int totalCount) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                List<T> list = db.Queryable<T>().OrderBy(orderBy).Where(where).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                return list;
            }
        }

        public List<T> GetPageList<T>(int pageIndex, int pageSize, string orderBy, string WhereStr, Expression<Func<T, bool>> where, ref int totalCount) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                List<T> list = db.Queryable<T>().OrderBy(orderBy).Where(WhereStr).Where(where).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                return list;
            }
        }

        public List<T> GetPageList<T>(int pageIndex, int pageSize, string orderBy, string WhereStr, List<Expression<Func<T, bool>>> wheres, ref int totalCount) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                var queryable = db.Queryable<T>();
                queryable = queryable.OrderBy(orderBy);
                queryable = queryable.Where(WhereStr);
                if (wheres.Count > 0)
                {
                    foreach (Expression<Func<T, bool>> exp in wheres)
                    {
                        queryable = queryable.Where(exp);
                    }
                }
                List<T> list = queryable.ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                return list;
            }
        }

        public List<T> GetPageList<T>(PageParams<T> param, ref int totalCount) where T : class, new()
        {
            using (var db = repository.GetInstance())
            {
                var queryable = db.Queryable<T>();
                if (!string.IsNullOrEmpty(param.WhereSql))
                {
                    queryable = queryable.Where(param.WhereSql);
                }
                if (param.Wheres != null)
                {
                    foreach (var where in param.Wheres)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                if (param.Where != null)
                {
                    queryable = queryable.Where(param.Where);
                }
                if (param.OrderColumns == null && string.IsNullOrEmpty(param.StrOrderColumns))
                {
                    throw new Exception("分页必须要排序。");
                }

                if (!string.IsNullOrEmpty(param.StrOrderColumns))
                {
                    queryable = queryable.OrderBy(param.StrOrderColumns);
                }
                if (param.OrderColumns != null)
                {
                    queryable = queryable.OrderBy(param.OrderColumns, (OrderByType)param.OrderType);
                }
                var key = queryable.ToSql();
                List<T> list = queryable.ToPageList(param.PageIndex, param.PageSize, ref totalCount).ToList();
                return list;
            }
        }

        public bool ExecuteSql(string sqlstr)
        {
            using (var db = repository.GetInstance())
            {
                return db.Ado.ExecuteCommand(sqlstr) > 0;
            }
        }
    }
}
