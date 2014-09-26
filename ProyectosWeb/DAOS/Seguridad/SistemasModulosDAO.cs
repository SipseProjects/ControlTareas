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
   public class SistemasModulosDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        private DbQueryResult _status = new DbQueryResult();
        public SistemasModulosDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarSistemasModulos(SistemasModulos sistemamodulo)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();          
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from sistemasmodulos o where o.idmodulo=" + sistemamodulo.idModulo + " and o.idsistema=" + sistemamodulo.idSistema + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into sistemasmodulos(idmodulo,idsistema,h3visible,divvisible) values(@parm1,@parm2,@parm3,@parm4)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = sistemamodulo.idModulo;
                       cmSql.Parameters["@parm2"].Value = sistemamodulo.idSistema;
                       cmSql.Parameters["@parm3"].Value = sistemamodulo.h3visible;
                       cmSql.Parameters["@parm4"].Value = sistemamodulo.divvisible;
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
        public DbQueryResult UpdateSistemasModulos(SistemasModulos sistemamodulo)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update sistemasmodulos set idsistema=@parm2,idmodulo=@parm3,h3visible=@parm4,divvisible=@parm5 where idsistemamodulo=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.Int);
                cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm5", SqlDbType.VarChar);
                cmSql.Parameters["@parm1"].Value = sistemamodulo.idSistemaModulo;
                cmSql.Parameters["@parm2"].Value = sistemamodulo.idSistema;
                cmSql.Parameters["@parm3"].Value = sistemamodulo.idModulo;
                cmSql.Parameters["@parm4"].Value = sistemamodulo.h3visible;
                cmSql.Parameters["@parm5"].Value = sistemamodulo.divvisible;
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

        public SistemasModulos getSistemaModulo(int idsistema, int idmodulo)
        {
            SistemasModulos p = new SistemasModulos();
            _status = new DbQueryResult();
            _status.Success = false;
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from sistemasmodulos o where o.idsistema=@parm1 and o.idmodulo=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = idsistema;
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
                            p.idSistemaModulo = int.Parse(drDatos["idsistemamodulo"].ToString());
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());                            
                            p.idSistema = int.Parse(drDatos["idsistema"].ToString());
                            p.h3visible = drDatos["h3visible"].ToString();
                            p.divvisible = drDatos["divvisible"].ToString();
                            _status.Success = true; 
                    }
                }
            }catch(Exception e){
                _status.ErrorMessage = e.Message;   
            }
            _conn.Close();
            return p;
        }
        public List<SistemasModulos> getSistemasModulos(int IdSistema, int idmodulo)
        {
            List<SistemasModulos> listado = new List<SistemasModulos>();
            _status = new DbQueryResult();
            _status.Success = false;
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if (IdSistema > 0 && idmodulo < 1)
                {
                    cmSql.CommandText = "select pm.idmodulo, pm.idsistemamodulo, pm.idsistema, pm.divvisible, m.idmodulo,m.nombre, m.h3id, m.divid, m.estado from modulos m"
                    + " left join sistemasmodulos pm"
                    + " on m.idmodulo=pm.idmodulo and pm.idsistema in(" + IdSistema + ") ";
                }
                else if (idmodulo > 0 && IdSistema > 0)
                {
                    cmSql.CommandText = "select pm.idmodulo, pm.idsistemamodulo, pm.idsistema, pm.divvisible, m.idmodulo,m.nombre, m.h3id, m.divid, m.estado from sistemasmodulos pm"
                    + " inner join modulos m"
                    + " on m.idmodulo=pm.idmodulo and pm.idsistema in(" + IdSistema + ") and pm.idmodulo=" + idmodulo + "";
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
                            SistemasModulos pmodulo = new SistemasModulos();
                            pmodulo.idModulo = drDatos["idmodulo"].ToString().Length>0 ? int.Parse(drDatos["idmodulo"].ToString()):0;
                            pmodulo.idSistema =  drDatos["idsistema"].ToString().Length>0 ? int.Parse(drDatos["idsistema"].ToString()):0;
                            pmodulo.idSistemaModulo = drDatos["idSistemaModulo"].ToString().Length > 0 ? int.Parse(drDatos["idSistemaModulo"].ToString()) : 0;
                            pmodulo.divvisible = drDatos["divvisible"].ToString();
                            pmodulo.h3visible = drDatos["divvisible"].ToString();
                            Modulo mod = new Modulo();
                            mod.idModulo = pmodulo.idModulo;
                            mod.estado = drDatos["estado"].ToString().Length > 0 ? int.Parse(drDatos["estado"].ToString()) : 0;
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
            catch (Exception e)
            {
                _status.ErrorMessage = e.Message;
            }
            _conn.Close();
            return listado;
        }        
    }
}
