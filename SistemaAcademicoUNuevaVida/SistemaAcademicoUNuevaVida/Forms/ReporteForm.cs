using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Módulo 5: Generación de reportes.
    /// Historial de estudiante, inscritos por materia, ranking por semestre.
    /// Exportación como .txt y .csv.
    /// </summary>
    public class ReporteForm : Form
    {
        private TabControl tabControl = null!;

        // Tab 1 – Historial
        private ComboBox cmbEstudianteHistorial = null!;
        private RichTextBox rtbHistorial = null!;
        private Button btnGenerarHistorial = null!, btnExportarHistorial = null!;

        // Tab 2 – Inscritos por materia
        private ComboBox cmbMateriaReporte = null!;
        private DataGridView dgvInscritos = null!;
        private RichTextBox rtbInscritos = null!;
        private Button btnGenerarInscritos = null!, btnExportarCsv = null!;

        // Tab 3 – Ranking
        private ComboBox cmbSemestreRanking = null!;
        private RichTextBox rtbRanking = null!;
        private Button btnGenerarRanking = null!, btnExportarRanking = null!;

        private GestorAcademico Gestor => MainForm.Gestor;
        private readonly ArchivosService _archivos = new();

        public ReporteForm()
        {
            InicializarComponentes();
            CargarCombos();
        }

        private void InicializarComponentes()
        {
            Text = "Reportes Académicos";
            Size = new Size(950, 650);
            MinimumSize = new Size(850, 540);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);

            tabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f) };

            tabControl.TabPages.Add(CrearTabHistorial());
            tabControl.TabPages.Add(CrearTabInscritos());
            tabControl.TabPages.Add(CrearTabRanking());

            Controls.Add(tabControl);
        }

        // ─────────────────────────────────────────────────────
        //  TAB 1 – HISTORIAL DEL ESTUDIANTE
        // ─────────────────────────────────────────────────────
        private TabPage CrearTabHistorial()
        {
            var tab = new TabPage("📄  Historial del Estudiante");
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };

            var pnlTop = new Panel { Height = 50, Dock = DockStyle.Top };
            pnlTop.Controls.Add(new Label { Text = "Estudiante:", AutoSize = true, Location = new Point(0, 15), Font = new Font("Segoe UI", 10f, FontStyle.Bold) });
            cmbEstudianteHistorial = new ComboBox { Location = new Point(100, 12), Width = 300, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };

            btnGenerarHistorial = CrearBoton("📄  Generar", Color.FromArgb(30, 60, 114), new Point(420, 10), 130);
            btnGenerarHistorial.Click += (s, e) => GenerarHistorial();

            btnExportarHistorial = CrearBoton("💾  Exportar .txt", Color.FromArgb(40, 167, 69), new Point(560, 10), 150);
            btnExportarHistorial.Click += ExportarHistorialTxt;

            pnlTop.Controls.AddRange(new Control[] { cmbEstudianteHistorial, btnGenerarHistorial, btnExportarHistorial });

            rtbHistorial = new RichTextBox
            {
                Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.White,
                Font = new Font("Consolas", 10f), BorderStyle = BorderStyle.None
            };

            panel.Controls.Add(rtbHistorial);
            panel.Controls.Add(pnlTop);
            tab.Controls.Add(panel);
            return tab;
        }

        // ─────────────────────────────────────────────────────
        //  TAB 2 – INSCRITOS POR MATERIA
        // ─────────────────────────────────────────────────────
        private TabPage CrearTabInscritos()
        {
            var tab = new TabPage("📋  Inscritos por Materia");
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };

            var pnlTop = new Panel { Height = 50, Dock = DockStyle.Top };
            pnlTop.Controls.Add(new Label { Text = "Materia:", AutoSize = true, Location = new Point(0, 15), Font = new Font("Segoe UI", 10f, FontStyle.Bold) });
            cmbMateriaReporte = new ComboBox { Location = new Point(80, 12), Width = 350, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };

            btnGenerarInscritos = CrearBoton("📋  Generar", Color.FromArgb(30, 60, 114), new Point(450, 10), 130);
            btnGenerarInscritos.Click += (s, e) => GenerarInscritos();

            btnExportarCsv = CrearBoton("📊  Exportar .csv", Color.FromArgb(23, 162, 184), new Point(590, 10), 150);
            btnExportarCsv.Click += ExportarInscritosCsv;

            pnlTop.Controls.AddRange(new Control[] { cmbMateriaReporte, btnGenerarInscritos, btnExportarCsv });

            // Split: grilla arriba, texto abajo
            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 300 };

            dgvInscritos = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, Font = new Font("Segoe UI", 9.5f)
            };
            EstilarDgv(dgvInscritos);
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Estudiante" });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Cédula", Width = 110 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nota1", HeaderText = "Nota 1", Width = 75 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nota2", HeaderText = "Nota 2", Width = 75 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nota3", HeaderText = "Nota 3", Width = 75 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Promedio", HeaderText = "Promedio", Width = 90 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", Width = 100 });

            rtbInscritos = new RichTextBox
            {
                Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.White,
                Font = new Font("Consolas", 10f), BorderStyle = BorderStyle.None
            };

            split.Panel1.Controls.Add(dgvInscritos);
            split.Panel2.Controls.Add(rtbInscritos);

            panel.Controls.Add(split);
            panel.Controls.Add(pnlTop);
            tab.Controls.Add(panel);
            return tab;
        }

        // ─────────────────────────────────────────────────────
        //  TAB 3 – RANKING POR SEMESTRE
        // ─────────────────────────────────────────────────────
        private TabPage CrearTabRanking()
        {
            var tab = new TabPage("🏆  Ranking por Semestre");
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };

            var pnlTop = new Panel { Height = 50, Dock = DockStyle.Top };
            pnlTop.Controls.Add(new Label { Text = "Semestre:", AutoSize = true, Location = new Point(0, 15), Font = new Font("Segoe UI", 10f, FontStyle.Bold) });
            cmbSemestreRanking = new ComboBox { Location = new Point(90, 12), Width = 100, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };
            for (int i = 1; i <= 10; i++) cmbSemestreRanking.Items.Add(i);
            cmbSemestreRanking.SelectedIndex = 0;

            btnGenerarRanking = CrearBoton("🏆  Generar", Color.FromArgb(30, 60, 114), new Point(210, 10), 130);
            btnGenerarRanking.Click += (s, e) => GenerarRanking();

            btnExportarRanking = CrearBoton("💾  Exportar .txt", Color.FromArgb(40, 167, 69), new Point(350, 10), 150);
            btnExportarRanking.Click += ExportarRankingTxt;

            pnlTop.Controls.AddRange(new Control[] { cmbSemestreRanking, btnGenerarRanking, btnExportarRanking });

            rtbRanking = new RichTextBox
            {
                Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.White,
                Font = new Font("Consolas", 10f), BorderStyle = BorderStyle.None
            };

            panel.Controls.Add(rtbRanking);
            panel.Controls.Add(pnlTop);
            tab.Controls.Add(panel);
            return tab;
        }

        // ═══════════════════════════════════════════════════
        //  LÓGICA – GENERACIÓN
        // ═══════════════════════════════════════════════════

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

        private void GenerarHistorial()
        {
            if (cmbEstudianteHistorial.SelectedItem is not Estudiante est)
            { MessageBox.Show("Seleccione un estudiante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            rtbHistorial.Text = Gestor.GenerarHistorialEstudiante(est.Id);
        }

        private void GenerarInscritos()
        {
            if (cmbMateriaReporte.SelectedItem is not Materia mat)
            { MessageBox.Show("Seleccione una materia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            // Texto
            rtbInscritos.Text = Gestor.GenerarReporteInscritosPorMateria(mat.Codigo);

            // Grilla
            dgvInscritos.Rows.Clear();
            var inscritos = Gestor.ObtenerInscritosPorMateria(mat.Codigo);
            foreach (var ins in inscritos.OrderBy(i => i.Estudiante.Nombre))
            {
                string n1 = ins.Calificacion?.Nota1.ToString("F1") ?? "-";
                string n2 = ins.Calificacion?.Nota2.ToString("F1") ?? "-";
                string n3 = ins.Calificacion?.Nota3.ToString("F1") ?? "-";
                string prom = ins.Calificacion?.PromedioFinal.ToString("F2") ?? "-";
                string estado = ins.Calificacion?.EstadoAcademico ?? "Sin calificar";

                int idx = dgvInscritos.Rows.Add(ins.Estudiante.Nombre, ins.Estudiante.Id, n1, n2, n3, prom, estado);
                if (ins.Calificacion != null)
                    dgvInscritos.Rows[idx].DefaultCellStyle.ForeColor =
                        ins.Calificacion.EstadoAcademico == "Aprobado"
                        ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
            }
        }

        private void GenerarRanking()
        {
            if (cmbSemestreRanking.SelectedItem is not int sem) return;
            rtbRanking.Text = Gestor.GenerarRankingPorSemestre(sem);
        }

        // ═══════════════════════════════════════════════════
        //  LÓGICA – EXPORTACIÓN
        // ═══════════════════════════════════════════════════

        private void ExportarHistorialTxt(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbHistorial.Text)) { MessageBox.Show("Genere el reporte primero."); return; }
            ExportarTxt(rtbHistorial.Text, "historial_estudiante");
        }

        private void ExportarRankingTxt(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbRanking.Text)) { MessageBox.Show("Genere el reporte primero."); return; }
            ExportarTxt(rtbRanking.Text, "ranking_semestre");
        }

        private void ExportarInscritosCsv(object? sender, EventArgs e)
        {
            if (cmbMateriaReporte.SelectedItem is not Materia mat)
            { MessageBox.Show("Seleccione y genere el reporte primero."); return; }

            using var dialog = new SaveFileDialog
            {
                Title = "Exportar inscritos como CSV",
                Filter = "Archivos CSV (*.csv)|*.csv",
                FileName = $"inscritos_{mat.Codigo}_{DateTime.Now:yyyyMMdd}"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            var inscritos = Gestor.ObtenerInscritosPorMateria(mat.Codigo);
            var (ok, msg) = _archivos.ExportarInscritosCsv(dialog.FileName, inscritos);
            MessageBox.Show(msg, ok ? "Exportado" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void ExportarTxt(string contenido, string nombreBase)
        {
            using var dialog = new SaveFileDialog
            {
                Title = "Exportar reporte",
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = $"{nombreBase}_{DateTime.Now:yyyyMMdd}"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            var (ok, msg) = _archivos.ExportarReporteTxt(dialog.FileName, contenido);
            MessageBox.Show(msg, ok ? "Exportado" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private static void EstilarDgv(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgv.EnableHeadersVisualStyles = false;
        }

        private static Button CrearBoton(string texto, Color color, Point loc, int w)
        {
            var btn = new Button { Text = texto, Width = w, Height = 36, Location = loc, BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f) };
            btn.FlatAppearance.BorderSize = 0; return btn;
        }
    }
}
