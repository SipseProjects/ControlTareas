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
    public class ModuloBL
    {
        private ModuloDAO _modulodao;
        public ModuloBL(SqlConnection con)
        {
            _modulodao = new ModuloDAO(con);
        }
        public List<Modulo> getModulos()
        {
           return _modulodao.getModulos();
        }
        public DbQueryResult registrarModulo(Modulo modulo)
        {
            return _modulodao.registrarModulo(modulo);
        }
        public DbQueryResult UpdateModulo(Modulo modulo)
        {
            return _modulodao.UpdateModulo(modulo);
        }
        public DbQueryResult DeleteModulo(Modulo modulo, int activar)
        {
            return _modulodao.DeleteModulo(modulo, activar);
        }
        public Modulo getModulo(String nombre, String h3Id, String divId, int idModulo)
        {
            return _modulodao.getModulo(nombre, h3Id, divId,idModulo);
        }
    }
}
