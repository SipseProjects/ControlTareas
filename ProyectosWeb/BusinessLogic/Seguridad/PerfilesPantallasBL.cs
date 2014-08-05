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
    public class PerfilesPantallasBL
    {
        private PerfilesPantallasDAO _ppantallasdao;
        public PerfilesPantallasBL(SqlConnection con)
        {
            _ppantallasdao = new PerfilesPantallasDAO(con);
        }
        public List<PerfilesPantallas> getPerfilesPantallas(int idperfil, int idpantalla, int idModulo)
        {
            return _ppantallasdao.getPerfilesPantallas(idperfil,idpantalla, idModulo);
        }
        public DbQueryResult registrarPerfilesPantallas(PerfilesPantallas ppantalla)
        {
            return _ppantallasdao.registrarPerfilesPantallas(ppantalla);
        }
        public DbQueryResult UpdatePerfilesPantallas(PerfilesPantallas ppantalla)
        {
            return _ppantallasdao.UpdatePerfilesPantallas(ppantalla);
        }
        public PerfilesPantallas getPerfilPantalla(int idperfil,int idpantalla)
        {
            return _ppantallasdao.getPerfilPantalla(idperfil, idpantalla);
        }
    }
}
