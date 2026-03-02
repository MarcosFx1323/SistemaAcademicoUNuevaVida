using SistemaAcademicoUNuevaVida.Services;
using SistemaAcademicoUNuevaVida.Models;

namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Módulo 4: Registro y consulta de calificaciones.
    /// Notas validadas en rango 0.0–5.0, promedio calculado en tiempo real.
    /// </summary>
    public class CalificacionForm : Form
    {
        private ComboBox cmbEstudiante = null!, cmbMateria = null!;
        private TextBox txtNota1 = null!, txtNota2 = null!, txtNota3 = null!;
        private Label lblPromedio = null!, lblEstado = null!,
                      lblInfoInscripcion = null!, lblPromedioGral = null!;
        private Button btnGuardar = null!, btnLimpiar = null!;
        private DataGridView dgvCalificaciones = null!;
        private Panel pnlEstado = null!;

        private Inscripcion? _inscripcionActual;
        private GestorAcademico Gestor => MainForm.Gestor;

        public CalificacionForm()
        {
            InicializarComponentes();
            CargarCombos();
        }

        private void InicializarComponentes()
        {
            Text = "Registro de Calificaciones";
            Size = new Size(1000, 650);
            MinimumSize = new Size(900, 540);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9.5f);
            BackColor = Color.FromArgb(245, 247, 250);

            // ── Panel superior: selección ─────────────────────
            var pnlTop = new Panel { Height = 80, Dock = DockStyle.Top, BackColor = Color.White, Padding = new Padding(15, 10, 15, 0) };
            pnlTop.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 4, BackColor = Color.FromArgb(30, 60, 114) });

            pnlTop.Controls.Add(new Label { Text = "Estudiante:", Location = new Point(15, 20), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) });
            cmbEstudiante = new ComboBox { Location = new Point(100, 17), Width = 300, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEstudiante.SelectedIndexChanged += CmbEstudiante_Changed;

            pnlTop.Controls.Add(new Label { Text = "Materia:", Location = new Point(430, 20), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) });
            cmbMateria = new ComboBox { Location = new Point(500, 17), Width = 350, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMateria.SelectedIndexChanged += CmbMateria_Changed;

            lblInfoInscripcion = new Label { Location = new Point(15, 50), Width = 800, AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f) };

            pnlTop.Controls.AddRange(new Control[] { cmbEstudiante, cmbMateria, lblInfoInscripcion });

            // ── Panel central: notas + resumen ────────────────
            var pnlCenter = new Panel { Height = 160, Dock = DockStyle.Top, Padding = new Padding(15, 10, 15, 0) };

            // Notas
            pnlCenter.Controls.Add(new Label { Text = "Nota 1 (0.0 – 5.0):", Location = new Point(15, 15), AutoSize = true });
            txtNota1 = new TextBox { Location = new Point(155, 12), Width = 80, Font = new Font("Segoe UI", 11f), TextAlign = HorizontalAlignment.Center };
            txtNota1.TextChanged += Nota_TextChanged;
            txtNota1.Validating += Nota_Validating;

            pnlCenter.Controls.Add(new Label { Text = "Nota 2 (0.0 – 5.0):", Location = new Point(270, 15), AutoSize = true });
            txtNota2 = new TextBox { Location = new Point(410, 12), Width = 80, Font = new Font("Segoe UI", 11f), TextAlign = HorizontalAlignment.Center };
            txtNota2.TextChanged += Nota_TextChanged;
            txtNota2.Validating += Nota_Validating;

            pnlCenter.Controls.Add(new Label { Text = "Nota 3 (0.0 – 5.0):", Location = new Point(525, 15), AutoSize = true });
            txtNota3 = new TextBox { Location = new Point(665, 12), Width = 80, Font = new Font("Segoe UI", 11f), TextAlign = HorizontalAlignment.Center };
            txtNota3.TextChanged += Nota_TextChanged;
            txtNota3.Validating += Nota_Validating;

            // Promedio en tiempo real
            pnlCenter.Controls.Add(new Label { Text = "Promedio Final:", Location = new Point(15, 60), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) });
            lblPromedio = new Label
            {
                Text = "—", Location = new Point(130, 57), Width = 80, Height = 30,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 114), TextAlign = ContentAlignment.MiddleCenter
            };

            pnlEstado = new Panel { Location = new Point(220, 57), Width = 120, Height = 30, BorderStyle = BorderStyle.FixedSingle };
            lblEstado = new Label
            {
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold), Text = "—"
            };
            pnlEstado.Controls.Add(lblEstado);

            pnlCenter.Controls.Add(new Label { Text = "Promedio general:", Location = new Point(380, 60), AutoSize = true, Font = new Font("Segoe UI", 10f) });
            lblPromedioGral = new Label
            {
                Text = "—", Location = new Point(520, 57), AutoSize = true,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50)
            };

            // Botones
            btnGuardar = new Button
            {
                Text = "💾  Guardar calificaciones", Location = new Point(15, 110),
                Width = 200, Height = 36, BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f)
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnLimpiar = new Button
            {
                Text = "🔄  Limpiar", Location = new Point(228, 110),
                Width = 100, Height = 36, BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f)
            };
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Click += (s, e) => LimpiarNotas();

            pnlCenter.Controls.AddRange(new Control[]
            { txtNota1, txtNota2, txtNota3, lblPromedio, pnlEstado, lblPromedioGral, btnGuardar, btnLimpiar });

            // ── Panel inferior: historial ─────────────────────
            var pnlBottom = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlBottom.Controls.Add(new Label
            {
                Text = "Historial de calificaciones del estudiante:",
                Dock = DockStyle.Top, Height = 28,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50)
            });

            dgvCalificaciones = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, Font = new Font("Segoe UI", 9.5f)
            };
            EstilarDgv(dgvCalificaciones);
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Materia", HeaderText = "Materia" });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nota1", HeaderText = "Nota 1", Width = 75 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nota2", HeaderText = "Nota 2", Width = 75 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nota3", HeaderText = "Nota 3", Width = 75 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Promedio", HeaderText = "Promedio", Width = 90 });
            dgvCalificaciones.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", Width = 100 });
            pnlBottom.Controls.Add(dgvCalificaciones);

            Controls.Add(pnlBottom);
            Controls.Add(pnlCenter);
            Controls.Add(pnlTop);
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
        }

        private void CmbEstudiante_Changed(object? sender, EventArgs e)
        {
            cmbMateria.Items.Clear();
            cmbMateria.Items.Add("-- Seleccionar materia inscrita --");
            _inscripcionActual = null;
            LimpiarNotas();

            if (cmbEstudiante.SelectedItem is not Estudiante est) return;

            foreach (var ins in est.InscripcionesActivas)
                cmbMateria.Items.Add(ins.Materia);

            cmbMateria.DisplayMember = "Nombre";
            cmbMateria.SelectedIndex = 0;

            // Cargar historial
            CargarHistorial(est);
            lblPromedioGral.Text = Gestor.CalcularPromedioEstudiante(est.Id).ToString("F2");
        }

        private void CmbMateria_Changed(object? sender, EventArgs e)
        {
            if (cmbEstudiante.SelectedItem is not Estudiante est) return;
            if (cmbMateria.SelectedItem is not Materia mat) { _inscripcionActual = null; return; }

            _inscripcionActual = est.Inscripciones.FirstOrDefault(i =>
                i.Materia.Codigo == mat.Codigo && i.Estado == EstadoInscripcion.Activa);

            if (_inscripcionActual == null) return;

            lblInfoInscripcion.Text = $"Inscrito el: {_inscripcionActual.FechaInscripcion:dd/MM/yyyy}  |  Docente: {mat.DocenteResponsable?.Nombre ?? "Sin asignar"}";

            // Si ya tiene notas, cargarlas
            if (_inscripcionActual.Calificacion != null)
            {
                var cal = _inscripcionActual.Calificacion;
                txtNota1.Text = cal.Nota1.ToString("F1");
                txtNota2.Text = cal.Nota2.ToString("F1");
                txtNota3.Text = cal.Nota3.ToString("F1");
            }
            else
            {
                LimpiarNotas();
            }
        }

        private void Nota_TextChanged(object? sender, EventArgs e) => ActualizarPromedioEnVivo();

        private void Nota_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not TextBox txt) return;
            if (string.IsNullOrWhiteSpace(txt.Text)) return;

            if (!double.TryParse(txt.Text.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double val)
                || val < 0 || val > 5)
            {
                txt.BackColor = Color.FromArgb(255, 200, 200);
                MessageBox.Show("La nota debe ser un número entre 0.0 y 5.0", "Nota inválida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                txt.BackColor = Color.White;
            }
        }

        private void ActualizarPromedioEnVivo()
        {
            if (!ParsearNota(txtNota1.Text, out double n1) ||
                !ParsearNota(txtNota2.Text, out double n2) ||
                !ParsearNota(txtNota3.Text, out double n3))
            {
                lblPromedio.Text = "—";
                lblEstado.Text = "—";
                pnlEstado.BackColor = Color.LightGray;
                return;
            }

            double promedio = Math.Round((n1 + n2 + n3) / 3.0, 2);
            bool aprobado = promedio >= 3.0;

            lblPromedio.Text = promedio.ToString("F2");
            lblEstado.Text = aprobado ? "Aprobado" : "Reprobado";
            lblEstado.ForeColor = Color.White;
            pnlEstado.BackColor = aprobado ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (_inscripcionActual == null)
            { MessageBox.Show("Seleccione un estudiante y una materia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!ParsearNota(txtNota1.Text, out double n1) ||
                !ParsearNota(txtNota2.Text, out double n2) ||
                !ParsearNota(txtNota3.Text, out double n3))
            { MessageBox.Show("Ingrese las tres notas válidas (0.0 – 5.0).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var (ok, msg) = Gestor.RegistrarCalificacion(_inscripcionActual, n1, n2, n3);
            MessageBox.Show(msg, ok ? "Guardado" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok && cmbEstudiante.SelectedItem is Estudiante est)
            {
                CargarHistorial(est);
                lblPromedioGral.Text = Gestor.CalcularPromedioEstudiante(est.Id).ToString("F2");
            }
        }

        private void CargarHistorial(Estudiante est)
        {
            dgvCalificaciones.Rows.Clear();
            var calificaciones = Gestor.ObtenerCalificacionesPorEstudiante(est.Id);
            foreach (var c in calificaciones)
            {
                int idx = dgvCalificaciones.Rows.Add(
                    c.Inscripcion.Materia.Nombre,
                    c.Nota1.ToString("F1"), c.Nota2.ToString("F1"), c.Nota3.ToString("F1"),
                    c.PromedioFinal.ToString("F2"), c.EstadoAcademico);

                dgvCalificaciones.Rows[idx].DefaultCellStyle.ForeColor =
                    c.EstadoAcademico == "Aprobado" ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
            }
        }

        private void LimpiarNotas()
        {
            txtNota1.Text = txtNota2.Text = txtNota3.Text = "";
            txtNota1.BackColor = txtNota2.BackColor = txtNota3.BackColor = Color.White;
            lblPromedio.Text = "—";
            lblEstado.Text = "—";
            pnlEstado.BackColor = Color.LightGray;
            lblInfoInscripcion.Text = "";
        }

        private static bool ParsearNota(string texto, out double valor)
        {
            return double.TryParse(texto.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out valor)
                && valor >= 0 && valor <= 5;
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
