using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAOS.Seguridad;
using DAOS.Tareas;
using Models.Seguridad;
using Models;
using Models.Tareas;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic.Tareas
{
    public class ConsultaReporteBL
    {
        private TareasDAO _pantalladao;
        public ConsultaReporteBL(SqlConnection con)
        {
            _pantalladao = new TareasDAO(con);
        }
        public DataTable getModuloTarea(ModuloTarea tarea, String opcionTarea, int idUsuario, int idSistema)
        {
            return _pantalladao.getModuloTarea(tarea,opcionTarea, idUsuario,idSistema);
        }
    }
}
