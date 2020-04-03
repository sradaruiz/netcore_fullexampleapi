using System;

namespace UniriojaREST.Models.Responses
{
    public class ClienteResponse
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }
        
        public string Apellido { get; set; }

        public string Telefono {get; set;}
        
        public bool EsVip {get; set;}
    }
}