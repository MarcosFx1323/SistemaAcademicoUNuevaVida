using SistemaAcademicoUNuevaVida.Services;
using SistemaAcademicoUNuevaVida.Models;

namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Módulo 3: Gestión de Inscripciones.
    /// Inscribir estudiantes en materias con validación de cupo,
    /// duplicados y semestre. Cancelar con confirmación.
    /// </summary>
    public class InscripcionForm : Form
    {
        private ComboBox cmbEstudiante = null!, cmbMateria = null!;
        private DataGridView dgvInscritos = null!, dgvTodasInscripciones = null!;
        private Button btnInscribir = null!, btnCancelar = null!;
        private Label lblInfoEstudiante = null!, lblInfoMateria = null!,
                      lblCupo = null!, lblContador = null!;
        private TabControl tabControl = null!;

        private GestorAcademico Gestor => MainForm.Gestor;

        public InscripcionForm()
        {
            InicializarComponentes();
            CargarCombos();
            CargarTodasInscripciones();
        }

        private void InicializarComponentes()
        {
            Text = "Gestión de Inscripciones";
            Size = new Size(1000, 650);
            MinimumSize = new Size(900, 550);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);

            tabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f) };
            var tabNueva = new TabPage("📝  Nueva Inscripción");
            var tabConsulta = new TabPage("📋  Todas las Inscripciones");

            tabNueva.Controls.Add(CrearPanelNuevaInscripcion());
            tabConsulta.Controls.Add(CrearPanelConsulta());

            tabControl.TabPages.AddRange(new[] { tabNueva, tabConsulta });
            Controls.Add(tabControl);
        }

        private Panel CrearPanelNuevaInscripcion()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };

            // ── Encabezado ────────────────────────────────────
            var pnlHeader = new Panel { Height = 40, Dock = DockStyle.Top };
            pnlHeader.Controls.Add(new Label
            {
                Text = "Inscribir estudiante en materia",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 114), AutoSize = true, Location = new Point(0, 8)
            });

            // ── Panel selección ───────────────────────────────
            var pnlSeleccion = new Panel { Height = 180, Dock = DockStyle.Top, Padding = new Padding(0, 10, 0, 0) };

            // Estudiante
            pnlSeleccion.Controls.Add(new Label { Text = "Estudiante:", Location = new Point(0, 15), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) });
            cmbEstudiante = new ComboBox
            {
                Location = new Point(0, 35), Width = 380, Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEstudiante.SelectedIndexChanged += CmbEstudiante_Changed;
            lblInfoEstudiante = new Label
            {
                Location = new Point(0, 65), Width = 380, Height = 30,
                ForeColor = Color.FromArgb(30, 60, 114), Font = new Font("Segoe UI", 9f)
            };

            // Materia
            pnlSeleccion.Controls.Add(new Label { Text = "Materia:", Location = new Point(420, 15), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) });
            cmbMateria = new ComboBox
            {
                Location = new Point(420, 35), Width = 380, Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMateria.SelectedIndexChanged += CmbMateria_Changed;
            lblInfoMateria = new Label
            {
                Location = new Point(420, 65), Width = 380, Height = 30,
                ForeColor = Color.FromArgb(30, 60, 114), Font = new Font("Segoe UI", 9f)
            };

            lblCupo = new Label
            {
                Location = new Point(420, 95), Width = 380, Height = 25,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };

            btnInscribir = new Button
            {
                Text = "✅  Inscribir Estudiante", Location = new Point(0, 110),
                Width = 200, Height = 40, BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            btnInscribir.FlatAppearance.BorderSize = 0;
            btnInscribir.Click += BtnInscribir_Click;

            pnlSeleccion.Controls.AddRange(new Control[]
            { cmbEstudiante, lblInfoEstudiante, cmbMateria, lblInfoMateria, lblCupo, btnInscribir });

            // ── Grilla inscritos por materia ──────────────────
            var pnlGrilla = new Panel { Dock = DockStyle.Fill };
            var lblGrillaHeader = new Label
            {
                Text = "Inscritos en la materia seleccionada:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Dock = DockStyle.Top, Height = 28,
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            dgvInscritos = CrearDgvInscritos();

            btnCancelar = new Button
            {
                Text = "❌  Cancelar inscripción seleccionada",
                Dock = DockStyle.Bottom, Height = 36,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f), Enabled = false
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelarInscripcion_Click;

            lblContador = new Label
            {
                Dock = DockStyle.Bottom, Height = 22, TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f)
            };

            pnlGrilla.Controls.Add(dgvInscritos);
            pnlGrilla.Controls.Add(lblContador);
            pnlGrilla.Controls.Add(btnCancelar);
            pnlGrilla.Controls.Add(lblGrillaHeader);

            panel.Controls.Add(pnlGrilla);
            panel.Controls.Add(pnlSeleccion);
            panel.Controls.Add(pnlHeader);

            return panel;
        }

        private Panel CrearPanelConsulta()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            dgvTodasInscripciones = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, Font = new Font("Segoe UI", 9.5f)
            };
            EstilarDgv(dgvTodasInscripciones);
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estudiante", HeaderText = "Estudiante" });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Cédula", Width = 100 });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Materia", HeaderText = "Materia" });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Semestre", HeaderText = "Sem.", Width = 55 });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fecha", HeaderText = "Fecha", Width = 100 });
            dgvTodasInscripciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", Width = 90 });

            panel.Controls.Add(dgvTodasInscripciones);
            return panel;
        }

        private DataGridView CrearDgvInscritos()
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
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estudiante", HeaderText = "Estudiante" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Cédula", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fecha", HeaderText = "Inscrito el", Width = 110 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Promedio", HeaderText = "Promedio", Width = 90 });
            dgv.SelectionChanged += (s, e) => btnCancelar.Enabled = dgv.SelectedRows.Count > 0;
            return dgv;
        }

        // ═══════════════════════════════════════════════════
        //  LÓGICA
        // ═══════════════════════════════════════════════════

        private void CargarCombos()
        {
            cmbEstudiante.Items.Clear();
            cmbEstudiante.Items.Add("-- Seleccionar estudiante --");
            foreach (var e in Gestor.Estudiantes.OrderBy(e => e.Nombre))
                cmbEstudiante.Items.Add(e);
            cmbEstudiante.DisplayMember = "Nombre";
            cmbEstudiante.SelectedIndex = 0;

            cmbMateria.Items.Clear();
            cmbMateria.Items.Add("-- Seleccionar materia --");
            foreach (var m in Gestor.Materias.OrderBy(m => m.Semestre))
                cmbMateria.Items.Add(m);
            cmbMateria.DisplayMember = "Nombre";
            cmbMateria.SelectedIndex = 0;
        }

        private void CmbEstudiante_Changed(object? sender, EventArgs e)
        {
            if (cmbEstudiante.SelectedItem is Estudiante est)
                lblInfoEstudiante.Text = $"{est.Programa} | Semestre {est.Semestre}";
            else
                lblInfoEstudiante.Text = "";
        }

        private void CmbMateria_Changed(object? sender, EventArgs e)
        {
            if (cmbMateria.SelectedItem is Materia mat)
            {
                lblInfoMateria.Text = $"Docente: {mat.DocenteResponsable?.Nombre ?? "Sin asignar"}";
                lblCupo.Text = $"Cupo: {mat.Inscritos}/{mat.CupoMaximo}  ({mat.CuposDisponibles} disponibles)";
                lblCupo.ForeColor = mat.TieneCupo ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
                CargarInscritosPorMateria(mat);
            }
            else
            {
                lblInfoMateria.Text = "";
                lblCupo.Text = "";
                dgvInscritos.Rows.Clear();
            }
        }

        private void CargarInscritosPorMateria(Materia mat)
        {
            dgvInscritos.Rows.Clear();
            var inscritos = Gestor.ObtenerInscritosPorMateria(mat.Codigo);
            foreach (var ins in inscritos)
            {
                string promedio = ins.Calificacion?.PromedioFinal.ToString("F2") ?? "Pendiente";
                dgvInscritos.Rows.Add(ins.Estudiante.Nombre, ins.Estudiante.Id,
                    ins.FechaInscripcion.ToString("dd/MM/yyyy"), promedio);
            }
            lblContador.Text = $"Inscritos: {inscritos.Count}";
            btnCancelar.Enabled = false;
        }

        private void CargarTodasInscripciones()
        {
            dgvTodasInscripciones.Rows.Clear();
            foreach (var ins in Gestor.Inscripciones)
            {
                dgvTodasInscripciones.Rows.Add(
                    ins.Estudiante.Nombre, ins.Estudiante.Id,
                    ins.Materia.Nombre, ins.Materia.Semestre,
                    ins.FechaInscripcion.ToString("dd/MM/yyyy"), ins.Estado.ToString());

                // Color según estado
                var fila = dgvTodasInscripciones.Rows[dgvTodasInscripciones.Rows.Count - 1];
                fila.DefaultCellStyle.ForeColor = ins.Estado == EstadoInscripcion.Cancelada
                    ? Color.FromArgb(150, 150, 150) : Color.Black;
            }
        }

        private void BtnInscribir_Click(object? sender, EventArgs e)
        {
            if (cmbEstudiante.SelectedItem is not Estudiante est)
            { MessageBox.Show("Seleccione un estudiante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (cmbMateria.SelectedItem is not Materia mat)
            { MessageBox.Show("Seleccione una materia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var (ok, msg) = Gestor.InscribirEstudiante(est, mat);

            if (ok)
            {
                MessageBox.Show(msg, "Inscripción exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInscritosPorMateria(mat);
                CargarTodasInscripciones();
                CmbMateria_Changed(null, EventArgs.Empty); // refrescar cupo
            }
            else
            {
                MessageBox.Show(msg, "No se puede inscribir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancelarInscripcion_Click(object? sender, EventArgs e)
        {
            if (dgvInscritos.SelectedRows.Count == 0) return;
            if (cmbMateria.SelectedItem is not Materia mat) return;

            string cedula = dgvInscritos.SelectedRows[0].Cells["Id"].Value?.ToString() ?? "";
            var inscripcion = Gestor.Inscripciones.FirstOrDefault(i =>
                i.Estudiante.Id == cedula &&
                i.Materia.Codigo == mat.Codigo &&
                i.Estado == EstadoInscripcion.Activa);

            if (inscripcion == null) return;

            var res = MessageBox.Show(
                $"¿Cancelar la inscripción de '{inscripcion.Estudiante.Nombre}' en '{mat.Nombre}'?\n\nEsta acción no se puede deshacer.",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res != DialogResult.Yes) return;

            var (ok, msg) = Gestor.CancelarInscripcion(inscripcion);
            MessageBox.Show(msg, ok ? "Cancelada" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok) { CargarInscritosPorMateria(mat); CargarTodasInscripciones(); CmbMateria_Changed(null, EventArgs.Empty); }
        }

        private static void EstilarDgv(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            dgv.EnableHeadersVisualStyles = false;
        }
    }
}
