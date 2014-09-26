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
    public class OpcionBL
    {
        private OpcionDAO _opciondao;
        public OpcionBL(SqlConnection con)
        {
            _opciondao = new OpcionDAO(con);
        }
        public List<Pantalla> getPantallas()
        {
            return _opciondao.getPantallas();
        }
        public DbQueryResult registrarOpcion(Opcion opcion)
        {
            return _opciondao.registrarOpcion(opcion);
        }
        public DbQueryResult UpdateOpcion(Opcion opcion)
        {
            return _opciondao.UpdateOpcion(opcion);
        }
        public DbQueryResult DeleteOpcion(int idOpcion, int activar)
        {
            return _opciondao.DeleteOpcion(idOpcion, activar);
        }
        public Opcion getOpcion(String idasp, String descripcion, int c, int pantallaindex)
        {
            return _opciondao.getOpcion(idasp, descripcion, c, pantallaindex);
        }
    }
}
