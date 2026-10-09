using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class Envio : EntityBase
    {
        [Required(ErrorMessage = "La dirección de envío es obligatoria.")]
        [MaxLength(200, ErrorMessage = "La dirección de envío no puede exceder los 200 caracteres.")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El costo del envío es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoEnvio { get; set; }

        [Required(ErrorMessage = "El estado del envío es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El estado del envío no puede exceder los 100 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        
        public DateTime HoraInicio { get; set; }

        
        public DateTime HoraLlegada { get; set; }

        public int VentaId { get; set; }
        public Venta? Venta { get; set; }
    }
}
