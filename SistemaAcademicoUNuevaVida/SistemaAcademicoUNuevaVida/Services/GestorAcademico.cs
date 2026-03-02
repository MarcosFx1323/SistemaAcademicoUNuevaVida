using SistemaAcademicoUNuevaVida.Models;

namespace SistemaAcademicoUNuevaVida.Services
{
    /// <summary>
    /// Controlador principal del sistema académico.
    /// Centraliza toda la lógica de negocio y sirve como puente
    /// entre los formularios y los datos del sistema.
    /// </summary>
    public class GestorAcademico
    {
        // ─────────────────────────────────────────────
        // Listas principales (fuente de verdad en memoria)
        // ─────────────────────────────────────────────
        public List<Estudiante> Estudiantes { get; private set; } = new();
        public List<Docente> Docentes { get; private set; } = new();
        public List<Materia> Materias { get; private set; } = new();
        public List<Inscripcion> Inscripciones { get; private set; } = new();
        public List<Calificacion> Calificaciones { get; private set; } = new();

        private readonly ArchivosService _archivos;

        public GestorAcademico()
        {
            _archivos = new ArchivosService();
        }

        // ═══════════════════════════════════════════════════════
        //  MÓDULO 1 – GESTIÓN DE ESTUDIANTES
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Agrega un nuevo estudiante validando que el ID no esté duplicado.
        /// </summary>
        public (bool exito, string mensaje) AgregarEstudiante(Estudiante estudiante)
        {
            if (string.IsNullOrWhiteSpace(estudiante.Id))
                return (false, "El ID del estudiante no puede estar vacío.");

            if (Estudiantes.Any(e => e.Id == estudiante.Id))
                return (false, $"Ya existe un estudiante con el ID '{estudiante.Id}'.");

            if (string.IsNullOrWhiteSpace(estudiante.Nombre))
                return (false, "El nombre del estudiante es obligatorio.");

            if (estudiante.Semestre < 1 || estudiante.Semestre > 10)
                return (false, "El semestre debe estar entre 1 y 10.");

            Estudiantes.Add(estudiante);
            return (true, "Estudiante registrado correctamente.");
        }

        /// <summary>
        /// Actualiza los datos de un estudiante existente (busca por ID).
        /// </summary>
        public (bool exito, string mensaje) ActualizarEstudiante(Estudiante actualizado)
        {
            var existente = Estudiantes.FirstOrDefault(e => e.Id == actualizado.Id);
            if (existente == null)
                return (false, "No se encontró el estudiante a actualizar.");

            existente.Nombre = actualizado.Nombre;
            existente.Email = actualizado.Email;
            existente.Telefono = actualizado.Telefono;
            existente.FechaNacimiento = actualizado.FechaNacimiento;
            existente.Programa = actualizado.Programa;
            existente.Semestre = actualizado.Semestre;

            return (true, "Estudiante actualizado correctamente.");
        }

        /// <summary>
        /// Elimina un estudiante solo si no tiene inscripciones activas.
        /// </summary>
        public (bool exito, string mensaje) EliminarEstudiante(string id)
        {
            var estudiante = Estudiantes.FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
                return (false, "Estudiante no encontrado.");

            if (estudiante.InscripcionesActivas.Any())
                return (false, "No se puede eliminar: el estudiante tiene inscripciones activas.");

            Estudiantes.Remove(estudiante);
            return (true, "Estudiante eliminado correctamente.");
        }

        /// <summary>
        /// Busca estudiantes por nombre o ID (búsqueda parcial, sin distinción de mayúsculas).
        /// </summary>
        public List<Estudiante> BuscarEstudiantes(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Estudiantes;

            filtro = filtro.ToLower();
            return Estudiantes
                .Where(e => e.Nombre.ToLower().Contains(filtro)
                         || e.Id.ToLower().Contains(filtro)
                         || e.Programa.ToLower().Contains(filtro))
                .ToList();
        }

