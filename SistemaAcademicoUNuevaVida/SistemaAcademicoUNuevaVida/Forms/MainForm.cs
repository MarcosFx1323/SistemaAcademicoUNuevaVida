using SistemaAcademicoUNuevaVida.Services;

namespace SistemaAcademicoUNuevaVida.Forms
{
    public partial class MainForm : Form
    {
        public static GestorAcademico Gestor { get; private set; } = new();
        private static readonly string RutaDatos =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datos.json");

        public MainForm()
        {
            InitializeComponent();
            CargarDatosIniciales();
            reloj.Start();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                Gestor.CargarDatos(RutaDatos);
                ActualizarEstado("✅ Datos cargados correctamente");
            }
            catch
            {
                ActualizarEstado("⚠️ Sistema iniciado sin datos previos.");
            }
        }

        private void AbrirFormulario<T>() where T : Form, new()
        {
            foreach (Form hijo in MdiChildren)
            {
                if (hijo is T) { hijo.BringToFront(); hijo.WindowState = FormWindowState.Normal; return; }
            }
            var form = new T { MdiParent = this };
            form.Show();
        }

        // ── Menú Personas ──────────────────────────────────
        private void mnuEstudiantes_Click(object sender, EventArgs e) => AbrirFormulario<EstudianteForm>();
        private void mnuDocentes_Click(object sender, EventArgs e) => AbrirFormulario<DocenteForm>();

        // ── Menú Académico ─────────────────────────────────
        private void mnuMaterias_Click(object sender, EventArgs e) => AbrirFormulario<MateriaForm>();
        private void mnuInscripciones_Click(object sender, EventArgs e) => AbrirFormulario<InscripcionForm>();
        private void mnuCalificaciones_Click(object sender, EventArgs e) => AbrirFormulario<CalificacionForm>();

        // ── Menú Reportes ──────────────────────────────────
        private void mnuReportes_Click(object sender, EventArgs e) => AbrirFormulario<ReporteForm>();

        // ── Menú Datos ─────────────────────────────────────
        private void mnuGuardar_Click(object sender, EventArgs e)
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

        private void mnuCargar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cargar datos guardados? Los cambios no guardados se perderán.",
                "Cargar datos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                foreach (Form hijo in MdiChildren.ToList()) hijo.Close();
                Gestor = new GestorAcademico();
                Gestor.CargarDatos(RutaDatos);
                ActualizarEstado("📂 Datos cargados correctamente");
                MessageBox.Show("Datos cargados.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuSalir_Click(object sender, EventArgs e) => Close();

        // ── Menú Ventana ───────────────────────────────────
        private void mnuCascada_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);
        private void mnuMosaicoH_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);
        private void mnuMosaicoV_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileVertical);
        private void mnuCerrarTodas_Click(object sender, EventArgs e)
        {
            foreach (Form hijo in MdiChildren.ToList()) hijo.Close();
        }

        // ── Reloj ──────────────────────────────────────────
        private void reloj_Tick(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dddd dd/MM/yyyy  HH:mm:ss");
        }

        // ── Cierre ─────────────────────────────────────────
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("¿Desea guardar los datos antes de salir?",
                "Salir", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Cancel) { e.Cancel = true; return; }
            if (res == DialogResult.Yes) Gestor.GuardarDatos(RutaDatos);
        }

        public void ActualizarEstado(string mensaje) => lblEstado.Text = mensaje;
    }
}
