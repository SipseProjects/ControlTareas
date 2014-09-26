using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAOS.Seguridad;
using DAOS.Seguridad.ControlAccesoUsuario;
using Models.Seguridad;
using Models;
using Models.Seguridad.ControlAcceso;
using BusinessLogic.Seguridad;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Security;

namespace BusinessLogic.Seguridad.ControlAcceso
{
    public sealed class ControlAcessoUsuarioBL
    {
        private CAUsuarioDAO _usuarioDao;
        private  PerfilesModulosBL _PerfilesModulosBL;
        private PerfilesPantallasBL _PerfilesPantallasBL;
        private PerfilesOpcionesBL _PerfilesOpcionesBL;
        private UsuariosModulosBL _UsuariosModulosBL;
        private UsuariosPantallasBL _UsuariosPantallasBL;
        private UsuariosOpcionesBL _UsuariosOpcionesBL;
        private SistemasModulosBL _SistemasModulosBL;
        private OpcionBL _opcionBL;
       public SistemasModulos _sistemamodulo= new SistemasModulos();

        public ControlAcessoUsuarioBL(SqlConnection conn)
        {
            _usuarioDao = new CAUsuarioDAO(conn);
            _PerfilesModulosBL=new PerfilesModulosBL(conn);
            _PerfilesPantallasBL = new PerfilesPantallasBL(conn);
            _PerfilesOpcionesBL = new PerfilesOpcionesBL(conn);
            _UsuariosModulosBL = new UsuariosModulosBL(conn);
            _UsuariosPantallasBL = new UsuariosPantallasBL(conn);
            _UsuariosOpcionesBL = new UsuariosOpcionesBL(conn);
            _SistemasModulosBL = new SistemasModulosBL(conn);
            _opcionBL = new OpcionBL(conn);
        }

        /// <summary>
        /// obtiene los Grupos, Perfiles, el Sistema, Módulos, Pantallas, Opciones  relacionados a un Usuario en especifico.
        /// </summary>
        public UsuarioLogin getUsuarioLogeado(string username, int idSistema)
        {
            UsuarioLogin usuarioLogin = _usuarioDao.getUsuarioLogeado(username);
            usuarioLogin.sistemasModulos = getSistemasModulosRelaciones(idSistema,usuarioLogin.idUsuario,usuarioLogin.Perfiles);
            return usuarioLogin;
        }

        public List<PerfilesModulos> getPerfilesModulosUsuario(List<PerfilLogin> perfiles, int idModulo)
        {
            List<PerfilesModulos> perfilesModulos =new  List<PerfilesModulos>();
            for(int p=0;p<perfiles.Count;p++){
                perfilesModulos.AddRange(_PerfilesModulosBL.getPerfilesModulos(perfiles[p].idPerfil, idModulo));
            }
            return perfilesModulos;
        }
        public List<PerfilesPantallas> getPerfilesPantallasUsuario(List<PerfilLogin> perfiles, int idPantalla, int idModulo)
        {
            List<PerfilesPantallas> perfilesPantallas = new List<PerfilesPantallas>();
            for (int p = 0; p < perfiles.Count; p++)
            {
                perfilesPantallas.AddRange(_PerfilesPantallasBL.getPerfilesPantallas(perfiles[p].idPerfil, idPantalla, idModulo));
            }
            return perfilesPantallas;
        }
        public List<PerfilesOpciones> getPerfilesOpcionesUsuario(List<PerfilLogin> perfiles, int idOpcion, int pantallaIndex, int idModulo)
        {
            List<PerfilesOpciones> perfilesOpciones = new List<PerfilesOpciones>();
            for (int p = 0; p < perfiles.Count; p++)
            {
                perfilesOpciones.AddRange(_PerfilesOpcionesBL.getPerfilesOpciones(perfiles[p].idPerfil, idOpcion, pantallaIndex, idModulo));
            }
            return perfilesOpciones;
        }
        public List<UsuariosModulos> getUsuariosModulos(int idUsuario, int idModulo)
        {
            return _UsuariosModulosBL.getUsuariosModulos(idUsuario, idModulo);            
        }
        public List<UsuariosPantallas> getUsuariosPantallas(int idUsuario, int idPantalla, int idModulo)
        {
            return _UsuariosPantallasBL.getUsuariosPantallas(idUsuario, idPantalla, idModulo);            
        }
        public List<UsuariosOpciones> getUsuariosOpciones(int idUsuario, int idOpcion, int pantallaIndex, int idModulo)
        {
            return _UsuariosOpcionesBL.getUsuariosOpciones(idUsuario, idOpcion, pantallaIndex, idModulo);            
        }

