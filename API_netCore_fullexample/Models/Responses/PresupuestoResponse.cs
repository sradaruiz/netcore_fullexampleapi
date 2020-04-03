using System;

namespace UniriojaREST.Models.Responses
{
    public class PresupuestoResponse
    {
        public Guid Id { get; set; }

        public int ImporteNegociado {get; set;}

        public string Observaciones { get; set; }
        
        public string Estado { get; set; }
    }
}