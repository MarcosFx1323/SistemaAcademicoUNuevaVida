namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class DocenteForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlForm       = new Panel();
            pnlGrid       = new Panel();
            pnlBorde      = new Panel();
            pnlBotones    = new Panel();
            pnlBuscar     = new Panel();
            lblTitulo     = new Label();
            lblContador   = new Label();
            lblBuscarTxt  = new Label();
            txtId             = new TextBox();
            txtNombre         = new TextBox();
            txtEmail          = new TextBox();
            txtTelefono       = new TextBox();
            txtTitulo         = new TextBox();
            txtDepartamento   = new TextBox();
            txtBuscar         = new TextBox();
            dtpNacimiento     = new DateTimePicker();
            btnGuardar        = new Button();
            btnNuevo          = new Button();
            btnEliminar       = new Button();
            dgvDocentes       = new DataGridView();

            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocentes).BeginInit();

            // ── pnlForm ───────────────────────────────────
            pnlForm.Width = 320; pnlForm.Dock = DockStyle.Left;
            pnlForm.BackColor = Color.White; pnlForm.Padding = new Padding(15);

            pnlBorde.Dock = DockStyle.Top; pnlBorde.Height = 4;
            pnlBorde.BackColor = Color.FromArgb(30, 60, 114);

            lblTitulo.Text = "Nuevo Docente";
            lblTitulo.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 60, 114);
            lblTitulo.Dock = DockStyle.Top; lblTitulo.Height = 35;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // Campos
            int x = 15, y = 100, gap = 52, lh = 18, cw = 278;

            Label[] labels = {
                new Label(), new Label(), new Label(),
                new Label(), new Label(), new Label(), new Label()
            };
            string[] labelTexts = {
                "Cédula *", "Nombre completo *", "Correo electrónico",
                "Teléfono", "Título máximo *", "Departamento *", "Fecha de nacimiento"
            };
            TextBox[] fields = { txtId, txtNombre, txtEmail, txtTelefono, txtTitulo, txtDepartamento };

            for (int i = 0; i < 6; i++)
            {
                labels[i].Text = labelTexts[i]; labels[i].AutoSize = true;
                labels[i].Location = new Point(x, y + gap * i);
                labels[i].Font = new Font("Segoe UI", 9f);
                fields[i].Location = new Point(x, y + gap * i + lh);
                fields[i].Width = cw; fields[i].Font = new Font("Segoe UI", 10f);
                pnlForm.Controls.Add(labels[i]);
                pnlForm.Controls.Add(fields[i]);
            }
            txtTitulo.PlaceholderText = "Ej: Magíster en Ingeniería";

            labels[6].Text = labelTexts[6]; labels[6].AutoSize = true;
            labels[6].Location = new Point(x, y + gap * 6); labels[6].Font = new Font("Segoe UI", 9f);
            dtpNacimiento.Location = new Point(x, y + gap * 6 + lh);
            dtpNacimiento.Width = cw; dtpNacimiento.Format = DateTimePickerFormat.Short;
            dtpNacimiento.MaxDate = DateTime.Today.AddYears(-22);
            pnlForm.Controls.Add(labels[6]);
            pnlForm.Controls.Add(dtpNacimiento);

            // Botones
            pnlBotones.Height = 50; pnlBotones.Dock = DockStyle.Bottom;
            ConfigBoton(btnGuardar, "💾 Guardar",  Color.FromArgb(30, 60, 114), new Point(0, 8),   120);
            ConfigBoton(btnNuevo,   "➕ Nuevo",    Color.FromArgb(40, 167, 69), new Point(128, 8),  80);
            ConfigBoton(btnEliminar,"🗑 Eliminar", Color.FromArgb(220, 53, 69), new Point(216, 8),  90);
            btnEliminar.Enabled = false;
            btnGuardar.Click  += btnGuardar_Click;
            btnNuevo.Click    += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            pnlBotones.Controls.AddRange(new Control[] { btnGuardar, btnNuevo, btnEliminar });

            pnlForm.Controls.Add(pnlBotones);
            pnlForm.Controls.Add(lblTitulo);
            pnlForm.Controls.Add(pnlBorde);

            // ── pnlGrid ───────────────────────────────────
            pnlGrid.Dock = DockStyle.Fill; pnlGrid.Padding = new Padding(10);

            pnlBuscar.Height = 45; pnlBuscar.Dock = DockStyle.Top;
            lblBuscarTxt.Text = "🔍 Buscar:"; lblBuscarTxt.AutoSize = true;
            lblBuscarTxt.Location = new Point(0, 13);
            txtBuscar.Location = new Point(75, 10); txtBuscar.Width = 260;
            txtBuscar.Font = new Font("Segoe UI", 10f);
            txtBuscar.PlaceholderText = "Nombre, cédula o departamento...";
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            pnlBuscar.Controls.AddRange(new Control[] { lblBuscarTxt, txtBuscar });

            // DataGridView
            dgvDocentes.Dock = DockStyle.Fill; dgvDocentes.ReadOnly = true;
            dgvDocentes.AllowUserToAddRows = false;
            dgvDocentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocentes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDocentes.BackgroundColor = Color.White; dgvDocentes.BorderStyle = BorderStyle.None;
            dgvDocentes.RowHeadersVisible = false; dgvDocentes.MultiSelect = false;
            dgvDocentes.Font = new Font("Segoe UI", 9.5f);
            dgvDocentes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgvDocentes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDocentes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvDocentes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgvDocentes.EnableHeadersVisualStyles = false;
            dgvDocentes.SelectionChanged += dgvDocentes_SelectionChanged;

            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "dColId",          HeaderText = "Cédula",       Width = 100 });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "dColNombre",       HeaderText = "Nombre" });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "dColTitulo",       HeaderText = "Título" });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "dColDepartamento", HeaderText = "Departamento" });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "dColMaterias",     HeaderText = "Materias", Width = 70 });

            lblContador.Dock = DockStyle.Bottom; lblContador.Height = 25;
            lblContador.TextAlign = ContentAlignment.MiddleRight;
            lblContador.ForeColor = Color.Gray; lblContador.Font = new Font("Segoe UI", 9f);

            pnlGrid.Controls.Add(dgvDocentes);
            pnlGrid.Controls.Add(pnlBuscar);
            pnlGrid.Controls.Add(lblContador);

            // ── Form ──────────────────────────────────────
            Text = "Gestión de Docentes";
            Size = new Size(980, 630); MinimumSize = new Size(850, 530);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlGrid); Controls.Add(pnlForm);

            ((System.ComponentModel.ISupportInitialize)dgvDocentes).EndInit();
            ResumeLayout(false);
        }

        private static void ConfigBoton(Button b, string txt, Color col, Point loc, int w)
        {
            b.Text = txt; b.Location = loc; b.Size = new Size(w, 36);
            b.BackColor = col; b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 9.5f);
        }

        private Panel pnlForm, pnlGrid, pnlBorde, pnlBotones, pnlBuscar;
        private Label lblTitulo, lblContador, lblBuscarTxt;
        private TextBox txtId, txtNombre, txtEmail, txtTelefono;
        private TextBox txtTitulo, txtDepartamento, txtBuscar;
        private DateTimePicker dtpNacimiento;
        private Button btnGuardar, btnNuevo, btnEliminar;
        private DataGridView dgvDocentes;
    }
}
