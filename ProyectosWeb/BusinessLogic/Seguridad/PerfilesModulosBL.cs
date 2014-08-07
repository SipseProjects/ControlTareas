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
    public class PerfilesModulosBL
    {
        private PerfilesModulosDAO _pmodulodao;
        public PerfilesModulosBL(SqlConnection con)
        {
            _pmodulodao = new PerfilesModulosDAO(con);
        }
        public List<PerfilesModulos> getPerfilesModulos(int idPerfil, int idmodulo)
        {
            return _pmodulodao.getPerfilesModulos(idPerfil, idmodulo);
        }
        public DbQueryResult registrarPerfilesModulos(PerfilesModulos pmodulo)
        {
            return _pmodulodao.registrarPerfilesModulos(pmodulo);
        }
        public DbQueryResult UpdatePerfilesModulos(PerfilesModulos pmodulo)
        {
            return _pmodulodao.UpdatePerfilesModulos(pmodulo);
        }
        public PerfilesModulos getPerfilModulo(int idperfil, int idmodulo)
        {
            return _pmodulodao.getPerfilModulo(idperfil, idmodulo);
        }
    }
}
