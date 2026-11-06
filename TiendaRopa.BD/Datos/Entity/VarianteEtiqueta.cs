using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopa.BD.Datos.Entity
{
    public class VarianteEtiqueta : EntityBase
    {
        //Foraneas
        public int VarianteId { get; set; }
        public Variante? Variante { get; set; }
        public int EtiquetaId { get; set; }
        public Etiqueta? Etiqueta { get; set; }
        
    }
}
