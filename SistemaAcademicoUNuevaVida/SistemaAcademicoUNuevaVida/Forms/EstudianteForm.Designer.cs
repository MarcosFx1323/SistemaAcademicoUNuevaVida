namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class EstudianteForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Instanciar controles ───────────────────────
            tabControl        = new TabControl();
            tabEstudiantes    = new TabPage();
            tabDocentes       = new TabPage();
            pnlForm           = new Panel();
            pnlGrid           = new Panel();
            pnlBorde          = new Panel();
            pnlBotones        = new Panel();
            pnlBuscar         = new Panel();
            lblTitulo         = new Label();
            lblId             = new Label();
            lblNombre         = new Label();
            lblEmail          = new Label();
            lblTelefono       = new Label();
            lblPrograma       = new Label();
            lblSemestre       = new Label();
            lblFechaNac       = new Label();
            lblContador       = new Label();
            lblBuscarTxt      = new Label();
            txtId             = new TextBox();
            txtNombre         = new TextBox();
            txtEmail          = new TextBox();
            txtTelefono       = new TextBox();
            txtPrograma       = new TextBox();
            txtBuscar         = new TextBox();
            nudSemestre       = new NumericUpDown();
            dtpNacimiento     = new DateTimePicker();
            btnGuardar        = new Button();
            btnNuevo          = new Button();
            btnEliminar       = new Button();
            dgvEstudiantes    = new DataGridView();
            colId             = new DataGridViewTextBoxColumn();
            colNombre         = new DataGridViewTextBoxColumn();
            colPrograma       = new DataGridViewTextBoxColumn();
            colSemestre       = new DataGridViewTextBoxColumn();
            colEmail          = new DataGridViewTextBoxColumn();
            colPromedio       = new DataGridViewTextBoxColumn();

            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSemestre).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvEstudiantes).BeginInit();

            // ── tabControl ────────────────────────────────
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10f);
            tabControl.Controls.Add(tabEstudiantes);
            tabControl.Controls.Add(tabDocentes);

            // ── tabEstudiantes ────────────────────────────
            tabEstudiantes.Text = "👤  Estudiantes";
            tabEstudiantes.Controls.Add(pnlGrid);
            tabEstudiantes.Controls.Add(pnlForm);

            // ── tabDocentes ───────────────────────────────
            tabDocentes.Text = "🎓  Docentes";
            tabDocentes.Controls.Add(new Label
            {
                Text = "Use el menú Personas → Gestión de Docentes",
                AutoSize = true, Location = new Point(20, 20), ForeColor = Color.Gray
            });

            // ── pnlForm (izquierda) ───────────────────────
            pnlForm.Width = 320;
            pnlForm.Dock = DockStyle.Left;
            pnlForm.BackColor = Color.White;
            pnlForm.Padding = new Padding(15);

            // Borde superior azul
            pnlBorde.Dock = DockStyle.Top;
            pnlBorde.Height = 4;
            pnlBorde.BackColor = Color.FromArgb(30, 60, 114);

            // Título
            lblTitulo.Text = "Nuevo Estudiante";
            lblTitulo.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 60, 114);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 35;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // ── Campos del formulario ─────────────────────
            int startY = 100;
            int gapY   = 52;
            int lblH   = 18;
            int ctrlH  = 26;
            int ctrlX  = 15;
            int ctrlW  = 278;

            ConfigurarLabel(lblId,       "Cédula / ID *",      ctrlX, startY);
            ConfigurarTextBox(txtId,      ctrlX, startY + lblH, ctrlW);

            ConfigurarLabel(lblNombre,   "Nombre completo *",  ctrlX, startY + gapY);
            ConfigurarTextBox(txtNombre,  ctrlX, startY + gapY + lblH, ctrlW);

            ConfigurarLabel(lblEmail,    "Correo electrónico", ctrlX, startY + gapY * 2);
            ConfigurarTextBox(txtEmail,   ctrlX, startY + gapY * 2 + lblH, ctrlW);

            ConfigurarLabel(lblTelefono, "Teléfono",           ctrlX, startY + gapY * 3);
            ConfigurarTextBox(txtTelefono,ctrlX, startY + gapY * 3 + lblH, ctrlW);

            ConfigurarLabel(lblPrograma, "Programa académico *",ctrlX, startY + gapY * 4);
            ConfigurarTextBox(txtPrograma, ctrlX, startY + gapY * 4 + lblH, ctrlW);

            ConfigurarLabel(lblSemestre, "Semestre *",         ctrlX, startY + gapY * 5);
            nudSemestre.Location = new Point(ctrlX, startY + gapY * 5 + lblH);
            nudSemestre.Width = 90;
            nudSemestre.Minimum = 1;
            nudSemestre.Maximum = 10;
            nudSemestre.Value = 1;
            nudSemestre.Font = new Font("Segoe UI", 10f);

            ConfigurarLabel(lblFechaNac, "Fecha de nacimiento", ctrlX, startY + gapY * 6);
            dtpNacimiento.Location = new Point(ctrlX, startY + gapY * 6 + lblH);
            dtpNacimiento.Width = ctrlW;
            dtpNacimiento.Format = DateTimePickerFormat.Short;
            dtpNacimiento.MaxDate = DateTime.Today.AddYears(-15);

            // ── Botones ───────────────────────────────────
            pnlBotones.Height = 50;
            pnlBotones.Dock = DockStyle.Bottom;

            btnGuardar.Text = "💾 Guardar";
            btnGuardar.Location = new Point(0, 8);
            btnGuardar.Size = new Size(120, 36);
            btnGuardar.BackColor = Color.FromArgb(30, 60, 114);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Font = new Font("Segoe UI", 9.5f);
            btnGuardar.Click += btnGuardar_Click;

            btnNuevo.Text = "➕ Nuevo";
            btnNuevo.Location = new Point(128, 8);
            btnNuevo.Size = new Size(80, 36);
            btnNuevo.BackColor = Color.FromArgb(40, 167, 69);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Font = new Font("Segoe UI", 9.5f);
            btnNuevo.Click += btnNuevo_Click;

            btnEliminar.Text = "🗑 Eliminar";
            btnEliminar.Location = new Point(216, 8);
            btnEliminar.Size = new Size(90, 36);
            btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Font = new Font("Segoe UI", 9.5f);
            btnEliminar.Enabled = false;
            btnEliminar.Click += btnEliminar_Click;

            pnlBotones.Controls.AddRange(new Control[] { btnGuardar, btnNuevo, btnEliminar });

            pnlForm.Controls.Add(pnlBotones);
            pnlForm.Controls.Add(lblId);
            pnlForm.Controls.Add(txtId);
            pnlForm.Controls.Add(lblNombre);
            pnlForm.Controls.Add(txtNombre);
            pnlForm.Controls.Add(lblEmail);
            pnlForm.Controls.Add(txtEmail);
            pnlForm.Controls.Add(lblTelefono);
            pnlForm.Controls.Add(txtTelefono);
            pnlForm.Controls.Add(lblPrograma);
            pnlForm.Controls.Add(txtPrograma);
            pnlForm.Controls.Add(lblSemestre);
            pnlForm.Controls.Add(nudSemestre);
            pnlForm.Controls.Add(lblFechaNac);
            pnlForm.Controls.Add(dtpNacimiento);
            pnlForm.Controls.Add(lblTitulo);
            pnlForm.Controls.Add(pnlBorde);

            // ── pnlGrid (derecha) ─────────────────────────
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Padding = new Padding(10);

            // Barra búsqueda
            pnlBuscar.Height = 45;
            pnlBuscar.Dock = DockStyle.Top;
            lblBuscarTxt.Text = "🔍 Buscar:";
            lblBuscarTxt.AutoSize = true;
            lblBuscarTxt.Location = new Point(0, 13);
            txtBuscar.Location = new Point(75, 10);
            txtBuscar.Width = 260;
            txtBuscar.Font = new Font("Segoe UI", 10f);
            txtBuscar.PlaceholderText = "Nombre, ID o programa...";
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            pnlBuscar.Controls.AddRange(new Control[] { lblBuscarTxt, txtBuscar });

            // DataGridView
            dgvEstudiantes.Dock = DockStyle.Fill;
            dgvEstudiantes.ReadOnly = true;
            dgvEstudiantes.AllowUserToAddRows = false;
            dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstudiantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstudiantes.BackgroundColor = Color.White;
            dgvEstudiantes.BorderStyle = BorderStyle.None;
            dgvEstudiantes.RowHeadersVisible = false;
            dgvEstudiantes.MultiSelect = false;
            dgvEstudiantes.Font = new Font("Segoe UI", 9.5f);
            dgvEstudiantes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgvEstudiantes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEstudiantes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvEstudiantes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgvEstudiantes.EnableHeadersVisualStyles = false;
            dgvEstudiantes.SelectionChanged += dgvEstudiantes_SelectionChanged;

            colId.Name = "colId"; colId.HeaderText = "Cédula"; colId.Width = 100;
            colNombre.Name = "colNombre"; colNombre.HeaderText = "Nombre";
            colPrograma.Name = "colPrograma"; colPrograma.HeaderText = "Programa";
            colSemestre.Name = "colSemestre"; colSemestre.HeaderText = "Sem."; colSemestre.Width = 55;
            colEmail.Name = "colEmail"; colEmail.HeaderText = "Email";
            colPromedio.Name = "colPromedio"; colPromedio.HeaderText = "Promedio"; colPromedio.Width = 80;
            dgvEstudiantes.Columns.AddRange(new DataGridViewColumn[]
                { colId, colNombre, colPrograma, colSemestre, colEmail, colPromedio });

            lblContador.Dock = DockStyle.Bottom;
            lblContador.Height = 25;
            lblContador.TextAlign = ContentAlignment.MiddleRight;
            lblContador.ForeColor = Color.Gray;
            lblContador.Font = new Font("Segoe UI", 9f);

            pnlGrid.Controls.Add(dgvEstudiantes);
            pnlGrid.Controls.Add(pnlBuscar);
            pnlGrid.Controls.Add(lblContador);

            // ── Form ──────────────────────────────────────
            Text = "Gestión de Estudiantes";
            Size = new Size(1000, 650);
            MinimumSize = new Size(850, 550);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(tabControl);

            ((System.ComponentModel.ISupportInitialize)nudSemestre).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvEstudiantes).EndInit();
            ResumeLayout(false);
        }

        private static void ConfigurarLabel(Label lbl, string texto, int x, int y)
        {
            lbl.Text = texto; lbl.AutoSize = true; lbl.Location = new Point(x, y);
            lbl.Font = new Font("Segoe UI", 9f);
        }

        private static void ConfigurarTextBox(TextBox txt, int x, int y, int w)
        {
            txt.Location = new Point(x, y); txt.Width = w;
            txt.Font = new Font("Segoe UI", 10f);
        }

        // ── Declaración de controles ───────────────────────
        private TabControl tabControl;
        private TabPage tabEstudiantes, tabDocentes;
        private Panel pnlForm, pnlGrid, pnlBorde, pnlBotones, pnlBuscar;
        private Label lblTitulo, lblId, lblNombre, lblEmail, lblTelefono;
        private Label lblPrograma, lblSemestre, lblFechaNac, lblContador, lblBuscarTxt;
        private TextBox txtId, txtNombre, txtEmail, txtTelefono, txtPrograma, txtBuscar;
        private NumericUpDown nudSemestre;
        private DateTimePicker dtpNacimiento;
        private Button btnGuardar, btnNuevo, btnEliminar;
        private DataGridView dgvEstudiantes;
        private DataGridViewTextBoxColumn colId, colNombre, colPrograma;
        private DataGridViewTextBoxColumn colSemestre, colEmail, colPromedio;
    }
}
