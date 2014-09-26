using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using ProyectosWeb.DAO.SeguridadDAOS;
using ProyectosWeb.Models;
using System.Data.SqlClient;

namespace ProyectosWeb.BusinessLogic.Seguridad
{
    public class UsuarioFacade
    {
        private UsuarioDAO _usuarioDao;

        public UsuarioFacade(SqlConnection conn){
        _usuarioDao = new UsuarioDAO(conn);
        }
        public Usuario getUsuario(int idusuario)
        {
           return _usuarioDao.getUsuario(idusuario);
        }

        public Usuario getUsuarioLogeado(string username)
        {
            return _usuarioDao.getUsuarioLogeado(username);
        }

        public void llenarListaUsuario(ListBox ListBoxUsuariosSeg)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idUsuario", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;

            List<Usuario> usuarios = _usuarioDao.getUsuarios(-1,-1);
            for (int i = 0; i < usuarios.Count; i++)
            {
                table2.Rows.Add(usuarios[i].idUsuario, usuarios[i].persona.nombre+" "+usuarios[i].persona.apellido);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idUsuario";
            ListBoxUsuariosSeg.DataBind();
        }

        public void DropDownBinUsuarios(DropDownList lista)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idusuario", typeof(int));
            table2.Columns.Add("nombre", typeof(string));
            table2.Rows.Add("0", "Seleccione un Usuario");
            List<Usuario> usuario = _usuarioDao.getUsuarios(-1,-1);
            for (int i = 0; i < usuario.Count; i++)
            {
                table2.Rows.Add(usuario[i].idUsuario, usuario[i].persona.nombre+" "+usuario[i].persona.apellido);
            }            
            lista.DataSource = table2;
            lista.DataValueField = "idusuario";
            lista.DataTextField = "nombre";
            lista.DataBind();
        }

        public int UpdateUsuarioPassword(int idusuario, string oldUsuario, string newUsuario, String pass)
        {
            return _usuarioDao.UpdateUsuarioPassword(idusuario, oldUsuario, newUsuario, pass);
        }

        public int UpdatePasswordRestore(int idusuario, String pass)
        {
            return _usuarioDao.UpdatePasswordRestore(idusuario,pass);
        }

        public void llenarListaUsuariosAsignados(ListBox ListBoxUsuariosSeg, int idgrupo)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idusuario", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;

            ListBoxUsuariosSeg.SelectionMode = ListSelectionMode.Multiple;
            List<Usuario> usuario = _usuarioDao.getUsuarios(idgrupo, 0);
            for (int i = 0; i < usuario.Count; i++)
            {
                table2.Rows.Add(usuario[i].idUsuario, usuario[i].persona.nombre+" "+usuario[i].persona.apellido);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idusuario";
            ListBoxUsuariosSeg.DataBind();
        }
        public void llenarListaUsuariosNoAsignados(ListBox ListBoxUsuariosSeg, int idgrupo)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idusuario", typeof(int));
            table2.Columns.Add("nombre", typeof(string));

            ListBoxUsuariosSeg.Height = 200;
            ListBoxUsuariosSeg.Width = 200;

            ListBoxUsuariosSeg.SelectionMode = ListSelectionMode.Multiple;
            List<Usuario> usuario = _usuarioDao.getUsuarios(idgrupo, 1);
            for (int i = 0; i < usuario.Count; i++)
            {
                table2.Rows.Add(usuario[i].idUsuario, usuario[i].persona.nombre + " " + usuario[i].persona.apellido);
            }
            ListBoxUsuariosSeg.DataSource = table2;
            ListBoxUsuariosSeg.DataTextField = "nombre";
            ListBoxUsuariosSeg.DataValueField = "idusuario";
            ListBoxUsuariosSeg.DataBind();
        }
        public Usuario getUserByEmail(String email) {
            return _usuarioDao.getUserByEmail(email);
        }
        public int setTiempoExpiracion(int idusuario)
        {
            return _usuarioDao.setTiempoExpiracion(idusuario);
        }
        public int setLinkCliked(int idusuario, int numClicks)
        {
            return _usuarioDao.setLinkCliked(idusuario, numClicks);
        }
        public void DropDownBinUsuariosCR(DropDownList lista)
        {
            DataTable table2 = new DataTable();
            table2.Columns.Add("idusuario", typeof(int));
            table2.Columns.Add("nombre", typeof(string));
            table2.Rows.Add("0", "Seleccione un Usuario");
            List<Usuario> usuario = _usuarioDao.getUsuarios(-1, -1);
            for (int i = 0; i < usuario.Count; i++)
            {
                table2.Rows.Add(usuario[i].idUsuario, usuario[i].persona.nombre + " " + usuario[i].persona.apellido);
            }
            lista.DataSource = table2;
            lista.DataValueField = "idusuario";
            lista.DataTextField = "nombre";
            lista.DataBind();
        }
    }

}