using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class MateriaForm : Form
    {
        private Materia? _materiaSeleccionada;
        private GestorAcademico Gestor => MainForm.Gestor;

        public MateriaForm()
        {
            InitializeComponent();
            CargarComboDocentes();
            CargarGrid();
        }

        private void CargarComboDocentes()
        {
            cmbDocente.Items.Clear();
            cmbDocente.Items.Add("-- Sin asignar --");
            foreach (var d in Gestor.Docentes) cmbDocente.Items.Add(d);
            cmbDocente.DisplayMember = "Nombre";
            cmbDocente.SelectedIndex = 0;
        }

        private void CargarGrid(List<Materia>? lista = null)
        {
            dgvMaterias.Rows.Clear();
            foreach (var m in lista ?? Gestor.Materias)
            {
                int idx = dgvMaterias.Rows.Add(m.Codigo, m.Nombre, m.Semestre, m.Creditos,
                    $"{m.Inscritos}/{m.CupoMaximo}", m.DocenteResponsable?.Nombre ?? "-");
                if (!m.TieneCupo)
                    dgvMaterias.Rows[idx].DefaultCellStyle.ForeColor = Color.FromArgb(220, 53, 69);
            }
            lblContador.Text = $"Total: {dgvMaterias.Rows.Count} materia(s)";
        }

        private void FiltrarGrid()
        {
            int semestre = cmbFiltroSemestre.SelectedIndex == 0 ? 0 : cmbFiltroSemestre.SelectedIndex;
            var lista = Gestor.FiltrarMateriasPorSemestre(semestre);
            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string f = txtBuscar.Text.ToLower();
                lista = lista.Where(m => m.Nombre.ToLower().Contains(f) || m.Codigo.ToLower().Contains(f)).ToList();
            }
            CargarGrid(lista);
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e) => FiltrarGrid();
        private void cmbFiltroSemestre_SelectedIndexChanged(object sender, EventArgs e) => FiltrarGrid();

        private void dgvMaterias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMaterias.SelectedRows.Count == 0) return;
            string codigo = dgvMaterias.SelectedRows[0].Cells["mColCodigo"].Value?.ToString() ?? "";
            _materiaSeleccionada = Gestor.Materias.FirstOrDefault(m => m.Codigo == codigo);
            if (_materiaSeleccionada == null) return;

            lblTitulo.Text = "Editar Materia";
            txtCodigo.Text = _materiaSeleccionada.Codigo; txtCodigo.Enabled = false;
            txtNombre.Text = _materiaSeleccionada.Nombre;
            nudSemestre.Value = _materiaSeleccionada.Semestre;
            nudCreditos.Value = _materiaSeleccionada.Creditos;
            nudCupo.Value = _materiaSeleccionada.CupoMaximo;

            cmbDocente.SelectedIndex = 0;
            if (_materiaSeleccionada.DocenteResponsable != null)
                for (int i = 0; i < cmbDocente.Items.Count; i++)
                    if (cmbDocente.Items[i] is Docente d && d.Id == _materiaSeleccionada.DocenteResponsable.Id)
                    { cmbDocente.SelectedIndex = i; break; }

            btnEliminar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text)) { Alerta("El código es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { Alerta("El nombre es obligatorio."); return; }

            Docente? docente = cmbDocente.SelectedItem as Docente;

            if (_materiaSeleccionada == null)
            {
                var nueva = new Materia
                {
                    Codigo = txtCodigo.Text.Trim().ToUpper(), Nombre = txtNombre.Text.Trim(),
                    Semestre = (int)nudSemestre.Value, Creditos = (int)nudCreditos.Value,
                    CupoMaximo = (int)nudCupo.Value, DocenteResponsable = docente
                };
                var (ok, msg) = Gestor.AgregarMateria(nueva);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); CargarComboDocentes(); Limpiar(); }
            }
            else
            {
                var actualizada = new Materia
                {
                    Codigo = _materiaSeleccionada.Codigo, Nombre = txtNombre.Text.Trim(),
                    Semestre = (int)nudSemestre.Value, Creditos = (int)nudCreditos.Value,
                    CupoMaximo = (int)nudCupo.Value, DocenteResponsable = docente
                };
                var (ok, msg) = Gestor.ActualizarMateria(actualizada);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); Limpiar(); }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_materiaSeleccionada == null) return;
            if (MessageBox.Show($"¿Eliminar '{_materiaSeleccionada.Nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            var (ok, msg) = Gestor.EliminarMateria(_materiaSeleccionada.Codigo);
            Resultado(ok, msg);
            if (ok) { CargarGrid(); Limpiar(); }
        }

        private void Limpiar()
        {
            _materiaSeleccionada = null;
            lblTitulo.Text = "Nueva Materia";
            txtCodigo.Text = txtNombre.Text = ""; txtCodigo.Enabled = true;
            nudSemestre.Value = nudCreditos.Value = 1; nudCupo.Value = 30;
            cmbDocente.SelectedIndex = 0; btnEliminar.Enabled = false;
            dgvMaterias.ClearSelection();
        }

        private static void Alerta(string m) =>
            MessageBox.Show(m, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private static void Resultado(bool ok, string m) =>
            MessageBox.Show(m, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }
}
