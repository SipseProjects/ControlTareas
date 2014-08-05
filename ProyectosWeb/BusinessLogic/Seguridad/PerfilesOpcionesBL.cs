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
    public class PerfilesOpcionesBL
    {
        private PerfilesOpcionesDAO _perfilopciondao;
        public PerfilesOpcionesBL(SqlConnection con)
        {
            _perfilopciondao = new PerfilesOpcionesDAO(con);
        }
        public List<PerfilesOpciones> getPerfilesOpciones(int idPerfil, int idopcion, int pantallaIndex, int idModulo)
        {
            return _perfilopciondao.getPerfilesOpciones(idPerfil,idopcion, pantallaIndex,idModulo);
        }
        public DbQueryResult registrarPerfilesOpciones(PerfilesOpciones perfilesOpciones)
        {
            return _perfilopciondao.registrarPerfilesOpciones(perfilesOpciones);
        }
        public DbQueryResult UpdatePerfilesOpciones(PerfilesOpciones perfilesOpciones)
        {
            return _perfilopciondao.UpdatePerfilesOpciones(perfilesOpciones);
        }
        public PerfilesOpciones getPerfilOpcion(int idOpcion, int idPerfil)
        {
            return _perfilopciondao.getPerfilOpcion(idOpcion, idPerfil);
        }
    }
}
