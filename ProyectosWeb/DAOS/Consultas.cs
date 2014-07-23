using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAOS
{
    public class Consultas
    {
        private SqlConnection _conn;
        public Consultas(SqlConnection con)
        {
            _conn = con;
        }

        public DataSet getDataset(string command)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(command, _conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                
            };
            return ds;
        }

        public bool existeEnDB(string consulta)
        {
            DataSet existe = getDataset(consulta);
            bool siexiste = false;
            if (existe.Tables.Count > 0)
            {
                if (existe.Tables[0].Rows.Count > 0)
                {
                    siexiste = true;
                }

            }
            return siexiste;
        }
    }
}
