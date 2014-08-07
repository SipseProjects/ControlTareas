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
    public class SistemasModulosBL
    {
        private SistemasModulosDAO _sistemamodulodao;
        public SistemasModulosBL(SqlConnection con)
        {
            _sistemamodulodao = new SistemasModulosDAO(con);
        }
        public List<SistemasModulos> getSistemasModulos(int idSistema, int idmodulo)
        {
            return _sistemamodulodao.getSistemasModulos(idSistema, idmodulo);
        }
        public DbQueryResult registrarSistemasModulos(SistemasModulos sistemamodulo)
        {
            return _sistemamodulodao.registrarSistemasModulos(sistemamodulo);
        }
        public DbQueryResult UpdateSistemasModulos(SistemasModulos sistemamodulo)
        {
            return _sistemamodulodao.UpdateSistemasModulos(sistemamodulo);
        }
        public SistemasModulos getSistemaModulo(int idsistema, int idmodulo)
        {
            return _sistemamodulodao.getSistemaModulo(idsistema, idmodulo);
        }
    }
}
