using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAOS.Seguridad;
using Models.Seguridad;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic.Seguridad
{
    public class UsuariosModulosBL
    {
        private UsuariosModulosDAO _umodulodao;
        public UsuariosModulosBL(SqlConnection con)
        {
            _umodulodao = new UsuariosModulosDAO(con);
        }
        public List<UsuariosModulos> getUsuariosModulos(int idUsuario, int idmodulo)
        {
            return _umodulodao.getUsuariosModulos(idUsuario, idmodulo);
        }
        public DbQueryResult registrarUsuariosModulos(UsuariosModulos usuariomodulo)
        {
            return _umodulodao.registrarUsuariosModulos(usuariomodulo);
        }
        public DbQueryResult UpdateUsuariosModulos(UsuariosModulos usuariomodulo)
        {
            return _umodulodao.UpdateUsuariosModulos(usuariomodulo);
        }
        public UsuariosModulos getUsuarioModulo(int idusuario, int idmodulo)
        {
            return _umodulodao.getUsuarioModulo(idusuario, idmodulo);
        }
    }
}
