namespace SistemaAcademicoUNuevaVida.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            menuPrincipal   = new MenuStrip();
            statusBar       = new StatusStrip();
            lblEstado       = new ToolStripStatusLabel();
            lblFecha        = new ToolStripStatusLabel();
            reloj           = new System.Windows.Forms.Timer(components);

            // ── Menú Personas ──────────────────────────────
            mnuPersonas     = new ToolStripMenuItem("👤  Personas");
            mnuEstudiantes  = new ToolStripMenuItem("Gestión de Estudiantes");
            mnuDocentes     = new ToolStripMenuItem("Gestión de Docentes");
            mnuPersonas.DropDownItems.AddRange(new ToolStripItem[] { mnuEstudiantes, mnuDocentes });

            // ── Menú Académico ─────────────────────────────
            mnuAcademico    = new ToolStripMenuItem("📚  Académico");
            mnuMaterias     = new ToolStripMenuItem("Gestión de Materias");
            mnuInscripciones = new ToolStripMenuItem("Inscripciones");
            mnuCalificaciones = new ToolStripMenuItem("Calificaciones");
            mnuAcademico.DropDownItems.AddRange(new ToolStripItem[] { mnuMaterias, mnuInscripciones, mnuCalificaciones });

            // ── Menú Reportes ──────────────────────────────
            mnuMenuReportes = new ToolStripMenuItem("📊  Reportes");
            mnuReportes     = new ToolStripMenuItem("Generar Reportes");
            mnuMenuReportes.DropDownItems.Add(mnuReportes);

            // ── Menú Datos ─────────────────────────────────
            mnuDatos        = new ToolStripMenuItem("💾  Datos");
            mnuGuardar      = new ToolStripMenuItem("Guardar datos");
            mnuCargar       = new ToolStripMenuItem("Cargar datos");
            mnuSalir        = new ToolStripMenuItem("Salir");
            mnuDatos.DropDownItems.AddRange(new ToolStripItem[] { mnuGuardar, mnuCargar, new ToolStripSeparator(), mnuSalir });

            // ── Menú Ventana ───────────────────────────────
            mnuVentana      = new ToolStripMenuItem("🗗  Ventana");
            mnuCascada      = new ToolStripMenuItem("Cascada");
            mnuMosaicoH     = new ToolStripMenuItem("Mosaico horizontal");
            mnuMosaicoV     = new ToolStripMenuItem("Mosaico vertical");
            mnuCerrarTodas  = new ToolStripMenuItem("Cerrar todas");
            mnuVentana.DropDownItems.AddRange(new ToolStripItem[]
                { mnuCascada, mnuMosaicoH, mnuMosaicoV, new ToolStripSeparator(), mnuCerrarTodas });

            // ── Barra de menú ──────────────────────────────
            menuPrincipal.BackColor = Color.FromArgb(30, 60, 114);
            menuPrincipal.ForeColor = Color.White;
            menuPrincipal.Font = new Font("Segoe UI", 10f);
            menuPrincipal.Items.AddRange(new ToolStripItem[]
                { mnuPersonas, mnuAcademico, mnuMenuReportes, mnuDatos, mnuVentana });

            foreach (ToolStripMenuItem m in menuPrincipal.Items.OfType<ToolStripMenuItem>())
                m.ForeColor = Color.White;

            // ── Status bar ─────────────────────────────────
            var separadorStatus = new ToolStripStatusLabel { Spring = true };
            lblEstado.Text = "✅ Sistema listo";
            lblEstado.ForeColor = Color.White;
            lblFecha.ForeColor = Color.LightCyan;
            lblFecha.Alignment = ToolStripItemAlignment.Right;
            statusBar.BackColor = Color.FromArgb(30, 60, 114);
            statusBar.Items.AddRange(new ToolStripItem[] { lblEstado, separadorStatus, lblFecha });

            // ── Reloj ──────────────────────────────────────
            reloj.Interval = 1000;
            reloj.Tick += reloj_Tick;

            // ── Eventos ────────────────────────────────────
            mnuEstudiantes.Click   += mnuEstudiantes_Click;
            mnuDocentes.Click      += mnuDocentes_Click;
            mnuMaterias.Click      += mnuMaterias_Click;
            mnuInscripciones.Click += mnuInscripciones_Click;
            mnuCalificaciones.Click += mnuCalificaciones_Click;
            mnuReportes.Click      += mnuReportes_Click;
            mnuGuardar.Click       += mnuGuardar_Click;
            mnuCargar.Click        += mnuCargar_Click;
            mnuSalir.Click         += mnuSalir_Click;
            mnuCascada.Click       += mnuCascada_Click;
            mnuMosaicoH.Click      += mnuMosaicoH_Click;
            mnuMosaicoV.Click      += mnuMosaicoV_Click;
            mnuCerrarTodas.Click   += mnuCerrarTodas_Click;
            FormClosing            += MainForm_FormClosing;

            // ── Propiedades del Form ───────────────────────
            SuspendLayout();
            Text = "Sistema de Gestión Académica – Corporación Universitaria Vida Nueva";
            Size = new Size(1200, 750);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(240, 244, 248);
            MainMenuStrip = menuPrincipal;
            Controls.Add(menuPrincipal);
            Controls.Add(statusBar);
            ResumeLayout(false);
            PerformLayout();
        }

        // ── Declaración de controles ───────────────────────
        private MenuStrip menuPrincipal;
        private StatusStrip statusBar;
        private ToolStripStatusLabel lblEstado;
        private ToolStripStatusLabel lblFecha;
        private System.Windows.Forms.Timer reloj;

        private ToolStripMenuItem mnuPersonas, mnuEstudiantes, mnuDocentes;
        private ToolStripMenuItem mnuAcademico, mnuMaterias, mnuInscripciones, mnuCalificaciones;
        private ToolStripMenuItem mnuMenuReportes, mnuReportes;
        private ToolStripMenuItem mnuDatos, mnuGuardar, mnuCargar, mnuSalir;
        private ToolStripMenuItem mnuVentana, mnuCascada, mnuMosaicoH, mnuMosaicoV, mnuCerrarTodas;
    }
}
