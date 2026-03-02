namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class MateriaForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlForm    = new Panel();
            pnlGrid    = new Panel();
            pnlBorde   = new Panel();
            pnlBotones = new Panel();
            pnlTools   = new Panel();
            lblTitulo  = new Label();
            lblContador = new Label();
            lblCodigo   = new Label();
            lblNombreM  = new Label();
            lblSemestreM= new Label();
            lblCreditos = new Label();
            lblCupo     = new Label();
            lblDocenteM = new Label();
            lblBuscarTxt= new Label();
            lblFiltroSem= new Label();
            txtCodigo   = new TextBox();
            txtNombre   = new TextBox();
            txtBuscar   = new TextBox();
            nudSemestre = new NumericUpDown();
            nudCreditos = new NumericUpDown();
            nudCupo     = new NumericUpDown();
            cmbDocente  = new ComboBox();
            cmbFiltroSemestre = new ComboBox();
            btnGuardar  = new Button();
            btnNuevo    = new Button();
            btnEliminar = new Button();
            dgvMaterias = new DataGridView();

            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSemestre).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCreditos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCupo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMaterias).BeginInit();

            // ── pnlForm ───────────────────────────────────
            pnlForm.Width = 320; pnlForm.Dock = DockStyle.Left;
            pnlForm.BackColor = Color.White; pnlForm.Padding = new Padding(15);
            pnlBorde.Dock = DockStyle.Top; pnlBorde.Height = 4;
            pnlBorde.BackColor = Color.FromArgb(30, 60, 114);
            lblTitulo.Text = "Nueva Materia";
            lblTitulo.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 60, 114);
            lblTitulo.Dock = DockStyle.Top; lblTitulo.Height = 35;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            int x = 15, y = 100, gap = 52, lh = 18, cw = 278;

            SetLabel(lblCodigo,    "Código *",              x, y);
            SetTxt(txtCodigo,       x, y + lh, cw); txtCodigo.PlaceholderText = "Ej: MAT101";

            SetLabel(lblNombreM,   "Nombre de la materia *",x, y + gap);
            SetTxt(txtNombre,       x, y + gap + lh, cw);

            SetLabel(lblSemestreM, "Semestre *",            x, y + gap * 2);
            nudSemestre.Location = new Point(x, y + gap * 2 + lh); nudSemestre.Width = 90;
            nudSemestre.Minimum = 1; nudSemestre.Maximum = 10; nudSemestre.Value = 1;
            nudSemestre.Font = new Font("Segoe UI", 10f);

            SetLabel(lblCreditos,  "Créditos *",            x, y + gap * 3);
            nudCreditos.Location = new Point(x, y + gap * 3 + lh); nudCreditos.Width = 90;
            nudCreditos.Minimum = 1; nudCreditos.Maximum = 10; nudCreditos.Value = 3;
            nudCreditos.Font = new Font("Segoe UI", 10f);

            SetLabel(lblCupo,      "Cupo máximo *",         x, y + gap * 4);
            nudCupo.Location = new Point(x, y + gap * 4 + lh); nudCupo.Width = 90;
            nudCupo.Minimum = 1; nudCupo.Maximum = 200; nudCupo.Value = 30;
            nudCupo.Font = new Font("Segoe UI", 10f);

            SetLabel(lblDocenteM,  "Docente responsable",   x, y + gap * 5);
            cmbDocente.Location = new Point(x, y + gap * 5 + lh); cmbDocente.Width = cw;
            cmbDocente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocente.Font = new Font("Segoe UI", 10f);

            // Botones
            pnlBotones.Height = 50; pnlBotones.Dock = DockStyle.Bottom;
            CB(btnGuardar,  "💾 Guardar",  Color.FromArgb(30, 60, 114), new Point(0,   8), 120);
            CB(btnNuevo,    "➕ Nuevo",    Color.FromArgb(40, 167, 69), new Point(128, 8), 80);
            CB(btnEliminar, "🗑 Eliminar", Color.FromArgb(220, 53, 69), new Point(216, 8), 90);
            btnEliminar.Enabled = false;
            btnGuardar.Click  += btnGuardar_Click;
            btnNuevo.Click    += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            pnlBotones.Controls.AddRange(new Control[] { btnGuardar, btnNuevo, btnEliminar });

            pnlForm.Controls.AddRange(new Control[] {
                pnlBotones, lblCodigo, txtCodigo, lblNombreM, txtNombre,
                lblSemestreM, nudSemestre, lblCreditos, nudCreditos,
                lblCupo, nudCupo, lblDocenteM, cmbDocente, lblTitulo, pnlBorde
            });

            // ── pnlGrid ───────────────────────────────────
            pnlGrid.Dock = DockStyle.Fill; pnlGrid.Padding = new Padding(10);

            pnlTools.Height = 45; pnlTools.Dock = DockStyle.Top;
            SetLabel(lblBuscarTxt, "🔍 Buscar:", 0, 13);
            SetTxt(txtBuscar, 75, 10, 200); txtBuscar.PlaceholderText = "Código o nombre...";
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            SetLabel(lblFiltroSem, "Semestre:", 290, 13);
            cmbFiltroSemestre.Location = new Point(360, 10); cmbFiltroSemestre.Width = 80;
            cmbFiltroSemestre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroSemestre.Font = new Font("Segoe UI", 10f);
            cmbFiltroSemestre.Items.Add("Todos");
            for (int i = 1; i <= 10; i++) cmbFiltroSemestre.Items.Add(i.ToString());
            cmbFiltroSemestre.SelectedIndex = 0;
            cmbFiltroSemestre.SelectedIndexChanged += cmbFiltroSemestre_SelectedIndexChanged;
            pnlTools.Controls.AddRange(new Control[] { lblBuscarTxt, txtBuscar, lblFiltroSem, cmbFiltroSemestre });

            dgvMaterias.Dock = DockStyle.Fill; dgvMaterias.ReadOnly = true;
            dgvMaterias.AllowUserToAddRows = false;
            dgvMaterias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaterias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMaterias.BackgroundColor = Color.White; dgvMaterias.BorderStyle = BorderStyle.None;
            dgvMaterias.RowHeadersVisible = false; dgvMaterias.MultiSelect = false;
            dgvMaterias.Font = new Font("Segoe UI", 9.5f);
            dgvMaterias.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgvMaterias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMaterias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvMaterias.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgvMaterias.EnableHeadersVisualStyles = false;
            dgvMaterias.SelectionChanged += dgvMaterias_SelectionChanged;
            dgvMaterias.Columns.Add(new DataGridViewTextBoxColumn { Name = "mColCodigo",   HeaderText = "Código",   Width = 90 });
            dgvMaterias.Columns.Add(new DataGridViewTextBoxColumn { Name = "mColNombre",   HeaderText = "Materia" });
            dgvMaterias.Columns.Add(new DataGridViewTextBoxColumn { Name = "mColSemestre", HeaderText = "Sem.",    Width = 50 });
            dgvMaterias.Columns.Add(new DataGridViewTextBoxColumn { Name = "mColCreditos", HeaderText = "Créd.",   Width = 55 });
            dgvMaterias.Columns.Add(new DataGridViewTextBoxColumn { Name = "mColCupo",     HeaderText = "Cupo",    Width = 80 });
            dgvMaterias.Columns.Add(new DataGridViewTextBoxColumn { Name = "mColDocente",  HeaderText = "Docente" });

            lblContador.Dock = DockStyle.Bottom; lblContador.Height = 25;
            lblContador.TextAlign = ContentAlignment.MiddleRight;
            lblContador.ForeColor = Color.Gray; lblContador.Font = new Font("Segoe UI", 9f);

            pnlGrid.Controls.Add(dgvMaterias);
            pnlGrid.Controls.Add(pnlTools);
            pnlGrid.Controls.Add(lblContador);

            Text = "Gestión de Materias"; Size = new Size(1000, 650);
            MinimumSize = new Size(860, 540); StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f); BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlGrid); Controls.Add(pnlForm);

            ((System.ComponentModel.ISupportInitialize)nudSemestre).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCreditos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCupo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMaterias).EndInit();
            ResumeLayout(false);
        }

        private static void SetLabel(Label l, string t, int x, int y)
        { l.Text = t; l.AutoSize = true; l.Location = new Point(x, y); l.Font = new Font("Segoe UI", 9f); }
        private static void SetTxt(TextBox t, int x, int y, int w)
        { t.Location = new Point(x, y); t.Width = w; t.Font = new Font("Segoe UI", 10f); }
        private static void CB(Button b, string txt, Color c, Point p, int w)
        { b.Text = txt; b.Location = p; b.Size = new Size(w, 36); b.BackColor = c;
          b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
          b.Font = new Font("Segoe UI", 9.5f); }

        private Panel pnlForm, pnlGrid, pnlBorde, pnlBotones, pnlTools;
        private Label lblTitulo, lblContador, lblCodigo, lblNombreM;
        private Label lblSemestreM, lblCreditos, lblCupo, lblDocenteM;
        private Label lblBuscarTxt, lblFiltroSem;
        private TextBox txtCodigo, txtNombre, txtBuscar;
        private NumericUpDown nudSemestre, nudCreditos, nudCupo;
        private ComboBox cmbDocente, cmbFiltroSemestre;
        private Button btnGuardar, btnNuevo, btnEliminar;
        private DataGridView dgvMaterias;
    }
}
