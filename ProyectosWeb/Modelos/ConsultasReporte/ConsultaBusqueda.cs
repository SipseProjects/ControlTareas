using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Seguridad.ControlAcceso;
using Models.Tareas;

namespace Models.ConsultasReporte
{
   public sealed  class ConsultaBusqueda
    {
        public UsuarioLogin usuario= new UsuarioLogin();
        public ModuloTarea moduloTarea = new ModuloTarea();
    }
}
