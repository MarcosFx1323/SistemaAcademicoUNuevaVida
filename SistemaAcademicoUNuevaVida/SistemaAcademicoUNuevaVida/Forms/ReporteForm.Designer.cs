namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class ReporteForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl             = new TabControl();
            tabHistorial           = new TabPage();
            tabInscritos           = new TabPage();
            tabRanking             = new TabPage();
            // Tab 1
            pnlTopHistorial        = new Panel();
            cmbEstudianteHistorial = new ComboBox();
            btnGenerarHistorial    = new Button();
            btnExportarHistorial   = new Button();
            rtbHistorial           = new RichTextBox();
            // Tab 2
            pnlTopInscritos        = new Panel();
            splitInscritos         = new SplitContainer();
            cmbMateriaReporte      = new ComboBox();
            btnGenerarInscritos    = new Button();
            btnExportarCsv         = new Button();
            dgvInscritos           = new DataGridView();
            rtbInscritos           = new RichTextBox();
            // Tab 3
            pnlTopRanking          = new Panel();
            cmbSemestreRanking     = new ComboBox();
            btnGenerarRanking      = new Button();
            btnExportarRanking     = new Button();
            rtbRanking             = new RichTextBox();

            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInscritos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitInscritos).BeginInit();
            splitInscritos.Panel1.SuspendLayout();
            splitInscritos.Panel2.SuspendLayout();

            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10f);
            tabControl.Controls.AddRange(new TabPage[] { tabHistorial, tabInscritos, tabRanking });

            // ── TAB 1 HISTORIAL ───────────────────────────
            tabHistorial.Text = "📄  Historial del Estudiante";
            tabHistorial.Padding = new Padding(15);

            pnlTopHistorial.Dock = DockStyle.Top; pnlTopHistorial.Height = 50;
            SL(new Label { Text = "Estudiante:", Location = new Point(0, 15) }, pnlTopHistorial);
            cmbEstudianteHistorial.Location = new Point(100, 12); cmbEstudianteHistorial.Width = 300;
            cmbEstudianteHistorial.Font = new Font("Segoe UI", 10f); cmbEstudianteHistorial.DropDownStyle = ComboBoxStyle.DropDownList;
            CB(btnGenerarHistorial,  "📄  Generar",       Color.FromArgb(30, 60, 114), new Point(420, 10), 130);
            CB(btnExportarHistorial, "💾  Exportar .txt", Color.FromArgb(40, 167, 69), new Point(560, 10), 150);
            btnGenerarHistorial.Click  += btnGenerarHistorial_Click;
            btnExportarHistorial.Click += btnExportarHistorial_Click;
            pnlTopHistorial.Controls.AddRange(new Control[] { cmbEstudianteHistorial, btnGenerarHistorial, btnExportarHistorial });

            rtbHistorial.Dock = DockStyle.Fill; rtbHistorial.ReadOnly = true;
            rtbHistorial.BackColor = Color.White; rtbHistorial.Font = new Font("Consolas", 10f);
            rtbHistorial.BorderStyle = BorderStyle.None;

            tabHistorial.Controls.Add(rtbHistorial);
            tabHistorial.Controls.Add(pnlTopHistorial);

            // ── TAB 2 INSCRITOS ───────────────────────────
            tabInscritos.Text = "📋  Inscritos por Materia";
            tabInscritos.Padding = new Padding(15);

            pnlTopInscritos.Dock = DockStyle.Top; pnlTopInscritos.Height = 50;
            SL(new Label { Text = "Materia:", Location = new Point(0, 15) }, pnlTopInscritos);
            cmbMateriaReporte.Location = new Point(80, 12); cmbMateriaReporte.Width = 350;
            cmbMateriaReporte.Font = new Font("Segoe UI", 10f); cmbMateriaReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            CB(btnGenerarInscritos, "📋  Generar",       Color.FromArgb(30, 60, 114), new Point(450, 10), 130);
            CB(btnExportarCsv,      "📊  Exportar .csv", Color.FromArgb(23, 162, 184), new Point(590, 10), 150);
            btnGenerarInscritos.Click += btnGenerarInscritos_Click;
            btnExportarCsv.Click      += btnExportarCsv_Click;
            pnlTopInscritos.Controls.AddRange(new Control[] { cmbMateriaReporte, btnGenerarInscritos, btnExportarCsv });

            splitInscritos.Dock = DockStyle.Fill;
            splitInscritos.Orientation = Orientation.Horizontal;
            splitInscritos.SplitterDistance = 280;

            // DataGridView en panel superior del split
            EstilarDgv(dgvInscritos);
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColNombre",  HeaderText = "Estudiante" });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColId",      HeaderText = "Cédula",    Width = 110 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColN1",      HeaderText = "Nota 1",    Width = 70 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColN2",      HeaderText = "Nota 2",    Width = 70 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColN3",      HeaderText = "Nota 3",    Width = 70 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColProm",    HeaderText = "Promedio",  Width = 90 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "rColEstado",  HeaderText = "Estado",    Width = 100 });
            splitInscritos.Panel1.Controls.Add(dgvInscritos);

            rtbInscritos.Dock = DockStyle.Fill; rtbInscritos.ReadOnly = true;
            rtbInscritos.BackColor = Color.White; rtbInscritos.Font = new Font("Consolas", 10f);
            rtbInscritos.BorderStyle = BorderStyle.None;
            splitInscritos.Panel2.Controls.Add(rtbInscritos);

            tabInscritos.Controls.Add(splitInscritos);
            tabInscritos.Controls.Add(pnlTopInscritos);

            // ── TAB 3 RANKING ─────────────────────────────
            tabRanking.Text = "🏆  Ranking por Semestre";
            tabRanking.Padding = new Padding(15);

            pnlTopRanking.Dock = DockStyle.Top; pnlTopRanking.Height = 50;
            SL(new Label { Text = "Semestre:", Location = new Point(0, 15) }, pnlTopRanking);
            cmbSemestreRanking.Location = new Point(90, 12); cmbSemestreRanking.Width = 100;
            cmbSemestreRanking.Font = new Font("Segoe UI", 10f); cmbSemestreRanking.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 1; i <= 10; i++) cmbSemestreRanking.Items.Add(i);
            cmbSemestreRanking.SelectedIndex = 0;
            CB(btnGenerarRanking,  "🏆  Generar",       Color.FromArgb(30, 60, 114), new Point(210, 10), 130);
            CB(btnExportarRanking, "💾  Exportar .txt", Color.FromArgb(40, 167, 69), new Point(350, 10), 150);
            btnGenerarRanking.Click  += btnGenerarRanking_Click;
            btnExportarRanking.Click += btnExportarRanking_Click;
            pnlTopRanking.Controls.AddRange(new Control[] { cmbSemestreRanking, btnGenerarRanking, btnExportarRanking });

            rtbRanking.Dock = DockStyle.Fill; rtbRanking.ReadOnly = true;
            rtbRanking.BackColor = Color.White; rtbRanking.Font = new Font("Consolas", 10f);
            rtbRanking.BorderStyle = BorderStyle.None;
            tabRanking.Controls.Add(rtbRanking);
            tabRanking.Controls.Add(pnlTopRanking);

            Text = "Reportes Académicos";
            Size = new Size(950, 650); MinimumSize = new Size(850, 540);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f); BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(tabControl);

            ((System.ComponentModel.ISupportInitialize)dgvInscritos).EndInit();
            splitInscritos.Panel1.ResumeLayout(false);
            splitInscritos.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitInscritos).EndInit();
            ResumeLayout(false);
        }

        private static void SL(Label l, Control parent)
        { l.AutoSize = true; l.Font = new Font("Segoe UI", 10f, FontStyle.Bold); parent.Controls.Add(l); }
        private static void CB(Button b, string t, Color c, Point p, int w)
        { b.Text = t; b.Location = p; b.Size = new Size(w, 36); b.BackColor = c;
          b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
          b.Font = new Font("Segoe UI", 9.5f); }
        private static void EstilarDgv(DataGridView d)
        { d.Dock = DockStyle.Fill; d.ReadOnly = true; d.AllowUserToAddRows = false;
          d.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
          d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
          d.BackgroundColor = Color.White; d.BorderStyle = BorderStyle.None;
          d.RowHeadersVisible = false; d.Font = new Font("Segoe UI", 9.5f);
          d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
          d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
          d.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
          d.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
          d.EnableHeadersVisualStyles = false; }

        private TabControl tabControl;
        private TabPage tabHistorial, tabInscritos, tabRanking;
        private Panel pnlTopHistorial, pnlTopInscritos, pnlTopRanking;
        private SplitContainer splitInscritos;
        private ComboBox cmbEstudianteHistorial, cmbMateriaReporte, cmbSemestreRanking;
        private Button btnGenerarHistorial, btnExportarHistorial;
        private Button btnGenerarInscritos, btnExportarCsv;
        private Button btnGenerarRanking, btnExportarRanking;
        private RichTextBox rtbHistorial, rtbInscritos, rtbRanking;
        private DataGridView dgvInscritos;
    }
}
