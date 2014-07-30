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
            _conn.Open();
            try
            {                              
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
            _conn.Open();
            try
            {
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
            _conn.Open();
            try
            {
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
        
        public List<UsuariosOpciones> getUsuariosOpciones(int idUsuario)
        {
            List<UsuariosOpciones> listado = new List<UsuariosOpciones>();
            _status = new DbQueryResult();
            _status.Success = false;
            _conn.Open();
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();

                if(idUsuario>0){
                 cmSql.CommandText = "select * from usuariosopciones p where p.Estado=0 and p.idusuario=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idUsuario;
                }else{                                
                cmSql.CommandText = "select * from usuariosOpciones p where p.Estado=0";
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
                            p.idOpcion= int.Parse(drDatos["idOpcion"].ToString());                            
                            p.visible = drDatos["visible"].ToString();
                            listado.Add(p);
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
