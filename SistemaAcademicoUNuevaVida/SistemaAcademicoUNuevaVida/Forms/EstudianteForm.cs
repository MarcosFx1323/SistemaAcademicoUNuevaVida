using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class EstudianteForm : Form
    {
        private Estudiante? _estudianteSeleccionado;
        private GestorAcademico Gestor => MainForm.Gestor;

        public EstudianteForm()
        {
            InitializeComponent();
            CargarGrid();
        }

        private void CargarGrid(List<Estudiante>? lista = null)
        {
            dgvEstudiantes.Rows.Clear();
            var estudiantes = lista ?? Gestor.Estudiantes;
            foreach (var e in estudiantes)
                dgvEstudiantes.Rows.Add(e.Id, e.Nombre, e.Programa, e.Semestre, e.Email,
                    e.PromedioGeneral > 0 ? e.PromedioGeneral.ToString("F2") : "-");
            lblContador.Text = $"Total: {dgvEstudiantes.Rows.Count} estudiante(s)";
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
            => CargarGrid(Gestor.BuscarEstudiantes(txtBuscar.Text));

        private void dgvEstudiantes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count == 0) return;
            string id = dgvEstudiantes.SelectedRows[0].Cells["colId"].Value?.ToString() ?? "";
            _estudianteSeleccionado = Gestor.Estudiantes.FirstOrDefault(est => est.Id == id);
            if (_estudianteSeleccionado == null) return;

            lblTitulo.Text   = "Editar Estudiante";
            txtId.Text       = _estudianteSeleccionado.Id;
            txtId.Enabled    = false;
            txtNombre.Text   = _estudianteSeleccionado.Nombre;
            txtEmail.Text    = _estudianteSeleccionado.Email;
            txtTelefono.Text = _estudianteSeleccionado.Telefono;
            txtPrograma.Text = _estudianteSeleccionado.Programa;
            nudSemestre.Value = _estudianteSeleccionado.Semestre;
            if (_estudianteSeleccionado.FechaNacimiento > DateTime.MinValue)
                dtpNacimiento.Value = _estudianteSeleccionado.FechaNacimiento;
            btnEliminar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (_estudianteSeleccionado == null)
            {
                var nuevo = new Estudiante
                {
                    Id = txtId.Text.Trim(), Nombre = txtNombre.Text.Trim(),
                    Email = txtEmail.Text.Trim(), Telefono = txtTelefono.Text.Trim(),
                    Programa = txtPrograma.Text.Trim(), Semestre = (int)nudSemestre.Value,
                    FechaNacimiento = dtpNacimiento.Value
                };
                var (ok, msg) = Gestor.AgregarEstudiante(nuevo);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); LimpiarFormulario(); }
            }
            else
            {
                _estudianteSeleccionado.Nombre   = txtNombre.Text.Trim();
                _estudianteSeleccionado.Email    = txtEmail.Text.Trim();
                _estudianteSeleccionado.Telefono = txtTelefono.Text.Trim();
                _estudianteSeleccionado.Programa = txtPrograma.Text.Trim();
                _estudianteSeleccionado.Semestre = (int)nudSemestre.Value;
                _estudianteSeleccionado.FechaNacimiento = dtpNacimiento.Value;
                var (ok, msg) = Gestor.ActualizarEstudiante(_estudianteSeleccionado);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); LimpiarFormulario(); }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e) => LimpiarFormulario();

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_estudianteSeleccionado == null) return;
            if (MessageBox.Show($"¿Eliminar a '{_estudianteSeleccionado.Nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            var (ok, msg) = Gestor.EliminarEstudiante(_estudianteSeleccionado.Id);
            Resultado(ok, msg);
            if (ok) { CargarGrid(); LimpiarFormulario(); }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))    { Alerta("La cédula/ID es obligatoria."); return false; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { Alerta("El nombre es obligatorio."); return false; }
            if (string.IsNullOrWhiteSpace(txtPrograma.Text)) { Alerta("El programa es obligatorio."); return false; }
            return true;
        }

        private void LimpiarFormulario()
        {
            _estudianteSeleccionado = null;
            lblTitulo.Text = "Nuevo Estudiante";
            txtId.Text = txtNombre.Text = txtEmail.Text = txtTelefono.Text = txtPrograma.Text = "";
            txtId.Enabled = true;
            nudSemestre.Value = 1;
            dtpNacimiento.Value = DateTime.Today.AddYears(-18);
            btnEliminar.Enabled = false;
            dgvEstudiantes.ClearSelection();
            txtId.Focus();
        }

        private static void Resultado(bool ok, string msg) =>
            MessageBox.Show(msg, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

        private static void Alerta(string msg) =>
            MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
