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
    public class PerfilBL
    {

        private PerfilDAO _perfilDao;

        public PerfilBL(SqlConnection conn)
        {
            _perfilDao = new PerfilDAO(conn);
        }
        public int agregarUsuarioPerfil(int idusuario, int idperfil) {
            return _perfilDao.agregarUsuarioPerfil(idusuario,idperfil);
        }
        public int eliminarUsuarioPerfil(int idusuario, int idperfil)
        {
            return _perfilDao.eliminarUsuarioPerfil(idusuario, idperfil);
        }
        public void DropDownBinPerfiles(DropDownList lista)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idperfil", typeof(int));
            table2.Columns.Add("nombre", typeof(string));
            List<Perfil> perfil = _perfilDao.getPerfiles(-1, -1);
            table2.Rows.Add(0, "Seleccione un Perfil");
            for (int i = 0; i < perfil.Count; i++)
            {
                table2.Rows.Add(perfil[i].idPerfil, perfil[i].nombre);
            }
            lista.DataSource = table2;
            lista.DataValueField = "idperfil";
            lista.DataTextField = "nombre";
            lista.DataBind();
        }
        public void llenarListaPerfilesNoAsignados(ListBox ListBoxUsuariosSeg, int idusuario)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idperfil", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;

            ListBoxUsuariosSeg.SelectionMode = ListSelectionMode.Multiple;
            List<Perfil> perfiles = _perfilDao.getPerfiles(idusuario,1);
            for (int i = 0; i < perfiles.Count; i++)
            {
                table2.Rows.Add(perfiles[i].idPerfil, perfiles[i].nombre);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idperfil";
            ListBoxUsuariosSeg.DataBind();
        }
        public void llenarListaPerfilesAsignados(ListBox ListBoxUsuariosSeg, int idusuario)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idperfil", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;

            ListBoxUsuariosSeg.SelectionMode = ListSelectionMode.Multiple;
            List<Perfil> perfiles = _perfilDao.getPerfiles(idusuario,0);
            for (int i = 0; i < perfiles.Count; i++)
            {
                table2.Rows.Add(perfiles[i].idPerfil, perfiles[i].nombre);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idperfil";
            ListBoxUsuariosSeg.DataBind();
        }
    }
}