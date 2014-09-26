using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ProyectosWeb.Models;
using ProyectosWeb.Models.Seguridad;
using System.Data;

namespace ProyectosWeb.DAO.SeguridadDAOS
{
    public class UsuarioDAO
    {
        private SqlConnection _conn;

        public UsuarioDAO(SqlConnection conn)
        {
            _conn = conn;
        }

        public List<Usuario> getUsuarios(int idgrupo, int sinusuario) {
            abrirConexion();
            List<Usuario> listado = new List<Usuario>();
            try
            {
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
            }catch(Exception e){
            
            }
            cerrarConexion();
            return listado;
        }
        public int UpdateUsuarioPassword(int idusuario, string oldUsuario, string newUsuario, String pass) {
            abrirConexion();
            int exito = 0;
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "update usuarios set nombre=@parm1,contraseña=@parm3 where  nombre=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters["@parm2"].Value = oldUsuario;
                cmSql.Parameters["@parm1"].Value = newUsuario;
                cmSql.Parameters["@parm3"].Value = pass;
                exito = cmSql.ExecuteNonQuery();
            }catch(Exception s){
            
            }
           cerrarConexion();
            return exito;
        }

        public void abrirConexion() {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
        }
         
        public void cerrarConexion(){
            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }
        }

        public int UpdatePasswordRestore(int idusuario, String pass)
        {
            abrirConexion();
            int exito = 0;
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "update usuarios set contraseña=@parm2 where  idusuario=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters["@parm1"].Value = idusuario;
                cmSql.Parameters["@parm2"].Value = pass;
                 exito = cmSql.ExecuteNonQuery();
            }catch(Exception c){
            
            }
            cerrarConexion();
            return exito;
        }

        public Usuario getUserByEmail(String email) {
            Usuario resultado = new Usuario();
            abrirConexion();
            try
            {
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
                        Persona p = new Persona();
                    }

                }
            }catch(Exception c){
            
            }
            cerrarConexion();
            return resultado;
        }
        public Usuario getUsuarioLogeado(string username)
        {
            abrirConexion();
            Usuario us = new Usuario();
            Persona p = new Persona();
            Perfil per = new Perfil();
            Grupo grupo = new Grupo();
            PerfilesUsuarios perfilUs = new PerfilesUsuarios();
            List<Perfil> PerfilesUsu = new List<Perfil>();
            List<Grupo> GruposUsu = new List<Grupo>();

            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = "select u.idusuario, u.nombre as nomUser,u.esempleado, p.idpersonas, p.nombre," 
                   + " p.apellido, p.email, per.idperfil as idperfilrel, perfil.nombre as nomPerfil,"
                   + " perfil.descripcion as perfilDesc, perfil.usuarioalta, perfil.usuariobaja, perfil.usuariomodifica,"
                   + " gus.idgrupo, grupo.nombre as nomGrupo, grupo.descripcion as grupoDesc"
                   + " from usuarios u"
                   + " inner join personas p on u.idusuario=p.idusuario"
                   + " left  join  perfilesusuarios per"
                   + " on per.idusuario=u.idusuario"
                   + " left  join  perfiles perfil"
                   + " on per.idperfil=perfil.idperfil"
                   + " left  join  gruposusuarios gus"
                   + " on gus.idusuario=u.idusuario"
                   + " left  join  grupos grupo"
                   + " on grupo.idgrupos=gus.idgrupo"
                   + " where u.nombre=@parm1 and u.estado=0";
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
            cmSql.Parameters["@parm1"].Value = username;
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                int filas=ds.Tables[0].Rows.Count;
                if (filas > 0)
                {
                    DataRow drDatos = dtDatos.Rows[0];
                    us = new Usuario();
                    us.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                    us.nombre = drDatos["nomUser"].ToString();
                    p.idPersona = int.Parse(drDatos["idpersonas"].ToString());
                    p.nombre = drDatos["nombre"].ToString();
                    p.apellido = drDatos["apellido"].ToString();
                    p.email = drDatos["email"].ToString();
                    us.persona = p;

                    cmSql.CommandText = "select distinct per.idperfil as idperfilrel, perfil.nombre as nomPerfil,"
                   + " perfil.descripcion as perfilDesc, perfil.usuarioalta, perfil.usuariobaja, perfil.usuariomodifica"
                   + " "
                   + " from usuarios u"
                   + " inner join personas p on u.idusuario=p.idusuario"
                   + " left  join  perfilesusuarios per"
                   + " on per.idusuario=u.idusuario"
                   + " left  join  perfiles perfil"
                   + " on per.idperfil=perfil.idperfil"
                   + " where u.nombre=@parm2 and u.estado=0";
                    cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                    cmSql.Parameters["@parm2"].Value = username;
                    SqlDataAdapter daperfil = new SqlDataAdapter(cmSql);
                    DataSet dsperfil = new DataSet();
                    daperfil.Fill(dsperfil);
                    if (dsperfil.Tables.Count > 0)
                    {
                        DataTable dtDatosp = dsperfil.Tables[0];
                        int filasp = dsperfil.Tables[0].Rows.Count;
                        for (int perf = 0; perf < filasp; perf++)
                        {
                            DataRow drrelaciones = dtDatosp.Rows[perf];
                            if ((drDatos["idperfilrel"].ToString().Length > 0))
                            {
                            per = new Perfil();
                            per.idPerfil =  int.Parse(drDatos["idperfilrel"].ToString());
                            per.nombre = drrelaciones["nomPerfil"].ToString();
                            per.descripcion = drrelaciones["perfilDesc"].ToString();
                            per.usuarioAlta = drrelaciones["usuarioalta"].ToString();
                            per.usuarioBaja = drrelaciones["usuariobaja"].ToString();
                            per.usuarioModifica = drrelaciones["usuariomodifica"].ToString();
                            us.PerfilesUsu.Add(per);
                            }
                        }
                    }
                    cmSql.CommandText = "select distinct gus.idgrupo as diferente,"
                   + " gus.idgrupo, grupo.nombre as nomGrupo, grupo.descripcion as grupoDesc"
                   + " from usuarios u"
                   + " inner join personas p on u.idusuario=p.idusuario"
                   + " left  join  gruposusuarios gus"
                   + " on gus.idusuario=u.idusuario"
                   + " left  join  grupos grupo"
                   + " on grupo.idgrupos=gus.idgrupo"
                   + " where u.nombre=@parm3 and u.estado=0";
                    cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                    cmSql.Parameters["@parm3"].Value = username;
                    SqlDataAdapter dagrupo = new SqlDataAdapter(cmSql);
                    DataSet dsgrupo = new DataSet();
                    dagrupo.Fill(dsgrupo);

                    if (dsgrupo.Tables.Count > 0)
                    {
                        DataTable dtDatosg = dsgrupo.Tables[0];
                        int filasg = dsgrupo.Tables[0].Rows.Count;

                            for (int perf = 0; perf < filasg; perf++)
                            {
                                DataRow drrelaciones = dtDatos.Rows[perf];
                                
                                if(drDatos["idgrupo"].ToString().Length>0){
                                grupo = new Grupo();
                                grupo.idGrupo =  int.Parse(drDatos["idgrupo"].ToString());
                                grupo.nombre = drrelaciones["nomgrupo"].ToString();
                                grupo.descripcion = drrelaciones["grupodesc"].ToString();
                                us.GruposUsu.Add(grupo);
                                }
                            }
                      }
                }

            }

            cerrarConexion();
            return us;
        }
        public Usuario getUsuario(int idusuario)
        {
            abrirConexion();
            Usuario us = new Usuario();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = "Select * from usuarios where idusuario=@parm1";
            cmSql.Parameters.Add("@parm1", SqlDbType.Int);
            cmSql.Parameters["@parm1"].Value = idusuario;
            try
            {
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
                        us.linkCliked = (drDatos["linkClicked"].ToString().Length > 0) ? int.Parse(drDatos["linkClicked"].ToString()) : 0;
                    }

                }
            }
            catch{
            
            }
            cerrarConexion();
            return us;
        }

        public int setLinkCliked(int idusuario, int numClicks) {
            abrirConexion();
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
            cerrarConexion();
            return exito;
        }

        public int setTiempoExpiracion(int idusuario)
        {
            abrirConexion();
            SqlCommand cmSql = _conn.CreateCommand();
            cmSql.CommandText = " UPDATE u"
                       + " SET   u.tiempoexpiracion=getdate(), u.linkclicked=0"
                       + " FROM Usuarios AS u"
                       + " INNER JOIN Personas AS P "
                       + "        ON u.IDUsuario = P.IDUsuario "
                       + " WHERE u.idusuario = @parm1";
            cmSql.Parameters.Add("@parm1", SqlDbType.Int);
            cmSql.Parameters["@parm1"].Value = idusuario;
            int exito = cmSql.ExecuteNonQuery();
            cerrarConexion();
            return exito;
        }
        
    }
}