using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Seguridad;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAOS.Seguridad
{
   public class UsuariosModulosDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        private DbQueryResult _status;
        public UsuariosModulosDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarUsuariosModulos(UsuariosModulos usuariomodulo)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();           
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from usuariosmodulos o where o.idmodulo=" + usuariomodulo.idModulo + " and o.idusuario=" + usuariomodulo.idUsuario + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into usuariosmodulos(idmodulo,idusuario,h3visible,divvisible) values(@parm1,@parm2,@parm3,@parm4)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = usuariomodulo.idModulo;
                       cmSql.Parameters["@parm2"].Value = usuariomodulo.idUsuario;
                       cmSql.Parameters["@parm3"].Value = usuariomodulo.h3Visible.Trim();
                       cmSql.Parameters["@parm4"].Value = usuariomodulo.divVisible.Trim();
                       int exito = cmSql.ExecuteNonQuery();
                       if (exito > 0)
                       {
                           resultado.Success = true;
                       }
                   }
                   else
                   {
                       resultado.ErrorMessage = "existe"; 
                   }
                
            }
            catch (Exception ex) {
                resultado.ErrorMessage = ex.Message;
            }
            _conn.Close();                          
            return resultado;
        }
        public DbQueryResult UpdateUsuariosModulos(UsuariosModulos usuariomodulo)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update usuariosmodulos set idmodulo=@parm1, idusuario=@parm2, h3visible=@parm3, divvisible=@parm4  where idusuariomodulo=@parm5";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm5", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = usuariomodulo.idModulo;
                cmSql.Parameters["@parm2"].Value = usuariomodulo.idUsuario;
                cmSql.Parameters["@parm3"].Value = usuariomodulo.h3Visible.Trim();
                cmSql.Parameters["@parm4"].Value = usuariomodulo.divVisible.Trim();
                cmSql.Parameters["@parm5"].Value = usuariomodulo.idUsuarioModulo;
                    int exito = cmSql.ExecuteNonQuery();
                    if (exito > 0)
                    {
                        resultado.Success = true;
                    }                
            }
            catch (Exception ex)
            {
                resultado.ErrorMessage = ex.Message;
            }
            _conn.Close();
            return resultado;
        }

        public UsuariosModulos getUsuarioModulo(int idusuario, int idmodulo)
        {
            UsuariosModulos p = new UsuariosModulos();            
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from usuariosmodulos o where o.idusuario=@parm1 and o.idmodulo=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = idusuario;
                cmSql.Parameters["@parm2"].Value = idmodulo;
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                            DataRow drDatos = dtDatos.Rows[0];
                            p.idUsuarioModulo = int.Parse(drDatos["idusuariomodulo"].ToString());
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());                            
                            p.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                            p.divVisible = drDatos["divvisible"].ToString();
                            p.h3Visible =drDatos["h3visible"].ToString();                           
                    }
                }
            }catch(Exception e){
                
            }
            _conn.Close();
            return p;
        }

        public List<UsuariosModulos> getUsuariosModulos(int IdUsuario, int idmodulo)
        {
            List<UsuariosModulos> listado = new List<UsuariosModulos>();
            _status=new DbQueryResult();
            _status.Success=false;
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if (IdUsuario > 0 && idmodulo < 1)
                {
                cmSql.CommandText = "select pm.idmodulo, pm.idusuariomodulo, pm.idusuario, pm.divvisible, m.idmodulo,m.nombre, m.h3id, m.divid from usuariosmodulos pm"
                +" inner join modulos m"
                +" on m.idmodulo=pm.idmodulo and pm.idusuario in("+IdUsuario+") ";
                }else if(idmodulo>0&&IdUsuario>0){
                    cmSql.CommandText = "select pm.idmodulo, pm.idusuariomodulo, pm.idusuario, pm.divvisible, m.idmodulo,m.nombre, m.h3id, m.divid from usuariosmodulos pm"
                    + " inner join modulos m"
                    + " on m.idmodulo=pm.idmodulo and pm.idusuario in(" + IdUsuario + ") and pm.idmodulo="+idmodulo+"";
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
                            UsuariosModulos pmodulo = new UsuariosModulos();
                            pmodulo.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            pmodulo.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                            pmodulo.divVisible = drDatos["divvisible"].ToString();
                            Modulo mod = new Modulo();
                            mod.Nombre = drDatos["nombre"].ToString();
                            mod.h3Id = drDatos["h3Id"].ToString();
                            mod.divId = drDatos["divId"].ToString();
                            pmodulo.modulo = mod;
                            listado.Add(pmodulo);
                            _status.Success = true;
                        }
                    }
                }
            }
            catch(Exception e) {
                _status.ErrorMessage = e.Message;
            }
            _conn.Close();
            return listado;
        }
    }
}
