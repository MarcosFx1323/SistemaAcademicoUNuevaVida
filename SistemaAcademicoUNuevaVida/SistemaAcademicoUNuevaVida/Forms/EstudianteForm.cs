using SistemaAcademicoUNuevaVida.Models;
using SistemaAcademicoUNuevaVida.Services;


namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Módulo 1 (parte A): CRUD de Estudiantes.
    /// TabControl con pestañas Estudiantes y Docentes.
    /// Búsqueda en tiempo real sobre DataGridView.
    /// </summary>
    public class EstudianteForm : Form
    {
        // ── Controles ────────────────────────────────────────
        private TabControl tabControl = null!;
        private TabPage tabEstudiantes = null!;

        // Panel izquierdo – formulario
        private TextBox txtId = null!, txtNombre = null!, txtEmail = null!,
                         txtTelefono = null!, txtPrograma = null!, txtBuscar = null!;
        private NumericUpDown nudSemestre = null!;
        private DateTimePicker dtpNacimiento = null!;
        private Button btnGuardar = null!, btnNuevo = null!,
                       btnEliminar = null!, btnCancelar = null!;
        private Label lblTitulo = null!;

        // Panel derecho – grilla
        private DataGridView dgvEstudiantes = null!;
        private Label lblContador = null!;

        private Estudiante? _estudianteSeleccionado;
        private GestorAcademico Gestor => MainForm.Gestor;

        public EstudianteForm()
        {
            InicializarComponentes();
            CargarGrid();
        }

        private void InicializarComponentes()
        {
            Text = "Gestión de Estudiantes y Docentes";
            Size = new Size(1000, 650);
            MinimumSize = new Size(850, 550);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);

            tabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f) };
            tabEstudiantes = new TabPage("👤  Estudiantes");
            var tabDocentes = new TabPage("🎓  Docentes");
            tabControl.TabPages.AddRange(new[] { tabEstudiantes, tabDocentes });
            tabDocentes.Controls.Add(new Label
            {
                Text = "Módulo Docentes – abrir desde menú Gestión de Docentes",
                AutoSize = true, Location = new Point(20, 20),
                ForeColor = Color.Gray
            });

            tabEstudiantes.Controls.Add(CrearPanelEstudiantes());
            Controls.Add(tabControl);
        }

        private Panel CrearPanelEstudiantes()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 247, 250) };

            // ── Panel formulario (izquierda) ─────────────────
            var pnlForm = new Panel
            {
                Width = 320, Dock = DockStyle.Left,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            pnlForm.Controls.Add(CrearBorde());

            lblTitulo = new Label
            {
                Text = "Nuevo Estudiante", Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 114), AutoSize = false,
                Height = 35, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleLeft
            };

            var campos = CrearCamposFormulario();
            var pnlBotones = CrearPanelBotones();

            pnlForm.Controls.Add(pnlBotones);
            pnlForm.Controls.Add(campos);
            pnlForm.Controls.Add(lblTitulo);

            // ── Panel grilla (derecha) ───────────────────────
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            // Barra de búsqueda
            var pnlBuscar = new Panel { Height = 45, Dock = DockStyle.Top };
            var lblBuscar = new Label
            {
                Text = "🔍 Buscar:", AutoSize = true, Location = new Point(0, 13),
                Font = new Font("Segoe UI", 9.5f)
            };
            txtBuscar = new TextBox
            {
                Location = new Point(75, 10), Width = 250, Height = 25,
                Font = new Font("Segoe UI", 10f), PlaceholderText = "Nombre, ID o programa..."
            };
            txtBuscar.TextChanged += (s, e) => FiltrarGrid();
            pnlBuscar.Controls.AddRange(new Control[] { lblBuscar, txtBuscar });

            dgvEstudiantes = CrearDataGridView();
            dgvEstudiantes.SelectionChanged += DgvEstudiantes_SelectionChanged;

            lblContador = new Label
            {
                Dock = DockStyle.Bottom, Height = 25, TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f)
            };

            pnlGrid.Controls.Add(dgvEstudiantes);
            pnlGrid.Controls.Add(pnlBuscar);
            pnlGrid.Controls.Add(lblContador);

            panel.Controls.Add(pnlGrid);
            panel.Controls.Add(pnlForm);
            return panel;
        }

        private FlowLayoutPanel CrearCamposFormulario()
        {
            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, Padding = new Padding(0, 5, 0, 0)
            };

            txtId = AgregarCampo(flow, "Cédula / ID *", new TextBox { Width = 270 });
            txtNombre = AgregarCampo(flow, "Nombre completo *", new TextBox { Width = 270 });
            txtEmail = AgregarCampo(flow, "Correo electrónico", new TextBox { Width = 270 });
            txtTelefono = AgregarCampo(flow, "Teléfono", new TextBox { Width = 270 });
            txtPrograma = AgregarCampo(flow, "Programa académico *", new TextBox { Width = 270 });

            // Semestre
            var pnlSem = new Panel { Width = 280, Height = 55 };
            pnlSem.Controls.Add(new Label { Text = "Semestre *", AutoSize = true, Location = new Point(0, 0) });
            nudSemestre = new NumericUpDown
            {
                Minimum = 1, Maximum = 10, Value = 1,
                Width = 80, Location = new Point(0, 20), Font = new Font("Segoe UI", 10f)
            };
            pnlSem.Controls.Add(nudSemestre);
            flow.Controls.Add(pnlSem);

            // Fecha nacimiento
            var pnlFecha = new Panel { Width = 280, Height = 55 };
            pnlFecha.Controls.Add(new Label { Text = "Fecha de nacimiento", AutoSize = true, Location = new Point(0, 0) });
            dtpNacimiento = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short, Width = 270,
                Location = new Point(0, 20), MaxDate = DateTime.Today.AddYears(-15)
            };
            pnlFecha.Controls.Add(dtpNacimiento);
            flow.Controls.Add(pnlFecha);

            return flow;
        }

        private Panel CrearPanelBotones()
        {
            var pnl = new Panel { Height = 50, Dock = DockStyle.Bottom };

            btnGuardar = new Button
            {
                Text = "💾 Guardar", Width = 120, Height = 36,
                Location = new Point(0, 7), BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f)
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnNuevo = new Button
            {
                Text = "➕ Nuevo", Width = 80, Height = 36,
                Location = new Point(128, 7), BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f)
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => LimpiarFormulario();

            btnEliminar = new Button
            {
                Text = "🗑 Eliminar", Width = 90, Height = 36,
                Location = new Point(216, 7), BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f),
                Enabled = false
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;

            pnl.Controls.AddRange(new Control[] { btnGuardar, btnNuevo, btnEliminar });
            return pnl;
        }

        private DataGridView CrearDataGridView()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, MultiSelect = false,
                Font = new Font("Segoe UI", 9.5f)
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgv.EnableHeadersVisualStyles = false;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Cédula", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Nombre" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Programa", HeaderText = "Programa" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Semestre", HeaderText = "Sem.", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Promedio", HeaderText = "Promedio", Width = 80 });

            return dgv;
        }

        // ═══════════════════════════════════════════════════
        //  LÓGICA
        // ═══════════════════════════════════════════════════

        private void CargarGrid(List<Estudiante>? lista = null)
        {
            dgvEstudiantes.Rows.Clear();
            var estudiantes = lista ?? Gestor.Estudiantes;

            foreach (var e in estudiantes)
            {
                dgvEstudiantes.Rows.Add(
                    e.Id, e.Nombre, e.Programa, e.Semestre, e.Email,
                    e.PromedioGeneral > 0 ? e.PromedioGeneral.ToString("F2") : "-"
                );
            }

            lblContador.Text = $"Total: {dgvEstudiantes.Rows.Count} estudiante(s)";
        }

        private void FiltrarGrid() => CargarGrid(Gestor.BuscarEstudiantes(txtBuscar.Text));

        private void DgvEstudiantes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count == 0) return;
            var fila = dgvEstudiantes.SelectedRows[0];
            string id = fila.Cells["Id"].Value?.ToString() ?? "";
            _estudianteSeleccionado = Gestor.Estudiantes.FirstOrDefault(est => est.Id == id);

            if (_estudianteSeleccionado != null)
            {
                lblTitulo.Text = "Editar Estudiante";
                txtId.Text = _estudianteSeleccionado.Id;
                txtId.Enabled = false;
                txtNombre.Text = _estudianteSeleccionado.Nombre;
                txtEmail.Text = _estudianteSeleccionado.Email;
                txtTelefono.Text = _estudianteSeleccionado.Telefono;
                txtPrograma.Text = _estudianteSeleccionado.Programa;
                nudSemestre.Value = _estudianteSeleccionado.Semestre;
                if (_estudianteSeleccionado.FechaNacimiento > DateTime.MinValue)
                    dtpNacimiento.Value = _estudianteSeleccionado.FechaNacimiento;
                btnEliminar.Enabled = true;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (_estudianteSeleccionado == null)
            {
                // Nuevo
                var nuevo = new Estudiante
                {
                    Id = txtId.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Programa = txtPrograma.Text.Trim(),
                    Semestre = (int)nudSemestre.Value,
                    FechaNacimiento = dtpNacimiento.Value
                };

                var (exito, msg) = Gestor.AgregarEstudiante(nuevo);
                MostrarResultado(exito, msg);
                if (exito) { CargarGrid(); LimpiarFormulario(); }
            }
            else
            {
                // Actualizar
                _estudianteSeleccionado.Nombre = txtNombre.Text.Trim();
                _estudianteSeleccionado.Email = txtEmail.Text.Trim();
                _estudianteSeleccionado.Telefono = txtTelefono.Text.Trim();
                _estudianteSeleccionado.Programa = txtPrograma.Text.Trim();
                _estudianteSeleccionado.Semestre = (int)nudSemestre.Value;
                _estudianteSeleccionado.FechaNacimiento = dtpNacimiento.Value;

                var (exito, msg) = Gestor.ActualizarEstudiante(_estudianteSeleccionado);
                MostrarResultado(exito, msg);
                if (exito) { CargarGrid(); LimpiarFormulario(); }
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_estudianteSeleccionado == null) return;

            var res = MessageBox.Show(
                $"¿Eliminar al estudiante '{_estudianteSeleccionado.Nombre}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res != DialogResult.Yes) return;

            var (exito, msg) = Gestor.EliminarEstudiante(_estudianteSeleccionado.Id);
            MostrarResultado(exito, msg);
            if (exito) { CargarGrid(); LimpiarFormulario(); }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            { MostrarError("La cédula/ID es obligatoria."); txtId.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            { MostrarError("El nombre es obligatorio."); txtNombre.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtPrograma.Text))
            { MostrarError("El programa académico es obligatorio."); txtPrograma.Focus(); return false; }
            return true;
        }

        private void LimpiarFormulario()
        {
            _estudianteSeleccionado = null;
            lblTitulo.Text = "Nuevo Estudiante";
            txtId.Text = txtNombre.Text = txtEmail.Text = txtTelefono.Text = txtPrograma.Text = "";
            txtId.Enabled = true;
            nudSemestre.Value = 1;
            dtpNacimiento.Value = DateTime.Today.AddYears(-18);
            btnEliminar.Enabled = false;
            dgvEstudiantes.ClearSelection();
            txtId.Focus();
        }

        private static void MostrarResultado(bool exito, string msg) =>
            MessageBox.Show(msg, exito ? "Éxito" : "Error",
                MessageBoxButtons.OK, exito ? MessageBoxIcon.Information : MessageBoxIcon.Error);

        private static void MostrarError(string msg) =>
            MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static T AgregarCampo<T>(FlowLayoutPanel parent, string etiqueta, T control) where T : Control
        {
            var pnl = new Panel { Width = 280, Height = 55 };
            pnl.Controls.Add(new Label { Text = etiqueta, AutoSize = true, Location = new Point(0, 0) });
            control.Location = new Point(0, 20);
            control.Font = new Font("Segoe UI", 10f);
            pnl.Controls.Add(control);
            parent.Controls.Add(pnl);
            return control;
        }

        private static Panel CrearBorde()
        {
            return new Panel
            {
                Dock = DockStyle.Top, Height = 4,
                BackColor = Color.FromArgb(30, 60, 114)
            };
        }
    }
}
