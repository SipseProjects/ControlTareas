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
using System.Configuration;
using Models;


namespace ProyectosWeb.Views.Login
{
    public partial class Inicio : System.Web.UI.Page
    {        
        static string connStr = ConfigurationManager.ConnectionStrings["ProyectosGestionConnectionString"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(connStr);
        private UsuarioFacade _usuarioFacade = new UsuarioFacade(conn);
        private EnvioEmail _enviarEmail = new EnvioEmail();
        private int usRestore;
        private ControlarConexion control = new ControlarConexion(conn);
        private SistemasBL _sistemasBl = new SistemasBL(conn);

        protected void Page_Load(object sender, EventArgs e)
        {
            _sistemasBl.DropDownBindSistemas(DropDownList1);
            lblcontrolarAcceso.ForeColor = System.Drawing.Color.Red;
            lblcontrolarAcceso.Text = GlobalDataSingleton.Instance.controlAcceso;
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            if (path.Contains("?"))
            {
                string[] split = path.Split(new Char[] { '=' });
                string emailex = split[1].Substring(0, 5);
                usRestore = (split[2].Length > 0 ? int.Parse(split[2]) : 0);

                if (emailex.Equals("email") && usRestore > 0)
                {
                    Usuario us2 = _usuarioFacade.getUsuario(usRestore);

                    DateTime ahora = DateTime.Now;
                    double horasTrancurridas = (ahora - us2.tiempoExpiracion).TotalHours;
                    if (horasTrancurridas < 3)
                    {
                        if (us2.linkCliked != 1)
                        {
                            LabUserRestore.Text = us2.nombre;
                            LabelNav.Text = "Restablecer Contraseña";
                            MultiView1.ActiveViewIndex = 2;

                            if (us2.linkCliked == 0)
                            {

                                int exito = _usuarioFacade.setLinkCliked(usRestore, int.Parse(hidClicks.Value.ToString()));
                                hidClicks.Value = (int.Parse(hidClicks.Value.ToString()) + 1).ToString();
                            }
                        }
                        else
                        {
                            LabelNav.Text = "Link Expirado";
                            MultiView1.ActiveViewIndex = 3;
                        }
                    }
                    else
                    {

                        LabelNav.Text = "Link Expirado";
                        MultiView1.ActiveViewIndex = 3;
                    }
                }
                else if (split[1].Contains("emailRestore"))
                {
                   
                }

            }

            if (Request.Form.AllKeys.Length > 0)
            {
                if (Request.Form.AllKeys[4] == ButtonEnviarEmailSeg.UniqueID)
                {
                    MultiView1.ActiveViewIndex = 1;
                }
            }
            else if(!LabelNav.Text.Equals("Link Expirado")&&!LabelNav.Text.Equals("Restablecer Contraseña")){
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
            String email = TextBoxEmailRegistrado.Text.Trim().ToLower();
            Usuario us = _usuarioFacade.getUserByEmail(email);

            if (us.idUsuario != 0)
            {
                _enviarEmail.SendMail("llopez@solutiaintelligence.com", email, us.idUsuario.ToString(), us.nombre, HttpContext.Current.Request.Url.Authority.ToString());
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

            control.abrirConexion();
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
                            string valor = Hidsistemaval.Value;                             
                            
                            string[] sistema = { valor.Trim(), "" };

                            GenericIdentity userIdentity = new GenericIdentity(d);

                            GlobalDataSingleton.Instance.sistemaID = int.Parse(valor);
                            
                            GenericPrincipal userPrincipal = new GenericPrincipal(userIdentity, sistema);
                            Context.User = userPrincipal;
                           
                            if (Context.User.Identity!=null)
                            {                              
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
                                GlobalDataSingleton.Instance.expirarTimepo = authTicket.Expiration;
                                Response.Cookies.Add(faCookie);

                                conn.Close();
                                GlobalDataSingleton.Instance.controlAcceso = "";
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

            if (conn.State == ConnectionState.Open)
            {
                control.cerrarConexion();
            }            
        }

        protected void SeleccionDropDownList(Object sender, EventArgs e)
        {
            
        }

        protected void RestablecerPasswordEmail_Click(object sender, EventArgs e)
        {
            String passwordHash = BCrypt.Net.BCrypt.HashPassword(PasswordUsRestore.Value.ToString(), BCrypt.Net.BCrypt.GenerateSalt(12));

            if (usRestore > 0)
            {
                int exito = _usuarioFacade.UpdatePasswordRestore(usRestore, passwordHash);

                if (exito > 0)
                {
                    LabRestorePass.ForeColor = System.Drawing.Color.Green;
                    LabRestorePass.Text = "Los datos han sido actualizados correctamente";
                    HyperLinkSesion.Visible = true;
                }
                else
                {
                    LabRestorePass.ForeColor = System.Drawing.Color.Red;
                    LabRestorePass.Text = "No se ha podido actualizar la Información";
                }
                ClientScript.RegisterStartupScript(this.GetType(), "setTimeout", "setTimeout(function () {$('[id$=LabRestorePass]').fadeOut();}, 7000);", true);
            }
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
            cmSql.Parameters["@parm2"].Value =u.ToLower();
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
                cmSql.Parameters["@parm2"].Value = email.Trim().ToLower();
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
                
            }
            catch
            {
                returnValue = "error";
            }
            conn.Close();
            return returnValue;

        }

    }
}