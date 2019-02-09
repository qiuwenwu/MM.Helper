using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MM.Helper.Data
{
    /// <summary>
    /// Sqlite数据库帮助类
    /// </summary>
    public class SQLite : ISql {
        #region 属性
        /// <summary>
        /// sqlite连接字符串
        /// </summary>
        public static string connStr_default = string.Format("Data Source={0};Version=3;", "./db.sqlite".ToFullName());
        /// <summary>
        /// sqlite连接字符串
        /// </summary>
        public string ConnStr { get; set; } = connStr_default;

        /// <summary>
        /// 路径
        /// </summary>
        public string Dir { get; set; } = "";

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        public string Database
        {
            get
            {
                string name = null;
                if (!string.IsNullOrEmpty(ConnStr))
                {
                    var arr = ConnStr.Split(';');
                    for (var i = 0; i < arr.Length; i++)
                    {
                        var o = arr[i].ToLower();
                        if (o.Contains("database"))
                        {
                            name = o.Split('=')[1];
                            break;
                        }
                    }
                }
                return name;
            }
        }

        private SQLiteConnection conn;

        /// <summary>
        /// 操作的数据表
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 单页显示条数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        public string Sql { get; set; } = "";

        /// <summary>
        /// 错误提示
        /// </summary>
        public string Ex { get; private set; }
        #endregion


        #region 配置
        /// <summary>
        /// 获取错误提示
        /// </summary>
        /// <returns>返回错误提示</returns>
        public string GetEx() {
            return Ex;
        }

        /// <summary>
        /// 获取当前的SQL语句
        /// </summary>
        /// <returns>返回SQL语句</returns>
        public string GetSql()
        {
            return Sql;
        }

        /// <summary>
        /// 获取数据库名
        /// </summary>
        /// <returns>返回数据库名</returns>
        public string GetDB()
        {
            return Database;
        }

        /// <summary>
        /// 设置分页大小——每页显示的数量
        /// </summary>
        /// <param name="size">数量</param>
        public void SetSize(int size)
        {
            Size = size;
        }

        /// <summary>
        /// 设置查询页 —— 分页后的结果第几页
        /// </summary>
        /// <param name="page">当前页</param>
        public void SetPage(int page)
        {
            Page = page;
        }

        /// <summary>
        /// 设置查询表
        /// </summary>
        /// <param name="table">查询表</param>
        public void SetTable(string table)
        {
            Table = table;
        }
        #endregion


        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">数据库文件路径</param>
        public SQLite(string db = null)
        {
            Init(db);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="db">数据库文件路径</param>
        public void Init(string db = null)
        {
            if (conn == null)
            {
                Link(db);
            }
        }

        /// <summary>
        /// 开启数据库连接
        /// </summary>
        public bool Open()
        {
            var bl = false;
            if (conn != null)
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    bl = true;
                }
                catch (Exception ex)
                {
                    Ex = ex.Message;
                    bl = false;
                }
            }
            return bl;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="db">数据库文件路径</param>
        public void Link(string db = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(db))
                {
                    if (db.Contains("Source"))
                    {
                        ConnStr = db;
                    }
                    else
                    {
                        db = db.ToFullName(Dir);
                        ConnStr = string.Format("Data Source={0};Version=3;", db);
                    }
                }
                conn = new SQLiteConnection(ConnStr);
                Close();
            }
            catch (Exception ex)
            {
                Ex = ex.Message;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 判断数据库是否连接
        /// </summary>
        /// <returns>连接成功返回true，失败返回false</returns>
        public bool CheckLink()
        {
            try
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Ex = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
            conn = null;
        }

        /// <summary>
        /// 结束函数
        /// </summary>
        ~SQLite()
        {
            Dispose();
        }
        #endregion


        #region 原始
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回执行成功条数</returns>
        public int Execute(string sql)
        {
            var n = 0;
            try
            {
                if (Open())
                {
                    n = conn.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                Sql = sql;
                Ex = ex.Message;
            }
            finally
            {
                Close();
            }
            return n;
        }

        /// <summary>
        /// 增删改——异步
        /// </summary>
        /// <param name="sql">sql语句</param>
        public Task<int> ExecuteAsync(string sql)
        {
            Task<int> task = null;
            if (Open())
            {
                task = conn.ExecuteAsync(sql);
            }
            return task;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回查询结果字符串</returns>
        public Task<IEnumerable<dynamic>> QueryAsync(string sql)
        {
            Task<IEnumerable<dynamic>> task = null;
            if (Open())
            {
                task = conn.QueryAsync(sql);
            }
            return task;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回查询结果字符串</returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql)
        {
            Task<IEnumerable<T>> task = null;
            if (Open())
            {
                task = conn.QueryAsync<T>(sql);
            }
            return task;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回项目集合</returns>
        public List<dynamic> Query(string sql)
        {
            var list = new List<dynamic>();
            try
            {
                if (Open())
                {
                    var m = conn.Query(sql);
                    list = m.ToList();
                }
            }
            catch (Exception ex)
            {
                Sql = sql;
                Ex = ex.Message;
            }
            finally
            {
                Close();
            }
            return list;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回项目</returns>
        public dynamic QueryFirst(string sql)
        {
            dynamic m = new { };
            try
            {
                if (Open())
                {
                    m = conn.QueryFirstOrDefault(sql);
                }
            }
            catch (Exception ex)
            {
                Sql = sql;
                Ex = ex.Message;
            }
            finally
            {
                Close();
            }
            return m;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回模型集合结果</returns>
        public List<T> Query<T>(string sql)
        {
            var list = new List<T>();
            try
            {
                if (Open())
                {
                    var m = conn.Query<T>(sql);
                    list = m.ToList();
                }
            }
            catch (Exception ex)
            {
                Sql = sql;
                Ex = ex.Message;
            }
            finally
            {
                Close();
            }
            return list;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回模型列表</returns>
        public T QueryFirst<T>(string sql)
        {
            var obj = default(T);
            try
            {
                if (Open())
                {
                    obj = conn.QueryFirstOrDefault<T>(sql);
                }
            }
            catch (Exception ex)
            {
                Sql = sql;
                Ex = ex.Message;
            }
            finally
            {
                Close();
            }
            return obj;
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回项目集合</returns>
        public string QueryStr(string sql)
        {
            return Query(sql).ToJson();
        }

        /// <summary>
        /// 执行SQL语句-查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回项目</returns>
        public string QueryFirstStr(string sql)
        {
            return QueryFirst(sql).ToJson();
        }
        #endregion


        #region 弱类型
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回列表查询结果</returns>
        public List<dynamic> Get(string whereStr, string field = "*", string orderBy = "")
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = " ORDER BY " + orderBy;
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("SELECT {0} FROM {1}{2}{3}", field, Table, whereStr, orderBy);
            if (Size != 0 && Page != 0)
            {
                int start = Size * (Page - 1);
                sql = string.Format("{0} limit {1},{2}", sql, start, Size);
            }
            return Query(sql);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回单条查询结果</returns>
        public dynamic GetFirst(string whereStr, string field = "*", string orderBy = "")
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = " ORDER BY " + orderBy;
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("SELECT {0} FROM {1}{2}{3}", field, Table, whereStr, orderBy);
            if (Size != 0 && Page != 0)
            {
                int start = Size * (Page - 1);
                sql = string.Format("{0} limit {1},{2}", sql, start, Size);
            }
            return QueryFirst(sql);
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回列表查询结果</returns>
        public List<dynamic> GetAll(string orderBy = "", string field = "*")
        {
            if (orderBy != "")
            {
                orderBy = " ORDER BY " + orderBy;
            }
            string sql = string.Format("SELECT {0} FROM {1}{2}", field, Table, orderBy);
            if (Size != 0 && Page != 0)
            {
                int start = Size * (Page - 1);
                sql = string.Format("{0} limit {1},{2}", sql, start, Size);
            }
            return Query(sql);
        }

        /// <summary>
        /// 正则查询
        /// </summary>
        /// <param name="whereField">字段</param>
        /// <param name="match">正则</param>
        /// <param name="field">获取的字段 *号表示获取全部字段</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回Json格式字符串</returns>
        public dynamic Regexp(string whereField, string match, string field = "*", string orderBy = "")
        {
            if (orderBy != "")
            {
                orderBy = " ORDER BY " + orderBy;
            }
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} REGEXP {3} {4};", field, Table, whereField, match, orderBy);
            return Query(sql);
        }

        /// <summary>
        /// 统计记录个数
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">统计的字段</param>
        /// <returns>返回统计结果</returns>
        public List<dynamic> Count(string whereStr, string field = null)
        {
            string countField = "count(1) AS counts";
            if (!string.IsNullOrEmpty(field))
            {
                countField = field + "," + countField;
                field = string.Format(" GROUP BY {0}", field);
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = "WHERE " + whereStr;
            }
            string sql = string.Format("SELECT {0} FROM `{1}` {2}{3}", countField, Table, whereStr, field);
            return Query(sql);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回Json格式查询结果</returns>
        public string GetStr(string whereStr, string field = "*", string orderBy = "")
        {
            return Get(whereStr, field, orderBy).ToJson();
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回Json格式查询结果</returns>
        public string GetFirstStr(string whereStr, string field = "*", string orderBy = "")
        {
            object obj = GetFirst(whereStr, field, orderBy);
            return obj.ToJson();
        }
        #endregion


        #region 泛型
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回模型结果</returns>
        public T GetFirst<T>(string whereStr, string field = "*", string orderBy = "")
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = " ORDER BY " + orderBy;
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("SELECT {0} FROM `{1}`{2}{3}", field, Table, whereStr, orderBy);
            if (Size != 0 && Page != 0)
            {
                int start = Size * (Page - 1);
                sql = string.Format("{0} limit {1},{2}", sql, start, Size);
            }
            return QueryFirst<T>(sql);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回模型集合结果</returns>
        public List<T> Get<T>(string whereStr, string field = "*", string orderBy = "")
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = " ORDER BY " + orderBy;
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("SELECT {0} FROM `{1}`{2}{3}", field, Table, whereStr, orderBy);
            if (Size != 0 && Page != 0)
            {
                int start = Size * (Page - 1);
                sql = string.Format("{0} limit {1},{2}", sql, start, Size);
            }
            return Query<T>(sql);
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回模型集合</returns>
        public List<T> GetAll<T>(string orderBy = "", string field = "*")
        {
            if (orderBy != "")
            {
                orderBy = " ORDER BY " + orderBy;
            }
            string sql = string.Format("SELECT {0} FROM `{1}`{2}", field, Table, orderBy);
            if (Size != 0 && Page != 0)
            {
                int start = Size * (Page - 1);
                sql = string.Format("{0} limit {1},{2}", sql, start, Size);
            }
            return Query<T>(sql);
        }
        #endregion


        #region 常用增删改操作
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="personStr">字段名</param>
        /// <param name="valueStr">对应字段值</param>
        /// <returns>添加成功返回true,失败返回false</returns>
        public int Add(string personStr, string valueStr)
        {
            string sql = string.Format("INSERT INTO `{0}` ({1}) VALUES ({2});", Table, personStr, valueStr);
            return Execute(sql);
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="setStr">修改项</param>
        /// <returns>修改成功返回true,失败返回false</returns>
        public int Set(string whereStr, string setStr)
        {
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("UPDATE `{0}` SET {1}{2};", Table, setStr, whereStr);
            return Execute(sql);
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <param name="setStr">修改项</param>
        /// <returns>修改成功返回true,失败返回false</returns>
        public Task<int> SetAsync(string whereStr, string setStr)
        {
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("UPDATE `{0}` SET {1}{2};", Table, setStr, whereStr);
            return ExecuteAsync(sql);
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="whereStr">查询条件</param>
        /// <returns>删除成功返回true,失败返回false</returns>
        public int Del(string whereStr)
        {
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = " WHERE " + whereStr;
            }
            string sql = string.Format("DELETE FROM `{0}`{1};", Table, whereStr);
            return Execute(sql);
        }

        /// <summary>
        /// 删除重复记录并留唯一
        /// </summary>
        /// <param name="whereStr">删除的条件语句</param>
        /// <param name="field">用作判断的字段</param>
        /// <param name="IDfield">ID字段</param>
        /// <param name="keep">保留方式，max或min</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public int DelAsOne(string whereStr, string field, string IDfield, string keep = "max")
        {
            string sql = string.Format("DELETE FROM `{0}` WHERE `{3}` NOT IN (SELECT minid FROM (SELECT {4}(`{3}`) AS minid FROM `{0}` GROUP BY {1}) b) AND {2};", Table, field, whereStr, IDfield, keep);
            return Execute(sql);
        }

        /// <summary>
        /// 增加或更新
        /// </summary>
        /// <param name="personStr">字段名,需带主键</param>
        /// <param name="valueStr">对应字段值</param>
        /// <param name="setStr"></param>
        /// <returns>添加成功返回true,失败返回false</returns>
        public int AddOrSet(string personStr, string valueStr, string setStr)
        {
            string sql = string.Format("INSERT INTO {0}({1}) VALUES({2}) ON DUPLICATE KEY UPDATE {3}", Table, personStr, valueStr, setStr);
            return Execute(sql);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="whereStr">条件</param>
        /// <param name="setStr">设置值</param>
        /// <returns>成功返回ture，失败返回false</returns>
        public int AddOrSet(string whereStr, string setStr)
        {
            if (string.IsNullOrEmpty(whereStr))
            {
                return -1;
            }
            var str = Get(whereStr);
            if (str == null)
            {
                string[] strArr = whereStr.Split('=');
                if (strArr == null)
                {
                    return -1;
                }
                if (strArr.Length != 2)
                {
                    return -1;
                }
                else
                {
                    string personStr = strArr[0];
                    string valueStr = strArr[1];
                    Add(personStr, valueStr);
                }
            }
            return Set(whereStr, setStr);
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        public int AddDB(string db = null)
        {
            if (string.IsNullOrEmpty(db))
            {
                db = connStr_default;
            }
            SQLiteConnection.CreateFile(db.ToFullName(Dir));
            return 1;
        }
        #endregion


        #region 操作表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="fieldAndType">字段名加字段类型 例如 number INT(11), 需设主键时在前面加Key</param>
        /// <param name="table">表名</param>
        /// <returns>创建成功返回ture，失败返回false</returns>
        public int AddTable(string fieldAndType, string table = null)
        {
            if (table != null)
            {
                Table = table;
            }
            string sql = string.Format("CREATE TABLE IF NOT EXISTS `{0}` ({1}); ", Table, fieldAndType);
            return Execute(sql);
        }

        /// <summary>
        /// 创建表带自动递增主键
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="table">表名</param>
        /// <returns>创建成功返回true，失败返回false</returns>
        public int AddTableKey(string key, string table = null)
        {
            return AddTable(key + " INTEGER PRIMARY KEY AUTOINCREMENT", table);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>创建成功返回ture，失败返回false</returns>
        public int DelTable(string table = null)
        {
            if (table != null)
            {
                Table = table;
            }
            string sql = string.Format("DROP TABLE IF EXISTS `{0}`; ", Table);
            return Execute(sql);
        }

        /// <summary>
        /// 查询表项数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="filed">字段</param>
        /// <returns>返回表项数值</returns>
        public int CountRow(string tableName = null, string filed = "*")
        {
            var n = -1;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = Table;
            }
            try
            {
                string sql = string.Format("SELECT {0} FROM {1}", filed, tableName);
                n = conn.Query(sql).Count();
            }
            catch (Exception ex)
            {
                Ex = ex.Message;
            }
            return n;
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="col">列值</param>
        /// <returns>添加成功返回true，失败返回false</returns>
        public int AddCol(string col)
        {
            var sql = string.Format("ALTER TABLE `{0}` ADD COLUMN {1};", Table, col);
            return Execute(sql);
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="name">列名称</param>
        /// <param name="type">类型</param>
        /// <param name="len">长度</param>
        /// <returns>添加成功返回true，失败返回false</returns>
        public int AddCol(string name, string type, int len = 0)
        {
            var defaultVal = "";
            switch (type.ToLower())
            {
                case "datetime":
                    defaultVal = " default '1970-01-01 00:00:00'";
                    break;
                case "string":
                case "varchar":
                    defaultVal = " default ''";
                    break;
                case "date":
                    defaultVal = " default '1970-01-01'";
                    break;
                case "float":
                case "double":
                case "int":
                    defaultVal = " default 0";
                    break;
                case "boolean":
                    defaultVal = " default false";
                    break;
            }
            var col = "";
            if (len > 0)
            {
                col = string.Format("`{0}` {1}({2}){3}", name, type, len, defaultVal);
            }
            else
            {
                col = string.Format("`{0}` {1}{2}", name, type, defaultVal);
            }
            return AddCol(col);
        }

        /// <summary>
        /// 列名
        /// </summary>
        /// <param name="name">删除列</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public int DelCol(string name)
        {
            var sql = string.Format("ALTER TABLE `{0}` DROP COLUMN `{1}`;", Table, name);
            return Execute(sql);
        }
        #endregion
    }
}
