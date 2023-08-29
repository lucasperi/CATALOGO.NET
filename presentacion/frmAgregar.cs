using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using tp2_dominio;
using presentacion;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

namespace presentacion
{
    public partial class frmAgregar : Form
    {
        private Articulo articulo = null;
        public frmAgregar()
        {
            InitializeComponent();
        }

        public frmAgregar(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "MODIFICAR ARTÍCULO";
            txtTitulo.Text = "MODIFICAR ARTÍCULO:";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool validarCodigo(string codigo) 
        {
            if (codigo.ToString().Length != 3)
            {
                return true;
            }
            return false;
        }

        private bool validarPrecio(string precio)
        {
            foreach (char caracter in precio)
            {
                if (char.IsNumber(caracter))
                {
                    return true;
                }
            }
            return false;
        }

        private bool validarVacio()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtDescripcion.Text))
            {
                DialogResult respuesta = MessageBox.Show("Faltan campos por completar.", "¿SEGURO QUIERES CONTINUAR?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.OK)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                if (!validarPrecio(txtPrecio.ToString()))
                {
                    MessageBox.Show("¡Debes ponerle un precio al artículo!", "ERROR");
                    return;
                }
                if (validarCodigo(txtCodigo.Text))
                {
                    MessageBox.Show("¡El código debe ser de 3 caracteres!", "ERROR");
                    return;
                }
                else if (validarVacio())
                {
                    if (articulo == null)
                    {
                        articulo = new Articulo();
                    }
                    articulo.Codigo = txtCodigo.Text;
                    articulo.Nombre = txtNombre.Text;
                    articulo.Descripcion = txtDescripcion.Text;
                    articulo.Categoria = (Categoria)cboxCategoria.SelectedItem;
                    articulo.Marca = (Marca)cboxMarca.SelectedItem;
                    articulo.Precio = decimal.Parse(txtPrecio.Text);
                    articulo.ImagenUrl = txtUrlImg.Text;

                    if (articulo.Id != 0)
                    {
                        articuloNegocio.modificar(articulo);
                        MessageBox.Show("ARTÍCULO MODIFICADO EXITOSAMENTE");
                    }
                    else
                    {
                        articuloNegocio.agregar(articulo);
                        MessageBox.Show("ARTÍCULO AGREGADO EXITOSAMENTE");
                    }
                    this.Close();
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoria = new CategoriaNegocio();
            MarcaNegocio marca = new MarcaNegocio();
            try
            {
                cboxCategoria.DataSource = categoria.listar_categoria();
                cboxCategoria.ValueMember = "categoria_id";
                cboxCategoria.DisplayMember = "categoria_descripcion";
                cboxMarca.DataSource = marca.listar_marcas();
                cboxMarca.ValueMember = "marca_id";
                cboxMarca.DisplayMember = "marca_descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImg.Text = articulo.ImagenUrl;
                    cargarImg(txtUrlImg.Text);
                    txtPrecio.Text = articulo.Precio.ToString();
                    cboxCategoria.SelectedValue = articulo.Categoria.categoria_id;
                    cboxMarca.SelectedValue = articulo.Marca.marca_id;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnVistaPrevia_Click_1(object sender, EventArgs e)
        {
            cargarImg(txtUrlImg.Text);
        }

        private void cargarImg (string imagen)
        {
            try
            {
                pboxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pboxArticulo.Load("https://media.istockphoto.com/id/1222357475/vector/image-preview-icon-picture-placeholder-for-website-or-ui-ux-design-vector-illustration.jpg?s=612x612&w=0&k=20&c=KuCo-dRBYV7nz2gbk4J9w1WtTAgpTdznHu55W9FjimE=");
            }
        }

        private void btnAgregarImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog img = new OpenFileDialog();
            img.Filter = "jpg|*.jpg;|png|*.png";
            if (img.ShowDialog() == DialogResult.OK)
            {
                txtUrlImg.Text = img.FileName;
                cargarImg(img.FileName);

                File.Copy(img.FileName, ConfigurationManager.AppSettings["Articulos"] + img.FileName);
            }
        }
    }
}
