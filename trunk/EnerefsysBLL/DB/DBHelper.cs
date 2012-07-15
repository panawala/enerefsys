using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;

namespace EnerefsysBLL.DB
{
    public class DBHelper
    {
        public static void Update(string updateStr)
        {
            //创建一个数据库文件
            string datasource = "d:/test.db3";
            //连接数据库
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection();
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
            conn.ConnectionString = connstr.ToString();
            conn.Open();
        }


        #region 数据库的连接属性
        private static SQLiteConnection scon;
        public static SQLiteConnection Connect
        {
            get
            {
                if (scon == null)
                {
                    string con = "Data Source=d:/test.db3;Password=admin;Pooling=true;FailIfMissing=false";
                    scon = new SQLiteConnection(con);
                    scon.Open();
                }
                else if (scon.State == ConnectionState.Broken)
                {
                    scon.Close();
                    scon.Open();
                }
                else if (scon.State == ConnectionState.Closed)
                {
                    scon.Open();
                }
                return scon;
            }
        }
        #endregion

        #region  执行增删改的操作
        /// <summary>
        /// 执行不带参数的增删改的操作
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>返回影响的行数</returns>
        public static int ExecuteNonQuery(string sql)
        {
            int row = 0;
            try
            {
                SQLiteCommand scmd = new SQLiteCommand(sql, Connect);
                row = scmd.ExecuteNonQuery();
            }
            catch (Exception ce)
            {
                Console.WriteLine("增删改失败" + ce.Message);
            }
            return row;
        }
        #endregion
        #region  执行带参数的增删改的操作
        /// <summary>
        /// 执行带参数的增删改的操作
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sp">要传入的参数列表</param>
        /// <returns>返回影响行数</returns>
        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] sp)
        {
            int row = 0;
            try
            {
                SQLiteCommand scmd = new SQLiteCommand(sql, Connect);
                scmd.Parameters.AddRange(sp);
                row = scmd.ExecuteNonQuery();
            }
            catch (Exception ce)
            {
                Console.WriteLine("增删改失败" + ce.Message);
            }
            return row;
        }
        #endregion
        #region 执行返回只进只读的查询语句
        public static SQLiteDataReader GetReader(string sql)
        {
            SQLiteCommand scmd = new SQLiteCommand(sql, Connect);
            return scmd.ExecuteReader();
        }
        #endregion
        #region 执行带参数返回只进只读的查询语句
        public static SQLiteDataReader GetReader(string sql, params SQLiteParameter[] sp)
        {
            SQLiteCommand scmd = new SQLiteCommand(sql, Connect);
            scmd.Parameters.AddRange(sp);
            return scmd.ExecuteReader();
        }
        #endregion
        #region 执行返回首行首列的查询结果
        public static object ExecuteScalar(string sql)
        {
            SQLiteCommand scmd = new SQLiteCommand(sql);
            return scmd.ExecuteScalar();
        }
        #endregion
        #region 执行带参数的返回首行首列的查询结果
        public static object ExecuteScalar(string sql, params SQLiteParameter[] sp)
        {
            SQLiteCommand scmd = new SQLiteCommand(sql);
            scmd.Parameters.AddRange(sp);
            return scmd.ExecuteScalar();
        }
        #endregion
        #region 执行返回数据表的查询结果
        public static DataTable GetTable(string sql)
        {
            SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, Connect);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds.Tables[0];
        }
        #endregion
        #region 执行带参数的返回数据表的查询结果
        public static DataTable GetTable(string sql, params SQLiteParameter[] sp)
        {
            SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, Connect);
            sda.SelectCommand.Parameters.AddRange(sp);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds.Tables[0];
        }
        #endregion
    }
}
