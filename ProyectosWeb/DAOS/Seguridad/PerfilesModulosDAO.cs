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
   public class PerfilesModulosDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        public PerfilesModulosDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarPerfilesModulos(PerfilesModulos pmodulo)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from perfilesmodulos o where o.idmodulo=" + pmodulo.idModulo + " and o.idperfil=" + pmodulo.idPerfil + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into perfilesmodulos(idmodulo,idperfil,h3visible,divvisible) values(@parm1,@parm2,@parm3,@parm4)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = pmodulo.idModulo;
                       cmSql.Parameters["@parm2"].Value = pmodulo.idPerfil;
                       cmSql.Parameters["@parm3"].Value = pmodulo.h3Visible.Trim();
                       cmSql.Parameters["@parm4"].Value = pmodulo.divVisible.Trim();
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
        public DbQueryResult UpdatePerfilesModulos(PerfilesModulos pmodulo)
        {
            DbQueryResult resultado = new DbQueryResult();
           
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update perfilesmodulos set idmodulo=@parm1, idperfil=@parm2, h3visible=@parm3, divvisible=@parm4  where idperfilmodulo=@parm5";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm5", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = pmodulo.idModulo;
                cmSql.Parameters["@parm2"].Value = pmodulo.idPerfil;
                cmSql.Parameters["@parm3"].Value = pmodulo.h3Visible.Trim();
                cmSql.Parameters["@parm4"].Value = pmodulo.divVisible.Trim();
                cmSql.Parameters["@parm5"].Value = pmodulo.idPerfilModulo;
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

        public PerfilesModulos getPerfilModulo(int idperfil, int idmodulo)
        {
            PerfilesModulos p = new PerfilesModulos();            
           
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from perfilesmodulos o where o.idperfil=@parm1 and o.idmodulo=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = idperfil;
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
                            p.idPerfilModulo = int.Parse(drDatos["idperfilmodulo"].ToString());
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());                            
                            p.idPerfil = int.Parse(drDatos["idperfil"].ToString());
                            p.divVisible = drDatos["divvisible"].ToString();
                            p.h3Visible =drDatos["h3visible"].ToString();                           
                    }
                }
            }catch(Exception e){
                
            }
            _conn.Close();
            return p;
        }

        public List<PerfilesModulos> getPerfilesModulos(int IdPerfil, int idmodulo)
        {
            List<PerfilesModulos> listado = new List<PerfilesModulos>();
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if (IdPerfil > 0 && idmodulo<1)
                {
                cmSql.CommandText = "select pm.idmodulo, pm.idperfilmodulo, pm.idperfil, pm.divvisible, m.idmodulo,m.nombre, m.h3id, m.divid from perfilesmodulos pm"
                +" inner join modulos m"
                +" on m.idmodulo=pm.idmodulo and pm.idperfil in("+IdPerfil+") ";
                }else if(idmodulo>0&&IdPerfil>0){
                    cmSql.CommandText = "select pm.idmodulo, pm.idperfilmodulo, pm.idperfil, pm.divvisible, m.idmodulo,m.nombre, m.h3id, m.divid from perfilesmodulos pm"
                    + " inner join modulos m"
                    + " on m.idmodulo=pm.idmodulo and pm.idperfil in(" + IdPerfil + ") and pm.idmodulo="+idmodulo+"";
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
                            PerfilesModulos pmodulo = new PerfilesModulos();
                            pmodulo.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            pmodulo.idPerfil = int.Parse(drDatos["idperfil"].ToString());
                            pmodulo.divVisible = drDatos["divvisible"].ToString();
                            Modulo mod = new Modulo();
                            mod.Nombre = drDatos["nombre"].ToString();
                            mod.h3Id = drDatos["h3Id"].ToString();
                            mod.divId = drDatos["divId"].ToString();
                            pmodulo.modulo = mod;
                            listado.Add(pmodulo);
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