        /// <summary>
        /// obtiene el acceso a Módulos,Pantallas,Opciones asignados al usuario.
        /// </summary>
        public SistemasModulos getSistemasModulosRelaciones(int idSistema, int idUsuario, List<PerfilLogin> perfiles)
        {
            
            SistemasModulos sistemasModulos= new SistemasModulos();
            List<PerfilesModulos> perfilesModulos = new List<PerfilesModulos>();
            List<PerfilesPantallas> perfilesPantallas = new List<PerfilesPantallas>();
            List<PerfilesOpciones> perfilesOpciones = new List<PerfilesOpciones>();
            List<UsuariosPantallas> usuariosPantallas = new List<UsuariosPantallas>();
            List<UsuariosOpciones> usuariosOpciones = new List<UsuariosOpciones>();
            List<UsuariosModulos> usuariosModulos = new List<UsuariosModulos>();

            sistemasModulos.sistemasModulos = _SistemasModulosBL.getSistemasModulos(idSistema, 0);
            for (int s = 0; s < sistemasModulos.sistemasModulos.Count;s++)
            {
                perfilesPantallas.AddRange(getPerfilesPantallasUsuario(perfiles, 0, sistemasModulos.sistemasModulos[s].idModulo));                 
                usuariosPantallas.AddRange(getUsuariosPantallas(idUsuario, 0, sistemasModulos.sistemasModulos[s].idModulo));
                perfilesOpciones.AddRange(getPerfilesOpcionesUsuario(perfiles, 0, 0, sistemasModulos.sistemasModulos[s].idModulo));
                usuariosOpciones.AddRange(getUsuariosOpciones(idUsuario, 0, 0, sistemasModulos.sistemasModulos[s].idModulo));
                perfilesModulos.AddRange(getPerfilesModulosUsuario(perfiles, sistemasModulos.sistemasModulos[s].idModulo));
                usuariosModulos.AddRange(getUsuariosModulos(idUsuario, sistemasModulos.sistemasModulos[s].idModulo));
            }
            sistemasModulos.perfilesModulos = perfilesModulos;
            sistemasModulos.perfilesPantallas = perfilesPantallas;
            sistemasModulos.usuariosPantallas = usuariosPantallas;
            sistemasModulos.perfilesOpciones = perfilesOpciones;
            sistemasModulos.usuariosOpciones = usuariosOpciones;
            sistemasModulos.usuariosModulos = usuariosModulos;

            return sistemasModulos;
        }

        /// <summary>
        /// obtiene el acceso a opciones asignados al usuario, verifica si estan registrados o desactivados en la base de datos, si es
        /// correcto se desactivan.
        /// </summary>
        public void ControlAccesoOpciones(String userName, String hidindexpantalla, string admin, string redirectMain, Label LblSinAcessoOpciones, string PageIndex, System.Web.HttpResponse Response, Page page, string idComponentePrincipal)
        {
            List<UsuariosOpciones> accesoopcionUsuario = _sistemamodulo.usuariosOpciones;
            accesoopcionUsuario = accesoopcionUsuario.FindAll(x => x.opcion.pantalla.pantallaIndex == int.Parse(hidindexpantalla));
            List<PerfilesOpciones> accesoopcion = _sistemamodulo.perfilesOpciones;
            accesoopcion = accesoopcion.FindAll(x => x.opcion.pantalla.pantallaIndex == int.Parse(hidindexpantalla));
            LblSinAcessoOpciones.Text = "";
            if (accesoopcion.Count < 1 && accesoopcionUsuario.Count < 1 && !userName.ToLower().Equals(admin))
            {
                GlobalDataSingleton.Instance.controlAccesoOpc = "No tiene acceso por las siguientes razones: <br /> No tiene asignado permiso en Opciones.  <br /> Se han desactivado las Opciones.  <br /> No se han registrado las Opciones.";
                Response.Redirect(redirectMain);
            }
            else
            {
                LblSinAcessoOpciones.Text = "";
                GlobalDataSingleton.Instance.controlAccesoOpc = "";
                for (int pm = 0; pm < accesoopcionUsuario.Count; pm++)
                {
                    UsuariosOpciones popcion = accesoopcionUsuario[pm];

                    if (PageIndex.Equals("Sistemas") && !popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible, page);
                    }
                    else if (PageIndex.Equals("Actualizacion de Sistemas") && popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible, page);
                    }
                    else if (!PageIndex.Contains("Sistemas"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible, page);
                    }
                }

