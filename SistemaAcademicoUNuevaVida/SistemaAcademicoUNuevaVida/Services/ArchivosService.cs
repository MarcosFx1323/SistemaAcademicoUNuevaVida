using System.Text.Json;
using System.Text.Json.Serialization;
using SistemaAcademicoUNuevaVida.Models;

namespace SistemaAcademicoUNuevaVida.Services
{
    // ─────────────────────────────────────────────────────────
    //  DTO de serialización: empaqueta todo el sistema en un
    //  solo objeto para guardar/cargar como JSON.
    // ─────────────────────────────────────────────────────────
    public class DatosSistema
    {
        public List<Estudiante> Estudiantes { get; set; } = new();
        public List<Docente> Docentes { get; set; } = new();
        public List<Materia> Materias { get; set; } = new();
        public List<Inscripcion> Inscripciones { get; set; } = new();
        public List<Calificacion> Calificaciones { get; set; } = new();
        public DateTime UltimoGuardado { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Servicio responsable de la lectura y escritura de datos
    /// en disco usando System.Text.Json (.NET 8).
    /// </summary>
    public class ArchivosService
    {
        // Opciones compartidas de serialización
        private static readonly JsonSerializerOptions _opciones = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve, // Maneja referencias circulares
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // ═══════════════════════════════════════════════════════
        //  GUARDAR
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Serializa todo el estado del sistema en un archivo JSON.
        /// Crea el directorio si no existe.
        /// </summary>
        public void GuardarDatos(string ruta, GestorAcademico gestor)
        {
            try
            {
                string? directorio = Path.GetDirectoryName(ruta);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                var datos = new DatosSistema
                {
                    Estudiantes = gestor.Estudiantes,
                    Docentes = gestor.Docentes,
                    Materias = gestor.Materias,
                    Inscripciones = gestor.Inscripciones,
                    Calificaciones = gestor.Calificaciones,
                    UltimoGuardado = DateTime.Now
                };

                string json = JsonSerializer.Serialize(datos, _opciones);
                File.WriteAllText(ruta, json, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar los datos: {ex.Message}", ex);
            }
        }

        // ═══════════════════════════════════════════════════════
        //  CARGAR
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Deserializa el archivo JSON y retorna el estado completo del sistema.
        /// Retorna null si el archivo no existe o está corrupto.
        /// </summary>
        public DatosSistema? CargarDatos(string ruta)
        {
            if (!File.Exists(ruta))
                return null;

            try
            {
                string json = File.ReadAllText(ruta, System.Text.Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return JsonSerializer.Deserialize<DatosSistema>(json, _opciones);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"El archivo de datos está corrupto o tiene un formato inválido: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al cargar los datos: {ex.Message}", ex);
            }
        }

        // ═══════════════════════════════════════════════════════
        //  EXPORTAR REPORTES
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Exporta el contenido de un reporte como archivo .txt.
        /// El usuario elige la ruta mediante SaveFileDialog en el formulario.
        /// </summary>
        public (bool exito, string mensaje) ExportarReporteTxt(string ruta, string contenido)
        {
            try
            {
                File.WriteAllText(ruta, contenido, System.Text.Encoding.UTF8);
                return (true, $"Reporte exportado correctamente en: {ruta}");
            }
            catch (Exception ex)
            {
                return (false, $"Error al exportar el reporte: {ex.Message}");
            }
        }

        /// <summary>
        /// Exporta una lista de inscripciones como archivo .csv.
        /// Columnas: Estudiante, ID, Materia, Semestre, Nota1, Nota2, Nota3, Promedio, Estado.
        /// </summary>
        public (bool exito, string mensaje) ExportarInscritosCsv(
            string ruta, List<Inscripcion> inscripciones)
        {
            try
            {
                var lineas = new List<string>
                {
                    "Estudiante,ID,Materia,Semestre,Nota1,Nota2,Nota3,Promedio,Estado"
                };

                foreach (var ins in inscripciones)
                {
                    string nota1 = ins.Calificacion?.Nota1.ToString("F2") ?? "-";
                    string nota2 = ins.Calificacion?.Nota2.ToString("F2") ?? "-";
                    string nota3 = ins.Calificacion?.Nota3.ToString("F2") ?? "-";
                    string promedio = ins.Calificacion?.PromedioFinal.ToString("F2") ?? "-";
                    string estado = ins.Calificacion?.EstadoAcademico ?? "Sin calificar";

                    lineas.Add(string.Join(",",
                        EscaparCsv(ins.Estudiante.Nombre),
                        EscaparCsv(ins.Estudiante.Id),
                        EscaparCsv(ins.Materia.Nombre),
                        ins.Materia.Semestre,
                        nota1, nota2, nota3, promedio,
                        estado));
                }

                File.WriteAllLines(ruta, lineas, System.Text.Encoding.UTF8);
                return (true, $"CSV exportado correctamente en: {ruta}");
            }
            catch (Exception ex)
            {
                return (false, $"Error al exportar CSV: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════
        //  BACKUP AUTOMÁTICO
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Crea una copia de seguridad del archivo de datos con timestamp.
        /// Útil para ejecutar antes de cada guardado.
        /// </summary>
        public bool CrearBackup(string rutaOriginal)
        {
            if (!File.Exists(rutaOriginal))
                return false;

            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string directorio = Path.GetDirectoryName(rutaOriginal)
                    ?? AppDomain.CurrentDomain.BaseDirectory;
                string nombreBackup = $"datos_backup_{timestamp}.json";
                string rutaBackup = Path.Combine(directorio, "backups", nombreBackup);

                Directory.CreateDirectory(Path.GetDirectoryName(rutaBackup)!);
                File.Copy(rutaOriginal, rutaBackup, overwrite: true);
                return true;
            }
            catch
            {
                return false; // El backup falla silenciosamente; no debe interrumpir el flujo
            }
        }

        // ═══════════════════════════════════════════════════════
        //  HELPERS PRIVADOS
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Escapa un campo para CSV envolviéndolo en comillas si contiene comas o comillas.
        /// </summary>
        private static string EscaparCsv(string valor)
        {
            if (valor.Contains(',') || valor.Contains('"') || valor.Contains('\n'))
                return $"\"{valor.Replace("\"", "\"\"")}\"";
            return valor;
        }
    }
}
