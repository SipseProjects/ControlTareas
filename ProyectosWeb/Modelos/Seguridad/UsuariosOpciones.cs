using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class UsuariosOpciones
    {
       public int idUsuarioOpcion;
       public int idUsuario;
       public int idOpcion;
       public String visible;
       public Opcion opcion = new Opcion();
    }
}
