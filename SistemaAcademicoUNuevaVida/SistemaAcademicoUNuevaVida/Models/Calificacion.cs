namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Registra las tres notas parciales de un estudiante en una materia.
    /// El promedio y el estado académico se calculan automáticamente.
    /// </summary>
    public class Calificacion
    {
        public Inscripcion Inscripcion { get; set; } = null!;

        private double _nota1;
        private double _nota2;
        private double _nota3;

        /// <summary>
        /// Primera nota parcial. Rango válido: 0.0 a 5.0
        /// </summary>
        public double Nota1
        {
            get => _nota1;
            set => _nota1 = ValidarNota(value);
        }

        /// <summary>
        /// Segunda nota parcial. Rango válido: 0.0 a 5.0
        /// </summary>
        public double Nota2
        {
            get => _nota2;
            set => _nota2 = ValidarNota(value);
        }

        /// <summary>
        /// Tercera nota parcial. Rango válido: 0.0 a 5.0
        /// </summary>
        public double Nota3
        {
            get => _nota3;
            set => _nota3 = ValidarNota(value);
        }

        /// <summary>
        /// Promedio final calculado automáticamente. No requiere asignación manual.
        /// </summary>
        public double PromedioFinal =>
            Math.Round((Nota1 + Nota2 + Nota3) / 3.0, 2);

        /// <summary>
        /// Estado académico basado en el promedio final.
        /// Aprobado: promedio >= 3.0 | Reprobado: promedio < 3.0
        /// </summary>
        public string EstadoAcademico =>
            PromedioFinal >= 3.0 ? "Aprobado" : "Reprobado";

        /// <summary>
        /// Valida que la nota esté en el rango permitido (0.0 - 5.0).
        /// Lanza una excepción si el valor está fuera de rango.
        /// </summary>
        private static double ValidarNota(double valor)
        {
            if (valor < 0.0 || valor > 5.0)
                throw new ArgumentOutOfRangeException(nameof(valor),
                    "La nota debe estar entre 0.0 y 5.0.");
            return valor;
        }

        public override string ToString() =>
            $"Notas: {Nota1} | {Nota2} | {Nota3} → Promedio: {PromedioFinal} ({EstadoAcademico})";
    }
}
