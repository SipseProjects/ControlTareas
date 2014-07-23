using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace ProyectosWeb
{
    public partial class Login : System.Web.UI.Page
    {
        private static SqlConnection conn = new SqlConnection("Data Source=172.16.1.31;Initial Catalog=ProyectosGestion;Persist Security Info=True;User ID=sa;Password=Adminpwd20");

        protected void Page_Load(object sender, EventArgs e)
        {

            
        }

        protected void ButtonIniciarSesion_Click(object sender, EventArgs e)
        {

            
        }

        [System.Web.Services.WebMethod]
        public static string verificarLogin(String PasswordIntroducido, String usuario)
        {
            string us = "";
            conn.Open();
            SqlCommand cmSql = conn.CreateCommand();
            cmSql.CommandText = "Select * from usuarios where nombre=@parm2";
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
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
                    bool passwordIsCorrect2 = BCrypt.Net.BCrypt.Verify(PasswordIntroducido, drDatos["contraseña"].ToString());
                }

            }
            conn.Close();
            return us;
        }
        
    }
}