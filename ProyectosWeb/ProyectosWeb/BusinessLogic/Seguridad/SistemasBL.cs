using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ProyectosWeb.DAO.SeguridadDAOS;
using ProyectosWeb.Models;
using ProyectosWeb.Models.Seguridad;
using System.Web.UI.WebControls;

namespace ProyectosWeb.BusinessLogic.Seguridad
{
    public class SistemasBL
    {
        private SistemasDAO _sistemasDao;
        public SistemasBL(SqlConnection con)
        {
            _sistemasDao = new SistemasDAO(con);
        }

        public DbQueryResult UpdateSistemas(Sistema sis)
        {
         return   _sistemasDao.UpdateSistemas(sis);
        }

        public DbQueryResult DeleteSistema(int idSistema)
        {
            return _sistemasDao.DeleteSistema(idSistema);
        }

        public List<Sistema> getSistemas()
        {
            return _sistemasDao.getSistemas();
        }

        public void DropDownBindSistemas(DropDownList lista)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idsistemas", typeof(string));
            table2.Columns.Add("nombre", typeof(string));
            List<Sistema> sis = _sistemasDao.getSistemas();
            table2.Rows.Add("", "Seleccione un Sistema");

            for (int i = 0; i < sis.Count; i++)
            {
                table2.Rows.Add(sis[i].idSistema, sis[i].nombre);
            }
            lista.DataSource = table2;
            lista.DataValueField = "idsistemas";
            lista.DataTextField = "nombre";
            lista.DataBind();
        }

        public Sistema getSistema(int idSistema)
        {
            return _sistemasDao.getSistema(idSistema);
        }

    }
}