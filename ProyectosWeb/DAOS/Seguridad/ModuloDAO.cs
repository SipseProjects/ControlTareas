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
   public class ModuloDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        public ModuloDAO(SqlConnection conn){
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult  registrarModulo(Modulo modulo)
        {
            DbQueryResult resultado = new DbQueryResult();          
           
            try
            {
                _conn.Open();              
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();              
                 
                 bool existe=  _consultas.existeEnDB("select * from modulos m where m.h3id='" + modulo.h3Id + "' and m.divid='" + modulo.divId + "'");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into modulos(nombre,descripcion,h3id,divid,estado) values(@parm1,@parm2,@parm3,@parm4,0)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
                       cmSql.Parameters["@parm1"].Value = modulo.Nombre.Trim();
                       cmSql.Parameters["@parm2"].Value = modulo.descripcion.Trim();
                       cmSql.Parameters["@parm3"].Value = modulo.h3Id.Trim();
                       cmSql.Parameters["@parm4"].Value = modulo.divId.Trim();
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
        public DbQueryResult UpdateModulo(Modulo modulo)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                
                cmSql.CommandText = " update modulos set nombre=@parm1, h3id=@parm2, divid=@parm3 where idmodulo=@parm4";
                    cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
                    cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                    cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                    cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = modulo.Nombre.Trim();
                    cmSql.Parameters["@parm2"].Value = modulo.h3Id.Trim();
                    cmSql.Parameters["@parm3"].Value = modulo.divId.Trim();
                    cmSql.Parameters["@parm4"].Value = modulo.idModulo;
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

        public DbQueryResult DeleteModulo(Modulo modulo, int activar)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = " update modulos  set estado=@parm7 where idmodulo=@parm4" +
                                    " update p  set p.estado=@parm7" +
                                     " from modulos as m"+
                                    " inner join pantallas as p"+
                                    " on p.idmodulo=m.idmodulo where p.idmodulo=@parm5" +
                                    " update op  set op.estado=@parm7" +
                                    " from modulos as m"+
                                    " inner join pantallas as p"+
                                    " on p.idmodulo=m.idmodulo "+
                                    " inner join opciones op"+
                                    " on op.idpantalla=p.idpantalla"+
                                    " where m.idmodulo=@parm6";               
                cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                cmSql.Parameters["@parm4"].Value = modulo.idModulo;
                cmSql.Parameters.Add("@parm5", SqlDbType.Int);
                cmSql.Parameters["@parm5"].Value = modulo.idModulo;
                cmSql.Parameters.Add("@parm6", SqlDbType.Int);
                cmSql.Parameters["@parm6"].Value = modulo.idModulo;
                cmSql.Parameters.Add("@parm7", SqlDbType.Int);
                cmSql.Parameters["@parm7"].Value = activar;
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

        public Modulo getModulo(String nombre, String h3Id, String divId, int idModulo) {
            Modulo p = new Modulo();            
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if (nombre != null && idModulo <1)
                {
                    cmSql.CommandText = "select * from modulos m where m.h3id='" + h3Id.Trim() + "' and m.divid='" + divId.Trim() + "' and m.nombre='" + nombre.Trim() + "'";
                }else
                    if (h3Id != null && nombre == null && idModulo < 1)
                {
                    cmSql.CommandText = "select * from modulos m where m.h3id='" + h3Id.Trim() + "' and m.divid='" + divId.Trim() + "'";
                }
                    else
                        if (h3Id == null && nombre == null && idModulo > 0 && divId == null)
                        {
                            cmSql.CommandText = "select * from modulos m where m.idmodulo=" + idModulo + "";
                        }
                
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                            DataRow drDatos = dtDatos.Rows[0];
                           
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            p.Nombre = drDatos["nombre"].ToString();
                            p.h3Id = drDatos["h3Id"].ToString();
                            p.divId = drDatos["divId"].ToString();
                            p.estado = (drDatos["estado"].ToString().Length > 0 ? int.Parse(drDatos["estado"].ToString()) : 0);                            
                    }
                }
            }catch(Exception e){
                
            }
            _conn.Close();
            return p;
        }

        public List<Modulo> getModulos()
        {
            List<Modulo> listado = new List<Modulo>();
            try
            {
                _conn.Open();
                
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = "select * from modulos p where p.Estado=0";

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
                            Modulo p = new Modulo();
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            p.Nombre = drDatos["nombre"].ToString();
                            p.h3Id = drDatos["h3Id"].ToString();
                            p.divId = drDatos["divId"].ToString();
                            listado.Add(p);
                        }
                    }
                }
            }catch(Exception e){
            
            }
            _conn.Close();
            return listado;
        }
    }
}
