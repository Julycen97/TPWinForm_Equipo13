﻿using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vistas
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;

        private Imagen imagen = new Imagen();

        private List<Imagen> imagenes = new List<Imagen>();                 // NUEVO PARA AGREGAR MUCHAS IMAGENES

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();

            this.articulo = articulo;

            Text = "Modificar Articulo";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            bool esAgregar = true;
            ArticuloNegocio artNeg = new ArticuloNegocio();
            ImagenNegocio imgNeg = new ImagenNegocio();

            if (string.IsNullOrEmpty(txtCodigo.Text) ||
                 string.IsNullOrEmpty(txtDescripcion.Text) ||
                 string.IsNullOrEmpty(txtNombre.Text) ||
                 /*cboMarca.SelectedItem == null ||
                 cboCategoria.SelectedItem == null ||*/
                  string.IsNullOrEmpty(txtPrecio.Text) ||
                  !float.TryParse(txtPrecio.Text, out float precio))
            {
                MessageBox.Show("Error: Todos los campos deben completarse con datos válidos");

                Close();
            }
            else
            {

                try
                {
                    if (articulo == null)
                    {
                        articulo = new Articulo();
                        esAgregar = true;
                        articulo.CodArt = txtCodigo.Text;
                        articulo.DescripcionArt = txtDescripcion.Text;
                        articulo.NombreArt = txtNombre.Text;
                        articulo.MarcaArt = (Marca)cboMarca.SelectedItem;
                        articulo.CategoriaArt = (Categoria)cboCategoria.SelectedItem;
                        articulo.PrecioArt = (decimal)float.Parse(txtPrecio.Text);
                        // falta mandar imagen


                    }
                    else
                    {
                        esAgregar = false;
                        articulo.CodArt = txtCodigo.Text;
                        articulo.DescripcionArt = txtDescripcion.Text;
                        articulo.NombreArt = txtNombre.Text;
                        articulo.MarcaArt = (Marca)cboMarca.SelectedItem;
                        articulo.CategoriaArt = (Categoria)cboCategoria.SelectedItem;
                        articulo.PrecioArt = (decimal)float.Parse(txtPrecio.Text);
                    }

                    artNeg.Agregar_ModificarDatos(articulo, esAgregar);

                    if (esAgregar)
                    {
                        MessageBox.Show("Registro agregado exitosamente!");

                        //imagen.IdArt = idUltimoArt();

                        int idArticulo = idUltimoArt();

                        foreach (Imagen item in imagenes)
                        {
                            item.IdImagen = idArticulo;
                        }

                        //imagen.URLImagen = txtImagenes.Text;

                        foreach (Imagen item in imagenes)
                        {
                            imgNeg.Agregar_ModificarDatos(item, esAgregar);
                        }

                        //imgNeg.Agregar_ModificarDatos(imagen, esAgregar);
                    }
                    else
                    {
                        MessageBox.Show("Registro modificado exitosamente!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            Close();
        }

        private int idUltimoArt()
        {
            AccesoDatos dato = new AccesoDatos();
            int id = 0;

            try
            {
                dato.SetearConsulta("SELECT TOP 1 Id FROM ARTICULOS ORDER BY Id DESC");
                dato.AbrirConexionEjecutarConsulta();

                while (dato.Lector.Read())
                {
                    id = (int)dato.Lector["Id"];
                }

                return id;
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                dato.CerrarConexion();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            ArticuloNegocio artNeg = new ArticuloNegocio();
                   
            try
            {
                cboMarca.DataSource = marcaNegocio.ObtenerDatos();
                cboMarca.ValueMember = "IdMarca";
                cboMarca.DisplayMember = "NombreMarca";

                cboCategoria.DataSource = categoriaNegocio.ObtenerDatos();
                cboCategoria.ValueMember = "IdCategoria";
                cboCategoria.DisplayMember = "NombreCategoria";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.CodArt;
                    txtNombre.Text = articulo.NombreArt;
                    txtDescripcion.Text = articulo.DescripcionArt;
                    cboMarca.SelectedValue = articulo.MarcaArt.IdMarca;
                    cboCategoria.SelectedValue = articulo.CategoriaArt.IdCategoria;
                    txtPrecio.Text = articulo.PrecioArt.ToString();

                    try
                    {
                        txtImagenes.Text = articulo.ImagenArt[0].URLImagen;
                        pbxImagen.Load(txtImagenes.Text);
                    }
                    catch (Exception)
                    {
                        //SI NO PUEDE CARGA UNA POR DEFECTO
                        pbxImagen.Load("https://images.assetsdelivery.com/compings_v2/pavelstasevich/pavelstasevich1811/pavelstasevich181101028.jpg");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //AGREGAR IMAGEN
        private void btnVerImagen_Click(object sender, EventArgs e)
        {
            try
            {
                pbxImagen.Load(txtImagenes.Text);

            }
            catch (Exception)
            {
                pbxImagen.Load("https://images.assetsdelivery.com/compings_v2/pavelstasevich/pavelstasevich1811/pavelstasevich181101028.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                Imagen nuevaImg = new Imagen();
                nuevaImg.URLImagen = txtImagenes.Text;
                imagenes.Add(nuevaImg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
