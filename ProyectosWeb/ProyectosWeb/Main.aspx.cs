using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ProyectosWeb.DAO;
using ProyectosWeb.DAO.SeguridadDAOS;
using System.ComponentModel;//for background worker class
using Microsoft.VisualBasic;//for interacation message box
using System.Net;
using System.Net.Mail;
using ProyectosWeb.Models;
using ProyectosWeb.BusinessLogic.Seguridad;



namespace ProyectosWeb
{
    public partial class Main : System.Web.UI.Page
    {
        //private SqlConnection conn = new SqlConnection("Data Source=(local); User ID=sa;Password=Adminpwd20; Integrated Security=true; Initial Catalog=ProyectosGestion;");
        
        private static SqlConnection conn = new SqlConnection("Data Source=172.16.1.31;Initial Catalog=ProyectosGestion;Persist Security Info=True;User ID=sa;Password=Adminpwd20");

        private UsuarioFacade _usuarioFacade = new UsuarioFacade(conn);

        private static PerfilBL _perfilBL = new PerfilBL(conn);
        public static GruposBL _grupoBL = new GruposBL(conn);
        private int usRestore;
        
        private SeguridadDAO accesDao = new SeguridadDAO(); 
        String PageIndex,PrevIndex;
        private int idusuarioSeleccionado;
     
       private  long j=0;
       DateTime hours;
       BackgroundWorker bw = new BackgroundWorker();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //Desactivar Modulo Seguridad
            MultiView1Seg.ActiveViewIndex = -1;
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            //Desactivar Modulo Tareas
            MultiView2.ActiveViewIndex = -1;
            MultiViewTareaGrid.ActiveViewIndex = -1;   
            activarbotonTarea(false);

