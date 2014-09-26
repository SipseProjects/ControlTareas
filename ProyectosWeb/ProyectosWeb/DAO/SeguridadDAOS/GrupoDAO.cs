using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ProyectosWeb.Models;
using System.Data;

namespace ProyectosWeb.DAO.SeguridadDAOS
{
    public class GrupoDAO
    {
        private SqlConnection _conn;
        public GrupoDAO(SqlConnection conn)
        {
        _conn=conn;
        }

        public int agregarUsuarioGrupo(int idusuario, int idgrupo) {
            int exito = 0;
            try
            {
                _conn.Open();
                
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = ""
                                    + "select * from gruposUsuarios g where g.idusuario=@parm1 and g.idgrupo=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idusuario;
                cmSql.Parameters["@parm2"].Value = idgrupo;
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {

                        cmSql.CommandText = "insert into gruposUsuarios(idusuario,idgrupo) values(@parm3,@parm4)";
                        cmSql.Parameters.Add("@parm3", SqlDbType.Int);
                        cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                        cmSql.Parameters["@parm3"].Value = idusuario;
                        cmSql.Parameters["@parm4"].Value = idgrupo;
                        exito = cmSql.ExecuteNonQuery();

                    }
                }
            }catch(Exception c){
            
            }
            _conn.Close();            
                return exito;
        }
        public int eliminarUsuarioGrupo(int idusuario, int idgrupo)
        {
            int exito = 0;
            try
            {
                _conn.Open();

                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = ""
                                    + "delete from gruposUsuarios where idusuario=@parm1 and idgrupo=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idusuario;
                cmSql.Parameters["@parm2"].Value = idgrupo;
                exito = cmSql.ExecuteNonQuery();
            }catch(Exception v){
            
            }
            _conn.Close();
            return exito;
        }
        public List<Grupo> getGrupos(int idusuario, int sinGrupo)
        {
            
            List<Grupo> listado = new List<Grupo>();
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();

                if (idusuario > 0 && sinGrupo == 0)
                {
                    cmSql.CommandText = ""
                        + " select g.idgrupos,g.nombre from grupos g "
                        + " inner join gruposusuarios gu"
                        + " on g.idgrupos=gu.idgrupo"
                        + " where g.Estado=0 and gu.idusuario=@parm1";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idusuario;
                }
                else if (sinGrupo > 0 && idusuario > 0)
                {
                    cmSql.CommandText = ""
                        + " select g.idgrupos,g.nombre from grupos g "
                        + " left join gruposusuarios gu"
                        + " on g.idgrupos=gu.idgrupo and gu.idusuario=@parm1 "
                        + " where  (gu.idgrupo is null and gu.idusuario is null) and g.Estado=0";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idusuario;
                }
                else
                {
                    cmSql.CommandText = "select * from grupos g where g.Estado=0";
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
                            Grupo g = new Grupo();
                            g.idGrupo = int.Parse(drDatos["idgrupos"].ToString());
                            g.nombre = drDatos["nombre"].ToString();
                            listado.Add(g);
                        }
                    }

                }
            }catch(Exception c){
            
            }
            _conn.Close();
            return listado;
        }
        public Grupo getGrupo(int idGrupo)
        {
            Grupo grupo = new Grupo();
            try
            {
                _conn.Open();

                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "Select * from grupos where idgrupos=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idGrupo;
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDatos = dtDatos.Rows[0];
                        grupo = new Grupo();
                        grupo.idGrupo = int.Parse(drDatos["idgrupos"].ToString());
                        grupo.nombre = drDatos["nombre"].ToString();
                    }

                }
            }catch(Exception c){
            
            }
            _conn.Close();
            return grupo;
        }
    }
}