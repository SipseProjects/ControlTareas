using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class PerfilesOpciones
    {
       public int idPerfilOpcion;
       public int idPerfil;
       public int idOpcion;
       public String visible;
       public Opcion opcion = new Opcion();
    }
}