            DropDownListDep.Visible = false;
            if (IsPostBack&&ViewState["Index"]!=null) {
                PageIndex = ViewState["Index"].ToString();
                /**/
                if (PageIndex.Equals("Usuarios")) {
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
            }
            
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            if(path.Contains("?")){
            string[] split = path.Split(new Char[] { '=' });
            string emailex=split[1].Substring(0,5);
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
                                hidClicks.Value = (int.Parse(hidClicks.Value.ToString())+1).ToString();
                            }
                        }
                        else {
                            PageIndex = "Link Expirado";
                            LabelNav.Text = PageIndex;
                            MultiView1Seg.ActiveViewIndex = 7;
                        }
                    }
                    else {
                        PageIndex = "Link Expirado";
                        LabelNav.Text = PageIndex;
                        MultiView1Seg.ActiveViewIndex = 7;
                    }
            }                
               
            }
            HyperLinkSesion.Visible = false;
            DropDownListDep.Visible = false;
            if (IsPostBack && ViewState["PrevIndex"] != null)
            {
                PrevIndex = ViewState["PrevIndex"].ToString();
                /**/
                
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
            GridView2Seg.Columns[9].HeaderText = "Telefono";
            ((BoundField)GridView2Seg.Columns[9]).DataField = "Telefono";            
            gvbindSeg();
        }
        protected void CuentaUsuarioOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            actualizaPagina("Actualizacion de Cuenta");
            MultiView1Seg.ActiveViewIndex = 4;
            msgactualizado.Text = "";                                      
        }
        protected void RestablecerPasswordOnClick(object sender, EventArgs e)
        {
            MultiView2SegGrid.ActiveViewIndex = -1;
            activarbotonSeg(false);
            actualizaPagina("Contraseña");
            MultiView1Seg.ActiveViewIndex = 5;
            msgactualizado.Text = "";
            LabEmailReg.Text = "";
        }
        protected void RestablecerPasswordEmailOnClick(object sender, EventArgs e)
        {
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
            conn.Open();
            string consulta="";
            GridView opcion=null;
            if(PageIndex.Equals("Usuarios")){
                consulta="select u.IDUsuario as 'leo',u.IDCatalogoProveedores,u.Nombre, u.esEmpleado,u.Contraseña,u.Estado,p.IDPersonas, p.IDUsuario,p.Nombre as nomUsuario,"
                + " p.Apellido,p.FechaRegistro,p.Tecnologias,p.Estado, p.Email, p.Telefono from Usuarios u inner join  Personas  p"
                + " on u.IDUsuario=p.IDUsuario where u.Estado=0";
                opcion = GridView2Seg;
            }else
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

            SqlCommand cmd = new SqlCommand(consulta, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();

            llenarGrid(ds, opcion);                         
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
                         + " if(parseInt(td.children[0].value.length)>11){ td.children[0].value=CellValue.substring(0, 12);}"
                         + " "
                        );
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
            if (PageIndex != null && PageIndex.Equals("Tareas")) {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into " + PageIndex + " values('" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '04' , 0  )  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();


            
            }else 
            if (PageIndex!=null&&PageIndex.Equals("Proyectos"))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into " + PageIndex + " values('" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '04' , 0  )  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            else {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into " + PageIndex + " values(  "+DropDownListDep.SelectedValue.ToString()+", '" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '04' , 0  )  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                 
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
                     + " on u.IDUsuario=p.IDUsuario where u.nombre='" + TextBoxUsuario.Text.Trim() + "'"
                     + " and p.Nombre='" + TextBoxNomUsuario.Text.Trim() + "' and p.apellido='" + TextBoxApellidos.Text.Trim() + "' and p.Tecnologias='" + tecs.Trim() + "'");

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

                         bool exito = registrar("insert into " + PageIndex + "(Nombre,esEmpleado,Contraseña) values('" + TextBoxUsuario.Text.Trim() + "','" + ((RadioButtonEmpleado.SelectedItem.ToString().Equals("Si")) ? 'Y' : 'N') + "','" + passwordHash + "')"
                            + " insert into Personas(IDUsuario,Nombre,apellido,FechaRegistro,Tecnologias,Email,Telefono) values((select idusuario from Usuarios where IDUsuario=@@IDENTITY),"
                            + " '" + TextBoxNomUsuario.Text.Trim() + "','" + TextBoxApellidos.Text + "',GETDATE(),'" + tecs.Trim() + "','" + TextBoxEmail.Text.Trim() + "', nullif(" + tel + ",0))"
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
        

        }

        protected void Button6seg_Click(object sender, EventArgs e) {
            gvbindSeg();
        }

        protected void ButtonActualizaUsuSeg_Click(object sender, EventArgs e)
        {
            String passwordHash = BCrypt.Net.BCrypt.HashPassword(PasswordUsUpdate.Value.ToString(), BCrypt.Net.BCrypt.GenerateSalt(12));
          int exito=  _usuarioFacade.UpdateUsuarioPassword(0,"llr3333",TextBoxUsuarioUpdate.Text.Trim(),passwordHash);
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
                int columncount = GridView2Seg.Rows[0].Cells.Count;
                grid.Rows[0].Cells.Clear();
                grid.Rows[0].Cells.Add(new TableCell());
                grid.Rows[0].Cells[0].ColumnSpan = columncount;
                grid.Rows[0].Cells[0].Text = "Sin resultados de busqueda";
            }
        }

        protected void GridView2Seg_RowEditing(object sender, GridViewEditEventArgs e)
        {
            HiddenRowIndexSegUpd.Value = "";
            HiddenRowIndexSegUpd.Value = "" + e.NewEditIndex;
            if (PageIndex.Equals("Usuarios")) {
                GridView2Seg.EditIndex = e.NewEditIndex;
            }else
            if (PageIndex.Equals("Grupos"))
            {
                GridView2SegGrupo.EditIndex = e.NewEditIndex;
            }else
            if (PageIndex.Equals("Perfiles"))
            {
                GridView2SegPerfil.EditIndex = e.NewEditIndex;
            }
            gvbindSeg();
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
            idtabla=PageIndex.Substring(0, PageIndex.Length - 1);
            cell = GridView2Seg.Rows[e.RowIndex].Cells[0];
            }else
                if (PageIndex.Equals("Perfiles"))
                {
                    idtabla = PageIndex.Substring(0, PageIndex.Length - 2);
                    cell = GridView2SegPerfil.Rows[e.RowIndex].Cells[0];
            } 
            else  if (PageIndex.Equals("Grupos")) {
                idtabla = PageIndex;
                cell = GridView2SegGrupo.Rows[e.RowIndex].Cells[0];
            }
            
            conn.Open();
            SqlCommand cmd = new SqlCommand("update " + PageIndex + " set Estado='1' where ID" + idtabla + "=" + Convert.ToInt32(cell.Text), conn);
            cmd.ExecuteNonQuery();
            conn.Close();
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
           
            TextBox usuario, esempleado, nombre, apellidos, tecnologias, email, telefono;
            GridViewRow row = (GridViewRow)opcion.Rows[e.RowIndex]; 
            TableCell ID = opcion.Rows[e.RowIndex].Cells[0];
            SqlCommand cmd =new SqlCommand();
            opcion.EditIndex = -1;
            if(PageIndex.Equals("Usuarios")){
            //usuario = (TextBox)row.Cells[1].Controls[0];
            esempleado = (TextBox)row.Cells[2].Controls[0];
            nombre = (TextBox)row.Cells[4].Controls[0];
            apellidos = (TextBox)row.Cells[5].Controls[0];
            tecnologias = (TextBox)row.Cells[7].Controls[0];
            email = (row.Cells[8].Controls[0] as TextBox);
            telefono = (row.Cells[9].Controls[0] as TextBox);
            //u.Nombre = '" + usuario.Text.Trim() + "',
            String actualizar = " UPDATE u"
                       + " SET   u.esEmpleado='" + esempleado.Text.Trim().ToUpper() + "'"
                       + " FROM Usuarios AS u"
                       + " INNER JOIN Personas AS P "
                       + "        ON u.IDUsuario = P.IDUsuario "
                       + " WHERE u.IDUsuario = "+ID.Text+""
                       + " UPDATE p"
                       + " SET  p.Nombre = '" + nombre.Text.Trim() + "', p.Apellido='" + apellidos.Text.Trim() + "',p.Tecnologias='" + tecnologias.Text.Trim() + "', p.email='" + email.Text.Trim() + "', p.telefono="+long.Parse(telefono.Text.Trim())+"" 
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
            smt.Credentials = new NetworkCredential("llr.allleo@gmail.com", "superl89");
            smt.EnableSsl = true;
            smt.Send(msg);        
        }
        

        [System.Web.Services.WebMethod]        
        public static string CheckEmail(string email)
        {
           return accesAjax("personas", "email", email);      
        }

        public static string accesAjax(string referencia, string param, String email) { 
            string returnValue = string.Empty;
            try{
                conn.Open();
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
                conn.Close();
            }
            catch
            {
                returnValue = "error";
            }
            return returnValue;  
        
        }
        
        [System.Web.Services.WebMethod]
        public static string CheckUserName(string userName)
        {
            string returnValue = string.Empty;
            try
            {
                conn.Open();
                SqlCommand cmSql = conn.CreateCommand();
                cmSql.CommandText = "Select * from usuarios where  nombre=@parm2";
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters["@parm2"].Value = userName;
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
                conn.Close();
            }
            catch
            {
                returnValue = "error";
            }
            
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
    }
}