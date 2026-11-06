using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class EtiquetaCrearDTO
    {
        [Required(ErrorMessage = "El nombre de la etiqueta es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la etiqueta no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
