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
        private PantallaDAO _pantalladao;
        public PantallaBL(SqlConnection con)
        {
            _pantalladao = new PantallaDAO(con);
        }
        public List<Pantalla> getPantallas()
        {
            return _pantalladao.getPantallas();
        }
        public DbQueryResult registrarPantalla(Pantalla pantalla)
        {
            return _pantalladao.registrarPantalla(pantalla);
        }
        public DbQueryResult UpdatePantalla(Pantalla pantalla)
        {
            return _pantalladao.UpdatePantalla(pantalla);
        }
        public DbQueryResult DeletePantalla(int idpantalla, int activar)
        {
            return _pantalladao.DeletePantalla(idpantalla, activar);
        }
        
        public Pantalla getPantalla(String nombre, String idasp)
        {
            return _pantalladao.getPantalla(nombre, idasp);
        }
    }
}
