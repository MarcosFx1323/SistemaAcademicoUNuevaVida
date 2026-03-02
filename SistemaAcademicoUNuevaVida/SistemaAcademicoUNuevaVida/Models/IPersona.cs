namespace SistemaAcademicoUNuevaVida.Models
{
    /// <summary>
    /// Contrato base para todas las personas del sistema académico.
    /// </summary>
    public interface IPersona
    {
        string Id { get; set; }
        string Nombre { get; set; }
        string Email { get; set; }

        /// <summary>
        /// Retorna información resumida de la persona.
        /// </summary>
        string ObtenerInfo();
    }
}
