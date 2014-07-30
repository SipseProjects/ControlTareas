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
   public class PerfilesOpcionesDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        private DbQueryResult _status = new DbQueryResult();
        public PerfilesOpcionesDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarPerfilesOpciones(PerfilesOpciones perfilesOpciones)
        {
            DbQueryResult resultado = new DbQueryResult();          
            _conn.Open();
            try
            {                              
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from perfilesopciones o where o.idOpcion=" + perfilesOpciones.idOpcion + " and o.idperfil=" + perfilesOpciones.idPerfil + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into perfilesopciones(idopcion,idperfil,visible) values(@parm1,@parm2,@parm3)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = perfilesOpciones.idOpcion;
                       cmSql.Parameters["@parm2"].Value = perfilesOpciones.idPerfil;
                       cmSql.Parameters["@parm3"].Value = perfilesOpciones.visible.Trim();
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
        public DbQueryResult UpdatePerfilesOpciones(PerfilesOpciones perfilesOpciones)
        {
            DbQueryResult resultado = new DbQueryResult();
            _conn.Open();
            try
            {
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update perfilesopciones set idperfil=@parm1, idopcion=@parm2, visible=@parm3  where idperfilopcion=@parm4";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = perfilesOpciones.idPerfil;
                cmSql.Parameters["@parm2"].Value = perfilesOpciones.idOpcion;
                cmSql.Parameters["@parm3"].Value = perfilesOpciones.visible.Trim();
                cmSql.Parameters["@parm4"].Value = perfilesOpciones.idPerfilOpcion;
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

        public PerfilesOpciones getPerfilOpcion(int idOpcion, int idPerfil)
        {
            PerfilesOpciones p = new PerfilesOpciones();  
            _status=new DbQueryResult();
            _status.Success=false;
            _conn.Open();
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from perfilesopciones o where o.idperfil=@parm1 and o.idopcion=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idPerfil;
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
                            p.idPerfilOpcion = int.Parse(drDatos["idPerfilOpcion"].ToString());
                            p.idPerfil = int.Parse(drDatos["idperfil"].ToString()); 
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
        
        public List<PerfilesOpciones> getPerfilesOpciones(int idPerfil, int idOpcion)
        {
            List<PerfilesOpciones> listado = new List<PerfilesOpciones>();
            _status = new DbQueryResult();
            _status.Success = false;
            _conn.Open();
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();

                if (idPerfil > 0 && idOpcion < 1)
                {
                    cmSql.CommandText =
                   "select po.idperfilopcion,po.idopcion,po.idperfil,po.visible,o.idpantalla" +
                        " p.nombre.p.descripcion,p.idasp,p.componenteindex,p.chkboxtreeindex," +
                        " p.estado,p.idcheckbox from perfilesopciones po" +
                        " inner join opciones o" +
                        " on o.idopcion=po.idopcion where o.estado=0 and po.idperfil=@parm1";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idPerfil;
                }
                else if (idOpcion > 0 && idPerfil > 0)
                {
                    cmSql.CommandText =
                      "select po.idperfilopcion,po.idopcion,po.idperfil,po.visible,o.idpantalla"+
                        " p.nombre.p.descripcion,p.idasp,p.componenteindex,p.chkboxtreeindex,"+
                        " p.estado,p.idcheckbox from perfilesopciones po"+
                        " inner join opciones o"+
                        " on o.idopcion=po.idopcion where o.estado=0 and po.idperfil=@parm1 and po.idopcion=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idPerfil;
                    cmSql.Parameters["@parm2"].Value = idOpcion;
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
                            PerfilesOpciones p = new PerfilesOpciones();
                            p.idPerfilOpcion = int.Parse(drDatos["idPerfilOpcion"].ToString());
                            p.idPerfil = int.Parse(drDatos["idperfil"].ToString());
                            p.idOpcion = int.Parse(drDatos["idOpcion"].ToString());                            
                            p.visible = drDatos["visible"].ToString();
                            Opcion opcion = new Opcion();
                            opcion.idOpcion = int.Parse(drDatos["idOpcion"].ToString());
                            opcion.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            opcion.nombre = drDatos["nombre"].ToString();
                            opcion.idAsp = drDatos["idasp"].ToString();
                            opcion.descripcion = drDatos["descripcion"].ToString();
                            opcion.componenteIndex = drDatos["componenteIndex"].ToString();
                            opcion.chkboxTreeindex = (drDatos["chkboxTreeindex"].ToString().Length>0 ? int.Parse(drDatos["chkboxTreeindex"].ToString()):0);
                            opcion.idcheckbox = drDatos["idcheckbox"].ToString();
                            p.opcion = opcion;
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
