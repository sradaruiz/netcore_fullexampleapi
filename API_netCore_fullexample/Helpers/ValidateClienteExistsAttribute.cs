using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniriojaREST.Services;

namespace UniriojaREST.Helpers
{
    public class ValidateClienteExistsAttribute : TypeFilterAttribute
    {
        public ValidateClienteExistsAttribute():base(typeof(ValidateClienteExistFilterImpl))
        {
        }

        private class ValidateClienteExistFilterImpl : IActionFilter
        {
            private readonly IClientesRepository _clientesRepository;

            public ValidateClienteExistFilterImpl(IClientesRepository clientesRepository)
            {
                _clientesRepository = clientesRepository;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = (System.Guid) context.ActionArguments["id"];

                    if ( !_clientesRepository.ClienteExists(id))
                    {
                        context.Result = new NotFoundObjectResult(id);
                        return;
                    }
                }
            }
            
        }
    }
}