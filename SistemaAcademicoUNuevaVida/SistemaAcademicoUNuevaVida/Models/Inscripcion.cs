namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Estado posible de una inscripción.
    /// </summary>
    public enum EstadoInscripcion
    {
        Activa,
        Cancelada
    }

    /// <summary>
    /// Representa la relación entre un Estudiante y una Materia.
    /// Contiene la fecha de inscripción, el estado y la calificación asociada.
    /// </summary>
    public class Inscripcion
    {
        public Estudiante Estudiante { get; set; } = null!;
        public Materia Materia { get; set; } = null!;
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
        public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Activa;

        /// <summary>
        /// Calificación asociada a esta inscripción.
        /// Puede ser null si aún no se han registrado notas.
        /// </summary>
        public Calificacion? Calificacion { get; set; }

        /// <summary>
        /// Indica si la inscripción ya tiene calificaciones registradas.
        /// </summary>
        public bool TieneCalificacion => Calificacion != null;

        public override string ToString() =>
            $"{Estudiante.Nombre} → {Materia.Nombre} | {Estado} | {FechaInscripcion:dd/MM/yyyy}";
    }
}
