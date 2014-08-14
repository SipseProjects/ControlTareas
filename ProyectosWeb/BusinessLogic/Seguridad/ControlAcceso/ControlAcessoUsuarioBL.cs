using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAOS.Seguridad;
using DAOS.Seguridad.ControlAccesoUsuario;
using Models.Seguridad;
using Models.Seguridad.ControlAcceso;
using BusinessLogic.Seguridad;
using System.Data.SqlClient;

namespace ProyectosWeb.BusinessLogic.Seguridad
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
        }

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
        public SistemasModulos getSistemasModulosRelaciones(int idSistema, int idUsuario, List<PerfilLogin> perfiles)
        {
            
            SistemasModulos sistemasModulos= new SistemasModulos();
            List<PerfilesModulos> perfilesModulos = new List<PerfilesModulos>();
            List<PerfilesPantallas> perfilesPantallas = new List<PerfilesPantallas>();
            List<PerfilesOpciones> perfilesOpciones = new List<PerfilesOpciones>();
            List<UsuariosPantallas> usuariosPantallas = new List<UsuariosPantallas>();
            List<UsuariosOpciones> usuariosOpciones = new List<UsuariosOpciones>();

            sistemasModulos.sistemasModulos = _SistemasModulosBL.getSistemasModulos(idSistema, 0);
            for (int s = 0; s < sistemasModulos.sistemasModulos.Count;s++)
            {
                perfilesPantallas.AddRange(getPerfilesPantallasUsuario(perfiles, 0, sistemasModulos.sistemasModulos[s].idModulo));                 
                usuariosPantallas.AddRange(getUsuariosPantallas(idUsuario, 0, sistemasModulos.sistemasModulos[s].idModulo));
                perfilesOpciones.AddRange(getPerfilesOpcionesUsuario(perfiles, 0, 0, sistemasModulos.sistemasModulos[s].idModulo));
                usuariosOpciones.AddRange(getUsuariosOpciones(idUsuario, 0, 0, sistemasModulos.sistemasModulos[s].idModulo));
            }
            sistemasModulos.perfilesModulos = perfilesModulos;
            sistemasModulos.perfilesPantallas = perfilesPantallas;
            sistemasModulos.usuariosPantallas = usuariosPantallas;
            sistemasModulos.perfilesOpciones = perfilesOpciones;
            sistemasModulos.usuariosOpciones = usuariosOpciones;

            return sistemasModulos;
        }
    }
}