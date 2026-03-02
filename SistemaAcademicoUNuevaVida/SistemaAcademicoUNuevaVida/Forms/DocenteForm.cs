using SistemaAcademicoUNuevaVida.Services;
using SistemaAcademicoUNuevaVida.Models;

namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Módulo 1 (parte B): CRUD de Docentes.
    /// Búsqueda en tiempo real, DataGridView con materias asignadas.
    /// </summary>
    public class DocenteForm : Form
    {
        private TextBox txtId = null!, txtNombre = null!, txtEmail = null!,
                         txtTelefono = null!, txtTitulo = null!, txtDepartamento = null!, txtBuscar = null!;
        private DateTimePicker dtpNacimiento = null!;
        private Button btnGuardar = null!, btnNuevo = null!, btnEliminar = null!;
        private DataGridView dgvDocentes = null!;
        private Label lblTitulo = null!, lblContador = null!;

        private Docente? _docenteSeleccionado;
        private GestorAcademico Gestor => MainForm.Gestor;

        public DocenteForm()
        {
            InicializarComponentes();
            CargarGrid();
        }

        private void InicializarComponentes()
        {
            Text = "Gestión de Docentes";
            Size = new Size(980, 630);
            MinimumSize = new Size(850, 530);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);

            // ── Panel formulario izquierda ────────────────────
            var pnlForm = new Panel
            {
                Width = 320, Dock = DockStyle.Left,
                BackColor = Color.White, Padding = new Padding(15)
            };

            var borde = new Panel { Dock = DockStyle.Top, Height = 4, BackColor = Color.FromArgb(30, 60, 114) };

            lblTitulo = new Label
            {
                Text = "Nuevo Docente", Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 114), AutoSize = false,
                Height = 35, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleLeft
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true, Padding = new Padding(0, 5, 0, 0)
            };

            txtId = AgregarCampo(flow, "Cédula *", new TextBox { Width = 270 });
            txtNombre = AgregarCampo(flow, "Nombre completo *", new TextBox { Width = 270 });
            txtEmail = AgregarCampo(flow, "Correo electrónico", new TextBox { Width = 270 });
            txtTelefono = AgregarCampo(flow, "Teléfono", new TextBox { Width = 270 });
            txtTitulo = AgregarCampo(flow, "Título máximo *", new TextBox { Width = 270, PlaceholderText = "Ej: Magíster en Ingeniería" });
            txtDepartamento = AgregarCampo(flow, "Departamento *", new TextBox { Width = 270 });

            var pnlFecha = new Panel { Width = 280, Height = 55 };
            pnlFecha.Controls.Add(new Label { Text = "Fecha de nacimiento", AutoSize = true, Location = new Point(0, 0) });
            dtpNacimiento = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short, Width = 270,
                Location = new Point(0, 20), MaxDate = DateTime.Today.AddYears(-22)
            };
            pnlFecha.Controls.Add(dtpNacimiento);
            flow.Controls.Add(pnlFecha);

            // Botones
            var pnlBotones = new Panel { Height = 50, Dock = DockStyle.Bottom };
            btnGuardar = CrearBoton("💾 Guardar", Color.FromArgb(30, 60, 114), new Point(0, 7), 120);
            btnGuardar.Click += BtnGuardar_Click;
            btnNuevo = CrearBoton("➕ Nuevo", Color.FromArgb(40, 167, 69), new Point(128, 7), 80);
            btnNuevo.Click += (s, e) => LimpiarFormulario();
            btnEliminar = CrearBoton("🗑 Eliminar", Color.FromArgb(220, 53, 69), new Point(216, 7), 90);
            btnEliminar.Enabled = false;
            btnEliminar.Click += BtnEliminar_Click;
            pnlBotones.Controls.AddRange(new Control[] { btnGuardar, btnNuevo, btnEliminar });

            pnlForm.Controls.Add(pnlBotones);
            pnlForm.Controls.Add(flow);
            pnlForm.Controls.Add(lblTitulo);
            pnlForm.Controls.Add(borde);

            // ── Panel grilla derecha ─────────────────────────
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            var pnlBuscar = new Panel { Height = 45, Dock = DockStyle.Top };
            pnlBuscar.Controls.Add(new Label { Text = "🔍 Buscar:", AutoSize = true, Location = new Point(0, 13) });
            txtBuscar = new TextBox
            {
                Location = new Point(75, 10), Width = 250, Font = new Font("Segoe UI", 10f),
                PlaceholderText = "Nombre, cédula o departamento..."
            };
            txtBuscar.TextChanged += (s, e) => FiltrarGrid();
            pnlBuscar.Controls.Add(txtBuscar);

            dgvDocentes = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, MultiSelect = false,
                Font = new Font("Segoe UI", 9.5f)
            };
            dgvDocentes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgvDocentes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDocentes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvDocentes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgvDocentes.EnableHeadersVisualStyles = false;

            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Cédula", Width = 100 });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Nombre" });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Titulo", HeaderText = "Título" });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Departamento", HeaderText = "Departamento" });
            dgvDocentes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Materias", HeaderText = "Materias", Width = 70 });
            dgvDocentes.SelectionChanged += DgvDocentes_SelectionChanged;

            lblContador = new Label
            {
                Dock = DockStyle.Bottom, Height = 25, TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f)
            };

            pnlGrid.Controls.Add(dgvDocentes);
            pnlGrid.Controls.Add(pnlBuscar);
            pnlGrid.Controls.Add(lblContador);

            Controls.Add(pnlGrid);
            Controls.Add(pnlForm);
        }

        private void CargarGrid(List<Docente>? lista = null)
        {
            dgvDocentes.Rows.Clear();
            var docentes = lista ?? Gestor.Docentes;
            foreach (var d in docentes)
                dgvDocentes.Rows.Add(d.Id, d.Nombre, d.Titulo, d.Departamento, d.TotalMaterias);
            lblContador.Text = $"Total: {dgvDocentes.Rows.Count} docente(s)";
        }

        private void FiltrarGrid() => CargarGrid(Gestor.BuscarDocentes(txtBuscar.Text));

        private void DgvDocentes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDocentes.SelectedRows.Count == 0) return;
            string id = dgvDocentes.SelectedRows[0].Cells["Id"].Value?.ToString() ?? "";
            _docenteSeleccionado = Gestor.Docentes.FirstOrDefault(d => d.Id == id);
            if (_docenteSeleccionado == null) return;

            lblTitulo.Text = "Editar Docente";
            txtId.Text = _docenteSeleccionado.Id; txtId.Enabled = false;
            txtNombre.Text = _docenteSeleccionado.Nombre;
            txtEmail.Text = _docenteSeleccionado.Email;
            txtTelefono.Text = _docenteSeleccionado.Telefono;
            txtTitulo.Text = _docenteSeleccionado.Titulo;
            txtDepartamento.Text = _docenteSeleccionado.Departamento;
            if (_docenteSeleccionado.FechaNacimiento > DateTime.MinValue)
                dtpNacimiento.Value = _docenteSeleccionado.FechaNacimiento;
            btnEliminar.Enabled = true;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) { Alerta("La cédula es obligatoria."); return; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { Alerta("El nombre es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(txtTitulo.Text)) { Alerta("El título es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(txtDepartamento.Text)) { Alerta("El departamento es obligatorio."); return; }

            if (_docenteSeleccionado == null)
            {
                var nuevo = new Docente
                {
                    Id = txtId.Text.Trim(), Nombre = txtNombre.Text.Trim(),
                    Email = txtEmail.Text.Trim(), Telefono = txtTelefono.Text.Trim(),
                    Titulo = txtTitulo.Text.Trim(), Departamento = txtDepartamento.Text.Trim(),
                    FechaNacimiento = dtpNacimiento.Value
                };
                var (ok, msg) = Gestor.AgregarDocente(nuevo);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); LimpiarFormulario(); }
            }
            else
            {
                _docenteSeleccionado.Nombre = txtNombre.Text.Trim();
                _docenteSeleccionado.Email = txtEmail.Text.Trim();
                _docenteSeleccionado.Telefono = txtTelefono.Text.Trim();
                _docenteSeleccionado.Titulo = txtTitulo.Text.Trim();
                _docenteSeleccionado.Departamento = txtDepartamento.Text.Trim();
                _docenteSeleccionado.FechaNacimiento = dtpNacimiento.Value;

                var (ok, msg) = Gestor.ActualizarDocente(_docenteSeleccionado);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); LimpiarFormulario(); }
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_docenteSeleccionado == null) return;
            if (MessageBox.Show($"¿Eliminar a '{_docenteSeleccionado.Nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            var (ok, msg) = Gestor.EliminarDocente(_docenteSeleccionado.Id);
            Resultado(ok, msg);
            if (ok) { CargarGrid(); LimpiarFormulario(); }
        }

        private void LimpiarFormulario()
        {
            _docenteSeleccionado = null;
            lblTitulo.Text = "Nuevo Docente";
            txtId.Text = txtNombre.Text = txtEmail.Text = txtTelefono.Text =
            txtTitulo.Text = txtDepartamento.Text = "";
            txtId.Enabled = true;
            dtpNacimiento.Value = DateTime.Today.AddYears(-30);
            btnEliminar.Enabled = false;
            dgvDocentes.ClearSelection();
        }

        private static T AgregarCampo<T>(FlowLayoutPanel parent, string label, T ctrl) where T : Control
        {
            var p = new Panel { Width = 280, Height = 55 };
            p.Controls.Add(new Label { Text = label, AutoSize = true, Location = new Point(0, 0) });
            ctrl.Location = new Point(0, 20); ctrl.Font = new Font("Segoe UI", 10f);
            p.Controls.Add(ctrl); parent.Controls.Add(p); return ctrl;
        }

        private static Button CrearBoton(string texto, Color color, Point loc, int w)
        {
            var btn = new Button
            {
                Text = texto, Width = w, Height = 36, Location = loc,
                BackColor = color, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static void Alerta(string m) =>
            MessageBox.Show(m, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void Resultado(bool ok, string m) =>
            MessageBox.Show(m, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }
}
