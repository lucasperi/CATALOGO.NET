using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tp2_dominio;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar_marcas()
        {
            List<Marca> lista_marcas = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("Select Id, Descripcion From MARCAS");
                datos.ejecutarConsulta();
                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.marca_id = (int)datos.Lector["Id"];
                    aux.marca_descripcion = (string)datos.Lector["Descripcion"];

                    lista_marcas.Add(aux);
                }

                return lista_marcas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
