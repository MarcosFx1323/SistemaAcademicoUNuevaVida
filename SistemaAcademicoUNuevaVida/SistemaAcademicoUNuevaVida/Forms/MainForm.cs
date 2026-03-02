using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    /// <summary>
    /// Formulario principal MDI. Contiene el menú de navegación
    /// y actúa como contenedor de todos los formularios hijos.
    /// </summary>
    public class MainForm : Form
    {
        // ── Servicios ────────────────────────────────────────
        public static GestorAcademico Gestor { get; private set; } = new();
        private static readonly string RutaDatos =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datos.json");

        // ── Controles ────────────────────────────────────────
        private MenuStrip menuPrincipal = null!;
        private StatusStrip statusBar = null!;
        private ToolStripStatusLabel lblEstado = null!;
        private ToolStripStatusLabel lblFecha = null!;
        private System.Windows.Forms.Timer reloj = null!;

        public MainForm()
        {
            InicializarComponentes();
            CargarDatosIniciales();
        }

        private void InicializarComponentes()
        {
            // ── Ventana ──────────────────────────────────────
            Text = "Sistema de Gestión Académica – Corporación Universitaria Vida Nueva";
            Size = new Size(1200, 750);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(240, 244, 248);

            // ── Menú principal ───────────────────────────────
            menuPrincipal = new MenuStrip
            {
                BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White,
                Padding = new Padding(5, 3, 0, 3),
                Font = new Font("Segoe UI", 10f)
            };

            // Menú Personas
            var mnuPersonas = CrearMenu("👤  Personas");
            mnuPersonas.DropDownItems.Add(CrearItem("Gestión de Estudiantes", (s, e) => AbrirFormulario<EstudianteForm>()));
            mnuPersonas.DropDownItems.Add(CrearItem("Gestión de Docentes", (s, e) => AbrirFormulario<DocenteForm>()));

            // Menú Académico
            var mnuAcademico = CrearMenu("📚  Académico");
            mnuAcademico.DropDownItems.Add(CrearItem("Gestión de Materias", (s, e) => AbrirFormulario<MateriaForm>()));
            mnuAcademico.DropDownItems.Add(CrearItem("Inscripciones", (s, e) => AbrirFormulario<InscripcionForm>()));
            mnuAcademico.DropDownItems.Add(CrearItem("Calificaciones", (s, e) => AbrirFormulario<CalificacionForm>()));

            // Menú Reportes
            var mnuReportes = CrearMenu("📊  Reportes");
            mnuReportes.DropDownItems.Add(CrearItem("Generar Reportes", (s, e) => AbrirFormulario<ReporteForm>()));

            // Menú Datos
            var mnuDatos = CrearMenu("💾  Datos");
            mnuDatos.DropDownItems.Add(CrearItem("Guardar datos", GuardarDatos));
            mnuDatos.DropDownItems.Add(CrearItem("Cargar datos", CargarDatos));
            mnuDatos.DropDownItems.Add(new ToolStripSeparator());
            mnuDatos.DropDownItems.Add(CrearItem("Salir", (s, e) => Close()));

            // Menú Ventana
            var mnuVentana = CrearMenu("🗗  Ventana");
            mnuVentana.DropDownItems.Add(CrearItem("Cascada", (s, e) => LayoutMdi(MdiLayout.Cascade)));
            mnuVentana.DropDownItems.Add(CrearItem("Mosaico horizontal", (s, e) => LayoutMdi(MdiLayout.TileHorizontal)));
            mnuVentana.DropDownItems.Add(CrearItem("Mosaico vertical", (s, e) => LayoutMdi(MdiLayout.TileVertical)));
            mnuVentana.DropDownItems.Add(CrearItem("Cerrar todas", (s, e) => CerrarTodasVentanas()));

            menuPrincipal.Items.AddRange(new ToolStripItem[]
                { mnuPersonas, mnuAcademico, mnuReportes, mnuDatos, mnuVentana });

            // Estilo del menú desplegable
            menuPrincipal.Renderer = new MenuRenderer();

            // ── Barra de estado ──────────────────────────────
            statusBar = new StatusStrip { BackColor = Color.FromArgb(30, 60, 114) };
            lblEstado = new ToolStripStatusLabel("✅ Sistema listo")
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f)
            };
            lblFecha = new ToolStripStatusLabel
            {
                Alignment = ToolStripItemAlignment.Right,
                ForeColor = Color.LightCyan,
                Font = new Font("Segoe UI", 9f)
            };
            var separador = new ToolStripStatusLabel { Spring = true };
            statusBar.Items.AddRange(new ToolStripItem[] { lblEstado, separador, lblFecha });

            // ── Reloj ────────────────────────────────────────
            reloj = new System.Windows.Forms.Timer { Interval = 1000 };
            reloj.Tick += (s, e) => lblFecha.Text = DateTime.Now.ToString("dddd dd/MM/yyyy  HH:mm:ss");
            reloj.Start();
            lblFecha.Text = DateTime.Now.ToString("dddd dd/MM/yyyy  HH:mm:ss");

            // ── Layout ───────────────────────────────────────
            MainMenuStrip = menuPrincipal;
            Controls.Add(menuPrincipal);
            Controls.Add(statusBar);

            // ── Evento cierre ────────────────────────────────
            FormClosing += MainForm_FormClosing;
        }

        // ═══════════════════════════════════════════════════
        //  HELPERS – Apertura de formularios
        // ═══════════════════════════════════════════════════

        private void AbrirFormulario<T>() where T : Form, new()
        {
            // Si ya está abierto, lo trae al frente
            foreach (Form hijo in MdiChildren)
            {
                if (hijo is T)
                {
                    hijo.BringToFront();
                    hijo.WindowState = FormWindowState.Normal;
                    return;
                }
            }

            var form = new T { MdiParent = this };
            form.Show();
        }

        // ═══════════════════════════════════════════════════
        //  PERSISTENCIA
        // ═══════════════════════════════════════════════════

        private void CargarDatosIniciales()
        {
            try
            {
                Gestor.CargarDatos(RutaDatos);
                ActualizarEstado("✅ Datos cargados correctamente");
            }
            catch
            {
                ActualizarEstado("⚠️ No se encontraron datos previos. Sistema iniciado vacío.");
            }
        }

        private void GuardarDatos(object? sender, EventArgs e)
        {
            try
            {
                Gestor.GuardarDatos(RutaDatos);
                ActualizarEstado("💾 Datos guardados correctamente");
                MessageBox.Show("Datos guardados correctamente.", "Guardar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "¿Desea cargar los datos guardados? Los cambios no guardados se perderán.",
                "Cargar datos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                CerrarTodasVentanas();
                Gestor = new GestorAcademico();
                Gestor.CargarDatos(RutaDatos);
                ActualizarEstado("📂 Datos cargados correctamente");
                MessageBox.Show("Datos cargados correctamente.", "Cargar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "¿Desea guardar los datos antes de salir?",
                "Salir", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) { e.Cancel = true; return; }
            if (result == DialogResult.Yes) Gestor.GuardarDatos(RutaDatos);
        }

        // ═══════════════════════════════════════════════════
        //  HELPERS – UI
        // ═══════════════════════════════════════════════════

        public void ActualizarEstado(string mensaje) => lblEstado.Text = mensaje;

        private void CerrarTodasVentanas()
        {
            foreach (Form hijo in MdiChildren.ToList())
                hijo.Close();
        }

        private static ToolStripMenuItem CrearMenu(string texto) =>
            new ToolStripMenuItem(texto) { ForeColor = Color.White, Font = new Font("Segoe UI", 10f) };

        private static ToolStripMenuItem CrearItem(string texto, EventHandler handler)
        {
            var item = new ToolStripMenuItem(texto) { Font = new Font("Segoe UI", 9.5f) };
            item.Click += handler;
            return item;
        }
    }

    // ── Renderer personalizado del menú ──────────────────
    internal class MenuRenderer : ToolStripProfessionalRenderer
    {
        public MenuRenderer() : base(new MenuColors()) { }
    }

    internal class MenuColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(59, 103, 187);
        public override Color MenuItemBorder => Color.FromArgb(59, 103, 187);
        public override Color MenuBorder => Color.FromArgb(30, 60, 114);
        public override Color ToolStripDropDownBackground => Color.FromArgb(245, 247, 250);
        public override Color ImageMarginGradientBegin => Color.FromArgb(230, 235, 245);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(230, 235, 245);
        public override Color ImageMarginGradientEnd => Color.FromArgb(230, 235, 245);
    }
}
