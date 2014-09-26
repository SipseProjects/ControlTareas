using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Tareas
{
  public sealed  class ModuloTarea
    {
      public int id;
      public string clave;
      public string nombre;
      public string descripcion;
      public string referencia;
      public string estado;
      public String FechaInicio;
      public String FechaFinEstimada;
      public String FechaFinReal;
      public String DiferenciaFechaFin;
      public TimeSpan horaEstimada;
      public TimeSpan horaReal;
      public TimeSpan DiferenciaHoraFin;
 
    }
}  
