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
   public class UsuariosPantallasDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        private DbQueryResult _status;
        public UsuariosPantallasDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarUsuariosPantallas(UsuariosPantallas usuariospantallas)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();           
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from usuariospantallas o where o.idpantalla=" + usuariospantallas.idPantalla + " and o.idusuario=" + usuariospantallas.idUsuario + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into usuariospantallas(idpantalla,idusuario,visible) values(@parm1,@parm2,@parm3)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = usuariospantallas.idPantalla;
                       cmSql.Parameters["@parm2"].Value = usuariospantallas.idUsuario;
                       cmSql.Parameters["@parm3"].Value = usuariospantallas.visible.Trim();
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
        public DbQueryResult UpdateUsuariosPantallas(UsuariosPantallas usuariospantallas)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update usuariospantallas set idusuario=@parm1, idpantalla=@parm2, visible=@parm3  where idusuariopantalla=@parm4";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = usuariospantallas.idUsuario;
                cmSql.Parameters["@parm2"].Value = usuariospantallas.idPantalla;
                cmSql.Parameters["@parm3"].Value = usuariospantallas.visible.Trim();
                cmSql.Parameters["@parm4"].Value = usuariospantallas.idUsuarioPantalla;
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

        public UsuariosPantallas getUsuarioPantalla(int idUsuario, int idPantalla)
        {
            UsuariosPantallas p = new UsuariosPantallas();            
           
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from usuariospantallas o where o.idusuario=@parm1 and o.idpantalla=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idUsuario;
                cmSql.Parameters["@parm2"].Value = idPantalla;


                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                            DataRow drDatos = dtDatos.Rows[0];
                            p.idUsuarioPantalla = int.Parse(drDatos["idusuariopantalla"].ToString());
                            p.idUsuario = int.Parse(drDatos["idusuario"].ToString()); 
                            p.idPantalla = int.Parse(drDatos["idpantalla"].ToString());                            
                            p.visible = drDatos["visible"].ToString();
                            p.componenteIndex =drDatos["componenteIndex"].ToString();                           
                    }
                }
            }catch(Exception e){
                
            }
            _conn.Close();
            return p;
        }
        public List<UsuariosPantallas> getUsuariosPantallas(int idUsuario, int idPantalla, int idModulo)
        {
            List<UsuariosPantallas> listado = new List<UsuariosPantallas>();
             _status=new DbQueryResult();
             _status.Success = false;
           
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if (idUsuario > 0 && idPantalla < 1 && idModulo < 1)
                {
                    cmSql.CommandText =
                    "select pp.idusuariopantalla,pp.idpantalla,pp.idusuario,pp.visible,pp.componenteindex,p.idmodulo," +
                    " p.nombre,p.descripcion,p.idasp, p.estado" +
                     " from usuariospantallas pp" +
                    " inner join pantallas p" +
                    " on pp.idpantalla=p.idpantalla where p.estado=0 and pp.idusuario=@parm1";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
                }
                else if (idPantalla > 0 && idUsuario > 0 && idModulo < 1)
                {
                    cmSql.CommandText =
                      "select pp.idusuariopantalla,pp.idpantalla,pp.idusuario,pp.visible,pp.componenteindex,p.idmodulo," +
                      " p.nombre,p.descripcion,p.idasp, p.estado" +
                       " from usuariospantallas pp" +
                      " inner join pantallas p" +
                      " on pp.idpantalla=p.idpantalla where p.estado=0 and pp.idusuario=@parm1 and pp.idpantalla=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
                    cmSql.Parameters["@parm2"].Value = idPantalla;
                }
                else if (idPantalla < 1 && idUsuario > 0 && idModulo > 0)
                {
                    cmSql.CommandText =
                      "select pp.idusuariopantalla,pp.idpantalla,pp.idusuario,pp.visible,pp.componenteindex,p.idmodulo," +
                      " p.nombre,p.descripcion,p.idasp, p.estado" +
                       " from usuariospantallas pp" +
                      " inner join pantallas p" +
                      " on pp.idpantalla=p.idpantalla where p.estado=0 and pp.idusuario=@parm1 and p.idmodulo=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
                    cmSql.Parameters["@parm2"].Value = idModulo;
                }
                else if (idPantalla < 1 && idUsuario < 1 && idModulo > 0)
                {
                    cmSql.CommandText =
                      "select pp.idusuariopantalla,pp.idpantalla,pp.idusuario,pp.visible,pp.componenteindex,p.idmodulo," +
                      " p.nombre,p.descripcion,p.idasp, p.estado" +
                       " from usuariospantallas pp" +
                      " inner join pantallas p" +
                      " on pp.idpantalla=p.idpantalla where p.estado=0 and p.idmodulo=@parm2";
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm2"].Value = idModulo;
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
                            UsuariosPantallas u = new UsuariosPantallas();
                            u.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            u.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                            u.idUsuarioPantalla = int.Parse(drDatos["idusuariopantalla"].ToString());
                            u.visible = drDatos["visible"].ToString();
                            u.componenteIndex = drDatos["componenteIndex"].ToString();
                            Pantalla pantalla = new Pantalla();
                            pantalla.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            pantalla.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            pantalla.idAsp = drDatos["idasp"].ToString();
                            pantalla.nombre = drDatos["nombre"].ToString();
                            pantalla.descripcion = drDatos["descripcion"].ToString();
                            u.pantalla = pantalla;
                            listado.Add(u);
                            _status.Success = true;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                _status.ErrorMessage = e.Message;
            }
            _conn.Close();
            return listado;
        }       
    }
}
