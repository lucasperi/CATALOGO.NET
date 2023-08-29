using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tp2_dominio;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar_categoria()
        {
            List<Categoria> lista_categorias = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("Select Id, Descripcion From CATEGORIAS");
                datos.ejecutarConsulta();
                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.categoria_id = (int)datos.Lector["Id"];
                    aux.categoria_descripcion = (string)datos.Lector["Descripcion"];

                    lista_categorias.Add(aux);
                }

                return lista_categorias;
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
