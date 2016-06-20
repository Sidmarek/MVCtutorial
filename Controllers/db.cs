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

       /* Async method
        * @param string table, string set, string where(optional), @result void
        * Method to update one single element
        */
        public async void singleItemUpdateAsync(string table, string set, string where=null) {
            dbConnection();
            await conn.OpenAsync();
            if (where==null) {
                SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET "+ set, conn);
                cmd.Parameters.AddWithValue("@set", set);
                await cmd.ExecuteNonQueryAsync();
            }
            else {
                SqlCommand cmd = new SqlCommand("UPDATE " + table + " SET "+ set +" WHERE " + where, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}