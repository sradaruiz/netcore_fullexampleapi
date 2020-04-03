using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UniriojaREST.Entities;
using UniriojaREST.Helpers;
using UniriojaREST.Services;
using UniriojaREST.Models.Responses;
using UniriojaREST.Models.Requests;
using UniriojaREST.Helpers.Filters;

namespace UniriojaREST.Controllers
{
    [Route("api/clientes")]
    public class ClientesController : Controller
    {
        private readonly IClientesRepository _clientesRepository;
        
        private IUrlHelper _urlHelper;

        public ClientesController(IClientesRepository clientesRepository, 
                                  IUrlHelper urlHelper)
        {
            _clientesRepository = clientesRepository;
            _urlHelper = urlHelper;
        }
        
        [HttpGet("{id}", Name ="GetCliente")]
        public IActionResult GetCliente(Guid id)
        {
            var clienteFromRepo = _clientesRepository.GetCliente(id);

            if (clienteFromRepo == null)
            {
                return NotFound();
            }
            
            var cliente = Mapper.Map<ClienteResponse>(clienteFromRepo);

            var links = CreateLinksForCliente(id);

            var response = new Halcyon.HAL.HALResponse(cliente)
                            .AddLinks(links);

            return Ok(response);
        }
        
        [HttpGet(Name = "GetClientes")]
        public IActionResult GetClientes(ClientesFilter clientesFilter)
        {
            var clientesFromRepo = _clientesRepository.GetClientes(clientesFilter);
                              
            var clientes = Mapper.Map<IEnumerable<ClienteResponse>>(clientesFromRepo);

            var paginationMetadata = new
            {
                totalCount = clientesFromRepo.TotalCount,
                pageSize = clientesFromRepo.PageSize,
                currentPage = clientesFromRepo.CurrentPage,
                totalPages = clientesFromRepo.TotalPages,
            };
            
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(clientes);
        }

        [HttpGet("destacados", Name="GetClientesVIP")]
        public IActionResult GetClientesVIP()
        {
            //var clientesFromRepo = _clientesRepository.GetClientes(new ClientesFilter()
            //{
            //    Vip = true
            //});

            //var clientes = Mapper.Map<IEnumerable<ClienteResponse>>(clientesFromRepo);

            //return Ok(clientes);
            return Ok(null);
        }

        /// <summary>
        /// Crea un cliente
        /// </summary>
        /// <remarks>
        /// Petición de ejemplo:
        ///
        ///     POST /clientes
        ///     {
        ///        "nombre": "Ned",
        ///        "apellido": "Flanders",
        ///        "telefono": "555555555",
        ///        "esVip": true
        ///     }
        ///
        /// </remarks>
        /// <param name="cliente"></param>
        /// <returns>Un nuevo cliente</returns>
        /// <response code="201">Devuelve el nuevo cliente</response>
        /// <response code="400">Si la peticion es nula</response>  
        [ProducesResponseType(typeof(ClienteResponse), 201)]
        [ProducesResponseType(typeof(ClienteResponse), 400)]
        [Produces("application/json")]
        [HttpPost(Name = "CreateCliente")]
        public IActionResult CreateCliente([FromBody] AddClienteRequest cliente)
        {
            if (cliente == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid){
                return new UnprocessableEntityObjectResult(ModelState);
            }

            _clientesRepository.ClienteExists(Guid.NewGuid());

             var clienteEntity = Mapper.Map<Cliente>(cliente);
            _clientesRepository.AddCliente(clienteEntity);

            if (!_clientesRepository.Save())
            {
                throw new Exception("Fallo al guardar cliente");
            }

            var clienteToReturn = Mapper.Map<ClienteResponse>(clienteEntity);
            
            return CreatedAtRoute("GetCliente",
                new { id = clienteToReturn.Id },
                clienteToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockClienteCreation(Guid id)
        {
            if (_clientesRepository.ClienteExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            
            return NotFound();
        }

        [HttpPut("{id}", Name = "UpdateCliente")]
        public IActionResult UpdateCliente(Guid id,
            [FromBody] UpdateClienteRequest cliente)
        {
            if (cliente == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!_clientesRepository.ClienteExists(id))
            {
                return NotFound();
            }

            var clienteFromRepo = _clientesRepository.GetCliente(id);

            Mapper.Map(cliente, clienteFromRepo);

            _clientesRepository.UpdateCliente(clienteFromRepo);

            if (!_clientesRepository.Save())
            {
                throw new Exception($"Updating book {id} failed on save.");
            }

            return Ok(clienteFromRepo);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateCliente")]
        public IActionResult PartiallyUpdateCliente(Guid id,
            [FromBody] JsonPatchDocument<UpdateClienteRequest> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_clientesRepository.ClienteExists(id))
            {
                return NotFound();
            }

            var clienteFromRepo = _clientesRepository.GetCliente(id);

            var clienteToPatch = Mapper.Map<UpdateClienteRequest>(clienteFromRepo);

            patchDoc.ApplyTo(clienteToPatch, ModelState);

            TryValidateModel(clienteToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
           
            Mapper.Map(clienteToPatch, clienteFromRepo);

            _clientesRepository.UpdateCliente(clienteFromRepo);

            if (!_clientesRepository.Save())
            {
                throw new Exception($"Patching cliente {id} fallo.");
            }

            return Ok(clienteFromRepo);
        }

        [HttpDelete("{id}", Name = "DeleteCliente")]
        public IActionResult DeleteCliente(Guid id)
        {
            var clienteFromRepo = _clientesRepository.GetCliente(id);
            if (clienteFromRepo == null)
            {
                return NotFound();
            }

            _clientesRepository.DeleteCliente(clienteFromRepo);

            if (!_clientesRepository.Save())
            {
                throw new Exception($"Borrando cliente con id {id} fallo.");
            }

            return NoContent();
        }

        private IEnumerable<Halcyon.HAL.Link> CreateLinksForCliente(Guid id)
        {
            var links = new List<Halcyon.HAL.Link>();

            links.Add(
                new Halcyon.HAL.Link("self",
                    _urlHelper.Link("GetCliente", new { id = id }),
                    null,
                    "GET"));
            
            links.Add(
              new Halcyon.HAL.Link("delete_cliente",
                 _urlHelper.Link("DeleteCliente", new { id = id }),
                  null,
                 "DELETE"));   

            links.Add(
              new Halcyon.HAL.Link( "update_cliente",
              _urlHelper.Link("PartiallyUpdateCliente", new { id = id }),
              null,
              "PATCH"));

            links.Add(
               new Halcyon.HAL.Link("presupuestos",
               _urlHelper.Link("GetPresupuestos", new { clienteId = id }),
               null,
               "GET"));

            return links;
        }
    }
}