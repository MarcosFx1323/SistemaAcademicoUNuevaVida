namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class InscripcionForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl            = new TabControl();
            tabNueva              = new TabPage();
            tabConsulta           = new TabPage();
            pnlSeleccion          = new Panel();
            pnlGrilla             = new Panel();
            cmbEstudiante         = new ComboBox();
            cmbMateria            = new ComboBox();
            lblEstudianteTxt      = new Label();
            lblMateriaTxt         = new Label();
            lblInfoEstudiante     = new Label();
            lblInfoMateria        = new Label();
            lblCupo               = new Label();
            lblContador           = new Label();
            lblGrillaHeader       = new Label();
            btnInscribir          = new Button();
            btnCancelar           = new Button();
            dgvInscritos          = new DataGridView();
            dgvTodasInscripciones = new DataGridView();

            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInscritos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTodasInscripciones).BeginInit();

            // ── tabControl ────────────────────────────────
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10f);
            tabControl.Controls.Add(tabNueva);
            tabControl.Controls.Add(tabConsulta);

            // ── tabNueva ──────────────────────────────────
            tabNueva.Text = "📝  Nueva Inscripción";
            tabNueva.Padding = new Padding(15);

            // Panel selección (parte superior)
            pnlSeleccion.Dock = DockStyle.Top;
            pnlSeleccion.Height = 160;
            pnlSeleccion.Padding = new Padding(0, 10, 0, 0);

            lblEstudianteTxt.Text = "Estudiante:";
            lblEstudianteTxt.Location = new Point(0, 15);
            lblEstudianteTxt.AutoSize = true;
            lblEstudianteTxt.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

            cmbEstudiante.Location = new Point(0, 35);
            cmbEstudiante.Width = 380;
            cmbEstudiante.Font = new Font("Segoe UI", 10f);
            cmbEstudiante.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstudiante.SelectedIndexChanged += cmbEstudiante_SelectedIndexChanged;

            lblInfoEstudiante.Location = new Point(0, 65);
            lblInfoEstudiante.Width = 380;
            lblInfoEstudiante.Height = 25;
            lblInfoEstudiante.Font = new Font("Segoe UI", 9f);
            lblInfoEstudiante.ForeColor = Color.FromArgb(30, 60, 114);

            lblMateriaTxt.Text = "Materia:";
            lblMateriaTxt.Location = new Point(420, 15);
            lblMateriaTxt.AutoSize = true;
            lblMateriaTxt.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

            cmbMateria.Location = new Point(420, 35);
            cmbMateria.Width = 380;
            cmbMateria.Font = new Font("Segoe UI", 10f);
            cmbMateria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMateria.SelectedIndexChanged += cmbMateria_SelectedIndexChanged;

            lblInfoMateria.Location = new Point(420, 65);
            lblInfoMateria.Width = 380;
            lblInfoMateria.Height = 25;
            lblInfoMateria.Font = new Font("Segoe UI", 9f);
            lblInfoMateria.ForeColor = Color.FromArgb(30, 60, 114);

            lblCupo.Location = new Point(420, 92);
            lblCupo.Width = 380;
            lblCupo.Height = 22;
            lblCupo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            btnInscribir.Text = "✅  Inscribir Estudiante";
            btnInscribir.Location = new Point(0, 110);
            btnInscribir.Size = new Size(200, 40);
            btnInscribir.BackColor = Color.FromArgb(30, 60, 114);
            btnInscribir.ForeColor = Color.White;
            btnInscribir.FlatStyle = FlatStyle.Flat;
            btnInscribir.FlatAppearance.BorderSize = 0;
            btnInscribir.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnInscribir.Click += btnInscribir_Click;

            pnlSeleccion.Controls.AddRange(new Control[] {
                lblEstudianteTxt, cmbEstudiante, lblInfoEstudiante,
                lblMateriaTxt, cmbMateria, lblInfoMateria, lblCupo, btnInscribir
            });

            // Panel grilla (parte inferior)
            pnlGrilla.Dock = DockStyle.Fill;

            lblGrillaHeader.Text = "Estudiantes inscritos en la materia seleccionada:";
            lblGrillaHeader.Dock = DockStyle.Top;
            lblGrillaHeader.Height = 28;
            lblGrillaHeader.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblGrillaHeader.ForeColor = Color.FromArgb(50, 50, 50);

            EstilarDgv(dgvInscritos);
            dgvInscritos.SelectionChanged += dgvInscritos_SelectionChanged;
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "iColNombre",  HeaderText = "Estudiante" });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "iColId",      HeaderText = "Cédula",     Width = 110 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "iColFecha",   HeaderText = "Inscrito el",Width = 110 });
            dgvInscritos.Columns.Add(new DataGridViewTextBoxColumn { Name = "iColProm",    HeaderText = "Promedio",   Width = 90 });

            btnCancelar.Text = "❌  Cancelar inscripción seleccionada";
            btnCancelar.Dock = DockStyle.Bottom;
            btnCancelar.Height = 36;
            btnCancelar.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Font = new Font("Segoe UI", 9.5f);
            btnCancelar.Enabled = false;
            btnCancelar.Click += btnCancelar_Click;

            lblContador.Dock = DockStyle.Bottom;
            lblContador.Height = 22;
            lblContador.TextAlign = ContentAlignment.MiddleRight;
            lblContador.ForeColor = Color.Gray;
            lblContador.Font = new Font("Segoe UI", 9f);

            pnlGrilla.Controls.Add(dgvInscritos);
            pnlGrilla.Controls.Add(lblContador);
            pnlGrilla.Controls.Add(btnCancelar);
            pnlGrilla.Controls.Add(lblGrillaHeader);

            tabNueva.Controls.Add(pnlGrilla);
            tabNueva.Controls.Add(pnlSeleccion);

            // ── tabConsulta ───────────────────────────────
            tabConsulta.Text = "📋  Todas las Inscripciones";
            tabConsulta.Padding = new Padding(10);

            EstilarDgv(dgvTodasInscripciones);
            dgvTodasInscripciones.Dock = DockStyle.Fill;
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "tColNombre",   HeaderText = "Estudiante" });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "tColId",       HeaderText = "Cédula",    Width = 100 });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "tColMateria",  HeaderText = "Materia" });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "tColSemestre", HeaderText = "Sem.",       Width = 55 });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "tColFecha",    HeaderText = "Fecha",     Width = 100 });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "tColEstado",   HeaderText = "Estado",    Width = 90 });
            tabConsulta.Controls.Add(dgvTodasInscripciones);

            Text = "Gestión de Inscripciones";
            Size = new Size(1000, 650); MinimumSize = new Size(900, 550);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f); BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(tabControl);

            ((System.ComponentModel.ISupportInitialize)dgvInscritos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTodasInscripciones).EndInit();
            ResumeLayout(false);
        }

        private static void EstilarDgv(DataGridView dgv)
        {
            dgv.Dock = DockStyle.Fill; dgv.ReadOnly = true; dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White; dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false; dgv.MultiSelect = false;
            dgv.Font = new Font("Segoe UI", 9.5f);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgv.EnableHeadersVisualStyles = false;
        }

        private TabControl tabControl;
        private TabPage tabNueva, tabConsulta;
        private Panel pnlSeleccion, pnlGrilla;
        private ComboBox cmbEstudiante, cmbMateria;
        private Label lblEstudianteTxt, lblMateriaTxt, lblInfoEstudiante;
        private Label lblInfoMateria, lblCupo, lblContador, lblGrillaHeader;
        private Button btnInscribir, btnCancelar;
        private DataGridView dgvInscritos, dgvTodasInscripciones;
    }
}
