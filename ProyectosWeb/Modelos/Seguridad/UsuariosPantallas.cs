using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class UsuariosPantallas
    {
       public int idUsuarioPantalla;
       public int idPantalla;
       public int idUsuario;
       public String visible;
       public String componenteIndex;
       public Pantalla pantalla = new Pantalla();
    }
}
