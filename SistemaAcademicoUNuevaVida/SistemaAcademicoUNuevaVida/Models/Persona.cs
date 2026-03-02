namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Clase base abstracta que implementa IPersona.
    /// Estudiante y Docente heredan de esta clase.
    /// </summary>
    public abstract class Persona : IPersona
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Edad calculada automáticamente a partir de FechaNacimiento.
        /// </summary>
        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                int edad = hoy.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
                return edad;
            }
        }

        /// <summary>
        /// Cada subclase debe implementar su propio resumen de información.
        /// </summary>
        public abstract string ObtenerInfo();

        public override string ToString() => ObtenerInfo();
    }
}
