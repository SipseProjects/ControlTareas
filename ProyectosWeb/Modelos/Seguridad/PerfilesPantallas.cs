using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class PerfilesPantallas
    {
       public int idPerfilPantalla;
       public int idPantalla;
       public int idPerfil;
       public String visible;
       public String componenteIndex;
       public Pantalla pantalla = new Pantalla();

    }
}
