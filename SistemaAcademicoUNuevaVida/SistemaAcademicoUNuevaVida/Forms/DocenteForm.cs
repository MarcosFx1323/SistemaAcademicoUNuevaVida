using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class DocenteForm : Form
    {
        private Docente? _docenteSeleccionado;
        private GestorAcademico Gestor => MainForm.Gestor;

        public DocenteForm()
        {
            InitializeComponent();
            CargarGrid();
        }

        private void CargarGrid(List<Docente>? lista = null)
        {
            dgvDocentes.Rows.Clear();
            foreach (var d in lista ?? Gestor.Docentes)
                dgvDocentes.Rows.Add(d.Id, d.Nombre, d.Titulo, d.Departamento, d.TotalMaterias);
            lblContador.Text = $"Total: {dgvDocentes.Rows.Count} docente(s)";
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
            => CargarGrid(Gestor.BuscarDocentes(txtBuscar.Text));

        private void dgvDocentes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDocentes.SelectedRows.Count == 0) return;
            string id = dgvDocentes.SelectedRows[0].Cells["dColId"].Value?.ToString() ?? "";
            _docenteSeleccionado = Gestor.Docentes.FirstOrDefault(d => d.Id == id);
            if (_docenteSeleccionado == null) return;

            lblTitulo.Text       = "Editar Docente";
            txtId.Text           = _docenteSeleccionado.Id;
            txtId.Enabled        = false;
            txtNombre.Text       = _docenteSeleccionado.Nombre;
            txtEmail.Text        = _docenteSeleccionado.Email;
            txtTelefono.Text     = _docenteSeleccionado.Telefono;
            txtTitulo.Text       = _docenteSeleccionado.Titulo;
            txtDepartamento.Text = _docenteSeleccionado.Departamento;
            if (_docenteSeleccionado.FechaNacimiento > DateTime.MinValue)
                dtpNacimiento.Value = _docenteSeleccionado.FechaNacimiento;
            btnEliminar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))           { Alerta("La cédula es obligatoria."); return; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))       { Alerta("El nombre es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))       { Alerta("El título es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(txtDepartamento.Text)) { Alerta("El departamento es obligatorio."); return; }

            if (_docenteSeleccionado == null)
            {
                var nuevo = new Docente
                {
                    Id = txtId.Text.Trim(), Nombre = txtNombre.Text.Trim(),
                    Email = txtEmail.Text.Trim(), Telefono = txtTelefono.Text.Trim(),
                    Titulo = txtTitulo.Text.Trim(), Departamento = txtDepartamento.Text.Trim(),
                    FechaNacimiento = dtpNacimiento.Value
                };
                var (ok, msg) = Gestor.AgregarDocente(nuevo);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); Limpiar(); }
            }
            else
            {
                _docenteSeleccionado.Nombre       = txtNombre.Text.Trim();
                _docenteSeleccionado.Email        = txtEmail.Text.Trim();
                _docenteSeleccionado.Telefono     = txtTelefono.Text.Trim();
                _docenteSeleccionado.Titulo       = txtTitulo.Text.Trim();
                _docenteSeleccionado.Departamento = txtDepartamento.Text.Trim();
                _docenteSeleccionado.FechaNacimiento = dtpNacimiento.Value;
                var (ok, msg) = Gestor.ActualizarDocente(_docenteSeleccionado);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); Limpiar(); }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_docenteSeleccionado == null) return;
            if (MessageBox.Show($"¿Eliminar a '{_docenteSeleccionado.Nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            var (ok, msg) = Gestor.EliminarDocente(_docenteSeleccionado.Id);
            Resultado(ok, msg);
            if (ok) { CargarGrid(); Limpiar(); }
        }

        private void Limpiar()
        {
            _docenteSeleccionado = null;
            lblTitulo.Text = "Nuevo Docente";
            txtId.Text = txtNombre.Text = txtEmail.Text =
            txtTelefono.Text = txtTitulo.Text = txtDepartamento.Text = "";
            txtId.Enabled = true;
            dtpNacimiento.Value = DateTime.Today.AddYears(-30);
            btnEliminar.Enabled = false;
            dgvDocentes.ClearSelection();
        }

        private static void Alerta(string m) =>
            MessageBox.Show(m, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private static void Resultado(bool ok, string m) =>
            MessageBox.Show(m, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }
}
