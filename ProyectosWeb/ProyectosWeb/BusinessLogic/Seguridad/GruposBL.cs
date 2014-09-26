using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using ProyectosWeb.DAO.SeguridadDAOS;
using ProyectosWeb.Models;
using System.Data.SqlClient;
using System.Data;

namespace ProyectosWeb.BusinessLogic.Seguridad
{
    public class GruposBL
    {
        private GrupoDAO _grupoDao;

        public GruposBL(SqlConnection conn)
        {
            _grupoDao = new GrupoDAO(conn);
        }

        public void llenarListaGrupos(ListBox ListBoxUsuariosSeg, int idusuario)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idgrupo", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;
            if (idusuario!=0)
            {
            ListBoxUsuariosSeg.SelectionMode = ListSelectionMode.Multiple;
            }
                List<Grupo> grupo = _grupoDao.getGrupos(idusuario, 1);
            for (int i = 0; i < grupo.Count; i++)
            {
                table2.Rows.Add(grupo[i].idGrupo, grupo[i].nombre);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idgrupo";
            ListBoxUsuariosSeg.DataBind();
        }
        public void DropDownBindGrupos(DropDownList lista)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idgrupo", typeof(int));
            table2.Columns.Add("nombre", typeof(string));
            table2.Rows.Add("0", "Seleccione un Grupo");
            List<Grupo> grupo = _grupoDao.getGrupos(0, 1);
            for (int i = 0; i < grupo.Count; i++)
            {
                table2.Rows.Add(grupo[i].idGrupo, grupo[i].nombre);
            }
            lista.DataSource = table2;
            lista.DataValueField = "idgrupo";
            lista.DataTextField = "nombre";
            lista.DataBind();
        }

        public int agregarUsuarioGrupo(int idusuario, int idgrupo)
        {
           return _grupoDao.agregarUsuarioGrupo(idusuario,idgrupo);
        }
        public int eliminarUsuarioGrupo(int idusuario, int idgrupo)
        {
         return   _grupoDao.eliminarUsuarioGrupo(idusuario, idgrupo);
        }
        public void llenarListaGruposAsignados(ListBox ListBoxUsuariosSeg, int idusuario)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idgrupo", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;

            ListBoxUsuariosSeg.SelectionMode = ListSelectionMode.Multiple;
            List<Grupo> grupo = _grupoDao.getGrupos(idusuario,0);
            for (int i = 0; i < grupo.Count; i++)
            {
                table2.Rows.Add(grupo[i].idGrupo, grupo[i].nombre);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idgrupo";
            ListBoxUsuariosSeg.DataBind();
        }

        public List<Grupo> tb(int idusuario)
        {
         List<Grupo> grupo = _grupoDao.getGrupos(idusuario,0);
         return grupo; 
        }
    }
}