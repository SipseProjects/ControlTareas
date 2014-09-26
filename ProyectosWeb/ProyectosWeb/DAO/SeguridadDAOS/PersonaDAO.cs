using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ProyectosWeb.Models;
using System.Data;

namespace ProyectosWeb.DAO.SeguridadDAOS
{
    public  class PersonaDAO
    {
        private static SqlConnection _conn;

        public PersonaDAO(SqlConnection conn)
        {
        _conn=conn;
        }

        
        public static string CheckEmail(string email)
        {
            string returnValue = string.Empty;
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "Select count(*) from personas where  email=@parm2";
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters["@parm2"].Value = email;
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);
                returnValue = "false";
                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDatos = dtDatos.Rows[0];
                        returnValue = "true";
                    }
                }
                _conn.Close();
            }
            catch
            {
                returnValue = "error";
            }
            return returnValue;
        }

    }
}