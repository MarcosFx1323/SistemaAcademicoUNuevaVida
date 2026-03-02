namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Representa una materia o asignatura del catálogo académico.
    /// </summary>
    public class Materia
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }
        public int Semestre { get; set; }
        public int CupoMaximo { get; set; }

        /// <summary>
        /// Docente responsable de la materia (relación de composición).
        /// </summary>
        public Docente? DocenteResponsable { get; set; }

        /// <summary>
        /// Lista de inscripciones asociadas a esta materia.
        /// Se usa para calcular el cupo disponible.
        /// </summary>
        public List<Inscripcion> Inscripciones { get; set; } = new();

        /// <summary>
        /// Cantidad de estudiantes actualmente inscritos (estado Activa).
        /// </summary>
        public int Inscritos =>
            Inscripciones.Count(i => i.Estado == EstadoInscripcion.Activa);

        /// <summary>
        /// Cupos restantes disponibles para nuevas inscripciones.
        /// </summary>
        public int CuposDisponibles => CupoMaximo - Inscritos;

        /// <summary>
        /// Indica si aún hay cupo para inscribir más estudiantes.
        /// </summary>
        public bool TieneCupo => CuposDisponibles > 0;

        public override string ToString() =>
            $"[{Codigo}] {Nombre} - Semestre {Semestre} | Créditos: {Creditos} | Cupo: {Inscritos}/{CupoMaximo}";
    }
}
