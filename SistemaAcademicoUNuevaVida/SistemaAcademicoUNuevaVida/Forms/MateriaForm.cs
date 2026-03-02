using SistemaAcademicoUNuevaVida.Services;
using SistemaAcademicoUNuevaVida.Models;

namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Módulo 2: Gestión de Materias.
    /// CRUD con asignación de docente, validación de cupo y filtro por semestre.
    /// </summary>
    public class MateriaForm : Form
    {
        private TextBox txtCodigo = null!, txtNombre = null!, txtBuscar = null!;
        private NumericUpDown nudCreditos = null!, nudCupo = null!, nudSemestre = null!;
        private ComboBox cmbDocente = null!, cmbFiltroSemestre = null!;
        private Button btnGuardar = null!, btnNuevo = null!, btnEliminar = null!;
        private DataGridView dgvMaterias = null!;
        private Label lblTitulo = null!, lblContador = null!;

        private Materia? _materiaSeleccionada;
        private GestorAcademico Gestor => MainForm.Gestor;

        public MateriaForm()
        {
            InicializarComponentes();
            CargarComboDocentes();
            CargarGrid();
        }

        private void InicializarComponentes()
        {
            Text = "Gestión de Materias";
            Size = new Size(1000, 650);
            MinimumSize = new Size(860, 540);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);

            // ── Panel formulario izquierda ────────────────────
            var pnlForm = new Panel { Width = 320, Dock = DockStyle.Left, BackColor = Color.White, Padding = new Padding(15) };
            pnlForm.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 4, BackColor = Color.FromArgb(30, 60, 114) });

            lblTitulo = new Label
            {
                Text = "Nueva Materia", Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 114), AutoSize = false,
                Height = 35, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleLeft
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, AutoScroll = true
            };

            txtCodigo = AgregarCampo(flow, "Código *", new TextBox { Width = 270, PlaceholderText = "Ej: MAT101" });
            txtNombre = AgregarCampo(flow, "Nombre de la materia *", new TextBox { Width = 270 });

            // Semestre
            var pnlSem = new Panel { Width = 280, Height = 55 };
            pnlSem.Controls.Add(new Label { Text = "Semestre *", AutoSize = true, Location = new Point(0, 0) });
            nudSemestre = new NumericUpDown { Minimum = 1, Maximum = 10, Value = 1, Width = 80, Location = new Point(0, 20), Font = new Font("Segoe UI", 10f) };
            pnlSem.Controls.Add(nudSemestre);
            flow.Controls.Add(pnlSem);

            // Créditos
            var pnlCred = new Panel { Width = 280, Height = 55 };
            pnlCred.Controls.Add(new Label { Text = "Créditos *", AutoSize = true, Location = new Point(0, 0) });
            nudCreditos = new NumericUpDown { Minimum = 1, Maximum = 10, Value = 3, Width = 80, Location = new Point(0, 20), Font = new Font("Segoe UI", 10f) };
            pnlCred.Controls.Add(nudCreditos);
            flow.Controls.Add(pnlCred);

            // Cupo máximo
            var pnlCupo = new Panel { Width = 280, Height = 55 };
            pnlCupo.Controls.Add(new Label { Text = "Cupo máximo *", AutoSize = true, Location = new Point(0, 0) });
            nudCupo = new NumericUpDown { Minimum = 1, Maximum = 200, Value = 30, Width = 80, Location = new Point(0, 20), Font = new Font("Segoe UI", 10f) };
            pnlCupo.Controls.Add(nudCupo);
            flow.Controls.Add(pnlCupo);

            // Docente responsable
            var pnlDocente = new Panel { Width = 280, Height = 55 };
            pnlDocente.Controls.Add(new Label { Text = "Docente responsable", AutoSize = true, Location = new Point(0, 0) });
            cmbDocente = new ComboBox
            {
                Width = 270, Location = new Point(0, 20), Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            pnlDocente.Controls.Add(cmbDocente);
            flow.Controls.Add(pnlDocente);

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

            // ── Panel grilla derecha ─────────────────────────
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            // Barra de herramientas
            var pnlTools = new Panel { Height = 45, Dock = DockStyle.Top };
            pnlTools.Controls.Add(new Label { Text = "🔍 Buscar:", AutoSize = true, Location = new Point(0, 13) });
            txtBuscar = new TextBox { Location = new Point(75, 10), Width = 200, Font = new Font("Segoe UI", 10f), PlaceholderText = "Código o nombre..." };
            txtBuscar.TextChanged += (s, e) => FiltrarGrid();

            pnlTools.Controls.Add(new Label { Text = "Semestre:", AutoSize = true, Location = new Point(290, 13) });
            cmbFiltroSemestre = new ComboBox
            {
                Location = new Point(360, 10), Width = 80,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10f)
            };
            cmbFiltroSemestre.Items.Add("Todos");
            for (int i = 1; i <= 10; i++) cmbFiltroSemestre.Items.Add(i.ToString());
            cmbFiltroSemestre.SelectedIndex = 0;
            cmbFiltroSemestre.SelectedIndexChanged += (s, e) => FiltrarGrid();
            pnlTools.Controls.AddRange(new Control[] { txtBuscar, cmbFiltroSemestre });

            dgvMaterias = CrearDataGridView();
            dgvMaterias.SelectionChanged += DgvMaterias_SelectionChanged;

            lblContador = new Label
            {
                Dock = DockStyle.Bottom, Height = 25, TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f)
            };

            pnlGrid.Controls.Add(dgvMaterias);
            pnlGrid.Controls.Add(pnlTools);
            pnlGrid.Controls.Add(lblContador);

            Controls.Add(pnlGrid);
            Controls.Add(pnlForm);
        }

        private DataGridView CrearDataGridView()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, MultiSelect = false, Font = new Font("Segoe UI", 9.5f)
            };
            EstilarDgv(dgv);
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Materia" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Semestre", HeaderText = "Sem.", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creditos", HeaderText = "Créd.", Width = 55 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cupo", HeaderText = "Cupo", Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Docente", HeaderText = "Docente" });
            return dgv;
        }

        private void CargarComboDocentes()
        {
            cmbDocente.Items.Clear();
            cmbDocente.Items.Add("-- Sin asignar --");
            foreach (var d in Gestor.Docentes)
                cmbDocente.Items.Add(d);
            cmbDocente.DisplayMember = "Nombre";
            cmbDocente.SelectedIndex = 0;
        }

        private void CargarGrid(List<Materia>? lista = null)
        {
            dgvMaterias.Rows.Clear();
            var materias = lista ?? Gestor.Materias;
            foreach (var m in materias)
            {
                string cupoStr = $"{m.Inscritos}/{m.CupoMaximo}";
                dgvMaterias.Rows.Add(m.Codigo, m.Nombre, m.Semestre, m.Creditos, cupoStr, m.DocenteResponsable?.Nombre ?? "-");

                // Color rojo si sin cupo
                if (!m.TieneCupo)
                    dgvMaterias.Rows[dgvMaterias.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.FromArgb(220, 53, 69);
            }
            lblContador.Text = $"Total: {dgvMaterias.Rows.Count} materia(s)";
        }

        private void FiltrarGrid()
        {
            int semestre = cmbFiltroSemestre.SelectedIndex == 0 ? 0 : cmbFiltroSemestre.SelectedIndex;
            var lista = Gestor.FiltrarMateriasPorSemestre(semestre);

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string f = txtBuscar.Text.ToLower();
                lista = lista.Where(m => m.Nombre.ToLower().Contains(f) || m.Codigo.ToLower().Contains(f)).ToList();
            }
            CargarGrid(lista);
        }

        private void DgvMaterias_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvMaterias.SelectedRows.Count == 0) return;
            string codigo = dgvMaterias.SelectedRows[0].Cells["Codigo"].Value?.ToString() ?? "";
            _materiaSeleccionada = Gestor.Materias.FirstOrDefault(m => m.Codigo == codigo);
            if (_materiaSeleccionada == null) return;

            lblTitulo.Text = "Editar Materia";
            txtCodigo.Text = _materiaSeleccionada.Codigo; txtCodigo.Enabled = false;
            txtNombre.Text = _materiaSeleccionada.Nombre;
            nudSemestre.Value = _materiaSeleccionada.Semestre;
            nudCreditos.Value = _materiaSeleccionada.Creditos;
            nudCupo.Value = _materiaSeleccionada.CupoMaximo;

            cmbDocente.SelectedIndex = 0;
            if (_materiaSeleccionada.DocenteResponsable != null)
            {
                for (int i = 0; i < cmbDocente.Items.Count; i++)
                {
                    if (cmbDocente.Items[i] is Docente d && d.Id == _materiaSeleccionada.DocenteResponsable.Id)
                    { cmbDocente.SelectedIndex = i; break; }
                }
            }
            btnEliminar.Enabled = true;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text)) { Alerta("El código es obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { Alerta("El nombre es obligatorio."); return; }

            Docente? docente = cmbDocente.SelectedItem as Docente;

            if (_materiaSeleccionada == null)
            {
                var nueva = new Materia
                {
                    Codigo = txtCodigo.Text.Trim().ToUpper(),
                    Nombre = txtNombre.Text.Trim(),
                    Semestre = (int)nudSemestre.Value,
                    Creditos = (int)nudCreditos.Value,
                    CupoMaximo = (int)nudCupo.Value,
                    DocenteResponsable = docente
                };
                var (ok, msg) = Gestor.AgregarMateria(nueva);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); CargarComboDocentes(); LimpiarFormulario(); }
            }
            else
            {
                _materiaSeleccionada.Nombre = txtNombre.Text.Trim();
                _materiaSeleccionada.Semestre = (int)nudSemestre.Value;
                _materiaSeleccionada.Creditos = (int)nudCreditos.Value;
                _materiaSeleccionada.CupoMaximo = (int)nudCupo.Value;

                var actualizada = new Materia
                {
                    Codigo = _materiaSeleccionada.Codigo,
                    Nombre = txtNombre.Text.Trim(),
                    Semestre = (int)nudSemestre.Value,
                    Creditos = (int)nudCreditos.Value,
                    CupoMaximo = (int)nudCupo.Value,
                    DocenteResponsable = docente
                };
                var (ok, msg) = Gestor.ActualizarMateria(actualizada);
                Resultado(ok, msg);
                if (ok) { CargarGrid(); LimpiarFormulario(); }
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_materiaSeleccionada == null) return;
            if (MessageBox.Show($"¿Eliminar la materia '{_materiaSeleccionada.Nombre}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            var (ok, msg) = Gestor.EliminarMateria(_materiaSeleccionada.Codigo);
            Resultado(ok, msg);
            if (ok) { CargarGrid(); LimpiarFormulario(); }
        }

        private void LimpiarFormulario()
        {
            _materiaSeleccionada = null;
            lblTitulo.Text = "Nueva Materia";
            txtCodigo.Text = txtNombre.Text = "";
            txtCodigo.Enabled = true;
            nudSemestre.Value = nudCreditos.Value = 1;
            nudCupo.Value = 30;
            cmbDocente.SelectedIndex = 0;
            btnEliminar.Enabled = false;
            dgvMaterias.ClearSelection();
        }

        private static void EstilarDgv(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgv.EnableHeadersVisualStyles = false;
        }

        private static T AgregarCampo<T>(FlowLayoutPanel p, string label, T ctrl) where T : Control
        {
            var pnl = new Panel { Width = 280, Height = 55 };
            pnl.Controls.Add(new Label { Text = label, AutoSize = true, Location = new Point(0, 0) });
            ctrl.Location = new Point(0, 20); ctrl.Font = new Font("Segoe UI", 10f);
            pnl.Controls.Add(ctrl); p.Controls.Add(pnl); return ctrl;
        }

        private static Button CrearBoton(string texto, Color color, Point loc, int w)
        {
            var btn = new Button { Text = texto, Width = w, Height = 36, Location = loc, BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f) };
            btn.FlatAppearance.BorderSize = 0; return btn;
        }

        private static void Alerta(string m) => MessageBox.Show(m, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private static void Resultado(bool ok, string m) => MessageBox.Show(m, ok ? "Éxito" : "Error", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }
}
