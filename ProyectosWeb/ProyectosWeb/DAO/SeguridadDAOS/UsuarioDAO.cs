using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ProyectosWeb.Models;
using System.Data;

namespace ProyectosWeb.DAO.SeguridadDAOS
{
    public class UsuarioDAO
    {
        private SqlConnection _conn;// = new SqlConnection("Data Source=172.16.1.31;Initial Catalog=ProyectosGestion;Persist Security Info=True;User ID=sa;Password=Adminpwd20");

        public UsuarioDAO(SqlConnection conn)
        {
            _conn = conn;
        }

        public List<Usuario> getUsuarios(int idgrupo, int sinusuario) {
            _conn.Close();
            _conn.Open();
            List<Usuario> listado = new List<Usuario>();
            SqlCommand cmSql = _conn.CreateCommand();
            if (idgrupo > 0 && sinusuario == 0)
            {
                cmSql.CommandText = ""
                    + " select g.idusuario as 'l',p.nombre as nomUsuario, p.apellido from usuarios g "
                    + " inner join gruposusuarios gu"
                    + " on g.idusuario=gu.idusuario inner join personas p on p.idusuario=g.idusuario"
                    + " where g.Estado=0 and gu.idgrupo=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idgrupo;
            }
            else if (sinusuario > 0 && idgrupo > 0)
            {
                cmSql.CommandText = ""
                    + " select g.idusuario as 'l',p.nombre as nomUsuario, p.apellido from usuarios g "
                    + " left join gruposusuarios gu"
                    + " on g.idusuario=gu.idusuario and gu.idgrupo=@parm1 inner join personas p on p.idusuario=g.idusuario "
                    + " where  (gu.idgrupo is null and gu.idusuario is null) and g.Estado=0";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idgrupo;
            }
            else
            {
                cmSql.CommandText = "select u.IDUsuario as 'l',u.IDCatalogoProveedores,u.Nombre, u.esEmpleado,u.Contraseña,u.Estado,p.IDPersonas, p.IDUsuario,p.Nombre as nomUsuario,"
                    + " p.Apellido,p.FechaRegistro,p.Tecnologias,p.Estado, p.Email, p.Telefono from Usuarios u inner join  Personas  p"
                    + " on u.IDUsuario=p.IDUsuario where u.Estado=0";
            }
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int g = 0; g < ds.Tables[0].Rows.Count; g++)
                    {
                        DataRow drDatos = dtDatos.Rows[g];
                        Usuario us = new Usuario();
                        Persona persona = new Persona();
                        us.idUsuario = int.Parse(drDatos["l"].ToString());
                        persona.nombre = drDatos["nomUsuario"].ToString();
                        persona.apellido = drDatos["apellido"].ToString();
                        us.persona = persona;
                        listado.Add(us);
                    }
                }

            }
            _conn.Close();
            return listado;
        }
        public int UpdateUsuarioPassword(int idusuario, string oldUsuario, string newUsuario, String pass) {
            _conn.Open();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = "update usuarios set nombre=@parm1,contraseña=@parm3 where  nombre=@parm2";
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
            cmSql.Parameters["@parm2"].Value = oldUsuario;
            cmSql.Parameters["@parm1"].Value = newUsuario;
            cmSql.Parameters["@parm3"].Value = pass;
           int exito= cmSql.ExecuteNonQuery();
            _conn.Close();
            return exito;
        }

        public int UpdatePasswordRestore(int idusuario, String pass)
        {
            _conn.Open();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = "update usuarios set contraseña=@parm2 where  idusuario=@parm1";          
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
            cmSql.Parameters["@parm1"].Value = idusuario;
            cmSql.Parameters["@parm2"].Value = pass;
            int exito = cmSql.ExecuteNonQuery();
            _conn.Close();
            return exito;
        }

        public Usuario getUserByEmail(String email) {
            Usuario resultado = new Usuario();
            _conn.Open();
          
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = "select u.IDUsuario as 'l',u.Nombre, u.esEmpleado,u.Estado,p.IDPersonas, p.IDUsuario,p.Nombre as nomUsuario,"
                    + " p.Apellido,p.FechaRegistro,p.Tecnologias,p.Estado, p.Email, p.Telefono from Usuarios u inner join  Personas  p"
                    + " on u.IDUsuario=p.IDUsuario where u.Estado=0 and p.email=@parm1";
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
            cmSql.Parameters["@parm1"].Value = email;
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drDatos = dtDatos.Rows[0];
                    resultado = new Usuario();
                    resultado.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                    resultado.nombre = drDatos["nombre"].ToString();

                }

            }
            _conn.Close();
            return resultado;
        }

        public Usuario getUsuario(int idusuario)
        {
            _conn.Open();
            Usuario us = new Usuario();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = "Select * from usuarios where idusuario=@parm1";
            cmSql.Parameters.Add("@parm1", SqlDbType.Int);
            cmSql.Parameters["@parm1"].Value = idusuario;
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drDatos = dtDatos.Rows[0];
                    us = new Usuario();
                    us.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                    us.nombre = drDatos["nombre"].ToString();
                    us.tiempoExpiracion = Convert.ToDateTime(drDatos["tiempoExpiracion"].ToString());
                    us.linkCliked = (drDatos["linkClicked"].ToString().Length>0) ? int.Parse(drDatos["linkClicked"].ToString()):0;
                }

            }
            _conn.Close();
            return us;
        }

        public int setLinkCliked(int idusuario, int numClicks) {
            _conn.Open();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = " UPDATE u"
                       + " SET   u.linkclicked=@parm2"
                       + " FROM Usuarios AS u"
                       + " INNER JOIN Personas AS P "
                       + "        ON u.IDUsuario = P.IDUsuario "
                       + " WHERE u.IDUsuario = @parm1";
            cmSql.Parameters.Add("@parm1", SqlDbType.Int);
            cmSql.Parameters.Add("@parm2", SqlDbType.Int);
            cmSql.Parameters["@parm1"].Value = idusuario;
            cmSql.Parameters["@parm2"].Value = numClicks;

            int exito = cmSql.ExecuteNonQuery();
            _conn.Close();
            return exito;
        }

        public int setTiempoExpiracion(int idusuario)
        {
            _conn.Open();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = " UPDATE u"
                       + " SET   u.tiempoexpiracion=getdate()"
                       + " FROM Usuarios AS u"
                       + " INNER JOIN Personas AS P "
                       + "        ON u.IDUsuario = P.IDUsuario "
                       + " WHERE u.idusuario = @parm1";
            cmSql.Parameters.Add("@parm1", SqlDbType.Int);
            cmSql.Parameters["@parm1"].Value = idusuario;
            int exito = cmSql.ExecuteNonQuery();
            _conn.Close();
            return exito;
        }
    }
}