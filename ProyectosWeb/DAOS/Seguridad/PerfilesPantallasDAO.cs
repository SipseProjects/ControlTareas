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
   public class PerfilesPantallasDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        public PerfilesPantallasDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult registrarPerfilesPantallas(PerfilesPantallas ppantallas)
        {
            DbQueryResult resultado = new DbQueryResult();          
            
            try
            {
                _conn.Open();          
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from perfilespantallas o where o.idpantalla=" + ppantallas.idPantalla + " and o.idperfil=" + ppantallas.idPerfil + "");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into perfilespantallas(idpantalla,idperfil,visible) values(@parm1,@parm2,@parm3)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);

                       cmSql.Parameters["@parm1"].Value = ppantallas.idPantalla;
                       cmSql.Parameters["@parm2"].Value = ppantallas.idPerfil;
                       cmSql.Parameters["@parm3"].Value = ppantallas.visible.Trim();
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
        public DbQueryResult UpdatePerfilesPantallas(PerfilesPantallas ppantallas)
        {
            DbQueryResult resultado = new DbQueryResult();
            
            try
            {
                _conn.Open();
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                cmSql.CommandText = " update perfilespantallas set idperfil=@parm1, idpantalla=@parm2, visible=@parm3  where idperfilpantalla=@parm4";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                cmSql.Parameters.Add("@parm4", SqlDbType.Int);

                cmSql.Parameters["@parm1"].Value = ppantallas.idPerfil;
                cmSql.Parameters["@parm2"].Value = ppantallas.idPantalla;
                cmSql.Parameters["@parm3"].Value = ppantallas.visible.Trim();
                cmSql.Parameters["@parm4"].Value = ppantallas.idPerfilPantalla;
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

        public PerfilesPantallas getPerfilPantalla(int idPerfil, int idPantalla)
        {
            PerfilesPantallas p = new PerfilesPantallas();            
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from perfilespantallas o where o.idperfil=@parm1 and o.idpantalla=@parm2";
                cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                cmSql.Parameters["@parm1"].Value = idPerfil;
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
                            p.idPerfilPantalla = int.Parse(drDatos["idperfilpantalla"].ToString());
                            p.idPerfil = int.Parse(drDatos["idperfil"].ToString()); 
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

        public List<PerfilesPantallas> getPerfilesPantallas(int idPerfil, int idPantalla, int idModulo)
        {
            List<PerfilesPantallas> listado = new List<PerfilesPantallas>();
            
            try
            {
                _conn.Open();
                SqlCommand cmSql = _conn.CreateCommand();
                if (idPerfil > 0 && idPantalla < 1 && idModulo < 1)
                {
                    cmSql.CommandText = 
                    "select pp.idperfilpantalla,pp.idpantalla,pp.idperfil,pp.visible,pp.componenteindex,p.idmodulo," +
                    " p.nombre,p.descripcion,p.idasp, p.estado" +
                     " from perfilespantallas pp" +
                    " inner join pantallas p" +
                    " on pp.idpantalla=p.idpantalla where p.estado=0 and pp.idperfil=@parm1";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idPerfil;
                }
                else if (idPantalla > 0 && idPerfil > 0 && idModulo<1)
                {
                    cmSql.CommandText =
                      "select pp.idperfilpantalla,pp.idpantalla,pp.idperfil,pp.visible,pp.componenteindex,p.idmodulo," +
                      " p.nombre,p.descripcion,p.idasp, p.estado" +
                       " from perfilespantallas pp" +
                      " inner join pantallas p" +
                      " on pp.idpantalla=p.idpantalla where p.estado=0 and pp.idperfil=@parm1 and pp.idpantalla=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idPerfil;
                    cmSql.Parameters["@parm2"].Value = idPantalla;
                }
                else if (idModulo > 0 && idPerfil > 0 && idPantalla<1)
                {
                    cmSql.CommandText =
                      "select pp.idperfilpantalla,pp.idpantalla,pp.idperfil,pp.visible,pp.componenteindex,p.idmodulo," +
                      " p.nombre,p.descripcion,p.idasp, p.estado" +
                       " from perfilespantallas pp" +
                      " inner join pantallas p" +
                      " on pp.idpantalla=p.idpantalla where p.estado=0 and pp.idperfil=@parm1 and p.idmodulo=@parm2";
                    cmSql.Parameters.Add("@parm1", SqlDbType.Int);
                    cmSql.Parameters.Add("@parm2", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = idPerfil;
                    cmSql.Parameters["@parm2"].Value = idModulo;
                }
                else if (idModulo > 0 && idPerfil < 1 && idPantalla < 1)
                {
                    cmSql.CommandText =
                      "select pp.idperfilpantalla,pp.idpantalla,pp.idperfil,pp.visible,pp.componenteindex,p.idmodulo," +
                      " p.nombre,p.descripcion,p.idasp, p.estado" +
                       " from perfilespantallas pp" +
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
                            PerfilesPantallas p = new PerfilesPantallas();
                            p.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            p.idPerfil = int.Parse(drDatos["idperfil"].ToString());
                            p.idPerfilPantalla = int.Parse(drDatos["idperfilpantalla"].ToString());
                            p.visible = drDatos["visible"].ToString();
                            p.componenteIndex = drDatos["componenteIndex"].ToString();
                            Pantalla pantalla=new Pantalla();
                            pantalla.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            pantalla.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            pantalla.idAsp = drDatos["idasp"].ToString();
                            pantalla.nombre = drDatos["nombre"].ToString();
                            pantalla.descripcion = drDatos["descripcion"].ToString();
                            p.pantalla = pantalla;
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