        // ═══════════════════════════════════════════════════════
        //  MÓDULO 1 – GESTIÓN DE DOCENTES
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Agrega un nuevo docente validando ID único y campos obligatorios.
        /// </summary>
        public (bool exito, string mensaje) AgregarDocente(Docente docente)
        {
            if (string.IsNullOrWhiteSpace(docente.Id))
                return (false, "La cédula del docente no puede estar vacía.");

            if (Docentes.Any(d => d.Id == docente.Id))
                return (false, $"Ya existe un docente con la cédula '{docente.Id}'.");

            if (string.IsNullOrWhiteSpace(docente.Nombre))
                return (false, "El nombre del docente es obligatorio.");

            Docentes.Add(docente);
            return (true, "Docente registrado correctamente.");
        }

        /// <summary>
        /// Actualiza los datos de un docente existente.
        /// </summary>
        public (bool exito, string mensaje) ActualizarDocente(Docente actualizado)
        {
            var existente = Docentes.FirstOrDefault(d => d.Id == actualizado.Id);
            if (existente == null)
                return (false, "No se encontró el docente a actualizar.");

            existente.Nombre = actualizado.Nombre;
            existente.Email = actualizado.Email;
            existente.Telefono = actualizado.Telefono;
            existente.FechaNacimiento = actualizado.FechaNacimiento;
            existente.Titulo = actualizado.Titulo;
            existente.Departamento = actualizado.Departamento;

            return (true, "Docente actualizado correctamente.");
        }

        /// <summary>
        /// Elimina un docente solo si no tiene materias asignadas.
        /// </summary>
        public (bool exito, string mensaje) EliminarDocente(string id)
        {
            var docente = Docentes.FirstOrDefault(d => d.Id == id);
            if (docente == null)
                return (false, "Docente no encontrado.");

            if (docente.MateriasAsignadas.Any())
                return (false, "No se puede eliminar: el docente tiene materias asignadas.");

            Docentes.Remove(docente);
            return (true, "Docente eliminado correctamente.");
        }

        /// <summary>
        /// Busca docentes por nombre, cédula o departamento.
        /// </summary>
        public List<Docente> BuscarDocentes(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Docentes;

            filtro = filtro.ToLower();
            return Docentes
                .Where(d => d.Nombre.ToLower().Contains(filtro)
                         || d.Id.ToLower().Contains(filtro)
                         || d.Departamento.ToLower().Contains(filtro))
                .ToList();
        }

        // ═══════════════════════════════════════════════════════
        //  MÓDULO 2 – GESTIÓN DE MATERIAS
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Agrega una nueva materia validando código único y docente responsable.
        /// </summary>
        public (bool exito, string mensaje) AgregarMateria(Materia materia)
        {
            if (string.IsNullOrWhiteSpace(materia.Codigo))
                return (false, "El código de la materia no puede estar vacío.");

            if (Materias.Any(m => m.Codigo == materia.Codigo))
                return (false, $"Ya existe una materia con el código '{materia.Codigo}'.");

            if (materia.CupoMaximo <= 0)
                return (false, "El cupo máximo debe ser mayor a 0.");

            if (materia.Creditos <= 0)
                return (false, "Los créditos deben ser mayor a 0.");

            Materias.Add(materia);

            // Registrar la materia en la lista del docente responsable
            if (materia.DocenteResponsable != null)
                materia.DocenteResponsable.MateriasAsignadas.Add(materia);

            return (true, "Materia registrada correctamente.");
        }

        /// <summary>
        /// Actualiza los datos de una materia existente.
        /// </summary>
        public (bool exito, string mensaje) ActualizarMateria(Materia actualizada)
        {
            var existente = Materias.FirstOrDefault(m => m.Codigo == actualizada.Codigo);
            if (existente == null)
                return (false, "Materia no encontrada.");

            // Si cambia el docente, actualizar las listas de ambos docentes
            if (existente.DocenteResponsable != null &&
                existente.DocenteResponsable.Id != actualizada.DocenteResponsable?.Id)
            {
                existente.DocenteResponsable.MateriasAsignadas.Remove(existente);
            }

            existente.Nombre = actualizada.Nombre;
            existente.Creditos = actualizada.Creditos;
            existente.Semestre = actualizada.Semestre;
            existente.CupoMaximo = actualizada.CupoMaximo;
            existente.DocenteResponsable = actualizada.DocenteResponsable;

            if (actualizada.DocenteResponsable != null &&
                !actualizada.DocenteResponsable.MateriasAsignadas.Contains(existente))
            {
                actualizada.DocenteResponsable.MateriasAsignadas.Add(existente);
            }

            return (true, "Materia actualizada correctamente.");
        }

