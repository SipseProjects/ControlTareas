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
            _conn.Open();
            try
            {                              
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from sistemasmodulos o where o.idmodulo=" + sistemamodulo.idModulo + " and o.idsistema=" + sistemamodulo.idSistema + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into sistemasmodulos(idmodulo,idsistema) values(@parm1,@parm2)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);

                       cmSql.Parameters["@parm1"].Value = sistemamodulo.idModulo;
                       cmSql.Parameters["@parm2"].Value = sistemamodulo.idSistema;
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
        public DbQueryResult DeleteSistemasModulos(SistemasModulos sistemamodulo)
        {
            DbQueryResult resultado = new DbQueryResult();
            _conn.Open();
            try
            {
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " delete sistemasmodulos where idsistemamodulo=@parm1";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = sistemamodulo.idSistemaModulo;
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
            _conn.Open();
            try
            {
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
                            _status.Success = true; 
                    }
                }
            }catch(Exception e){
                _status.ErrorMessage = e.Message;   
            }
            _conn.Close();
            return p;
        }

        public List<SistemasModulos> getSistemasModulos(int Idsistema, int idmodulo)
        {
            List<SistemasModulos> listado = new List<SistemasModulos>();
            _status = new DbQueryResult();
            _status.Success = false;
            _conn.Open();
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();
                if (Idsistema > 0 && idmodulo<1)
                {
                cmSql.CommandText = "select * from sistemasmodulos s where s.idsistema in("+Idsistema+") ";
                }else if(idmodulo>0&&Idsistema>0){
                    cmSql.CommandText = "select * from sistemasmodulos s where s.idsistema in(" + Idsistema + ") and s.idmodulo=" + idmodulo + "";
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
                            pmodulo.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            pmodulo.idSistema = int.Parse(drDatos["idperfil"].ToString());
                            pmodulo.idSistemaModulo = int.Parse(drDatos["idsistemamodulo"].ToString());                            
                            listado.Add(pmodulo);
                            _status.Success = true;
                        }
                    }
                }
            }
            catch(Exception c) {
                _status.ErrorMessage = c.Message;
            }
            _conn.Close();
            return listado;
        }
    }
}
