using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
namespace ProyectosWeb.BusinessLogic.general
{
    public class ControlarConexion
    {
        private SqlConnection _conn;
        public ControlarConexion(SqlConnection conn)
        {
            _conn=conn;
        }

        public void abrirConexion()
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
        }

        public void cerrarConexion()
        {
            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }
        }
    }
}