        /// <summary>
        /// Elimina una materia solo si no tiene inscripciones activas.
        /// </summary>
        public (bool exito, string mensaje) EliminarMateria(string codigo)
        {
            var materia = Materias.FirstOrDefault(m => m.Codigo == codigo);
            if (materia == null)
                return (false, "Materia no encontrada.");

            if (materia.Inscripciones.Any(i => i.Estado == EstadoInscripcion.Activa))
                return (false, "No se puede eliminar: la materia tiene estudiantes inscritos.");

            materia.DocenteResponsable?.MateriasAsignadas.Remove(materia);
            Materias.Remove(materia);
            return (true, "Materia eliminada correctamente.");
        }

        /// <summary>
        /// Filtra materias por semestre. Retorna todas si semestre = 0.
        /// </summary>
        public List<Materia> FiltrarMateriasPorSemestre(int semestre)
        {
            return semestre == 0
                ? Materias
                : Materias.Where(m => m.Semestre == semestre).ToList();
        }

        /// <summary>
        /// Retorna materias que aún tienen cupo disponible.
        /// </summary>
        public List<Materia> ObtenerMateriasConCupo() =>
            Materias.Where(m => m.TieneCupo).ToList();

        // ═══════════════════════════════════════════════════════
        //  MÓDULO 3 – GESTIÓN DE INSCRIPCIONES
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Inscribe un estudiante en una materia aplicando todas las reglas de negocio:
        /// 1. Cupo disponible en la materia.
        /// 2. El estudiante no esté ya inscrito en la misma materia.
        /// 3. La materia corresponda al semestre del estudiante.
        /// </summary>
        public (bool exito, string mensaje) InscribirEstudiante(Estudiante estudiante, Materia materia)
        {
            if (estudiante == null || materia == null)
                return (false, "Estudiante o materia no válidos.");

            // Regla 1: cupo disponible
            if (!materia.TieneCupo)
                return (false, $"La materia '{materia.Nombre}' no tiene cupos disponibles ({materia.Inscritos}/{materia.CupoMaximo}).");

            // Regla 2: no duplicados
            bool yaInscrito = estudiante.Inscripciones
                .Any(i => i.Materia.Codigo == materia.Codigo
                       && i.Estado == EstadoInscripcion.Activa);

            if (yaInscrito)
                return (false, $"El estudiante '{estudiante.Nombre}' ya está inscrito en '{materia.Nombre}'.");

            // Regla 3: semestre correspondiente
            if (materia.Semestre != estudiante.Semestre)
                return (false, $"La materia es de semestre {materia.Semestre} pero el estudiante está en semestre {estudiante.Semestre}.");

            // Crear inscripción
            var inscripcion = new Inscripcion
            {
                Estudiante = estudiante,
                Materia = materia,
                FechaInscripcion = DateTime.Now,
                Estado = EstadoInscripcion.Activa
            };

            estudiante.Inscripciones.Add(inscripcion);
            materia.Inscripciones.Add(inscripcion);
            Inscripciones.Add(inscripcion);

            return (true, $"Estudiante '{estudiante.Nombre}' inscrito exitosamente en '{materia.Nombre}'.");
        }

        /// <summary>
        /// Cancela una inscripción activa por referencia directa.
        /// </summary>
        public (bool exito, string mensaje) CancelarInscripcion(Inscripcion inscripcion)
        {
            if (inscripcion == null)
                return (false, "Inscripción no válida.");

            if (inscripcion.Estado == EstadoInscripcion.Cancelada)
                return (false, "La inscripción ya se encuentra cancelada.");

            inscripcion.Estado = EstadoInscripcion.Cancelada;
            return (true, $"Inscripción cancelada: {inscripcion.Estudiante.Nombre} en {inscripcion.Materia.Nombre}.");
        }

