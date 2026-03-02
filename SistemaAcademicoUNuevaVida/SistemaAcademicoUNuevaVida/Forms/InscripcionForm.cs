using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class InscripcionForm : Form
    {
        private GestorAcademico Gestor => MainForm.Gestor;

        public InscripcionForm()
        {
            InitializeComponent();
            CargarCombos();
            CargarTodasInscripciones();
        }

        private void CargarCombos()
        {
            cmbEstudiante.Items.Clear();
            cmbEstudiante.Items.Add("-- Seleccionar estudiante --");
            foreach (var e in Gestor.Estudiantes.OrderBy(e => e.Nombre)) cmbEstudiante.Items.Add(e);
            cmbEstudiante.DisplayMember = "Nombre";
            cmbEstudiante.SelectedIndex = 0;

            cmbMateria.Items.Clear();
            cmbMateria.Items.Add("-- Seleccionar materia --");
            foreach (var m in Gestor.Materias.OrderBy(m => m.Semestre)) cmbMateria.Items.Add(m);
            cmbMateria.DisplayMember = "Nombre";
            cmbMateria.SelectedIndex = 0;
        }

        private void cmbEstudiante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstudiante.SelectedItem is Estudiante est)
                lblInfoEstudiante.Text = $"{est.Programa}  |  Semestre {est.Semestre}";
            else
                lblInfoEstudiante.Text = "";
        }

        private void cmbMateria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMateria.SelectedItem is Materia mat)
            {
                lblInfoMateria.Text = $"Docente: {mat.DocenteResponsable?.Nombre ?? "Sin asignar"}";
                lblCupo.Text = $"Cupo: {mat.Inscritos}/{mat.CupoMaximo}  ({mat.CuposDisponibles} disponibles)";
                lblCupo.ForeColor = mat.TieneCupo ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
                CargarInscritosPorMateria(mat);
            }
            else
            {
                lblInfoMateria.Text = ""; lblCupo.Text = "";
                dgvInscritos.Rows.Clear();
            }
        }

        private void CargarInscritosPorMateria(Materia mat)
        {
            dgvInscritos.Rows.Clear();
            foreach (var ins in Gestor.ObtenerInscritosPorMateria(mat.Codigo))
                dgvInscritos.Rows.Add(ins.Estudiante.Nombre, ins.Estudiante.Id,
                    ins.FechaInscripcion.ToString("dd/MM/yyyy"),
                    ins.Calificacion?.PromedioFinal.ToString("F2") ?? "Pendiente");
            lblContador.Text = $"Inscritos: {dgvInscritos.Rows.Count}";
            btnCancelar.Enabled = false;
        }

        private void CargarTodasInscripciones()
        {
            dgvTodasInscripciones.Rows.Clear();
            foreach (var ins in Gestor.Inscripciones)
            {
                int idx = dgvTodasInscripciones.Rows.Add(
                    ins.Estudiante.Nombre, ins.Estudiante.Id,
                    ins.Materia.Nombre, ins.Materia.Semestre,
                    ins.FechaInscripcion.ToString("dd/MM/yyyy"), ins.Estado.ToString());
                if (ins.Estado == EstadoInscripcion.Cancelada)
                    dgvTodasInscripciones.Rows[idx].DefaultCellStyle.ForeColor = Color.Silver;
            }
        }

        private void dgvInscritos_SelectionChanged(object sender, EventArgs e)
            => btnCancelar.Enabled = dgvInscritos.SelectedRows.Count > 0;

        private void btnInscribir_Click(object sender, EventArgs e)
        {
            if (cmbEstudiante.SelectedItem is not Estudiante est)
            { MessageBox.Show("Seleccione un estudiante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cmbMateria.SelectedItem is not Materia mat)
            { MessageBox.Show("Seleccione una materia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var (ok, msg) = Gestor.InscribirEstudiante(est, mat);
            MessageBox.Show(msg, ok ? "Inscripción exitosa" : "No se puede inscribir",
                MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok) { CargarInscritosPorMateria(mat); CargarTodasInscripciones(); cmbMateria_SelectedIndexChanged(null!, EventArgs.Empty); }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (dgvInscritos.SelectedRows.Count == 0) return;
            if (cmbMateria.SelectedItem is not Materia mat) return;
            string cedula = dgvInscritos.SelectedRows[0].Cells["iColId"].Value?.ToString() ?? "";
            var ins = Gestor.Inscripciones.FirstOrDefault(i =>
                i.Estudiante.Id == cedula && i.Materia.Codigo == mat.Codigo && i.Estado == EstadoInscripcion.Activa);
            if (ins == null) return;

            if (MessageBox.Show($"¿Cancelar la inscripción de '{ins.Estudiante.Nombre}' en '{mat.Nombre}'?",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            var (ok, msg) = Gestor.CancelarInscripcion(ins);
            MessageBox.Show(msg, ok ? "Cancelada" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) { CargarInscritosPorMateria(mat); CargarTodasInscripciones(); cmbMateria_SelectedIndexChanged(null!, EventArgs.Empty); }
        }
    }
}