                for (int pm = 0; pm < accesoopcion.Count; pm++)
                {
                    PerfilesOpciones popcion = accesoopcion[pm];
                    if (PageIndex.Equals("Sistemas") && !popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible, page);
                    }
                    else if (PageIndex.Equals("Actualizacion de Sistemas") && popcion.opcion.idAsp.Contains("Sistema"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible, page);
                    }
                    else if (!PageIndex.Contains("Sistemas"))
                    {
                        activarComponenteOpcion(popcion.opcion.componenteIndex, popcion.opcion.idAsp, popcion.visible, page);
                    }
                }
                if (!userName.ToLower().Equals(admin))
                {
                    validarOpcionesRegistradas(idComponentePrincipal,page,LblSinAcessoOpciones,hidindexpantalla);
                }
            }
            System.GC.Collect();
        }

        public void activarComponenteOpcion(String componenteIndex, String idAsp, String visible, Page page)
        {
            if (componenteIndex.Trim().Length > 0)
            {
                GridView grid = (page.FindControl(idAsp) as GridView);
                if (grid != null)
                {
                    (grid.Columns[int.Parse(componenteIndex)]).Visible = bool.Parse(visible);
                }
            }
            else
            {
                Control c = page.FindControl(idAsp);
                if (c != null)
                {
                    c.Visible = bool.Parse(visible);
                }
            }
        }

        /// <summary>
        /// obtiene el acceso a modulos, sistemas, pantallas, opciones asignados al usuario, verifica tiempo de expiracion de la sesion 
        /// 
        /// </summary>
        public bool controlAccesoPantallasModulos(System.Web.HttpRequest Request, System.Web.HttpResponse Response, Page page, string redirectinicio, string userName, string admin,String idlink )
        {
            // Get the authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            bool estado = false;
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket =
               FormsAuthentication.Decrypt(authCookie.Value);

                double minTrancurridas = (authTicket.Expiration - GlobalDataSingleton.Instance.expirarTimepo).TotalMinutes;
                if (minTrancurridas < 5)
                {
                    GlobalDataSingleton.Instance.expirarTimepo = authTicket.Expiration;
                   
                  UsuarioLogin  uslogin = getUsuarioLogeado(userName, GlobalDataSingleton.Instance.sistemaID);
                    if (uslogin.Perfiles.Count > 0 || userName.ToLower().Equals(admin))
                    {
                       _sistemamodulo = uslogin.sistemasModulos;
                        List<SistemasModulos> acceso = _sistemamodulo.sistemasModulos;

                        if (acceso.Count > 0 || userName.ToLower().Equals(admin))
                        {
                            for (int pm = 0; pm < acceso.Count; pm++)
                            {
                                SistemasModulos pmodulo = acceso[pm];

                                if (pmodulo.idSistemaModulo > 0 && pmodulo.modulo.estado == 0)
                                {
                                    Control c = page.FindControl(pmodulo.modulo.h3Id);
                                    if (c != null)
                                    {
                                        c.Visible = bool.Parse(pmodulo.divvisible);
                                    }
                                    Control c2 =
                                    page.FindControl(pmodulo.modulo.divId);
                                    if (c2 != null)
                                    {
                                        c2.Visible = bool.Parse(pmodulo.divvisible);
                                    }
                                }
                                else
                                {
                                    Control c3 = page.FindControl(pmodulo.modulo.h3Id);
                                    if (c3 != null)
                                    {
                                        c3.Visible = false;
                                    }


                                    Control c4 = page.FindControl(pmodulo.modulo.divId);
                                    if (c4 != null)
                                    {
                                        c4.Visible = false;
                                    }
                                }
                            }
                            List<UsuariosModulos> accModulUsuario = _sistemamodulo.usuariosModulos;
                            for (int pm = 0; pm < accModulUsuario.Count; pm++)
                            {
                                UsuariosModulos pmodulo = accModulUsuario[pm];
                                int noasignado = SistemamoduloNoAsignado(pmodulo.idModulo);
                                if (pmodulo.modulo.estado == 0)
                                {
                                    Control c = page.FindControl(pmodulo.modulo.h3Id);
                                    if (c != null)
                                    {
                                        c.Visible = bool.Parse(pmodulo.divVisible);
                                    }
                                    Control c2 = page.FindControl(pmodulo.modulo.divId);
                                    if (c2 != null)
                                    {

                                        c2.Visible = bool.Parse(pmodulo.divVisible);
                                    }
                                }
                                if (pmodulo.modulo.estado > 0 || noasignado == 1)
                                {
                                    Control c = page.FindControl(pmodulo.modulo.h3Id);
                                    if (c != null)
                                    {
                                        c.Visible = false;
                                    }
                                    Control c2 = page.FindControl(pmodulo.modulo.divId);
                                    if (c2 != null)
                                    {
                                        c2.Visible = false;
                                    }
                                }
                            }

                            List<PerfilesModulos> accModuloPerfil = _sistemamodulo.perfilesModulos;
                            for (int pm = 0; pm < accModuloPerfil.Count; pm++)
                            {
                                PerfilesModulos pmodulo = accModuloPerfil[pm];
                                int noasignado = SistemamoduloNoAsignado(pmodulo.idModulo);
                                if (pmodulo.modulo.estado == 0)
                                {
                                    Control c = page.FindControl(pmodulo.modulo.h3Id);
                                    if (c != null)
                                    {
                                        c.Visible = bool.Parse(pmodulo.divVisible);
                                    }
                                    Control c2 = page.FindControl(pmodulo.modulo.divId);
                                    if (c2 != null)
                                    {
                                        c2.Visible = bool.Parse(pmodulo.divVisible);
                                    }
                                }
                                if (pmodulo.modulo.estado > 0 || noasignado == 1)
                                {
                                    Control c = page.FindControl(pmodulo.modulo.h3Id);
                                    if (c != null)
                                    {
                                        c.Visible = false;
                                    }
                                    Control c2 = page.FindControl(pmodulo.modulo.divId);
                                    if (c2 != null)
                                    {
                                        c2.Visible = false;
                                    }
                                }
                            }

                            List<UsuariosPantallas> accesoUsuariopantalla = _sistemamodulo.usuariosPantallas;
                            List<PerfilesPantallas> accesopantalla = _sistemamodulo.perfilesPantallas;
                            if (accesoUsuariopantalla.Count < 1 && accesopantalla.Count < 1 && !userName.ToLower().Equals(admin))
                            {

                                GlobalDataSingleton.Instance.controlAcceso = "El Usuario no tiene asociado ninguna pantalla o se han desactivado.";
                                Response.Redirect(redirectinicio);

                            }
                            else
                            {
                                for (int pm = 0; pm < accesoUsuariopantalla.Count; pm++)
                                {
                                    UsuariosPantallas ppantalla = accesoUsuariopantalla[pm];
                                    Control c = page.FindControl(ppantalla.pantalla.idAsp);
                                    if (c != null)
                                    {
                                        c.Visible = bool.Parse(ppantalla.visible);
                                    }
                                }

                                for (int pm = 0; pm < accesopantalla.Count; pm++)
                                {
                                    PerfilesPantallas ppantalla = accesopantalla[pm];
                                    Control c2 = page.FindControl(ppantalla.pantalla.idAsp);
                                    if (c2 != null)
                                    {
                                        c2.Visible = bool.Parse(ppantalla.visible.ToLower());
                                    }
                                }
                            }
                            estado = true;
                        }
                        else
                        {
                            GlobalDataSingleton.Instance.controlAcceso = "El Sistema no tiene asociado ningun Módulo.";
                            Response.Redirect(redirectinicio);
                        }
                    }
                    else
                    {
                        GlobalDataSingleton.Instance.controlAcceso = "El Usuario no tiene asociado ningun Perfil.";
                        Response.Redirect(redirectinicio);
                    }                    
                    
                }
                else
                {
                    //expirarSesion();
                }
            }
            else
            {
                GlobalDataSingleton.Instance.controlAcceso = "";
                Response.Redirect(redirectinicio);
            }
            return estado;
        
        }

        public  int SistemamoduloNoAsignado(int moduloid)
        {
            SistemasModulos smodulo = _SistemasModulosBL.getSistemaModulo(GlobalDataSingleton.Instance.sistemaID, moduloid);
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

        /// <summary>
        ///valida si estan registradas las opciones en la base de datos, si no estan registradas
        ///se desactivan 
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="page"></param>
        /// <param name="LblSinAcessoOpciones"></param>
        public void validarOpcionesRegistradas(String opcion, Page page, Label LblSinAcessoOpciones, String hidindexpantalla)
        {
            
            int totalOp = 0;
            int countOpcionReg = 0;

            if (opcion.Trim().Length > 0)
            {
                string[] splitOps = opcion.Split(new Char[] { ',' });

                for (int num = 0; num < splitOps.Length; num++)
                {
                    Control cd = page.FindControl(splitOps[num]);

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
                                            Opcion existe = _opcionBL.getOpcion(splitid[0], splitid[0] + splitid[ps + 1], 1, int.Parse(hidindexpantalla));
                                            if (existe.idOpcion < 1 || existe.idcheckbox.Trim().Length < 1 || existe.estado > 0)
                                            {
                                                activarComponenteOpcion(splitid[ps + 1], splitid[0], "false",page);
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
            if (totalOp == countOpcionReg && totalOp > 0)
            {
                LblSinAcessoOpciones.Text = "\n Se han desactivado las Opciones. \n No se han registrado las Opciones. ";
            }
        }
    }
}