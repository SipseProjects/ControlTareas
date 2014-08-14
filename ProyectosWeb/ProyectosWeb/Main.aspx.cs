using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ProyectosWeb.DAO;
using ProyectosWeb.DAO.SeguridadDAOS;
using System.Net;
using System.Net.Mail;
using ProyectosWeb.Models;
using ProyectosWeb.BusinessLogic.Seguridad;
using System.Security.Principal;
using System.Web.Security;
using System.Configuration;
using ProyectosWeb.Models.Seguridad;
using System.Globalization;
using BusinessLogic.Seguridad;
using ProyectosWeb.BusinessLogic.general;
using Models.Seguridad;
using Models.Seguridad.ControlAcceso;
using Models;
using System.Diagnostics;


namespace ProyectosWeb
{
    public partial class Main : System.Web.UI.Page
    {        
        static string connStr = ConfigurationManager.ConnectionStrings["ProyectosGestionConnectionString"].ConnectionString;
        private static SqlConnection conn = new SqlConnection(connStr);
        private UsuarioFacade _usuarioFacade = new UsuarioFacade(conn);
        private static PerfilBL _perfilBL = new PerfilBL(conn);
        public static GruposBL _grupoBL = new GruposBL(conn);
        public static SistemasBL _sistemaBL = new SistemasBL(conn);        
        private int usRestore;        
        private SeguridadDAO accesDao = new SeguridadDAO(); 
        String PageIndex,PrevIndex;
        private int idusuarioSeleccionado;                 
       private ModuloBL _modulosBl = new ModuloBL(conn);
       private PantallaBL _pantallaBL = new PantallaBL(conn);
       private OpcionBL _opcionBL = new OpcionBL(conn);
       private PerfilesModulosBL _PerfilesModulosBL = new PerfilesModulosBL(conn);
       private PerfilesPantallasBL _PerfilesPantallasBL = new PerfilesPantallasBL(conn);
       private PerfilesOpcionesBL _PerfilesOpcionesBL = new PerfilesOpcionesBL(conn);
       private UsuariosModulosBL _UsuariosModulosBL = new UsuariosModulosBL(conn);
       private UsuariosPantallasBL _UsuariosPantallasBL = new UsuariosPantallasBL(conn);
       private UsuariosOpcionesBL _UsuariosOpcionesBL = new UsuariosOpcionesBL(conn);
       private SistemasModulosBL _SistemasModulosBL = new SistemasModulosBL(conn);
       private ControlAcessoUsuarioBL _CAUsuarioBL = new ControlAcessoUsuarioBL(conn);
       private SistemasModulos _sistemamodulo = new SistemasModulos();        
      private const string litreepant = "litreepant";
      private const string litreeop = "litreeop";
      private const string litreepop = "litreepop";
      private const string Modtree = "Modtree";
      private const string litreesubop = "litreesubop";
      private const string limod = "limod";
      private const string redirectinicio="Views/Login/Inicio.aspx";
      private const string redirectMain = "Main.aspx";
      private const string admin = "admin";
      private int contCBKSTotal;
      private int subOpcionul;
      private int countOpcion;      
      private UsuarioLogin uslogin;
      private String userName;
      private eliminarCheckboxs eliminarOp = new eliminarCheckboxs();
      private static ControlarConexion controlConn = new ControlarConexion(conn);

        private int checkboxParent=0;
        private String analizador = "";
        private int subTotalCHK=0;
       private  int indexParentEliminar;

