using Microsoft.AspNetCore.Mvc;
using Halcyon.HAL.Attributes;
using Halcyon.HAL;

namespace UniriojaREST.Controllers
{
    [Route("api")]
    public class RootController : Controller
    {
        private IUrlHelper _urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var response = new HALResponse(null)
                .AddLinks(new Link("clientes", _urlHelper.Link("GetClientes", new { }), null, "GET"))
                .AddLinks(new Link("clientes_destacados", _urlHelper.Link("GetClientesVIP", new { }), null, "GET"))
                .AddLinks(new Link("create_cliente", _urlHelper.Link("CreateCliente", new { }), null, "POST"));
                
            return this.Ok(response);
        }
    }
}