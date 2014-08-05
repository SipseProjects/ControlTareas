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
    public class UsuariosOpcionesBL
    {
        private UsuariosOpcionesDAO _usuarioopciondao;
        public UsuariosOpcionesBL(SqlConnection con)
        {
            _usuarioopciondao = new UsuariosOpcionesDAO(con);
        }
        public List<UsuariosOpciones> getUsuariosOpciones(int idUsuario, int idOpcion,int pantallaIndex, int idModulo)
        {
            return _usuarioopciondao.getUsuariosOpciones(idUsuario, idOpcion, pantallaIndex, idModulo);
        }
        public DbQueryResult registrarUsuariosOpciones(UsuariosOpciones usuariosOpciones)
        {
            return _usuarioopciondao.registrarUsuariosOpciones(usuariosOpciones);
        }
        public DbQueryResult UpdateUsuariosOpciones(UsuariosOpciones usuariosOpciones)
        {
            return _usuarioopciondao.UpdateUsuariosOpciones(usuariosOpciones);
        }
        public UsuariosOpciones getUsuarioOpcion(int idOpcion, int idUsuario)
        {
            return _usuarioopciondao.getUsuarioOpcion(idOpcion, idUsuario);
        }
    }
}