        /// <summary>
        /// Retorna los estudiantes inscritos (activos) en una materia específica.
        /// </summary>
        public List<Inscripcion> ObtenerInscritosPorMateria(string codigoMateria) =>
            Inscripciones
                .Where(i => i.Materia.Codigo == codigoMateria
                         && i.Estado == EstadoInscripcion.Activa)
                .ToList();

        // ═══════════════════════════════════════════════════════
        //  MÓDULO 4 – REGISTRO DE CALIFICACIONES
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Registra o actualiza las calificaciones de una inscripción activa.
        /// Valida que las notas estén en el rango 0.0 – 5.0.
        /// </summary>
        public (bool exito, string mensaje) RegistrarCalificacion(
            Inscripcion inscripcion, double nota1, double nota2, double nota3)
        {
            if (inscripcion == null)
                return (false, "Inscripción no válida.");

            if (inscripcion.Estado == EstadoInscripcion.Cancelada)
                return (false, "No se pueden registrar notas en una inscripción cancelada.");

            if (!NotaValida(nota1) || !NotaValida(nota2) || !NotaValida(nota3))
                return (false, "Las notas deben estar entre 0.0 y 5.0.");

            if (inscripcion.Calificacion == null)
            {
                var calificacion = new Calificacion
                {
                    Inscripcion = inscripcion,
                    Nota1 = nota1,
                    Nota2 = nota2,
                    Nota3 = nota3
                };
                inscripcion.Calificacion = calificacion;
                Calificaciones.Add(calificacion);
            }
            else
            {
                // Actualizar notas existentes
                inscripcion.Calificacion.Nota1 = nota1;
                inscripcion.Calificacion.Nota2 = nota2;
                inscripcion.Calificacion.Nota3 = nota3;
            }

            return (true, $"Calificaciones registradas. Promedio: {inscripcion.Calificacion.PromedioFinal} ({inscripcion.Calificacion.EstadoAcademico})");
        }

        /// <summary>
        /// Obtiene todas las calificaciones de un estudiante específico.
        /// </summary>
        public List<Calificacion> ObtenerCalificacionesPorEstudiante(string idEstudiante) =>
            Calificaciones
                .Where(c => c.Inscripcion.Estudiante.Id == idEstudiante)
                .ToList();

        /// <summary>
        /// Calcula el promedio general de un estudiante en todas sus materias calificadas.
        /// </summary>
        public double CalcularPromedioEstudiante(string idEstudiante)
        {
            var notas = Calificaciones
                .Where(c => c.Inscripcion.Estudiante.Id == idEstudiante)
                .Select(c => c.PromedioFinal)
                .ToList();

            return notas.Count > 0 ? Math.Round(notas.Average(), 2) : 0;
        }

        // ═══════════════════════════════════════════════════════
        //  MÓDULO 5 – REPORTES
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Genera el historial académico completo de un estudiante
        /// con todas sus materias, notas y promedios.
        /// </summary>
        public string GenerarHistorialEstudiante(string idEstudiante)
        {
            var estudiante = Estudiantes.FirstOrDefault(e => e.Id == idEstudiante);
            if (estudiante == null)
                return "Estudiante no encontrado.";

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine($"  HISTORIAL ACADÉMICO");
            sb.AppendLine($"  Estudiante : {estudiante.Nombre}");
            sb.AppendLine($"  ID         : {estudiante.Id}");
            sb.AppendLine($"  Programa   : {estudiante.Programa}");
            sb.AppendLine($"  Semestre   : {estudiante.Semestre}");
            sb.AppendLine("═══════════════════════════════════════════════════");

            if (!estudiante.Inscripciones.Any())
            {
                sb.AppendLine("  Sin inscripciones registradas.");
                return sb.ToString();
            }

            foreach (var ins in estudiante.Inscripciones.OrderBy(i => i.Materia.Semestre))
            {
                sb.AppendLine($"\n  Materia  : [{ins.Materia.Codigo}] {ins.Materia.Nombre}");
                sb.AppendLine($"  Semestre : {ins.Materia.Semestre} | Estado: {ins.Estado}");

                if (ins.Calificacion != null)
                {
                    var cal = ins.Calificacion;
                    sb.AppendLine($"  Notas    : {cal.Nota1} / {cal.Nota2} / {cal.Nota3}");
                    sb.AppendLine($"  Promedio : {cal.PromedioFinal}  →  {cal.EstadoAcademico}");
                }
                else
                {
                    sb.AppendLine("  Notas    : Pendientes de registro");
                }
            }

            sb.AppendLine("\n───────────────────────────────────────────────────");
            sb.AppendLine($"  Promedio General: {estudiante.PromedioGeneral}");
            sb.AppendLine("═══════════════════════════════════════════════════");

            return sb.ToString();
        }

