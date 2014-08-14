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
   public class UsuariosOpcionesDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        private DbQueryResult _status = new DbQueryResult();
        public UsuariosOpcionesDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarUsuariosOpciones(UsuariosOpciones usuariosOpciones)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();           
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from usuariosopciones o where o.idOpcion=" + usuariosOpciones.idOpcion + " and o.idusuario=" + usuariosOpciones.idUsuario + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into usuariosopciones(idopcion,idusuario,visible) values(@parm1,@parm2,@parm3)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = usuariosOpciones.idOpcion;
                       cmSql.Parameters["@parm2"].Value = usuariosOpciones.idUsuario;
                       cmSql.Parameters["@parm3"].Value = usuariosOpciones.visible.Trim();
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
        public DbQueryResult UpdateUsuariosOpciones(UsuariosOpciones usuariosOpciones)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update usuariosopciones set idusuario=@parm1, idopcion=@parm2, visible=@parm3  where idusuarioopcion=@parm4";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = usuariosOpciones.idUsuario;
                cmSql.Parameters["@parm2"].Value = usuariosOpciones.idOpcion;
                cmSql.Parameters["@parm3"].Value = usuariosOpciones.visible.Trim();
                cmSql.Parameters["@parm4"].Value = usuariosOpciones.idUsuarioOpcion;
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

        public UsuariosOpciones getUsuarioOpcion(int idOpcion, int idUsuario)
        {
            UsuariosOpciones p = new UsuariosOpciones();  
            _status=new DbQueryResult();
            _status.Success=false;
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from usuariosopciones o where o.idusuario=@parm1 and o.idopcion=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idUsuario;
                cmSql.Parameters["@parm2"].Value = idOpcion;

                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                            DataRow drDatos = dtDatos.Rows[0];
                            p.idUsuarioOpcion = int.Parse(drDatos["idUsuarioOpcion"].ToString());
                            p.idUsuario = int.Parse(drDatos["idusuario"].ToString()); 
                            p.idOpcion= int.Parse(drDatos["idOpcion"].ToString());                            
                            p.visible = drDatos["visible"].ToString();
                        _status.Success=true;   
                    }
                }
            }catch(Exception e){
                _status.ErrorMessage=e.Message;
            }
            _conn.Close();
            return p;
        }

        public List<UsuariosOpciones> getUsuariosOpciones(int idUsuario, int idOpcion, int pantallaIndex, int idModulo)
        {
            List<UsuariosOpciones> listado = new List<UsuariosOpciones>();
            _status = new DbQueryResult();
            _status.Success = false;
            
            String consulta = "select po.idusuarioopcion,po.idopcion,po.idusuario,po.visible," +
                    " o.idpantalla, o.nombre,o.descripcion,o.idasp,o.componenteindex," +
                    " o.chkboxtreeindex, o.estado,o.idcheckbox," +
                     " p.idmodulo, p.nombre as nomPantalla, p.descripcion as descPantalla, " +
                         " p.idasp as idaspPantalla, p.estado as estadoPantalla, p.pantallaindex" +
                        " from usuariosopciones po" +
                        " inner join opciones o" +
                        " on o.idopcion=po.idopcion" +
                        " inner join pantallas p on" +
                        " p.idpantalla=o.idpantalla" +
                        " where o.estado=0 ";
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();

                if (idUsuario > 0 && idOpcion < 1 && pantallaIndex < 1 && idModulo < 1)
                {
                    cmSql.CommandText = consulta + " and po.idusuario=@parm1";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
                }
                else if (idOpcion > 0 && idUsuario > 0 && pantallaIndex < 1 && idModulo < 1)
                {
                    cmSql.CommandText =
                      consulta+" and po.idusuario=@parm1 and po.idopcion=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
                    cmSql.Parameters["@parm2"].Value = idOpcion;
                }
                else if (idUsuario > 0 && idOpcion < 1 && pantallaIndex > 0 && idModulo < 1)
                {
                    cmSql.CommandText = consulta + " and po.idusuario=@parm1 and p.pantallaIndex=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm2"].Value = pantallaIndex;
                }
                else if (idUsuario > 0 && idOpcion < 1 && pantallaIndex < 1 && idModulo > 0)
                {
                    cmSql.CommandText = consulta + " and po.idusuario=@parm1 and p.idmodulo=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idUsuario;
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
                            UsuariosOpciones p = new UsuariosOpciones();
                            p.idUsuarioOpcion = int.Parse(drDatos["idUsuarioOpcion"].ToString());
                            p.idUsuario = int.Parse(drDatos["idusuario"].ToString());
                            p.idOpcion = int.Parse(drDatos["idOpcion"].ToString());
                            p.visible = drDatos["visible"].ToString();
                            Opcion opcion = new Opcion();
                            opcion.idOpcion = int.Parse(drDatos["idOpcion"].ToString());
                            opcion.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            opcion.nombre = drDatos["nombre"].ToString();
                            opcion.idAsp = drDatos["idasp"].ToString();
                            opcion.descripcion = drDatos["descripcion"].ToString();
                            opcion.componenteIndex = drDatos["componenteIndex"].ToString();
                            opcion.chkboxTreeindex = (drDatos["chkboxTreeindex"].ToString().Length > 0 ? int.Parse(drDatos["chkboxTreeindex"].ToString()) : 0);
                            opcion.idcheckbox = drDatos["idcheckbox"].ToString();
                            Pantalla panta = new Pantalla();
                            panta.idPantalla = opcion.idPantalla;
                            panta.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            panta.idAsp = drDatos["idasppantalla"].ToString();
                            panta.nombre = drDatos["nompantalla"].ToString();
                            panta.descripcion = drDatos["descpantalla"].ToString();
                            panta.estado = int.Parse(drDatos["estadopantalla"].ToString());
                            panta.pantallaIndex = (drDatos["pantallaIndex"].ToString().Length > 0 ? int.Parse(drDatos["pantallaIndex"].ToString()) : 0);
                            opcion.pantalla = panta;
                            p.opcion = opcion;
                            listado.Add(p);

                            _status.Success = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _status.ErrorMessage = e.Message;
            }
            _conn.Close();
            return listado;
        }        
    }
}
