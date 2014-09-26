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
   public class OpcionDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        public OpcionDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult  registrarOpcion(Opcion opcion)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();              
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from opciones o where o.idasp='" + opcion.idAsp + "' and o.idcheckbox='" + opcion.idcheckbox + "'");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into opciones(idpantalla,nombre,descripcion,idasp,componenteindex,idcheckbox) values(@parm1,@parm2,@parm3,@parm4,@parm5,@parm6)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm5", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm6", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = opcion.idPantalla;
                       cmSql.Parameters["@parm2"].Value = opcion.nombre.Trim();
                       cmSql.Parameters["@parm3"].Value = opcion.descripcion.Trim();
                       cmSql.Parameters["@parm4"].Value = opcion.idAsp.Trim();
                       cmSql.Parameters["@parm5"].Value = opcion.componenteIndex.Trim();
                       cmSql.Parameters["@parm6"].Value = opcion.idcheckbox.Trim();
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
        public DbQueryResult UpdateOpcion(Opcion opcion)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update opciones set idpantalla=@parm1, nombre=@parm2, descripcion=@parm3, idasp=@parm4, componenteindex=@parm5, idcheckbox=@parm6  where idopcion=@parm7";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm5", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm6", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm7", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = opcion.idPantalla;
                cmSql.Parameters["@parm2"].Value = opcion.nombre.Trim();
                cmSql.Parameters["@parm3"].Value = opcion.descripcion.Trim();
                cmSql.Parameters["@parm4"].Value = opcion.idAsp.Trim();
                cmSql.Parameters["@parm5"].Value = opcion.componenteIndex;
                cmSql.Parameters["@parm6"].Value = opcion.idcheckbox;
                cmSql.Parameters["@parm7"].Value = opcion.idOpcion;
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

        public Opcion getOpcion(String idAsp, String idcheckbox, int d, int pantallaindex)
        {
            Opcion p = new Opcion();            
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if(d==0){
                cmSql.CommandText = "select * from opciones o where o.idasp='" + idAsp.Trim() + "' and o.idcheckbox='" + idcheckbox + "'";                                
                }else{
                    cmSql.CommandText = "select * from opciones o inner join pantallas p on"+
                    " p.idpantalla=o.idpantalla and p.pantallaindex="+pantallaindex+""+
                        " where o.idasp='" + idAsp.Trim() + "' and o.idcheckbox like '%" + idcheckbox + "%'";                                               
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
                            p.idOpcion = int.Parse(drDatos["idopcion"].ToString());
                            p.idPantalla = int.Parse(drDatos["idpantalla"].ToString());                            
                            p.nombre = drDatos["nombre"].ToString();
                            p.idAsp = drDatos["idasp"].ToString();
                            p.idcheckbox = drDatos["idcheckbox"].ToString();
                            p.componenteIndex = drDatos["componenteIndex"].ToString(); 
                            p.estado = (drDatos["estado"].ToString().Length > 0 ? int.Parse(drDatos["estado"].ToString()) : 0);
                          
                    }
                }
            }catch(Exception e){
                
            }
            _conn.Close();
            return p;
        }
        public DbQueryResult DeleteOpcion(int idOpcion, int activar)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = " update opciones   set estado=@parm6 where idopcion=@parm4";
                cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                cmSql.Parameters["@parm4"].Value = idOpcion;
                cmSql.Parameters.Add("@parm6", SqlDbType.Int);
                cmSql.Parameters["@parm6"].Value = activar;
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
        public List<Pantalla> getPantallas()
        {
            List<Pantalla> listado = new List<Pantalla>();
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from pantallas p where p.Estado=0";
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
                            Pantalla p = new Pantalla();
                            p.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            p.nombre = drDatos["nombre"].ToString();
                            p.idAsp = drDatos["idasp"].ToString();
                            listado.Add(p);
                        }
                    }
                }
            }
            catch { 
            
            }
            _conn.Close();
            return listado;
        }
    }
}
