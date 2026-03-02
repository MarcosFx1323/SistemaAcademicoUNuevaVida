using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class CalificacionForm : Form
    {
        private Inscripcion? _inscripcionActual;
        private GestorAcademico Gestor => MainForm.Gestor;

        public CalificacionForm()
        {
            InitializeComponent();
            CargarCombos();
        }

        private void CargarCombos()
        {
            cmbEstudiante.Items.Clear();
            cmbEstudiante.Items.Add("-- Seleccionar estudiante --");
            foreach (var e in Gestor.Estudiantes.OrderBy(e => e.Nombre)) cmbEstudiante.Items.Add(e);
            cmbEstudiante.DisplayMember = "Nombre";
            cmbEstudiante.SelectedIndex = 0;
        }

        private void cmbEstudiante_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMateria.Items.Clear();
            cmbMateria.Items.Add("-- Seleccionar materia inscrita --");
            _inscripcionActual = null;
            LimpiarNotas();

            if (cmbEstudiante.SelectedItem is not Estudiante est) return;

            foreach (var ins in est.InscripcionesActivas) cmbMateria.Items.Add(ins.Materia);
            cmbMateria.DisplayMember = "Nombre";
            cmbMateria.SelectedIndex = 0;
            CargarHistorial(est);
            lblPromedioGral.Text = Gestor.CalcularPromedioEstudiante(est.Id).ToString("F2");
        }

        private void cmbMateria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstudiante.SelectedItem is not Estudiante est) return;
            if (cmbMateria.SelectedItem is not Materia mat) { _inscripcionActual = null; return; }

            _inscripcionActual = est.Inscripciones.FirstOrDefault(i =>
                i.Materia.Codigo == mat.Codigo && i.Estado == EstadoInscripcion.Activa);
            if (_inscripcionActual == null) return;

            lblInfoInscripcion.Text = $"Inscrito el: {_inscripcionActual.FechaInscripcion:dd/MM/yyyy}  |  Docente: {mat.DocenteResponsable?.Nombre ?? "Sin asignar"}";

            if (_inscripcionActual.Calificacion != null)
            {
                txtNota1.Text = _inscripcionActual.Calificacion.Nota1.ToString("F1");
                txtNota2.Text = _inscripcionActual.Calificacion.Nota2.ToString("F1");
                txtNota3.Text = _inscripcionActual.Calificacion.Nota3.ToString("F1");
            }
            else LimpiarNotas();
        }

        private void txtNota_TextChanged(object sender, EventArgs e) => ActualizarPromedioEnVivo();

        private void txtNota_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not TextBox txt || string.IsNullOrWhiteSpace(txt.Text)) return;
            if (!Parsear(txt.Text, out double v) || v < 0 || v > 5)
            {
                txt.BackColor = Color.FromArgb(255, 200, 200);
                MessageBox.Show("La nota debe ser un número entre 0.0 y 5.0", "Nota inválida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else txt.BackColor = Color.White;
        }

        private void ActualizarPromedioEnVivo()
        {
            if (!Parsear(txtNota1.Text, out double n1) ||
                !Parsear(txtNota2.Text, out double n2) ||
                !Parsear(txtNota3.Text, out double n3))
            {
                lblPromedio.Text = "—"; lblEstado.Text = "—";
                pnlEstado.BackColor = Color.LightGray; return;
            }
            double prom = Math.Round((n1 + n2 + n3) / 3.0, 2);
            lblPromedio.Text = prom.ToString("F2");
            lblEstado.Text = prom >= 3.0 ? "Aprobado" : "Reprobado";
            lblEstado.ForeColor = Color.White;
            pnlEstado.BackColor = prom >= 3.0 ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_inscripcionActual == null)
            { MessageBox.Show("Seleccione un estudiante y una materia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!Parsear(txtNota1.Text, out double n1) || !Parsear(txtNota2.Text, out double n2) || !Parsear(txtNota3.Text, out double n3))
            { MessageBox.Show("Ingrese las tres notas válidas (0.0 – 5.0).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var (ok, msg) = Gestor.RegistrarCalificacion(_inscripcionActual, n1, n2, n3);
            MessageBox.Show(msg, ok ? "Guardado" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok && cmbEstudiante.SelectedItem is Estudiante est)
            {
                CargarHistorial(est);
                lblPromedioGral.Text = Gestor.CalcularPromedioEstudiante(est.Id).ToString("F2");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarNotas();

        private void CargarHistorial(Estudiante est)
        {
            dgvCalificaciones.Rows.Clear();
            foreach (var c in Gestor.ObtenerCalificacionesPorEstudiante(est.Id))
            {
                int idx = dgvCalificaciones.Rows.Add(
                    c.Inscripcion.Materia.Nombre,
                    c.Nota1.ToString("F1"), c.Nota2.ToString("F1"), c.Nota3.ToString("F1"),
                    c.PromedioFinal.ToString("F2"), c.EstadoAcademico);
                dgvCalificaciones.Rows[idx].DefaultCellStyle.ForeColor =
                    c.EstadoAcademico == "Aprobado" ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
            }
        }

        private void LimpiarNotas()
        {
            txtNota1.Text = txtNota2.Text = txtNota3.Text = "";
            txtNota1.BackColor = txtNota2.BackColor = txtNota3.BackColor = Color.White;
            lblPromedio.Text = "—"; lblEstado.Text = "—";
            pnlEstado.BackColor = Color.LightGray;
            lblInfoInscripcion.Text = "";
        }

        private static bool Parsear(string txt, out double val) =>
            double.TryParse(txt.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out val) && val >= 0 && val <= 5;
    }
}
