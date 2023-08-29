using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tp2_dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar_articulos()
        {
            List<Articulo> lista_articulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("Select Codigo, Nombre, A.Descripcion as Descripcion, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, A.IdCategoria, A.IdMarca, A.Id as IdArt, Precio From ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca");
                datos.ejecutarConsulta();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["IdArt"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl")))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.Categoria = new Categoria();
                    aux.Categoria.categoria_id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.categoria_descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.marca_id = (int)datos.Lector["IdMarca"];
                    aux.Marca.marca_descripcion = (string)datos.Lector["Marca"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    lista_articulos.Add(aux);
                }

                return lista_articulos;
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

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdCategoria, IdMarca, ImagenUrl, Precio)Values('" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', @idCategoria, @idMarca, @imagenUrl, @precio)");
                datos.setParametro("@idCategoria", nuevo.Categoria.categoria_id);
                datos.setParametro("@idMarca", nuevo.Marca.marca_id);
                datos.setParametro("@imagenUrl", nuevo.ImagenUrl);
                datos.setParametro("@precio", nuevo.Precio);
                datos.ejecutarAccion();
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

        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idmarca, IdCategoria = @idcategoria, ImagenUrl = @imagenurl, Precio = @precio where Id = @id");
                datos.setParametro("@codigo", articulo.Codigo);
                datos.setParametro("@nombre", articulo.Nombre);
                datos.setParametro("@descripcion", articulo.Descripcion);
                datos.setParametro("@idcategoria", articulo.Categoria.categoria_id);
                datos.setParametro("@idmarca", articulo.Marca.marca_id);
                datos.setParametro("@imagenurl", articulo.ImagenUrl);
                datos.setParametro("@precio", articulo.Precio);
                datos.setParametro("id", articulo.Id);

                datos.ejecutarAccion();
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

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("Delete From ARTICULOS where Id = @id");
                datos.setParametro("@id", id);
                datos.ejecutarAccion();
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

        public void verDetalle(Articulo articulo) 
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("Select Codigo, Nombre, A.Descripcion as Descripcion, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, Precio From ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = M.Id and A.Id = " + articulo.Id + "");
                datos.ejecutarConsulta();
                while (datos.Lector.Read())
                {
                    articulo.Codigo = (string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.Categoria = new Categoria();
                    articulo.Categoria.categoria_descripcion = (string)datos.Lector["Categoria"];
                    articulo.Marca = new Marca();
                    articulo.Marca.marca_descripcion = (string)datos.Lector["Marca"];
                    articulo.Precio = (decimal)datos.Lector["Precio"];
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl")))
                    {
                        articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                }
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

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista_articulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Codigo, Nombre, A.Descripcion as Descripcion, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, A.IdCategoria, A.IdMarca, A.Id as IdArt, Precio From ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca and ";
                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con:":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;

                        case "Termina con:":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;

                        case "Contiene:":
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                } else if (campo == "Categoría")
                {
                        switch (criterio)
                        {
                            case "Comienza con:":
                                consulta += "C.Descripcion like '" + filtro + "%'";
                                break;

                            case "Termina con:":
                                consulta += "C.Descripcion like '%" + filtro + "'";
                                break;

                            case "Contiene:":
                                consulta += "C.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                } else if (campo == "Marca")
                {
                        switch (criterio)
                        {
                            case "Comienza con:":
                                consulta += "M.Descripcion like '" + filtro + "%'";
                                break;

                            case "Termina con:":
                                consulta += "M.Descripcion like '%" + filtro + "'";
                                break;

                            case "Contiene:":
                                consulta += "M.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                } else
                {
                    switch (criterio)
                    {
                        case "Mayor a:":
                            consulta += "Precio > " + filtro;
                            break;

                        case "Menor a:":
                            consulta += "Precio < " + filtro;
                            break;

                        case "Igual a:":
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                datos.setConsulta(consulta);
                datos.ejecutarConsulta();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["IdArt"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl")))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    aux.Categoria = new Categoria();
                    aux.Categoria.categoria_id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.categoria_descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.marca_id = (int)datos.Lector["IdMarca"];
                    aux.Marca.marca_descripcion = (string)datos.Lector["Marca"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    lista_articulos.Add(aux);
                }

                return lista_articulos;
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