       public static Stopwatch stopWatch = new Stopwatch();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            if (authCookie != null)
            {
                 userName = Context.User.Identity.Name;
                usuarioLogin.Text = userName + ", ";                               
                uslogin = _CAUsuarioBL.getUsuarioLogeado(userName, GlobalDataSingleton.Instance.sistemaID);
                if (uslogin.Perfiles.Count > 0 || userName.ToLower().Equals(admin))
                {
                _sistemamodulo = uslogin.sistemasModulos;
                List<SistemasModulos> acceso = _sistemamodulo.sistemasModulos;
                if (acceso.Count > 0 || userName.ToLower().Equals(admin))
                { 
                    for (int pm = 0; pm < acceso.Count; pm++)
                    {
                        SistemasModulos pmodulo = acceso[pm];
                        FindControl(pmodulo.modulo.h3Id).Visible = bool.Parse(pmodulo.divvisible);
                        FindControl(pmodulo.modulo.divId).Visible = bool.Parse(pmodulo.divvisible);
                    }
                
                    List<UsuariosPantallas> accesoUsuariopantalla = _sistemamodulo.usuariosPantallas;
                    List<PerfilesPantallas> accesopantalla = _sistemamodulo.perfilesPantallas;
                    if (accesoUsuariopantalla.Count < 1 && accesopantalla.Count < 1 && !userName.ToLower().Equals(admin))
                    {
                        GlobalDataSingleton.Instance.controlAcceso = "El Usuario no tiene asociado ninguna pantalla.";
                        Response.Redirect(redirectinicio);
                    }
                    else
                    {
                        for (int pm = 0; pm < accesoUsuariopantalla.Count; pm++)
                        {
                            UsuariosPantallas ppantalla = accesoUsuariopantalla[pm];
                            FindControl(ppantalla.pantalla.idAsp).Visible = bool.Parse(ppantalla.visible);
                        }

                        for (int pm = 0; pm < accesopantalla.Count; pm++)
                        {
                            PerfilesPantallas ppantalla = accesopantalla[pm];
                            FindControl(ppantalla.pantalla.idAsp).Visible = bool.Parse(ppantalla.visible.ToLower());
                        }
                    }
                }else {
                    GlobalDataSingleton.Instance.controlAcceso = "El Sistema no tiene asociado ningun Módulo.";
                    Response.Redirect(redirectinicio);
                }
                }
                else {
                    GlobalDataSingleton.Instance.controlAcceso = "El Usuario no tiene asociado ningun Perfil.";
                    Response.Redirect(redirectinicio);
                }                                
                //Desactivar Modulo Seguridad
                MultiView1Seg.ActiveViewIndex = -1;
                MultiView2SegGrid.ActiveViewIndex = -1;
                activarbotonSeg(false);
                //Desactivar Modulo Tareas
                MultiView2.ActiveViewIndex = -1;
                MultiViewTareaGrid.ActiveViewIndex = -1;
                activarbotonTarea(false);                
                //!Context.User.Identity.AuthenticationType.ToLower().Equals("ntlm")   
                //desactivar Modulo  consulta/reportes
                MultiViewConsultaReporte.ActiveViewIndex = -1;
                
                //desactivar Modulo seguimiento tarea
                MultiViewSeguimientoTarea.ActiveViewIndex = -1;

                int linkButtonTareas = 0;
                int linkButtonConsultaReporte = 0;
                int linkButtonSeguimientoTareas = 0;
                 int linkButtonSeguridad = 0;
                if (Request.Form["__EVENTTARGET"] != null)
                {
                    if (Request.Form["__EVENTTARGET"] == LinkProyecto.UniqueID || Request.Form["__EVENTTARGET"] == LinkRequerimiento.UniqueID
                        || Request.Form["__EVENTTARGET"] == LinkCasosUso.UniqueID || Request.Form["__EVENTTARGET"] == LinkComponente.UniqueID
                        || Request.Form["__EVENTTARGET"] == LinkTarea.UniqueID)
                    {
                        linkButtonTareas = 1;
                    }
                    if (Request.Form["__EVENTTARGET"] == LinkButton1.UniqueID)
                    {
                     linkButtonSeguimientoTareas = 1;
                     }

                    if ( origenLinkButton(Request.Form["__EVENTTARGET"],DivProyecto1.ClientID))
                    {
                     linkButtonSeguridad = 1;
                     }
                    
                }                

                if (IsPostBack && ViewState["Index"] != null)
                {
                    PageIndex = ViewState["Index"].ToString();
                    /**/

                    if (linkButtonTareas != 1 && linkButtonSeguimientoTareas!=1)
                    {
                        if (PageIndex.Equals("Usuarios"))
                        {
                            MultiView1Seg.ActiveViewIndex = 0;
                            MultiView2SegGrid.ActiveViewIndex = 0;
                            activarbotonSeg(true);
                        }
                        else if (PageIndex.Equals("Grupos"))
                        {
                            MultiView1Seg.ActiveViewIndex = 1;
                            MultiView2SegGrid.ActiveViewIndex = 1;
                            activarbotonSeg(true);
                        }
                        else if (PageIndex.Equals("Perfiles"))
                        {
                            MultiView1Seg.ActiveViewIndex = 2;
                            MultiView2SegGrid.ActiveViewIndex = 2;
                            activarbotonSeg(true);
                        }
                        else if (PageIndex.Equals("Relacion de Usuarios") || PageIndex.Equals("Relacion de Grupos") || PageIndex.Equals("Relacion de Perfiles"))
                        {
                            MultiView1Seg.ActiveViewIndex = 3;
                        }
                        else if (PageIndex.Equals("Actualizacion de Cuenta"))
                        {
                            MultiView1Seg.ActiveViewIndex = 4;
                        }
                        else if (PageIndex.Equals("Contraseña"))
                        {
                            MultiView1Seg.ActiveViewIndex = 5;
                        }
                        else if (PageIndex.Equals("Contraseña Nueva"))
                        {
                            MultiView1Seg.ActiveViewIndex = 6;
                        }
                        else if (PageIndex.Equals("Sistemas"))
                        {
                            activarbotonSeg(true);
                            MultiView1Seg.ActiveViewIndex = 8;
                            MultiView2SegGrid.ActiveViewIndex = 3;                            
                        }
                        else if (PageIndex.Equals("Actualizacion de Sistemas"))
                        {
                            MultiView2SegGrid.ActiveViewIndex = -1;
                            MultiView1Seg.ActiveViewIndex = 8;               
                        }
                        else if (PageIndex.Equals("Módulos"))
                        {
                            MultiView1Seg.ActiveViewIndex = 9;
                        }
                        else if (PageIndex.Equals("Pantallas"))
                        {
                            MultiView1Seg.ActiveViewIndex = 10;
                            llenarTree(false,false);
                        }
                        else if (PageIndex.Equals("Opciones"))
                        {
                            MultiView1Seg.ActiveViewIndex = 10;
                            llenarTree(true,false);
                        }
                        else if (PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios"))
                        {
                            MultiView1Seg.ActiveViewIndex = 11;
                            llenarTree(true,true);
                        }
                        else if (PageIndex.Equals("Sistemas y Módulos"))
                        {
                            MultiView1Seg.ActiveViewIndex = 12;
                            llenarTree(false,false);
                        }
                        
                    }
                    //modulo tarea
                   if ((PageIndex.Equals("Proyectos") || PageIndex.Equals("Requerimientos") || PageIndex.Equals("CasosUso")
                            || PageIndex.Equals("Componentes") || PageIndex.Equals("Tareas") ||  linkButtonTareas == 1 )&&
                       (linkButtonSeguimientoTareas == 0 && linkButtonSeguridad == 0 && linkButtonConsultaReporte == 0))
                    {
                        activarbotonTarea(true);
                        MultiView2.ActiveViewIndex = 0;
                        MultiViewTareaGrid.ActiveViewIndex = 0;
                    }

                    //modulo seguimiento de tareas
                    if ((PageIndex.Equals("Captura de Tareas") || linkButtonSeguimientoTareas == 1) && linkButtonTareas == 0 && linkButtonSeguridad == 0 && linkButtonConsultaReporte == 0)
                    {
                        MultiViewSeguimientoTarea.ActiveViewIndex = 0;
                    }
                    
                }

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
                                PageIndex = "Contraseña Nueva";
                                LabelNav.Text = PageIndex;
                                MultiView1Seg.ActiveViewIndex = 6;

                                if (us2.linkCliked == 0)
                                {

                                    int exito = _usuarioFacade.setLinkCliked(usRestore, int.Parse(hidClicks.Value.ToString()));
                                    hidClicks.Value = (int.Parse(hidClicks.Value.ToString()) + 1).ToString();
                                }
                            }
                            else
                            {
                                PageIndex = "Link Expirado";
                                LabelNav.Text = PageIndex;
                                MultiView1Seg.ActiveViewIndex = 7;
                            }
                        }
                        else
                        {
                            PageIndex = "Link Expirado";
                            LabelNav.Text = PageIndex;
                            MultiView1Seg.ActiveViewIndex = 7;
                        }
                    }
                    else if (split[1].Contains("emailRestore"))
                    {
                        MultiView1Seg.ActiveViewIndex = 5;
                    }

                }
                HyperLinkSesion.Visible = false;
               
                if (IsPostBack && ViewState["PrevIndex"] != null)
                {
                    PrevIndex = ViewState["PrevIndex"].ToString();
                    /**/

                }
            }
            else {
                GlobalDataSingleton.Instance.controlAcceso = "";
                Response.Redirect(redirectinicio);
            }   
        }
        private void getPantallaIndex()
        {           
            if (Request.Form["__EVENTTARGET"] != null)
            {
                LinkButton pantallalink = (FindControl(Request.Form["__EVENTTARGET"]) as LinkButton);
                string[] indexpantallas = pantallalink.CommandName.Split(new Char[] { ',' });                
                if (indexpantallas.Length > 2)
                {
                    hidindexpantalla.Value = (indexpantallas[2].Length>0 ? indexpantallas[2]:"0");
                }                
            }           
        }
        private void ControlAccesoOpciones() {                       
            List<UsuariosOpciones> accesoopcionUsuario = _sistemamodulo.usuariosOpciones;
            accesoopcionUsuario = accesoopcionUsuario.FindAll(x => x.opcion.pantalla.pantallaIndex == int.Parse(hidindexpantalla.Value));
            List<PerfilesOpciones> accesoopcion = _sistemamodulo.perfilesOpciones;
            if (accesoopcion.Count < 1 && accesoopcionUsuario.Count < 1 && !userName.ToLower().Equals(admin))
            {
                Response.Redirect(redirectMain);
            }else{
                for (int pm = 0; pm < accesoopcionUsuario.Count; pm++)
                {
                    UsuariosOpciones popcion = accesoopcionUsuario[pm];

                    if (PageIndex.Equals("Sistemas") && !popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible);
                    }
                    else if (PageIndex.Equals("Actualizacion de Sistemas") && popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible);
                    }
                    else if (!PageIndex.Contains("Sistemas"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible);
                    }
                }

                accesoopcion = accesoopcion.FindAll(x => x.opcion.pantalla.pantallaIndex == int.Parse(hidindexpantalla.Value));
                for (int pm = 0; pm < accesoopcion.Count; pm++)
                {
                    PerfilesOpciones popcion = accesoopcion[pm];
                    if (PageIndex.Equals("Sistemas") && !popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible);
                    }
                    else if (PageIndex.Equals("Actualizacion de Sistemas") && popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible);
                    }
                    else if (!PageIndex.Contains("Sistemas"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible);
                    }
                }
            }
        }

        private void activarComponenteOpcion(String componenteIndex, String idAsp, String visible)
        {
            if (componenteIndex.Trim().Length > 0)
            {
                GridView grid = (FindControl(idAsp) as GridView);
                (grid.Columns[int.Parse(componenteIndex)]).Visible = bool.Parse(visible);
            }
            else
            {
                FindControl(idAsp).Visible = bool.Parse(visible);
            }
        }

        protected void ProyectoOnClick(object sender, EventArgs e)
        {
            activarbotonTarea(true);
            MultiView2.ActiveViewIndex = 0;
            MultiViewTareaGrid.ActiveViewIndex = 0; 
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Proyectos";
            LabelNav.Text =PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "IDProyectos";
            ((BoundField)GridView1.Columns[1]).DataField = "ClaveProyectos";
            gvbind();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void RequerimientoOnClick(object sender, EventArgs e)
        {
            activarbotonTarea(true);
            MultiView2.ActiveViewIndex = 0;
            MultiViewTareaGrid.ActiveViewIndex = 0; 
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Requerimientos";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID"+PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave"+PageIndex;
            DropDownListDep.Visible = true;
            gvbind();
            DropDownBindProyectos();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void CasosUsoOnClick(object sender, EventArgs e)
        {
            activarbotonTarea(true);
            MultiView2.ActiveViewIndex = 0;
            MultiViewTareaGrid.ActiveViewIndex = 0; 
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "CasosUso";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID" + PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave" + PageIndex;
            DropDownListDep.Visible = true;
            gvbind();
            DropDownBindRequerimientos();
            getPantallaIndex();
            ControlAccesoOpciones();
        }
        
        protected void ComponenteOnClick(object sender, EventArgs e)
        {
            activarbotonTarea(true);
            MultiView2.ActiveViewIndex = 0;
            MultiViewTareaGrid.ActiveViewIndex = 0; 
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Componentes";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID" + PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave" + PageIndex;
            DropDownListDep.Visible = true;
            gvbind();
            DropDownBindCasosUso();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void TareaOnClick(object sender, EventArgs e)
        {
            activarbotonTarea(true);
            MultiView2.ActiveViewIndex = 0;
            MultiViewTareaGrid.ActiveViewIndex = 0; 
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Tareas";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID" + PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave" + PageIndex;
            DropDownListDep.Visible = true;
            gvbind();
            DropDownBindComponentes();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void UsuariosOnClick(object sender, EventArgs e)
        {            
                limpiarFormUsuarios();
                PrevIndex = PageIndex;
                ViewState["PrevIndex"] = PrevIndex;
                PageIndex = "Usuarios";
                LabelNav.Text = PageIndex;
                ViewState["Index"] = PageIndex;
                activarbotonTarea(false);
                activarbotonSeg(true);
                MultiView1Seg.ActiveViewIndex = 0;
                MultiView2SegGrid.ActiveViewIndex = 0;

                GridView2Seg.Columns[0].HeaderText = "ID";
                ((BoundField)GridView2Seg.Columns[0]).DataField = "ID" + PageIndex.Substring(0, PageIndex.Length - 1);
                GridView2Seg.Columns[1].HeaderText = "Usuario";
                ((BoundField)GridView2Seg.Columns[1]).DataField = "Nombre";
                GridView2Seg.Columns[2].HeaderText = "Es Empleado";
                ((BoundField)GridView2Seg.Columns[2]).DataField = "EsEmpleado";
                GridView2Seg.Columns[3].HeaderText = "Estado";
                ((BoundField)GridView2Seg.Columns[3]).DataField = "Estado";
                GridView2Seg.Columns[4].HeaderText = "Nombre";
                ((BoundField)GridView2Seg.Columns[4]).DataField = "nomUsuario";
                GridView2Seg.Columns[5].HeaderText = "Apellidos";
                ((BoundField)GridView2Seg.Columns[5]).DataField = "Apellido";
                GridView2Seg.Columns[6].HeaderText = "Fecha Registro";
                ((BoundField)GridView2Seg.Columns[6]).DataField = "FechaRegistro";
                ((BoundField)GridView2Seg.Columns[6]).ReadOnly = true;
                GridView2Seg.Columns[7].HeaderText = "Tecnologias";
                ((BoundField)GridView2Seg.Columns[7]).DataField = "Tecnologias";
                GridView2Seg.Columns[8].HeaderText = "Email";
                ((BoundField)GridView2Seg.Columns[8]).DataField = "Email";
                ((BoundField)GridView2Seg.Columns[8]).ReadOnly = true;
                GridView2Seg.Columns[9].HeaderText = "Telefono";
                ((BoundField)GridView2Seg.Columns[9]).DataField = "Telefono";
                gvbindSeg();
                getPantallaIndex();
                ControlAccesoOpciones(); 
        }
        protected void CuentaUsuarioOnClick(object sender, EventArgs e)
        {
            TextBoxUsuarioUpdate.Text = Context.User.Identity.Name;
            HidValidUserUpdate.Value = Context.User.Identity.Name;
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            actualizaPagina("Actualizacion de Cuenta");
            MultiView1Seg.ActiveViewIndex = 4;
            msgactualizado.Text = "";
            getPantallaIndex();
            //ControlAccesoOpciones();                        
        }
        protected void RestablecerPasswordOnClick(object sender, EventArgs e)
        {
            //MultiView2SegGrid.ActiveViewIndex = -1;
            //activarbotonSeg(false);
            //actualizaPagina("Contraseña");
            //MultiView1Seg.ActiveViewIndex = 5;
            //msgactualizado.Text = "";
            //LabEmailReg.Text = "";
        }
        protected void RestablecerPasswordEmailOnClick(object sender, EventArgs e)
        {
            LabUserRestore.Text = Context.User.Identity.Name;
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            actualizaPagina("Contraseña Nueva");
            MultiView1Seg.ActiveViewIndex = 6;
            LabRestorePass.Text = "";
        }

        
        protected void GruposOnClick(object sender, EventArgs e)
        {
            limpiarFormGrupos();
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Grupos";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            activarbotonTarea(false);
            activarbotonSeg(true);
            MultiView1Seg.ActiveViewIndex = 1;
            MultiView2SegGrid.ActiveViewIndex = 1;                        
            GridView2SegGrupo.Columns[0].HeaderText = "ID";
            (GridView2SegGrupo.Columns[0] as BoundField).DataField = "ID" + PageIndex;
            GridView2SegGrupo.Columns[1].HeaderText = "Nombre";
            (GridView2SegGrupo.Columns[1] as BoundField).DataField = "Nombre";
            GridView2SegGrupo.Columns[2].HeaderText = "Descripcion";
            (GridView2SegGrupo.Columns[2] as BoundField).DataField = "Descripcion";
            //eliminaColumnasGridView(GridView2Seg, 3, 5, GridView2Seg.Columns.Count); //2
            gvbindSeg();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void PerfilesOnClick(object sender, EventArgs e)
        {
            limpiarFormPerfiles();
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Perfiles";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            activarbotonTarea(false);
            activarbotonSeg(true);
            MultiView1Seg.ActiveViewIndex = 2;
            MultiView2SegGrid.ActiveViewIndex = 2;          
            GridView2SegPerfiles();           
            gvbindSeg();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        private void GridView2SegPerfiles()
        {
            GridView2SegPerfil.Columns[0].HeaderText = "ID";
            (GridView2SegPerfil.Columns[0] as BoundField).DataField = "ID" + PageIndex.Substring(0, PageIndex.Length - 2);
            GridView2SegPerfil.Columns[1].HeaderText = "Nombre";
            (GridView2SegPerfil.Columns[1] as BoundField).DataField = "Nombre";
            GridView2SegPerfil.Columns[2].HeaderText = "Descripcion";
            (GridView2SegPerfil.Columns[2] as BoundField).DataField = "Descripcion";
            GridView2SegPerfil.Columns[3].HeaderText = "Puede Registrar ";
            (GridView2SegPerfil.Columns[3] as BoundField).DataField = "usuarioalta";
            GridView2SegPerfil.Columns[4].HeaderText = "Puede Eliminar";
            (GridView2SegPerfil.Columns[4] as BoundField).DataField = "usuariobaja";
            GridView2SegPerfil.Columns[5].HeaderText = "Puede Modificar";
            (GridView2SegPerfil.Columns[5] as BoundField).DataField = "usuariomodifica";
            //eliminaColumnasGridView(GridView2Seg, 6, 3, GridView2Seg.Columns.Count);                                        
        }
        private void eliminarColumnasGridView(GridView grid)
        {
            for (int d = 1; d <= (grid.Columns.Count); d++)
            {
                grid.Columns.RemoveAt(d-1);
                
            }
        }

        private void eliminaColumnasGridView(GridView grid, int position, int limite, int columnas) {
            if (grid.Columns.Count==columnas)
            {
            for (int d = position; d <= (grid.Columns.Count - 2)+limite; d++)
            {
                grid.Columns.Remove(((BoundField)grid.Columns[position]));
            }
        }
        }
        private void agregarColumnasGridView(GridView grid, int columnas)
        {
            
                for (int d = 0; d <= columnas-2; d++)
                {
                    grid.Columns.Add(new BoundField());
                }
            
        }
        void activarbotonSeg(bool activo) {
            opcionesTarea(Button4seg, activo);
            opcionesTarea(Button5seg, activo);
            opcionesTarea(Button6seg, activo);
        }

        void activarbotonTarea(bool activo)
        {
            opcionesTarea(Button1, activo);
            opcionesTarea(Button2, activo);
            opcionesTarea(Button3, activo);
        }

        void opcionesTarea(Button b, bool activo)
        {
            b.Visible = activo;
            b.Enabled = activo;
        }

        protected void RelacionesUsuariosOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            ListBoxGruposSeg.Items.Clear();
            ListBoxGruposAsigSeg.Items.Clear();
            actualizaPagina("Relacion de Usuarios");
            cambiarNomEtiquetaRelaciones("Seleccione un Usuario :", "Grupos");
            //_usuarioFacade.llenarListaUsuario(ListBoxUsuariosSeg);
            _usuarioFacade.DropDownBinUsuarios(DropDownListadoSeg);
            MultiView1Seg.ActiveViewIndex = 3;
            getPantallaIndex();
            ControlAccesoOpciones();
        }
        
        protected void RelacionesGruposOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            ListBoxGruposSeg.Items.Clear();
            ListBoxGruposAsigSeg.Items.Clear();
            actualizaPagina("Relacion de Grupos");
            cambiarNomEtiquetaRelaciones("Seleccione un Grupo :", "Usuarios");
            //_grupoBL.llenarListaGrupos(ListBoxUsuariosSeg,0);
            _grupoBL.DropDownBindGrupos(DropDownListadoSeg);
            MultiView1Seg.ActiveViewIndex = 3;
            getPantallaIndex();
            ControlAccesoOpciones();
        }
        protected void RelacionesPerfilesOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            ListBoxGruposSeg.Items.Clear();
            ListBoxGruposAsigSeg.Items.Clear();
            actualizaPagina("Relacion de Perfiles");
            cambiarNomEtiquetaRelaciones("Seleccione un Usuario :", "Perfiles");
            //_usuarioFacade.llenarListaUsuario(ListBoxUsuariosSeg);
            _usuarioFacade.DropDownBinUsuarios(DropDownListadoSeg);
            MultiView1Seg.ActiveViewIndex = 3;
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void ControlAccesSistemaOnClick(object sender, EventArgs e)
        {
            mostrarSistemas();
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        private void mostrarSistemas() {
            lblUpdateSistema.Text = "";
            GridViewsistema.EditIndex = -1;
            limpiarFormSistemas();
            ButtonUpdateSistema.Visible = false;
            ButtonCancelSistema.Visible = false;
            ButtonConsultaSistemasSeg.Visible = false;
            activarbotonSeg(true);
            actualizaPagina("Sistemas");
            gvbindSeg();
            MultiView2SegGrid.ActiveViewIndex = 3;
            MultiView1Seg.ActiveViewIndex = 8;
        }

        protected void ControlAccesModuloOnClick(object sender, EventArgs e)
        {
            actualizaPagina("Módulos");
            LabUpdateModulo.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            llenarCompararModuloCheckBox(CheckBoxListModulo);
            MultiView1Seg.ActiveViewIndex = 9;
            getPantallaIndex();
            ControlAccesoOpciones();            
         }

        private void llenarCompararModuloCheckBox(CheckBoxList listado) {
            List<string> modulos = new List<string>();
            List<string> identidicadorhtml = new List<string>();

            foreach (Control c in accordion.Controls)
            {
                string identificadores = "";
                string modulo = "";
                try
                {
                    if (c.GetType() == typeof(HtmlGenericControl) && ((HtmlGenericControl)c).TagName.ToLower().Equals("h3"))
                    {
                        modulo = ((HtmlGenericControl)c).InnerText.Trim() + "," + ((HtmlGenericControl)c).ClientID;
                    }

                    if (c.GetType() == typeof(HtmlGenericControl) && ((HtmlGenericControl)c).TagName.ToLower().Equals("div"))
                    {
                        identificadores = "" + ((HtmlGenericControl)c).ClientID;
                    }

                }
                catch
                {

                    if (c.GetType() == typeof(HtmlGenericControl) && ((HtmlGenericControl)c).TagName.ToLower().Equals("h3"))
                    {
                        identificadores = "" + ((HtmlGenericControl)c).ClientID;
                    }

                    if (c.GetType() == typeof(HtmlGenericControl) && ((HtmlGenericControl)c).TagName.ToLower().Equals("div"))
                    {
                        modulo = "" + ((HtmlGenericControl)c).ClientID;
                    }
                }
                if (identificadores.Length > 0)
                {
                    modulos.Add(identificadores);
                }
                if (modulo.Length > 0)
                {
                    identidicadorhtml.Add(modulo);
                }
            }

            DataTable table2 = new DataTable();
            table2.Columns.Add("idmodulo", typeof(string));
            table2.Columns.Add("nombre", typeof(string));

            listado.Height = 200;
            listado.Width = 200;

            for (int i = 0; i < modulos.Count; i++)
            {
                string[] split = identidicadorhtml[i].Split(new Char[] { ',' });
                Modulo m= _modulosBl.getModulo(null,split[1], modulos[i],0);
                if (m.idModulo>0)
                {
                    table2.Rows.Add(split[1] + "," + modulos[i] + "," + m.idModulo, split[0]);
                }
                else {
                    table2.Rows.Add(split[1] + "," + modulos[i], split[0]);
                }                
            }
            listado.DataSource = table2;
            listado.DataTextField = "nombre";
            listado.DataValueField = "idmodulo";
            listado.DataBind();
            actualizaCheckBoxListModulo(listado);
            
        }

        private void actualizaCheckBoxListModulo(CheckBoxList listado)
        {
            for (int i = 0; i < listado.Items.Count; i++)
            {
                string[] split = listado.Items[i].Value.Split(new Char[] { ',' });

                if (split.Length > 2)
                {
                    if (split[2].Length > 0)
                    {
                        
                        Modulo modulo = _modulosBl.getModulo(null, null, null, int.Parse(split[2]));

                        if (modulo.idModulo > 1)
                        {
                            if (modulo.estado == 0)
                            {
                                listado.Items[i].Selected = true;
                                listado.Items[i].Attributes.Add("style", "color: green !important;");
                            }
                            else if (modulo.estado > 0)
                            {
                                listado.Items[i].Attributes.Add("style", "color: red !important; ");
                            }
                        }

                    }
                }
            }
        }

        protected void ControlAccesPantallasOnClick(object sender, EventArgs e)
        {
            actualizaPagina("Pantallas");
            LblupdatePantalla.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 10;
            llenarTree(false,false);
            getPantallaIndex();
            ControlAccesoOpciones();
        }


        protected void ControlAccesOpcionesOnClick(object sender, EventArgs e)
        {
            actualizaPagina("Opciones");
            LblupdatePantalla.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 10;
            llenarTree(true,false);
            getPantallaIndex();
            ControlAccesoOpciones();
        }
        
        protected void RelAccesoPerfilesOnClick(object sender, EventArgs e)
        {
            actualizaPagina("Acceso por Perfiles");
            HidUsuSeleccionadoSeg.Value = "0";
            LblStatusAccePerfil.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            _perfilBL.DropDownBinPerfiles(DropDownAccesoRelaciones);
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 11;
            llenarTree(true, true);
            llenarTree(true,true);
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void RelAccesoModSistemaOnClick(object sender, EventArgs e)
        {
            actualizaPagina("Sistemas y Módulos");
            HidUsuSeleccionadoSeg.Value = "0";
            MultiView2SegGrid.ActiveViewIndex = -1;
            _sistemaBL.DropDownBindSistemas(DropDownListModSis);
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 12;
            llenarTree(false,false);
            llenarTree(false, false);
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected void RelAccesoUsuariosOnClick(object sender, EventArgs e)
        {
            actualizaPagina("Acceso por Usuarios");
            HidUsuSeleccionadoSeg.Value = "0";
            LblStatusAccePerfil.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            _perfilBL.DropDownBinPerfiles(DropDownAccesoRelaciones);
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 11;
            llenarTree(true, true);
            llenarTree(true,true);
            _usuarioFacade.DropDownBinUsuarios(DropDownAccesoRelaciones);
            getPantallaIndex();
            ControlAccesoOpciones();
        }

        protected internal bool origenLinkButton(String eventtargetorigen, String idcontrol){
            bool equivalente = false;
            Control f = FindControl(idcontrol); 
        foreach (Control c in f.Controls)
            {
                string idlink = "";
                try
                {
                    if (c.GetType() == typeof(LinkButton))
                    {
                        idlink =  ((LinkButton)c).ClientID;
                    }                    

                }
                catch
                {
                    if (c.GetType() == typeof(LinkButton))
                    {
                        idlink = ((LinkButton)c).ClientID;
                    }
                }
            if(idlink.Trim().Length>0){
                if (eventtargetorigen.Equals(idlink))
                {
                    equivalente = true;
                    break;
                }
            }
        }
        return equivalente;
        }

        public void cambiarNomEtiquetaRelaciones(string nombre, string nombre2)
        { 
        LabelUsuariosSeg.Text=nombre;
        LabelGrupAsigSeg.Text = nombre2 + " Asignados";
        LabelGruposSeg.Text = nombre2 + " No Asignados";   
        }

        protected void SeleccionDropDownList(Object sender, EventArgs e)
        {
            string valor = DropDownListadoSeg.SelectedValue;
            if (valor.Trim().Length>0)
            {
               int idseleccionado = int.Parse(valor);

                if (PageIndex.Equals("Relacion de Usuarios"))
                {
                    _grupoBL.llenarListaGruposAsignados(ListBoxGruposAsigSeg, idseleccionado);
                    _grupoBL.llenarListaGrupos(ListBoxGruposSeg, idseleccionado);
                }
                else if (PageIndex.Equals("Relacion de Grupos"))
                {
                    _usuarioFacade.llenarListaUsuariosAsignados(ListBoxGruposAsigSeg, idseleccionado);
                    _usuarioFacade.llenarListaUsuariosNoAsignados(ListBoxGruposSeg, idseleccionado);
                }
                else if (PageIndex.Equals("Relacion de Perfiles"))
                {
                    _perfilBL.llenarListaPerfilesAsignados(ListBoxGruposAsigSeg, idseleccionado);
                    _perfilBL.llenarListaPerfilesNoAsignados(ListBoxGruposSeg, idseleccionado);
                }
                HidUsuSeleccionadoSeg.Value = "" + idseleccionado;
            }
            }        

        protected void SeleccionDropDownListAcceso(Object sender, EventArgs e)
        {
            string valor = DropDownAccesoRelaciones.SelectedValue;
           
            if (valor.Trim().Length > 0)
            {
                int idseleccionado = int.Parse(valor);
                HidUsuSeleccionadoSeg.Value = "" + idseleccionado;

                if (PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios"))
                {
                    llenarTree(true, true);
                }                                
            }
        }

        protected void SeleccionDropDownModuloSistema(Object sender, EventArgs e)
        {
            string valor = DropDownListModSis.SelectedValue;
               if(valor.Trim().Length==0){
                   valor = "0";
               }
                int idseleccionado = int.Parse(valor);
                HidUsuSeleccionadoSeg.Value = "" + idseleccionado;
               llenarTree(false, false);                                                
            
        }

        protected void SeleccionUsuarioOnClick(object sender, EventArgs e)
        {
            if (DropDownListadoSeg.SelectedItem != null)
            {
                if (!DropDownListadoSeg.SelectedItem.Equals(""))
                {
                    idusuarioSeleccionado = int.Parse(DropDownListadoSeg.SelectedValue.ToString());

                    if (PageIndex.Equals("Relacion de Usuarios"))
                    {
                    _grupoBL.llenarListaGruposAsignados(ListBoxGruposAsigSeg, idusuarioSeleccionado);
                    _grupoBL.llenarListaGrupos(ListBoxGruposSeg, idusuarioSeleccionado);                    
                    }
                    else if (PageIndex.Equals("Relacion de Grupos"))
                    {
                        _usuarioFacade.llenarListaUsuariosAsignados(ListBoxGruposAsigSeg, idusuarioSeleccionado);
                        _usuarioFacade.llenarListaUsuariosNoAsignados(ListBoxGruposSeg, idusuarioSeleccionado); 
                    }
                    else if (PageIndex.Equals("Relacion de Perfiles"))
                    {
                        _perfilBL.llenarListaPerfilesAsignados(ListBoxGruposAsigSeg, idusuarioSeleccionado);
                        _perfilBL.llenarListaPerfilesNoAsignados(ListBoxGruposSeg, idusuarioSeleccionado);
                    }
                    HidUsuSeleccionadoSeg.Value = "" + idusuarioSeleccionado;
                }
            }
        }
        protected void AgregarUsuariosGruposOnClick(object sender, EventArgs e)
        {
  
        }

        private void actualizaPagina(string pageindex) {
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = pageindex;
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ListBoxGruposAsigSeg.Height = 200;
            ListBoxGruposAsigSeg.Width = 200;
            ListBoxGruposSeg.Height = 200;
            ListBoxGruposSeg.Width = 200;
        }

        protected void DropDownBindProyectos() {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDProyectos,ClaveProyectos from proyectos where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDProyectos";
            DropDownListDep.DataTextField = "ClaveProyectos";
            DropDownListDep.DataBind();
        
        }

        protected void DropDownBindRequerimientos()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDRequerimientos,ClaveRequerimientos from Requerimientos where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDRequerimientos";
            DropDownListDep.DataTextField = "ClaveRequerimientos";
            DropDownListDep.DataBind();

        }

        protected void DropDownBindCasosUso()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDCasosUso,ClaveCasosUso from CasosUso where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDCasosUso";
            DropDownListDep.DataTextField = "ClaveCasosUso";
            DropDownListDep.DataBind();

        }
 
        protected void DropDownBindComponentes()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDComponentes,ClaveComponentes from Componentes where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDComponentes";
            DropDownListDep.DataTextField = "ClaveComponentes";
            DropDownListDep.DataBind();

        }

        protected void gvbindSeg()
        {
            if (PageIndex != null)
            {
                conn.Open();
                string consulta = "";
                GridView opcion = null;
                if (PageIndex.Equals("Usuarios"))
                {
                    consulta = "select u.IDUsuario as 'leo',u.IDCatalogoProveedores,u.Nombre, u.esEmpleado,u.Contraseña,u.Estado,p.IDPersonas, p.IDUsuario,p.Nombre as nomUsuario,"
                    + " p.Apellido,p.FechaRegistro,p.Tecnologias,p.Estado, p.Email, p.Telefono from Usuarios u inner join  Personas  p"
                    + " on u.IDUsuario=p.IDUsuario where u.Estado=0";
                    opcion = GridView2Seg;
                }
                else
                    if (PageIndex.Equals("Grupos"))
                    {
                        consulta = "select * from grupos g where g.Estado=0";
                        opcion = GridView2SegGrupo;
                    }
                    else
                        if (PageIndex.Equals("Perfiles"))
                        {
                            consulta = "select * from perfiles p where p.estado=0";
                            opcion = GridView2SegPerfil;
                        }
                        else
                            if (PageIndex.Equals("Sistemas"))
                            {
                                consulta = "select * from sistemas p where p.estado=0";
                                opcion = GridViewsistema;
                            }

                SqlCommand cmd = new SqlCommand(consulta, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                conn.Close();

                llenarGrid(ds, opcion);
            }       
       }
        private void llenarGrid(DataSet ds, GridView grid){
            if (ds.Tables[0].Rows.Count > 0)
            {
                grid.DataSource = ds.Tables[0];                
                grid.DataBind();
               
                for (int b = 0; b < grid.Rows.Count; b++)
                {
                    if (PageIndex.Equals("Perfiles"))
                    {
                        grid.Rows[b].Cells[1].Attributes.Add("onkeyup", " "
                       + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                       + " i = irow + 1;   "
                       + " var table = document.getElementById('"+grid.ID.ToString()+"');"
                       + " Row = table.rows[i];"
                       + " td = Row.cells[1]; "
                       + "  CellValue = td.children[0].value;"
                       + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                       + " "
                      );
                    grid.Rows[b].Cells[2].Attributes.Add("onkeyup", " "
                       + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                       + " i = irow + 1;   "
                       + " var table = document.getElementById('GridView2SegPerfil');"
                       + " Row = table.rows[i];"
                       + " td = Row.cells[2]; "
                       + "  CellValue = td.children[0].value;"
                       + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                       + " "
                      );
                     }else
                    if (PageIndex.Equals("Grupos"))
                    {
                        grid.Rows[b].Cells[1].Attributes.Add("onkeyup", " "
                           + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                           + " i = irow + 1;   "
                           + " var table = document.getElementById('GridView2SegGrupo');"
                           + " Row = table.rows[i];"
                           + " td = Row.cells[1]; "
                           + "  CellValue = td.children[0].value;"
                           + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                           + " "
                          );
                        grid.Rows[b].Cells[2].Attributes.Add("onkeyup", " "
                           + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                           + " i = irow + 1;   "
                           + " var table = document.getElementById('GridView2SegGrupo');"
                           + " Row = table.rows[i];"
                           + " td = Row.cells[2]; "
                           + "  CellValue = td.children[0].value;"
                           + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                           + " "
                          );
                        grid.Rows[b].Cells[4].Attributes.Add("onclick", " " + JQueryConfirmDialogGridview("   javascript:__doPostBack('GridView2SegGrupo','Delete$" + b + "'); "));
                    }
                      if (PageIndex.Equals("Usuarios"))
                     {                        
                         grid.Rows[b].Cells[2].Attributes.Add("onkeyup", " "
                           + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                           + " i = irow + 1;   "
                           + " var table = document.getElementById('GridView2Seg');"
                           + " Row = table.rows[i];"
                           + " td = Row.cells[2]; "
                           + "  CellValue = td.children[0].value;"
                           + " if(parseInt(td.children[0].value.length)>1){ td.children[0].value=CellValue.substring(0, 1);}"
                           + " if(parseInt(td.children[0].value.length)==1){ if(td.children[0].value==='N'||td.children[0].value==='Y'||td.children[0].value==='n'||td.children[0].value==='y') {td.children[0].value=td.children[0].value;}else{td.children[0].value=''; }}"
                          );
                         grid.Rows[b].Cells[3].Attributes.Add("onkeyup", " "
                          + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                          + " i = irow + 1;   "
                          + " var table = document.getElementById('GridView2Seg');"
                          + " Row = table.rows[i];"
                          + " td = Row.cells[3]; "
                          + "  CellValue = td.children[0].value;"
                          + " if(parseInt(td.children[0].value.length)>1){ td.children[0].value=CellValue.substring(0, 1);}"
                          + " if(parseInt(td.children[0].value.length)==1){ if(td.children[0].value==='1'||td.children[0].value==='0') {td.children[0].value=td.children[0].value;}else{td.children[0].value=''; }}"
                         );
                         grid.Rows[b].Cells[4].Attributes.Add("onkeyup", " "
                          + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                          + " i = irow + 1;   "
                          + " var table = document.getElementById('GridView2Seg');"
                          + " Row = table.rows[i];"
                          + " td = Row.cells[4]; "
                          + "  CellValue = td.children[0].value;"
                          + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                          + " "
                         );
                         grid.Rows[b].Cells[5].Attributes.Add("onkeyup", " "
                          + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                          + " i = irow + 1;   "
                          + " var table = document.getElementById('GridView2Seg');"
                          + " Row = table.rows[i];"
                          + " td = Row.cells[5]; "
                          + "  CellValue = td.children[0].value;"
                          + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                          + " "
                         );
                         grid.Rows[b].Cells[7].Attributes.Add("onkeyup", " "
                          + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                          + " i = irow + 1;   "
                          + " var table = document.getElementById('GridView2Seg');"
                          + " Row = table.rows[i];"
                          + " td = Row.cells[7]; "
                          + "  CellValue = td.children[0].value;"
                          + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                          + " "
                         );
                         grid.Rows[b].Cells[8].Attributes.Add("onkeyup", " "
                         + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                         + " i = irow + 1;   "
                         + " var table = document.getElementById('GridView2Seg');"
                         + " Row = table.rows[i];"
                         + " td = Row.cells[8]; "
                         + "  CellValue = td.children[0].value;"
                         + " if(parseInt(td.children[0].value.length)>29){ td.children[0].value=CellValue.substring(0, 30);}"
                         + " "
                        );
                         grid.Rows[b].Cells[9].Attributes.Add("onkeyup", " "
                         + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                         + " i = irow + 1;   "
                         + " var table = document.getElementById('GridView2Seg');"
                         + " Row = table.rows[i];"
                         + " td = Row.cells[9]; "
                         + "  CellValue = td.children[0].value;"
                         + " if(isNaN(CellValue)){ td.children[0].value='';}"
                         + " if(parseInt(td.children[0].value.length)>11){ td.children[0].value=CellValue.substring(0, 12);}"
                         + " "
                        );
                         grid.Rows[b].Cells[11].Attributes.Add("onclick", " " + JQueryConfirmDialogGridview("   javascript:__doPostBack('GridView2Seg','Delete$"+b+"'); "));
                     }
                    if (PageIndex.Equals("Perfiles"))
                    {
                        for (int n = 5; n > 2; n--)
                        {
                            grid.Rows[b].Cells[n].Attributes.Add("onkeyup", " "
                           + " var i, CellValue, Row, td; var irow = parseInt(document.getElementById('HiddenRowIndexSegUpd').value);"
                           + " i = irow + 1;   "
                           + " var table = document.getElementById('GridView2SegPerfil');"
                           + " Row = table.rows[i];"
                           + " td = Row.cells[" + n + "]; "
                           + "  CellValue = td.children[0].value;"
                           + " if(parseInt(td.children[0].value.length)>1){ td.children[0].value=CellValue.substring(0, 1);}"
                           + " if(parseInt(td.children[0].value.length)==1){ if(td.children[0].value==='N'||td.children[0].value==='Y'||td.children[0].value==='n'||td.children[0].value==='y') {td.children[0].value=td.children[0].value;}else{td.children[0].value=''; }}"
                          );
                        }
                        grid.Rows[b].Cells[7].Attributes.Add("onclick", " " + JQueryConfirmDialogGridview("   javascript:__doPostBack('GridView2SegPerfil','Delete$" + b + "'); "));                    
                    }else if(PageIndex.Equals("Sistemas")){
                        grid.Rows[b].Cells[grid.Rows[b].Cells.Count-1].Attributes.Add("onclick", " " + JQueryConfirmDialogGridview("   javascript:__doPostBack('GridViewsistema','Delete$" + b + "'); "));                                      
                    }
                }
                
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grid.DataSource = ds;
                grid.DataBind();
                int columncount = grid.Rows[0].Cells.Count;
                grid.Rows[0].Cells.Clear();
                grid.Rows[0].Cells.Add(new TableCell());
                grid.Rows[0].Cells[0].ColumnSpan = columncount;
                grid.Rows[0].Cells[0].Text = "Sin registros";
            }
            if(PageIndex.Contains("Sistema")){
                ControlAccesoOpciones();
            }
                            
        }

        private String JQueryConfirmDialogGridview(String direccion) {
            return "  $(function () {"
                    + "event.preventDefault(); "
                    + " $('#dialog-confirm').dialog({"
                    + "   height: 200,"
                    + "  width: 300,"
                    + " modal: true,"
                    + " buttons: {"
                    + "   'Si': function () {"
                    + "     $(this).dialog('close');"
                    + direccion
                    + " },"
                    + "  'No': function () {"
                    + "     $(this).dialog('close'); return false;"
                    + " }"
                    + " }"
                    + " });  return false;"
                    + " });";
        }

        protected void gvbind()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * from "+PageIndex+" where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                  
                GridView1.DataSource = ds;
                GridView1.DataBind();
                for (int b = 0; b < GridView1.Rows.Count; b++)
                {
                    GridView1.Rows[b].Cells[GridView1.Rows[b].Cells.Count - 1].Attributes.Add("onclick", " " + JQueryConfirmDialogGridview("   javascript:__doPostBack('GridView1','Delete$" + b + "'); "));
                }
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                GridView1.DataSource = ds;
                GridView1.DataBind();
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                GridView1.Rows[0].Cells[0].Text = "Sin registros";
            }

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            gvbind();
            ViewState["Index"]=PageIndex;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ViewState["Index"] = PageIndex;
            gvbind();

        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            ViewState["Index"] = PageIndex;
            gvbind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TableCell cell = GridView1.Rows[e.RowIndex].Cells[0];
            conn.Open();
            SqlCommand cmd = new SqlCommand("update "+PageIndex+" set Estado='1' where ID"+PageIndex+"="+Convert.ToInt32(cell.Text), conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            ViewState["Index"] = PageIndex;
            gvbind();

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox Clave,Nombre, Desc, Cliente, FechaInicio, FechaFinEstimada, FechaFinReal, Tecnologias;
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            TableCell ID = GridView1.Rows[e.RowIndex].Cells[0];
            Clave = (TextBox)row.Cells[1].Controls[0];
            Nombre = (TextBox)row.Cells[2].Controls[0];
            Desc = (TextBox)row.Cells[3].Controls[0];
            Cliente = (TextBox)row.Cells[4].Controls[0];
            FechaInicio = (TextBox)row.Cells[6].Controls[0];
            FechaFinEstimada = (TextBox)row.Cells[7].Controls[0];
            FechaFinReal = (TextBox)row.Cells[8].Controls[0];
            Tecnologias = (TextBox)row.Cells[9].Controls[0];
            GridView1.EditIndex = -1;
            conn.Open();
            SqlCommand cmd = new SqlCommand("update " + PageIndex + " set Clave" + PageIndex + "='" + Clave.Text + "', Nombre = '" + Nombre.Text + "', Descripcion='" + Desc.Text.ToString() + "', Cliente='" + Cliente.Text + "', FechaInicio=Convert(datetime,'" + FechaInicio.Text + "',111) , FechaFinEstimada=Convert(datetime,'" + FechaFinEstimada.Text + "',111) , FechaFinReal=Convert(datetime,'" + FechaFinReal.Text + "',111) , Tecnologias='"+Tecnologias.Text+"' where ID"+PageIndex+"="+ID.Text+"  ",conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();



        }

        private bool registrar(String consulta) {
          bool  exito = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = consulta;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            conn.Open();
            if (cmd.ExecuteNonQuery() > 0) {
                exito=true;
            }
            return exito;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            int idcomponente = (DropDownListDep.SelectedValue.Length > 0 ? int.Parse(DropDownListDep.SelectedValue) : 0);
            if (PageIndex != null && PageIndex.Equals("Tareas")) {
                SqlCommand cmd = new SqlCommand();
               
                bool existe = accesDao.existeEnDB("select * from " + PageIndex + " s where s.idcomponentes=" + idcomponente + " and s.clavetareas='" + TextBoxClave.Text + "' and s.nombre='" + TextBoxNombre.Text.Trim() + "' and s.descripcion='" + TextBoxDescripcion.Text.Trim() + "' and s.cliente='" + TextBoxCliente.Text.Trim() + "' and s.horasestimadas=CONVERT(time,'" + DropDownListHoras.SelectedValue + ":00:00',0)");

                if (!existe)
                {
                    cmd.CommandText = "insert into " + PageIndex + " values(nullif(" + idcomponente + ",0), '" + TextBoxClave.Text.Trim() + "' , '" + TextBoxNombre.Text.Trim() + "' , '" + TextBoxDescripcion.Text.Trim() + "' , '" + TextBoxCliente.Text.Trim() + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) ,CONVERT(time,'" + DropDownListHoras.SelectedValue + ":00:00',0),CONVERT(time,'00:00:00',0),'', 0  )  ";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                
            
            }else 
            if (PageIndex!=null&&PageIndex.Equals("Proyectos"))
            {
                SqlCommand cmd = new SqlCommand();
                bool existe = accesDao.existeEnDB("select * from " + PageIndex + " s where s.clave" + PageIndex + "='" + TextBoxClave.Text + "' and s.nombre='" + TextBoxNombre.Text.Trim() + "' and s.descripcion='" + TextBoxDescripcion.Text.Trim() + "' and s.cliente='" + TextBoxCliente.Text.Trim() + "'");

                if (!existe)
                {
                    cmd.CommandText = "insert into " + PageIndex + " values('" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '04' , 0  )  ";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            else {
                SqlCommand cmd = new SqlCommand();
               String idcomponente2="";
                switch(PageIndex){
                    case "Requerimientos":
                        idcomponente2="proyecto";
                        break;
                    case "CasosUso":
                        idcomponente2 = "requerimientos";
                        break;
                    case "Componentes":
                        idcomponente2 = "casosuso";
                        break;
                
                }

                bool existe = accesDao.existeEnDB("select * from " + PageIndex + " s where s.id" + idcomponente2 + "=" + idcomponente + " and s.clave" + PageIndex + "='" + TextBoxClave.Text + "' and s.nombre='" + TextBoxNombre.Text.Trim() + "' and s.descripcion='" + TextBoxDescripcion.Text.Trim() + "' and s.cliente='" + TextBoxCliente.Text.Trim() + "'");

                if (!existe)
                {
                    cmd.CommandText = "insert into " + PageIndex + " values(  " + DropDownListDep.SelectedValue.ToString() + ", '" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '' , 0  )  ";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                 
            }            
            
            conn.Close();
            ViewState["Index"] = PageIndex;
            gvbind();
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from "+PageIndex+" where Clave"+PageIndex+" ='"+TextBoxClave.Text+"'    ", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                GridView1.DataSource = ds;
                GridView1.DataBind();
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                GridView1.Rows[0].Cells[0].Text = "Sin resultados de busqueda";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            gvbind();
        }

        protected void Button4seg_Click(object sender, EventArgs e){
        if (PageIndex != null)
            {
                if (PageIndex.Equals("Usuarios"))
                {
                    string tecs = TextAreaTecnologias.Value.ToString();
                    DataSet existe = accesDao.getDataset("select * from " + PageIndex + " u inner join  Personas  p "
                     + " on u.IDUsuario=p.IDUsuario where u.nombre='" + TextBoxUsuario.Text.Trim().ToLower() + "'"
                     + " and p.Nombre='" + TextBoxNomUsuario.Text.Trim() + "' and p.apellido='" + TextBoxApellidos.Text.Trim() + "' and p.Tecnologias='" + tecs.Trim() + "' and p.email='" + TextBoxEmail.Text.Trim().ToLower() + "'");

                    if (existe.Tables.Count>0)
                    {
                        if (existe.Tables[0].Rows.Count < 1)
                        {
                            long tel= 0;
                                if(TextBoxTelefono.Text.Trim().Length>0){
                                    tel = long.Parse(TextBoxTelefono.Text.Trim());
                                } 
                                //Hash user password
                                String passwordHash = BCrypt.Net.BCrypt.HashPassword(PasswordUs.Value.ToString(), BCrypt.Net.BCrypt.GenerateSalt(12));

                         bool exito = registrar("insert into " + PageIndex + "(Nombre,esEmpleado,Contraseña) values('" + TextBoxUsuario.Text.Trim().ToLower() + "','" + ((RadioButtonEmpleado.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "','" + passwordHash + "')"
                            + " insert into Personas(IDUsuario,Nombre,apellido,FechaRegistro,Tecnologias,Email,Telefono) values((select idusuario from Usuarios where IDUsuario=@@IDENTITY),"
                            + " '" + TextBoxNomUsuario.Text.Trim() + "','" + TextBoxApellidos.Text + "',GETDATE(),'" + tecs.Trim() + "','" + TextBoxEmail.Text.Trim().ToLower() + "', nullif(" + tel + ",0))"
                                );

                            if(exito){
                                limpiarFormUsuarios();
                            }
                                //accesDao.verificarLogin(PasswordUs.Value.ToString(), "$2a$12$iIPArq345DuIqku6fXqOHOgBAQBEYT2sTf7HsQswUVT8Atz4FrD3e", TextBoxUsuario.Text);
                        }
                    }
                    
                }else
                if (PageIndex.Equals("Grupos"))
                {
                    bool existe=accesDao.existeEnDB("select * from grupos g where g.nombre='"+TextBoxNomGrupo.Text.Trim()+"' and g.descripcion='"+TextBoxDescripcionGrupo.Text.Trim()+"'");
                    if (!existe)
                    {
                       bool exito=registrar("insert into " + PageIndex + "(Nombre,Descripcion) values('" + TextBoxNomGrupo.Text.Trim() + "', '" + TextBoxDescripcionGrupo.Text.Trim() + "')");
                       if (exito) {
                           limpiarFormGrupos();
                       }
                    }
                }else
                if (PageIndex.Equals("Perfiles"))
                {
                    bool existe = accesDao.existeEnDB("select * from perfiles p where p.nombre='" + TextBoxNomPerfil.Text.Trim() + "' and p.descripcion='" + TextBoxDescripcionPerfil.Text.Trim() + "' "
                     + " and p.usuarioAlta='" + ((RadioButtonListAltaPerfil.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "' and p.usuariobaja='" + ((RadioButtonListEliminarPerfil.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "' and p.usuariomodifica='" + ((RadioButtonListEliminarPerfil.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "'");
                    if (!existe)
                    {
                        bool exito = registrar("insert into " + PageIndex + "(Nombre,Descripcion,usuarioalta, usuariobaja,usuariomodifica) values('" + TextBoxNomPerfil.Text.Trim() + "', '" + TextBoxDescripcionPerfil.Text.Trim() + "','" + ((RadioButtonListAltaPerfil.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "', '" + ((RadioButtonListEliminarPerfil.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "', '" + ((RadioButtonListModificaPerfil.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "')");
                        if (exito)
                        {
                            limpiarFormPerfiles();
                        }
                    }
                }
                else
                    if (PageIndex.Equals("Sistemas"))
                    {
                        bool existe = accesDao.existeEnDB("select * from sistemas s where s.nombre='" + TextBoxNombreSis.Text.Trim() + "' and s.clavesistemas='" + TextBoxClaveSis.Text.Trim() + "' and s.cliente='" + TextBoxClienteSis.Text.Trim() + "'");
                         
                        if (!existe)
                        {
                            bool exito = registrar("insert into " + PageIndex + "(clavesistemas,Nombre,cliente,descripcion,fecharegistro, fechainicio,fechafinestimada, fechafinreal, tecnologias) "
                                + " values('" + TextBoxClaveSis.Text.Trim() + "', '" + TextBoxNombreSis.Text.Trim() + "','" + TextBoxClienteSis.Text.Trim() + "', '" + TextBoxDescSis.Text.Trim() + "', getdate(),CONVERT(DATE,'" + TextBoxFechaoIniSis.Text + "',20),CONVERT(DATE,'" + TextBoxFechaFinEsSis.Text + "',20),nullif(CONVERT(DATE,'"+TextBoxFinRealSis.Text.Trim()+"',20),'1900/01/01'),'" + TextBoxTecSistema.Text.Trim() + "')");
                            
                            if (exito)
                            {
                                limpiarFormSistemas();
                            }
                        }
                    }

                conn.Close();
                ViewState["Index"] = PageIndex;
                gvbindSeg();

            }
        }
        protected void Button5seg_Click(object sender, EventArgs e) {
            if(PageIndex.Equals("Usuarios")){
                string tecs = TextAreaTecnologias.Value.ToString();
                
                String consulta = "select u.IDUsuario,u.IDCatalogoProveedores,u.Nombre, u.esEmpleado,u.Contraseña,u.Estado,p.IDPersonas, p.IDUsuario,p.Nombre as nomUsuario,"
                + " p.Apellido,p.FechaRegistro,p.Tecnologias,p.Estado, p.Email, p.Telefono from " + PageIndex + " u inner join  Personas  p " 
                     + " on u.IDUsuario=p.IDUsuario where "
                     + " p.Nombre like '" + TextBoxNomUsuario.Text.Trim() + "%' and u.estado=0";
                if (TextBoxApellidos.Text.Trim().Length>0)
                {
                consulta=consulta+" and p.apellido like '" + TextBoxApellidos.Text.Trim() + "%'";
                }
                gvBindSegBusqueda(consulta,GridView2Seg);
            }else if(PageIndex.Equals("Grupos")){
                String consulta = "select * from grupos g where "
                     + " g.Nombre like '" + TextBoxNomGrupo.Text.Trim() + "%' and g.estado=0";
                if (TextBoxDescripcionGrupo.Text.Trim().Length > 0)
                {
                    consulta = consulta + " and g.descripcion like '" + TextBoxDescripcionGrupo.Text.Trim() + "%'";
                }
                gvBindSegBusqueda(consulta,GridView2SegGrupo);
            }
            else if (PageIndex.Equals("Perfiles"))
            {
                String consulta = "select * from perfiles p where "
                     + " p.Nombre like '" + TextBoxNomPerfil.Text.Trim() + "%' and p.estado=0";
                if (TextBoxDescripcionPerfil.Text.Trim().Length > 0)
                {
                    consulta = consulta + " and p.descripcion like '" + TextBoxDescripcionPerfil.Text.Trim() + "%'";
                }
                gvBindSegBusqueda(consulta, GridView2SegPerfil);
            }
            else if (PageIndex.Equals("Sistemas"))
            {
                String consulta = "select * from sistemas p where "
                     + " p.clavesistemas like '" + TextBoxClaveSis.Text.Trim() + "%' and p.estado=0";
                if (TextBoxDescripcionPerfil.Text.Trim().Length > 0)
                {
                    consulta = consulta + " and p.nombre like '" + TextBoxNombreSis.Text.Trim() + "%'";
                }
                gvBindSegBusqueda(consulta, GridViewsistema);
                ControlAccesoOpciones();
            }
        

        }

        protected void Button6seg_Click(object sender, EventArgs e) {
            gvbindSeg();
        }

        protected void ButtonUpdateSistemaseg_Click(object sender, EventArgs e)
        {
            Sistema sistema = new Sistema();
            sistema.idSistema = int.Parse(HidSistemaUpdate.Value);
            sistema.clave = TextBoxClaveSis.Text;
            sistema.nombre = TextBoxNombreSis.Text;
            sistema.descripcion = TextBoxDescSis.Text;
            sistema.cliente = TextBoxClienteSis.Text;
            sistema.fechaInicio = TextBoxFechaoIniSis.Text;
            sistema.fechaFinEstimada = TextBoxFechaFinEsSis.Text;
            sistema.fechaFinReal = TextBoxFinRealSis.Text;
            sistema.tecnologias = TextBoxTecSistema.Text;                       
            ProyectosWeb.Models.DbQueryResult resultado= _sistemaBL.UpdateSistemas(sistema);
            if (resultado.Success)
            {
                lblUpdateSistema.Text = "Actualización exitosa";
            }
            else {
                lblUpdateSistema.Text = "Actualización no exitosa :"+resultado.ErrorMessage;
                lblUpdateSistema.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void ButtonConsultaSistemasSeg_Click(object sender, EventArgs e)
        {
            mostrarSistemas();
            ControlAccesoOpciones();
        }        

        protected void ButtonCancelSistemaseg_Click(object sender, EventArgs e)
        {            
            mostrarSistemas();
            ControlAccesoOpciones();
        }

        protected void ButtonGuardarModuloSeg_Click(object sender, EventArgs e)
        {
            LabUpdateModulo.Text = ""; 
           foreach (ListItem item in CheckBoxListModulo.Items){
               if (item.Selected) {
                   string[] split = item.Value.Split(new Char[] { ',' });
                   Modulo modulo = new Modulo();
                   modulo.Nombre = item.Text.Trim();
                   modulo.descripcion = item.Text;
                   modulo.h3Id = split[0];
                   modulo.divId = split[1];
                   _modulosBl.registrarModulo(modulo);
               }           
            }
           updateListadoModulo();
        }

        private void updateListadoModulo(){
        for (int i = 0; i < CheckBoxListModulo.Items.Count; i++)
           {
               if (CheckBoxListModulo.Items[i].Selected)
               {
               string[] split = CheckBoxListModulo.Items[i].Value.Split(new Char[] { ',' });
               Modulo modulo = _modulosBl.getModulo(null,split[0], split[1],0); 
               
               if (modulo.idModulo > 1)
               {
                   if (modulo.estado == 0)
                   {
                       CheckBoxListModulo.Items[i].Attributes.Add("style", "color: green;");
                   }
                   else if (modulo.estado > 0)
                   {
                       CheckBoxListModulo.Items[i].Attributes.Add("style", "color: red;");
                   }
               }
               }
           }
        }

        private void agregarCheckboxmodulos(Modulo modulo,int conul,bool subop,Control principal, bool relAccesoPerfil) {
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.ID = limod+modulo.Nombre;
         
            principal.Controls.Add(li);
            CheckBox v = new CheckBox();
            v.Text = modulo.Nombre;
            v.ID = "CB1tree"+Modtree+"," + modulo.idModulo;
            li.Controls.Add(v);
            
            if (PageIndex.Equals("Sistemas y Módulos"))
            {
                sistemasModulosEnDB(v, int.Parse(HidUsuSeleccionadoSeg.Value), modulo.idModulo);
            }else{
            if (PageIndex.Equals("Acceso por Perfiles")) {
                perfilesModulosEnDB(v, int.Parse(HidUsuSeleccionadoSeg.Value), modulo.idModulo);
            }
            else if (PageIndex.Equals("Acceso por Usuarios")) {
                UsuariosModulosEnDB(v, int.Parse(HidUsuSeleccionadoSeg.Value), modulo.idModulo);
            };
            
            agregarCheckboxPantallas(li, modulo,conul,subop);
            }
        }
        private void agregarCheckboxPantallas(HtmlGenericControl licont,Modulo modulo, int conul,bool subop)
        {           
            Control cd = FindControl(modulo.divId);            
             
            int cont = 0;
            int op = 0;
            int valido = 0;
            int opsub = 0;
            int aux = 0;
            int tieneop = 0;
            int subOpcion = 0;
            subOpcionul = 0;
            int auxop4 = 0;
            int auxp4 = 0;
            HtmlGenericControl ulp4 = new HtmlGenericControl("ul");
            HtmlGenericControl liop5 = new HtmlGenericControl("li");
            foreach (Control c in cd.Controls)
            {
                valido = 0;
               
                string pantalaId = "";
                string pantalla = "";
                string opcionp = "";
                string indicePantalla = "";
                try
                {                    
                    if (c.GetType() == typeof(LinkButton))
                    {
                        pantalla = ((LinkButton)c).Text.Trim() + "";
                        pantalaId = "" + ((LinkButton)c).ClientID;
                        opcionp = "" + ((LinkButton)c).CommandName;
                        valido = 1;
                        indicePantalla = "" + ((LinkButton)c).CommandName;
                    }
                    if (c.GetType() == typeof(Label))
                    {
                        hidopcionpant.Value = "" + ((Label)c).Text.Trim();
                        hidpantallaid.Value = "" + ((Label)c).ClientID;
                        opcionp = "" + ((Label)c).Attributes["CommandName"];
                        valido = 1;
                        opsub = 0;                        
                    }
                    if (c.GetType() == typeof(Label) || c.GetType() == typeof(LinkButton))
                    {
                        tieneop++;                    
                    }
                    
                }
                catch
                {
                }
                string[] indexpantallas = indicePantalla.Split(new Char[] { ',' });
                int indexpantalla = 0;
                if (indexpantallas.Length > 2)
                {
                    indexpantalla = int.Parse(indexpantallas[2]);
                }

                if(tieneop==1){
                    HtmlGenericControl ul = new HtmlGenericControl("ul");
                    ul.ID = "ultree" + "" + conul + "" + modulo.idModulo;
                    licont.Controls.Add(ul);
                    tieneop++;
                }

                if (opcionp.Trim().Contains("op4"))
                {
                    cont++;
                    ulp4 = new HtmlGenericControl("ul");
                    ulp4.ID = "ultreep4" + "" + tieneop + "" + modulo.idModulo;
                    liop5 = new HtmlGenericControl("li");
                    liop5.ID = litreepant + "" + cont + "" + modulo.idModulo;
                    CheckBox vop = new CheckBox();
                    vop.Text = hidopcionpant.Value;
                    
                    vop.ID = hidpantallaid.Value + "," + cont + "," + modulo.idModulo + "," + indexpantalla.ToString();
                    liop5.Controls.Add(vop);
                    ulp4.Controls.Add(liop5);
                    pantallasRegistradas(vop, hidopcionpant.Value, hidpantallaid.Value);
                    auxp4 = tieneop;
                    opsub = 2;
                    op++;                    
                }

                if (opcionp.Trim().Contains("sub") || opcionp.Trim().Contains("op5"))
                {
                    opsub++;
                    HtmlGenericControl li;
                    if (opsub == 1)
                    {
                        cont++;
                        li = new HtmlGenericControl("li");                   
                        li.ID = litreepant + "" + cont + "" + modulo.idModulo;                        
                            FindControl("ultree" + "" + conul + "" + modulo.idModulo).Controls.Add(li);                       
                        CheckBox v = new CheckBox();
                        v.Text = hidopcionpant.Value;
                        
                        v.ID = hidpantallaid.Value + "," + "" + cont + "" + opsub + "," + modulo.idModulo+","+indexpantalla.ToString();
                        pantallasRegistradas(v,hidopcionpant.Value, hidpantallaid.Value);                        
                            li.Controls.Add(v);                                                
                        aux = cont;
                    }
                    else {

                        li = (FindControl(litreepant + "" + aux + "" + modulo.idModulo) as HtmlGenericControl);
                    }
                    
                        if (pantalla.Trim().Length > 0)
                        {
                            op++;
                            cont++;
                            HtmlGenericControl ulop = new HtmlGenericControl("ul");
                            ulop.ID = "ultreeop" + "" + op + "" + modulo.idModulo;
                            if (opcionp.Trim().Contains("op5"))
                            {
                                li.Controls.Add(ulp4);
                                liop5.Controls.Add(ulop);
                            }
                            else{
                            li.Controls.Add(ulop);
                             }
                            HtmlGenericControl liop = new HtmlGenericControl("li");                            
                            liop.ID = litreepant + "" + cont + "" + modulo.idModulo;                            
                                ulop.Controls.Add(liop);                            
                            CheckBox vop = new CheckBox();
                            vop.Text = pantalla;
                           
                            vop.ID = pantalaId + "," + op + "," + modulo.idModulo + "," + indexpantalla;                            
                          
                            pantallasRegistradas(vop, pantalla, pantalaId);
                            liop.Controls.Add(vop);

                            if (subop)
                            {
                                agregarCheckboxOpciones(liop, pantalla, modulo.idModulo, opcionp, vop.ID, pantalaId, indicePantalla);                                
                            }
                        }                                       
                }
                else if (!opcionp.Trim().Contains("sub")&&!opcionp.Trim().Contains("op5")&&(pantalla.Trim().Length > 0) && valido == 1)
                {                    
                    cont++;
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.ID = litreepant + "" + cont + "" + modulo.idModulo;
                    FindControl("ultree" + "" + conul + "" + modulo.idModulo).Controls.Add(li);
                    CheckBox v = new CheckBox();
                    v.Text = pantalla;                                                   
                    v.ID = pantalaId + "," + cont + "," + modulo.idModulo+","+indexpantalla;
                    pantallasRegistradas(v,pantalla,pantalaId);
                    li.Controls.Add(v);

                    if (subop)
                    {
                        agregarCheckboxOpciones(li, pantalla, modulo.idModulo, opcionp, v.ID, "", indicePantalla);
                    }
                }
                
            }
            contCBKSTotal =contCBKSTotal+cont;
            hidcontchecks.Value = contCBKSTotal.ToString();
           
        }
        private HtmlGenericControl agregarOpcionCHKBOX(String idLiUlparent, String idLi, String nomChkbox, String idChkbox)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.ID = idLi;
            FindControl(idLiUlparent).Controls.Add(li);
            CheckBox v = new CheckBox();
            v.Text = nomChkbox;
            v.ID = idChkbox;
            li.Controls.Add(v);
            return li;
        }
        private void agregarSubOpcionCHKBOX(String ulId, HtmlGenericControl liParent, String idliSub, String nomCHKBox, String idChkBox)
        {
            HtmlGenericControl ulop = new HtmlGenericControl("ul");
            ulop.ID = ulId;
            liParent.Controls.Add(ulop);
            HtmlGenericControl liop = new HtmlGenericControl("li");
            liop.ID = idliSub;
            ulop.Controls.Add(liop);
            CheckBox vop = new CheckBox();
            vop.Text = nomCHKBox;
            vop.ID = idChkBox;
            liop.Controls.Add(vop);
        }

        private void pantallasRegistradas(CheckBox pregis,String nombre, String idasp) {
            
            if (PageIndex != null)
            {
                Pantalla pid = _pantallaBL.getPantalla(null, idasp);
                if (PageIndex.Equals("Pantallas"))
                {
                    pregis.ForeColor = System.Drawing.Color.Blue;                    
                }
                if (pid.idPantalla > 0)
                {
                    pregis.ID = pregis.ID + "," + pid.idPantalla;
                    if (PageIndex.Equals("Pantallas"))
                    {                        
                        if(pid.estado>0){
                            pregis.ForeColor = System.Drawing.Color.Red;
                        }else{
                            pregis.Checked = true;
                            pregis.ForeColor = System.Drawing.Color.Green;                            
                        }
                    }
                    else if (PageIndex.Equals("Acceso por Perfiles"))
                    {
                        perfilesPantallasEnDB(pregis, int.Parse(HidUsuSeleccionadoSeg.Value), pid.idPantalla);
                    }else if (PageIndex.Equals("Acceso por Usuarios"))
                    {
                        UsuariosPantallasEnDB(pregis, int.Parse(HidUsuSeleccionadoSeg.Value), pid.idPantalla);
                    }

                    
                }                
            }
        }        

        private void OpcionesRegistradas(CheckBox pregis, String idasp, String idcheckbox)
        {
            if (PageIndex != null)
            {
                if (PageIndex.Equals("Opciones"))
                {
                    pregis.ForeColor = System.Drawing.Color.Blue;                    
                }
                    Opcion oid = _opcionBL.getOpcion(idasp, idcheckbox);
                    if (oid.idPantalla > 0)
                    {
                        pregis.ID = pregis.ID + "," + oid.idOpcion;
                        if (PageIndex.Equals("Opciones"))
                        {                            
                            if(oid.estado>0){
                             pregis.ForeColor = System.Drawing.Color.Red;
                            }else{
                                pregis.Checked = true;
                                pregis.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                        else if (PageIndex.Equals("Acceso por Perfiles"))
                        {
                            perfilesOpcionesEnDB(pregis,oid.idOpcion, int.Parse(HidUsuSeleccionadoSeg.Value));
                        }
                        else if (PageIndex.Equals("Acceso por Usuarios"))
                        {
                            UsuariosOpcionesEnDB(pregis, oid.idOpcion, int.Parse(HidUsuSeleccionadoSeg.Value));
                        }                        
                }
            }
        }

        private void perfilesModulosEnDB(CheckBox regis, int idperfil, int idmodulo)
        {
            PerfilesModulos pmod = _PerfilesModulosBL.getPerfilModulo(idperfil, idmodulo);
            if (pmod.idPerfilModulo > 0)
            {
                regis.ID = regis.ID + "," + pmod.idPerfilModulo;

                if (bool.Parse(pmod.divVisible))
                {
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void perfilesPantallasEnDB(CheckBox regis, int idperfil, int idpantalla)
        {
            PerfilesPantallas ppantalla = _PerfilesPantallasBL.getPerfilPantalla(idperfil, idpantalla);
            if (ppantalla.idPerfilPantalla > 0)
            {
                regis.ID = regis.ID + "," + ppantalla.idPerfilPantalla;

                if (bool.Parse(ppantalla.visible))
                {
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void perfilesOpcionesEnDB(CheckBox regis, int idOpcion, int idPerfil)
        {
            PerfilesOpciones perfilopcion = _PerfilesOpcionesBL.getPerfilOpcion(idOpcion, idPerfil);
            if (perfilopcion.idPerfilOpcion > 0)
            {
                regis.ID = regis.ID + "," + perfilopcion.idPerfilOpcion;

                if (bool.Parse(perfilopcion.visible))
                {
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void UsuariosModulosEnDB(CheckBox regis, int idusuario, int idmodulo)
        {
           UsuariosModulos umod = _UsuariosModulosBL.getUsuarioModulo(idusuario, idmodulo);
           if (umod.idUsuarioModulo > 0)
            {
                regis.ID = regis.ID + "," + umod.idUsuarioModulo;

                if (bool.Parse(umod.divVisible))
                {
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void UsuariosPantallasEnDB(CheckBox regis, int idusuario, int idpantalla)
        {
            UsuariosPantallas usuariopantalla = _UsuariosPantallasBL.getUsuarioPantalla(idusuario, idpantalla);
            if (usuariopantalla.idUsuarioPantalla > 0)
            {
                regis.ID = regis.ID + "," + usuariopantalla.idUsuarioPantalla;

                if (bool.Parse(usuariopantalla.visible))
                {
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void UsuariosOpcionesEnDB(CheckBox regis, int idOpcion, int idUsuario)
        {
            UsuariosOpciones perfilopcion = _UsuariosOpcionesBL.getUsuarioOpcion(idOpcion, idUsuario);
            if (perfilopcion.idUsuarioOpcion > 0)
            {
                regis.ID = regis.ID + "," + perfilopcion.idUsuarioOpcion;

                if (bool.Parse(perfilopcion.visible))
                {
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void sistemasModulosEnDB(CheckBox regis, int idsistema, int idmodulo)
        {
            SistemasModulos sismod = _SistemasModulosBL.getSistemaModulo(idsistema, idmodulo);
            if (sismod.idSistemaModulo > 0)
            {
                regis.ID = regis.ID + "," + sismod.idSistemaModulo;
                if(bool.Parse(sismod.divvisible)){
                    regis.Checked = true;
                    regis.ForeColor = System.Drawing.Color.Green;                
                }
                }
        }

        private void agregarCheckboxOpciones(HtmlGenericControl liPantalla, String nompantalla, int idmodulo, String comando, String idpantalla, String subop3,String indicePantalla)
        {
            String opcion = "";
            if(comando.ToLower().Contains("usuarios")){                   
                    opcion = "opcionesRegSeg," + MultiView2SegGrid.Views[0].ClientID; 
            }
            if (comando.ToLower().Contains("grupos"))
            {
                opcion = "opcionesRegSeg," + MultiView2SegGrid.Views[1].ClientID;
            }
            if (comando.ToLower().Contains("perfiles"))
            {
               opcion = "opcionesRegSeg," + MultiView2SegGrid.Views[2].ClientID;
            }
            if (comando.ToLower().Contains("relaciones"))
            {
                opcion = MultiView1Seg.Views[3].ClientID;
            }
            if (comando.ToLower().Contains("sistemas"))
            {
                opcion = "opcionesRegSeg," + MultiView2SegGrid.Views[3].ClientID + ",opcionesUpdateSistem";
            }
            else if (comando.ToLower().Contains("camodulo"))
            {
                opcion = MultiView1Seg.Views[9].ClientID;
            }
            else if (comando.ToLower().Contains("capantalla") || comando.ToLower().Contains("caopcion"))
            {
                opcion = MultiView1Seg.Views[10].ClientID;
            }
            else if (comando.ToLower().Contains("opciontarea"))
            {
                opcion = "opcionesRegTarea," + MultiViewTareaGrid.Views[0].ClientID;
            }
            else if (comando.ToLower().Contains("op4"))
            {
                opcion = subop3;
            }
            else if (comando.ToLower().Contains("op5"))
            {
                string[] splitidview = comando.Split(new Char[] { ',' });
                if (splitidview.Length>1)
                {
                    opcion = MultiView1Seg.Views[int.Parse(splitidview[1])].ClientID;
                }                
            }
            else if (comando.ToLower().Contains("seguimientotarea"))
            {
                opcion = MultiViewSeguimientoTarea.Views[0].ClientID;
            }
            


            string[] indicepantallas = idpantalla.Split(new Char[] { ',' });
            string idpantallaop = "";
            if (indicepantallas.Length > 4)
            {
                idpantallaop = indicepantallas[4];
            }
            
            //agregado indicepantalla

            if (opcion.Trim().Length > 0)
            {
                string[] splitOps = opcion.Split(new Char[] { ',' });

                for (int num = 0; num < splitOps.Length; num++)
                {
                    Control cd = FindControl(splitOps[num]);

                    if (cd != null)
                    {
                        int ulcontsub = 0;
                        if (cd.Controls.Count>0)
                        {
                        foreach (Control c in cd.Controls)
                        {
                            string subOpcionId = "";
                            string subOpcion = "";
                            string opcionp = "";
                            int[] indices = new int[2];
                            string[] opGrid = new string[2];
                            try
                            {
                                if (c.GetType() == typeof(Button))
                                {
                                    subOpcion = ((Button)c).Text.Trim() + ",";
                                    subOpcionId = ((Button)c).ClientID + ",";
                                    opcionp = "" + ((Button)c).CommandName;
                                }
                                if (c.GetType() == typeof(FileUpload))
                                {
                                    subOpcion ="Seleccionar Archivo,";
                                    subOpcionId = (c as FileUpload).ClientID + ",";
                                    //opcionp = "" + ((Button)c).CommandName;
                                }
                                if (c.GetType() == typeof(GridView))
                                {
                                    GridView cs = (c as GridView);
                                    subOpcionId = cs.ClientID + ",";
                                    int col = cs.Columns.Count;
                                    if (col > 2)
                                    {
                                        if (cs.Columns[col - 1].GetType() == typeof(CommandField))
                                        {
                                            subOpcion = "Eliminar";
                                            subOpcionId = subOpcionId + "" + (col - 1);
                                        }
                                        if (cs.Columns[col - 2].GetType() == typeof(CommandField))
                                        {
                                            subOpcion = subOpcion + ",Editar";
                                            subOpcionId = subOpcionId + "," + (col - 2);
                                        }

                                    }
                                }
                            }
                            catch
                            {
                            }


                            if (subOpcion.Trim().Length > 0)
                            {
                                string[] split = subOpcion.Split(new Char[] { ',' });
                                string[] splitid = subOpcionId.Split(new Char[] { ',' });
                                
                                if (ulcontsub == 0)
                                {
                                    HtmlGenericControl ulop = new HtmlGenericControl("ul");
                                    ulop.ID = "ultreesubop" + ulcontsub + "" + idmodulo + "" + subOpcionul;
                                    liPantalla.Controls.Add(ulop);
                                }

                                for (int ps = 0; ps <= (split.Length - 1); ps++)
                                {
                                    if (split[ps].Length > 0)
                                    {
                                        ulcontsub++;
                                        
                                        HtmlGenericControl lisuop = new HtmlGenericControl("li");
                                        lisuop.ID = litreesubop + "" + countOpcion + "" + idmodulo;
                                        FindControl("ultreesubop" + "0" + "" + idmodulo + "" + subOpcionul).Controls.Add(lisuop);
                                        CheckBox vop = new CheckBox();
                                        vop.Text = split[ps];
                                        vop.ID = idpantallaop + "," + splitid[0] + "," + splitid[ps + 1] + "," + ulcontsub + "," + subOpcionul;
                                        OpcionesRegistradas(vop, splitid[0], idpantallaop  + splitid[0]+ splitid[ps + 1] );
                                        lisuop.Controls.Add(vop);
                                        countOpcion++;
                                    }
                                }

                            }
                        }
                        }
                        
                        subOpcionul++;
                    }
                }
            }
            hidcontchecksubop.Value = countOpcion.ToString();
        }

        private void agregarOpcionSubop(String opcionp, int opsub, String liidop,String ulparentid, String nomCheck,String chkboxid) { 
                
        }

        protected void ButtonAtualizaPantallasSeg_Click(object sender, EventArgs e)
        {            
            abdCheckBoxTree(false,true,false,false,false);
        }

        protected void ButtonDeletePantallaOpcion_Click(object sender, EventArgs e)
        {            
            abdCheckBoxTree(false,false,false,false,true);
        }
        

        private void llenarTree(bool subopciones, bool relAccesoPerfilb) {
            Control divTree = new Control();
            if (PageIndex != null)
            {
                if (PageIndex.Trim().Equals("Acceso por Perfiles") || PageIndex.Trim().Equals("Acceso por Usuarios"))
                {
                    FindControl(ulModSis.ID).Controls.Clear();
                    FindControl(ulconttree.ID).Controls.Clear();
                    divTree = FindControl(ulTreeAccePerfil.ID);
                 divTree.Controls.Clear();
                }else if ( PageIndex.Trim().Equals("Sistemas y Módulos"))
                {
                    FindControl(ulconttree.ID).Controls.Clear();
                    FindControl(ulTreeAccePerfil.ID).Controls.Clear();
                    divTree = FindControl(ulModSis.ID);
                    divTree.Controls.Clear();
                }
                else {
                    FindControl(ulModSis.ID).Controls.Clear();
                    FindControl(ulTreeAccePerfil.ID).Controls.Clear();
                   divTree = FindControl(ulconttree.ID);
                   divTree.Controls.Clear();
                }
            }

            divTree.Controls.Clear();            

            int cont = 0;
            
            if(PageIndex.Trim().Equals("Acceso por Perfiles") || PageIndex.Trim().Equals("Acceso por Usuarios")){
                List<SistemasModulos> sistema=new List<SistemasModulos>();
                if (userName.Equals("admin"))
                {
                 List<Sistema>  sis= _sistemaBL.getSistemas();
                  Sistema  sisSICT=sis.Find(x => x.clave.Trim().ToLower() == "sict");
                    sistema=   _SistemasModulosBL.getSistemasModulos(sisSICT.idSistema,0);
                }else{
                sistema= _sistemamodulo.sistemasModulos;
                }                 
                //_sistemamodulo.sistemasModulos.Count;
                    for (int sm = 0; sm < sistema.Count; sm++)
                    {
                        cont++;
                        Modulo d = sistema[sm].modulo;
                        if (bool.Parse(sistema[sm].divvisible) && bool.Parse(sistema[sm].h3visible))
                        {
                            agregarCheckboxmodulos(d, cont, subopciones, divTree, relAccesoPerfilb);
                        }
                    }                                   
            }else{
            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;
                agregarCheckboxmodulos(d, cont, subopciones, divTree, relAccesoPerfilb);
            }
            }
        }
        protected void ButtonRegistrarPantallasSeg_Click(object sender, EventArgs e)
        {
            abdCheckBoxTree(true,false,false,false,false);
        }
        private void abdCheckBoxTree(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs, bool delete) {
            if (PageIndex != null)
            {
                if (PageIndex.Trim().ToLower().Equals("pantallas"))
                {
                    insertUpdatePantallas(registra, actualiza, relAccesoperfil, relaccesoUs, delete);
                }
                else if (PageIndex.Trim().ToLower().Equals("opciones"))
                {
                    insertUpdateOpciones(registra, actualiza, relAccesoperfil, relaccesoUs, delete);
                }
            }
        }        

        private void RegModulosRelAcceso(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs)
        {
            LblStatusAccePerfil.Text = "";
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();
           
            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;
                
                Control c = FindControl(limod + d.Nombre); 
                    if (c != null)
                    {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox modulo = (c.Controls[0] as CheckBox);
                            
                                checkstree++;
                                string[] split = modulo.ClientID.Split(new Char[] { ',' });
                                if (split[1].Trim().Length > 0)
                                {
                                    if(PageIndex.Equals("Acceso por Perfiles"))
                                    {
                                        PerfilesModulos pmodulo = new PerfilesModulos();
                                        pmodulo.idModulo = int.Parse(split[1]);
                                        pmodulo.h3Visible = modulo.Checked.ToString();
                                        pmodulo.divVisible = modulo.Checked.ToString();
                                        int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                        pmodulo.idPerfil = idSeleecionado;
                                        if (registra)
                                        {
                                            if (pmodulo.idPerfil > 0)
                                            {
                                                _PerfilesModulosBL.registrarPerfilesModulos(pmodulo);

                                            }
                                        }
                                        else if (actualiza && (split.Length > 2))
                                        {
                                            if (split[2].Trim().Length > 0)
                                            {
                                                pmodulo.idPerfilModulo = int.Parse(split[2]);
                                                actualizados.Add(_PerfilesModulosBL.UpdatePerfilesModulos(pmodulo).Success + "," + d.Nombre);
                                            }

                                        }
                                    }
                                    else if(PageIndex.Equals("Acceso por Usuarios")) {
                                        UsuariosModulos umodulo = new UsuariosModulos();
                                        umodulo.idModulo = int.Parse(split[1]);
                                        umodulo.h3Visible = modulo.Checked.ToString();
                                        umodulo.divVisible = modulo.Checked.ToString();
                                        int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                        umodulo.idUsuario = idSeleecionado;
                                        if (registra)
                                        {
                                            if (umodulo.idUsuario > 0)
                                            {
                                                _UsuariosModulosBL.registrarUsuariosModulos(umodulo);
                                            }
                                        }
                                        else if (actualiza && (split.Length > 2))
                                        {
                                            if (split[2].Trim().Length > 0)
                                            {
                                                umodulo.idUsuarioModulo = int.Parse(split[2]);
                                                actualizados.Add(_UsuariosModulosBL.UpdateUsuariosModulos(umodulo).Success + "," + d.Nombre);
                                            }

                                        }
                                    }
                                    else if (PageIndex.Equals("Sistemas y Módulos"))
                                    {                                        
                                        SistemasModulos sistemamodulo = new SistemasModulos();
                                        sistemamodulo.idModulo = int.Parse(split[1]);
                                        sistemamodulo.divvisible = modulo.Checked.ToString();
                                        sistemamodulo.h3visible = modulo.Checked.ToString();
                                        int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                        sistemamodulo.idSistema = idSeleecionado;
                                        if (registra)
                                        {
                                            if (sistemamodulo.idSistema > 0)
                                            {
                                                _SistemasModulosBL.registrarSistemasModulos(sistemamodulo);
                                            }
                                        }
                                        else if (actualiza && (split.Length > 2))
                                        {
                                            if (split[2].Trim().Length > 0)
                                            {
                                                sistemamodulo.idSistemaModulo = int.Parse(split[2]);
                                                actualizados.Add(_SistemasModulosBL.UpdateSistemasModulos(sistemamodulo).Success + "," + d.Nombre);
                                            }

                                        }
                                        
                                    }
                                }
                            
                        }                    
                }
            }
            if (PageIndex.Equals("Sistemas y Módulos"))
            {
                llenarTree(false, false);
            }else{
                insertUpdatePerfilesPantallas(registra, actualiza, true, false);
                insertUpdatePerfilesOpciones(registra, actualiza, true, false);
                llenarTree(true, relAccesoperfil);            
            
            if (actualiza && (checkstree > 0))
            {
                updateMsg(actualizados, LblStatusAccePerfil, "Modulos no actualizados");
            }
            }
            
        }

        private void insertUpdatePantallas(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs, bool delete)
        {
            LblupdatePantalla.Text = "";
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();

            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;

                for (int chk = 1; chk <= int.Parse(hidcontchecks.Value); chk++)
                {

                    Control c = FindControl(litreepant + "" + chk + "" + d.idModulo);
                    if (c != null)
                    {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox pantalla = (c.Controls[0] as CheckBox);
                            if (!pantalla.UniqueID.Contains(Modtree) && pantalla.Checked &&(delete==false))
                            {
                                checkstree++;
                                string[] split = pantalla.ClientID.Split(new Char[] { ',' });
                                if (split[2].Trim().Length > 0)
                                {
                                    Pantalla panta = new Pantalla();
                                    panta.nombre = pantalla.Text;
                                    panta.descripcion = pantalla.Text + " " + (c.Parent.Parent.Controls[0] as CheckBox).Text;
                                    panta.idAsp = split[0];
                                    panta.pantallaIndex = int.Parse(split[3]);
                                    if(registra){                                    
                                        panta.idModulo = int.Parse(split[2]);
                                        _pantallaBL.registrarPantalla(panta);                                        
                                    }else{
                                        if(split.Length>4){
                                            if (split[4].Trim().Length > 0)
                                            {
                                                panta.idPantalla = int.Parse(split[4]);
                                                if (actualiza)
                                                {                                                    
                                                    actualizados.Add(_pantallaBL.UpdatePantalla(panta).Success + "," + panta.nombre);
                                                }                                               
                                            }
                                        }
                                    }
                                }
                            }
                            if (delete)
                            {
                                eliminarPantallasOpciones(c);
                            }
                        }                        
                           
                    }
                }
            }
            if (registra)
            {
                llenarTree(false,false);
            }
            else if(actualiza&&(checkstree>0))
            {
                updateMsg(actualizados,LblupdatePantalla, "Las siguientes Pantallas no se actualizaron");                
            }
        }
        private void eliminarPantallasOpciones(Control c) {            
                CheckBox pantalla = (c.Controls[0] as CheckBox);
                if (pantalla.Checked)
                {
                    eliminarOp = new eliminarCheckboxs();
                    subTotalCHK = 0;
                    eliminarCheckboxs listado = EliminarChkBoxRecursivo(c.Controls);
                    int totalchkhijo = 0;
                    for (int cp = listado.subopcParentList.Count - 1; cp >= 0; cp--)
                    {

                        for (int ch = 0; ch < listado.subopcParentList[cp].subopcParent.Count; ch++)
                        {
                            CheckBox chkhijo = listado.subopcParentList[cp].subopcParent[ch].opciones;
                            if (chkhijo.Checked)
                            {
                                totalchkhijo++;
                                EliminaOpcionesTree(chkhijo.ClientID);
                            }
                        }
                        if (listado.subopcParentList[cp].subopcParent.Count == totalchkhijo)
                        {
                            totalchkhijo = 0;
                            EliminaOpcionesTree(listado.subopcParentList[cp].opciones.ClientID);
                        }
                    }
                }
            
        }

        private void EliminaOpcionesTree(String idComponente) {
            string[] split = idComponente.Split(new Char[] { ',' });
            if(PageIndex.Equals("Pantallas")){
            if (split.Length > 4)
            {
                if (split[4].Trim().Length > 0)
                {
                    _pantallaBL.DeletePantalla(int.Parse(split[4]));
                }
            }
            }
            else if (PageIndex.Equals("Opciones"))
            {
                if (split.Length > 5)
                {
                    if (split[5].Trim().Length > 0)
                    {
                        _opcionBL.DeleteOpcion(int.Parse(split[5]));
                    }
                }
            }
        }

       public class eliminarCheckboxs{       
            public List<eliminarSubCheckboxs> subopcParentList = new List<eliminarSubCheckboxs>();            
        }

        public class eliminarSubCheckboxs
        {
            public CheckBox opciones = new CheckBox();
            public List<eliminarSubCheckboxsParent> subopcParent = new List<eliminarSubCheckboxsParent>();
        }
        public class eliminarSubCheckboxsParent
        {
            public CheckBox opciones = new CheckBox();
        }

        private eliminarCheckboxs EliminarChkBoxRecursivo(ControlCollection controles) {
          
            for (int c = 0; c < controles.Count;c++)
            {
                Control con=controles[c];
                if (con.GetType() == typeof(HtmlGenericControl) && (con as HtmlGenericControl).TagName.ToLower().Equals("ul"))
                {
                    subTotalCHK = 2;
                    EliminarChkBoxRecursivo(con.Controls);                    
                }
                if (con.GetType() == typeof(HtmlGenericControl) && (con as HtmlGenericControl).TagName.ToLower().Equals("li"))
                {
                 ControlCollection   subcontroles = con.Controls;
                    for (int co = 0; co < subcontroles.Count; co++)
                    {
                        Control controlUl = subcontroles[co];
                        if (controlUl.GetType() == typeof(HtmlGenericControl) && (controlUl as HtmlGenericControl).TagName.ToLower().Equals("ul"))
                        {
                            subTotalCHK = 0;
                        }
                    }
                    EliminarChkBoxRecursivo(con.Controls);
                }
                if (con.GetType() == typeof(CheckBox))
                {                    
                    if ((subTotalCHK!=2))
                    {                        
                        eliminarSubCheckboxs cd=new eliminarSubCheckboxs();
                        cd.opciones=(con as CheckBox);
                        eliminarOp.subopcParentList.Add(cd);
                        indexParentEliminar= eliminarOp.subopcParentList.Count-1;
                    }
                    else
                    {                        
                        eliminarSubCheckboxsParent cd = new eliminarSubCheckboxsParent();
                        cd.opciones=(con as CheckBox);
                        eliminarOp.subopcParentList[indexParentEliminar].subopcParent.Add(cd);                       
                    }                                       
                }
            }
            return eliminarOp;
        }

         private string AnalizarJerarquia(Control control) {
             if (control.Parent.Controls[0].GetType() != typeof(View))
             {
                 if (checkboxParent!=0 &&(checkboxParent % 3) == 0)
                 {
                     if (control.Parent.Controls[0].Controls[0].GetType() == typeof(CheckBox))
                     {
                         analizador = (control.Parent.Controls[0].Controls[0] as CheckBox).ClientID + "," + (checkboxParent / 3) + "," + subTotalCHK;
                     }
                 }
                 if (control.Parent.Controls.Count > 0)
                 {
                     checkboxParent++;
                     AnalizarJerarquia(control.Parent);
                 }                 
             }            
            return analizador;
        }

        private void insertUpdateOpciones(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs, bool delete)
        {
            LblupdatePantalla.Text = "";
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();

            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;

                for (int chk = 0; chk <= int.Parse(hidcontchecksubop.Value); chk++)
                {

                    Control c = FindControl(litreesubop + "" + chk + "" + d.idModulo);
                    if (c != null)
                    {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox opciones = (c.Controls[0] as CheckBox);
                            if (opciones.Checked)
                            {
                                checkstree++;
                                string[] split = opciones.ClientID.Split(new Char[] { ',' });
                                if (split[1].Trim().Length > 0 && delete==false)
                                {
                                    Opcion op = new Opcion();
                                    op.nombre = opciones.Text;
                                    op.descripcion = opciones.Text +" "+ (c.Parent.Parent.Controls[0] as CheckBox).Text;
                                    op.idAsp = split[1];
                                    op.componenteIndex = split[2].Trim();
                                    op.chkboxTreeindex = chk;
                                    op.idcheckbox = split[0] + split[1] + split[2];
                                    if (split[0].Length > 0)
                                    {
                                        op.idPantalla = int.Parse(split[0]);
                                        if (registra)
                                        {
                                            _opcionBL.registrarOpcion(op);
                                        }
                                        else
                                            if (actualiza&&(split.Length > 5))
                                            {
                                                if (split[5].Trim().Length > 0)
                                                {
                                                    op.idOpcion = int.Parse(split[5]);
                                                    actualizados.Add(_opcionBL.UpdateOpcion(op).Success + "," + (c.Parent.Parent.Controls[0] as CheckBox).Text + " > " + op.nombre);
                                                }
                                            }
                                    }
                                }
                            }
                            if (delete)
                            {
                                eliminarPantallasOpciones(c);
                            }
                        }
                        
                    }
                }
            }
            if (registra)
            {
                llenarTree(true,false);
            }
            else if (actualiza&&(checkstree > 0))
            {
                updateMsg(actualizados, LblupdatePantalla, "Las siguientes Opciones no se actualizaron");
            }
        }

        private void insertUpdatePerfilesPantallas(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs)
        {
            
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();

            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;

                for (int chk = 1; chk <= int.Parse(hidcontchecks.Value); chk++)
                {

                    Control c = FindControl(litreepant + "" + chk + "" + d.idModulo);
                    if (c != null)
                    {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox pantalla = (c.Controls[0] as CheckBox);

                            checkstree++;
                            string[] split = pantalla.ClientID.Split(new Char[] { ',' });
                            if (split.Length > 3)
                            {
                                if (split[3].Trim().Length > 0)
                                {
                                    if(PageIndex.Equals("Acceso por Perfiles"))
                                    {
                                    PerfilesPantallas ppantalla = new PerfilesPantallas();
                                    
                                    ppantalla.visible = pantalla.Checked.ToString();
                                    int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                    ppantalla.idPerfil = idSeleecionado;

                                    if (registra)
                                    {
                                        if ( split.Length > 4)
                                        {
                                            if (split[4].Trim().Length > 0)
                                            {
                                                ppantalla.idPantalla = int.Parse(split[4]);
                                                if (ppantalla.idPerfil > 0)
                                                {
                                                    _PerfilesPantallasBL.registrarPerfilesPantallas(ppantalla);
                                                }
                                            }
                                        }
                                    }
                                    else if (actualiza && split.Length >5 )
                                    {
                                        if (split[5].Trim().Length > 0)
                                        {
                                            ppantalla.idPantalla = int.Parse(split[4]);
                                            ppantalla.idPerfilPantalla = int.Parse(split[5]);
                                            actualizados.Add(_PerfilesPantallasBL.UpdatePerfilesPantallas(ppantalla).Success+","+pantalla.Text);
                                        }
                                    }
                                    }
                                    else if (PageIndex.Equals("Acceso por Usuarios"))
                                    {
                                        UsuariosPantallas usuariopantalla = new UsuariosPantallas();
                                        
                                        usuariopantalla.visible = pantalla.Checked.ToString();
                                        int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                        usuariopantalla.idUsuario = idSeleecionado;

                                        if (registra)
                                        {
                                            if (split.Length > 4)
                                            {
                                                if (split[4].Trim().Length > 0)
                                                {
                                                    usuariopantalla.idPantalla = int.Parse(split[4]);
                                                    if (usuariopantalla.idUsuario > 0)
                                                    {
                                                        _UsuariosPantallasBL.registrarUsuariosPantallas(usuariopantalla);
                                                    }
                                                }
                                            }
                                        }
                                        else if (actualiza && split.Length > 5)
                                        {
                                            if (split[5].Trim().Length > 0)
                                            {
                                                usuariopantalla.idPantalla = int.Parse(split[4]);
                                                usuariopantalla.idUsuarioPantalla = int.Parse(split[5]);
                                                actualizados.Add(_UsuariosPantallasBL.UpdateUsuariosPantallas(usuariopantalla).Success + "," + pantalla.Text);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            
             if (actualiza && (checkstree > 0))
            {
                updateMsg(actualizados, LblStatusAccePerfil, "Pantallas no actualizadas");
            }
                
        }

        private void insertUpdatePerfilesOpciones(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs)
        {          
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();

            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;

                for (int chk = 0; chk <= int.Parse(hidcontchecksubop.Value); chk++)
                {

                    Control c = FindControl(litreesubop + "" + chk + "" + d.idModulo);
                    if (c != null)
                    {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox opciones = (c.Controls[0] as CheckBox);
                            
                                checkstree++;
                                string[] split = opciones.ClientID.Split(new Char[] { ',' });
                                if (split.Length > 5)
                                {
                                if (split[5].Trim().Length > 0)
                                {
                                    if (PageIndex.Equals("Acceso por Perfiles"))
                                    {
                                    PerfilesOpciones op = new PerfilesOpciones();
                                    op.visible = opciones.Checked.ToString();
                                    int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                    op.idPerfil = idSeleecionado;                                    
                                    op.idOpcion = int.Parse(split[5]);                                                                       
                                        if (registra)
                                        {
                                            _PerfilesOpcionesBL.registrarPerfilesOpciones(op);
                                        }
                                        else
                                            if (actualiza && (split.Length > 6))
                                            {
                                                if (split[6].Trim().Length > 0)
                                                {
                                                    op.idPerfilOpcion = int.Parse(split[6]);
                                                    actualizados.Add(_PerfilesOpcionesBL.UpdatePerfilesOpciones(op).Success + "," + (c.Parent.Parent.Controls[0] as CheckBox).Text + " > " + opciones.Text.Trim());
                                                }
                                            }
                                    }
                                    else if (PageIndex.Equals("Acceso por Usuarios"))
                                    {
                                        UsuariosOpciones op = new UsuariosOpciones();
                                        op.visible = opciones.Checked.ToString();
                                        int idSeleecionado = int.Parse(HidUsuSeleccionadoSeg.Value);
                                        op.idUsuario = idSeleecionado;
                                        op.idOpcion = int.Parse(split[5]);
                                        if (registra)
                                        {
                                            _UsuariosOpcionesBL.registrarUsuariosOpciones(op);
                                        }
                                        else
                                            if (actualiza && (split.Length > 6))
                                            {
                                                if (split[6].Trim().Length > 0)
                                                {
                                                    op.idUsuarioOpcion = int.Parse(split[6]);
                                                    actualizados.Add(_UsuariosOpcionesBL.UpdateUsuariosOpciones(op).Success + "," + (c.Parent.Parent.Controls[0] as CheckBox).Text + " > " + opciones.Text.Trim());
                                                }
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (actualiza && (checkstree > 0))
            {
                updateMsg(actualizados, LblStatusAccePerfil, "Opciones no actualizados");
            }
        }

        private void updateMsg(List<String> actualizados, Label lblmsg, String msgInicial) {
            int resact = 0;
            string falla = "";
            bool error = false;
            for (int a = 0; a < actualizados.Count; a++)
            {
                string[] split = actualizados[a].Split(new Char[] { ',' });

                if (bool.Parse(split[0]) == false)
                {
                    falla = falla + split[1] + ". </br>";
                    error = true;
                }
                else
                {
                    resact++;
                }
            }
            if(error){
             falla=msgInicial + " : </br>" + falla;
            }

            if (resact == actualizados.Count && actualizados.Count>0&&(!error))
            {
                string msglab = lblmsg.Text.Trim();
                if (msglab.Length > 0 && !msglab.Equals("Actualización correcta"))
                {
                    falla = falla  + lblmsg.Text+ " </br>";
                }
                int longitud=falla.Trim().Length;
                if (longitud<2)
                {
                    cambiarTextLabel(lblmsg, "Actualización correcta", System.Drawing.Color.Green);
                }
                else {
                    cambiarTextLabel(lblmsg, falla, System.Drawing.Color.Red);
                }
            }
            else if (error)
            {                
                        falla = falla + " </br>" + lblmsg.Text;                    
                cambiarTextLabel(lblmsg, falla, System.Drawing.Color.Red);
            }
        }
        
        protected void ButtonAtualizaModuloSeg_Click(object sender, EventArgs e)
        {
            LabUpdateModulo.Text = "";
          bool actualizado=false;
          int numModulos=0;
            foreach (ListItem item in CheckBoxListModulo.Items)
            {
                if (item.Selected)
                {
                    string[] split = item.Value.Split(new Char[] { ',' });
                    if (split.Length > 2)
                    {
                        if (split[2].Length > 0)
                        {
                            numModulos++;
                            Modulo modulo = new Modulo();
                            modulo.idModulo = int.Parse(split[2]);
                            modulo.Nombre = item.Text;
                            modulo.descripcion = item.Text;
                            modulo.h3Id = split[0];
                            modulo.divId = split[1];
                            Modulo modCambio = _modulosBl.getModulo(item.Text, split[0], split[1],0);
                            if (modCambio.idModulo==0) {
                                actualizado = _modulosBl.UpdateModulo(modulo).Success;
                            }                          
                        }
                    }
                }
            }
            updateListadoModulo();
            if(actualizado){                
                cambiarTextLabel(LabUpdateModulo, "Los datos han sido actualizados", System.Drawing.Color.Green);
            }
            else if ((numModulos > 0 )&& (!actualizado))
            {
                cambiarTextLabel(LabUpdateModulo, "No se pudo realizar la actualización :D", System.Drawing.Color.Red);
            }
            
        }

        private void cambiarTextLabel(Label lab, String texto, System.Drawing.Color color) {
            lab.Text = texto;
            lab.ForeColor = color;
        }

        protected void ButtonEliminaModuloSeg_Click(object sender, EventArgs e)
        {
            LabUpdateModulo.Text = "";
            foreach (ListItem item in CheckBoxListModulo.Items)
            {
                if (item.Selected)
                {
                    string[] split = item.Value.Split(new Char[] { ',' });
                    if (split.Length > 2)
                    {
                        if (split[2].Length > 0)
                        {
                            Modulo modulo = new Modulo();
                            modulo.idModulo = int.Parse(split[2]);
                            _modulosBl.DeleteModulo(modulo);                           
                        }
                    }
                }
            }
            updateListadoModulo();
        }

        protected void ButtonRegistrarRelAccesoSeg_Click(object sender, EventArgs e)
        {
            RegModulosRelAcceso(true,false,true,false);
        }

        protected void ButtonAtualizaRelAccesoSeg_Click(object sender, EventArgs e)
        {
            RegModulosRelAcceso(false, true, true, false);
        }

        protected void ButtonRegistrarRelAccesoSisModSeg_Click(object sender, EventArgs e)
        {
            RegModulosRelAcceso(true, false, false, false);
        }

        protected void ButtonDeleteRelAccesoSisModSeg_Click(object sender, EventArgs e)
        {
            RegModulosRelAcceso(false, true, false, false);
        }

        protected void ButtonActualizaUsuSeg_Click(object sender, EventArgs e)
        {
            String passwordHash = BCrypt.Net.BCrypt.HashPassword(PasswordUsUpdate.Value.ToString(), BCrypt.Net.BCrypt.GenerateSalt(12));
            int exito = _usuarioFacade.UpdateUsuarioPassword(0, HidValidUserUpdate.Value, TextBoxUsuarioUpdate.Text.Trim().ToLower(), passwordHash);
            if(exito>0){
                msgactualizado.ForeColor = System.Drawing.Color.Green;
                msgactualizado.Text = "Los datos han sido actualizados correctamente";                
            }else{
                msgactualizado.ForeColor=System.Drawing.Color.Red;
                msgactualizado.Text="No se ha podido actualizar la Información";            
            }
            ClientScript.RegisterStartupScript(this.GetType(), "setTimeout", "setTimeout(function () {$('[id$=msgactualizado]').fadeOut();}, 7000);", true);                                     
        }

        protected void ButtonEnviarEmailSeg_Click(object sender, EventArgs e)
        {
            //enviar email
            String email = TextBoxEmailRegistrado.Text.Trim();
          Usuario us = _usuarioFacade.getUserByEmail(email);

          if (us.idUsuario != 0)
            {
                SendMail("llr.allleo@gmail.com", email, us.idUsuario.ToString(), us.nombre);
                _usuarioFacade.setTiempoExpiracion(us.idUsuario);
                LabEmailReg.Text = "Correo Enviado. \n Sigue las instrucciones que te hemos enviado. ";
            }
            else {
                LabEmailReg.Text = "No se ha podido enviar el email, intente mas tarde :D";
            }
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

        private void limpiarFormUsuarios()
        {
            TextBoxNomUsuario.Text = "";
            TextBoxApellidos.Text = "";
            TextBoxUsuario.Text = "";
            PasswordUs.Value = "";
            RadioButtonEmpleado.SelectedIndex =0;
            TextBoxTelefono.Text = "";
            TextBoxEmail.Text = "";
            TextAreaTecnologias.Value = "";
        }
        private void limpiarFormGrupos()
        {
            TextBoxNomGrupo.Text = "";
            TextBoxDescripcionGrupo.Text = "";
        }
        private void limpiarFormPerfiles()
        {
            TextBoxNomPerfil.Text = "";
            TextBoxDescripcionPerfil.Text = "";
            RadioButtonListAltaPerfil.SelectedIndex = 1;
            RadioButtonListEliminarPerfil.SelectedIndex = 1;
            RadioButtonListModificaPerfil.SelectedIndex = 1;
        }
        private void limpiarFormSistemas()
        {
            TextBoxClaveSis.Text = "";
            TextBoxNombreSis.Text = "";
            TextBoxDescSis.Text = "";
            TextBoxClienteSis.Text = "";
            TextBoxFechaoIniSis.Text = "";
            TextBoxFechaFinEsSis.Text = "";
            TextBoxFinRealSis.Text = "";
            TextBoxTecSistema.Text = "";
        }
        private void gvBindSegBusqueda(string consulta, GridView grid) {
            conn.Open();
            SqlCommand cmd = new SqlCommand(consulta, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                grid.DataSource = null;
                grid.DataSource = ds;
                grid.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grid.DataSource = ds;
                grid.DataBind();
                int columncount = grid.Rows[0].Cells.Count;
                grid.Rows[0].Cells.Clear();
                grid.Rows[0].Cells.Add(new TableCell());
                grid.Rows[0].Cells[0].ColumnSpan = columncount;
                grid.Rows[0].Cells[0].Text = "Sin resultados de busqueda";
            }
        }

        protected void GridView2Seg_RowEditing(object sender, GridViewEditEventArgs e)
        {            
            if (PageIndex.Equals("Sistemas"))
            {
                lblUpdateSistema.Text = "";
                HidSistemaUpdate.Value = GridViewsistema.Rows[e.NewEditIndex].Cells[0].Text;
                TextBoxClaveSis.Text = GridViewsistema.Rows[e.NewEditIndex].Cells[1].Text;
                TextBoxNombreSis.Text = GridViewsistema.Rows[e.NewEditIndex].Cells[2].Text;
                TextBoxClienteSis.Text = GridViewsistema.Rows[e.NewEditIndex].Cells[4].Text;
                TextBoxDescSis.Text = GridViewsistema.Rows[e.NewEditIndex].Cells[3].Text;
                TextBoxFechaoIniSis.Text = GridViewsistema.Rows[e.NewEditIndex].Cells[6].Text;
                TextBoxFechaFinEsSis.Text = GridViewsistema.Rows[e.NewEditIndex].Cells[7].Text;
                
                string fechafinreal = GridViewsistema.Rows[e.NewEditIndex].Cells[8].Text.Trim();
                if (fechafinreal.Contains("&nbsp;"))
                {
                    fechafinreal = fechafinreal.Replace("&nbsp;", "");
                }
                TextBoxFinRealSis.Text = fechafinreal;

                string tecnologiasis =GridViewsistema.Rows[e.NewEditIndex].Cells[9].Text.Trim();
                if (tecnologiasis.Contains("&nbsp;"))
                {
                    tecnologiasis = tecnologiasis.Replace("&nbsp;", "");
                }
                TextBoxTecSistema.Text = tecnologiasis;
                MultiView2SegGrid.ActiveViewIndex = -1;
                ButtonUpdateSistema.Visible = true;
                ButtonCancelSistema.Visible = true;
                ButtonConsultaSistemasSeg.Visible = true;
                activarbotonSeg(false);                
                actualizaPagina("Actualizacion de Sistemas");
                ControlAccesoOpciones();
            }
            else {
                HiddenRowIndexSegUpd.Value = "";
                HiddenRowIndexSegUpd.Value = "" + e.NewEditIndex;
                if (PageIndex.Equals("Usuarios"))
                {
                    GridView2Seg.EditIndex = e.NewEditIndex;
                }
                else
                    if (PageIndex.Equals("Grupos"))
                    {
                        GridView2SegGrupo.EditIndex = e.NewEditIndex;
                    }
                    else
                        if (PageIndex.Equals("Perfiles"))
                        {
                            GridView2SegPerfil.EditIndex = e.NewEditIndex;
                        }
                gvbindSeg();
            }

            ViewState["Index"] = PageIndex;
        }

        protected void GridView2Seg_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndex.Equals("Usuarios"))
            {
                GridView2Seg.PageIndex = e.NewPageIndex;
            }
            else
                if (PageIndex.Equals("Grupos"))
                {
                    GridView2SegGrupo.PageIndex = e.NewPageIndex;
                }
                else
                    if (PageIndex.Equals("Perfiles"))
                    {
                        GridView2SegPerfil.PageIndex = e.NewPageIndex;
                    }
            
            ViewState["Index"] = PageIndex;
            gvbindSeg();

        }
        protected void GridView2Seg_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (PageIndex.Equals("Usuarios"))
            {
                GridView2Seg.EditIndex = -1;
            } else if (PageIndex.Equals("Grupos"))
            {
                GridView2SegGrupo.EditIndex = -1;
            }
            else if (PageIndex.Equals("Perfiles"))
            {
                GridView2SegPerfil.EditIndex = -1;
            }
            
            ViewState["Index"] = PageIndex;
            gvbindSeg();
        }

        protected void GridView2Seg_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TableCell cell = null;
            string idtabla = "";
            if (PageIndex.Equals("Usuarios"))
            {
                idtabla = PageIndex.Substring(0, PageIndex.Length - 1);
                cell = GridView2Seg.Rows[e.RowIndex].Cells[0];
            }
            else if (PageIndex.Equals("Perfiles"))
            {
                idtabla = PageIndex.Substring(0, PageIndex.Length - 2);
                cell = GridView2SegPerfil.Rows[e.RowIndex].Cells[0];
            }
            else if (PageIndex.Equals("Grupos"))
            {
                idtabla = PageIndex;
                cell = GridView2SegGrupo.Rows[e.RowIndex].Cells[0];
            }
            else if (PageIndex.Equals("Sistemas"))
            {
                idtabla = PageIndex;
                cell = GridViewsistema.Rows[e.RowIndex].Cells[0];
                List<PerfilLogin> listado = new List<PerfilLogin>();
                PerfilLogin defaultp = new PerfilLogin();
                listado.Add(defaultp);

                SistemasModulos validarRelaciones = _CAUsuarioBL.getSistemasModulosRelaciones(Convert.ToInt32(cell.Text), 0, listado);


                if (validarRelaciones.sistemasModulos.Count < 1 &&
                    validarRelaciones.perfilesPantallas.Count < 1 && validarRelaciones.usuariosPantallas.Count < 1)
                {
                    _sistemaBL.DeleteSistema(Convert.ToInt32(cell.Text));
                }
                else
                {
                    HidnoEliminarSistema.Value = "1";
                }

            }

            if (!PageIndex.Equals("Sistemas"))
            {
                
                if (PageIndex.Equals("Usuarios"))
                {
                    cell = GridView2Seg.Rows[e.RowIndex].Cells[1];
                    UsuarioLogin validarRelaciones = _CAUsuarioBL.getUsuarioLogeado(cell.Text.Trim(), GlobalDataSingleton.Instance.sistemaID);
                    if (validarRelaciones.idUsuario > 0)
                    {
                        if (validarRelaciones.sistemasModulos.perfilesPantallas.Count < 1 && validarRelaciones.sistemasModulos.usuariosPantallas.Count < 1)
                        {
                            cell = GridView2Seg.Rows[e.RowIndex].Cells[0];
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("update " + PageIndex + " set Estado='1' where ID" + idtabla + "=" + Convert.ToInt32(cell.Text), conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        else
                        {
                            HidnoEliminar.Value = "1";
                        }
                    }                                            
                }
                else {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update " + PageIndex + " set Estado='1' where ID" + idtabla + "=" + Convert.ToInt32(cell.Text), conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                
            }

            ViewState["Index"] = PageIndex;
            gvbindSeg();

        }

        protected void GridView2Seg_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView opcion = null;
            if (PageIndex.Equals("Usuarios")) {
                opcion = GridView2Seg;
            }
            else if (PageIndex.Equals("Grupos")) {
                opcion = GridView2SegGrupo;
            }
            else if (PageIndex.Equals("Perfiles"))
            {
                opcion = GridView2SegPerfil;
            }
           
            TextBox  esempleado, nombre, apellidos, tecnologias, email, telefono;
            GridViewRow row = (GridViewRow)opcion.Rows[e.RowIndex]; 
            TableCell ID = opcion.Rows[e.RowIndex].Cells[0];
            SqlCommand cmd =new SqlCommand();
            opcion.EditIndex = -1;
            if(PageIndex.Equals("Usuarios")){
       
            esempleado = (TextBox)row.Cells[2].Controls[0];
            nombre = (TextBox)row.Cells[4].Controls[0];
            apellidos = (TextBox)row.Cells[5].Controls[0];
            tecnologias = (TextBox)row.Cells[7].Controls[0];
            
            telefono = (row.Cells[9].Controls[0] as TextBox);          

            string stringtel = "";
            if (telefono.Text.Trim().Length > 0)
            {
                long tel = long.Parse(telefono.Text.Trim());
                                    stringtel = ", p.telefono=" + tel + "";
            }
                

            String actualizar = " UPDATE u"
                       + " SET   u.esEmpleado='" + esempleado.Text.Trim().ToUpper() + "'"
                       + " FROM Usuarios AS u"
                       + " INNER JOIN Personas AS P "
                       + "        ON u.IDUsuario = P.IDUsuario "
                       + " WHERE u.IDUsuario = "+ID.Text+""
                       + " UPDATE p"
                       + " SET  p.Nombre = '" + nombre.Text.Trim() + "', p.Apellido='" + apellidos.Text.Trim() + "',p.Tecnologias='" + tecnologias.Text.Trim() + "' " +stringtel
                       + " FROM Usuarios AS u"
                       + " INNER JOIN Personas AS P"
                       + "        ON u.IDUsuario = P.IDUsuario "
                       + " WHERE u.IDUsuario = " + ID.Text + "";    
            actualizarSeg(cmd,actualizar);
            }else if(PageIndex.Equals("Grupos")){
                TextBox nombreGrupo, descripcion;
                nombreGrupo = (row.Cells[1].Controls[0] as TextBox);
                descripcion = (row.Cells[2].Controls[0] as TextBox);
                String actualizar = " UPDATE grupos"
                       + " SET  Nombre = '" + nombreGrupo.Text.Trim() + "', descripcion='" + descripcion.Text.Trim() + "'"                     
                       + " WHERE IDgrupos = " + ID.Text + "";
                actualizarSeg(cmd, actualizar);
            }
            else if (PageIndex.Equals("Perfiles"))
            {
                TextBox nombrePerfil, descripcionPerfil;
                nombrePerfil = (row.Cells[1].Controls[0] as TextBox);
                descripcionPerfil = (row.Cells[2].Controls[0] as TextBox);
                TextBox alta, baja, modifica;
                alta = (row.Cells[3].Controls[0] as TextBox);
                baja = (row.Cells[4].Controls[0] as TextBox);
                modifica = (row.Cells[5].Controls[0] as TextBox);
                String actualizar = " UPDATE perfiles"
                       + " SET  Nombre = '" + nombrePerfil.Text.Trim() + "', descripcion='" + descripcionPerfil.Text.Trim() + "'"
                       + " , usuarioalta='" + alta.Text.Trim().ToUpper() + "', usuariobaja='" + baja.Text.Trim().ToUpper() + "',usuariomodifica='" + modifica.Text.Trim().ToUpper() + "'"
                       + " WHERE IDperfil = " + ID.Text + "";
                actualizarSeg(cmd, actualizar);
            
            }

            
        }
        private void actualizarSeg(SqlCommand cmd, String consulta) {
            conn.Open();
            cmd = new SqlCommand(consulta, conn);            
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbindSeg();
        }     
    
        public void SendMail(String emailde, String emailpara, string id,string nombre)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(emailde);
            msg.To.Add(emailpara);
            msg.Body =
            " Este es un email automatico, Favor de no responder. <br/> <br/> "
            + " Hola " + nombre + " <br/> <br/> "
            + " Tus Datos de Control de Tareas. <br/><br/> "
            + " Para recuperar tus datos de Inicio de Sesion para el Control de Tareas, simplemente  <a href='http://localhost:51575/Main.aspx?id=email&u=" + id + "'>Sigue este link</a> <br/><br/>"
            + " Nota: Este proceso expira en 3 Horas. <br/> <br/> "
            + " Nota: El link es valido solo una vez que se inicia el proceso de recuperacion de datos de Inicio de Sesion. <br/> <br/> ";
            msg.IsBodyHtml = true;
            msg.Subject = "Tus Datos de Control de Tareas";
            SmtpClient smt = new SmtpClient("smtp.gmail.com");
            smt.Port = 587;
            smt.Credentials = new NetworkCredential("llopez@SolutiaIntelligence.com", "gtleo144");
            smt.EnableSsl = true;
            smt.Send(msg);        
        }
        

        [System.Web.Services.WebMethod]        
        public static string CheckEmail(string email)
        {
           string returnValue = string.Empty;
           controlConn.abrirConexion();
           try
           {
               SqlCommand cmSql = conn.CreateCommand();
               cmSql.CommandText = "Select * from personas where  email=@parm2";
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
           controlConn.cerrarConexion();
           return returnValue;
        }

        public static string accesAjax(string referencia, string param, String email) { 
            string returnValue = string.Empty;
            controlConn.abrirConexion();
            try{                
                SqlCommand cmSql = conn.CreateCommand();
                cmSql.CommandText = "Select * from "+referencia+" where  "+param+"=@parm2";
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
                
            }
            catch
            {
                returnValue = "error";
            }
            controlConn.cerrarConexion();
            return returnValue;  
        
        }
        
        [System.Web.Services.WebMethod]
        public static string CheckUserName(string userName)
        {
            string returnValue = string.Empty;
            controlConn.abrirConexion();
            try
            {                
                SqlCommand cmSql = conn.CreateCommand();
                cmSql.CommandText = "Select * from usuarios where  nombre=@parm2";
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters["@parm2"].Value = userName.ToLower();
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
                        returnValue="true";
                    }
                }               
            }
            catch
            {
                returnValue = "error";
            }
            controlConn.cerrarConexion();
            return returnValue;
        }


        [System.Web.Services.WebMethod]
        public static List<Grupo> agregarUsuarioGrupoWeb(int[] idusuarioseleccionado)
        {
            List<Grupo> res= new List<Grupo>();
            int cantidad = idusuarioseleccionado.Length;
            if (cantidad > 0)
            {
                if (idusuarioseleccionado[0] != 0)
                {
                    if (idusuarioseleccionado[cantidad - 1] != 0)
                    {
                        if (cantidad > 2)
                        {
                            for (int d = 0; d <= cantidad - 2; d++)
                            {
                                _grupoBL.agregarUsuarioGrupo(idusuarioseleccionado[cantidad - 1], idusuarioseleccionado[d]);
                            }
                        }
                        else if (cantidad == 2)
                        {
                            _grupoBL.agregarUsuarioGrupo(idusuarioseleccionado[1], idusuarioseleccionado[0]);
                       res=_grupoBL.tb(idusuarioseleccionado[1]);

                        }
                    }
                }
            }
                  return res;
        }        

         protected void guardarusurioGrupo(object sender, EventArgs e)
         {
             string idus = Request.Form[HidUsuSeleccionadoSeg.UniqueID];
             int idusuario= int.Parse(idus);
             if (idusuario != 0)
             {
                 string leftSelectedItems = Request.Form[ListBoxGruposAsigSeg.UniqueID];
                 ListBoxGruposAsigSeg.Items.Clear();
                 if (!string.IsNullOrEmpty(leftSelectedItems))
                 {
                     foreach (string item in leftSelectedItems.Split(','))
                     {
                         if (PageIndex.Equals("Relacion de Usuarios"))
                         {
                             _grupoBL.agregarUsuarioGrupo(int.Parse(idus), int.Parse(item));
                         }
                         else if (PageIndex.Equals("Relacion de Grupos"))
                         {
                             _grupoBL.agregarUsuarioGrupo(int.Parse(item), int.Parse(idus));
                         }
                         else if (PageIndex.Equals("Relacion de Perfiles"))
                         {
                             _perfilBL.agregarUsuarioPerfil(int.Parse(idus), int.Parse(item));
                         }


                     }
                 }
                 string rightSelectedItems = Request.Form[ListBoxGruposSeg.UniqueID];
                 ListBoxGruposSeg.Items.Clear();
                 if (!string.IsNullOrEmpty(rightSelectedItems))
                 {
                     foreach (string item in rightSelectedItems.Split(','))
                     {
                         if (PageIndex.Equals("Relacion de Usuarios"))
                         {
                             _grupoBL.eliminarUsuarioGrupo(int.Parse(idus), int.Parse(item));
                         }
                         else if (PageIndex.Equals("Relacion de Grupos"))
                         {
                             _grupoBL.eliminarUsuarioGrupo(int.Parse(item), int.Parse(idus));
                         }
                         else if (PageIndex.Equals("Relacion de Perfiles"))
                         {
                             _perfilBL.eliminarUsuarioPerfil(int.Parse(idus), int.Parse(item));
                         }

                     }
                 }
                 if (PageIndex.Equals("Relacion de Usuarios"))
                 {
                     _grupoBL.llenarListaGruposAsignados(ListBoxGruposAsigSeg, int.Parse(idus));
                     _grupoBL.llenarListaGrupos(ListBoxGruposSeg, int.Parse(idus));
                 }
                 else if (PageIndex.Equals("Relacion de Grupos"))
                 {
                     _usuarioFacade.llenarListaUsuariosAsignados(ListBoxGruposAsigSeg, int.Parse(idus));
                     _usuarioFacade.llenarListaUsuariosNoAsignados(ListBoxGruposSeg, int.Parse(idus));
                 }
                 else if (PageIndex.Equals("Relacion de Perfiles"))
                 {
                     _perfilBL.llenarListaPerfilesAsignados(ListBoxGruposAsigSeg, int.Parse(idus));
                     _perfilBL.llenarListaPerfilesNoAsignados(ListBoxGruposSeg, int.Parse(idus));
                 }
             }
                 //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Left ListBox Items: " + leftSelectedItems + "\\nRight ListBox Items: " + rightSelectedItems + "');", true);
             
         }

         protected void Application_AuthenticateRequest(Object sender, EventArgs e)
         {
             // Get the authentication cookie
             string cookieName = FormsAuthentication.FormsCookieName;
             HttpCookie authCookie = Context.Request.Cookies[cookieName];

             // If the cookie can't be found, don't issue the ticket
             if (authCookie == null) return;

             // Get the authentication ticket and rebuild the principal 
             // & identity
             FormsAuthenticationTicket authTicket =
               FormsAuthentication.Decrypt(authCookie.Value);
             string[] roles = authTicket.UserData.Split(new Char[] { '|' });
             GenericIdentity userIdentity =new GenericIdentity(authTicket.Name);
             GenericPrincipal userPrincipal =new GenericPrincipal(userIdentity, roles);
             Context.User = userPrincipal;
         }

         protected void LinkButton1_Click(object sender, EventArgs e)
         {
             //Response.Redirect("CapturaTareas.aspx");
             actualizaPagina("Captura de Tareas");
             getPantallaIndex();
             ControlAccesoOpciones();
             MultiViewSeguimientoTarea.ActiveViewIndex = 0;
         }
         
         protected void cerrarSesiononclick(Object sender, EventArgs e)
         {
             FormsAuthentication.SignOut();
             Response.Redirect("Views/Login/Inicio.aspx");
             GlobalDataSingleton.Instance.controlAcceso = "";
        }
        //Modulo seguimiento de Tarea
         protected void ButtonUpload_Click(object sender, EventArgs e)
         {
             Boolean fileOK = false;
             String path = Server.MapPath("~/UploadedExcels/");
             if (FileUploadTareas.HasFile)
             {
                 String fileExtension =
                     System.IO.Path.GetExtension(FileUploadTareas.FileName).ToLower();
                 String[] allowedExtensions = { ".xls", ".xlsx" };
                 for (int i = 0; i < allowedExtensions.Length; i++)
                 {
                     if (fileExtension == allowedExtensions[i])
                     {
                         fileOK = true;
                     }
                 }
             }

             if (fileOK)
             {
                 try
                 {
                     FileUploadTareas.PostedFile.SaveAs(path
                         + FileUploadTareas.FileName);
                     LabelSubir.Text = "Tareas cargadas con exito";
                 }
                 catch (Exception ex)
                 {
                     LabelSubir.Text = "Error en carga de tareas";
                 }
             }
             else
             {
                 LabelSubir.Text = "No se aceptan archivos de este tipo";
             }
         }

         protected void dtm1_Tick(object sender, EventArgs e)
         {
             if (stopWatch.IsRunning)
             {

                 TimeSpan ts = stopWatch.Elapsed;
                 this.LabelSeguimientoTarea.Text = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
             
             }
         }

         protected void ButtonIniciar_Click(object sender, EventArgs e)
         {
             if (stopWatch.IsRunning)
             {
                 stopWatch.Stop();
                 ButtonIniciar.Text = "Iniciar";

             }
             else
             {
                 stopWatch.Start();
                 ButtonIniciar.Text = "Detener";
             }
             

                 TimeSpan ts = stopWatch.Elapsed;
                 this.LabelSeguimientoTarea.Text = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

             
         }

         protected void ButtonReset_Click(object sender, EventArgs e)
         {

             LabelSeguimientoTarea.Text = "00:00:00";
             stopWatch.Reset();


         }
         protected void ButtonEnviar_Click(object sender, EventArgs e)
         {

         }

         [System.Web.Services.WebMethod]
         public static String tiempo()
         {
             TimeSpan ts = stopWatch.Elapsed;
            String tiempo= String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            return tiempo;
         }

        
    }
}