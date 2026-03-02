namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class CalificacionForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop            = new Panel();
            pnlCenter         = new Panel();
            pnlBottom         = new Panel();
            pnlEstado         = new Panel();
            cmbEstudiante     = new ComboBox();
            cmbMateria        = new ComboBox();
            lblEstudianteTxt  = new Label();
            lblMateriaTxt     = new Label();
            lblInfoInscripcion= new Label();
            lblNota1Txt       = new Label();
            lblNota2Txt       = new Label();
            lblNota3Txt       = new Label();
            lblPromedioTxt    = new Label();
            lblPromedioGralTxt= new Label();
            lblPromedio       = new Label();
            lblEstado         = new Label();
            lblPromedioGral   = new Label();
            lblHistorialTxt   = new Label();
            txtNota1          = new TextBox();
            txtNota2          = new TextBox();
            txtNota3          = new TextBox();
            btnGuardar        = new Button();
            btnLimpiar        = new Button();
            dgvCalificaciones = new DataGridView();

            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCalificaciones).BeginInit();

            // ── pnlTop ────────────────────────────────────
            pnlTop.Dock = DockStyle.Top; pnlTop.Height = 80;
            pnlTop.BackColor = Color.White; pnlTop.Padding = new Padding(15, 10, 15, 0);

            var borde = new Panel { Dock = DockStyle.Top, Height = 4, BackColor = Color.FromArgb(30, 60, 114) };

            lblEstudianteTxt.Text = "Estudiante:"; lblEstudianteTxt.Location = new Point(15, 20);
            lblEstudianteTxt.AutoSize = true; lblEstudianteTxt.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);

            cmbEstudiante.Location = new Point(110, 17); cmbEstudiante.Width = 300;
            cmbEstudiante.Font = new Font("Segoe UI", 10f); cmbEstudiante.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstudiante.SelectedIndexChanged += cmbEstudiante_SelectedIndexChanged;

            lblMateriaTxt.Text = "Materia inscrita:"; lblMateriaTxt.Location = new Point(440, 20);
            lblMateriaTxt.AutoSize = true; lblMateriaTxt.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);

            cmbMateria.Location = new Point(565, 17); cmbMateria.Width = 320;
            cmbMateria.Font = new Font("Segoe UI", 10f); cmbMateria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMateria.SelectedIndexChanged += cmbMateria_SelectedIndexChanged;

            lblInfoInscripcion.Location = new Point(15, 50); lblInfoInscripcion.Width = 800;
            lblInfoInscripcion.AutoSize = true; lblInfoInscripcion.ForeColor = Color.Gray;
            lblInfoInscripcion.Font = new Font("Segoe UI", 9f);

            pnlTop.Controls.AddRange(new Control[] { borde, lblEstudianteTxt, cmbEstudiante, lblMateriaTxt, cmbMateria, lblInfoInscripcion });

            // ── pnlCenter ─────────────────────────────────
            pnlCenter.Dock = DockStyle.Top; pnlCenter.Height = 160;
            pnlCenter.Padding = new Padding(15, 10, 15, 0);

            SetLbl(lblNota1Txt, "Nota 1 (0.0 – 5.0):", 15,  15);
            SetLbl(lblNota2Txt, "Nota 2 (0.0 – 5.0):", 270, 15);
            SetLbl(lblNota3Txt, "Nota 3 (0.0 – 5.0):", 525, 15);

            SetNotaTxt(txtNota1, 155, 12); SetNotaTxt(txtNota2, 410, 12); SetNotaTxt(txtNota3, 665, 12);
            txtNota1.TextChanged += txtNota_TextChanged; txtNota1.Validating += txtNota_Validating;
            txtNota2.TextChanged += txtNota_TextChanged; txtNota2.Validating += txtNota_Validating;
            txtNota3.TextChanged += txtNota_TextChanged; txtNota3.Validating += txtNota_Validating;

            SetLbl(lblPromedioTxt, "Promedio Final:", 15, 60);
            lblPromedioTxt.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

            lblPromedio.Text = "—"; lblPromedio.Location = new Point(130, 57);
            lblPromedio.Size = new Size(80, 32); lblPromedio.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblPromedio.ForeColor = Color.FromArgb(30, 60, 114); lblPromedio.TextAlign = ContentAlignment.MiddleCenter;

            pnlEstado.Location = new Point(220, 57); pnlEstado.Size = new Size(120, 32);
            pnlEstado.BorderStyle = BorderStyle.FixedSingle; pnlEstado.BackColor = Color.LightGray;
            lblEstado.Dock = DockStyle.Fill; lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            lblEstado.Font = new Font("Segoe UI", 10f, FontStyle.Bold); lblEstado.Text = "—";
            pnlEstado.Controls.Add(lblEstado);

            SetLbl(lblPromedioGralTxt, "Promedio general:", 380, 60);
            lblPromedioGral.Text = "—"; lblPromedioGral.Location = new Point(520, 57);
            lblPromedioGral.AutoSize = true; lblPromedioGral.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblPromedioGral.ForeColor = Color.FromArgb(50, 50, 50);

            btnGuardar.Text = "💾  Guardar calificaciones";
            btnGuardar.Location = new Point(15, 110); btnGuardar.Size = new Size(200, 36);
            btnGuardar.BackColor = Color.FromArgb(30, 60, 114); btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat; btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Font = new Font("Segoe UI", 9.5f); btnGuardar.Click += btnGuardar_Click;

            btnLimpiar.Text = "🔄  Limpiar";
            btnLimpiar.Location = new Point(228, 110); btnLimpiar.Size = new Size(100, 36);
            btnLimpiar.BackColor = Color.FromArgb(108, 117, 125); btnLimpiar.ForeColor = Color.White;
            btnLimpiar.FlatStyle = FlatStyle.Flat; btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Font = new Font("Segoe UI", 9.5f); btnLimpiar.Click += btnLimpiar_Click;

            pnlCenter.Controls.AddRange(new Control[] {
                lblNota1Txt, txtNota1, lblNota2Txt, txtNota2, lblNota3Txt, txtNota3,
                lblPromedioTxt, lblPromedio, pnlEstado, lblPromedioGralTxt, lblPromedioGral,
                btnGuardar, btnLimpiar
            });

            // ── pnlBottom ─────────────────────────────────
            pnlBottom.Dock = DockStyle.Fill; pnlBottom.Padding = new Padding(10);

            lblHistorialTxt.Text = "Historial de calificaciones del estudiante:";
            lblHistorialTxt.Dock = DockStyle.Top; lblHistorialTxt.Height = 28;
            lblHistorialTxt.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblHistorialTxt.ForeColor = Color.FromArgb(50, 50, 50);

            dgvCalificaciones.Dock = DockStyle.Fill; dgvCalificaciones.ReadOnly = true;
            dgvCalificaciones.AllowUserToAddRows = false;
            dgvCalificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCalificaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCalificaciones.BackgroundColor = Color.White; dgvCalificaciones.BorderStyle = BorderStyle.None;
            dgvCalificaciones.RowHeadersVisible = false; dgvCalificaciones.Font = new Font("Segoe UI", 9.5f);
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvCalificaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgvCalificaciones.EnableHeadersVisualStyles = false;

            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "cColMateria",  HeaderText = "Materia" });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "cColN1",       HeaderText = "Nota 1", Width = 75 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "cColN2",       HeaderText = "Nota 2", Width = 75 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "cColN3",       HeaderText = "Nota 3", Width = 75 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "cColProm",     HeaderText = "Promedio", Width = 90 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "cColEstado",   HeaderText = "Estado", Width = 100 });

            pnlBottom.Controls.Add(dgvCalificaciones);
            pnlBottom.Controls.Add(lblHistorialTxt);

            Text = "Registro de Calificaciones";
            Size = new Size(1000, 650); MinimumSize = new Size(900, 540);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f); BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlBottom); Controls.Add(pnlCenter); Controls.Add(pnlTop);

            ((System.ComponentModel.ISupportInitialize)dgvCalificaciones).EndInit();
            ResumeLayout(false);
        }

        private static void SetLbl(Label l, string t, int x, int y)
        { l.Text = t; l.AutoSize = true; l.Location = new Point(x, y); l.Font = new Font("Segoe UI", 9.5f); }
        private static void SetNotaTxt(TextBox t, int x, int y)
        { t.Location = new Point(x, y); t.Width = 80; t.Font = new Font("Segoe UI", 11f); t.TextAlign = HorizontalAlignment.Center; }

        private Panel pnlTop, pnlCenter, pnlBottom, pnlEstado;
        private ComboBox cmbEstudiante, cmbMateria;
        private Label lblEstudianteTxt, lblMateriaTxt, lblInfoInscripcion;
        private Label lblNota1Txt, lblNota2Txt, lblNota3Txt;
        private Label lblPromedioTxt, lblPromedioGralTxt;
        private Label lblPromedio, lblEstado, lblPromedioGral, lblHistorialTxt;
        private TextBox txtNota1, txtNota2, txtNota3;
        private Button btnGuardar, btnLimpiar;
        private DataGridView dgvCalificaciones;
    }
}
