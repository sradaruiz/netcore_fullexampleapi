using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniriojaREST.Entities
{
    public class Cliente
    {  
        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }
        
        public string Apellido { get; set; }

        public string Telefono {get; set;}
        
        public bool EsVip {get; set;}

        public ICollection<Presupuesto> Presupuestos { get; set; }
            = new List<Presupuesto>();
       
    }
}