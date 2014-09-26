using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ProyectosWeb.Models;
using System.Data;

namespace ProyectosWeb.DAO.SeguridadDAOS
{
    public class PerfilDAO
    {
        private SqlConnection _conn;

        public PerfilDAO(SqlConnection conn)
        {
        _conn=conn;
        }

        public int agregarUsuarioPerfil(int idusuario, int idperfil)
        {
            int exito = 0;
            try
            {
                _conn.Open();

                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = ""
                                    + "select * from perfilesUsuarios g where g.idusuario=@parm1 and g.idperfil=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idusuario;
                cmSql.Parameters["@parm2"].Value = idperfil;
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {

                        cmSql.CommandText = "insert into perfilesUsuarios(idusuario,idperfil) values(@parm3,@parm4)";
                        cmSql.Parameters.Add("@parm3", SqlDbType.Int);
                        cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                        cmSql.Parameters["@parm3"].Value = idusuario;
                        cmSql.Parameters["@parm4"].Value = idperfil;
                        exito = cmSql.ExecuteNonQuery();

                    }
                }
            }catch(Exception c){
            
            }
            _conn.Close();

            return exito;
        }
        public List<Perfil> getPerfiles(int idusuario,int sinperfil)
        {
            List<Perfil> listado = new List<Perfil>();
            try
            {
                _conn.Open();

                SqlCommand cmSql = _conn.CreateCommand();

                if (idusuario > 0 && sinperfil == 0)
                {
                    cmSql.CommandText = ""
                        + " select g.idperfil,g.nombre from perfiles g  "
                        + " inner join perfilesusuarios gu"
                        + " on g.idperfil=gu.idperfil where g.Estado=0"
                        + " and gu.idusuario=@parm1";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idusuario;
                }
                else if (sinperfil > 0 && idusuario > 0)
                {
                    cmSql.CommandText = ""
                        + " select g.idperfil,g.nombre from perfiles g "
                        + " left join perfilesusuarios gu"
                        + " on g.idperfil=gu.idperfil and gu.idusuario=@parm1 "
                        + " where  (gu.idperfil is null and gu.idusuario is null) and g.Estado=0";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idusuario;
                }
                else
                {

                    cmSql.CommandText = "select * from perfiles p where p.Estado=0";
                }
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int g1 = 0; g1 < ds.Tables[0].Rows.Count; g1++)
                        {
                            DataRow drDatos = dtDatos.Rows[g1];
                            Perfil p = new Perfil();
                            p.idPerfil = int.Parse(drDatos["idperfil"].ToString());
                            p.nombre = drDatos["nombre"].ToString();
                            listado.Add(p);
                        }
                    }

                }
            }catch(Exception c){
            
            }
            _conn.Close();
            return listado;
        }
        public int eliminarUsuarioPerfil(int idusuario, int idperfil)
        {
            
            int exito = 0;
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "delete from perfilesUsuarios where idusuario=@parm1 and idperfil=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idusuario;
                cmSql.Parameters["@parm2"].Value = idperfil;
                exito = cmSql.ExecuteNonQuery();
            }catch(Exception ex){
            
            }
            _conn.Close();
            return exito;
        }
        public Perfil getPerfil(int idPerfil)
        {
            Perfil perfil = new Perfil();
            try
            {
                _conn.Open();

                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "Select * from perfiles where idpefil=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idPerfil;
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
                            perfil = new Perfil();
                            perfil.idGrupo = int.Parse(drDatos["idperfil"].ToString());
                            perfil.nombre = drDatos["nombre"].ToString();
                        }
                    }

                }
            }catch(Exception s){
            
            }
            _conn.Close();
            return perfil;
        }
    }
}