using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class Opcion
    {
       public int idOpcion;
       public int idPantalla;
       public String nombre;
       public String descripcion;
       public String idAsp;
       public String componenteIndex;
       public int chkboxTreeindex;
       public String idcheckbox;
       public int estado;
       public Pantalla pantalla = new Pantalla();
    }
}
