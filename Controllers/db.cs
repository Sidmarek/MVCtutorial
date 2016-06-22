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
        public SqlConnection conn;

        /*
         * @param paramless, @result void
         *  Method to inicialize dbConnection
         */
        public void dbConnection()
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
                SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table, conn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result = r.GetValue(0);
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM "+ table +" WHERE " + where, conn);
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
            int i = 0;
            List<object> result = new List<object>();

            if (where == null)
            {
                SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM " + table, conn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result.Add(r[column]);
                    i++;
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT " + column + " FROM " + table + " WHERE " + where, conn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result.Add(r[column]);
                    i++;
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
                SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET  @set WHERE " + where, conn);
                cmd.Parameters.AddWithValue("@set", set);
                cmd.ExecuteNonQuery();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET  @set", conn);
                cmd.Parameters.AddWithValue("@set", set);
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
                SqlCommand cmd = new SqlCommand("INSERT INTO " + table + "(" + column + ") VALUES  @values WHERE " + where, conn);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO " + table + " VALUES  @values", conn);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
        }

        /* Async method
         * @param string table, string column, string value, string where(optional), @result void
         * Method to insert one single element
         */
        public async void singleItemInsertAsync(string table, string column, string value, string where = null)
        {
            dbConnection();
            await conn.OpenAsync();
            if (where == null)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO " + table + "(" + column + ") VALUES  ("+ value +")", conn);
                await cmd.ExecuteNonQueryAsync();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO " + table + "(" + column + ") VALUES  ("+ value +") WHERE " + where, conn);
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
                SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET "+ set, conn);
                await cmd.ExecuteNonQueryAsync();
            }
            else {
                SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET "+ set +" WHERE " + where, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}