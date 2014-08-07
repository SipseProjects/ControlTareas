using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class PerfilesModulos
    {
       public int idPerfilModulo;
       public int idModulo;
       public int idPerfil;
       public String h3Visible;
       public String divVisible;
       public Modulo modulo = new Modulo();
    }
}
