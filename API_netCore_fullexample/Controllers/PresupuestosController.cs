using System;
using Microsoft.AspNetCore.Mvc;
using UniriojaREST.Services;
using UniriojaREST.Models.Responses;
using UniriojaREST.Models.Requests;
using System.Collections.Generic;
using AutoMapper;

namespace UniriojaREST.Controllers
{
    [Route("api/clientes/{clienteId}/presupuestos")]
    public class PresupuestosController : Controller
    {
        private readonly IClientesRepository _clientesRepository;

        public PresupuestosController(IClientesRepository clientesRepository)
        {
            _clientesRepository = clientesRepository;
        }

        [HttpGet(Name="GetPresupuestos")]
        public IActionResult GetPresupuestosForCliente(Guid clienteId)
        {
            if (!_clientesRepository.ClienteExists(clienteId))
            {
                return NotFound();
            }

            var presupuestoFromRepo = _clientesRepository.GetPresupuestosForCliente(clienteId);
            
            var presupuestos = Mapper.Map<IEnumerable<PresupuestoResponse>>(presupuestoFromRepo);
            
            return Ok(presupuestos);
        }
        
    }
}