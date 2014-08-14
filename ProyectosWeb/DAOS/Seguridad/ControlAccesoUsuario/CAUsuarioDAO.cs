using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Models;
using Models.General;
using Models.Seguridad;
using System.Data;
using Models.Seguridad.ControlAcceso;

namespace DAOS.Seguridad.ControlAccesoUsuario
{
    public class CAUsuarioDAO
    {
        private SqlConnection _conn;
        private ControlarConexion _controlarConexion;

        public CAUsuarioDAO(SqlConnection conn)
        {
            _conn = conn;
            _controlarConexion = new ControlarConexion(_conn);
        }
        public UsuarioLogin getUsuarioLogeado(string username)
        {
            _controlarConexion.abrirConexion();
            UsuarioLogin us = new UsuarioLogin();
            PersonaLogin p = new PersonaLogin();
            PerfilLogin per = new PerfilLogin();
            GrupoLogin grupo = new GrupoLogin();

            List<PerfilLogin> PerfilesUsu = new List<PerfilLogin>();
            List<GrupoLogin> GruposUsu = new List<GrupoLogin>();

            try
            {
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
                    int filas = ds.Tables[0].Rows.Count;
                    if (filas > 0)
                    {
                        DataRow drDatos = dtDatos.Rows[0];
                        us = new UsuarioLogin();
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
                                if ((drrelaciones["idperfilrel"].ToString().Length > 0))
                                {
                                    per = new PerfilLogin();
                                    per.idPerfil = int.Parse(drrelaciones["idperfilrel"].ToString());
                                    per.nombre = drrelaciones["nomPerfil"].ToString();
                                    per.descripcion = drrelaciones["perfilDesc"].ToString();
                                    per.usuarioAlta = drrelaciones["usuarioalta"].ToString();
                                    per.usuarioBaja = drrelaciones["usuariobaja"].ToString();
                                    per.usuarioModifica = drrelaciones["usuariomodifica"].ToString();
                                    us.Perfiles.Add(per);
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

                                if (drrelaciones["idgrupo"].ToString().Length > 0)
                                {
                                    grupo = new GrupoLogin();
                                    grupo.idGrupo = int.Parse(drrelaciones["idgrupo"].ToString());
                                    grupo.nombre = drrelaciones["nomgrupo"].ToString();
                                    grupo.descripcion = drrelaciones["grupodesc"].ToString();
                                    us.Grupos.Add(grupo);
                                }
                            }
                        }
                    }
                }
            }catch(Exception e){
            
            }
            _controlarConexion.cerrarConexion();
            return us;
        }
    }
}