using System;
using System.Collections.Generic;
using UniriojaREST.Entities;
using System.Linq;
using UniriojaREST.Helpers;
using UniriojaREST.Helpers.Filters;

namespace UniriojaREST.Services
{
    public class ClientesRepository : IClientesRepository
    {
        private ConcesionariosContext _context;

        public ClientesRepository(ConcesionariosContext context)
        {
            _context = context;
        }

        public void AddCliente(Cliente cliente)
        {
            cliente.Id = Guid.NewGuid();
            _context.Clientes.Add(cliente);

            if (cliente.Presupuestos.Any())
            {
                foreach (var presupuesto in cliente.Presupuestos)
                {
                    presupuesto.Id = Guid.NewGuid();
                }
            }
        }

        public bool ClienteExists(Guid clienteId)
        {
            return _context.Clientes.Any(a => a.Id == clienteId);
        }

        public void DeleteCliente(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
        }

        public Cliente GetCliente(Guid clienteId)
        {
             return _context.Clientes.FirstOrDefault(a => a.Id == clienteId);
        }

        public PagedList<Cliente> GetClientes(ClientesFilter clientesFilter)
        {
            var collectionBeforePaging = _context.Clientes.OrderBy(x => x.Id).AsQueryable();

            if (clientesFilter.Vip.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.EsVip == clientesFilter.Vip.Value);
            }

            if (!string.IsNullOrEmpty(clientesFilter.SearchQuery))
            {
                var searchQueryForWhereClause = clientesFilter.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Nombre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Apellido.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Cliente>.Create(
                collectionBeforePaging,
                clientesFilter.PageNumber,
                clientesFilter.PageSize);            
        }

        public IEnumerable<Presupuesto> GetPresupuestosForCliente(Guid clienteId)
        {
            return _context.Presupuestos
                        .Where(b => b.ClienteId == clienteId)
                        .OrderBy(b => b.Id).ToList();
        }
        
        public void UpdateCliente(Cliente cliente)
        {
            // no code neccesary    
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}