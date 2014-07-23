using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Configuration;

namespace ProyectosWeb.DAO
{
      
    public class SeguridadDAO
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ProyectosGestionConnectionString"].ConnectionString;

        private SqlConnection conn = new SqlConnection(connStr);

        public  DataSet getDataset(string command)
        {          
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(command, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            };
            return ds;
        }

        public bool existeEnDB(string consulta) { 
        DataSet existe = getDataset(consulta);
        bool siexiste=false;
        if (existe.Tables.Count > 0)
        {
            if (existe.Tables[0].Rows.Count >0)
            {
                siexiste = true;
            }

        }
        return siexiste;
        }

        public string verificarLogin(String PasswordIntroducido, String DBUserPassHashed, String usuario) {

            string us="";
            //Verify user password
            bool passwordIsCorrect = BCrypt.Net.BCrypt.Verify(PasswordIntroducido, DBUserPassHashed);
            conn.Open();
            SqlCommand cmSql = conn.CreateCommand();
            cmSql.CommandText = "Select * from usuarios where contraseña=@parm1 and nombre=@parm2";
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
            cmSql.Parameters["@parm1"].Value = DBUserPassHashed;
            cmSql.Parameters["@parm2"].Value = usuario;
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);
            
            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drDatos = dtDatos.Rows[0];
                    us = drDatos["nombre"].ToString();
                    bool passwordIsCorrect2 = BCrypt.Net.BCrypt.Verify(PasswordIntroducido, drDatos["contraseña"].ToString());
                }

            }
            conn.Close(); 
            return us;
        }
        public string verificarExisteUsuario( String usuario)
        {
            string us = "";            
            conn.Open();
            SqlCommand cmSql = conn.CreateCommand();
            cmSql.CommandText = "Select * from usuarios where  nombre=@parm2";
            cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
            cmSql.Parameters["@parm2"].Value = usuario;
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drDatos = dtDatos.Rows[0];
                    us = drDatos["nombre"].ToString();
                }

            }
            conn.Close();
            return us;
        }
    }
}