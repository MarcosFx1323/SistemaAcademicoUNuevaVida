namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Representa un docente de la universidad.
    /// Hereda de Persona.
    /// </summary>
    public class Docente : Persona
    {
        public string Titulo { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;

        /// <summary>
        /// Lista de materias que tiene asignadas este docente.
        /// </summary>
        public List<Materia> MateriasAsignadas { get; set; } = new();

        /// <summary>
        /// Cantidad de materias actualmente asignadas.
        /// </summary>
        public int TotalMaterias => MateriasAsignadas.Count;

        public override string ObtenerInfo() =>
            $"{Nombre} | {Titulo} | Depto: {Departamento}";
    }
}
