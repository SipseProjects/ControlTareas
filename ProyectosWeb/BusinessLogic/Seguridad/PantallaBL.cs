using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAOS.Seguridad;
using Models.Seguridad;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic.Seguridad
{
    public class PantallaBL
    {
        private PantallaDAO _modulodao;
        public PantallaBL(SqlConnection con)
        {
            _modulodao = new PantallaDAO(con);
        }
        public List<Pantalla> getPantallas()
        {
            return _modulodao.getPantallas();
        }
        public DbQueryResult registrarPantalla(Pantalla pantalla)
        {
            return _modulodao.registrarPantalla(pantalla);
        }
        public DbQueryResult UpdatePantalla(Pantalla pantalla)
        {
            return _modulodao.UpdatePantalla(pantalla);
        }
        public Pantalla getPantalla(String nombre, String idasp)
        {
            return _modulodao.getPantalla(nombre, idasp);
        }
    }
}
