using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Seguridad
{
   public class SistemasModulos
    {
       public int idSistemaModulo;
       public int idModulo;
       public int idSistema;
       public String h3visible;
       public String divvisible;
       public Modulo modulo = new Modulo();
       public List<SistemasModulos> sistemasModulos = new List<SistemasModulos>();
       public List<PerfilesModulos> perfilesModulos = new List<PerfilesModulos>();
       public List<PerfilesPantallas> perfilesPantallas = new List<PerfilesPantallas>();
       public List<PerfilesOpciones> perfilesOpciones = new List<PerfilesOpciones>();
       public List<UsuariosModulos> usuariosModulos = new List<UsuariosModulos>();
       public List<UsuariosPantallas> usuariosPantallas = new List<UsuariosPantallas>();
       public List<UsuariosOpciones> usuariosOpciones = new List<UsuariosOpciones>();
    }
}
