using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Seguridad;
using Models;
using Models.Tareas;
using System.Data;
using System.Data.SqlClient;

namespace DAOS.Tareas
{
   public class TareasDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        private DbQueryResult _status;
        public TareasDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }       
                
        public DataTable getModuloTarea(ModuloTarea tareas, String opcionTarea, int idUsuario, int idSistema)
        {
            DataTable ds = new DataTable();
            //DataSet ds = new DataSet();
            ds.Namespace = "DatosModuloTarea";
            ds.TableName = "ModuloTarea";
            _status = null;
             _status = new DbQueryResult();
             _status.Success = false;
            try
            {
                 _conn.Open();               
                 SqlDataAdapter sqlDA = new SqlDataAdapter("BusquedaConsultas", _conn);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;                
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_nombre", SqlDbType.VarChar));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_opciontarea", SqlDbType.VarChar));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_estado", SqlDbType.VarChar));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_usuario", SqlDbType.Int));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_sistema", SqlDbType.Int));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_FechaInicio", SqlDbType.VarChar));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_FechaFinEstimada", SqlDbType.VarChar));
                sqlDA.SelectCommand.Parameters.Add(new SqlParameter("@p_FechaFinReal", SqlDbType.VarChar));
                sqlDA.SelectCommand.Parameters["@p_nombre"].Value =tareas.nombre.Trim();
                sqlDA.SelectCommand.Parameters["@p_opciontarea"].Value = opcionTarea.Trim();
                sqlDA.SelectCommand.Parameters["@p_estado"].Value = tareas.estado.Trim();
                sqlDA.SelectCommand.Parameters["@p_usuario"].Value = idUsuario;
                sqlDA.SelectCommand.Parameters["@p_sistema"].Value = idSistema;
                sqlDA.SelectCommand.Parameters["@p_FechaInicio"].Value = tareas.FechaInicio;
                sqlDA.SelectCommand.Parameters["@p_FechaFinEstimada"].Value = tareas.FechaFinEstimada;
                sqlDA.SelectCommand.Parameters["@p_FechaFinReal"].Value = tareas.FechaFinReal;

                SqlDataReader rd = sqlDA.SelectCommand.ExecuteReader();
                rd.AsParallel();
                ds.Load(rd);  
                rd.Close();
                //SqlDataAdapter da = new SqlDataAdapter(sqlDA);
                //sqlDA.Fill(ds);
                _status.Success = true;
                
            }catch(Exception e){
                _status.ErrorMessage = e.Message;
            }
            _conn.Close();
            return ds;
        }
        
    }
}
