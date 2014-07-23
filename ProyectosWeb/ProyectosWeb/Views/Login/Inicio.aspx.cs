using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Security.Authentication;
using System.Web.Security;
using ProyectosWeb.BusinessLogic.general;
using ProyectosWeb.Models;
using ProyectosWeb.BusinessLogic.Seguridad;


namespace ProyectosWeb.Views.Login
{
    public partial class Inicio : System.Web.UI.Page
    {
        private static SqlConnection conn = new SqlConnection("Data Source=172.16.1.31;Initial Catalog=ProyectosGestion;Persist Security Info=True;User ID=sa;Password=Adminpwd20");
        private UsuarioFacade _usuarioFacade = new UsuarioFacade(conn);
        private EnvioEmail _enviarEmail = new EnvioEmail();

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Request.Form.AllKeys.Length > 0)
            {
                if (Request.Form.AllKeys[4] == ButtonEnviarEmailSeg.UniqueID)
                {
                    MultiView1.ActiveViewIndex = 1;
                }
            }
            else {
                LabelNav.Text = "Iniciar Sesión";
                MultiView1.ActiveViewIndex = 0;
            }
        }

        protected void linkIniciaSesion(object sender, EventArgs e)
        {
            LabelNav.Text = "Iniciar Sesión";
            MultiView1.ActiveViewIndex = 0;
        }

        protected void linkRestorePassUss(object sender, EventArgs e)
        {
            LabelNav.Text = "Restablecer contraseña";
            LabEmailReg.Text = "";
            TextBoxEmailRegistrado.Text = "";
            MultiView1.ActiveViewIndex = 1;
        }
        protected void ButtonEnviarEmailSeg_Click(object sender, EventArgs e)
        {                        
            String email = TextBoxEmailRegistrado.Text.Trim();
            Usuario us = _usuarioFacade.getUserByEmail(email);

            if (us.idUsuario != 0)
            {
                _enviarEmail.SendMail("llr.allleo@gmail.com", email, us.idUsuario.ToString(), us.nombre);
                _usuarioFacade.setTiempoExpiracion(us.idUsuario);
                LabEmailReg.Text = "Correo Enviado. \n Sigue las instrucciones que te hemos enviado. ";
            }
            else
            {
                LabEmailReg.Text = "No se ha podido enviar el email, intente mas tarde :D";
            }
        }

        protected void ButtonIniciarSesion_Click(object sender, EventArgs e)
        {            
            String returnValue = string.Empty;
            msgusu.Text = "";
            msgpass.Text = "";
            conn.Open();
            try
            {
                SqlCommand cmSql = conn.CreateCommand();
                cmSql.CommandText = "Select * from usuarios where nombre=@parm2 and estado=0";
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters["@parm2"].Value = TextBoxUsuario.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDatos = dtDatos.Rows[0];
                        string d = drDatos["nombre"].ToString();
                        string idus = drDatos["idusuario"].ToString();

                        bool passwordIsCorrect = BCrypt.Net.BCrypt.Verify(PasswordUsuario.Value.ToString(), drDatos["contraseña"].ToString());
                        if (passwordIsCorrect)
                        {                          
                            string[] roles = { "Admin", d };

                            GenericIdentity userIdentity = new GenericIdentity(d);                            

                            GenericPrincipal userPrincipal =
                              new GenericPrincipal(userIdentity, roles);
                            Context.User = userPrincipal;
                           
                            if (Context.User.Identity!=null)
                            {
                                //Response.Redirect("../../Main.aspx");
                                // Create and tuck away the cookie
                                FormsAuthenticationTicket authTicket =
                                  new FormsAuthenticationTicket(1,
                                                                d,
                                                                DateTime.Now,
                                                                DateTime.Now.AddMinutes(5),
                                                                false,
                                                                d);
                                string encTicket = FormsAuthentication.Encrypt(authTicket);
                                HttpCookie faCookie =
                                  new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                                Response.Cookies.Add(faCookie);

                                //// And send the user where they were heading
                                //string redirectUrl =
                                //  FormsAuthentication.GetRedirectUrl(d, false);
                                Response.Redirect("../../Main.aspx");
                            }
                        }
                        else
                        {
                            msgpass.Text = "Contraseña no valida";
                            msgpass.ForeColor = System.Drawing.Color.Red;
                        }

                    }
                    else
                    {

                        msgusu.Text = "El nombre de Usuario es incorrecto";
                        msgusu.ForeColor = System.Drawing.Color.Red;
                    }

                }
            }
            catch
            {
                returnValue = "error";
            }
            conn.Close();

        }

        [System.Web.Services.WebMethod]
        public static String verificarLogin(String u, String p)
        {
            String returnValue = string.Empty;
            conn.Open();
            try{
            SqlCommand cmSql = conn.CreateCommand();
            cmSql.CommandText = "Select * from usuarios where nombre=@parm2 and estado=0";
            cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
            cmSql.Parameters["@parm2"].Value =u;
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drDatos = dtDatos.Rows[0];
                    string d = drDatos["nombre"].ToString();
                    bool passwordIsCorrect = BCrypt.Net.BCrypt.Verify(p, drDatos["contraseña"].ToString());
                    if (passwordIsCorrect)
                    {
                       
                    
                    }else {
                    returnValue = "errorPass";
                    }

                }
                else {

                    returnValue = "errorUsuario";
                }

            }
            }
            catch
            {
                returnValue = "error";
            }
            conn.Close();
            return returnValue;
            
        }
        [System.Web.Services.WebMethod]
        public static String CheckEmail(String email)
        {
            return accesAjax("personas", "email", email);
        }

        public static string accesAjax(string referencia, string param, String email)
        {
            string returnValue = string.Empty;
            try
            {
                conn.Open();
                SqlCommand cmSql = conn.CreateCommand();
                cmSql.CommandText = "Select * from " + referencia + " where  " + param + "=@parm2";
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
                conn.Close();
            }
            catch
            {
                returnValue = "error";
            }
            return returnValue;

        }
    }
}