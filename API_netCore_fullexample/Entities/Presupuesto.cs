using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniriojaREST.Entities
{

    public enum PresupuestoEstado { Abierto = 0, Aceptado = 1, Cerrado = 2}

    public class Presupuesto
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public int ImporteNegociado {get; set;}

        [MaxLength(500)]
        public string Observaciones { get; set; }

        public PresupuestoEstado Estado { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        public Guid ClienteId { get; set; }
    }
}