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
    public class UsuariosPantallasBL
    {
        private UsuariosPantallasDAO _upantallasdao;
        public UsuariosPantallasBL(SqlConnection con)
        {
            _upantallasdao = new UsuariosPantallasDAO(con);
        }
        public List<UsuariosPantallas> getUsuariosPantallas(int idUsuario,int idPantalla, int idModulo)
        {
            return _upantallasdao.getUsuariosPantallas(idUsuario, idPantalla, idModulo);
        }
        public DbQueryResult registrarUsuariosPantallas(UsuariosPantallas usuariopantalla)
        {
            return _upantallasdao.registrarUsuariosPantallas(usuariopantalla);
        }
        public DbQueryResult UpdateUsuariosPantallas(UsuariosPantallas usuariopantalla)
        {
            return _upantallasdao.UpdateUsuariosPantallas(usuariopantalla);
        }
        public UsuariosPantallas getUsuarioPantalla(int idusuario, int idpantalla)
        {
            return _upantallasdao.getUsuarioPantalla(idusuario, idpantalla);
        }
    }
}
