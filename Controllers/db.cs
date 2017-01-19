using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MVCtutorial.Controllers
{
    //Class for MSSQL db
    public class db
    {
        // Reference to db on which is open connection
        public int dbIdx;
        //Only shared connection
        private SqlConnection conn;
        public NpgsqlConnection connection;
        /// <summary>
        /// MSSQL constructor
        /// </summary>
        public db() {
            string ConnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn = new SqlConnection(ConnStr);
        }
        /// <summary>
        /// Constructor to establish db connection on PostgreSQL database
        /// </summary>
        /// <param name="adb">database name</param>
        public db(string aDB, int dataserverNumber, int dbIndex)
        {
            string db = aDB;
            dbIdx = dbIndex;
            string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
              "192.168.2." + dataserverNumber, 5432, "postgres", "Nordit0276", db);
            connection = new NpgsqlConnection(connstring);
        }

        /// <summary>
        /// Method to get one single element as an object
        /// </summary>
        /// <param name="column">columnn SQL to select</param>
        /// <param name="table">table SQL to select</param>
        /// <param name="where">where statement to specify select</param>
        /// <returns>Data in object</returns>
        public object singleItemSelect(string column, string table, string where=null) {
            conn.Open();

            object result = new object();

            if (where == null)
            {
                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table, conn);
                string sql = string.Format("SELECT {0} FROM {1}", column, table);
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }
            else
            {
                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table +" WHERE " + where, conn);
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2}", column, table, where);
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }
            
            return result;
        }

        /// <summary>
        /// Method to get multiple elements as an list of objects
        /// </summary>
        /// <param name="column">columnn SQL to select</param>
        /// <param name="table">table SQL to select</param>
        /// <param name="where">where statement to specify select</param>
        /// <returns>Data in list of objects</returns>
        public List<object> multipleItemSelect(string column, string table, string where = null)
        {
            conn.Open();
            List<object> result = new List<object>();

            if (where == null)
            {
                string sql = string.Format("SELECT {0} FROM {1}", column, table);
                SqlCommand cmd = new SqlCommand(sql, conn);

                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM " + table, conn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        result.Add(r[i]);
                    }
                }
            }
            else
            {
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2}", column, table, where);
                SqlCommand cmd = new SqlCommand(sql,conn);
                //SqlCommand cmd = new SqlCommand("SELECT "+ column +" FROM " + table + " WHERE bakeryId = @where" , conn);
                //cmd.Parameters.AddWithValue("@where", 15014);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    for (int i=0; i<r.FieldCount;i++)
                    {
                        result.Add(r[i]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Method to update one single element
        /// </summary>
        /// <param name="table">columnn SQL to select<</param>
        /// <param name="set">values to set</param>
        /// <param name="where">where statement to specify select</param>
        public void singleItemUpdate(string table, string set, string where=null) {
            conn.Open();
            if (where == null)
            {
                string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", table, set, where);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET  @set WHERE " + where, conn);
                //cmd.Parameters.AddWithValue("@set", set);
                cmd.ExecuteNonQuery();
            }
            else
            {
                string sql = string.Format("UPDATE {0} SET {1}", table, set);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET  @set", conn);
                //cmd.Parameters.AddWithValue("@set", set);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method to insert one single element
        /// </summary>
        /// <param name="table">table SQL in which do you need insert</param>
        /// <param name="column">column SQL in which do you need insert</param>
        /// <param name="value">values which you want to insert</param>
        /// <param name="where">where statement to specify select</param>
        public void singleItemInsert(string table, string column, string value, string where = null)
        {
            conn.Open();
            if (where == null)
            {
                string sql = string.Format("INSERT INTO {0} ({1}) VALUES {2} WHERE {3}", table, column, value, where);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //SqlCommand cmd = new SqlCommand("INSERT INTO " + table + "(" + column + ") VALUES  @values WHERE " + where, conn);
                //cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
            else
            {
                string sql = string.Format("INSERT INTO {0} ({1}) VALUES {2}", table, column, value);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //SqlCommand cmd = new SqlCommand("INSERT INTO " + table + " VALUES  @values", conn);
                //cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Async method 
        /// Method to insert one single element
        /// </summary>
        /// <param name="table">string SQL table to write into that</param>
        /// <param name="column">string SQL column</param>
        /// <param name="value">string value to defined values to write into database</param>
        /// <param name="where">string where condition</param>
        public async void singleItemInsertAsync(string table, string column, string value, string where = null)
        {
            await conn.OpenAsync();
            string tableSQL = string.Format(@"{0}", table);
            string columnSQL = string.Format(@"{0}", column);
            string valueSQL = string.Format(@"{0}", value);
            
            if (where == null)
            {
                string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", table, column, value);
                SqlCommand cmd = new SqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync();
            }
            else
            {
                string whereSQL = string.Format(@"{0}", where);
                string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2}) WHERE {3}", table, column, value, where);
                SqlCommand cmd = new SqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Async method
        /// Method to delete selected item(s)
        /// </summary>
        /// <param name="table">table SQL to delete</param>
        /// <param name="where">string where condition</param>
        public async void singleItemDeleteAsync(string table, string where)
        {
            await conn.OpenAsync();
            SqlCommand cmd = new SqlCommand("DELETE FROM " + table + " WHERE " + where, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Async method
        /// Method to update one single element
        /// </summary>
        /// <param name="table">table SQL to update</param>
        /// <param name="set">values to set</param>
        /// <param name="where">string where condition</param>
        public async void singleItemUpdateAsync(string table, string set, string where=null) {
            await conn.OpenAsync();
            if (where==null) {
                //SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET "+ set, conn);
                string sql = string.Format("UPDATE {0} SET {1}", table, set);
                SqlCommand cmd = new SqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync();
            }
            else {
                //SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET "+ set +" WHERE " + where, conn);
                string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", table, set, where);
                SqlCommand cmd = new SqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Select single item from 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="table"></param>
        /// <param name="where">string where condition - function result</param>
        /// <returns></returns>
        public object singleItemSelectPostgres(string column, string table, Func<string> where = null)
        {
            object result = new object();

            if (where == null)
            {
                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table, conn);
                string sql = string.Format("SELECT {0} FROM {1}", column, table);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }
            else
            {
                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table +" WHERE " + where, conn);
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2}", column, table, where);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }

            return result;
        }
        public object singleItemSelectPostgres(string column, string table, string whereMultiple = null, string groupBy = null, string order = null)
        {
            object result = new object();

            if (whereMultiple == null)
            {
                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table, conn);
                string sql = string.Format("SELECT {0} FROM {1}", column, table);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }
            else
            {
                //SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table +" WHERE " + where, conn);
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2}", column, table, whereMultiple);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                NpgsqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }

            return result;
        }
        public object singleItemSelectPostgres(string column, string table, string sum = null, string whereMultiple = null, string groupBy = null, string order = null)
        {
            object result = new object();
            string sql = null;

            if (sum == null)
            {
                sql = string.Format("SELECT {0} FROM {1}", column, table);
            }
            else
            {
                if (whereMultiple == null)
                {
                    if (groupBy == null)
                    {
                        if (order == null)
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2}", column, sum, table);
                        }
                        else
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} {3}", column, sum, table, order);
                        }
                    }
                    else
                    {
                        if (order == null)
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} {3}", column, sum, table, groupBy);
                        }
                        else
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} {3} {4}", column, sum, table, groupBy, order);
                        }
                    }
                }
                else {
                    if (groupBy == null)
                    {
                        if (order == null)
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} WHERE {3}", column, sum, table, whereMultiple);
                        }
                        else
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} WHERE {3} {4}", column, sum, table, whereMultiple, order);
                        }
                    }
                    else
                    {
                        if (order == null)
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} WHERE {3} {4}", column, sum, table, whereMultiple, groupBy);
                        }
                        else
                        {
                            sql = string.Format("SELECT {0}, {1} FROM {2} WHERE {3} {4} {5}", column, sum, table, whereMultiple, groupBy, order);
                        }
                    }
                }
            }
            
            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
            NpgsqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                result = r.GetValue(0);
            }

            return result;
        }

        /// <summary>
        /// Method to select multiple items from postrgres db
        /// </summary>
        /// <param name="column"></param>
        /// <param name="table"></param>
        /// <param name="whereMultiple">string where condition - function result</param>
        /// <param name="groupBy"></param>
        /// <param name="order"></param>
        /// <returns></returns>        
        public List<object> multipleItemSelectPostgres(string column, string table, string whereMultiple = null, string groupBy = null, string order = null) {
            string sql = null;
            List<object> result = new List<object>();
            if (connection.FullState == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            if (whereMultiple == null)
            {
                if (groupBy == null)
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1}", column, table);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} {2}", column, table, order);
                    }
                }
                else
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1} {2}", column, table, groupBy);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} {2} {3}", column, table, groupBy, order);
                    }
                }
            }
            else
            {
                if (groupBy == null)
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2}", column, table, whereMultiple);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2} {3}", column, table, whereMultiple, order);
                    }
                }
                else
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2} {3}", column, table, whereMultiple, groupBy);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2} {3} {4}", column, table, whereMultiple, groupBy, order);
                    }
                }
            }

            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
            NpgsqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    result.Add(r[i]);
                }
            }
            r.Close();
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Async method
        /// Method to select multiple items from postrgres db
        /// </summary>
        /// <param name="column">columnn SQL to select</param>
        /// <param name="table">table SQL to select</param>
        /// <param name="whereMultiple">string where condition - function result</param>
        /// <param name="groupBy">string groupBy condition - function result</param>
        /// <param name="order">string orderBy condition - function result</param>
        /// <returns></returns>
        public async Task<List<object[]>> multipleItemSelectPostgresAsync(string column, string table, string whereMultiple = null, string groupBy = null, string order = null)
        {
            string sql = null;
            List<object[]> result = new List<object[]>();
            if (connection.FullState == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            if (whereMultiple == null)
            {
                if (groupBy == null)
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1}", column, table);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} {2}", column, table, order);
                    }
                }
                else
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1} {2}", column, table, groupBy);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} {2} {3}", column, table, groupBy, order);
                    }
                }
            }
            else
            {
                if (groupBy == null)
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2}", column, table, whereMultiple);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2} {3}", column, table, whereMultiple, order);
                    }
                }
                else
                {
                    if (order == null)
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2} {3}", column, table, whereMultiple, groupBy);
                    }
                    else
                    {
                        sql = string.Format("SELECT {0}  FROM {1} WHERE {2} {3} {4}", column, table, whereMultiple, groupBy, order);
                    }
                }
            }

            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
            NpgsqlDataReader r = cmd.ExecuteReader();
            object[] tmpObjectArray = new object[r.FieldCount];            
            while (await r.ReadAsync())
            {
                for(int i = 0; i<(r.FieldCount);i++) {
                    tmpObjectArray[i] = r[i];                    
                }
                result.Add(tmpObjectArray);
            }
            r.Close();
            cmd.Dispose();
            return result;
        }

        public static string where(string conditionVariable1, string Operator , string conditionVariable2) {
            string whereSQL = conditionVariable1 + Operator + conditionVariable2;
            return whereSQL;
        }
        public static string whereMultiple (string[] conditionVariables1, string[] Operators, string[] conditionVariables2)
        {
            string whereSQL = null;
            if (conditionVariables1.Length == conditionVariables2.Length && conditionVariables1.Length == Operators.Length && conditionVariables2.Length == Operators.Length) {
                whereSQL = conditionVariables1[0] + Operators[0] + conditionVariables2[0];
                for (int i=1;i<conditionVariables1.Length;i++)
                {
                    whereSQL += " AND " + conditionVariables1[i] + Operators[i] + conditionVariables2[i];
                }
            }
            return whereSQL;
        }
        public static string sum(string column, string newcolumn=null) {
            string sumSQL = null;
            if (newcolumn == null)
            {
                sumSQL = "SUM(" + column + ")";
            }
            else {
                sumSQL = "SUM(" + column + ") AS " + newcolumn;
            }
            return sumSQL;
        }
        public static string order(string column, string order)
        {
            string orderBySQL = "ORDER BY " + column + " " + order;
            return orderBySQL;
        }
        public static string groupBy(string column, string order)
        {
            string groupBySQL = "GROUP BY " + column + " " + order;
            return groupBySQL;
        }

    }
}