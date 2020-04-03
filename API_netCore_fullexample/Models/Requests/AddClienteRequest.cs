using System;
using FluentValidation;

namespace UniriojaREST.Models.Requests
{
    public class AddClienteRequest
    {
        public string Nombre { get; set; }
        
        public string Apellido { get; set; }

        public string Telefono {get; set;}
        
        public bool EsVip {get; set;}
    }

    public class AddClienteRequestValidation : AbstractValidator<AddClienteRequest>
    {
        public AddClienteRequestValidation()
        {
            RuleFor(x => x.Nombre)
                   .NotEmpty()
                   .MaximumLength(100);

            RuleFor(x => x.Apellido)
                   .NotEmpty();

            RuleFor(x => x.Telefono)
                    .NotEmpty()
                    .Must(BeAValidPhoneNumber);
        }
        
        private bool BeAValidPhoneNumber(string phone = "")
        {
            return !string.IsNullOrEmpty(phone) && phone.Length >= 9;
        }
       
    }
}