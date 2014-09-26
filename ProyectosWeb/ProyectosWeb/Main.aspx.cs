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
using BusinessLogic.Seguridad.ControlAcceso;
using BusinessLogic.Tareas;
using Models.Tareas;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using BusinessLogic.General;
using System.IO;
using OfficeOpenXml;
using System.Threading;
using System.Globalization;

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
      private String userName;
      private eliminarCheckboxs eliminarOp = new eliminarCheckboxs();
      private static ControlarConexion controlConn = new ControlarConexion(conn);
        
        private int subTotalCHK=0;
       private  int indexParentEliminar;

       public static Stopwatch stopWatch = new Stopwatch();
        
        private static int activarPantalla = 0;
        private static int activarOpciones = 0;

        private ConsultaReporteBL _ConsultaReporteBL = new ConsultaReporteBL(conn);

        private static DataTable tarea;
        private static long contConsultaCR;      

        private static string linkantarior="";        
        private static bool busquedaseg = false;
        private static String consultabusqueda = "";

        SICTWS.test _sictws = new SICTWS.test();
        private static LinkButton PantalaOp;
        private static List<Modulo> modulossict;

        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES", false);
            //actualizado sliding en web.config, eliminar usuarios admin
            // Get the authentication cookie
            LblSinAcessoOpciones.Text = "";
            if (_CAUsuarioBL.controlAccesoPantallasModulos(Request, Response, this.Page, redirectinicio, Context.User.Identity.Name, admin, linkantarior))
            {                                             
                userName = Context.User.Identity.Name;
                usuarioLogin.Text = userName + ", ";                
              _sistemamodulo=_CAUsuarioBL._sistemamodulo;                   
                    //Desactivar Pantallas desactivadas o no registradas :D
                    if (!userName.ToLower().Equals(admin))
                    {
                       origenLinkButton("",DivProyecto1.ClientID,true);                      
                           origenLinkButton("",DivProyecto2.ClientID,true);                      
                           origenLinkButton("",DivProyecto3.ClientID,true);                    
                           origenLinkButton("",DivProyecto4.ClientID,true);   
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
                    bool postdropdown = false;                    

                    if (Request.Form["__EVENTTARGET"] != null)
                    {
                       String evento = Request.Form["__EVENTTARGET"].ToString();
                       if (evento.Trim().Length>0)
                       {
                        Control link = FindControl(Request.Form["__EVENTTARGET"].ToString());
                        if(link!=null){
                        if (link.GetType() == typeof(LinkButton))
                        {
                            linkActivadoPrevio(System.Drawing.Color.Black);
                            (link as LinkButton).ForeColor = System.Drawing.Color.Blue;                            
                        }
                        if (link.GetType() == typeof(DropDownList))
                        {
                            postdropdown = true;
                        }
                        }
                        }
                        if (Request.Form["__EVENTTARGET"] == LinkProyecto.UniqueID || Request.Form["__EVENTTARGET"] == LinkRequerimiento.UniqueID
                            || Request.Form["__EVENTTARGET"] == LinkCasosUso.UniqueID || Request.Form["__EVENTTARGET"] == LinkComponente.UniqueID
                            || Request.Form["__EVENTTARGET"] == LinkTarea.UniqueID)
                        {
                            linkButtonTareas = 1;
                            accordionInd.Value = "1";
                            consultabusqueda = "";
                            busquedaseg = false; 
                        }
                        if (Request.Form["__EVENTTARGET"] == LinkButton1.UniqueID)
                        {
                            linkButtonSeguimientoTareas = 1;
                            accordionInd.Value = "3";
                            consultabusqueda = "";
                            busquedaseg = false; 
                        }

                        if (origenLinkButton(Request.Form["__EVENTTARGET"], DivProyecto1.ClientID,false))
                        {
                            linkButtonSeguridad = 1;
                            accordionInd.Value = "0";
                            consultabusqueda = "";
                            busquedaseg = false; 
                        }
                        if (origenLinkButton(Request.Form["__EVENTTARGET"], DivProyecto3.ClientID,false))
                        {
                            linkButtonConsultaReporte = 1;
                            accordionInd.Value = "2";
                            consultabusqueda = "";
                            busquedaseg = false; 
                        }
                    }

                    if (IsPostBack && ViewState["Index"] != null)
                    {
                        PageIndex = ViewState["Index"].ToString();
                        /**/
                    
                        if (Request.Form["__EVENTTARGET"].Trim().Length < 1 || postdropdown)
                        {
                            linkActivadoPrevio(System.Drawing.Color.Blue);
                        }
                        else {
                            linkActivadoPrevio(System.Drawing.Color.Black);
                        }

                        if (linkButtonTareas != 1 && linkButtonSeguimientoTareas != 1 && linkButtonConsultaReporte==0)
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
                                llenarTree(false, false);
                            }
                            else if (PageIndex.Equals("Opciones"))
                            {
                                MultiView1Seg.ActiveViewIndex = 10;
                                llenarTree(true, false);
                            }
                            else if (PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios"))
                            {
                                MultiView1Seg.ActiveViewIndex = 11;
                                llenarTree(true, true);
                            }
                            else if (PageIndex.Equals("Sistemas y Módulos"))
                            {
                                MultiView1Seg.ActiveViewIndex = 12;
                                llenarTree(false, false);
                            }

                        }
                        //modulo tarea
                        if ((PageIndex.Equals("Proyectos") || PageIndex.Equals("Requerimientos") || PageIndex.Equals("CasosUso")
                                 || PageIndex.Equals("Componentes") || PageIndex.Equals("Tareas") || linkButtonTareas == 1) &&
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

                        //modulo consultas y reportes
                        if ((PageIndex.Equals("Consultas") || linkButtonConsultaReporte == 1) && linkButtonTareas == 0 && linkButtonSeguridad == 0 && linkButtonSeguimientoTareas == 0)
                        {
                            MultiViewConsultaReporte.ActiveViewIndex = 0;
                        }
                        System.GC.Collect();
                        //activar Opciones
                        if (int.Parse(hidindexpantalla.Value)>0 && !linkantarior.Equals("LinkcuentaUsSeg"))
                        {                           
                            ControlAccesoOpciones();
                        }
                        System.GC.Collect();
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
                    LblSinAcessoOpciones.Text = GlobalDataSingleton.Instance.controlAccesoOpc;
                } 
                else {
                    expirarSesion();
            }   
        }

        private void linkActivadoPrevio(System.Drawing.Color color)
        {
            if (linkantarior.Trim().Length > 0)
            {
                Control linka = FindControl(linkantarior);

               var vc= Request.Form[linka.UniqueID];

                if (linka != null)
                {
                    LinkButton link = (linka as LinkButton);
                    link.ForeColor = color;
                }                
            }
        }
        protected internal int SistemamoduloNoAsignado(int moduloid){
                        SistemasModulos smodulo = _SistemasModulosBL.getSistemaModulo(GlobalDataSingleton.Instance.sistemaID,moduloid);
                                int noasignado = 0;
                                if (smodulo.idSistemaModulo > 0)
                                {
                                    if (smodulo.divvisible.Trim().Length > 0)
                                    {
                                        if (!bool.Parse(smodulo.divvisible))
                                        {
                                            noasignado = 1;
                                        }
                                    }
                                }
                        return noasignado;
        }

        private void getPantallaIndex()
        {           
            if (Request.Form["__EVENTTARGET"] != null)
            {
                hidindexpantalla.Value = "0";
                LinkButton pantallalink = (FindControl(Request.Form["__EVENTTARGET"]) as LinkButton);
                string[] indexpantallas = pantallalink.CommandName.Split(new Char[] { ',' });                
                if (indexpantallas.Length > 2)
                {
                    hidindexpantalla.Value = (indexpantallas[2].Length>0 ? indexpantallas[2]:"0");
                }

                linkantarior = pantallalink.ID;
                PantalaOp = null;
                PantalaOp = pantallalink;
            }           
        }
        private void ControlAccesoOpciones() {
            String opcion = componentesOpciones(PantalaOp.CommandName, "");
            _CAUsuarioBL.ControlAccesoOpciones(userName, hidindexpantalla.Value, admin, redirectMain, LblSinAcessoOpciones, PageIndex, Response, Page, opcion);            
        }

        private void activarComponenteOpcion(String componenteIndex, String idAsp, String visible)
        {
            if (componenteIndex.Trim().Length > 0)
            {
                GridView grid = (FindControl(idAsp) as GridView);
                if (grid != null)
                {
                    (grid.Columns[int.Parse(componenteIndex)]).Visible = bool.Parse(visible);
                }
            }
            else
            {
                Control c = FindControl(idAsp);
                if (c != null)
                {
                    c.Visible = bool.Parse(visible);
                }
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
            busquedaseg = false;
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
            busquedaseg = false;
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
            busquedaseg = false;
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
            busquedaseg = false;
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
            busquedaseg = false;
        }

        protected void UsuariosOnClick(object sender, EventArgs e)
        {
            busquedaseg = false;
                limpiarFormUsuarios();
                PrevIndex = PageIndex;
                GridView2Seg.EditIndex = -1;
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
                ((BoundField)GridView2Seg.Columns[3]).ReadOnly = true;
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
                busquedaseg = false;
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
            busquedaseg = false;        
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
            GridView2SegGrupo.EditIndex = -1;
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
            gvbindSeg();
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
        }

        protected void PerfilesOnClick(object sender, EventArgs e)
        {
            limpiarFormPerfiles();
            PrevIndex = PageIndex;
            GridView2SegPerfil.EditIndex = -1;
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
            busquedaseg = false;            
        }

        private void GridView2SegPerfiles()
        {
            GridView2SegPerfil.Columns[0].HeaderText = "ID";
            (GridView2SegPerfil.Columns[0] as BoundField).DataField = "ID" + PageIndex.Substring(0, PageIndex.Length - 2);
            GridView2SegPerfil.Columns[1].HeaderText = "Nombre";
            (GridView2SegPerfil.Columns[1] as BoundField).DataField = "Nombre";
            GridView2SegPerfil.Columns[2].HeaderText = "Descripcion";
            (GridView2SegPerfil.Columns[2] as BoundField).DataField = "Descripcion";
            GridView2SegPerfil.Columns[3].HeaderText = "Registrar ";
            (GridView2SegPerfil.Columns[3] as BoundField).DataField = "usuarioalta";
            GridView2SegPerfil.Columns[4].HeaderText = "Eliminar";
            (GridView2SegPerfil.Columns[4] as BoundField).DataField = "usuariobaja";
            GridView2SegPerfil.Columns[5].HeaderText = "Modificar";
            (GridView2SegPerfil.Columns[5] as BoundField).DataField = "usuariomodifica";                                               
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
            _usuarioFacade.DropDownBinUsuarios(DropDownListadoSeg);
            MultiView1Seg.ActiveViewIndex = 3;
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
            lblStatusRelacionUs.Text = "";
        }
        
        protected void RelacionesGruposOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            ListBoxGruposSeg.Items.Clear();
            ListBoxGruposAsigSeg.Items.Clear();
            actualizaPagina("Relacion de Grupos");
            cambiarNomEtiquetaRelaciones("Seleccione un Grupo :", "Usuarios");           
            _grupoBL.DropDownBindGrupos(DropDownListadoSeg);
            MultiView1Seg.ActiveViewIndex = 3;
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
            lblStatusRelacionUs.Text = "";
        }
        protected void RelacionesPerfilesOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            ListBoxGruposSeg.Items.Clear();
            ListBoxGruposAsigSeg.Items.Clear();
            actualizaPagina("Relacion de Perfiles");
            cambiarNomEtiquetaRelaciones("Seleccione un Usuario :", "Perfiles");            
            _usuarioFacade.DropDownBinUsuarios(DropDownListadoSeg);
            MultiView1Seg.ActiveViewIndex = 3;
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
            lblStatusRelacionUs.Text = "";
        }

        protected void ControlAccesSistemaOnClick(object sender, EventArgs e)
        {
            getPantallaIndex();
            mostrarSistemas();            
            ControlAccesoOpciones();
            busquedaseg = false;
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
            busquedaseg = false;
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
            LblSinRelacion.Text = "";
            actualizaPagina("Pantallas");
            LblupdatePantalla.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 10;
            llenarTree(false,false);
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
        }


        protected void ControlAccesOpcionesOnClick(object sender, EventArgs e)
        {
            LblSinRelacion.Text = "";
            actualizaPagina("Opciones");
            LblupdatePantalla.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 10;
            llenarTree(true,false);
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
        }
        
        protected void RelAccesoPerfilesOnClick(object sender, EventArgs e)
        {
            LblSinRelacionRel.Text = "";
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
            busquedaseg = false;
        }

        protected void RelAccesoModSistemaOnClick(object sender, EventArgs e)
        {
            LblStatusModSis.Text = "";
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
            busquedaseg = false;
        }

        protected void RelAccesoUsuariosOnClick(object sender, EventArgs e)
        {
            LblSinRelacionRel.Text = "";
            actualizaPagina("Acceso por Usuarios");
            HidUsuSeleccionadoSeg.Value = "0";
            LblStatusAccePerfil.Text = "";
            MultiView2SegGrid.ActiveViewIndex = -1;
            _perfilBL.DropDownBinPerfiles(DropDownAccesoRelaciones);
            activarbotonSeg(false);
            MultiView1Seg.ActiveViewIndex = 11;
            llenarTree(true, true);
            llenarTree(true,true);
            _usuarioFacade.DropDownBinUsuariosCR(DropDownAccesoRelaciones);
            getPantallaIndex();
            ControlAccesoOpciones();
            busquedaseg = false;
        }

        protected internal bool origenLinkButton(String eventtargetorigen, String idcontrol, bool validarRegistroPantallas){
            bool equivalente = false;
            Control f = FindControl(idcontrol); 
        foreach (Control c in f.Controls)
            {
                string idlink = "";
                try
                {
                    if (c.GetType() == typeof(LinkButton))
                    {
                        idlink = ((LinkButton)c).ClientID;
                    }
                    if (c.GetType() == typeof(Label))
                    {
                        idlink = ((Label)c).ClientID;
                    } 

                }
                catch
                {
                    if (c.GetType() == typeof(LinkButton))
                    {
                        idlink = ((LinkButton)c).ClientID;
                    }
                }
                if (idlink.Trim().Length > 0)
                {
                    //linkbutton origen
                    if (eventtargetorigen.Equals(idlink) && !validarRegistroPantallas)
                    {
                        equivalente = true;
                        break;
                    }

                    if ( f.Visible && validarRegistroPantallas)
                    {
                        Pantalla existe = _pantallaBL.getPantalla(null, idlink);
                        if (existe.idPantalla < 1 || existe.estado > 0)
                        {
                            //desactivar pantalla
                            if (!idlink.Equals("LlbCatalogoSeg") && !idlink.Equals("LinkcuentaUsSeg"))
                            {
                            FindControl(idlink).Visible = false;
                            }
                        }
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
            if (linkantarior.Trim().Length > 0)
            {
                Control linka = FindControl(linkantarior);
                if (linka != null)
                {
                    (linka as LinkButton).ForeColor = System.Drawing.Color.Blue;
                }
            }
            lblStatusRelacionUs.Text = "";
            string valor = DropDownListadoSeg.SelectedValue;
            if (valor.Trim().Length>0)
            {
               int idseleccionado = int.Parse(valor);
               if (idseleccionado > 0)
               {
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
               else {
                   ListBoxGruposSeg.Items.Clear();
                   ListBoxGruposAsigSeg.Items.Clear();
               }
               HidUsuSeleccionadoSeg.Value = "" + idseleccionado;
            }
            }        

        protected void SeleccionDropDownListAcceso(Object sender, EventArgs e)
        {
            LblStatusAccePerfil.Text = "";
            string valor = DropDownAccesoRelaciones.SelectedValue;
            if (valor.Trim().Length == 0)
            {
                valor = "0";
            }
                int idseleccionado = int.Parse(valor);
                HidUsuSeleccionadoSeg.Value = "" + idseleccionado;

                if (PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios"))
                {
                    llenarTree(true, true);
                }                                
            
        }

        protected void SeleccionDropDownModuloSistema(Object sender, EventArgs e)
        {
            LblStatusModSis.Text = "";
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
                SqlCommand cmd = null;
                if (busquedaseg)
                {
                    if (consultabusqueda.Trim().Length>0)
                    {
                    cmd = new SqlCommand(consultabusqueda, conn);
                    }                   
                }
                else
                {
                    consultabusqueda = "";
                 cmd = new SqlCommand(consulta, conn);
                   
                }
                if (cmd != null)
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();
                    llenarGrid(ds, opcion);
                }
            }       
       }
        private void validarGridView(GridView grid)
        {
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
                       + " if(parseInt(td.children[0].value.length)>64){ td.children[0].value=CellValue.substring(0, 65);}"
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
                           + " if(parseInt(td.children[0].value.length)>64){ td.children[0].value=CellValue.substring(0, 65);}"
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
        private void llenarGrid(DataSet ds, GridView grid){
            if (ds.Tables[0].Rows.Count > 0)
            {
                grid.DataSource = ds.Tables[0];                
                grid.DataBind();
               validarGridView(grid);                                
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
            if(PageIndex.Contains("Sistemas")){               
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
            GridView1.Rows[e.NewEditIndex].Cells[GridView1.Rows[e.NewEditIndex].Cells.Count - 1].Attributes.Remove("onclick");              
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
            busquedaseg = false;
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
                if (TextBoxNombreSis.Text.Trim().Length > 0)
                {
                    consulta = consulta + " and p.nombre like '" + TextBoxNombreSis.Text.Trim() + "%'";
                }
                gvBindSegBusqueda(consulta, GridViewsistema);
                ControlAccesoOpciones();
            }
        

        }

        protected void Button6seg_Click(object sender, EventArgs e) {
            busquedaseg = false;
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
              //  lblUpdateSistema.ForeColor = System.Drawing.Color.Red;
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
            int seleccionados=0;
           foreach (ListItem item in CheckBoxListModulo.Items){
               if (item.Selected) {
                   seleccionados++;
                   string[] split = item.Value.Split(new Char[] { ',' });
                   Modulo modulo = new Modulo();
                   modulo.Nombre = item.Text.Trim();
                   modulo.descripcion = item.Text;
                   modulo.h3Id = split[0];
                   modulo.divId = split[1];
                   _modulosBl.registrarModulo(modulo);
               }           
            }
            if(seleccionados>0){
            LabUpdateModulo.Text = "Los Módulos registrados correctamente se muestran en color Verde.";           
            }else{
                LabUpdateModulo.Text = "No ha seleccionado ningun Módulo.";
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
            if (cd != null)
            {
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

                    if (tieneop == 1)
                    {
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
                        int existe = pantallasRegistradas(vop, hidopcionpant.Value, hidpantallaid.Value);
                        if ((PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios")) && existe < 1)
                        {
                            vop.Visible = false;
                            ulp4.Visible = false;
                        }
                        liop5.Controls.Add(vop);

                        ulp4.Controls.Add(liop5);
                        //pantallasRegistradas(vop, hidopcionpant.Value, hidpantallaid.Value);
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

                            v.ID = hidpantallaid.Value + "," + "" + cont + "" + opsub + "," + modulo.idModulo + "," + indexpantalla.ToString();
                            int existe = pantallasRegistradas(v, hidopcionpant.Value, hidpantallaid.Value);
                            if ((PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios")) && existe < 1)
                            {
                                v.Visible = false;
                                li.Visible = false;
                            }
                            li.Controls.Add(v);

                            aux = cont;
                        }
                        else
                        {

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
                            else
                            {
                                li.Controls.Add(ulop);
                            }
                            HtmlGenericControl liop = new HtmlGenericControl("li");
                            liop.ID = litreepant + "" + cont + "" + modulo.idModulo;
                            ulop.Controls.Add(liop);
                            CheckBox vop = new CheckBox();
                            vop.Text = pantalla;
                            vop.ID = pantalaId + "," + op + "," + modulo.idModulo + "," + indexpantalla;
                            int existe = pantallasRegistradas(vop, pantalla, pantalaId);
                            if ((PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios")) && existe < 1)
                            {
                                vop.Visible = false;
                                liop.Visible = false;

                            }
                            liop.Controls.Add(vop);
                            if (existe > 0 || (PageIndex.Equals("Opciones") || PageIndex.Equals("Pantallas")))
                            {
                                if (subop)
                                {
                                    agregarCheckboxOpciones(liop, pantalla, modulo.idModulo, opcionp, vop.ID, pantalaId, indicePantalla);
                                }
                            }

                        }
                    }
                    else if (!opcionp.Trim().Contains("sub") && !opcionp.Trim().Contains("op5") && (pantalla.Trim().Length > 0) && valido == 1)
                    {
                        cont++;
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.ID = litreepant + "" + cont + "" + modulo.idModulo;
                        FindControl("ultree" + "" + conul + "" + modulo.idModulo).Controls.Add(li);
                        CheckBox v = new CheckBox();
                        v.Text = pantalla;
                        v.ID = pantalaId + "," + cont + "," + modulo.idModulo + "," + indexpantalla;
                        int existe2 = pantallasRegistradas(v, pantalla, pantalaId);
                        if ((PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios")) && existe2 < 1)
                        {
                            v.Visible = false;
                            li.Visible = false;
                        }
                        li.Controls.Add(v);
                        if (existe2 > 0 || (PageIndex.Equals("Opciones") || PageIndex.Equals("Pantallas")))
                        {
                            if (subop)
                            {
                                agregarCheckboxOpciones(li, pantalla, modulo.idModulo, opcionp, v.ID, "", indicePantalla);
                            }
                        }

                    }

                }
                contCBKSTotal = contCBKSTotal + cont;
                hidcontchecks.Value = contCBKSTotal.ToString();
            }
           
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

        private int pantallasRegistradas(CheckBox pregis,String nombre, String idasp) {
            Pantalla pid = _pantallaBL.getPantalla(null, idasp);
           int estado = 1;
            if (PageIndex != null)
            {
                //Pantalla pid = _pantallaBL.getPantalla(null, idasp);
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
                if (pid.idPantalla < 1 || pid.estado>0)
                {
                estado = 0;
                }                
            }
            return estado;
        }        

        private int OpcionesRegistradas(CheckBox pregis, String idasp, String idcheckbox)
        {
            Opcion oid = _opcionBL.getOpcion(idasp, idcheckbox,0,0);
            int estado = 0;
            if (PageIndex != null)
            {
                if (PageIndex.Equals("Opciones"))
                {
                    pregis.ForeColor = System.Drawing.Color.Blue;                    
                }

                if (oid.idPantalla > 0)
                {
                    pregis.ID = pregis.ID + "," + oid.idOpcion;
                    if (PageIndex.Equals("Opciones"))
                    {
                        if (oid.estado > 0)
                        {
                            pregis.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            pregis.Checked = true;
                            pregis.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    else if (PageIndex.Equals("Acceso por Perfiles"))
                    {
                        perfilesOpcionesEnDB(pregis, oid.idOpcion, int.Parse(HidUsuSeleccionadoSeg.Value));
                    }
                    else if (PageIndex.Equals("Acceso por Usuarios"))
                    {
                        UsuariosOpcionesEnDB(pregis, oid.idOpcion, int.Parse(HidUsuSeleccionadoSeg.Value));
                    }
                }
                if(oid.idPantalla == 0 || oid.estado>0) {
                    estado = 1;
                }
            }
            return estado;
        }

        private void perfilesModulosEnDB(CheckBox regis, int idperfil, int idmodulo)
        {
            PerfilesModulos pmod = _PerfilesModulosBL.getPerfilModulo(idperfil, idmodulo);
            if (pmod.idPerfilModulo > 0)
            {
                regis.ID = regis.ID + "," + pmod.idPerfilModulo;

                regis.ForeColor = System.Drawing.Color.Green;
                if (bool.Parse(pmod.divVisible))
                {
                    regis.Checked = true;                    
                }
            }
        }

        private void perfilesPantallasEnDB(CheckBox regis, int idperfil, int idpantalla)
        {
            PerfilesPantallas ppantalla = _PerfilesPantallasBL.getPerfilPantalla(idperfil, idpantalla);
            if (ppantalla.idPerfilPantalla > 0)
            {
                regis.ID = regis.ID + "," + ppantalla.idPerfilPantalla;

                regis.ForeColor = System.Drawing.Color.Green;
                if (bool.Parse(ppantalla.visible))
                {
                    regis.Checked = true;                    
                }
            }
        }

        private void perfilesOpcionesEnDB(CheckBox regis, int idOpcion, int idPerfil)
        {
            PerfilesOpciones perfilopcion = _PerfilesOpcionesBL.getPerfilOpcion(idOpcion, idPerfil);
            if (perfilopcion.idPerfilOpcion > 0)
            {
                regis.ID = regis.ID + "," + perfilopcion.idPerfilOpcion;

                regis.ForeColor = System.Drawing.Color.Green;
                if (bool.Parse(perfilopcion.visible))
                {
                    regis.Checked = true;                   
                }
            }
        }

        private void UsuariosModulosEnDB(CheckBox regis, int idusuario, int idmodulo)
        {
           UsuariosModulos umod = _UsuariosModulosBL.getUsuarioModulo(idusuario, idmodulo);
           if (umod.idUsuarioModulo > 0)
            {
                regis.ID = regis.ID + "," + umod.idUsuarioModulo;

                regis.ForeColor = System.Drawing.Color.Green;
                if (bool.Parse(umod.divVisible))
                {
                    regis.Checked = true;                    
                }
            }
        }

        private void UsuariosPantallasEnDB(CheckBox regis, int idusuario, int idpantalla)
        {
            UsuariosPantallas usuariopantalla = _UsuariosPantallasBL.getUsuarioPantalla(idusuario, idpantalla);
            if (usuariopantalla.idUsuarioPantalla > 0)
            {
                regis.ID = regis.ID + "," + usuariopantalla.idUsuarioPantalla;

                regis.ForeColor = System.Drawing.Color.Green;
                if (bool.Parse(usuariopantalla.visible))
                {
                    regis.Checked = true;                   
                }
            }
        }

        private void UsuariosOpcionesEnDB(CheckBox regis, int idOpcion, int idUsuario)
        {
            UsuariosOpciones perfilopcion = _UsuariosOpcionesBL.getUsuarioOpcion(idOpcion, idUsuario);
            if (perfilopcion.idUsuarioOpcion > 0)
            {
                regis.ID = regis.ID + "," + perfilopcion.idUsuarioOpcion;

                regis.ForeColor = System.Drawing.Color.Green;
                if (bool.Parse(perfilopcion.visible))
                {
                    regis.Checked = true;                    
                }
            }
        }

        private void sistemasModulosEnDB(CheckBox regis, int idsistema, int idmodulo)
        {
            SistemasModulos sismod = _SistemasModulosBL.getSistemaModulo(idsistema, idmodulo);
            if (sismod.idSistemaModulo > 0)
            {
                regis.ID = regis.ID + "," + sismod.idSistemaModulo;

                regis.ForeColor = System.Drawing.Color.Green;
                if(bool.Parse(sismod.divvisible)){
                    regis.Checked = true;                                    
                }
                }
        }

        private void agregarCheckboxOpciones(HtmlGenericControl liPantalla, String nompantalla, int idmodulo, String comando, String idpantalla, String subop3,String indicePantalla)
        {
            String opcion =componentesOpciones( comando, subop3);            

            string[] indicepantallas = idpantalla.Split(new Char[] { ',' });
            string idpantallaop = "";
            if (indicepantallas.Length > 4)
            {
                idpantallaop = indicepantallas[4];
            }                        

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
                                   int estado=OpcionesRegistradas(vop, splitid[0], idpantallaop + splitid[0] + splitid[ps + 1]);
                                   if (estado > 0 && (PageIndex.Equals("Acceso por Perfiles") || PageIndex.Equals("Acceso por Usuarios")))
                                   {
                                       vop.Visible = false;
                                   }
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

        protected internal string componentesOpciones(String comando, String subop3)
        {
            String opcion = "";
            if (comando.ToLower().Contains("usuarios"))
            {
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
                if (splitidview.Length > 1)
                {
                    opcion = MultiView1Seg.Views[int.Parse(splitidview[1])].ClientID;
                }
            }
            else if (comando.ToLower().Contains("seguimientotarea"))
            {
                opcion = MultiViewSeguimientoTarea.Views[0].ClientID;
            }
            else if (comando.ToLower().Contains("crconsulta"))
            {
                opcion = MultiViewConsultaReporte.Views[0].ClientID;
            }
            return opcion;
        }

        private void validarOpcionesRegistradas( String comando, String subop3)
        {
            //valida si estan registradas las opciones en la base de datos, si no estan registradas
            //se desactivan 
            String opcion =componentesOpciones( comando, subop3);
            int totalOp = 0;
            int countOpcionReg = 0;                        

            if (opcion.Trim().Length > 0)
            {
                string[] splitOps = opcion.Split(new Char[] { ',' });

                for (int num = 0; num < splitOps.Length; num++)
                {
                    Control cd = FindControl(splitOps[num]);

                    if (cd != null)
                    {
                        
                        if (cd.Controls.Count > 0)
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
                                        totalOp++;
                                    }
                                    if (c.GetType() == typeof(FileUpload))
                                    {
                                        subOpcion = "Seleccionar Archivo,";
                                        subOpcionId = (c as FileUpload).ClientID + ",";
                                        totalOp++;
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
                                                totalOp++;
                                            }
                                            if (cs.Columns[col - 2].GetType() == typeof(CommandField))
                                            {
                                                subOpcion = subOpcion + ",Editar";
                                                subOpcionId = subOpcionId + "," + (col - 2);
                                                totalOp++;
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
                                    for (int ps = 0; ps <= (split.Length - 1); ps++)
                                    {
                                        if (split[ps].Length > 0)
                                        {                                            
                                            Opcion existe = _opcionBL.getOpcion(splitid[0], splitid[0] + splitid[ps + 1], 1, int.Parse(hidindexpantalla.Value));
                                            if (existe.idOpcion < 1 || existe.idcheckbox.Trim().Length < 1 || existe.estado>0)
                                            {
                                                activarComponenteOpcion(splitid[ps + 1], splitid[0], "false");
                                                countOpcionReg++;
                                            }
                                                
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            if(totalOp==countOpcionReg && totalOp>0){
                LblSinAcessoOpciones.Text = "\n Se han desactivado las Opciones. \n No se han registrado las Opciones. ";         
            }
        }

        private void agregarOpcionSubop(String opcionp, int opsub, String liidop,String ulparentid, String nomCheck,String chkboxid) { 
                
        }

        protected void ButtonAtualizaPantallasSeg_Click(object sender, EventArgs e)
        {
            LblSinRelacion.Text = "";
            abdCheckBoxTree(false,true,false,false,false);
        }

        protected void ButtonDeletePantallaOpcion_Click(object sender, EventArgs e)
        {
            LblSinRelacion.Text = "";
            activarPantalla = 0;
            activarOpciones = 0;
            abdCheckBoxTree(false,false,false,false,true);
            actualizaActivarDesactivar();
        }


        protected void ButtonDesactivarPantallaOpcion_Click(object sender, EventArgs e)
        {
            activarPantalla = 1;
            activarOpciones = 1;
            abdCheckBoxTree(false,false,false,false,true);
            actualizaActivarDesactivar();
            LblSinRelacion.Text = "";
        }

        private void actualizaActivarDesactivar() { 
        if(PageIndex.Equals("Pantallas")){
         llenarTree(false, false);
        }
        else if (PageIndex.Equals("Opciones"))
        {
            llenarTree(true, false);
        }
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
                
                    for (int sm = 0; sm < sistema.Count; sm++)
                    {
                        cont++;
                        Modulo d = sistema[sm].modulo;
                        if (sistema[sm].idSistemaModulo > 0 && sistema[sm].modulo.estado==0)
                        {
                        if (bool.Parse(sistema[sm].divvisible) && bool.Parse(sistema[sm].h3visible))
                        {
                            agregarCheckboxmodulos(d, cont, subopciones, divTree, relAccesoPerfilb);
                        }
                        }
                    }
                    if (sistema.Count == 0)
                    {
                        LblSinRelacionRel.Text = "No se ha asignado ningun Módulo al Sistema";
                    }
                                       
            }else{
                List<Sistema> sis = _sistemaBL.getSistemas();
                Sistema sisSICT = sis.Find(x => x.clave.Trim().ToLower() == "sict");
              List<SistemasModulos>  sistema = _SistemasModulosBL.getSistemasModulos(sisSICT.idSistema, 0);
              modulossict = null;
                modulossict = new List<Modulo>();
              for (int s = 0; s < sistema.Count;s++ )
              {
                  if (sistema[s].modulo.idModulo>0)
                  {
                  modulossict.Add(sistema[s].modulo);
                  }
                  }

              foreach (Modulo d in modulossict)
            {
                cont++;
                agregarCheckboxmodulos(d, cont, subopciones, divTree, relAccesoPerfilb);
            }
            if (PageIndex.Trim().Equals("Pantallas")||PageIndex.Trim().Equals("Opciones"))
            {
                if (_modulosBl.getModulos().Count==0)
                {
                    LblSinRelacion.Text = "No se ha registrado ningun Módulo";
                }
            }
            
            }
        }
        protected void ButtonRegistrarPantallasSeg_Click(object sender, EventArgs e)
        {
            LblSinRelacion.Text = "";
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
            LblStatusModSis.Text = "";
            int idSeleecionadoini = int.Parse(HidUsuSeleccionadoSeg.Value);
            if (idSeleecionadoini > 0)
            {
                int cont = 0;
                int checkstree = 0;
                List<String> actualizados = new List<String>();
                int checkstreeAsignado = 0;

                foreach (Modulo d in _modulosBl.getModulos())
                {
                    cont++;

                    Control c = FindControl(limod + d.Nombre);
                    if (c != null)
                    {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox modulo = (c.Controls[0] as CheckBox);
                            if (modulo.Checked)
                            {
                                checkstreeAsignado++;
                            }

                            checkstree++;
                            string[] split = modulo.ClientID.Split(new Char[] { ',' });
                            if (split[1].Trim().Length > 0)
                            {
                                if (PageIndex.Equals("Acceso por Perfiles"))
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
                                else if (PageIndex.Equals("Acceso por Usuarios"))
                                {
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

                    if (actualiza)
                    {
                        updateMsg(actualizados, LblStatusModSis, "Módulos no actualizados");
                    }
                    if (registra && (checkstreeAsignado > 0) && int.Parse(HidUsuSeleccionadoSeg.Value) > 0)
                    {
                        LblStatusModSis.Text = "Las Módulos en color verde se han Asignado correctamente.";
                    }
                    if (registra && (checkstreeAsignado < 1) && int.Parse(HidUsuSeleccionadoSeg.Value) > 0)
                    {
                        LblStatusModSis.Text = "No ha seleccionado ninguna Opción";
                    }

                }
                else
                {

                    insertUpdatePerfilesPantallas(registra, actualiza, true, false);
                    insertUpdatePerfilesOpciones(registra, actualiza, true, false);
                    llenarTree(true, relAccesoperfil);

                    if (actualiza && (checkstree > 0))
                    {
                        updateMsg(actualizados, LblStatusAccePerfil, "Módulos no actualizados");
                    }
                }
            }
            else if (PageIndex.Equals("Acceso por Perfiles"))
            {
                LblStatusAccePerfil.Text = "No ha seleccionado ningun Perfil.";
            }
            else if (PageIndex.Equals("Acceso por Usuarios"))
            {
                LblStatusAccePerfil.Text = "No ha seleccionado ningun Usuario.";
            }
            else if (PageIndex.Equals("Sistemas y Módulos"))
            {
                LblStatusModSis.Text = "No ha seleccionado ningun Sistema.";
            }
        }

        private void insertUpdatePantallas(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs, bool delete)
        {
            LblupdatePantalla.Text = "";
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();
            int chkActivado = 0;

            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;

                for (int chk = 1; chk <= int.Parse(hidcontchecks.Value); chk++)
                {

                    Control c = FindControl(litreepant + "" + chk + "" + d.idModulo);
                    if (c != null)
                    {
                        if (c.Controls.Count>0)
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
                                   Object estado=  _pantallaBL.registrarPantalla(panta);
                                   String [] sta=estado.GetType().GetProperties().Select(p => p.Name).ToArray();
                                   string vcv = estado.GetType().GetProperty("Success").GetValue(estado, null).ToString();
                                   actualizados.Add(estado.GetType().GetProperty("Success").GetValue(estado, null) + "," + panta.nombre + ". Detalle: " + estado.GetType().GetProperty("ErrorMessage").GetValue(estado, null));                                        
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
                                if((c.Controls[0] as CheckBox).Checked){
                                chkActivado++;
                                }
                                eliminarPantallasOpciones(c);
                            }
                        }                        
                    }      
                    }
                }
            }                       
                if (chkActivado > 0 && activarPantalla == 0)
                {
                    LblupdatePantalla.Text = "Las Pantallas en color verde se han Activado correctamente.";
                }
                else if (chkActivado > 0 && activarPantalla == 1)
                {
                    LblupdatePantalla.Text = "Las Pantallas en color rojo se han Desactivado correctamente.";
                }
            
           if (chkActivado == 0 && delete)
            {
                LblupdatePantalla.Text = "No ha seleccionado ninguna Pantalla";
            }
            if (registra)
            {
                llenarTree(false,false);
                if (checkstree > 0)
                {
                LblupdatePantalla.Text="Las Pantallas en color verde se han registrado correctamente.";
                }
                    //generar log de todas las pantallas;
                    //updateMsg(actualizados, LblupdatePantalla, "Las siguientes Pantallas no se registraron");                
            }
            else if(actualiza &&(checkstree>0))
            {
                updateMsg(actualizados,LblupdatePantalla, "Las siguientes Pantallas no se actualizaron");                
            }
            if (checkstree <1 && !delete)
            {
             LblupdatePantalla.Text = "No ha seleccionado ninguna Pantalla";
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
                    _pantallaBL.DeletePantalla(int.Parse(split[4]), activarPantalla);                                                            
                }
            }
            }
            else if (PageIndex.Equals("Opciones"))
            {
                
                if (split.Length > 5)
                {
                    if (split[5].Trim().Length > 0)
                    {
                        _opcionBL.DeleteOpcion(int.Parse(split[5]), activarOpciones);                                                
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

        private void insertUpdateOpciones(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs, bool delete)
        {
            LblupdatePantalla.Text = "";
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();
            int chkActivado = 0;

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
                                if((c.Controls[0] as CheckBox).Checked){
                                    chkActivado++;
                                }
                                eliminarPantallasOpciones(c);
                            }
                        }
                        
                    }
                }
            }
            if (chkActivado > 0 && activarOpciones == 0)
            {
                LblupdatePantalla.Text = "Las Opciones en color verde se han Activado correctamente.";
            }
            else if (chkActivado > 0 && activarOpciones == 1)
            {
                LblupdatePantalla.Text = "Las Opciones en color rojo se han Desactivado correctamente.";
            }
            if (chkActivado == 0 && delete)
            {
                LblupdatePantalla.Text = "No ha seleccionado ninguna Opción";
            }
            
            if (registra)
            {
                llenarTree(true,false);
                if (checkstree > 0)
                {
                LblupdatePantalla.Text = "Las Opciones en color verde se han registrado correctamente.";
                }
            }
            else if (actualiza&&(checkstree > 0))
            {
                updateMsg(actualizados, LblupdatePantalla, "Las siguientes Opciones no se actualizaron");
            }
            if ((checkstree < 1 && !delete))
                {
                    LblupdatePantalla.Text = "No ha seleccionado ninguna Opcion.";
                }
        }

        private void insertUpdatePerfilesPantallas(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs)
        {
            //por default todas las opciones se asignan y se actualizan
            int cont = 0;
            int checkstree = 0;
            List<String> actualizados = new List<String>();
            int checkstreeAsignado = 0;

            foreach (Modulo d in _modulosBl.getModulos())
            {
                cont++;

                for (int chk = 1; chk <= int.Parse(hidcontchecks.Value); chk++)
                {

                    Control c = FindControl(litreepant + "" + chk + "" + d.idModulo);
                    if (c != null)
                    {
                        if (c.Controls.Count>0)
                        {
                        if (c.Controls[0].GetType() == typeof(CheckBox))
                        {
                            CheckBox pantalla = (c.Controls[0] as CheckBox);

                            if (pantalla.Checked)
                            {
                            checkstreeAsignado++;
                            }

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
            }
            if (registra && (checkstreeAsignado > 0) && int.Parse(HidUsuSeleccionadoSeg.Value) > 0)
            {
                LblStatusAccePerfil.Text = "Las opciones en color verde se han asignado correctamente.";
            }
            if (registra && (checkstreeAsignado < 1) && int.Parse(HidUsuSeleccionadoSeg.Value) > 0)
            {
                LblStatusAccePerfil.Text = "No ha seleccionado ninguna Opción";
            }
             if (actualiza && (checkstree > 0))
            {
                updateMsg(actualizados, LblStatusAccePerfil, "Pantallas no actualizadas");
            }                            
        }

        private void insertUpdatePerfilesOpciones(bool registra, bool actualiza, bool relAccesoperfil, bool relaccesoUs)
        {
            //por default todas las opciones se asignan y se actualizan
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
            List<String> actualizados = new List<string>();
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
                            actualizados.Add(_modulosBl.UpdateModulo(modulo).Success + "," + modulo.Nombre);                                                      
                        }
                    }
                }
            }
            updateListadoModulo();
            if (numModulos==0)
            {
                LabUpdateModulo.Text = "No ha seleccionado ningun Módulo.";
            }
            else if (numModulos > 0)
            {
                updateMsg(actualizados,LabUpdateModulo,"Módulos no actualizados");
            }                        
        }

        private void cambiarTextLabel(Label lab, String texto, System.Drawing.Color color) {
            lab.Text = texto;
            lab.ForeColor = color;
        }
        
        protected void ButtonEliminaModuloSeg_Click(object sender, EventArgs e)
        {
            LabUpdateModulo.Text = "Los Módulos activados se muestran en color Verde.";
            activarDesactivarModulo(0);             
        }
        private void activarDesactivarModulo(int activarModulo){

            int seleccionados = 0;
            foreach (ListItem item in CheckBoxListModulo.Items)
            {
                if (item.Selected)
                {
                    seleccionados++;
                    string[] split = item.Value.Split(new Char[] { ',' });
                    if (split.Length > 2)
                    {
                        if (split[2].Length > 0)
                        {
                            Modulo modulo = new Modulo();
                            modulo.idModulo = int.Parse(split[2]);                            
                                _modulosBl.DeleteModulo(modulo,activarModulo);                                                      
                        }
                    }
                }
            }
            updateListadoModulo();
            if(seleccionados==0){
                LabUpdateModulo.Text = "No ha seleccionado ningun Módulo."; 
            }
        }
        protected void ButtonDesactivaModuloSeg_Click(object sender, EventArgs e)
        {
            LabUpdateModulo.Text = "Los Módulos desactivados se muestran en color Rojo."; 
            activarDesactivarModulo(1);                                    
        }

        protected void ButtonRegistrarRelAccesoSeg_Click(object sender, EventArgs e)
        {
            RegModulosRelAcceso(true,false,true,false);
            LblSinRelacionRel.Text = "";
        }

        protected void ButtonAtualizaRelAccesoSeg_Click(object sender, EventArgs e)
        {
            RegModulosRelAcceso(false, true, true, false);
            LblSinRelacionRel.Text = "";
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
            consultabusqueda = "";
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
                busquedaseg = true;
                consultabusqueda = consulta;
                validarGridView(grid); 
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
        protected internal string formatoFecha(String fechainicial)
        {
        string fecha="";
        if (fechainicial.Trim().Length > 0)
        {
            string[] split = fechainicial.Split(new Char[] { ' ' });
                string[] splitFormato = split[0].Split(new Char[] { '/' });
                    fecha=splitFormato[2]+"/"+splitFormato[1]+"/"+splitFormato[0];
                }
            return fecha;
        }

        protected void GridView2Seg_RowEditing(object sender, GridViewEditEventArgs e)
        {            
            if (PageIndex.Equals("Sistemas"))
            {
                lblUpdateSistema.Text = "";
                Sistema sistema = _sistemaBL.getSistema(int.Parse(GridViewsistema.Rows[e.NewEditIndex].Cells[0].Text));
                HidSistemaUpdate.Value = GridViewsistema.Rows[e.NewEditIndex].Cells[0].Text;
                TextBoxClaveSis.Text = sistema.clave;
                TextBoxNombreSis.Text = sistema.nombre;
                TextBoxClienteSis.Text = sistema.cliente;
                TextBoxDescSis.Text =sistema.descripcion;
                TextBoxFechaoIniSis.Text = formatoFecha(sistema.fechaInicio);
                TextBoxFechaFinEsSis.Text = formatoFecha(sistema.fechaFinEstimada);
                TextBoxFinRealSis.Text = formatoFecha(sistema.fechaFinReal);               
                TextBoxTecSistema.Text = sistema.tecnologias;
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
            
                if (PageIndex.Equals("Usuarios"))
                {
                    GridView2Seg.Rows[e.NewEditIndex].Cells[GridView2Seg.Rows[e.NewEditIndex].Cells.Count - 1].Attributes.Remove("onclick");
                }
                else
                    if (PageIndex.Equals("Grupos"))
                    {
                        GridView2SegGrupo.Rows[e.NewEditIndex].Cells[GridView2SegGrupo.Rows[e.NewEditIndex].Cells.Count - 1].Attributes.Remove("onclick");
                    }
                    else
                        if (PageIndex.Equals("Perfiles"))
                        {
                            GridView2SegPerfil.Rows[e.NewEditIndex].Cells[GridView2SegPerfil.Rows[e.NewEditIndex].Cells.Count - 1].Attributes.Remove("onclick");
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
                    List<Sistema> sis = _sistemaBL.getSistemas();
                    Sistema sisSICT = sis.Find(x => x.clave.Trim().ToLower() == "sict");
                    UsuarioLogin validarRelaciones = _CAUsuarioBL.getUsuarioLogeado(cell.Text.Trim(), sisSICT.idSistema);
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

                 int statusAgrega = 2;
                 int statusDesasigna = 2;
                 if (!string.IsNullOrEmpty(leftSelectedItems))
                 {
                     foreach (string item in leftSelectedItems.Split(','))
                     {
                         if (PageIndex.Equals("Relacion de Usuarios"))
                         {
                             statusAgrega = _grupoBL.agregarUsuarioGrupo(int.Parse(idus), int.Parse(item));
                         }
                         else if (PageIndex.Equals("Relacion de Grupos"))
                         {
                             statusAgrega = _grupoBL.agregarUsuarioGrupo(int.Parse(item), int.Parse(idus));
                         }
                         else if (PageIndex.Equals("Relacion de Perfiles"))
                         {
                             statusAgrega = _perfilBL.agregarUsuarioPerfil(int.Parse(idus), int.Parse(item));
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
                            statusDesasigna= _grupoBL.eliminarUsuarioGrupo(int.Parse(idus), int.Parse(item));
                         }
                         else if (PageIndex.Equals("Relacion de Grupos"))
                         {
                             statusDesasigna = _grupoBL.eliminarUsuarioGrupo(int.Parse(item), int.Parse(idus));
                         }
                         else if (PageIndex.Equals("Relacion de Perfiles"))
                         {
                            statusDesasigna= _perfilBL.eliminarUsuarioPerfil(int.Parse(idus), int.Parse(item));
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

                 if (statusAgrega > 0 || statusDesasigna > 0)
                 {
                     lblStatusRelacionUs.Text = "Datos guardados con exito.";
                 }
                 else if(statusAgrega == 0 || statusDesasigna ==0) {
                     lblStatusRelacionUs.Text = "No hay cambios por guardar.";
                 }
                 else if (statusAgrega < 0 || statusDesasigna < 0)
                 {
                     lblStatusRelacionUs.Text = "Algunos datos no se han podido guardar.";
                 }
             }
             else if (PageIndex.Equals("Relacion de Usuarios") || PageIndex.Equals("Relacion de Perfiles"))
             {
                 lblStatusRelacionUs.Text = "No ha seleccionado ningun Usuario.";
             }
             else if (PageIndex.Equals("Relacion de Grupos"))
             {
                 lblStatusRelacionUs.Text = "No ha seleccionado ningun Grupo.";
             }
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
             actualizaPagina("Captura de Tareas");
             getPantallaIndex();
             ControlAccesoOpciones();
             MultiViewSeguimientoTarea.ActiveViewIndex = 0;
             busquedaseg = false;
         }
         
         protected void cerrarSesiononclick(Object sender, EventArgs e)
         {
             expirarSesion();
        }

         private void expirarSesion() {
             FormsAuthentication.SignOut();
             GlobalDataSingleton.Instance.controlAcceso = "";
             Response.Redirect("Views/Login/Inicio.aspx");            
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

        //fin modulo seguimiento tarea
        //Inicio Módulo Consultas y Reportes

         protected void ConsultasOnClick(object sender, EventArgs e)
         {
             DropDownBindProyectosCR();
             LblStatusDescargaCR.Text = "";
             LblStatusDescargaCR.ForeColor = System.Drawing.Color.Black;
             actualizaPagina("Consultas");             
             _usuarioFacade.DropDownBinUsuariosCR(DropDownListUsuarioCR);          
             limpiaFormConsultasCR();
             MultiViewConsultaReporte.ActiveViewIndex = 0;
             System.GC.Collect();
             getPantallaIndex();
             ControlAccesoOpciones();             
         }

         protected void ButtonBusquedaCR_Click(object sender, EventArgs e)
         {
             LblStatusDescargaCR.Text = "";
             LblStatusDescargaCR.ForeColor = System.Drawing.Color.Black;           
             System.GC.Collect();
             SICTWS.ModuloTarea datostarea = new SICTWS.ModuloTarea();
             datostarea.nombre = TextBoxTarea.Text;
             datostarea.estado = DropDownListEstado.SelectedValue;
             datostarea.FechaInicio = TextBoxFechaInicioCR.Text;
             datostarea.FechaFinEstimada = TextBoxFechaFinEstimCR.Text;
             datostarea.FechaFinReal = TextBoxFinRCR.Text;
              tarea = null;              
              tarea = _sictws.getModuloTarea(datostarea, DropDownListOpcTareasCR.SelectedItem.Text.ToLower(), int.Parse(DropDownListUsuarioCR.SelectedValue), int.Parse(DropDownListSistemaCR.SelectedValue));                      
             GridViewCRConsulta.AutoGenerateColumns = true;
              gvBindCRBusqueda(tarea, GridViewCRConsulta);            
             System.GC.Collect();
         }
         private  void gvBindCRBusqueda(DataTable  tareas,GridView grid)
         {                                                                        
             if (tareas.Rows.Count > 0)
             {                
                 grid.DataSource = null;
                 grid.DataSource = tareas;                                
                 grid.DataBind();                 
             }
             else
             {
                 LblStatusDescargaCR.Text = "Sin resultados de busqueda.";
                 grid.DataSource = null;
                 grid.DataBind();
             }
           System.GC.Collect();
         }

         protected void ButtonExportarExcelCR_Click(object sender, EventArgs e)
         {
             if (tarea != null)
             {
                 if (tarea.Rows.Count > 0)
                 {
                     contConsultaCR++;
                     String nombre = "ConsultaSICT" + contConsultaCR;
                     if (TextBoxFechaInicioCR.Text.Trim().Length > 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFinRCR.Text.Trim().Length == 0 && TextBoxTarea.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaInicio";
                     }
                     else if (TextBoxFechaFinEstimCR.Text.Trim().Length > 0 && TextBoxFechaInicioCR.Text.Trim().Length == 0 && TextBoxFinRCR.Text.Trim().Length == 0 && TextBoxTarea.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaFinEstimada";
                     }
                     else if (TextBoxFinRCR.Text.Trim().Length > 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFechaInicioCR.Text.Trim().Length == 0 && TextBoxTarea.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaFinReal";
                     }
                     else if (TextBoxFechaInicioCR.Text.Trim().Length > 0 && TextBoxFechaFinEstimCR.Text.Trim().Length > 0 && TextBoxFinRCR.Text.Trim().Length == 0 && TextBoxTarea.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaInicioFechaFinEstimada";
                     }
                     else if (TextBoxFechaInicioCR.Text.Trim().Length > 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFinRCR.Text.Trim().Length > 0 && TextBoxTarea.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaInicioFechaFinReal";
                     }
                     else if (TextBoxTarea.Text.Trim().Length == 0 && DropDownListOpcTareasCR.SelectedIndex > 0 && TextBoxFinRCR.Text.Trim().Length == 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFechaInicioCR.Text.Trim().Length == 0 && TextBoxTarea.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + DropDownListOpcTareasCR.SelectedItem.Text;
                     }
                     else if (TextBoxTarea.Text.Trim().Length > 0 && DropDownListOpcTareasCR.SelectedIndex == 0 && TextBoxFinRCR.Text.Trim().Length == 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFechaInicioCR.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "Tarea";
                     }
                     else if (TextBoxTarea.Text.Trim().Length == 0 && DropDownListOpcTareasCR.SelectedIndex == 0 && TextBoxFinRCR.Text.Trim().Length > 0 && TextBoxFechaFinEstimCR.Text.Trim().Length > 0 && TextBoxFechaInicioCR.Text.Trim().Length > 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaInicioFechaFinEstimada";
                     }
                     else if (TextBoxTarea.Text.Trim().Length == 0 && DropDownListOpcTareasCR.SelectedIndex == 0 && TextBoxFinRCR.Text.Trim().Length > 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFechaInicioCR.Text.Trim().Length > 0 && DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "FechaInicioFechaFinReal";
                     }
                     else if (TextBoxTarea.Text.Trim().Length > 0 && DropDownListOpcTareasCR.SelectedIndex == 0 && TextBoxFinRCR.Text.Trim().Length == 0 && TextBoxFechaFinEstimCR.Text.Trim().Length == 0 && TextBoxFechaInicioCR.Text.Trim().Length == 0 && DropDownListUsuarioCR.SelectedIndex > 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "TareasUsuario";
                     }
                     else if (DropDownListUsuarioCR.SelectedIndex > 0 && DropDownListSistemaCR.SelectedIndex > 0)
                     {
                         nombre = nombre + "UsuarioProyecto";
                     }
                     else if (DropDownListUsuarioCR.SelectedIndex > 0 && DropDownListSistemaCR.SelectedIndex == 0)
                     {
                         nombre = nombre + "Usuario";
                     }
                     else if (DropDownListUsuarioCR.SelectedIndex == 0 && DropDownListSistemaCR.SelectedIndex > 0)
                     {
                         nombre = nombre + "Proyecto";
                     }

                     ExportaExcel c = new ExportaExcel();
                     if (c.SetDataTable_To_Excel(nombre, tarea, Response).Success)
                     {
                         LblStatusDescargaCR.Text = "Descarga exitosa.";
                     }
                     else
                     {
                         LblStatusDescargaCR.ForeColor = System.Drawing.Color.Red;
                         LblStatusDescargaCR.Text = "No se ha podido descargar el archivo: <br /> " + c.status.ErrorMessage;

                     }
                     ClientScript.RegisterStartupScript(this.GetType(), "setTimeout", "setTimeout(function () {$('[id$=LblStatusDescargaCR]').fadeOut();}, 20000);", true);
                     System.GC.Collect();

                 }
                 else {
                     LblStatusDescargaCR.Text = "No hay datos para generar el archivo Excel.";
                 }
             }
             else
             {
                 LblStatusDescargaCR.Text = "No hay datos para generar el archivo Excel.";
             }         
    }        

        protected internal void limpiaFormConsultasCR(){
            TextBoxFechaInicioCR.Text = "";
            TextBoxFechaFinEstimCR.Text = "";
            TextBoxFinRCR.Text = "";
            TextBoxTarea.Text = "";
            TextBoxFechaInicioCR.Text = "";
            DropDownListEstado.SelectedIndex = 0;
            DropDownListOpcTareasCR.SelectedIndex = 0;
            DropDownListSistemaCR.SelectedIndex = 0;
        }
        protected void DropDownBindProyectosCR()
        {
            try{
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDProyectos,ClaveProyectos from proyectos where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ds.Tables.Add("d");
            ds.Tables[0].Columns.Add("IDProyectos", typeof(string));
            ds.Tables[0].Columns.Add("ClaveProyectos", typeof(string));
            DataRow f = ds.Tables[0].NewRow();
            Object [] v = new Object[2];
            v[0]="0";
            v[1]="Seleccione un Proyecto";
            f.ItemArray=v;
            ds.Tables[0].Rows.Add(f);
            da.Fill(ds.Tables[0]);                                   
            conn.Close();
            DropDownListSistemaCR.DataSource = ds;
            DropDownListSistemaCR.DataValueField = "IDProyectos";
            DropDownListSistemaCR.DataTextField = "ClaveProyectos";
            DropDownListSistemaCR.DataBind();
            }catch(Exception v){
            
            }
            System.GC.Collect();
        }
       
    }
}