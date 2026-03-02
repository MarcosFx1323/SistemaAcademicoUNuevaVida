namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Representa un estudiante matriculado en la universidad.
    /// Hereda de Persona.
    /// </summary>
    public class Estudiante : Persona
    {
        public string Programa { get; set; } = string.Empty;
        public int Semestre { get; set; }

        /// <summary>
        /// Lista de inscripciones activas y canceladas del estudiante.
        /// </summary>
        public List<Inscripcion> Inscripciones { get; set; } = new();

        /// <summary>
        /// Retorna solo las inscripciones con estado Activa.
        /// </summary>
        public IEnumerable<Inscripcion> InscripcionesActivas =>
            Inscripciones.Where(i => i.Estado == EstadoInscripcion.Activa);

        /// <summary>
        /// Calcula el promedio general del estudiante sobre todas sus calificaciones.
        /// Retorna 0 si no tiene ninguna calificación registrada.
        /// </summary>
        public double PromedioGeneral
        {
            get
            {
                var calificaciones = Inscripciones
                    .Where(i => i.Calificacion != null)
                    .Select(i => i.Calificacion!.PromedioFinal)
                    .ToList();

                return calificaciones.Count > 0
                    ? Math.Round(calificaciones.Average(), 2)
                    : 0;
            }
        }

        public override string ObtenerInfo() =>
            $"{Nombre} | {Programa} | Semestre {Semestre}";
    }
}
