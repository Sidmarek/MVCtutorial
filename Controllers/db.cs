using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCtutorial.Controllers
{
    //Class for MSSQL db
    public class db
    {
        //Only shared connection
        private SqlConnection conn;
        private NpgsqlConnection connection;

        /*
         * @param paramless, @result void
         *  Method to inicialize dbConnection
         */
        private void dbConnection()
        {
            string ConnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conn = new SqlConnection(ConnStr);
        }
    
        /* 
         * @param string column, string table, string where(optional), @result object
         * Method to get one single element as an object
         */
        public object singleItemSelect(string column, string table, string where=null) {
            dbConnection();
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

        /* 
         * @param string column, string table, string where(optional), @result object
         * Method to get multiple elements as an list of objects
         */
        public List<object> multipleItemSelect(string column, string table, string where = null)
        {
            dbConnection();
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

        /* 
         * @param string table, string set, string where(optional), @result void
         * Method to update one single element
         */
        public void singleItemUpdate(string table, string set, string where=null) {
            dbConnection();
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

        /* 
         * @param string table, string column, string value, string where(optional), @result void
         * Method to insert one single element
         */
        public void singleItemInsert(string table, string column, string value, string where = null)
        {
            dbConnection();
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
            dbConnection();
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

        /* Async method
        * @param string table, string where, @result void
        * Method to delete selected item(s)
        */
        public async void singleItemDeleteAsync(string table, string where)
        {
            dbConnection();
            await conn.OpenAsync();
            SqlCommand cmd = new SqlCommand("DELETE FROM " + table + " WHERE " + where, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        /* Async method
         * @param string table, string set, string where(optional), @result void
         * Method to update one single element
         */
        public async void singleItemUpdateAsync(string table, string set, string where=null) {
            dbConnection();
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
        /// Method to establish db connection on PostgreSQL database
        /// </summary>
        /// <param name="adb">database name</param>
        private void dbConnectionPostgres(string aDB) {
            string db = aDB;
            string connString = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "192.168.2.12", 5432, "postgres", "Nordit0276", db);
            NpgsqlConnection connection = new NpgsqlConnection(connString);
        }

        public object singleItemSelectPostgres(string database, string column, string table, string where = null)
        {
            dbConnectionPostgres(database);
            connection.Open();
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
    }
}