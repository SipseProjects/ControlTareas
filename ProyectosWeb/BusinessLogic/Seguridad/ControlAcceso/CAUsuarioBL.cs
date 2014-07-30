using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAOS.Seguridad;
using DAOS.Seguridad.ControlAccesoUsuario;
using Models;
using System.Data.SqlClient;

namespace ProyectosWeb.BusinessLogic.Seguridad
{
    public class CAUsuarioBL
    {
        private CAUsuarioDAO _usuarioDao;

        public CAUsuarioBL(SqlConnection conn)
        {
            _usuarioDao = new CAUsuarioDAO(conn);
        }

        //public Usuario getUsuarioLogeado(string username)
        //{
        //    return _usuarioDao.getUsuarioLogeado(username);
        //}

    }

}