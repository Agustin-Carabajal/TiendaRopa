using System.ComponentModel.DataAnnotations;

namespace TiendaRopa.Shared.DTO.Producto_y_mas
{
    public class MarcaCrearDTO
    {
        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la marca no puede exceder los 100 caracteres.")]
        public string NombreMarca { get; set; } = string.Empty;
    }
}