        /// <summary>
        /// Genera el listado de inscritos de una materia con sus calificaciones.
        /// </summary>
        public string GenerarReporteInscritosPorMateria(string codigoMateria)
        {
            var materia = Materias.FirstOrDefault(m => m.Codigo == codigoMateria);
            if (materia == null)
                return "Materia no encontrada.";

            var inscritos = ObtenerInscritosPorMateria(codigoMateria);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine($"  INSCRITOS EN: [{materia.Codigo}] {materia.Nombre}");
            sb.AppendLine($"  Docente : {materia.DocenteResponsable?.Nombre ?? "Sin asignar"}");
            sb.AppendLine($"  Cupo    : {materia.Inscritos}/{materia.CupoMaximo}");
            sb.AppendLine("═══════════════════════════════════════════════════");

            if (!inscritos.Any())
            {
                sb.AppendLine("  Sin estudiantes inscritos.");
                return sb.ToString();
            }

            int num = 1;
            foreach (var ins in inscritos.OrderBy(i => i.Estudiante.Nombre))
            {
                sb.AppendLine($"\n  {num++}. {ins.Estudiante.Nombre} ({ins.Estudiante.Id})");

                if (ins.Calificacion != null)
                    sb.AppendLine($"     Promedio: {ins.Calificacion.PromedioFinal} → {ins.Calificacion.EstadoAcademico}");
                else
                    sb.AppendLine("     Promedio: Pendiente");
            }

            sb.AppendLine("═══════════════════════════════════════════════════");
            return sb.ToString();
        }

        /// <summary>
        /// Genera el ranking de estudiantes ordenado por promedio general (mayor a menor).
        /// </summary>
        public string GenerarRankingPorSemestre(int semestre)
        {
            var estudiantesSemestre = Estudiantes
                .Where(e => e.Semestre == semestre)
                .OrderByDescending(e => e.PromedioGeneral)
                .ToList();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════════════");
            sb.AppendLine($"  RANKING ACADÉMICO – SEMESTRE {semestre}");
            sb.AppendLine("═══════════════════════════════════════════════════");

            if (!estudiantesSemestre.Any())
            {
                sb.AppendLine("  Sin estudiantes en este semestre.");
                return sb.ToString();
            }

            int pos = 1;
            foreach (var e in estudiantesSemestre)
            {
                sb.AppendLine($"  #{pos++}  {e.Nombre} — Promedio: {e.PromedioGeneral}");
            }

            sb.AppendLine("═══════════════════════════════════════════════════");
            return sb.ToString();
        }

        // ═══════════════════════════════════════════════════════
        //  PERSISTENCIA – Delega en ArchivosService
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Guarda todos los datos del sistema en el archivo JSON.
        /// </summary>
        public void GuardarDatos(string ruta) =>
            _archivos.GuardarDatos(ruta, this);

        /// <summary>
        /// Carga los datos desde el archivo JSON y los restaura en memoria.
        /// </summary>
        public void CargarDatos(string ruta)
        {
            var datos = _archivos.CargarDatos(ruta);
            if (datos == null) return;

            Estudiantes = datos.Estudiantes;
            Docentes = datos.Docentes;
            Materias = datos.Materias;
            Inscripciones = datos.Inscripciones;
            Calificaciones = datos.Calificaciones;
        }

        // ═══════════════════════════════════════════════════════
        //  HELPERS PRIVADOS
        // ═══════════════════════════════════════════════════════

        private static bool NotaValida(double nota) => nota >= 0.0 && nota <= 5.0;
    }
}
