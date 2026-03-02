using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class ReporteForm : Form
    {
        private GestorAcademico Gestor => MainForm.Gestor;
        private readonly ArchivosService _archivos = new();

        public ReporteForm()
        {
            InitializeComponent();
            CargarCombos();
        }

        private void CargarCombos()
        {
            cmbEstudianteHistorial.Items.Clear();
            cmbEstudianteHistorial.Items.Add("-- Seleccionar --");
            foreach (var e in Gestor.Estudiantes.OrderBy(e => e.Nombre))
                cmbEstudianteHistorial.Items.Add(e);
            cmbEstudianteHistorial.DisplayMember = "Nombre";
            cmbEstudianteHistorial.SelectedIndex = 0;

            cmbMateriaReporte.Items.Clear();
            cmbMateriaReporte.Items.Add("-- Seleccionar --");
            foreach (var m in Gestor.Materias.OrderBy(m => m.Nombre))
                cmbMateriaReporte.Items.Add(m);
            cmbMateriaReporte.DisplayMember = "Nombre";
            cmbMateriaReporte.SelectedIndex = 0;
        }

        // ── Tab 1: Historial ───────────────────────────────
        private void btnGenerarHistorial_Click(object sender, EventArgs e)
        {
            if (cmbEstudianteHistorial.SelectedItem is not Estudiante est)
            { MessageBox.Show("Seleccione un estudiante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            rtbHistorial.Text = Gestor.GenerarHistorialEstudiante(est.Id);
        }

        private void btnExportarHistorial_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbHistorial.Text)) { MessageBox.Show("Genere el reporte primero."); return; }
            ExportarTxt(rtbHistorial.Text, "historial_estudiante");
        }

        // ── Tab 2: Inscritos por materia ───────────────────
        private void btnGenerarInscritos_Click(object sender, EventArgs e)
        {
            if (cmbMateriaReporte.SelectedItem is not Materia mat)
            { MessageBox.Show("Seleccione una materia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            rtbInscritos.Text = Gestor.GenerarReporteInscritosPorMateria(mat.Codigo);

            dgvInscritos.Rows.Clear();
            foreach (var ins in Gestor.ObtenerInscritosPorMateria(mat.Codigo).OrderBy(i => i.Estudiante.Nombre))
            {
                int idx = dgvInscritos.Rows.Add(
                    ins.Estudiante.Nombre, ins.Estudiante.Id,
                    ins.Calificacion?.Nota1.ToString("F1") ?? "-",
                    ins.Calificacion?.Nota2.ToString("F1") ?? "-",
                    ins.Calificacion?.Nota3.ToString("F1") ?? "-",
                    ins.Calificacion?.PromedioFinal.ToString("F2") ?? "-",
                    ins.Calificacion?.EstadoAcademico ?? "Sin calificar");
                if (ins.Calificacion != null)
                    dgvInscritos.Rows[idx].DefaultCellStyle.ForeColor =
                        ins.Calificacion.EstadoAcademico == "Aprobado"
                        ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
            }
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            if (cmbMateriaReporte.SelectedItem is not Materia mat)
            { MessageBox.Show("Seleccione y genere el reporte primero."); return; }
            using var dlg = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"inscritos_{mat.Codigo}_{DateTime.Now:yyyyMMdd}"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            var (ok, msg) = _archivos.ExportarInscritosCsv(dlg.FileName, Gestor.ObtenerInscritosPorMateria(mat.Codigo));
            MessageBox.Show(msg, ok ? "Exportado" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        // ── Tab 3: Ranking ────────────────────────────────
        private void btnGenerarRanking_Click(object sender, EventArgs e)
        {
            if (cmbSemestreRanking.SelectedItem is int sem)
                rtbRanking.Text = Gestor.GenerarRankingPorSemestre(sem);
        }

        private void btnExportarRanking_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbRanking.Text)) { MessageBox.Show("Genere el reporte primero."); return; }
            ExportarTxt(rtbRanking.Text, "ranking_semestre");
        }

        private void ExportarTxt(string contenido, string nombreBase)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "Texto (*.txt)|*.txt",
                FileName = $"{nombreBase}_{DateTime.Now:yyyyMMdd}"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            var (ok, msg) = _archivos.ExportarReporteTxt(dlg.FileName, contenido);
            MessageBox.Show(msg, ok ? "Exportado" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }
    }
}
